using System;
namespace PresetPedalForms
{
    public class Bigsky : MidiDevice
    {
        public Bigsky()
        {
        }

        public override ManufacturerTypeCode ManufacturerTypeCode
        {
            get { return ManufacturerTypeCode.STRYMON; }
        }

        public override DeviceTypeCode DeviceTypeCode
        {
            get { return DeviceTypeCode.BIGSKY; }
        }

        public override int ProgramCount
        {
            get { return 300; }
        }

        public override void ParseProgram(byte[] rawProgram)
        {

        }
    }
}
