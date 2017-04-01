using System;
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

        public void ConfigurePage()
        {
            // Loops
            if(bindingPreset.Loops.Count > 0)
            {   
                loopTableSection = new TableSection("Loops");

                int numLoopsAdded = 0;
                var numLoops = bindingPreset.Loops.Count;
                var numLoopGrids = Math.Ceiling((double)numLoops / 4);
                // Add loop grid mod 4 until out of numberOfLoops
                for(int i = 0; i < numLoopGrids; i++)
                {
                    Grid loopGrid = new Grid { HorizontalOptions = LayoutOptions.FillAndExpand };
                    loopGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    loopTableSection.Add(new ViewCell { View = loopGrid });

                    for(int j = 0; j < 4; j++)
                    {
                        LoopButton loopButton = new LoopButton { Text = "Loop " + (numLoopsAdded + 1).ToString(), VerticalOptions = LayoutOptions.Center };
                        loopButton.SetBinding(LoopButton.SelectedProperty, "Loops[" + numLoopsAdded.ToString() + "]");
                        loopGrid.Children.Add(loopButton, j, 0);
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
                    var midiGrid = new Grid { HorizontalOptions = LayoutOptions.StartAndExpand };
                    midiGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    midiGrid.Children.Add(new Label { Text = device.GetType().Name + ":", VerticalOptions = LayoutOptions.Center }, 0, 0);
                    var picker = new Picker { VerticalOptions = LayoutOptions.Center };
                    for(int i = 0; i < device.ProgramCount; i++)
                    {
                        picker.Items.Add(i.ToString());
                    }
                    picker.SetBinding(Picker.SelectedIndexProperty, "MidiDevices[" + currentIndex.ToString() + "].SelectedProgram");
                    midiGrid.Children.Add(picker, 1, 0);
                    var enabledSwitch = new Switch { VerticalOptions = LayoutOptions.Center };
                    enabledSwitch.SetBinding(Switch.IsToggledProperty, "MidiDevices[" + currentIndex.ToString() + "].Enabled");
                    midiGrid.Children.Add(enabledSwitch, 2, 0);
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

                    var expGrid = new Grid { HorizontalOptions = LayoutOptions.StartAndExpand };
                    expGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    expGrid.Children.Add(new Label { Text = device.GetType().GetTypeInfo().Name + ":", VerticalOptions = LayoutOptions.Center }, 0, 0);
                    var slider = new Slider { VerticalOptions = LayoutOptions.Center };
                    slider.SetBinding(Slider.ValueProperty, "ExpressionDevices[" + currentIndex.ToString() + "].ExpressionDoubleValue");
                    var sliderLabel = new Label { VerticalOptions = LayoutOptions.Center, Text = device.ExpressionIntValue.ToString() };
                    slider.ValueChanged += (sender, e) =>
                    {
                        sliderLabel.Text = Convert.ToInt32(slider.Value * device.SLIDERRANGE).ToString();
                    };
                    //sliderLabel.SetBinding(Label.TextProperty, "ExpressionDevices[" + currentIndex.ToString() + "].ExpressionValue");
                    expGrid.Children.Add(slider, 1, 0);
                    expGrid.Children.Add(sliderLabel, 2, 0);

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

                    var switchGrid = new Grid { HorizontalOptions = LayoutOptions.StartAndExpand };
                    switchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    switchGrid.Children.Add(new Label { Text = device.GetType().GetTypeInfo().Name + ":", VerticalOptions = LayoutOptions.Center }, 0, 0);
                    var switchControl = new Switch { VerticalOptions = LayoutOptions.Center };
                    switchControl.SetBinding(Switch.IsToggledProperty, "SwitchDevices[" + currentIndex.ToString() + "].SwitchValue");
                    switchGrid.Children.Add(switchControl, 1, 0);

                    switchTableSection.Add(new ViewCell { View = switchGrid });
                }
                tableView.Root.Add(switchTableSection);
            }

        }

        public PresetDetailPage()
        {
            nameEntry = new Entry();
            nameEntry.SetBinding(Entry.TextProperty, "Name");

            var nameGrid = new Grid {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(5)
            };
            nameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
            nameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            nameGrid.Children.Add(new Label { Text = "Name:", VerticalTextAlignment = TextAlignment.Center }, 0, 0);
            nameGrid.Children.Add(nameEntry, 1, 0);

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

    }
}

