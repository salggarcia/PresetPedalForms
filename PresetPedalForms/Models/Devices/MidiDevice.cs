using System;
namespace PresetPedalForms
{
    public abstract class MidiDevice : IDevice
    {
        public MidiDevice()
        {
            ID = GetIDFromTime();
        }
        public long ID { get; set; }
        public abstract int ProgramCount { get; }
        public abstract ManufacturerTypeCode ManufacturerTypeCode { get; }
        public abstract DeviceTypeCode DeviceTypeCode { get; }

        public abstract void ParseProgram(byte[] rawProgram);

        public bool Enabled { get; set;}

        public int SelectedProgram { get; set;}

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
