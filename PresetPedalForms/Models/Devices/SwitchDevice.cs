using System;
namespace PresetPedalForms
{
    public abstract class SwitchDevice : IDevice
    {
        public SwitchDevice()
        {
        }

        public abstract ManufacturerTypeCode ManufacturerTypeCode { get; }
        public abstract DeviceTypeCode DeviceTypeCode { get; }

        public bool SwitchValue { get; set; }
    }
}
