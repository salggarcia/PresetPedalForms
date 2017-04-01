using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace PresetPedalForms
{
    public class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage(bool isPreset)
        {
            PresetsPage presetsPage = new PresetsPage();
            var presetsNavPage = new NavigationPage(presetsPage);
            presetsNavPage.Icon = "PresetsIcon.png";
            presetsNavPage.Title = "Presets";
            //presetsNavPage.ToolbarItems.Add(new ToolbarItem("Add", "", HandleAddAction, ToolbarItemOrder.Primary, 0));
            Children.Add(presetsNavPage);

            SongsPage songsPage = new SongsPage();
            var songsNavPage = new NavigationPage(songsPage);
            songsNavPage.Icon = "SongsIcon.png";
            songsNavPage.Title = "Songs";
            Children.Add(songsNavPage);

            this.CurrentPage = isPreset ? presetsNavPage as NavigationPage : songsNavPage as NavigationPage;
        }

    }
}

