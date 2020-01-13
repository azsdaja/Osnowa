namespace UI
{
	public class GenericBarWithMirror : GenericBar
	{
		public GenericBar[] Mirrors;

		public override void OnChanged(float value)
		{
			base.OnChanged(value);
			foreach (GenericBar mirror in Mirrors)
			{
				mirror.OnChanged(value);
			}
		}
	}
}
