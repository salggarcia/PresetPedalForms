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

        public override byte ManualMessageType
        {
            get
            {
                return 0xA1;
            }
        }

        public override ExpressionDevice Copy()
        {
        	var device = new WetReverb();
        	device.ExpressionDoubleValue = this.ExpressionDoubleValue;
        	return device;
        }
    }
}
