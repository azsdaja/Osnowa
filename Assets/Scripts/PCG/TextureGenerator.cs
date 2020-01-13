namespace PCG
{
	using Osnowa.Osnowa.Core;
	using UnityEngine;

	public static class TextureGenerator
	{
		public static void ApplyValueMapToTexture(ValueMap valueMap, Gradient colorGradient, Texture2D texture)
		{
			int width = valueMap.XSize;
			int height = valueMap.YSize;

			Color[] colourMap = new Color[width * height];
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					float mapValue = valueMap.Get(new Position(x, y));
					Color color = colorGradient.Evaluate(mapValue);
					colourMap[y * width + x] = color;
				}
			}

			ApplyColourMapToTexture(colourMap, width, height, texture);
		}

		public static void ApplyColourMapToTexture(Color[] colourMap, int width, int height, Texture2D texture)
		{
			texture.SetPixels(0, 0, width, height, colourMap);
			texture.Apply();
		}

		public static Sprite CreateSprite(ValueMap values)
		{
			Texture2D texture = CreateTexture(values);
			Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect);
			return sprite;
		}

		public static Texture2D CreateTexture(ValueMap valueMap)
		{
			Texture2D texture = new Texture2D(valueMap.XSize, valueMap.YSize, TextureFormat.RGBA32, false)
			{
				filterMode = FilterMode.Point, // doesn't seem to help with blended map texure in build
				wrapMode = TextureWrapMode.Clamp,
				name = $"{valueMap.XSize}x{valueMap.XSize} with middle value {valueMap.Get(valueMap.XSize/2, valueMap.YSize/2)}"
			};
			return texture;
		}
	}
}
