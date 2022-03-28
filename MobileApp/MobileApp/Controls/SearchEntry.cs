using System;
using Xamarin.Forms;

namespace MobileApp.Controls
{
    public class SearchEntry : Entry
    {
		public SearchEntry()
		{
			this.HeightRequest = 50;
		}

		public static readonly BindableProperty ImageProperty =
			BindableProperty.Create(nameof(Image), typeof(string), typeof(SearchEntry), string.Empty);

		public static readonly BindableProperty ImageHeightProperty =
			BindableProperty.Create(nameof(ImageHeight), typeof(int), typeof(SearchEntry), 20);

		public static readonly BindableProperty ImageWidthProperty =
			BindableProperty.Create(nameof(ImageWidth), typeof(int), typeof(SearchEntry), 20);

		public static readonly BindableProperty ImageAlignmentProperty =
			BindableProperty.Create(nameof(ImageAlignment), typeof(ImageAlignment), typeof(SearchEntry), ImageAlignment.Left);

		public static BindableProperty CornerRadiusProperty =
			BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(SearchEntry), 3);

		public static BindableProperty BorderThicknessProperty =
			BindableProperty.Create(nameof(BorderThickness), typeof(int), typeof(SearchEntry), 1);

		public static BindableProperty PaddingProperty =
			BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(SearchEntry), new Thickness(5));

		public static BindableProperty BorderColorProperty =
			BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(SearchEntry), Color.Transparent);

		/// <summary>
        /// Image Width
        /// </summary>
		public int ImageWidth
		{
			get { return (int)GetValue(ImageWidthProperty); }
			set { SetValue(ImageWidthProperty, value); }
		}

		/// <summary>
		/// Image Height
		/// </summary>
		public int ImageHeight
		{
			get { return (int)GetValue(ImageHeightProperty); }
			set { SetValue(ImageHeightProperty, value); }
		}

		/// <summary>
        /// Image
        /// </summary>
		public string Image
		{
			get { return (string)GetValue(ImageProperty); }
			set { SetValue(ImageProperty, value); }
		}

		/// <summary>
        /// Image Alignment
        /// </summary>
		public ImageAlignment ImageAlignment
		{
			get { return (ImageAlignment)GetValue(ImageAlignmentProperty); }
			set { SetValue(ImageAlignmentProperty, value); }
		}

		/// <summary>
        /// Corner Radius for the Entry
        /// </summary>
		public int CornerRadius
		{
			get => (int)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}

		/// <summary>
        /// Border Thickness for the Entry
        /// </summary>
		public int BorderThickness
		{
			get => (int)GetValue(BorderThicknessProperty);
			set => SetValue(BorderThicknessProperty, value);
		}

		/// <summary>
        /// Border Color for the entry
        /// </summary>
		public Color BorderColor
		{
			get => (Color)GetValue(BorderColorProperty);
			set => SetValue(BorderColorProperty, value);
		}

		/// <summary>
		/// Padding
		/// </summary>
		public Thickness Padding
		{
			get => (Thickness)GetValue(PaddingProperty);
			set => SetValue(PaddingProperty, value);
		}
	}

	/// <summary>
    /// Image Alignment for the SearchEntry
    /// </summary>
	public enum ImageAlignment
	{
		Left,
		Right
	}
}