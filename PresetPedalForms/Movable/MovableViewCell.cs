using System;
using System.Windows.Input;
using PresetPedalForms.Models;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace PresetPedalForms
{
    /// <summary>
    /// ViewCell which can be reordered in ListView by drag and drop.
    /// There are 2 types of working with <see cref="MovableViewCell"/>:
    /// <list type="table">
    /// <item>
    /// <term>
    /// Internal reordering logics
    /// </term>
    /// <description>
    /// All already done for you.
    /// But for property <see cref="ListView.ItemsSource"/> you must set
    /// collection which implements <see cref="IObservableCollectionEx"/> (for example <see cref="ObservableCollectionEx"/>).
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// Custom reordering logics
    /// </term>
    /// <description>
    /// Necessary when your reordering logic is not trivial. 
    /// In which case you must set <see cref="CustomReorderCommaond"/> 
    /// and change source list manually.
    /// </description>
    /// </item>
    /// </list>
    /// </summary>
    public class MovableViewCell : ViewCell
    {
        public MovableViewCell(bool noActions = false)
        {
            StackLayout layout = new StackLayout();
            layout.Padding = new Thickness(15, 0);
            Label label = new Label { VerticalOptions = LayoutOptions.Center };

            label.SetBinding(Label.TextProperty, "Name");
            layout.Children.Add(label);

            var moreAction = new MenuItem { Text = "Copy" };
            moreAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            moreAction.Clicked += OnCopy;

            var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true }; // red background
            deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            deleteAction.Clicked += OnDelete;

            if(!noActions)
            {
                this.ContextActions.Add(moreAction);
                this.ContextActions.Add(deleteAction);
            }

            layout.VerticalOptions = LayoutOptions.Center;
            View = layout;
        }

        void OnCopy(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            if(item.CommandParameter.GetType().Equals(typeof(Song)))
            {
                var song = item.CommandParameter as Song;
                CopySong(song);
            }
            else if(item.CommandParameter.GetType().Equals(typeof(Preset)))
            {
                var preset = item.CommandParameter as Preset;
                CopyPreset(preset);
            }
        }

        public void CopyPreset(Preset preset)
        {
            var presetIdx = App.Presets.IndexOf(preset);
            //var temploops = new List<LoopDevice>();
            //foreach(var item in preset.LoopDevices)
            //{
            //    temploops.Add(item);
            //}
            //var tempmidi = new List<MidiDevice>();
            //foreach(var item in preset.MidiDevices)
            //{
            //    tempmidi.Add(item);
            //}
            //var tempExpr = new List<ExpressionDevice>();
            //foreach(var item in preset.ExpressionDevices)
            //{
            //    tempExpr.Add(item);
            //}
            //var tempswitch = new List<SwitchDevice>();
            //foreach(var item in preset.SwitchDevices)
            //{
            //    tempswitch.Add(item);
            //}
            var newPreset = new Preset()
            {
                Name = preset.Name + " Copy",
                LoopDevices = preset.LoopDevices.Select(a => a.Copy()).ToList(),
                MidiDevices = preset.MidiDevices.Select(a => a.Copy()).ToList(),
                ExpressionDevices = preset.ExpressionDevices.Select(a => a.Copy()).ToList(),
                SwitchDevices = preset.SwitchDevices.Select(a => a.Copy()).ToList()
            };
            App.Presets.Insert(presetIdx + 1, newPreset);

            //SavePresetFile();
            App.SaveData();
        }

        public void CopySong(Song song)
        {
        	var songIdx = App.Songs.IndexOf(song);
        	var newSong = new Song()
        	{
        		Name = song.Name,
        		Presets = song.Presets
        	};
        	App.Songs.Insert(songIdx + 1, newSong);

        	//SavePresetFile();
        	App.SaveData();
        }

        void OnDelete(object sender, EventArgs e)
        {
        	var item = (MenuItem)sender;
            if(item.CommandParameter.GetType().Equals(typeof(Song)))
            {
                var song = item.CommandParameter as Song;
                App.Songs.Remove(song);
            }
            else if(item.CommandParameter.GetType().Equals(typeof(Preset)))
            {
                var preset = item.CommandParameter as Preset;
                App.Presets.Remove(preset);
            }

        	App.SaveData();
        }

        public static BindableProperty CustomReorderCommaondProperty = BindableProperty.Create("CustomReorderCommaond", typeof(Command<ReorderCommandParam>), typeof(MovableViewCell));
        public static BindableProperty BeginReorderCommandProperty = BindableProperty.Create("BeginReorderCommand", typeof(Command<ReorderCommandParam>), typeof(MovableViewCell));
        public static BindableProperty EndReorderCommandProperty = BindableProperty.Create("EndReorderCommand", typeof(ICommand), typeof(MovableViewCell));

        /// <summary>
        /// Command for custom reordering.
        /// Called when touch point within a new row.
        /// </summary>
        /// <remarks>
        /// If not null, all internal reordering logics will be ignored.
        /// </remarks>
        public Command<ReorderCommandParam> CustomReorderCommaond
        {
            get { return (Command<ReorderCommandParam>)GetValue(CustomReorderCommaondProperty); }
            set { SetValue(CustomReorderCommaondProperty, value); }
        }

        public Command<ReorderCommandParam> BeginReorderCommand
        {
            get { return (Command<ReorderCommandParam>)GetValue(BeginReorderCommandProperty); }
            set { SetValue(BeginReorderCommandProperty, value); }
        }

        public ICommand EndReorderCommand
        {
            get { return (ICommand)GetValue(EndReorderCommandProperty); }
            set { SetValue(EndReorderCommandProperty, value); }
        }
    }
}
