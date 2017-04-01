using System;
using System.Diagnostics;
using PresetPedalForms.Models;
using Xamarin.Forms;

namespace PresetPedalForms
{
    public class SongsPage : ContentPage
    {
        ListView listView;
        public SongsPage()
        {
            listView = new ListView();
            listView.ItemsSource = App.Songs;
            //listView.ItemTemplate = new DataTemplate(typeof(SongViewModel));
            listView.ItemSelected += ListView_ItemSelected;

            Content = listView;
            ToolbarItems.Add(new ToolbarItem("Add", "", HandleAddAction, ToolbarItemOrder.Primary, 0));
            Title = "Songs";
            //Padding = new Thickness(0, 20, 0, 0);
            //Icon = "Images/SongsIcon.png";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = null;
            listView.ItemsSource = App.Songs;
        }

        void HandleAddAction()
        {
            App.Songs.Add(new Song());
        }

        async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                var songDetailPage = new SongDetailPage();
                songDetailPage.BindingContext = e.SelectedItem as Song;
                listView.SelectedItem = null;
                await Navigation.PushAsync(songDetailPage);
            }
        }
    }
}

