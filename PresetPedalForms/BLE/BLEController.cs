using System;
using System.Diagnostics;
using System.Collections.Generic;
using Plugin.BLE;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions;
using Xamarin.Forms;
using PresetPedalForms.Models;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Exceptions;

namespace PresetPedalForms
{
    public class BLEController
    {
        public BLEController()
        {
        }

        const string PEDAL_DEVICE_ID = "Pedal";
        const string WRITE_CHAR = "2222";
        public ICharacteristic writeChar;

        //const string CONTROL_DEVICE_ID = "Knob";
        //const string READ_CHAR = "2221";

        const string CONTROL_DEVICE_ID = "SalButton";
        //const string READ_CHAR = "2221";
        const string READ_CHAR = "2d30c082-f39f-4ce6-923f-3484ea480596";
        public ICharacteristic readChar;

        IBluetoothLE ble;
        IAdapter adapter;
        public IDevice PedalDevice;
        public IDevice ControlDevice;

        byte[] presetData = new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        byte[] messageData = new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public delegate void PedalConnectedEventHandler();
        public event PedalConnectedEventHandler PedalConnectedEvent;
        public delegate void PedalAutoConnectedEventHandler();
        public event PedalAutoConnectedEventHandler PedalAutoConnectedEvent;
        public delegate void PedalDisconnectedEventHandler();
        public event PedalDisconnectedEventHandler PedalDisconnectedEvent;

        public delegate void ControlConnectedEventHandler();
        public event ControlConnectedEventHandler ControlConnectedEvent;
        public delegate void ControlAutoConnectedEventHandler();
        public event ControlAutoConnectedEventHandler ControlAutoConnectedEvent;
        public delegate void ControlDisconnectedEventHandler();
        public event ControlDisconnectedEventHandler ControlDisconnectedEvent;

        public delegate void ControlUpdatedEventHandler(object sender, ControlUpdatedEventArgs e);
        public event ControlUpdatedEventHandler ControlUpdatedEvent;
        public sealed class ControlUpdatedEventArgs : EventArgs
        {
            public ControlUpdatedEventArgs(int val)
            {
                Val = val;
            }
            public int Val { get; set; }
        }

        CancellationTokenSource PBcts;
        CancellationTokenSource CTcts;
        public async void InitBLE()
        {
            if(!DependencyService.Get<PlatformSpecificInterface>().CheckIfSimulator()) // only if not simulator
            {
                ble = CrossBluetoothLE.Current;
                adapter = CrossBluetoothLE.Current.Adapter;
                //adapter.ScanTimeout = 4000;

                var state = ble.State;
                ble.StateChanged += (s, e) =>
                {
                    Debug.WriteLine("The bluetooth state changed to " + e.NewState);
                };

                adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
                adapter.DeviceConnectionLost += Adapter_DeviceConnectionLost;

                if(!App.mainProfile.BondedPedalDevice.ID.Equals(Guid.Empty))
                {
                    AutoConnectPedal();
                }
                //// wait a little until Pedal is connected or else services get updated while searching through them when adding device
                //await Task.Delay(500);
                if(!App.mainProfile.BondedControllerDevice.ID.Equals(Guid.Empty))
                {
                    AutoConnectController();
                }
            }

        }

        public async void AutoConnectPedal()
        {
            PBcts = new CancellationTokenSource();
            try
            {
                var device = await adapter.ConnectToKnownDeviceAsync(App.mainProfile.BondedPedalDevice.ID, new ConnectParameters(true, false), PBcts.Token);
                Debug.WriteLine("Pedalboard CONNECTED AUTOMATICALLY!");
                pedalConnectedState = true;
                PedalDevice = device;

				PedalAutoConnectedEvent();

				GetPedalServices(PedalDevice);
            }
            catch(DeviceConnectionException e)
            {
                Debug.WriteLine("Could not connect to pedalboard device: " + e);
            }
            PBcts = null;
        }

        public async void AutoConnectController()
        {
            CTcts = new CancellationTokenSource();
            try
            {
                var device = await adapter.ConnectToKnownDeviceAsync(App.mainProfile.BondedControllerDevice.ID, new ConnectParameters(true, false), CTcts.Token);
                Debug.WriteLine("Controller CONNECTED AUTOMATICALLY!");
                controllerConnectedState = true;
                ControlDevice = device;

				ControlAutoConnectedEvent();

				GetControlServices(ControlDevice);
            }
            catch(DeviceConnectionException e)
            {
                Debug.WriteLine("Could not connect to controller device: " + e);
            }
            CTcts = null;
        }

        public void Adapter_DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            Debug.WriteLine("Device discovered: " + e.Device.Name);
            if(e.Device.Name == PEDAL_DEVICE_ID)
            {
                connectPedalDevice(e.Device);
            }
            else if(e.Device.Name == CONTROL_DEVICE_ID)
            {
                connectControlDevice(e.Device);
            }
        }

        void Adapter_DeviceConnectionLost(object sender, DeviceEventArgs e)
        {
            Debug.WriteLine("Device DISCONNECTED: " + e.Device.Name);
            if(e.Device.Name == PEDAL_DEVICE_ID)
            {
                pedalConnectedState = false;
                PedalDevice = null;
                PedalDisconnectedEvent();
            }
            else if(e.Device.Name == CONTROL_DEVICE_ID)
            {
                controllerConnectedState = false;
                ControlDevice = null;
                ControlDisconnectedEvent();
            }
        }

        public void StartScan()
        {
            if(!DependencyService.Get<PlatformSpecificInterface>().CheckIfSimulator())
            {
                adapter.StartScanningForDevicesAsync();
            }
        }

        public void StopScan()
        {
            if(!DependencyService.Get<PlatformSpecificInterface>().CheckIfSimulator())
            {
                adapter.StopScanningForDevicesAsync();
            }
        }

        private bool _pedalConnectedState { get; set; }
        public bool pedalConnectedState { 
            get
            {
                return _pedalConnectedState;
            }
            set
            {
                _pedalConnectedState = value;
                if(value)
                    pedalConnectedStateString = "Pedalboard: Connected";
                else
                    pedalConnectedStateString = "Pedalboard: Disconnected";
            } 
        }

        private bool _controllerConnectedState { get; set; }
        public bool controllerConnectedState
        {
            get
            {
                return _controllerConnectedState;
            }
            set
            {
                _controllerConnectedState = value;
                if(value)
                    controllerConnectedString = "Controller: Connected";
                else
                    controllerConnectedString = "Controller: Disconnected";
            }
        }

        public string pedalConnectedStateString { get; set; }
        public string controllerConnectedString { get; set; }

        async void connectPedalDevice(IDevice device)
        {
            try 
            {
                await adapter.ConnectToDeviceAsync(device);
                Debug.WriteLine("PB CONNECTED!");
                pedalConnectedState = true;
                PedalDevice = device;
                App.mainProfile.BondedPedalDevice.ID = PedalDevice.Id;
                App.mainProfile.BondedPedalDevice.Name = PedalDevice.Name;
                PedalConnectedEvent();
		        GetPedalServices(PedalDevice);
            }
            catch(DeviceConnectionException e)
            {
                Debug.WriteLine("Could not connect: " + e);
            }
        }

        async void GetPedalServices(IDevice device)
        {
            var services = await device.GetServicesAsync();
            foreach(var service in services)
            {
                var characteristics = await service.GetCharacteristicsAsync();
                foreach(var characteristic in characteristics)
                {
                    if(characteristic.Uuid == WRITE_CHAR)
                    {
                        writeChar = characteristic;
                    }
                }
            }
        }

        async void connectControlDevice(IDevice device)
        {
            try
            {
                await adapter.ConnectToDeviceAsync(device);
                Debug.WriteLine("Controller connected!");
                controllerConnectedState = true;
                ControlDevice = device;
                App.mainProfile.BondedControllerDevice.ID = ControlDevice.Id;
                App.mainProfile.BondedControllerDevice.Name = ControlDevice.Name;
                ControlConnectedEvent();
                GetControlServices(ControlDevice);
            }
            catch(DeviceConnectionException e)
            {
                Debug.WriteLine("Could not controller connect: " + e);
            }
        }

        async void GetControlServices(IDevice device)
        {
            var services = await device.GetServicesAsync();
            foreach(var service in services)
            {
                var characteristics = await service.GetCharacteristicsAsync();
                foreach(var characteristic in characteristics)
                {
                    if(characteristic.Uuid == READ_CHAR)
                    {
                        readChar = characteristic;
                        readChar.ValueUpdated += ReadChar_ValueUpdated;

                        await characteristic.StartUpdatesAsync();
                    }
                }
            }
        }

        void ReadChar_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            var bytes = e.Characteristic.Value;
            //Debug.WriteLine("Got values: " + ByteArrayToString(bytes));
            if(bytes.Length > 0)
            {
                int val = BitConverter.ToInt32(bytes, 0);
                if(App.controlReady)
                {
                    ControlUpdatedEvent(this, new ControlUpdatedEventArgs(val));
                }
            }
        }

        public int[] LogExp = new int[] { 0,0,0,0,0,1,1,1,1,1,1,2,2,2,2,2,3,3,3,3,3,4,4,4,4,5,5,5,6,6,6,7,7,8,8,8,9,9,10,10,11,11,12,12,13,14,14,15,
                                    16,16,17,18,19,19,20,21,22,23,24,25,26,27,29,30,31,32,34,35,37,38,40,42,43,45,47,49,51,53,55,57,60,63,65,
                                    66,68,69,70,72,73,75,76,77,79,80,82,83,84,86,87,89,90,91,93,94,96,97,98,100,101,103,104,105,107,108,110,
                                    111,112,114,115,117,118,119,121,122,124,125,126,127};

        async public void SendPreset(Preset preset)
        {
            presetData[0] = 0x00; // Preset mode indicator
            // Loops
            for(int i = 0; i < preset.LoopDevices.Count; i++)
            {
                presetData[i+1] = (byte)(Convert.ToByte(preset.LoopDevices[i].OnOff) & 0xFF);
            }
            //presetData[1] = (byte)(Convert.ToByte(preset.Loop1State) & 0xFF);
            //presetData[2] = (byte)(Convert.ToByte(preset.Loop2State) & 0xFF);
            //presetData[3] = (byte)(Convert.ToByte(preset.Loop3State) & 0xFF);
            //presetData[4] = (byte)(Convert.ToByte(preset.Loop4State) & 0xFF);

            // midi
            foreach(var device in preset.MidiDevices)
            {
                var i = preset.MidiDevices.IndexOf(device);
                presetData[i+5] = (byte)(Convert.ToByte(device.SelectedProgram) & 0xFF);
                presetData[i+11] = (byte)(Convert.ToByte(device.EnabledIntValue) & 0xFF); // enabled/bypass
            }
            //if(preset.TLByp)
            //    presetData[11] = (byte)(Convert.ToByte(127) & 0xFF);
            //else
            //    presetData[11] = (byte)(Convert.ToByte(0) & 0xFF);

            //if(preset.BSByp)
            //    presetData[12] = (byte)(Convert.ToByte(127) & 0xFF);
            //else
            //    presetData[12] = (byte)(Convert.ToByte(0) & 0xFF);

            //if(preset.MOByp)
            //    presetData[13] = (byte)(Convert.ToByte(127) & 0xFF);
            //else
            //    presetData[13] = (byte)(Convert.ToByte(0) & 0xFF);
            //presetData[5] = (byte)(Convert.ToByte(preset.TLProgNum) & 0xFF);
            //presetData[6] = (byte)(Convert.ToByte(preset.BSProgNum) & 0xFF);
            //presetData[7] = (byte)(Convert.ToByte(preset.MOProgNum) & 0xFF);

            // expression
            foreach(var device in preset.ExpressionDevices)
            {
                var i = preset.ExpressionDevices.IndexOf(device);
                if(device.GetType().Equals(typeof(Riverside)))
                    presetData[i+8] = (byte)(Convert.ToByte(127 - LogExp[device.ExpressionIntValue]) & 0xFF);
                else
                    presetData[i+8] = (byte)(Convert.ToByte(LogExp[device.ExpressionIntValue]) & 0xFF);

            }

            //presetData[8] = (byte)(Convert.ToByte(127 - LogExp[preset.EXP1Val]) & 0xFF);
            ////presetData[9] = (byte)(Convert.ToByte(127-preset.EXP2Val) & 0xFF);
            //presetData[9] = (byte)(Convert.ToByte(LogExp[preset.EXP2Val]) & 0xFF);

            // switches
            foreach(var device in preset.SwitchDevices)
            {
                var i = preset.SwitchDevices.IndexOf(device);
                presetData[i+10] = (byte)(Convert.ToByte(device.SwitchValue) & 0xFF);
            }
            //presetData[10] = (byte)(Convert.ToByte(preset.FAV) & 0xFF);

            // TODO: BPM
            //short BPM;
            //if(preset.TLBPM < 120)
            //{
            //    BPM = (short)(preset.TLBPM - 1);
            //}
            //else /*if (preset.TLBPM >= 123)*/
            //{
            //    BPM = (short)(preset.TLBPM - 2);
            //}
            //var BPMbytes = BitConverter.GetBytes(BPM);
            //presetData[14] = (byte)(BPMbytes[0] & 0xFF);
            //presetData[15] = (byte)(BPMbytes[1] & 0xFF);

            if(writeChar != null)
            {
                Debug.WriteLine("Sending Char: " + writeChar.Uuid + " Value: " + ByteArrayToString(presetData));
                await writeChar.WriteAsync(presetData);
            }
        }

        async public void SendBPMMessage(byte type, short BPM)
        {
            if(BPM < 120)
            {
                BPM = (short)(BPM - 1);
            }
            else /*if(BPM >= 121)*/
            {
                BPM = (short)(BPM - 2);
            }
            messageData[0] = 0x01; // Manual mode indicator
            messageData[1] = (byte)(type & 0xFF);
            var BPMbytes = BitConverter.GetBytes(BPM);
            messageData[2] = (byte)(BPMbytes[0] & 0xFF);
            messageData[3] = (byte)(BPMbytes[1] & 0xFF);

            if(writeChar != null)
            {
                Debug.WriteLine("Sending Char: " + writeChar.Uuid + " Value: " + ByteArrayToString(messageData));
                await writeChar.WriteAsync(messageData);
            }
        }

        async public void SendManualMessage(byte type, byte val)
        {
            messageData[0] = 0x01; // Manual mode indicator
            messageData[1] = (byte)(type & 0xFF);
            messageData[2] = (byte)(val & 0xFF);

            if(writeChar != null)
            {
                Debug.WriteLine("Sending Char: " + writeChar.Uuid + " Value: " + ByteArrayToString(messageData));
                await writeChar.WriteAsync(messageData);
            }
        }

        async public void SendManualMessageCC(byte type, byte val1, byte val2)
        {
            messageData[0] = 0x01; // Manual mode indicator
            messageData[1] = (byte)(type & 0xFF);
            messageData[2] = (byte)(val1 & 0xFF);
            messageData[3] = (byte)(val2 & 0xFF);

            if(writeChar != null)
            {
                Debug.WriteLine("Sending Char: " + writeChar.Uuid + " Value: " + ByteArrayToString(messageData));
                await writeChar.WriteAsync(messageData);
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach(byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }
    }
}
