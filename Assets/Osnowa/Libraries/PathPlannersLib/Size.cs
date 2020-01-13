namespace Libraries.PathPlannersLib
{
	using System.ComponentModel;
	using System.Globalization;

	public struct Size
	{

		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.Empty"]/*' />
		/// <devdoc>
		///    Initializes a new instance of the <see cref='Size'/> class.
		/// </devdoc>
		public static readonly Size Empty = new Size();

		private int width;
		private int height;

		/**
         * Create a new Size object from a point
         */
		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.Size"]/*' />
		/// <devdoc>
		///    <para>
		///       Initializes a new instance of the <see cref='Size'/> class from
		///       the specified <see cref='Point'/>.
		///    </para>
		/// </devdoc>
		public Size(Point pt)
		{
			width = pt.X;
			height = pt.Y;
		}

		/**
         * Create a new Size object of the specified dimension
         */
		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.Size1"]/*' />
		/// <devdoc>
		///    Initializes a new instance of the <see cref='Size'/> class from
		///    the specified dimensions.
		/// </devdoc>
		public Size(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.operator+"]/*' />
		/// <devdoc>
		///    <para>
		///       Performs vector addition of two <see cref='Size'/> objects.
		///    </para>
		/// </devdoc>
		public static Size operator +(Size sz1, Size sz2)
		{
			return Add(sz1, sz2);
		}

		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.operator-"]/*' />
		/// <devdoc>
		///    <para>
		///       Contracts a <see cref='Size'/> by another <see cref='Size'/>
		///       .
		///    </para>
		/// </devdoc>
		public static Size operator -(Size sz1, Size sz2)
		{
			return Subtract(sz1, sz2);
		}

		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.operator=="]/*' />
		/// <devdoc>
		///    Tests whether two <see cref='Size'/> objects
		///    are identical.
		/// </devdoc>
		public static bool operator ==(Size sz1, Size sz2)
		{
			return sz1.Width == sz2.Width && sz1.Height == sz2.Height;
		}

		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.operator!="]/*' />
		/// <devdoc>
		///    <para>
		///       Tests whether two <see cref='Size'/> objects are different.
		///    </para>
		/// </devdoc>
		public static bool operator !=(Size sz1, Size sz2)
		{
			return !(sz1 == sz2);
		}

		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.operatorPoint"]/*' />
		/// <devdoc>
		///    Converts the specified <see cref='Size'/> to a
		/// <see cref='Point'/>.
		/// </devdoc>
		public static explicit operator Point(Size size)
		{
			return new Point(size.Width, size.Height);
		}

		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.IsEmpty"]/*' />
		/// <devdoc>
		///    Tests whether this <see cref='Size'/> has zero
		///    width and height.
		/// </devdoc>
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return width == 0 && height == 0;
			}
		}

		/**
         * Horizontal dimension
         */
		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.Width"]/*' />
		/// <devdoc>
		///    <para>
		///       Represents the horizontal component of this
		///    <see cref='Size'/>.
		///    </para>
		/// </devdoc>
		public int Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
			}
		}

		/**
         * Vertical dimension
         */
		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.Height"]/*' />
		/// <devdoc>
		///    Represents the vertical component of this
		/// <see cref='Size'/>.
		/// </devdoc>
		public int Height
		{
			get
			{
				return height;
			}
			set
			{
				height = value;
			}
		}

		/// <devdoc>
		///    <para>
		///       Performs vector addition of two <see cref='Size'/> objects.
		///    </para>
		/// </devdoc>
		public static Size Add(Size sz1, Size sz2)
		{
			return new Size(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
		}

		/// <devdoc>
		///    <para>
		///       Contracts a <see cref='Size'/> by another <see cref='Size'/> .
		///    </para>
		/// </devdoc>
		public static Size Subtract(Size sz1, Size sz2)
		{
			return new Size(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
		}

		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.Equals"]/*' />
		/// <devdoc>
		///    <para>
		///       Tests to see whether the specified object is a
		///    <see cref='Size'/> 
		///    with the same dimensions as this <see cref='Size'/>.
		/// </para>
		/// </devdoc>
		public override bool Equals(object obj)
		{
			if (!(obj is Size))
				return false;

			Size comp = (Size)obj;
			// Note value types can't have derived classes, so we don't need to 
			// check the types of the objects here.  -- Microsoft, 2/21/2001
			return (comp.width == this.width) &&
				   (comp.height == this.height);
		}

		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.GetHashCode"]/*' />
		/// <devdoc>
		///    <para>
		///       Returns a hash code.
		///    </para>
		/// </devdoc>
		public override int GetHashCode()
		{
			return width ^ height;
		}

		/// <include file='doc\Size.uex' path='docs/doc[@for="Size.ToString"]/*' />
		/// <devdoc>
		///    <para>
		///       Creates a human-readable string that represents this
		///    <see cref='Size'/>.
		///    </para>
		/// </devdoc>
		public override string ToString()
		{
			return "{Width=" + width.ToString(CultureInfo.CurrentCulture) + ", Height=" + height.ToString(CultureInfo.CurrentCulture) + "}";
		}
	}
}