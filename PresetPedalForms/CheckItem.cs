using System;

using PresetPedalForms.Models;

namespace PresetPedalForms
{
    public class CheckItem
    {
        public CheckItem()
        {
        }

        public Preset preset { get; set; }

        public bool Selected { get; set; }

    }
}
