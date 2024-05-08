using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotFx.DoMain.Models.ModelsResponse
{
    public class AccountFxResponse
    {
        public string Id { get; set; }
        public bool AutoTrading { get; set; }
        public int SignalType { get; set; }
        public int IsOnline { get; set; }
    }
}
