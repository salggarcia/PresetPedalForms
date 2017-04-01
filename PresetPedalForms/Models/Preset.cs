using System;

using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Text;

using System.Diagnostics;
using System.Linq;

namespace PresetPedalForms.Models
{
    public class Preset
    {
        public Preset()
        {
            ID = GetIDFromTime();
            Name = "New Preset";

            Loops = new List<bool>(App.mainProfile.NumberOfLoops);
            Loops.Resize(App.mainProfile.NumberOfLoops, false);

            MidiDevices = App.mainProfile.MidiDevices.Select(m => Activator.CreateInstance(m) as MidiDevice).ToList();
            ExpressionDevices = App.mainProfile.ExpressionDevices.Select(m => Activator.CreateInstance(m) as ExpressionDevice).ToList();
            SwitchDevices = App.mainProfile.SwitchDevices.Select(m => Activator.CreateInstance(m) as SwitchDevice).ToList();

            // setup devices
            //foreach(var device in App.mainProfile.MidiDevices)
            //{
            //    bindingPreset.MidiDevices.Add(Activator.CreateInstance(device) as MidiDevice);
            //}
            //foreach(var device in App.mainProfile.ExpressionDevices)
            //{
            //    bindingPreset.ExpressionDevices.Add(Activator.CreateInstance(device) as ExpressionDevice);
            //}
            //foreach(var device in App.mainProfile.SwitchDevices)
            //{
            //    bindingPreset.SwitchDevices.Add(Activator.CreateInstance(device) as SwitchDevice);
            //}

            //var tempMidiArray = new MidiDevice[] {};
            //App.mainProfile.MidiDevices.CopyTo(tempMidiArray);
            //MidiDevices = tempMidiArray.ToList();
            //var tempExpArray = new ExpressionDevice[] { };
            //App.mainProfile.ExpressionDevices.CopyTo(tempExpArray);
            //ExpressionDevices = tempExpArray.ToList();
            //var tempSwitchArray = new SwitchDevice[] { };
            //App.mainProfile.SwitchDevices.CopyTo(tempSwitchArray);
            //SwitchDevices = tempSwitchArray.ToList();


            //TLProgNum = TLProgram.A0;
            //BSProgNum = BSProgram.A0;

            //MOProgNum = MOProgram.A0;
            //EXP1Val = 0;
            //EXP2Val = 0;
            //TLBPM = 120;
        }

        public override string ToString()
        {
            return Name;
        }

        public long ID { get; set; }
        public string Name { get; set; }

        public List<bool> Loops { get; set; }
        public List<MidiDevice> MidiDevices { get; set; }
        public List<ExpressionDevice> ExpressionDevices { get; set; }
        public List<SwitchDevice> SwitchDevices { get; set; }

        //public TLProgram TLProgNum { get; set; }
        //public BSProgram BSProgNum { get; set; }
        //public MOProgram MOProgNum { get; set; }

        //public int EXP1Val { get; set; }
        //public int EXP2Val { get; set; }
        //public bool FAV { get; set; }

        //public bool TLByp { get; set; }
        //public bool BSByp { get; set; }
        //public bool MOByp { get; set; }

        //public short TLBPM { get; set; }
    
        public long GetIDFromTime()
        {
            long now = DateTime.Now.Year + DateTime.Now.Month +
                        DateTime.Now.Day +
                        DateTime.Now.Hour +
                        DateTime.Now.Minute +
                        DateTime.Now.Second +
                        DateTime.Now.Millisecond;

            return now;
        }
    }

    //public enum TLProgram
    //{
    //    A0, B0,
    //    A1, B1,
    //    A2, B2,
    //    A3, B3,
    //    A4, B4,
    //    A5, B5,
    //    A6, B6,
    //    A7, B7,
    //    A8, B8,
    //    A9, B9,
    //    A10, B10,
    //    A11, B11,
    //    A12, B12,
    //    A13, B13,
    //    A14, B14,
    //    A15, B15,
    //    A16, B16,
    //    A17, B17,
    //    A18, B18,
    //    A19, B19,
    //    A20, B20,
    //    A21, B21,
    //    A22, B22,
    //    A23, B23,
    //    A24, B24,
    //    A25, B25,
    //    A26, B26,
    //    A27, B27,
    //    A28, B28,
    //    A29, B29,
    //    A30, B30,
    //    A31, B31,
    //    A32, B32,
    //    A33, B33,
    //    A34, B34,
    //    A35, B35,
    //    A36, B36,
    //    A37, B37,
    //    A38, B38,
    //    A39, B39,
    //    A40, B40,
    //    A41, B41,
    //    A42, B42,
    //    A43, B43,
    //    A44, B44,
    //    A45, B45,
    //    A46, B46,
    //    A47, B47,
    //    A48, B48,
    //    A49, B49,
    //    A50, B50,
    //    A51, B51,
    //    A52, B52,
    //    A53, B53,
    //    A54, B54,
    //    A55, B55,
    //    A56, B56,
    //    A57, B57,
    //    A58, B58,
    //    A59, B59,
    //    A60, B60,
    //    A61, B61,
    //    A62, B62,
    //    A63, B63,// *** highest without having to send CC
    //    //A64, B64,
    //    //A65, B65,
    //    //A66, B66,
    //    //A67, B67,
    //    //A68, B68,
    //    //A69, B69,
    //    //A70, B70,
    //    //A71, B71,
    //    //A72, B72,
    //    //A73, B73,
    //    //A74, B74,
    //    //A75, B75,
    //    //A76, B76,
    //    //A77, B77,
    //    //A78, B78,
    //    //A79, B79,
    //    //A80, B80,
    //    //A81, B81,
    //    //A82, B82,
    //    //A83, B83,
    //    //A84, B84,
    //    //A85, B85,
    //    //A86, B86,
    //    //A87, B87,
    //    //A88, B88,
    //    //A89, B89,
    //    //A90, B90,
    //    //A91, B91,
    //    //A92, B92,
    //    //A93, B93,
    //    //A94, B94,
    //    //A95, B95,
    //    //A96, B96,
    //    //A97, B97,
    //    //A98, B98,
    //    //A99, B99
    //}

    //public enum MOProgram
    //{
    //    A0, B0,
    //    A1, B1,
    //    A2, B2,
    //    A3, B3,
    //    A4, B4,
    //    A5, B5,
    //    A6, B6,
    //    A7, B7,
    //    A8, B8,
    //    A9, B9,
    //    A10, B10,
    //    A11, B11,
    //    A12, B12,
    //    A13, B13,
    //    A14, B14,
    //    A15, B15,
    //    A16, B16,
    //    A17, B17,
    //    A18, B18,
    //    A19, B19,
    //    A20, B20,
    //    A21, B21,
    //    A22, B22,
    //    A23, B23,
    //    A24, B24,
    //    A25, B25,
    //    A26, B26,
    //    A27, B27,
    //    A28, B28,
    //    A29, B29,
    //    A30, B30,
    //    A31, B31,
    //    A32, B32,
    //    A33, B33,
    //    A34, B34,
    //    A35, B35,
    //    A36, B36,
    //    A37, B37,
    //    A38, B38,
    //    A39, B39,
    //    A40, B40,
    //    A41, B41,
    //    A42, B42,
    //    A43, B43,
    //    A44, B44,
    //    A45, B45,
    //    A46, B46,
    //    A47, B47,
    //    A48, B48,
    //    A49, B49,
    //    A50, B50,
    //    A51, B51,
    //    A52, B52,
    //    A53, B53,
    //    A54, B54,
    //    A55, B55,
    //    A56, B56,
    //    A57, B57,
    //    A58, B58,
    //    A59, B59,
    //    A60, B60,
    //    A61, B61,
    //    A62, B62,
    //    A63, B63,// ***
    //    //A64, B64,
    //    //A65, B65,
    //    //A66, B66,
    //    //A67, B67,
    //    //A68, B68,
    //    //A69, B69,
    //    //A70, B70,
    //    //A71, B71,
    //    //A72, B72,
    //    //A73, B73,
    //    //A74, B74,
    //    //A75, B75,
    //    //A76, B76,
    //    //A77, B77,
    //    //A78, B78,
    //    //A79, B79,
    //    //A80, B80,
    //    //A81, B81,
    //    //A82, B82,
    //    //A83, B83,
    //    //A84, B84,
    //    //A85, B85,
    //    //A86, B86,
    //    //A87, B87,
    //    //A88, B88,
    //    //A89, B89,
    //    //A90, B90,
    //    //A91, B91,
    //    //A92, B92,
    //    //A93, B93,
    //    //A94, B94,
    //    //A95, B95,
    //    //A96, B96,
    //    //A97, B97,
    //    //A98, B98,
    //    //A99, B99
    //}

    //public enum BSProgram
    //{
    //    A0, B0, C0,
    //    A1, B1, C1,
    //    A2, B2, C2,
    //    A3, B3, C3,
    //    A4, B4, C4,
    //    A5, B5, C5,
    //    A6, B6, C6,
    //    A7, B7, C7,
    //    A8, B8, C8,
    //    A9, B9, C9,
    //    A10, B10, C10,
    //    A11, B11, C11,
    //    A12, B12, C12,
    //    A13, B13, C13,
    //    A14, B14, C14,
    //    A15, B15, C15,
    //    A16, B16, C16,
    //    A17, B17, C17,
    //    A18, B18, C18,
    //    A19, B19, C19,
    //    A20, B20, C20,
    //    A21, B21, C21,
    //    A22, B22, C22,
    //    A23, B23, C23,
    //    A24, B24, C24,
    //    A25, B25, C25,
    //    A26, B26, C26,
    //    A27, B27, C27,
    //    A28, B28, C28,
    //    A29, B29, C29,
    //    A30, B30, C30,
    //    A31, B31, C31,
    //    A32, B32, C32,
    //    A33, B33, C33,
    //    A34, B34, C34,
    //    A35, B35, C35,
    //    A36, B36, C36,
    //    A37, B37, C37,
    //    A38, B38, C38,
    //    A39, B39, C39,
    //    A40, B40, C40,
    //    A41, B41, C41,// ***
    //    //A42, B42, C42,
    //    //A43, B43, C43,
    //    //A44, B44, C44,
    //    //A45, B45, C45,
    //    //A46, B46, C46,
    //    //A47, B47, C47,
    //    //A48, B48, C48,
    //    //A49, B49, C49,
    //    //A50, B50, C50,
    //    //A51, B51, C51,
    //    //A52, B52, C52,
    //    //A53, B53, C53,
    //    //A54, B54, C54,
    //    //A55, B55, C55,
    //    //A56, B56, C56,
    //    //A57, B57, C57,
    //    //A58, B58, C58,
    //    //A59, B59, C59,
    //    //A60, B60, C60,
    //    //A61, B61, C61,
    //    //A62, B62, C62,
    //    //A63, B63, C63,
    //    //A64, B64, C64,
    //    //A65, B65, C65,
    //    //A66, B66, C66,
    //    //A67, B67, C67,
    //    //A68, B68, C68,
    //    //A69, B69, C69,
    //    //A70, B70, C70,
    //    //A71, B71, C71,
    //    //A72, B72, C72,
    //    //A73, B73, C73,
    //    //A74, B74, C74,
    //    //A75, B75, C75,
    //    //A76, B76, C76,
    //    //A77, B77, C77,
    //    //A78, B78, C78,
    //    //A79, B79, C79,
    //    //A80, B80, C80,
    //    //A81, B81, C81,
    //    //A82, B82, C82,
    //    //A83, B83, C83,
    //    //A84, B84, C84,
    //    //A85, B85, C85,
    //    //A86, B86, C86,
    //    //A87, B87, C87,
    //    //A88, B88, C88,
    //    //A89, B89, C89,
    //    //A90, B90, C90,
    //    //A91, B91, C91,
    //    //A92, B92, C92,
    //    //A93, B93, C93,
    //    //A94, B94, C94,
    //    //A95, B95, C95,
    //    //A96, B96, C96,
    //    //A97, B97, C97,
    //    //A98, B98, C98,
    //    //A99, B99, C99
    //}
}
