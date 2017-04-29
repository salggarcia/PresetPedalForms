using System;

using Xamarin.Forms;

namespace PresetPedalForms
{
    public class BLENewPBPage : ContentPage
    {
        Button scanButton;
        Button pairedButton;
        public BLENewPBPage()
        {
            scanButton = new Button { Text = "Scan", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.CenterAndExpand };
            scanButton.Clicked += ScanConnectButton_Clicked;
            pairedButton = new Button { Text = "Paired to Pedalboard", VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
            //pairedButton.Clicked += ScanConnectButton_Clicked;
            pairedButton.IsVisible = false;
            Content = new StackLayout
            {
                Children = {
                    scanButton,
                    pairedButton
                }
            };
            Title = "Connect to New Pedalboard";

            App.BLE.PedalConnectedEvent += () =>
            {
                scanButton.Text = "Connected";
                scanButton.IsEnabled = false;
                pairedButton.IsVisible = true;
                pairedButton.Image = "Checked.png";
                App.SaveData();
            };

            App.BLE.PedalDisconnectedEvent += () =>
            {
                scanButton.Text = "Disconnected";
                //pedalConLabel.SetTitle("Disconnected", UIControlState.Normal);
                //pedalConLabel.SetTitleColor(UIColor.Red, UIControlState.Normal);
                //liveButton.Enabled = false;
            };

            //App.BLE.ControlConnectedEvent += () =>
            //{
            //    //controlConLabel.SetTitle("Connected", UIControlState.Normal);
            //    //controlConLabel.SetTitleColor(UIColor.Green, UIControlState.Normal);
            //};

            //App.BLE.ControlDisconnectedEvent += () =>
            //{
            //    //controlConLabel.SetTitle("Disconnected", UIControlState.Normal);
            //    //controlConLabel.SetTitleColor(UIColor.Red, UIControlState.Normal);
            //};
        }

        void ScanConnectButton_Clicked(object sender, EventArgs e)
        {
            // TODO: Scan when press
            App.BLE.StartScan();

            // TODO: Get bonded info and save it
        }



        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    
        //}
    }
}

