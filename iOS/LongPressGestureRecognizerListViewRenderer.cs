using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using PresetPedalForms;
using PresetPedalForms.iOS;

[assembly: ExportRenderer(typeof(ListViewWithLongPressGesture), typeof(LongPressGestureRecognizerListViewRenderer))]
namespace PresetPedalForms.iOS
{
    public class LongPressGestureRecognizerListViewRenderer : ListViewRenderer
    {
        ListViewWithLongPressGesture view;

        public LongPressGestureRecognizerListViewRenderer()
        {
        	this.AddGestureRecognizer(new UILongPressGestureRecognizer((longPress) =>
        	{
        		if(longPress.State == UIGestureRecognizerState.Began)
        		{
        			view.HandleLongPress(view, EventArgs.Empty);
        		}
        	}));
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if(e.NewElement != null)
                view = e.NewElement as ListViewWithLongPressGesture;
        }
    }
}
