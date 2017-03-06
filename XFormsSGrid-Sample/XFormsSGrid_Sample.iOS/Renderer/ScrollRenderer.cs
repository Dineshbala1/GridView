using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using XamarinForms_StaggeredGrid.Control;
using XFormsSGrid_Sample.iOS.Renderer;

//[assembly:Xamarin.Forms.ExportRenderer(typeof(StaggeredScrollView),typeof(ScrollRenderer))]
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
            if (NativeView is UIScrollView)
                (NativeView as UIScrollView).Scrolled += ScrollRenderer_Scrolled;
        }

        private void ScrollRenderer_Scrolled(object sender, EventArgs e)
        {

        }
    }
}
