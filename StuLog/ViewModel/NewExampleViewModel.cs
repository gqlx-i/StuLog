using StuLog.Common;
using StuLog.Model;
using StuLog.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StuLog.ViewModel
{
    public class NewExampleViewModel : ViewModelBase
    {
        private Example _example;
        public Example Example
        {
            get => _example;
            set => SetAndNotify(ref _example, value);
        }

        public ICommand FinishCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public NewExampleViewModel()
        {
            FinishCommand = new DelegateCommand(Finish);
            CancelCommand = new DelegateCommand(Cancel);
            Example = new Example();
        }

        public void Finish(object obj)
        {
            Example example = new Example
            {
                Name = Example.Name,
                Code = Example.Code
            };
            Base.GetSingleton<EventAggregator>().Publish(example, "AddExample");
        }
        public void Cancel(object obj)
        {
            CloseWindow();
        }

        public void CloseWindow()
        {
            Example.Name = "";
            Example.Code = "";
            Base.GetSingleton<NewExampleWindow>().Close();
        }
    }
}
