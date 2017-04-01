using System;
using System.Collections.Generic;

namespace PresetPedalForms
{
    public class Profile
    {
        public Profile()
        {
            NumberOfLoops = 4;
            MidiDevices = new List<Type>();
            ExpressionDevices = new List<Type>();
            SwitchDevices = new List<Type>();
        }

        public int NumberOfLoops { get; set; }

        public List<Type> MidiDevices { get; set; }
        public List<Type> ExpressionDevices { get; set; }
        public List<Type> SwitchDevices { get; set; }

    }
}
