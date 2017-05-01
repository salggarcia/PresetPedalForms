using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            listView.ItemTemplate = new DataTemplate(() => { return new MovableViewCell(false, true); });
            listView.ItemSelected += ListView_ItemSelected;
            listView.RowHeight = 40;

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
            App.SaveData();
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

        //public class textViewCell : ViewCell
        //{
        //	public textViewCell()
        //	{
        //		StackLayout layout = new StackLayout();
        //		layout.Padding = new Thickness(15, 0);
        //		Label label = new Label { VerticalOptions = LayoutOptions.Center };

        //		label.SetBinding(Label.TextProperty, "Name");
        //		layout.Children.Add(label);

        //		var moreAction = new MenuItem { Text = "Copy" };
        //		moreAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
        //		moreAction.Clicked += OnCopy;

        //		var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true }; // red background
        //		deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
        //		deleteAction.Clicked += OnDelete;

        //		this.ContextActions.Add(moreAction);
        //		this.ContextActions.Add(deleteAction);

        //		layout.VerticalOptions = LayoutOptions.Center;
        //		View = layout;
        //	}

        //	void OnCopy(object sender, EventArgs e)
        //	{
        //		var item = (MenuItem)sender;
        //		var song = item.CommandParameter as Song;
        //		CopySong(song);
        //	}

        //	public void CopySong(Song song)
        //	{
        //		var songIdx = App.Songs.IndexOf(song);
        //        var newSong = new Song()
        //        {
        //            Name = song.Name,
        //            Presets = song.Presets
        //		};
        //		App.Songs.Insert(songIdx + 1, newSong);

        //		//SavePresetFile();
        //        App.SaveData();
        //	}

        //	void OnDelete(object sender, EventArgs e)
        //	{
        //		var item = (MenuItem)sender;

        //        App.Songs.Remove(item.CommandParameter as Song);
        //        App.SaveData();
        //    }
        //}
    }
}

