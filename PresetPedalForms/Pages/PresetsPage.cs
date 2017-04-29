using System;
using System.Diagnostics;
using PresetPedalForms.Models;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace PresetPedalForms
{
    public class PresetsPage : ContentPage
    {
        ListView listView;

        public PresetsPage()
        {
            listView = new ListView();
            listView.ItemsSource = App.Presets;
            var temp = new DataTemplate(() => { return new MovableViewCell(); });
            listView.ItemTemplate = temp;
            listView.ItemSelected += ListView_ItemSelected;
            listView.RowHeight = 40;

            Content = listView;
            ToolbarItems.Add(new ToolbarItem("Add", "", HandleAddAction, ToolbarItemOrder.Primary, 0));
            Title = "Presets";
            //Padding = new Thickness(0, 20, 0, 0);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = null;
            listView.ItemsSource = App.Presets;

            //if (Navigation.NavigationStack)
            //App.SaveData();
        }

        void HandleAddAction()
        {
            App.Presets.Add(new Preset());
            App.SaveData();
        }

        async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                Preset preset = e.SelectedItem as Preset;
                if(App.globalModes.LiveMode)
                    App.BLE.SendPreset(preset);
                else
                {
                    var presetDetailPage = new PresetDetailPage();
                    presetDetailPage.BindingContext = preset;
                    listView.SelectedItem = null;
                    await Navigation.PushAsync(presetDetailPage);
                }
            }
        }


        //public class textViewCell : ViewCell
        //{
        //    //public static readonly BindableProperty ShowOrderProperty = BindableProperty.Create(
        //    //"ShowOrder", typeof(bool), typeof(textViewCell), false, BindingMode.TwoWay, propertyChanged: OnEventNameChanged);

        //    //static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        //    //{
        //    //    // Property changed implementation goes here
        //    //    (bindable as textViewCell).ShowOrder = (bool)newValue;
        //    //}

        //    //public bool ShowOrder
        //    //{
        //    //	get { return (bool)GetValue(ShowOrderProperty); }
        //    //	set
        //    //	{
        //    //		SetValue(ShowOrderProperty, value);
        //    //		ToggleShowOrder(value);
        //    //    }
        //    //}

        //    //public void ToggleShowOrder(bool show)
        //    //{
        //    //    up.IsEnabled = show;
        //    //    up.IsVisible = show;
        //    //    down.IsEnabled = show;
        //    //    down.IsVisible = show;
        //    //}

        //    public Image up;
        //    public Image down;
        //    public textViewCell()
        //    {
        //        StackLayout layout = new StackLayout();
        //        layout.Padding = new Thickness(15, 0);
        //        layout.Orientation = StackOrientation.Horizontal;
        //        Label label = new Label { VerticalOptions = LayoutOptions.Center};

        //        label.SetBinding(Label.TextProperty, "Name");

        //        var moreAction = new MenuItem { Text = "Copy" };
        //        moreAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
        //        moreAction.Clicked += OnCopy;

        //        var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true }; // red background
        //        deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
        //        deleteAction.Clicked += OnDelete;

        //        up = new Image();
        //        up.IsEnabled = false;
        //        up.IsVisible = false;
        //        up.GestureRecognizers.Add(new TapGestureRecognizer((obj) => MoveUp(this.BindingContext as Preset)));
        //        up.Source = ImageSource.FromFile("Checked.png");
        //        up.BackgroundColor = Color.Transparent;
        //        up.WidthRequest = 25;
        //        up.HeightRequest = 25;

        //        down = new Image();
        //        down.IsEnabled = false;
        //        down.IsVisible = false;
        //        down.GestureRecognizers.Add(new TapGestureRecognizer((obj) => MoveDown(this.BindingContext as Preset)));
        //        down.Source = ImageSource.FromFile("hamburger.png");
        //        down.BackgroundColor = Color.Transparent;
        //        down.WidthRequest = 25;
        //        down.HeightRequest = 25;

        //        this.ContextActions.Add(moreAction);
        //        this.ContextActions.Add(deleteAction);

        //        layout.Children.Add(label);
        //        layout.Children.Add(up);
        //        layout.Children.Add(down);

        //        layout.VerticalOptions = LayoutOptions.Center;
        //        View = layout;
        //    }

        //    private void MoveUp(Preset item)
        //    {
        //    	int i = App.Presets.IndexOf(item);
        //    	App.Presets.RemoveAt(i);
        //    	App.Presets.Insert(i-1, item);
        //    }

        //    private void MoveDown(Preset item)
        //    {
        //        int i = App.Presets.IndexOf(item);
        //        App.Presets.RemoveAt(i);
        //        App.Presets.Insert(i+1, item);
        //    }

        //    void OnCopy(object sender, EventArgs e)
        //    {
        //        var item = (MenuItem)sender;
        //        var preset = item.CommandParameter as Preset;
        //        CopyPreset(preset);
        //    }

        //    public void CopyPreset(Preset preset)
        //    {
        //        var presetIdx = App.Presets.IndexOf(preset);
        //        var newPreset = new Preset()
        //        {
        //            Name = preset.Name,
        //            LoopDevices = preset.LoopDevices.ToList(),
        //            MidiDevices = preset.MidiDevices.ToList(),
        //            ExpressionDevices = preset.ExpressionDevices.ToList(),
        //            SwitchDevices = preset.SwitchDevices.ToList()
        //        };
        //        App.Presets.Insert(presetIdx + 1, newPreset);

        //        //SavePresetFile();
        //    }

        //    void OnDelete(object sender, EventArgs e)
        //    {
        //        var item = (MenuItem)sender;

        //        App.Presets.Remove(item.CommandParameter as Preset);
        //    }
        //}
    
    }
}

