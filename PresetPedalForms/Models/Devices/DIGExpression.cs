﻿using System;
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

        public override byte ManualMessageType
        {
            get
            {
                return 0xA0;
            }
        }

        public override ExpressionDevice Copy()
        {
        	var device = new DIGExpression();
        	device.ExpressionDoubleValue = this.ExpressionDoubleValue;
        	return device;
        }
    }
}
