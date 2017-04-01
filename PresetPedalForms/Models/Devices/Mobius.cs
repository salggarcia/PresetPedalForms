using System;
namespace PresetPedalForms
{
    public class Mobius : MidiDevice
    {
        public Mobius()
        {
        }

        public override ManufacturerTypeCode ManufacturerTypeCode
        {
            get { return ManufacturerTypeCode.STRYMON; }
        }

        public override DeviceTypeCode DeviceTypeCode
        {
            get { return DeviceTypeCode.MOBIUS; }
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
