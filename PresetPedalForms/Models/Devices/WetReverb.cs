using System;
namespace PresetPedalForms
{
    public class WetReverb : ExpressionDevice
    {
        public WetReverb()
        {
        }

        public override ManufacturerTypeCode ManufacturerTypeCode
        {
            get { return ManufacturerTypeCode.NEUNABER; }
        }

        public override DeviceTypeCode DeviceTypeCode
        {
            get { return DeviceTypeCode.WETREVERB; }
        }
    }
}
