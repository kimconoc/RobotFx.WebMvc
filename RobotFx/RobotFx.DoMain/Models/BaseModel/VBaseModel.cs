using RobotFx.DoMain.Models.BaseModel.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotFx.DoMain.Models.BaseModel
{
    public class VBaseModel<Tkey> : IEntity<Tkey>
    {
        [Key]
        public Tkey Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
