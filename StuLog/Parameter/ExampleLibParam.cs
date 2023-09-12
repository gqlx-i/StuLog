using StuLog.Common;
using StuLog.Interface;
using StuLog.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuLog.Parameter
{
    [Export(typeof(IParameter))]
    public class ExampleLibParam : ParameterBase
    {
        public ExampleLibParamItem Item { get; set; } = new ExampleLibParamItem();
        public override void Copy(IParameter source)
        {
            ExampleLibParam sp = source as ExampleLibParam;
            if (sp != null && sp.Item != null)
            {
                this.Item = (ExampleLibParamItem)sp.Item.Clone();
            }
        }
    }
    public class ExampleLibParamItem : PropertyNotifyBase, IParameterItem
    {
        private ObservableCollection<Example> _examples = new ObservableCollection<Example>();
        public ObservableCollection<Example> Examples
        {
            get => _examples;
            set => SetAndNotify(ref _examples, value);
        }
        public IParameterItem Clone()
        {
            return (IParameterItem)base.MemberwiseClone();
        }
    }
}
