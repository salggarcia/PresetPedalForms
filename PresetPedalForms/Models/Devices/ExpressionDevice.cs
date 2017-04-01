using System;
namespace PresetPedalForms
{
    public abstract class ExpressionDevice : IDevice
    {
        public ExpressionDevice()
        {
        }

        public abstract ManufacturerTypeCode ManufacturerTypeCode { get; }
        public abstract DeviceTypeCode DeviceTypeCode { get; }

        public int SLIDERRANGE = 127;
        public double ExpressionDoubleValue { get; set; }
        public int ExpressionIntValue 
        { 
            get
            {
                return Convert.ToInt32(ExpressionDoubleValue * SLIDERRANGE);
            }
            set{}
        }

    }
}
