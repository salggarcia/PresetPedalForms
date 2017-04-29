using System;
using System.Diagnostics;
using System.Threading;
using PresetPedalForms.Models;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PresetPedalForms
{
    public class LivePage : ContentPage
    {
        ListView presetList;
        ListView songList;
        StackLayout outerStack;
        Label songsLabel;
        Label presetsLabel;
        public LivePage()
        {
            //this.BackgroundColor = Color.Black;
            clickTimer = new Timer(HandleClickedElapsedEventHandler, null, Timeout.Infinite, Timeout.Infinite);
            holdTimer = new Timer(HandleHoldElapsedEventHandler, null, Timeout.Infinite, Timeout.Infinite);

            songList = new ListView() {
                HasUnevenRows = true
            };
            songList.BackgroundColor = Color.Black;
            songList.ItemsSource = App.Songs;
            songList.ItemTemplate = new DataTemplate(typeof(songViewCell));
            songList.ItemSelected += (sender, e) => {
                var item = e.SelectedItem as Song;
                Device.BeginInvokeOnMainThread(() =>
                {
                    presetList.ItemsSource = item.Presets;
                });

            };
            songList.SelectedItem = App.Songs[0];
            songList.RowHeight = 40;

            presetList = new ListView() {
            	HasUnevenRows = true
            };
            presetList.BackgroundColor = Color.Black;
            presetList.ItemsSource = App.Songs[0].Presets;
            presetList.ItemTemplate = new DataTemplate(typeof(presetViewCell));
            presetList.ItemSelected += (sender, e) => {
                var preset = e.SelectedItem as Preset;
                if(App.globalModes.LiveMode)
                    App.BLE.SendPreset(preset);
            };
            presetList.RowHeight = 40;

            Title = "Detail";
            songsLabel = new Label { Text = "Songs:", VerticalOptions = LayoutOptions.FillAndExpand };
            presetsLabel = new Label { Text = "Presets:", HorizontalOptions = LayoutOptions.FillAndExpand };                                                                         
            outerStack = new StackLayout
            {
                Children = {
                    //songsLabel,
                    songList,
                    //presetsLabel,
                    presetList
                }
            };
            Content = outerStack;

            App.BLE.ControlUpdatedEvent += BLE_ControlUpdatedEvent;
            App.controlReady = true;


     //       App.BLE.ControlUpdatedEvent += (sender, e) => 
     //       {
     //           Debug.WriteLine("Control event on: " + e.Val);
     //           int val = e.Val;
     //           if (val == 127)
     //           {

					//ButtonOnAction();
     //           }
     //           else if(val == 0)
     //           {
					//ButtonOffAction();
     //           }
     //           else if (val > 0) // scroll up
     //           {

					//ScrollUpAction();
     //           }
     //           else if (val< 0) // scroll down
     //           {

					//ScrollDownAction();
     //           }
     //       };
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            App.BLE.ControlUpdatedEvent -= BLE_ControlUpdatedEvent;
        }

        void BLE_ControlUpdatedEvent(object sender, PresetPedalForms.BLEController.ControlUpdatedEventArgs e)
        {
            //Debug.WriteLine("Control event on: " + e.Val);
            int val = e.Val;
            if (val == 127)
            {
				ButtonOnAction();
            }
            else if(val == 0)
            {

				ButtonOffAction();
            }
        }

        public int currentSelectedRow;
        public bool waitingForRelease = false;

        Timer clickTimer;
        Timer holdTimer;
        int count = 0;
        int doubeTime = 300;
        int holdTime = 1000;
        public void HandleClickedElapsedEventHandler(object state)
        {
            if(count == 1)
            {
                SingleButtonClick();
            }
            else if(count == 2)
            {
                DoubleButtonClick();
            }
            count = 0;
            clickTimer.Change(Timeout.Infinite, Timeout.Infinite); // stop timer
        }

        public void HandleHoldElapsedEventHandler(object state)
        {
            HoldButton();
            holdTimer.Change(Timeout.Infinite, Timeout.Infinite); // stop hold timer
            waitingForRelease = true;
        }

        public void ButtonOnAction()
        {
            //heldDown = true;
            holdTimer.Change(holdTime, Timeout.Infinite); // start hold timer
        }

        public void ButtonOffAction()
        {
            //heldDown = false;
            if(waitingForRelease)
            {
                waitingForRelease = false;
            }
            else
            {
                holdTimer.Change(Timeout.Infinite, Timeout.Infinite); // stop timer
                if(count == 0)
                {
                    clickTimer.Change(doubeTime, Timeout.Infinite);
                    count++;
                }
                else if(count == 1)
                {
                    clickTimer.Change(doubeTime, Timeout.Infinite);
                    count++;
                }
                else
                {
                    TripleButtonClick();
                    count = 0;
                    clickTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }
        }

        //public bool heldDown = false;

        public void SingleButtonClick()
        {
            Debug.WriteLine("Single click");
            NextPreset();
        }
        public void DoubleButtonClick()
        {
            Debug.WriteLine("Double click");
            PreviousPreset();
        }
        public void TripleButtonClick()
        {
            Debug.WriteLine("Triple click");
            PreviousSong();
        }
        public void HoldButton()
        {
            Debug.WriteLine("Hold button");
            NextSong();
        }

        public void NextPreset()
        {
            if (App.globalModes.LiveMode)             {                 var currentPresets = presetList.ItemsSource as ObservableCollection<Preset>;                 var selectedPreset = presetList.SelectedItem as Preset;                 var index = currentPresets.IndexOf(selectedPreset);                 int row;                 if(selectedPreset == null)                     row = 0;                 else                     row = index + 1;                  if(row < currentPresets.Count)                 {                     Device.BeginInvokeOnMainThread(() =>                     {                                             presetList.SelectedItem = currentPresets[row];                         currentSelectedRow = row;                     });                 }             } 
        }

        public void PreviousPreset()
        {
            if (App.globalModes.LiveMode)             {                 var currentPresets = presetList.ItemsSource as ObservableCollection<Preset>;                 var selectedPreset = presetList.SelectedItem as Preset;                 var row = currentPresets.IndexOf(selectedPreset) - 1;                  if(row >= 0)                 {                     Device.BeginInvokeOnMainThread(() =>                     {                                             presetList.SelectedItem = currentPresets[row];                         currentSelectedRow = row;                     });                 }             } 
        }

        public void NextSong()
        {
            if (App.globalModes.LiveMode)             {                 var selectedSong = songList.SelectedItem as Song;                 var row = App.Songs.IndexOf(selectedSong) + 1;                 //Debug.WriteLine("Button: " + row);                 if (row < App.Songs.Count)                 {                     Device.BeginInvokeOnMainThread(() =>                     {                         songList.SelectedItem = App.Songs[row];                     });                 }             }
        }

        public void PreviousSong()
        {
            if (App.globalModes.LiveMode)             {                 var selectedSong = songList.SelectedItem as Song;                 var row = App.Songs.IndexOf(selectedSong) - 1;                 //Debug.WriteLine("Button: " + row);                 if(row >= 0)                 {                     Device.BeginInvokeOnMainThread(() =>                     {                         songList.SelectedItem = App.Songs[row];                     });                 }             }
        }

     //   public int currentSelectedRow;      //   public bool waitingForRelease = false;      //   public void ScrollUpAction()      //   {      //       if (App.globalModes.LiveMode)      //       {
     //           var currentPresets = presetList.ItemsSource as ObservableCollection<Preset>;
     //           var selectedPreset = presetList.SelectedItem as Preset;
     //           var index = currentPresets.IndexOf(selectedPreset);      //           int row;
     //           if(selectedPreset == null)
     //               row = 0;
     //           else
     //               row = index + 1;      //           //Debug.WriteLine("scroll up: " + row);
      //           if(row < currentPresets.Count)      //           {      //               Device.BeginInvokeOnMainThread(() =>
     //               {                    
     //                   presetList.SelectedItem = currentPresets[row];
     //                   currentSelectedRow = row;
     //               });      //               //if (!heldDown)      //               //    presetPicker.Delegate.Selected(presetPicker, row, 0);      //               //else      //               //    waitingForRelease = true;      //           }      //       }      //   }       //   public void ScrollDownAction()      //   {      //       if (App.globalModes.LiveMode)      //       {
     //           var currentPresets = presetList.ItemsSource as ObservableCollection<Preset>;
     //           var selectedPreset = presetList.SelectedItem as Preset;
     //           var row = currentPresets.IndexOf(selectedPreset) - 1;
      //           //Debug.WriteLine("scroll down: " + row);      //           if(row >= 0)      //           {
     //               Device.BeginInvokeOnMainThread(() =>
     //               {                    
     //                   presetList.SelectedItem = currentPresets[row];
     //                   currentSelectedRow = row;
     //               });      //               //if (!heldDown)      //               //    presetPicker.Delegate.Selected(presetPicker, row, 0);      //               //else      //               //    waitingForRelease = true;      //           }      //       }      //   }       //   Timer sw;      //   int count = 0;       //   public void HandleElapsedEventHandler(object state)      //   {      //       Debug.WriteLine("Single click");      //       SingleButtonClick();      //       count = 0;      //       sw.Change(Timeout.Infinite, Timeout.Infinite);      //       //sw.Elapsed -= HandleElapsedEventHandler;      //   }       //   public void ButtonOnAction()      //   {      //       heldDown = true;      //   }       //   public void ButtonOffAction()      //   {      //       heldDown = false;      //       //if(waitingForRelease)      //       //{      //       //    presetPicker.Delegate.Selected(presetPicker, currentSelectedRow, 0);      //       //    waitingForRelease = false;      //       //}      //       //else      //       //{      //           if (count == 0)      //           {
     //               sw.Change(350,Timeout.Infinite);      //               count++;      //               //sw.Elapsed += HandleElapsedEventHandler;      //           }      //           else      //           {      //               Debug.WriteLine("Double click");      //               DoubleButtonClick();      //               count = 0;      //               sw.Change(Timeout.Infinite, Timeout.Infinite);      //               //sw.Elapsed -= HandleElapsedEventHandler;      //           }      //       //}        //   }       //   public bool heldDown = false;       //   public void SingleButtonClick()      //   {      //       if (App.globalModes.LiveMode)      //       {      //           //songPicker.InvokeOnMainThread(() =>      //           //{
     //           var selectedSong = songList.SelectedItem as Song;
     //           var row = App.Songs.IndexOf(selectedSong) + 1;      //           Debug.WriteLine("Button: " + row);      //           if (row < App.Songs.Count)      //           {
     //               Device.BeginInvokeOnMainThread(() =>
     //               {
     //                   songList.SelectedItem = App.Songs[row];
     //                   //presetList.SelectedItem = (presetList.ItemsSource as ObservableCollection<Preset>)[0];
     //               });      //           }      //           //} );      //       }      //   }      //   public void DoubleButtonClick()      //   {      //       if (App.globalModes.LiveMode)      //       {      //           var selectedSong = songList.SelectedItem as Song;
     //           var row = App.Songs.IndexOf(selectedSong) - 1;      //           Debug.WriteLine("Button: " + row);      //           if(row >= 0)      //           {      //               Device.BeginInvokeOnMainThread(() =>
     //               {
     //                   songList.SelectedItem = App.Songs[row];
     //                   //presetList.ItemsSource = selectedSong.Presets;
     //                   //presetList.SelectedItem = (presetList.ItemsSource as ObservableCollection<Preset>)[0];
     //               });      //           }      //       }      //   } 

        private double width = 0;
        private double height = 0;

        void Handle_SizeChanged(object sender, EventArgs e)
        {
            //if(width != this.width || height != this.height)
            //{
            //    this.width = width;
            //    this.height = height;
                //if(Width > Height)
                //{
                //    outerStack.Children.Remove(songsLabel);
                //    outerStack.Children.Remove(presetsLabel);
                //    outerStack.Orientation = StackOrientation.Horizontal;
                //}
                //else
                //{
                //    outerStack.Children.Add(songsLabel);
                //    outerStack.Children.Add(presetsLabel);
                //    outerStack.Orientation = StackOrientation.Vertical;
                //}
            //}
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            //if (width != this.width || height != this.height) 
            //{
            //	this.width = width;
            //	this.height = height;
            	if(width > height)
            	{
                    //outerStack.Children.Remove(songsLabel);
                    //outerStack.Children.Remove(presetsLabel);
            		outerStack.Orientation = StackOrientation.Horizontal;
            	}
            	else
            	{
                    //outerStack.Children.Add(songsLabel);
                    //outerStack.Children.Add(presetsLabel);
            		outerStack.Orientation = StackOrientation.Vertical;
                }
            //}
        }
    }

    public class songViewCell : ViewCell
    {
        public songViewCell()
        {
            StackLayout layout = new StackLayout();
            layout.Padding = new Thickness(5, 0);
            Label label = new Label { VerticalOptions = LayoutOptions.Center };

            label.SetBinding(Label.TextProperty, "Name");
            label.FontSize = 35;
            label.TextColor = Color.DarkCyan;
            layout.Children.Add(label);

            layout.VerticalOptions = LayoutOptions.Center;
            View = layout;
        }
    }

    public class presetViewCell : ViewCell
    {
    	public presetViewCell()
    	{
    		StackLayout layout = new StackLayout();
    		layout.Padding = new Thickness(5, 0);
    		Label label = new Label { VerticalOptions = LayoutOptions.Center };

    		label.SetBinding(Label.TextProperty, "Name");
    		label.FontSize = 25;
    		label.TextColor = Color.OrangeRed;
    		layout.Children.Add(label);

    		layout.VerticalOptions = LayoutOptions.Center;
    		View = layout;
        }
    }
}

