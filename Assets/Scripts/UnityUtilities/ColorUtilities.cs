namespace UnityUtilities
{
	using UnityEngine;

	public class ColorUtilities
	{
		/// <summary>
		/// Calculates interpolation of three colors. 
		/// Note that color2 is present in both first and second half of the range, so it's dominating.
		/// </summary>
		public static Color Lerp3(Color color1, Color color2, Color color3, float value)
		{
			if (value < .5f)
			{
				float firstHalfValue = value * 2;
				return Color.Lerp(color1, color2, firstHalfValue);
			
			}
			else
			{
				float secondHalfValue = (value - .5f) * 2;
				return Color.Lerp(color2, color3, secondHalfValue);
			}
		}

		public static Color WithAlpha(Color color, float alpha)
		{
			return new Color(color.r, color.g, color.b, alpha);
		}
	}
}
