using System;
namespace PresetPedalForms
{
    public abstract class ExpressionDevice : IPedalDevice
    {
        public ExpressionDevice()
        {
        }
        public long ID { get; set; }
        public abstract ManufacturerTypeCode ManufacturerTypeCode { get; }
        public abstract DeviceTypeCode DeviceTypeCode { get; }
        public abstract byte ManualMessageType { get; }

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

        public abstract ExpressionDevice Copy();

    }
}
