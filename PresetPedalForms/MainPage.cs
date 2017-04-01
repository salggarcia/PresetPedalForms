using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace PresetPedalForms
{
    public class MainPage : MasterDetailPage
    {
        MasterPage masterPage;

        public MainPage()
        {
            masterPage = new MasterPage();
            Master = masterPage;
            Detail = new NavigationPage(new PresetsPage());

            masterPage.ListView.ItemSelected += OnItemSelected;

            if(Device.OS == TargetPlatform.Windows)
            {
                Master.Icon = "swap.png";
            }
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if(item != null)
            {
                if(item.TargetType.Equals(typeof(ProfilePage)))
                {
                    Debug.WriteLine("Going to profile page");
                    ProfilePage profilePage = (ProfilePage)Activator.CreateInstance(item.TargetType);
                    profilePage.BindingContext = App.mainProfile;
                    Detail = (Page)Activator.CreateInstance(typeof(NavigationPage), profilePage);
                }
                else
                {
                    Detail = (Page)Activator.CreateInstance(typeof(NavigationPage), Activator.CreateInstance(item.TargetType));
                }
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}

