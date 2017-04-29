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

        public override byte ManualMessageType
        {
            get { return 0x0C; }
        }

        public override ExpressionDevice Copy()
        {
        	var device = new Riverside();
        	device.ExpressionDoubleValue = this.ExpressionDoubleValue;
        	return device;
        }
    }
}
