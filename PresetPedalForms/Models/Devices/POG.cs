using System;
namespace PresetPedalForms
{
    public class POG :  ExpressionDevice
    {
        public POG()
        {
        }

        public override ManufacturerTypeCode ManufacturerTypeCode
        {
            get { return ManufacturerTypeCode.ELECTROHARMONIX; }
        }

        public override DeviceTypeCode DeviceTypeCode
        {
            get { return DeviceTypeCode.POG; }
        }

        public override byte ManualMessageType
        {
            get { return 0x0D; }
        }

        public override ExpressionDevice Copy()
        {
        	var device = new POG();
        	device.ExpressionDoubleValue = this.ExpressionDoubleValue;
        	return device;
        }
    }
}
