using System;
namespace PresetPedalForms
{
    public class Timeline : MidiDevice
    {
        public Timeline()
        {
        }

        public override ManufacturerTypeCode ManufacturerTypeCode
        {
            get { return ManufacturerTypeCode.STRYMON; }
        }

        public override DeviceTypeCode DeviceTypeCode
        {
            get { return DeviceTypeCode.TIMELINE; }
        }

        public override int ProgramCount
        {
            get { return 200; }
        }

        public override void ParseProgram(byte[] rawProgram)
        {
            
        }
    }
}
