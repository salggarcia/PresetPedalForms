using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PresetPedalForms.Models
{
    public class Song
    {
        public Song()
        {
            Name = "New Song";
            Presets = new ObservableCollection<Preset>();
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public ObservableCollection<Preset> Presets { get; set; }

        //public List<long> PresetIDs { get; set; }
    }
}
