using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotFx.DoMain.Models.ModelsResponse
{
    public class AccountFxResponse
    {
        public string AccountNumber { get; set; }
        public bool AutoTrading { get; set; }
        public bool SignalFlag { get; set; }
        public int Signal { get; set; }
    }
}
