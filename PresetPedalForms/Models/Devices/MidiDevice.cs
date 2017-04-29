using System;
using System.Linq;

namespace PresetPedalForms
{
    public abstract class MidiDevice : IPedalDevice
    {
        public MidiDevice()
        {
            ID = GetIDFromTime();
        }
        public long ID { get; set; }
        public abstract int ProgramCount { get; }
        public abstract ManufacturerTypeCode ManufacturerTypeCode { get; }
        public abstract DeviceTypeCode DeviceTypeCode { get; }

        public abstract byte ManualMessageType { get; }
        public abstract byte ManualCCType { get; }

        public abstract void ParseProgram(byte[] rawProgram);

        public abstract MidiDevice Copy();

        public bool Enabled { get; set;}
        public int EnabledIntValue
        {
            get
            {
                if(Enabled)
                    return 127;
                else
                    return 0;
            }
            set { }
        }

        public int SelectedProgram { get; set;}

        //public MidiDevice Copy()
        //{
        //    var result = App.mainProfile.MidiDevices.Select(m => Activator.CreateInstance(m) as MidiDevice).ToList();

        //    return result;
        //}

        public long GetIDFromTime()
        {
            long now = DateTime.Now.Year + DateTime.Now.Month +
                        DateTime.Now.Day +
                        DateTime.Now.Hour +
                        DateTime.Now.Minute +
                        DateTime.Now.Second +
                        DateTime.Now.Millisecond;

            return now;
        }
    }
}
