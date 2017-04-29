using System.Collections.Generic;
using Xamarin.Forms;

namespace PresetPedalForms
{
    public class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        ListView listView;

        public MasterPage()
        {
            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Account",
                IconSource = "Account.png",
                TargetType = typeof(ProfilePage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Presets",
                IconSource = "PresetsIcon.png",
                TargetType = typeof(PresetsPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Songs",
                IconSource = "SongsIcon.png",
                TargetType = typeof(SongsPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Settings",
                IconSource = "Settings.png",
                TargetType = typeof(SettingsPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Live",
                IconSource = "SongsIcon.png",
                TargetType = typeof(LivePage)
            });

            listView = new ListView
            {
                ItemsSource = masterPageItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var imageCell = new ImageCell();
                    imageCell.SetBinding(TextCell.TextProperty, "Title");
                    imageCell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
                    return imageCell;
                }),
                VerticalOptions = LayoutOptions.Start,
                HasUnevenRows = true,
                HeightRequest = 230,
                SeparatorVisibility = SeparatorVisibility.None
            };

            var liveModeGrid = new Grid { VerticalOptions = LayoutOptions.Start, Padding = new Thickness(15, 0, 15, 0) };
            liveModeGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            liveModeGrid.Children.Add(new Label { Text = "Live Mode", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var switchControl = new Switch { VerticalOptions = LayoutOptions.Center };
            switchControl.SetBinding(Switch.IsToggledProperty, "LiveMode");
            liveModeGrid.Children.Add(switchControl, 1, 0);

            var manualModeGrid = new Grid { VerticalOptions = LayoutOptions.Start, Padding = new Thickness(15, 0, 15, 0) };
            manualModeGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            manualModeGrid.Children.Add(new Label { Text = "Manual Mode", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var manualSwitchControl = new Switch { VerticalOptions = LayoutOptions.Center };
            manualSwitchControl.SetBinding(Switch.IsToggledProperty, "ManualMode");
            manualModeGrid.Children.Add(manualSwitchControl, 1, 0);

            Padding = new Thickness(0, 40, 0, 0);
            Icon = "hamburger.png";
            Title = "Personal Organiser";
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    listView,
                    liveModeGrid,
                    manualModeGrid
                }
            };
        }
    }
}

