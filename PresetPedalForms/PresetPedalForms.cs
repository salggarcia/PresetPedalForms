using System;
using System.Collections.Generic;
using System.Diagnostics;
using PresetPedalForms.Models;
using System.Collections.ObjectModel;
using PCLStorage;
using Xamarin.Forms;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PresetPedalForms
{
    public class App : Application
    {
        public static User User = new User();

        public static ObservableCollectionEx<Preset> Presets
        {
            get { return User.Presets; }
            set { User.Presets = value; }
        }

        public static ObservableCollectionEx<Song> Songs
        {
            get { return User.Songs; }
            set { User.Songs = value; }
        }

        public static Profile mainProfile
        {
            get { return User.mainProfile; }
            set { User.mainProfile = value; }
        }

        public static BLEController BLE = new BLEController();
        public static bool controlReady = false;
        public static Modes globalModes = new Modes();

        public App()
        {
            AsyncHelpers.RunSync(() => ReadData());

            // Initliaze BLE
            BLE.InitBLE();

            var mainPage = new MainPage();
            mainPage.BindingContext = BLE;
            MainPage = mainPage;
        }

        public static async Task SaveData()
        {
            Contract.Ensures(Contract.Result<Task>() != null);
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("PresetPedalFolder",
                CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync("user.txt",
                CreationCollisionOption.ReplaceExisting);
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
            jsonSettings.TypeNameHandling = TypeNameHandling.All;
            string jsonData = JsonConvert.SerializeObject(User, jsonSettings);
            await file.WriteAllTextAsync(jsonData);
        }

        public static async Task ReadData()
        {
            Contract.Ensures(Contract.Result<Task>() != null);
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            var checkFolder = await rootFolder.CheckExistsAsync("PresetPedalFolder");
            if(checkFolder == ExistenceCheckResult.FolderExists)
            {
                IFolder folder = await rootFolder.GetFolderAsync("PresetPedalFolder");
                var checkFile = await folder.CheckExistsAsync("user.txt");
                if(checkFile == ExistenceCheckResult.FileExists)
                {
                    try
                    {
                        IFile file = await folder.GetFileAsync("user.txt");
                        string jsonData = await file.ReadAllTextAsync();
                        JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                        jsonSettings.TypeNameHandling = TypeNameHandling.All;
                        User = JsonConvert.DeserializeObject<User>(jsonData, jsonSettings);
                        Debug.WriteLine("Read Data");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Read Data exception: " + ex);
                    }
                }
            }
            // First time, create first User and save it
            //User.mainProfile.NumberOfLoops = 4;
            User.Presets.Add(new Preset() { Name = "First Preset" });
            User.Songs.Add(new Song() { Name = "First Song", Presets = new ObservableCollection<Preset>() { User.Presets[0] } });
            await SaveData();
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
