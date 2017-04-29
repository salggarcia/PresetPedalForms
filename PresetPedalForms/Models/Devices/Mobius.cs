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

        public override byte ManualMessageType
        {
            get
            {
                return 0x0B;
            }
        }

        public override byte ManualCCType
        {
            get { return 0x11; }
        }

        public override void ParseProgram(byte[] rawProgram)
        {

        }

        public override MidiDevice Copy()
        {
        	var device = new Mobius();
        	device.Enabled = this.Enabled;
        	device.SelectedProgram = this.SelectedProgram;
        	return device;
        }
    }
}
