using System;
namespace PresetPedalForms
{
    public interface IDevice
    {
        //int ProgramCount { get; }
        ManufacturerTypeCode ManufacturerTypeCode { get; }
        DeviceTypeCode DeviceTypeCode { get; }

        //void ParseProgram(byte[] rawProgram);

        //bool Enabled { get; set; }
    }
}
