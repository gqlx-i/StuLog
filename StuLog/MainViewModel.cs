using StuLog.Common;
using StuLog.Interface;
using StuLog.Model;
using StuLog.Parameter;
using StuLog.View;
using StuLog.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StuLog
{
    public class MainViewModel : ViewModelBase, IHandle<Example>
    {
        #region Peoperty_Field
        private Example _currentExample;
        public Example CurrentExample
        {
            get => _currentExample;
            set => SetAndNotify(ref _currentExample, value);        
        }

        private ViewModelBase _currentViewModel = new ViewModelBase();
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetAndNotify(ref _currentViewModel, value);
        }

        private ObservableCollection<Example> _examples = new ObservableCollection<Example>();
        public ObservableCollection<Example> Examples
        {
            get => _examples;
            set => SetAndNotify(ref _examples, value);
        }

        public ICommand AddExCommand { get; set; }
        public ICommand DeleteExCommand { get; set; }
        public ICommand EditExCommand { get; set; }
        public ICommand FindExCommand { get; set; }
        public ICommand CopyExCommand { get; set; }
        public ICommand ShowCodeCommand { get; set; }
        #endregion
        private void Init()
        {
            Param = Base.GetParameter<ExampleLibParam>();

            CurrentViewModel = Base.GetSingleton<TestViewModel>();
            
            AddExCommand = new DelegateCommand(AddEx);
            DeleteExCommand = new DelegateCommand(DeleteEx);
            EditExCommand = new DelegateCommand(EditEx);
            FindExCommand = new DelegateCommand(FindEx);
            CopyExCommand = new DelegateCommand(CopyEx);
            ShowCodeCommand = new DelegateCommand(ShowCode);

            #region Test

            #endregion
            CurrentExample = Param.Item.Examples.FirstOrDefault();

            newExampleWindow = Base.GetSingleton<NewExampleWindow>();
            newExampleWindow.Closing -= NewExampleWindow_Closing;
            newExampleWindow.Closing += NewExampleWindow_Closing;

            Base.GetSingleton<EventAggregator>().Subscribe(this, "AddExample");
        }
        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }

        private ExampleLibParam _param = new ExampleLibParam();
        public ExampleLibParam Param
        {
            get => _param;
            set => SetAndNotify(ref _param, value);
        }

        NewExampleWindow newExampleWindow;
        public MainViewModel()
        {
            IoC ioc = new IoC();
            Type[] types = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "StuLog.View");
            foreach (var t in types)
            {
                //Type.GetType(t.Name,true,t.)
               // t.AssemblyQualifiedName;
            }
            Init();
        }
        public void ShowCode(object obj)
        {
            CurrentExample = Param.Item.Examples.Find((w) => w.Name == obj.ToString());
        }
        public void AddEx(object obj)
        {
            newExampleWindow.Show();
        }

        public void DeleteEx(object obj)
        {
            var res = MessageBox.Show($"{CurrentExample.Name}", "是否删除当前例子？", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                var example = Param.Item.Examples.Find((w) => w.Name == CurrentExample.Name);
                var b = Param.Item.Examples.Remove(example);
            }
            Param.Write();
        }
        public void EditEx(object obj)
        {
            newExampleWindow.Show();
        }
        public void FindEx(object obj)
        {
            Param.Item.Examples.Find((w) => w.Name == obj.ToString());
        }
        public void CopyEx(object obj)
        {
            Clipboard.SetText(CurrentExample.Code);
        }

        private void NewExampleWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            NewExampleWindow win = sender as NewExampleWindow;
            win.Hide();
        }

        public void Handle(Example example)
        {
            if (Param.Item.Examples.Find((w) => w.Name == example.Name) != null)
            {
                MessageBox.Show("例子名称重复");
                return;
            }
            else
            {
                Param.Item.Examples.Add(example);
                Param.Write();
                newExampleWindow.Close();
            }
        }
    }
}
