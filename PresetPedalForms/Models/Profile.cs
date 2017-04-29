using System;
using System.Collections.Generic;
using Plugin.BLE;

namespace PresetPedalForms
{
    public class Profile
    {
        public Profile()
        {
            LoopDevices = new List<LoopDevice>();
            MidiDevices = new List<Type>();
            ExpressionDevices = new List<Type>();
            SwitchDevices = new List<Type>();
            BondedPedalDevice = new BondedDevice();
            BondedControllerDevice = new BondedDevice();
        }

        //public int NumberOfLoops { get; set; }
        public List<LoopDevice> LoopDevices { get; set; }
        public List<Type> MidiDevices { get; set; }
        public List<Type> ExpressionDevices { get; set; }
        public List<Type> SwitchDevices { get; set; }

        public BondedDevice BondedPedalDevice { get; set; }
        public BondedDevice BondedControllerDevice { get; set; }


    }
}
