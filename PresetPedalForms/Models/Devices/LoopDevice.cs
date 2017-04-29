using System;
namespace PresetPedalForms
{
    public /*abstract*/ class LoopDevice /*: IPedalDevice*/
    {
        public LoopDevice(/*string deviceName*/)
        {
            //ID = GetIDFromTime();
            //Name = deviceName;
            //OnOff = false;
        }

        //public abstract ManufacturerTypeCode ManufacturerTypeCode { get; }
        //public abstract DeviceTypeCode DeviceTypeCode { get; }
        //public abstract byte ManualMessageType { get; }
        public long ID { get; set; }
        public string Name { get; set; }
        public bool OnOff { get; set; }

        public LoopDevice Copy()
        {
            var result = new LoopDevice();
            result.ID = this.ID;
            result.Name = this.Name;
            result.OnOff = this.OnOff;
            return result;
        }
        //public long GetIDFromTime()
        //{
        //	long now = DateTime.Now.Year + DateTime.Now.Month +
        //				DateTime.Now.Day +
        //				DateTime.Now.Hour +
        //				DateTime.Now.Minute +
        //				DateTime.Now.Second +
        //				DateTime.Now.Millisecond;

        //	return now;
        //}
    }
}
