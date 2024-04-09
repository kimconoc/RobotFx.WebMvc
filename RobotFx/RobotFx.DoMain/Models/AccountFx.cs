using RobotFx.DoMain.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotFx.DoMain.Models
{
    public class AccountFx : VBaseModel<int>
    {
        public int UserId { get; set; }
        public string IdAccountFx { get; set; }
        public string? Name { get; set; }
        public double StartingPrice { get; set; }
        public int PriceSpacing1To5 { get; set; }
        public int PriceSpacing5To13 { get; set; }
        public int EnterType { get; set; }
        public int IsOnline { get; set; }
    }
}
