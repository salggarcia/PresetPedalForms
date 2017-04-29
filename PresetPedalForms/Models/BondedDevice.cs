using System;
namespace PresetPedalForms
{
    public class BondedDevice
    {
        public BondedDevice()
        {
            ID = Guid.Empty;
        }

        public Guid ID { get; set;}

        public string Name { get; set; }
    }
}
