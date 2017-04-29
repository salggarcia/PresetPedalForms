using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace PresetPedalForms
{
    public class SettingsPage : ContentPage
    {
        TableSection pedalboardTableSection;
        TableSection controllerTableSection;
        public SettingsPage()
        {
            var viewPBView = new ViewCell
            {
                View = new Label { Text = "View Pedalboard", VerticalOptions = LayoutOptions.Center, Margin = new Thickness(15, 0, 0, 0) }
            };
            viewPBView.Tapped += async (sender, e) => {
                var bleViewPBPage = new BLEViewPBPage();
                //Preset preset = e.SelectedItem as Preset;
                bleViewPBPage.BindingContext = App.mainProfile.BondedPedalDevice;
                await Navigation.PushAsync(bleViewPBPage);
            };
            var newPBView = new ViewCell {
                View = new Label { Text = "Connect to New Pedalboard", VerticalOptions = LayoutOptions.Center, Margin = new Thickness(15, 0, 0, 0) }
            };
            newPBView.Tapped += async (sender, e) => {
                var bleNewPBPage = new BLENewPBPage();
                //Preset preset = e.SelectedItem as Preset;
                //presetDetailPage.BindingContext = preset;
                await Navigation.PushAsync(bleNewPBPage);
            };
            pedalboardTableSection = new TableSection("Pedalboard") {
                viewPBView,
                newPBView
            };

            var viewCTView = new ViewCell
            {
                View = new Label { Text = "View Controller", VerticalOptions = LayoutOptions.Center, Margin = new Thickness(15, 0, 0, 0) }
            };
            viewCTView.Tapped += async(sender, e) => {
                var bleViewCTPage = new BLEViewCTPage();
                //Preset preset = e.SelectedItem as Preset;
                bleViewCTPage.BindingContext = App.mainProfile.BondedControllerDevice;
                await Navigation.PushAsync(bleViewCTPage);
            };
            var newCTView = new ViewCell {
                View = new Label { Text = "Connect to New Controller", VerticalOptions = LayoutOptions.Center, Margin = new Thickness(15, 0, 0, 0) }
            };
            newCTView.Tapped += async(sender, e) => {
                var bleNewCTPage = new BLENewCTPage();
                //Preset preset = e.SelectedItem as Preset;
                //presetDetailPage.BindingContext = preset;
                await Navigation.PushAsync(bleNewCTPage);
            };
            controllerTableSection = new TableSection("Controller")
            {
        	    viewCTView,
                newCTView
            };


            TableView tableView = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot("TableView Title")
                {
                    pedalboardTableSection,
                    controllerTableSection
                },
            };

            Title = "Profile";
            Content = new StackLayout
            {
                Children = {
                    tableView
                }
            };

            ConfigurePage();
        }

        public void ConfigurePage()
        {
            //// Loop stepper
            //loopStepper.Value = App.mainProfile.NumberOfLoops;
            //loopNumberLabel.Text = App.mainProfile.NumberOfLoops.ToString();

            //// Midi devices
            //foreach(var device in App.mainProfile.MidiDevices)
            //{
            //    AddMidiDevice(false, device.Name);
            //}

            //// Expression devices
            //foreach(var device in App.mainProfile.ExpressionDevices)
            //{
            //    AddExpDevice(false, device.Name);
            //}

            //// Switch devices
            //foreach(var device in App.mainProfile.SwitchDevices)
            //{
            //    AddSwitchDevice(false, device.Name);
            //}
        }
    }
}

