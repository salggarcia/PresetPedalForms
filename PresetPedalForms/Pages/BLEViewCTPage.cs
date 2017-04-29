using System;

using Xamarin.Forms;

namespace PresetPedalForms
{
    public class BLEViewCTPage : ContentPage
    {
        public BLEViewCTPage()
        {
        	Label ctLabel = new Label { VerticalOptions = LayoutOptions.Center, Margin = new Thickness(15, 0, 0, 0) };
        	ctLabel.SetBinding(Label.TextProperty, "Name");
        	ViewCell ctView = new ViewCell { View = ctLabel };
        	ctView.Tapped += CtView_Tapped;
        	TableView tableView = new TableView
        	{
        		Intent = TableIntent.Form,
        		Root = new TableRoot("TableView Title")
        				{
        					new TableSection
        					{
        						ctView
        					}
        				}
        	};
        	Title = "View Controller";

        	Content = new StackLayout
        	{
        		Children = {
        					tableView
        				}
        	};
        }

        void CtView_Tapped(object sender, EventArgs e)
        {
        	// TODO: Connect to bonded CT by clicking, show progress indicator
        }
    }
}

