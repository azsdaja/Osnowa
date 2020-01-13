namespace UI
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityUtilities;

	public class GenericBar : MonoBehaviour
	{
		public Color ColorZero;
		public Color ColorCenter;
		public Color ColorFull;
		public Image BarValueImage;
		
		public virtual void OnChanged(float value)
		{
			BarValueImage.fillAmount = value;
			Color barColor = ColorUtilities.Lerp3(ColorZero, ColorCenter, ColorFull, value);
			BarValueImage.color = barColor;
		}
	}
}
