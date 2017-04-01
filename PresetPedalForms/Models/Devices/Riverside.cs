using System;
namespace PresetPedalForms
{
    public class Riverside : ExpressionDevice
    {
        public Riverside()
        {
        }

        public override ManufacturerTypeCode ManufacturerTypeCode
        {
            get { return ManufacturerTypeCode.STRYMON; }
        }

        public override DeviceTypeCode DeviceTypeCode
        {
            get { return DeviceTypeCode.RIVERSIDE; }
        }
    }
}
