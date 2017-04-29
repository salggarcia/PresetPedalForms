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
        public List<LoopDevice> tempLoopDevices = new List<LoopDevice>();
        public List<Type> tempMidiDevices = new List<Type>();
        public List<Type> tempExpDevices = new List<Type>();
        public List<Type> tempSwitchDevices = new List<Type>();

        public List<LoopDevice> originalLoopDevices = new List<LoopDevice>();
        public List<LoopDevice> removedLoops = new List<LoopDevice>();
        public List<LoopDevice> addedLoops = new List<LoopDevice>();

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
        //Stepper loopStepper;
        //Label loopNumberLabel;

        public ProfilePage()
        {
            
            // Loops
            //loopNumberLabel = new Label { VerticalOptions = LayoutOptions.Center };
            //loopStepper = new Stepper { VerticalOptions = LayoutOptions.Center };
            //loopStepper.ValueChanged += (sender, e) => {
            //    loopNumberLabel.Text = loopStepper.Value.ToString();
            //};
            //var loopGrid = new Grid { HorizontalOptions = LayoutOptions.Fill, Padding = new Thickness(15, 0, 15, 0) };
            //loopGrid.Children.Add(new Label { Text = "Number of Loops:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            //loopGrid.Children.Add(loopStepper, 1, 0);
            //loopGrid.Children.Add(loopNumberLabel, 2, 0);
            //loopGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.5, GridUnitType.Star) });
            //loopGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //loopGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            var addLoopGrid = new Grid { HorizontalOptions = LayoutOptions.Start, Padding = new Thickness(15, 0, 15, 0) };
            addLoopGrid.Children.Add(new Label { Text = "Add Loop:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var addLoopButton = new Button() { Text = "+" };
            addLoopButton.Clicked += (sender, e) =>
            {
				AddLoopDevice(true, null);
            };
            addLoopGrid.Children.Add(addLoopButton, 1, 0);
            addLoopGrid.BackgroundColor = Color.FromRgb(240, 240, 240);
            addLoopGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            addLoopGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            // Midi devices
            var addMidiGrid = new Grid { HorizontalOptions = LayoutOptions.Start, Padding = new Thickness(15, 0, 15, 0) };
            addMidiGrid.Children.Add(new Label { Text = "Add Midi Pedal:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var addMidiButton = new Button() { Text = "+" };
            addMidiButton.Clicked += (sender, e) =>
            {
                AddMidiDevice(true, null);
            };
            addMidiGrid.Children.Add(addMidiButton, 1, 0);
            addMidiGrid.BackgroundColor = Color.FromRgb(240, 240, 240);
            addMidiGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            addMidiGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
    
            // Expression devices
            var addExpGrid = new Grid { HorizontalOptions = LayoutOptions.Start, Padding = new Thickness(15, 0, 15, 0) };
            addExpGrid.Children.Add(new Label { Text = "Add Expression Pedal:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var addExpButton = new Button() { Text = "+" };
            addExpButton.Clicked += (sender, e) =>
            {
                AddExpDevice(true, null);
            };
            addExpGrid.Children.Add(addExpButton, 1, 0);
            addExpGrid.BackgroundColor = Color.FromRgb(240, 240, 240);
            addExpGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            addExpGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            // Switch devices
            var addSwitchGrid = new Grid { HorizontalOptions = LayoutOptions.Start, Padding = new Thickness(15, 0, 15, 0) };
            addSwitchGrid.Children.Add(new Label { Text = "Add Switch Pedal:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var addSwitchButton = new Button() { Text = "+" };
            addSwitchButton.Clicked += (sender, e) =>
            {
                AddSwitchDevice(true, null);
            };
            addSwitchGrid.Children.Add(addSwitchButton, 1, 0);
            addSwitchGrid.BackgroundColor = Color.FromRgb(240, 240, 240);
            addSwitchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            addSwitchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

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
                    View = addLoopGrid
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

            Title = "Profile";
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
            //loopStepper.Value = App.mainProfile.NumberOfLoops;
            //loopNumberLabel.Text = App.mainProfile.NumberOfLoops.ToString();
            // Loop devices
            foreach(var device in App.mainProfile.LoopDevices)
            {
				AddLoopDevice(false, device);
            }

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
            //App.mainProfile.NumberOfLoops = (int)loopStepper.Value;
            // save oiginals
            originalLoopDevices = App.mainProfile.LoopDevices.ToList();
            // update new
            App.mainProfile.LoopDevices = tempLoopDevices.ToList();
            // Find difference between old and new
             //= new Tuple<List<LoopDevice>, List<LoopDevice>>(new List<LoopDevice>(), new List<LoopDevice>());
            //if(originalLoopDevices.Count != App.mainProfile.LoopDevices.Count)
            //{
            //var resultLoop = FindLoopChanges(originalLoopDevices, App.mainProfile.LoopDevices);
            //}

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
                //added
                foreach(var device in addedLoops)
                {
                    preset.LoopDevices.Add(device);
                }
                //removed
                foreach(var device in removedLoops)
                {
                    preset.LoopDevices.RemoveAt((int)device.ID);
                }
                // loop name changes
                foreach(var device in preset.LoopDevices)
                {
                    int index = preset.LoopDevices.IndexOf(device);
                    if(device.Name != App.mainProfile.LoopDevices[index].Name)
                    {
                        device.Name = App.mainProfile.LoopDevices[index].Name;
                    }
                } 
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

            App.SaveData();
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

        public Tuple<List<LoopDevice>, List<LoopDevice>> FindLoopChanges(List<LoopDevice> original, List<LoopDevice> modified)
        {
            // get difference between modified and original
            // Added
            //var lookup1 = original.ToLookup(str => str);
            //var addedList = (from str in modified
            //				 group str by str into strGroup
            //				 let missingCount
            //					  = Math.Max(0, strGroup.Count() - lookup1[strGroup.Key].Count())
            //				 from missingStr in strGroup.Take(missingCount)
            //				 select missingStr).ToList();
            //// Removed
            //var lookup2 = modified.ToLookup(str => str);
            //var removedList = (from str in original
            //				   group str by str into strGroup
            //				   let missingCount
            //						= Math.Max(0, strGroup.Count() - lookup2[strGroup.Key].Count())
            //				   from missingStr in strGroup.Take(missingCount)
            //				   select missingStr).ToList();

            var addedList = modified.Except(original, new IdComparer()).ToList();
            var removedList = original.Except(modified, new IdComparer()).ToList();

        	return Tuple.Create(addedList, removedList);
        }

        public class IdComparer : IEqualityComparer<LoopDevice>
        {
        	public int GetHashCode(LoopDevice co)
        	{
        		if(co == null)
        		{
        			return 0;
        		}
        		return co.ID.GetHashCode();
        	}

        	public bool Equals(LoopDevice x1, LoopDevice x2)
        	{
        		if(object.ReferenceEquals(x1, x2))
        		{
        			return true;
        		}
        		if(object.ReferenceEquals(x1, null) ||
        			object.ReferenceEquals(x2, null))
        		{
        			return false;
        		}
        		return x1.ID == x2.ID;
            }
        }

        public void AddLoopDevice(bool newGrid, LoopDevice selectedDevice)
        {
            var loopGrid = new Grid { HorizontalOptions = LayoutOptions.Fill, Padding = new Thickness(15, 0, 15, 0) };
        	loopGrid.Children.Add(new Label { Text = "Name for Loop:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            //var picker = new Picker { VerticalOptions = LayoutOptions.Center };
            //foreach(var device in Extensions.GetInstances<MidiDevice>())
            //{
            //	picker.Items.Add(device.Name);
            //}

            if(newGrid) // select first if new, else select predefined selected device
            {
                var loopToAdd = new LoopDevice { ID = tempLoopDevices.Count, Name = "", OnOff = false };
                tempLoopDevices.Add(loopToAdd);
                addedLoops.Add(loopToAdd);
            }
            else
            	tempLoopDevices.Add(new LoopDevice { ID = selectedDevice.ID, Name = selectedDevice.Name, OnOff = selectedDevice.OnOff });

            //picker.SelectedIndexChanged += (sender2, e2) =>
            //{
            //	var tempIndex = midiTableSection.IndexOf(loopGrid.Parent as ViewCell);
            //	tempMidiDevices[tempIndex] = Type.GetType("PresetPedalForms." + picker.Items[picker.SelectedIndex] + ", PresetPedalForms");
            //};
            //loopGrid.Children.Add(picker, 1, 0);
            var entry = new Entry { VerticalOptions = LayoutOptions.Center };
            if(newGrid)
                entry.Text = "";
            else
                entry.Text = selectedDevice.Name;
            entry.Keyboard = Keyboard.Text;
            entry.TextChanged += (sender2, e) => 
            {
                var tempIndex = loopTableSection.IndexOf(loopGrid.Parent as ViewCell);
                tempLoopDevices[tempIndex].Name = e.NewTextValue;
            };
            loopGrid.Children.Add(entry, 1, 0);
        	var deleteButton = new Button { Text = "Delete" };
        	deleteButton.Clicked += (sender1, e1) =>
        	{
        		var removeIndex = loopTableSection.IndexOf(loopGrid.Parent as ViewCell);
        		removedLoops.Add(tempLoopDevices[removeIndex]);
        		tempLoopDevices.RemoveAt(removeIndex);
                foreach(var item in tempLoopDevices)
                {
                    var index = tempLoopDevices.IndexOf(item);
                    item.ID = index;
                }
        		loopTableSection.Remove(((Button)sender1).Parent.Parent as ViewCell);
        	};
        	loopGrid.Children.Add(deleteButton, 2, 0);
        	loopGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
        	loopGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        	loopGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
        	loopTableSection.Insert(loopTableSection.Count - 1, new ViewCell { View = loopGrid } );         } 

        //public static readonly int numRowsAbove = 1;
        public void AddMidiDevice(bool newGrid, string selectedDevice)
        {
            var midiGrid = new Grid { HorizontalOptions = LayoutOptions.Fill, Padding = new Thickness(15,0,15,0) };
            midiGrid.Children.Add(new Label { Text = "Choose Midi Pedal:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var picker = new Picker { VerticalOptions = LayoutOptions.Center };
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
            midiGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            midiGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            midiGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            midiTableSection.Insert(midiTableSection.Count - 1, new ViewCell { View = midiGrid });
        }

        //public static readonly int numRowsAboveMidi = 1;
        public void AddExpDevice(bool newGrid, string selectedDevice)
        {
            var expGrid = new Grid { HorizontalOptions = LayoutOptions.Fill, Padding = new Thickness(15, 0, 15, 0) };
            expGrid.Children.Add(new Label { Text = "Choose Expression Pedal:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var picker = new Picker { VerticalOptions = LayoutOptions.Center };
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
            expGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            expGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            expGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            expTableSection.Insert(expTableSection.Count - 1, new ViewCell { View = expGrid });
        }

        public void AddSwitchDevice(bool newGrid, string selectedDevice)
        {
            var switchGrid = new Grid { HorizontalOptions = LayoutOptions.Fill, Padding = new Thickness(15, 0, 15, 0) };
            switchGrid.Children.Add(new Label { Text = "Choose Switch Pedal:", VerticalOptions = LayoutOptions.Center }, 0, 0);
            var picker = new Picker { VerticalOptions = LayoutOptions.Center };
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
            switchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            switchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            switchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            switchTableSection.Insert(switchTableSection.Count - 1, new ViewCell { View = switchGrid });
        }

    }
}

