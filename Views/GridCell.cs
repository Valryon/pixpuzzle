using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace PixPuzzle
{
	public struct CellColor
	{
		public float A;
		public float R;
		public float G;
		public float B;

		public CellColor (float a, float r, float g, float b)
		{
			A = a;
			R = r;
			G = g;
			B = b;
		}

		public override bool Equals (object obj)
		{
			if (obj is CellColor) {
				CellColor otherColor = (CellColor)obj;

				return A == otherColor.A && R == otherColor.R && G == otherColor.G && B == otherColor.B;
			}
			return false;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}

		public UIColor UIColor {
			get {
				return new UIColor (R, G, B, A);
			}
		}
	}

	public class GridCell : UIView
	{
		// Values if cell is a start or end
		private int pathLength;
		// Values if cell is path part
		private CellColor pathColor;
		private PointF pathDirection;
		// Common to all cells
		private CellColor color;
		private UILabel label;

		public GridCell (int x, int y, RectangleF frame)
			: base(frame)
		{
			X = x;
			Y = y;

			// Create label
			label = new UILabel (new RectangleF (0, 0, frame.Width, frame.Height));
			label.Hidden = false;
			label.BackgroundColor = UIColor.FromRGB (230, 230, 230);
			label.TextColor = UIColor.Black;
			label.Text = "";
			label.TextAlignment = UITextAlignment.Center;

			// The border
			this.Layer.BorderColor = UIColor.Black.CGColor;
			this.Layer.BorderWidth = 1;

			AddSubview (label);

			// Default values
			pathLength = -1;
		}
		/// <summary>
		/// Sets the color for this cell from the pixel data of the image
		/// </summary>
		/// <param name="color">Color.</param>
		public void DefineBaseColor (CellColor color)
		{
			Color = color;

			UIColor uiColor = Color.UIColor;
			label.TextColor = uiColor;
		}
		/// <summary>
		/// Sets the number to display. It also means that the cell is a path start or end.
		/// </summary>
		/// <param name="val">Value.</param>
		public void DefineCellAsPathStartOrEnd (int pathLength)
		{
			label.Text = pathLength.ToString ();
		}
		/// <summary>
		/// Mark the cell as being in a complete path
		/// </summary>
		public void MarkComplete ()
		{
			label.BackgroundColor = color.UIColor;
			label.TextColor = UIColor.Yellow;
		}
		/// <summary>
		/// The cell isn't in a valid path anymore
		/// </summary>
		public void UnmarkComplete ()
		{
			label.BackgroundColor = UIColor.White;
			label.TextColor = color.UIColor;
		}

		#region Events

		/// <summary>
		/// Cell has been selected (touched)
		/// </summary>
		public void SelectCell ()
		{
			this.Transform = CGAffineTransform.MakeScale (1.25f, 1.25f);

			UIView.Animate (0.5f,
            () => {
				this.Transform = CGAffineTransform.MakeScale (1f, 1f);					
			});
		}
		/// <summary>
		/// Touch released
		/// </summary>
		public void UnselectCell ()
		{

		}
		/// <summary>
		/// Tell the cell that its part of a path
		/// </summary>
		/// <param name="direction">Direction.</param>
		/// <param name="firstCell">First cell.</param>
		public void CreatePath (PointF direction, GridCell firstCell)
		{

		}
		#endregion

		#region Properties

		/// <summary>
		/// Tells if we are on a cell that is the start or the end of a complete path
		/// </summary>
		/// <value><c>true</c> if this instance is path start or end; otherwise, <c>false</c>.</value>
		public bool IsPathStartOrEnd {
			get {
				return pathLength > 0;
			}
		}
		/// <summary>
		/// Tells if the cell has the right path
		/// </summary>
		public bool IsValidPath {
			get {
				if (IsPathStartOrEnd) {
					return true;
				} else {
					return color.Equals(pathColor);
				}
			}
		}
		/// <summary>
		/// Location (X)
		/// </summary>
		/// <value>The x.</value>
		public int X {
			get;
			private set;
		}
		/// <summary>
		/// Location (Y)
		/// </summary>
		/// <value>The y.</value>
		public int Y {
			get;
			private set;
		}
		/// <summary>
		/// Set the right color form the image
		/// </summary>
		/// <value>The color.</value>
		public CellColor Color {
			get;
			private set;
		}
		/// <summary>
		/// Cells has been marked by grid creator
		/// </summary>
		/// <value><c>true</c> if this instance is marked; otherwise, <c>false</c>.</value>
		public bool IsMarked {
			get;
			set;
		}
		#endregion
	}
}

