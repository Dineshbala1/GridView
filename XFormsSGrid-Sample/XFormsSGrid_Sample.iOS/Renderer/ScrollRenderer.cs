using System;
using System.Collections.Generic;
using System.Text;
using CoreGraphics;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using XamarinForms_GridView.Control;
using XFormsSGrid_Sample.iOS.Renderer;

[assembly:Xamarin.Forms.ExportRenderer(typeof(GridView),typeof(ScrollRenderer))]
namespace XFormsSGrid_Sample.iOS.Renderer
{
    public class ScrollRenderer : ScrollViewRenderer
    {
        public static void init()
        {
            
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;
            e.NewElement.PropertyChanged += NewElement_PropertyChanged;
            if (NativeView is UIScrollView)
                (NativeView as UIScrollView).Scrolled += ScrollRenderer_Scrolled;
        }

        private void NewElement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Renderer")
            {
                (NativeView as UIScrollView).ContentSize = new CGSize( (Element as GridView).Content.Width, (Element as GridView).Content.Height);
            }
        }

        public override void Draw(CGRect rect)
        {
            (NativeView as UIScrollView).ContentSize = new CGSize((Element as GridView).Content.Width,
                (Element as GridView).Content.Height);
            base.Draw(rect);
        }

        private void ScrollRenderer_Scrolled(object sender, EventArgs e)
        {

        }
    }
}
