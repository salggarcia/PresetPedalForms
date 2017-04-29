using System;

using Xamarin.Forms;

namespace PresetPedalForms
{
    public class BLEViewPBPage : ContentPage
    {
        public BLEViewPBPage()
        {
            Label pbLabel = new Label { VerticalOptions = LayoutOptions.Center, Margin = new Thickness(15, 0, 0, 0) };
            pbLabel.SetBinding(Label.TextProperty, "Name");
            ViewCell pbView = new ViewCell { View = pbLabel };
            pbView.Tapped += PbView_Tapped;
            TableView tableView = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot("TableView Title")
                {
                    new TableSection 
                    { 
                        pbView
                    }
                }
            };
            Title = "View Pedalboard";

            Content = new StackLayout
            {
                Children = {
                    tableView
                }
            };
        }

        void PbView_Tapped(object sender, EventArgs e)
        {
            // TODO: Connect to bonded PB by clicking, show progress indicator
        }
    }
}

