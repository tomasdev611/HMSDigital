using System;
using System.Drawing;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using MobileApp.Controls;
using MobileApp.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchEntry), typeof(SearchEntryRenderer))]
namespace MobileApp.iOS.Renderers
{
    public class SearchEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            var element = (SearchEntry)this.Element;
            var textField = this.Control;
            switch (element.ImageAlignment)
            {
                case ImageAlignment.Left:
                    textField.LeftViewMode = UITextFieldViewMode.Always;
                    textField.LeftView = GetImageView(element.Image, element.ImageHeight, element.ImageWidth);
                    break;
                case ImageAlignment.Right:
                    textField.RightViewMode = UITextFieldViewMode.Always;
                    textField.RightView = GetImageView(element.Image, element.ImageHeight, element.ImageWidth);
                    break;
            }

            UpdateBackground();

            textField.Layer.MasksToBounds = true;
        }

        protected void UpdateBackground()
        {
            var element = (SearchEntry)this.Element;

            if (this.Control == null) return;
            this.Control.Layer.CornerRadius = element.CornerRadius;
            this.Control.Layer.BorderWidth = element.BorderThickness;
            this.Control.Layer.BorderColor = element.BorderColor.ToCGColor();
        }

        private UIView GetImageView(string imagePath, int height, int width)
        {
            UIImageView uiImageView;

            if (String.IsNullOrEmpty(imagePath))
            {
                uiImageView = new UIImageView(UIImage.FromBundle("searchIcon.png"))
                {
                    Frame = new RectangleF(15, 0, width, height)
                };
            }
            else
            {
                uiImageView = new UIImageView(UIImage.FromBundle(imagePath))
                {
                    Frame = new RectangleF(0, 0, width, height)
                };
            }

            UIView objLeftView = new UIView(new System.Drawing.Rectangle(5, 0, width + 15, height));
            objLeftView.AddSubview(uiImageView);

            return objLeftView;
        }
    }
}