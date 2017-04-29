using System;
namespace PresetPedalForms
{
    public interface IPedalDevice
    {
        //int ProgramCount { get; }
        long ID { get; }
        ManufacturerTypeCode ManufacturerTypeCode { get; }
        DeviceTypeCode DeviceTypeCode { get; }

        //void ParseProgram(byte[] rawProgram);

        //bool Enabled { get; set; }
    }
}
