using System;

using Xamarin.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace PresetPedalForms
{
    public class SelectMultipleBasePage<T> : ContentPage
    {
        public class WrappedSelection<T> : INotifyPropertyChanged
        {
            public T Item { get; set; }
            bool isSelected = false;
            public bool IsSelected
            {
                get
                {
                    return isSelected;
                }
                set
                {
                    if (isSelected != value)
                    {
                        isSelected = value;
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                        //                      PropertyChanged (this, new PropertyChangedEventArgs (nameof (IsSelected))); // C# 6
                    }
                }
            }
            public event PropertyChangedEventHandler PropertyChanged = delegate { };
        }
        public class WrappedItemSelectionTemplate : ViewCell
        {
            public WrappedItemSelectionTemplate() : base()
            {
                Label name = new Label { VerticalOptions = LayoutOptions.Center };
                name.SetBinding(Label.TextProperty, new Binding("Item.preset.Name"));
                Switch mainSwitch = new Switch { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.EndAndExpand, Scale = 0.75 };
                mainSwitch.SetBinding(Switch.IsToggledProperty, new Binding("IsSelected"));

                var stack = new StackLayout
                {
                    Padding = new Thickness(15, 0, 0, 0),
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    Children = { name, mainSwitch }
                };
                View = stack;
            }
        }
        public List<WrappedSelection<T>> WrappedItems = new List<WrappedSelection<T>>();
        public SelectMultipleBasePage(List<T> items)
        {
            WrappedItems = items.Select(item => new WrappedSelection<T>() { Item = item, IsSelected = (item as CheckItem).Selected }).ToList();
            ListView mainList = new ListView()
            {
                ItemsSource = WrappedItems,
                ItemTemplate = new DataTemplate(typeof(WrappedItemSelectionTemplate)),
            };
            mainList.RowHeight = 35;

            mainList.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null) return;
                var o = (WrappedSelection<T>)e.SelectedItem;
                o.IsSelected = !o.IsSelected;
                ((ListView)sender).SelectedItem = null; //de-select
            };
            Content = mainList;
            if (Device.OS == TargetPlatform.Windows)
            {   // fix issue where rows are badly sized (as tall as the screen) on WinPhone8.1
                mainList.RowHeight = 40;
                // also need icons for Windows app bar (other platforms can just use text)
                //ToolbarItems.Add(new ToolbarItem("All", "check.png", SelectAll, ToolbarItemOrder.Primary));
                ToolbarItems.Add(new ToolbarItem("None", "cancel.png", SelectNone, ToolbarItemOrder.Primary));
            }
            else
            {
                //ToolbarItems.Add(new ToolbarItem("All", null, SelectAll, ToolbarItemOrder.Primary));
                ToolbarItems.Add(new ToolbarItem("None", null, SelectNone, ToolbarItemOrder.Primary));
            }
        }
        //void SelectAll()
        //{
        //    foreach (var wi in WrappedItems)
        //    {
        //        wi.IsSelected = true;
        //    }
        //}
        void SelectNone()
        {
            foreach (var wi in WrappedItems)
            {
                wi.IsSelected = false;
            }
        }
        public List<T> GetSelection()
        {
            return WrappedItems.Where(item => item.IsSelected).Select(wrappedItem => wrappedItem.Item).ToList();
        }
    }
}