using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using PresetPedalForms.Models;
using Xamarin.Forms;

namespace PresetPedalForms
{
    public class PresetDetailPage : ContentPage
    {
        Preset bindingPreset;

        Entry nameEntry;
        TableView tableView;
        TableSection midiTableSection;
        TableSection loopTableSection;
        TableSection expTableSection;
        TableSection switchTableSection;
            
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            bindingPreset = BindingContext as Preset;

            ConfigurePage();
        }

        public List<string> defualtTLnames = new List<string>
        {
            "DOT8",
            "QRTR",
            "DOT8LL",
            "DOT8LH",
            "DOT8ML",
            "DOT8MH",
            "DOT8HL",
            "DOT8HH",
            "QRTRLL",
            "QRTRLH",
            "QRTRML",
            "QRTRMH",
            "QRTRHL",
            "QRTRHH",

        };

        public void ConfigurePage()
        {
            // Loops
            if(bindingPreset.LoopDevices.Count > 0)
            {   
                loopTableSection = new TableSection("Loops");

                int numLoopsAdded = 0;
                var numLoops = bindingPreset.LoopDevices.Count;
                var numLoopGrids = Math.Ceiling((double)numLoops / 4);
                // Add loop grid mod 4 until out of numberOfLoops
                for(int i = 0; i < numLoopGrids; i++)
                {
                    Grid loopGrid = new Grid { HorizontalOptions = LayoutOptions.StartAndExpand, Padding = new Thickness(15,0,15,0), ColumnSpacing = 15  };
                    loopTableSection.Add(new ViewCell { View = loopGrid });

                    for(int j = 0; j < 4; j++)
                    {

                        var loopNum = new Label { Text = (numLoopsAdded + 1).ToString(), FontSize = 12 };
                        LoopButton loopButton = new LoopButton { /*Text = (numLoopsAdded + 1).ToString() + ": ",*/ VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand, WidthRequest = 60 };
                        loopButton.SetBinding(Button.TextProperty, "LoopDevices[" + numLoopsAdded.ToString() + "].Name");
                        loopButton.SetBinding(LoopButton.SelectedProperty, "LoopDevices[" + numLoopsAdded.ToString() + "].OnOff");
                        loopButton.LoopNumber = numLoopsAdded+1;
                        loopButton.Clicked += (sender, e) => 
                        {
                            if(App.globalModes.ManualMode)
                                App.BLE.SendManualMessage(Convert.ToByte(loopButton.LoopNumber), Convert.ToByte(loopButton.Selected));
                        };
                        StackLayout loopStack = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = {
                                loopNum, loopButton
                             }
                        };
                        loopGrid.Children.Add(loopStack, j, 0);
                        loopGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                        numLoopsAdded++;
                        if(numLoopsAdded == numLoops)
                            break;
                    }
                }
                tableView.Root.Add(loopTableSection);
            }

            // Midi
            if(bindingPreset.MidiDevices.Count > 0)
            {
                midiTableSection = new TableSection("Midi Devices");
                foreach(var device in bindingPreset.MidiDevices)
                {
                    var currentIndex = bindingPreset.MidiDevices.IndexOf(device);
                    var midiGrid = new Grid { HorizontalOptions = LayoutOptions.Fill, Padding = new Thickness(15, 0, 15, 0) };
                    midiGrid.Children.Add(new Label { Text = device.GetType().Name + ":", VerticalOptions = LayoutOptions.Center }, 0, 0);
                    var picker = new Picker { VerticalOptions = LayoutOptions.Center };
                    picker.SelectedIndexChanged += (sender, e) => 
                    {
                        if(App.globalModes.ManualMode)
                            App.BLE.SendManualMessage(device.ManualMessageType, Convert.ToByte(picker.SelectedIndex));
                    };
                    for(int i = 0; i < device.ProgramCount; i++)
                    {
                        picker.Items.Add(i.ToString());
                    }
                    picker.SetBinding(Picker.SelectedIndexProperty, "MidiDevices[" + currentIndex.ToString() + "].SelectedProgram");
                    midiGrid.Children.Add(picker, 1, 0);
                    var enabledSwitch = new Switch { VerticalOptions = LayoutOptions.Center };
                    enabledSwitch.SetBinding(Switch.IsToggledProperty, "MidiDevices[" + currentIndex.ToString() + "].Enabled");
                    enabledSwitch.Toggled += (sender, e) => 
                    {
                        if(App.globalModes.ManualMode)
                            App.BLE.SendManualMessageCC(device.ManualCCType, Convert.ToByte(102), Convert.ToByte(device.EnabledIntValue)); // TODO: change 102 midi_byp_cc to general midi
                    };
                    midiGrid.Children.Add(enabledSwitch, 2, 0);
                    midiGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });
                    midiGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    midiGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                    midiTableSection.Add(new ViewCell { View = midiGrid });
                }
                tableView.Root.Add(midiTableSection);
            }

            // Expression controls
            if(bindingPreset.ExpressionDevices.Count > 0)
            {
                expTableSection = new TableSection("Expression Devices");
                foreach(var device in bindingPreset.ExpressionDevices)
                {
                    var currentIndex = bindingPreset.ExpressionDevices.IndexOf(device);

                    var expGrid = new Grid { HorizontalOptions = LayoutOptions.Fill, Padding = new Thickness(15, 0, 15, 0) };
                    expGrid.Children.Add(new Label { Text = device.GetType().GetTypeInfo().Name + ":", VerticalOptions = LayoutOptions.Center }, 0, 0);
                    var slider = new Slider { VerticalOptions = LayoutOptions.Center };
                    slider.SetBinding(Slider.ValueProperty, "ExpressionDevices[" + currentIndex.ToString() + "].ExpressionDoubleValue");
                    var sliderLabel = new Label { VerticalOptions = LayoutOptions.Center, Text = device.ExpressionIntValue.ToString() };
                    slider.ValueChanged += (sender, e) =>
                    {
                        var actualValue = Convert.ToInt32(slider.Value * device.SLIDERRANGE);
                        sliderLabel.Text = actualValue.ToString();
                        if(App.globalModes.ManualMode)
                        {
                            if(device.GetType().Equals(typeof(Riverside)))
                                App.BLE.SendManualMessage(device.ManualMessageType, Convert.ToByte((int)(127 - App.BLE.LogExp[actualValue])));
                            else
                                App.BLE.SendManualMessage(device.ManualMessageType, Convert.ToByte(App.BLE.LogExp[actualValue]));
                        }

                    };
                    expGrid.Children.Add(slider, 1, 0);
                    expGrid.Children.Add(sliderLabel, 2, 0);
                    expGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
                    expGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) });
                    expGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    expTableSection.Add(new ViewCell { View = expGrid });
                }
                tableView.Root.Add(expTableSection);
            }

            // Switch controls
            if(bindingPreset.SwitchDevices.Count > 0)
            {
                switchTableSection = new TableSection("Switch Devices");
                foreach(var device in bindingPreset.SwitchDevices)
                {
                    var currentIndex = bindingPreset.SwitchDevices.IndexOf(device);

                    var switchGrid = new Grid { HorizontalOptions = LayoutOptions.StartAndExpand, Padding = new Thickness(15, 0, 15, 0) };
                    switchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    switchGrid.Children.Add(new Label { Text = device.GetType().GetTypeInfo().Name + ":", VerticalOptions = LayoutOptions.Center }, 0, 0);
                    var switchControl = new Switch { VerticalOptions = LayoutOptions.Center };
                    switchControl.SetBinding(Switch.IsToggledProperty, "SwitchDevices[" + currentIndex.ToString() + "].SwitchValue");
                    switchControl.Toggled += (sender, e) => 
                    {
                        if(App.globalModes.ManualMode)
                            App.BLE.SendManualMessage(device.ManualMessageType, Convert.ToByte(device.SwitchValue));
                    };
                    switchGrid.Children.Add(switchControl, 1, 0);

                    switchTableSection.Add(new ViewCell { View = switchGrid });
                }
                tableView.Root.Add(switchTableSection);
            }

        }

        public ToolbarItem manualButton;
        public PresetDetailPage()
        {
            manualButton = new ToolbarButton();
            manualButton.Order = ToolbarItemOrder.Primary;
            manualButton.Text = "Manual";
            //manualButton.SetBinding(ToolbarButton.SelectedProperty, "App.globalModes.ManualMode");
            ToolbarItems.Add(manualButton);

            nameEntry = new Entry();
            nameEntry.Effects.Add(new ClearEntryEffect());
            nameEntry.Effects.Add(new EntryCapitalizeKeyboard());
            nameEntry.SetBinding(Entry.TextProperty, "Name");
            nameEntry.Keyboard = Keyboard.Text;
            //nameEntry.Keyboard = Keyboard.Create(KeyboardFlags.)
            var nameGrid = new Grid {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(5)
            };
            nameGrid.Children.Add(new Label { Text = "Name:", VerticalTextAlignment = TextAlignment.Center }, 0, 0);
            nameGrid.Children.Add(nameEntry, 1, 0);
            nameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
            nameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            nameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });

            tableView = new TableView
            {
                Intent = TableIntent.Data,
                Root = new TableRoot("TableView Title")
            };

            Title = "Detail";
            Content = new StackLayout
            {
                Children = {
                    nameGrid,
                    tableView
                }
            };

        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            App.SaveData();
        }

    }
}

