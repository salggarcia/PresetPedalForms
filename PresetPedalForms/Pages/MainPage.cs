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
            masterPage.BindingContext = App.globalModes;
            Master = masterPage;
            var detailPage = new NavigationPage(new PresetsPage());

            tbitemPB = new ToolbarItem("Pedalboard: Disconnected", "", HandlePBHitAction, ToolbarItemOrder.Secondary, 0);
            App.BLE.PedalAutoConnectedEvent += () => 
            {
                tbitemPB.SetBinding(ToolbarItem.TextProperty, "pedalConnectedStateString");
            };
            detailPage.ToolbarItems.Add(tbitemPB);

            tbitemCT = new ToolbarItem("Controller: Disconnected", "", HandleCTHitAction, ToolbarItemOrder.Secondary, 1);
            App.BLE.ControlAutoConnectedEvent += () => 
            {
                tbitemCT.SetBinding(ToolbarItem.TextProperty, "controllerConnectedString");
            };
            detailPage.ToolbarItems.Add(tbitemCT);

            Detail = detailPage;
            masterPage.ListView.ItemSelected += OnItemSelected;

            App.BLE.PedalDisconnectedEvent += () =>
            {
                tbitemPB.Text = "Pedalboard: Disconnected";
            };

            App.BLE.ControlDisconnectedEvent += () =>
            {
                tbitemCT.Text = "Controller: Disconnected";
            };

            //if(Device.OS == TargetPlatform.Windows)
            //{
            //    Master.Icon = "swap.png";
            //}
        }

        void HandlePBHitAction()
        {
            Debug.WriteLine("HIT PEDALBOARD");
            if(!App.BLE.pedalConnectedState && !App.mainProfile.BondedPedalDevice.ID.Equals(Guid.Empty))
                App.BLE.AutoConnectPedal();
        }

        void HandleCTHitAction()
        {
	        Debug.WriteLine("HIT CONTROLLER");
            if(!App.BLE.controllerConnectedState && !App.mainProfile.BondedControllerDevice.ID.Equals(Guid.Empty))
                App.BLE.AutoConnectController();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        ToolbarItem tbitemPB;
        ToolbarItem tbitemCT;
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
                //else if(item.TargetType.Equals(typeof(PresetsPage)))
                //{
                //    Debug.WriteLine("Going to presets page");
                //    PresetsPage presetsPage = (PresetsPage)Activator.CreateInstance(item.TargetType);
                //    presetsPage.BindingContext = App.myOrder;
                //    Detail = (Page)Activator.CreateInstance(typeof(NavigationPage), presetsPage);
                //}
                else
                {
                    var page = (Page)Activator.CreateInstance(typeof(NavigationPage), Activator.CreateInstance(item.TargetType));

                    tbitemPB = new ToolbarItem("Pedalboard: Disconnected", "", HandlePBHitAction, ToolbarItemOrder.Secondary, 0);
                    tbitemPB.SetBinding(ToolbarItem.TextProperty, "pedalConnectedStateString");
                    page.ToolbarItems.Add(tbitemPB);

                    tbitemCT = new ToolbarItem("Controller: Disconnected", "", HandleCTHitAction, ToolbarItemOrder.Secondary, 1);
                    tbitemCT.SetBinding(ToolbarItem.TextProperty, "controllerConnectedString");
                    page.ToolbarItems.Add(tbitemCT);

                    Detail = page;
                }
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}

