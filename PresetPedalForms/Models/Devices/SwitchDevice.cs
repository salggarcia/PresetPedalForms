using System;
namespace PresetPedalForms
{
    public abstract class SwitchDevice : IPedalDevice
    {
        public SwitchDevice()
        {
        }
        public long ID { get; set; }
        public abstract ManufacturerTypeCode ManufacturerTypeCode { get; }
        public abstract DeviceTypeCode DeviceTypeCode { get; }
        public abstract byte ManualMessageType { get; }

        public bool SwitchValue { get; set; }

        public abstract SwitchDevice Copy();
    }
}
