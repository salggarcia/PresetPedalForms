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

        public override byte ManualMessageType
        {
            get { return 0x0A; }
        }

        public override byte ManualCCType
        {
            get { return 0x10; }
        }

        public override void ParseProgram(byte[] rawProgram)
        {

        }

        public override MidiDevice Copy()
        {
        	var device = new Bigsky();
        	device.Enabled = this.Enabled;
        	device.SelectedProgram = this.SelectedProgram;
        	return device;
        }
    }
}
