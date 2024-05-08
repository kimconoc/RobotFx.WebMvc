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
    public enum SignalTypeEnum
    {
        [Description("Buy")]
        Buy = 0,
        [Description("Sell")]
        Sell = 1,
        [Description("Random")]
        random = 2,
    }
    public enum IsOnlineTypeEnum
    {
        [Description("Off")]
        Off = 0,
        [Description("On")]
        On = 1,
    }
}
