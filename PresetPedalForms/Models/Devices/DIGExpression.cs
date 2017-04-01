using System;
namespace PresetPedalForms
{
    public class DIGExpression : ExpressionDevice
    {
        public DIGExpression()
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
