using System;
using Xamarin.Forms;

namespace PresetPedalForms
{
    public class LoopButton : Button
    {
        public LoopButton()
        {
            this.Clicked += (sender, e) =>
            {
                Selected = !Selected;
            };
        }

        public static readonly BindableProperty SelectedProperty = BindableProperty.Create (
            "Selected", typeof(bool), typeof(LoopButton), false, BindingMode.TwoWay, propertyChanged: OnEventNameChanged);

        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Property changed implementation goes here
            (bindable as LoopButton).Selected = (bool)newValue;
        }

        public int LoopNumber { get; set; }

        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); ToggleColor(value); }
        }

        public void ToggleColor(bool selected)
        {
            if(selected)
            {
                this.BackgroundColor = Color.Blue;
                this.TextColor = Color.White;
            }
            else
            {
                this.BackgroundColor = Color.Transparent;
                this.TextColor = Color.Default;
            }
        }
         
    }
}
