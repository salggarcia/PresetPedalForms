using System;
namespace PresetPedalForms
{
    public class DIGFavorite : SwitchDevice
    {
        public DIGFavorite()
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

        public override byte ManualMessageType
        {
            get
            {
                return 0x0E;
            }
        }

        public override SwitchDevice Copy()
        {
            var device = new DIGFavorite();
            device.SwitchValue = this.SwitchValue;
            return device;
        }
    }
}
