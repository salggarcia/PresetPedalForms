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
                IconSource = "PresetsIcon.png",
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
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.None
            };

            Padding = new Thickness(0, 40, 0, 0);
            Icon = "hamburger.png";
            Title = "Personal Organiser";
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    listView
                }
            };
        }
    }
}

