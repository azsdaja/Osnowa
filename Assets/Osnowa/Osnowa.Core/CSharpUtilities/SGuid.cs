namespace Osnowa.Osnowa.Core.CSharpUtilities
{
	using System;

	[Serializable]
	public struct SGuid : IComparable, IComparable<SGuid>, IEquatable<SGuid>
	{
		public string Value;

		private SGuid(string value)
		{
			Value = value;
		}

		public static implicit operator SGuid(Guid guid)
		{
			return new SGuid(guid.ToString());
		}

		public static implicit operator Guid(SGuid serializableGuid)
		{
			return new Guid(serializableGuid.Value);
		}

		public int CompareTo(object value)
		{
			if (value == null)
				return 1;
			if (!(value is SGuid))
				throw new ArgumentException("Must be SerializableGuid");
			SGuid guid = (SGuid)value;
			return guid.Value == Value ? 0 : 1;
		}

		public int CompareTo(SGuid other)
		{
			return other.Value == Value ? 0 : 1;
		}

		public bool Equals(SGuid other)
		{
			return Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return (Value != null ? Value.GetHashCode() : 0);
		}

		public override string ToString()
		{
			return (Value != null ? new Guid(Value).ToString() : string.Empty);
		}
	}
}