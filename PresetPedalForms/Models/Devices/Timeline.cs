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

        public override byte ManualMessageType
        {
            get { return 0x09; }
        }

        public override byte ManualCCType
        {
            get { return 0x0F; }
        }

        public override void ParseProgram(byte[] rawProgram)
        {
            
        }

        public override MidiDevice Copy()
        {
            var timeline = new Timeline();
            timeline.Enabled = this.Enabled;
            timeline.SelectedProgram = this.SelectedProgram;
            return timeline;
        }
    }
}
