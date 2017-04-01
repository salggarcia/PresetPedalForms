using System;
namespace PresetPedalForms
{
    public class DIGSwitch : SwitchDevice
    {
        public DIGSwitch()
        {
        }

        public override ManufacturerTypeCode ManufacturerTypeCode
        {
            get { return ManufacturerTypeCode.STRYMON; }
        }

        public override DeviceTypeCode DeviceTypeCode
        {
            get { return DeviceTypeCode.DIG; }
        }
    }
}
