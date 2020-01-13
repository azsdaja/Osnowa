namespace Libraries.PathPlannersLib
{
	using System.ComponentModel;
	using System.Globalization;

	public struct Point
	{

		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.Empty"]/*' />
		/// <devdoc>
		///    Creates a new instance of the <see cref='Point'/> class
		///    with member data left uninitialized.
		/// </devdoc>
		public static readonly Point Empty = new Point();

		private int x;
		private int y;

		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.Point"]/*' />
		/// <devdoc>
		///    Initializes a new instance of the <see cref='Point'/> class
		///    with the specified coordinates.
		/// </devdoc>
		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.Point2"]/*' />
		/// <devdoc>
		///    Initializes a new instance of the Point class using
		///    coordinates specified by an integer value.
		/// </devdoc>
		public Point(int dw)
		{
			unchecked
			{
				this.x = (short)LOWORD(dw);
				this.y = (short)HIWORD(dw);
			}
		}

		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.IsEmpty"]/*' />
		/// <devdoc>
		///    <para>
		///       Gets a value indicating whether this <see cref='Point'/> is empty.
		///    </para>
		/// </devdoc>
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return x == 0 && y == 0;
			}
		}

		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.X"]/*' />
		/// <devdoc>
		///    Gets the x-coordinate of this <see cref='Point'/>.
		/// </devdoc>
		public int X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.Y"]/*' />
		/// <devdoc>
		///    <para>
		///       Gets the y-coordinate of this <see cref='Point'/>.
		///    </para>
		/// </devdoc>
		public int Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}
		
		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.operator=="]/*' />
		/// <devdoc>
		///    <para>
		///       Compares two <see cref='Point'/> objects. The result specifies
		///       whether the values of the <see cref='Point.X'/> and <see cref='Point.Y'/> properties of the two <see cref='Point'/>
		///       objects are equal.
		///    </para>
		/// </devdoc>
		public static bool operator ==(Point left, Point right)
		{
			return left.X == right.X && left.Y == right.Y;
		}

		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.operator!="]/*' />
		/// <devdoc>
		///    <para>
		///       Compares two <see cref='Point'/> objects. The result specifies whether the values
		///       of the <see cref='Point.X'/> or <see cref='Point.Y'/> properties of the two
		///    <see cref='Point'/> 
		///    objects are unequal.
		/// </para>
		/// </devdoc>
		public static bool operator !=(Point left, Point right)
		{
			return !(left == right);
		}
		
		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.Equals"]/*' />
		/// <devdoc>
		///    <para>
		///       Specifies whether this <see cref='Point'/> contains
		///       the same coordinates as the specified <see cref='System.Object'/>.
		///    </para>
		/// </devdoc>
		public override bool Equals(object obj)
		{
			if (!(obj is Point)) return false;
			Point comp = (Point)obj;
			// Note value types can't have derived classes, so we don't need 
			// to check the types of the objects here.  -- Microsoft, 2/21/2001
			return comp.X == this.X && comp.Y == this.Y;
		}

		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.GetHashCode"]/*' />
		/// <devdoc>
		///    <para>
		///       Returns a hash code.
		///    </para>
		/// </devdoc>
		public override int GetHashCode()
		{
			return unchecked(x ^ y);
		}

		/**
         * Offset the current Point object by the given amount
         */
		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.Offset"]/*' />
		/// <devdoc>
		///    Translates this <see cref='Point'/> by the specified amount.
		/// </devdoc>
		public void Offset(int dx, int dy)
		{
			X += dx;
			Y += dy;
		}

		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.Offset2"]/*' />
		/// <devdoc>
		///    Translates this <see cref='Point'/> by the specified amount.
		/// </devdoc>
		public void Offset(Point p)
		{
			Offset(p.X, p.Y);
		}

		/// <include file='doc\Point.uex' path='docs/doc[@for="Point.ToString"]/*' />
		/// <devdoc>
		///    <para>
		///       Converts this <see cref='Point'/>
		///       to a human readable
		///       string.
		///    </para>
		/// </devdoc>
		public override string ToString()
		{
			return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) + "}";
		}

		private static int HIWORD(int n)
		{
			return (n >> 16) & 0xffff;
		}

		private static int LOWORD(int n)
		{
			return n & 0xffff;
		}
	}
}