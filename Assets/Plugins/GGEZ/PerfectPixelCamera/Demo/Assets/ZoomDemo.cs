using UnityEngine;

namespace GGEZ
{
public enum ZoomDemoMode
{
    Bubbling,
    Shimmering,
    Marching,
}

public class ZoomDemo : MonoBehaviour
{
public Camera MainCamera;
public AnimationCurve TimeToOrthographicSize;
public UnityEngine.UI.Text TitleText;
public UnityEngine.UI.Text BodyText;
public UnityEngine.UI.Text SwitchModesButtonText;
private ZoomDemoMode mode = ZoomDemoMode.Marching;

public void SwitchModes ()
    {
    switch (this.mode)
        {
        case ZoomDemoMode.Shimmering:
            {
            this.mode = ZoomDemoMode.Bubbling;
            this.SwitchModesButtonText.text = "Switch to Marching";
            }
            break;
        case ZoomDemoMode.Bubbling:
            {
            this.mode = ZoomDemoMode.Marching;
            this.SwitchModesButtonText.text = "Switch to Shimmering";
            }
            break;
        case ZoomDemoMode.Marching:
            {
            this.mode = ZoomDemoMode.Shimmering;
            this.SwitchModesButtonText.text = "Switch to Bubbling";
            }
            break;
        }
    this.TitleText.text = this.mode.ToString();
    }

void Start ()
    {
    this.SwitchModes ();
    }

void Update ()
    {
    PerfectPixelCamera perfectPixelCamera = this.MainCamera.GetComponent (typeof(PerfectPixelCamera)) as PerfectPixelCamera;
    bool mainCameraIsFixed = perfectPixelCamera != null && perfectPixelCamera.isActiveAndEnabled;
    float orthographicSize = this.MainCamera.orthographicSize;
    float orthographicSizeSnapped = orthographicSize;
    if (mainCameraIsFixed)
        {
        float zoomFactor = Mathf.Max (1f, Mathf.Ceil ((1f * this.MainCamera.pixelRect.height) / (this.MainCamera.orthographicSize * 2f * perfectPixelCamera.TexturePixelsPerWorldUnit)));
        orthographicSizeSnapped = (1f * this.MainCamera.pixelRect.height) / (zoomFactor * 2f * perfectPixelCamera.TexturePixelsPerWorldUnit);
        }
    switch (this.mode)
        {
        case ZoomDemoMode.Shimmering:
            {
            this.MainCamera.orthographicSize = this.TimeToOrthographicSize.Evaluate (Time.time);
            this.MainCamera.transform.position = Vector3.back * 10;
            if (mainCameraIsFixed)
                {
                this.BodyText.text = "The Perfect Pixel Camera snaps the orthographicSize of the camera component behind the scenes to fix zooming issues (" + orthographicSize.ToString ("0.0") + " snapped to " + orthographicSizeSnapped.ToString ("0.0") + ")";
                }
            else
                {
                this.BodyText.text = "A miscalibrated camera shows a shimmer or wave pattern as it zooms. Add the PerfectPixelCamera component to the MainCamera object to check out the fix, or tap the button to see other errors.";
                }
            }
            break;
        case ZoomDemoMode.Bubbling:
            {
            this.MainCamera.transform.position = new Vector3 (
                    Mathf.Cos (Time.time * 0.5f) * 5f,
                    Mathf.Sin (Time.time * 0.5f) * 5f,
                    -10f
                    );
            if (mainCameraIsFixed)
                {
                this.BodyText.text = "With the PerfectPixelCamera enabled, panning doesn't jiggle anymore!";
                }
            else
                {
                this.BodyText.text = "When panning, the image appears to jiggle and bubble. Add the PerfectPixelCamera component to the MainCamera object to check out the fix, or tap the button to see other errors.";
                }
            }
            break;
        case ZoomDemoMode.Marching:
            {
            this.MainCamera.orthographicSize = (this.MainCamera.pixelHeight / (2 * 16f)) * 0.5f;
            this.MainCamera.transform.position = new Vector3 (
                    (Mathf.PingPong (Time.time * 0.1f, 2f)-1f) * 0.1f,
                    (Mathf.PingPong (Time.time * 0.1f, 2f)-1f) * 3f,
                    -10f
                    );
            if (mainCameraIsFixed)
                {
                this.BodyText.text = "With the PerfectPixelCamera enabled, the camera's projection matrix is automatically offset by less than a pixel and marching is fixed.";
                }
            else
                {
                this.BodyText.text = "Even with orthographicSize set correctly to " + this.MainCamera.orthographicSize.ToString("0.0") + ", an alignment issue is easily visible at the border at max zoom. Use the scale slider to zoom in. Add the PerfectPixelCamera component to the MainCamera object to check out the fix, or tap the button to see other errors.";
                }
            }
            break;
        }
    }
}
}
