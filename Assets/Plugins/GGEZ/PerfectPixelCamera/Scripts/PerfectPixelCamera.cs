using UnityEngine;

namespace GGEZ
{

	[
		ExecuteInEditMode, // Run this script in edit mode so the preview window looks good
		RequireComponent(typeof(Camera)), // Only add this component if there is a camera
		HelpURL("http://ggez.org/posts/perfect-pixel-camera/"), // Website opened by clicking the book icon on the component
		DisallowMultipleComponent, // Only one of these per GameObject
		AddComponentMenu("GGEZ/Camera/Perfect Pixel Camera") // Insert into the "Add Component..." menu
	]
	public class PerfectPixelCamera : MonoBehaviour
	{

// Set this value to the same value as Pixels Per Unit when importing sprites
		[
			Tooltip(
				"The number of texture pixels that fit in 1.0 world units. Common values are 8, 16, 32 and 64. If you're making a tile-based game, this is your tile size."
			),
			Range(1, 64)
		] public int TexturePixelsPerWorldUnit = 16;

// Reference to the camera on this same GameObject. Found
// by the OnEnable function.
		private Camera cameraComponent;

// Set to a value that compensates for the half-pixel offset when rendering
// with Direct3D. This is automatically handled by Unity 5.5 and later.
// If that's the case, it is declared as a constant 0 which the compiler
// can use to optimize calculations in LateUpdate.
// See: https://docs.unity3d.com/Manual/UpgradeGuide55.html
#if UNITY_5_5_OR_NEWER
		private const float halfPixelOffsetIfNeededForD3D = 0f;
#else
private float halfPixelOffsetIfNeededForD3D;
#endif

// Objects that you want to be perfectly aligned should have X and Y
// coordinates that are integer multiples of this value. It is always
// safe to align to 1.0 / TexturePixelsPerWorldUnit, but this value can
// be smaller if the camera is zoomed and will make movement more smooth.
		public float SnapSizeWorldUnits { get; private set; }

//---------------------------------------------------------------------------
// OnEnable - Called by Unity when the component is created or enabled
//---------------------------------------------------------------------------
		void OnEnable()
		{

			// Grab a reference to the camera
			this.cameraComponent = (Camera) this.GetComponent(typeof(Camera));

#if !UNITY_5_5_OR_NEWER

// Detect whether we are using Direct3D, because D3D rendering has a
// half-pixel offset from OpenGL rendering.
    bool isD3D =
            SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D9
            || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11
            || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D12;

    // 0.4975f and not 0.5f is used because 0.5f is able to be represented
    // as a perfect IEEE float. This means that when added to other
    // floats that are imperfect, the results can sometimes be rounded
    // the wrong way. It can be tricky to reproduce so this isn't part
    // of the main demo.
    this.halfPixelOffsetIfNeededForD3D = isD3D ? 0.4975f : 0f;

#endif

			// Run the LateUpdate immediately so that the projection gets set up
			this.LateUpdate();

		}

//---------------------------------------------------------------------------
// OnDisable - Called by Unity when the component is disabled or destroyed
// This function cleans up after the PerfectPixelCamera so that the
// projection matrix isn't left in an altered state by this component.
//---------------------------------------------------------------------------
		void OnDisable()
		{

			if (this.cameraComponent == null)
			{
				return;
			}
			this.cameraComponent.ResetProjectionMatrix();
			this.cameraComponent = null;

		}

//---------------------------------------------------------------------------
// LateUpdate - Called by Unity after all other functions have run Update.
// If you have other scripts that use LateUpdate, you might want to use
// the Script Execution Order project setting to make this script run last.
//---------------------------------------------------------------------------
		void LateUpdate()
		{

			// Get a local reference
			Camera camera = this.cameraComponent;

			// Make sure the camera is in 2D mode
			camera.transparencySortMode = TransparencySortMode.Orthographic;
			camera.orthographic = true;
			camera.transform.rotation = Quaternion.identity;
			camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.00001f);

			// This is the code that computes the parameters needed to perfectly map
			// world-space pixels to screen-space pixels.
			var pixelRect = camera.pixelRect;
			float texturePixelsPerWorldUnit = this.TexturePixelsPerWorldUnit;
			float cameraOrthographicSize = camera.orthographicSize;

			float zoomFactor = CalculateZoomFactor(pixelRect, cameraOrthographicSize, texturePixelsPerWorldUnit);
			float halfWidth = (1f*pixelRect.width)/(zoomFactor*2f*texturePixelsPerWorldUnit);
			float halfHeight = (1f*pixelRect.height)/(zoomFactor*2f*texturePixelsPerWorldUnit);
			float snapSizeWorldUnits = 1f/(zoomFactor*texturePixelsPerWorldUnit);
			float halfPixelOffsetInWorldUnits = halfPixelOffsetIfNeededForD3D*snapSizeWorldUnits;
			float pixelPerfectXOffset = halfPixelOffsetInWorldUnits -
			                            Mathf.Repeat(
				                            snapSizeWorldUnits + Mathf.Repeat(camera.transform.position.x, snapSizeWorldUnits),
				                            snapSizeWorldUnits);
			float pixelPerfectYOffset = halfPixelOffsetInWorldUnits -
			                            Mathf.Repeat(
				                            snapSizeWorldUnits + Mathf.Repeat(camera.transform.position.y, snapSizeWorldUnits),
				                            snapSizeWorldUnits);

			// Save the snap size so other scripts can use it
			this.SnapSizeWorldUnits = snapSizeWorldUnits;

			// Build a manual projection matrix that fixes the camera!
			camera.projectionMatrix = Matrix4x4.Ortho(
				-halfWidth + pixelPerfectXOffset,
				halfWidth + pixelPerfectXOffset,
				-halfHeight + pixelPerfectYOffset,
				halfHeight + pixelPerfectYOffset,
				camera.nearClipPlane,
				camera.farClipPlane
			);

		}

		private static float CalculateZoomFactor(Rect pixelRect, float cameraOrthographicSize, float texturePixelsPerWorldUnit)
		{
			return Mathf.Max(1f,
				Mathf.Ceil((1f*pixelRect.height)/(cameraOrthographicSize * 2f*texturePixelsPerWorldUnit)));
		}

		public float CalculateZoomFactor(float cameraOrthographicSize)
		{
			Camera camera = this.cameraComponent;
			var pixelRect = camera.pixelRect;
			float texturePixelsPerWorldUnit = this.TexturePixelsPerWorldUnit;

			return Mathf.Max(1f,
				Mathf.Ceil((1f * pixelRect.height) / (cameraOrthographicSize * 2f * texturePixelsPerWorldUnit)));
		}
	}
}
