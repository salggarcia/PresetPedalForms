using System;
using System.Collections.Generic;
using PresetPedalForms.Models;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace PresetPedalForms
{
    public class App : Application
    {
        public static ObservableCollection<Preset> Presets = new ObservableCollection<Preset>();
        public static ObservableCollection<Song> Songs = new ObservableCollection<Song>();

        public static Profile mainProfile = new Profile(); 

        public App()
        {
            Presets.Add(new Preset() { Name = "First Preset" });

            Songs.Add(new Song() { Name = "First Song", Presets = new ObservableCollection<Preset>() { Presets[0] } });

            var mainPage = new MainPage();
            MainPage = mainPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
