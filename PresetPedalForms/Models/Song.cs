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
            Presets = new ObservableCollectionEx<Preset>();
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public ObservableCollectionEx<Preset> Presets { get; set; }

        //public List<long> PresetIDs { get; set; }
    }
}
