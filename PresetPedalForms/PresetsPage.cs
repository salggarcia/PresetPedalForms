using System;
using System.Diagnostics;
using PresetPedalForms.Models;
using Xamarin.Forms;
using System.Collections.Generic;

namespace PresetPedalForms
{
    public class PresetsPage : ContentPage
    {
        ListView listView;
        public PresetsPage()
        {
            listView = new ListView();

            listView.ItemsSource = App.Presets;
            //listView.ItemTemplate = new DataTemplate(typeof(PresetViewModel));
            listView.ItemSelected += ListView_ItemSelected;

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
        }

        void HandleAddAction()
        {
            App.Presets.Add(new Preset());
        }

        async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                var presetDetailPage = new PresetDetailPage();
                Preset preset = e.SelectedItem as Preset;
                presetDetailPage.BindingContext = preset;
                listView.SelectedItem = null;
                await Navigation.PushAsync(presetDetailPage);
            }
        }
    }
}

