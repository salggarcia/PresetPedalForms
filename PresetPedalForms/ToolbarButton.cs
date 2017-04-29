using System;
using Xamarin.Forms;

namespace PresetPedalForms
{
    public class ToolbarButton : ToolbarItem
    {
        public ToolbarButton()
        {
            this.Clicked += (sender, e) =>
            {
                Selected = !Selected;
            };

        }

        public static readonly BindableProperty SelectedProperty = BindableProperty.Create(
            "Selected", typeof(bool), typeof(ToolbarButton), false, BindingMode.TwoWay, propertyChanged: OnEventNameChanged);

        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Property changed implementation goes here
            (bindable as ToolbarButton).Selected = (bool)newValue;
        }

        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set { 
                SetValue(SelectedProperty, value); 
                ToggleColor(value);
            }
        }

        public void ToggleColor(bool selected)
        {
            if(selected)
            {
                this.Icon = "ManualOn.png";
            }
            else
            {
                this.Icon = "ManualOff.png";
            }
        }

    }
}
