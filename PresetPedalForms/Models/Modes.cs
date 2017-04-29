using System;
namespace PresetPedalForms
{
    public class Modes
    {
        public Modes()
        {
            ManualMode = false;
            LiveMode = false;
        }

        public bool ManualMode { get; set; }
        public bool LiveMode { get; set; }
    }
}
