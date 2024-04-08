using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotFx.DoMain.Enum
{
    public enum UserAgentEnum
    {
        [Description("Mobile-Iphone")]
        MobileIphone = 0,
        [Description("Mobile-Android")]
        MobileAndroid = 1,
        [Description("Computer-Windows")]
        ComputerWindows = 2,
        [Description("Mobile-Ipad-Os")]
        IpadOs = 3,
        [Description("Unknown-Device")]
        UnknownDevice = 4,

    }
}
