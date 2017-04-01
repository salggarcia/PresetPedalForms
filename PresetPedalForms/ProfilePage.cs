using System;
using System.Diagnostics;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace PresetPedalForms
{
    public class ProfilePage : ContentPage
    {
        public List<Type> tempMidiDevices = new List<Type>();
        public List<Type> tempExpDevices = new List<Type>();
        public List<Type> tempSwitchDevices = new List<Type>();

        public List<Type> originalMidiDevices = new List<Type>();
        public List<Tuple<int, Type>> removedMidiTuples = new List<Tuple<int, Type>>();

        public List<Type> originalExpDevices = new List<Type>();
        public List<Tuple<int, Type>> removedExpTuples = new List<Tuple<int, Type>>();

        public List<Type> originalSwitchDevices = new List<Type>();
        public List<Tuple<int, Type>> removedSwitchTuples = new List<Tuple<int, Type>>();

        TableSection loopTableSection;
        TableSection midiTableSection;
        TableSection expTableSection;
        TableSection switchTableSection;
        Stepper loopStepper;
        Label loopNumberLabel;

        public ProfilePage()
        {
            // Loops
            loopNumberLabel = new Label{ VerticalOptions = LayoutOptions.Center };
            loopStepper = new Stepper { VerticalOptions = LayoutOptions.Center };
            //loopStepper.SetBinding(Stepper.ValueProperty, "NumberOfLoops");
            loopStepper.ValueChanged += (sender, e) => {
                loopNumberLabel.Text = loopStepper.Value.ToString();
            };
            var loopGrid = new Grid { HorizontalOptions = LayoutOptions.StartAndExpand };
            loopGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            loopGrid.Children.Add(new Label { Text = "Number of Loops", VerticalOptions = LayoutOptions.Center }, 0, 0);
            loopGrid.Children.Add(loopStepper, 1, 0);
            loopGrid.Children.Add(loopNumberLabel, 2, 0);

            // Midi devices
            var addMidiGrid = new Grid { HorizontalOptions = LayoutOptions.StartAndExpand, };
            addMidiGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            addMidiGrid.Children.Add(new Label { Text = "Add Midi Pedal:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var addMidiButton = new Button() { Text = "+" };
            addMidiButton.Clicked += (sender, e) =>
            {
                AddMidiDevice(true, null);
            };
            addMidiGrid.Children.Add(addMidiButton, 1, 0);

            // Expression devices
            var addExpGrid = new Grid { HorizontalOptions = LayoutOptions.StartAndExpand, };
            addExpGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            addExpGrid.Children.Add(new Label { Text = "Add Expression Pedal:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var addExpButton = new Button() { Text = "+" };
            addExpButton.Clicked += (sender, e) =>
            {
                AddExpDevice(true, null);
            };
            addExpGrid.Children.Add(addExpButton, 1, 0);

            // Switch devices
            var addSwitchGrid = new Grid { HorizontalOptions = LayoutOptions.StartAndExpand, };
            addSwitchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            addSwitchGrid.Children.Add(new Label { Text = "Add Switch Pedal:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var addSwitchButton = new Button() { Text = "+" };
            addSwitchButton.Clicked += (sender, e) =>
            {
                AddSwitchDevice(true, null);
            };
            addSwitchGrid.Children.Add(addSwitchButton, 1, 0);

            // Save button
            var saveButton = new Button { Text = "Save", VerticalOptions = LayoutOptions.End };
            saveButton.Clicked += async (sender, e) =>
            {
                saveButton.BorderColor = Color.Blue;
                var answer = await DisplayAlert("Are you sure?", "Making changes will remove some preset information", "Yes", "No");
                if(answer == true)
                    SaveProfileChanges();
            };

            loopTableSection = new TableSection("Pedalboard: Loops")
            {
                new ViewCell
                {
                    View = loopGrid
                }
            };
            midiTableSection = new TableSection("Pedalboard: Midi")
            {
                new ViewCell
                {
                    View = addMidiGrid
                }
            };
            expTableSection = new TableSection("Pedalboard: Expression")
            {
                new ViewCell
                {
                    View = addExpGrid
                }
            };
            switchTableSection = new TableSection("Pedalboard: Switches")
            {
                new ViewCell
                {
                    View = addSwitchGrid
                }
            };

            TableView tableView = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot("TableView Title")
                {
                    loopTableSection,
                    midiTableSection,
                    expTableSection,
                    switchTableSection
                }
            };

            Title = "Detail";
            Content = new StackLayout
            {
                Children = {
                    tableView,
                    saveButton
                }
            };

            ConfigurePage();
        }

        public void ConfigurePage()
        {
            // Loop stepper
            loopStepper.Value = App.mainProfile.NumberOfLoops;
            loopNumberLabel.Text = App.mainProfile.NumberOfLoops.ToString();

            // Midi devices
            foreach(var device in App.mainProfile.MidiDevices)
            {
                AddMidiDevice(false, device.Name);
            }

            // Expression devices
            foreach(var device in App.mainProfile.ExpressionDevices)
            {
                AddExpDevice(false, device.Name);
            }

            // Switch devices
            foreach(var device in App.mainProfile.SwitchDevices)
            {
                AddSwitchDevice(false, device.Name);
            }
        }

        public void SaveProfileChanges()
        {
            // ============================== Loops ==============================
            App.mainProfile.NumberOfLoops = (int)loopStepper.Value;

            // ============================== MIDI ==============================
            // save oiginals
            originalMidiDevices = App.mainProfile.MidiDevices.ToList();
            // update new
            App.mainProfile.MidiDevices = tempMidiDevices.ToList();
            // Find difference between old and new
            var resultMidi = FindChanges(originalMidiDevices, App.mainProfile.MidiDevices);

            // ============================== EXP ==============================
            // save oiginals
            originalExpDevices = App.mainProfile.ExpressionDevices.ToList();
            // update new
            App.mainProfile.ExpressionDevices = tempExpDevices.ToList();
            // Find difference between old and new
            var resultExp = FindChanges(originalExpDevices, App.mainProfile.ExpressionDevices);

            // ============================== SWITCH ==============================
            // save oiginals
            originalSwitchDevices = App.mainProfile.SwitchDevices.ToList();
            // update new
            App.mainProfile.SwitchDevices = tempSwitchDevices.ToList();
            // Find difference between old and new
            var resultSwitch = FindChanges(originalSwitchDevices, App.mainProfile.SwitchDevices);

            foreach(var preset in App.Presets)
            {
                // loops
                preset.Loops.Resize(App.mainProfile.NumberOfLoops, false);

                // midi added
                if(resultMidi.Item1.Count > 0)
                {
                    foreach(var device in resultMidi.Item1)
                        preset.MidiDevices.Add(Activator.CreateInstance(device) as MidiDevice);
                }
                // midi removed
                if(resultMidi.Item2.Count > 0)
                {
                    foreach(var device in resultMidi.Item2)
                    {
                        foreach(var tuple in removedMidiTuples)
                        {
                            if(device.Equals(tuple.Item2))
                            {
                                var removeIndex = tuple.Item1;
                                preset.MidiDevices.RemoveAt(removeIndex);
                            }
                        }
                        removedMidiTuples.Clear();
                    }
                }

                // Expressions added
                if(resultExp.Item1.Count > 0)
                {
                    foreach(var device in resultExp.Item1)
                        preset.ExpressionDevices.Add(Activator.CreateInstance(device) as ExpressionDevice);
                }
                // Expressions removed
                if(resultExp.Item2.Count > 0)
                {
                    foreach(var device in resultExp.Item2)
                    {
                        foreach(var tuple in removedExpTuples)
                        {
                            if(device.Equals(tuple.Item2))
                            {
                                var removeIndex = tuple.Item1;
                                preset.ExpressionDevices.RemoveAt(removeIndex);
                            }
                        }
                        removedExpTuples.Clear();
                    }
                }

                // Switches added
                if(resultSwitch.Item1.Count > 0)
                {
                    foreach(var device in resultSwitch.Item1)
                        preset.SwitchDevices.Add(Activator.CreateInstance(device) as SwitchDevice);
                }
                // Switches removed
                if(resultSwitch.Item2.Count > 0)
                {
                    foreach(var device in resultSwitch.Item2)
                    {
                        foreach(var tuple in removedSwitchTuples)
                        {
                            if(device.Equals(tuple.Item2))
                            {
                                var removeIndex = tuple.Item1;
                                preset.SwitchDevices.RemoveAt(removeIndex);
                            }
                        }
                        removedSwitchTuples.Clear();
                    }
                }

            }
        }

        public Tuple<List<Type>, List<Type>> FindChanges(List<Type> original, List<Type> modified)
        {
            // get difference between modified and original
            // Added
            var lookup1 = original.ToLookup(str => str);
            var addedList = (from str in modified
                             group str by str into strGroup
                             let missingCount
                                  = Math.Max(0, strGroup.Count() - lookup1[strGroup.Key].Count())
                             from missingStr in strGroup.Take(missingCount)
                             select missingStr).ToList();
            // Removed
            var lookup2 = modified.ToLookup(str => str);
            var removedList = (from str in original
                               group str by str into strGroup
                               let missingCount
                                    = Math.Max(0, strGroup.Count() - lookup2[strGroup.Key].Count())
                               from missingStr in strGroup.Take(missingCount)
                               select missingStr).ToList();

            return Tuple.Create(addedList, removedList);
        }

        //public static readonly int numRowsAbove = 1;
        public void AddMidiDevice(bool newGrid, string selectedDevice)
        {
            var midiGrid = new Grid { HorizontalOptions = LayoutOptions.StartAndExpand, };
            midiGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            midiGrid.Children.Add(new Label { Text = "Choose Midi Pedal:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var picker = new Picker();
            foreach(var device in Extensions.GetInstances<MidiDevice>())
            {
                picker.Items.Add(device.Name);
            }

            if(newGrid) // select first if new, else select predefined selected device
                picker.SelectedIndex = 0;            
            else
                picker.SelectedIndex = picker.Items.IndexOf(selectedDevice);

            tempMidiDevices.Add(Type.GetType("PresetPedalForms." + picker.Items[picker.SelectedIndex] + ", PresetPedalForms"));

            picker.SelectedIndexChanged += (sender2, e2) =>
            {
                var tempIndex = midiTableSection.IndexOf(midiGrid.Parent as ViewCell);
                tempMidiDevices[tempIndex] = Type.GetType("PresetPedalForms."+picker.Items[picker.SelectedIndex]+", PresetPedalForms");
            };

            midiGrid.Children.Add(picker, 1, 0);
            var deleteButton = new Button { Text = "Delete" };
            deleteButton.Clicked += (sender1, e1) =>
            {
                var removeIndex = midiTableSection.IndexOf(midiGrid.Parent as ViewCell);
                removedMidiTuples.Add(new Tuple<int, Type>(removeIndex, tempMidiDevices[removeIndex]));
                tempMidiDevices.RemoveAt(removeIndex);
                midiTableSection.Remove(((Button)sender1).Parent.Parent as ViewCell);
            };
            midiGrid.Children.Add(deleteButton, 2, 0);
            midiTableSection.Insert(midiTableSection.Count - 1, new ViewCell { View = midiGrid });
        }

        //public static readonly int numRowsAboveMidi = 1;
        public void AddExpDevice(bool newGrid, string selectedDevice)
        {
            var expGrid = new Grid { HorizontalOptions = LayoutOptions.StartAndExpand, };
            expGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            expGrid.Children.Add(new Label { Text = "Choose Expression Pedal:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var picker = new Picker();
            foreach(var device in Extensions.GetInstances<ExpressionDevice>())
            {
                picker.Items.Add(device.Name);
            }

            if(newGrid) // select first if new, else select predefined selected device
                picker.SelectedIndex = 0;
            else
                picker.SelectedIndex = picker.Items.IndexOf(selectedDevice);

            tempExpDevices.Add(Type.GetType("PresetPedalForms." + picker.Items[picker.SelectedIndex] + ", PresetPedalForms"));

            picker.SelectedIndexChanged += (sender2, e2) =>
            {
                var tempIndex = expTableSection.IndexOf(expGrid.Parent as ViewCell);
                tempExpDevices[tempIndex] = Type.GetType("PresetPedalForms." + picker.Items[picker.SelectedIndex] + ", PresetPedalForms");
            };

            expGrid.Children.Add(picker, 1, 0);
            var deleteButton = new Button { Text = "Delete" };
            deleteButton.Clicked += (sender1, e1) =>
            {
                var removeIndex = expTableSection.IndexOf(expGrid.Parent as ViewCell);
                removedExpTuples.Add(new Tuple<int, Type>(removeIndex, tempExpDevices[removeIndex]));
                tempExpDevices.RemoveAt(removeIndex);
                expTableSection.Remove(((Button)sender1).Parent.Parent as ViewCell);
            };
            expGrid.Children.Add(deleteButton, 2, 0);
            expTableSection.Insert(expTableSection.Count - 1, new ViewCell { View = expGrid });
        }

        public void AddSwitchDevice(bool newGrid, string selectedDevice)
        {
            var switchGrid = new Grid { HorizontalOptions = LayoutOptions.StartAndExpand, };
            switchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            switchGrid.Children.Add(new Label { Text = "Choose Switch Pedal:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var picker = new Picker();
            foreach(var device in Extensions.GetInstances<SwitchDevice>())
            {
                picker.Items.Add(device.Name);
            }

            if(newGrid) // select first if new, else select predefined selected device
                picker.SelectedIndex = 0;
            else
                picker.SelectedIndex = picker.Items.IndexOf(selectedDevice);

            tempSwitchDevices.Add(Type.GetType("PresetPedalForms." + picker.Items[picker.SelectedIndex] + ", PresetPedalForms"));

            picker.SelectedIndexChanged += (sender2, e2) =>
            {
                var tempIndex = switchTableSection.IndexOf(switchGrid.Parent as ViewCell);
                tempSwitchDevices[tempIndex] = Type.GetType("PresetPedalForms." + picker.Items[picker.SelectedIndex] + ", PresetPedalForms");
            };

            switchGrid.Children.Add(picker, 1, 0);
            var deleteButton = new Button { Text = "Delete" };
            deleteButton.Clicked += (sender1, e1) =>
            {
                var removeIndex = switchTableSection.IndexOf(switchGrid.Parent as ViewCell);
                removedSwitchTuples.Add(new Tuple<int, Type>(removeIndex, tempSwitchDevices[removeIndex]));
                tempSwitchDevices.RemoveAt(removeIndex);
                switchTableSection.Remove(((Button)sender1).Parent.Parent as ViewCell);
            };
            switchGrid.Children.Add(deleteButton, 2, 0);
            switchTableSection.Insert(switchTableSection.Count - 1, new ViewCell { View = switchGrid });
        }

    }
}

