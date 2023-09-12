using StuLog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuLog.EventModel
{
    public class ExampleOperation
    {
        public Example Example { get; set; }
        public ExOpt Opteration { get; set; }
    }

    public enum ExOpt
    {
        Add,
        Edit,
    }
}
