namespace UnityUtilities
{
	using System;

	public abstract class Nullable<TType> where TType : struct
	{
		public bool HasValue;
		public abstract TType? Value { get; }
	}

	[Serializable]
	public class NullableFloat : Nullable<float>
	{
		public float NonNullableValue;
		public override float? Value => HasValue ? NonNullableValue : default(float?);
	}

	[Serializable]
	public class NullableInt : Nullable<int>
	{
		public int NonNullableValue;
		public override int? Value => HasValue ? NonNullableValue : default(int?);
	}

	[Serializable]
	public class NullableBool : Nullable<bool>
	{
		public bool NonNullableValue;
		public override bool? Value => HasValue ? NonNullableValue : default(bool?);
	}
}