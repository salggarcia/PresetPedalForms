using System;
using Xamarin.Forms;
using System.Diagnostics;

namespace PresetPedalForms
{
    public class ListViewWithLongPressGesture : ListView
    {
        public event HandleLongPressEventHandler LongPressActivated;
        public delegate void HandleLongPressEventHandler();

        public void HandleLongPress(object sender, EventArgs e)
        {
            LongPressActivated();
        }

    }
}
