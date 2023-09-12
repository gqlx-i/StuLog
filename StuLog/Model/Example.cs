using StuLog.Common;
using StuLog.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuLog.Model
{
    public class Example : PropertyNotifyBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }
        private string _code;
        public string Code
        {
            get => _code;
            set => SetAndNotify(ref _code, value);
        }
    }
}
