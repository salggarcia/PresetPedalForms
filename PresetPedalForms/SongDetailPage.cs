using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using PresetPedalForms.Models;

namespace PresetPedalForms
{
    public class SongDetailPage : ContentPage
    {
        Entry nameEntry;
        ListView presetList;
        Song bindingSong;
        public SongDetailPage()
        {
            //var nameLabel = new Label
            //{
            //    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            //    FontAttributes = FontAttributes.Bold
            //};
            //nameLabel.SetBinding(Label.TextProperty, "Name");

            ToolbarItems.Add(new ToolbarItem("Pick Preset", "", HandlePickPresetAction, ToolbarItemOrder.Primary, 0));

            nameEntry = new Entry();
            nameEntry.SetBinding(Entry.TextProperty, "Name");

            presetList = new ListView();
            //presetList.ItemsSource = 
            presetList.SetBinding(ListView.ItemsSourceProperty, "Presets");

            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(5)
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(new Label { Text = "Name:", VerticalTextAlignment = TextAlignment.Center }, 0, 0);
            grid.Children.Add(nameEntry, 1, 0);

            Title = "Detail";
            Content = new StackLayout
            {
                Children = {
                    grid,
                    new Label { Text = "Presets:", VerticalTextAlignment = TextAlignment.Center },
                    presetList
                }
            };
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    if (this.Navigation.NavigationStack.Count > 0)
        //        Debug.WriteLine(this.Navigation.NavigationStack.Count);
        //}
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            bindingSong = this.BindingContext as Song;
        }

        async void HandlePickPresetAction()
        {
            List<CheckItem> checkItems = new List<CheckItem>();
            foreach (var preset in App.Presets)
            {
                checkItems.Add(new CheckItem() { preset = preset, Selected = bindingSong.Presets.Contains(preset) });
            }
            SelectMultipleBasePage<CheckItem> selectPage = new SelectMultipleBasePage<CheckItem>(checkItems);
            selectPage.Disappearing += (sender, e) =>
            {
                bindingSong.Presets = selectPage.GetSelection().Select(c => c.preset).ToList().ToObservableCollection();
            };
            selectPage.Title = "Pick Presets";
            await Navigation.PushAsync(selectPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            presetList.ItemsSource = null;
            presetList.ItemsSource = bindingSong.Presets;
        }

        //protected override void OnDisappearing()
        //{
        //     //nameEntry.Text;
        //}
    }
}

