using StuLog.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StuLog.ViewModel
{
    public class TestViewModel : ViewModelBase
    {
        private string _name = "test";
        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }

        public ICommand Add1 { get; set; }
        public ICommand Add2 { get; set; }
        public ICommand Add3 { get; set; }
        public TestViewModel()
        {
            Init();
            Add1 = new DelegateCommand(Ad1);
            Add2 = new DelegateCommand(Ad2);
            Add3 = new DelegateCommand(Ad3);
        }

        private void Ad1(object obj)
        {
            A.Add(new A() { X = 1, Y = 1 });
        }
        private void Ad2(object obj)
        {
            if (SelectedA.A1 == null)
            {
                SelectedA.A1 = new ObservableCollection<Point>();
            }
            SelectedA.A1.Add(new Point(1, 1));
        }
        private void Ad3(object obj)
        {
            if (SelectedA.A2 == null)
            {
                SelectedA.A2 = new ObservableCollection<Point>();
            }
            SelectedA.A2.Add(new Point(2, 2));
        }

        public void Init()
        {
            if (A == null)
            {
                A = new ObservableCollection<A>();
                A.Add(new A()
                {
                    X = 0,
                    Y = 0,
                    A1 = new ObservableCollection<Point>()
                        {
                            new Point(1,1),
                            new Point(2,2),
                            new Point(3,3)
                        },
                    A2 = new ObservableCollection<Point>()
                        {
                            new Point(4,4),
                            new Point(5,5),
                            new Point(6,6)
                        },
                    A12B = new ObservableCollection<B>()
                        {
                            new B(),
                            new B(),
                            new B(),
                        }
                });
            }
        }

        private ObservableCollection<A> a;
        public ObservableCollection<A> A
        {
            get => a;
            set { SetAndNotify(ref a, value); }
        }
        private A selectedA;
        public A SelectedA
        {
            get => selectedA;
            set { SetAndNotify(ref selectedA, value, nameof(SelectedA)); }
        }
    }

    public class A : PropertyNotifyBase
    {
        private int x;
        public int X
        {
            get => x;
            set
            {
                SetAndNotify(ref x, value);
            }
        }
        private int y;
        public int Y
        {
            get => y;
            set
            {
                SetAndNotify(ref y, value);
            }
        }
        private ObservableCollection<Point> a1;
        public ObservableCollection<Point> A1
        {
            get => a1;
            set { SetAndNotify(ref a1, value); }
        }
        private Point selectedA1;
        public Point SelectedA1
        {
            get => selectedA1;
            set { SetAndNotify(ref selectedA1, value); Update(); }
        }
        private ObservableCollection<Point> a2;
        public ObservableCollection<Point> A2
        {
            get => a2;
            set { SetAndNotify(ref a2, value); }
        }
        private Point selectedA2;
        public Point SelectedA2
        {
            get => selectedA2;
            set { SetAndNotify(ref selectedA2, value); Update(); }
        }
        private ObservableCollection<B> a12b;
        public ObservableCollection<B> A12B
        {
            get => a12b;
            set { SetAndNotify(ref a12b, value); }
        }
        private B selectedA12B;
        public B SelectedA12B
        {
            get => selectedA12B;
            set { SetAndNotify(ref selectedA12B, value); }
        }
        public void Update()
        {
            var temp = Base.GetSingleton<TestViewModel>().SelectedA;
            if (temp.A12B == null)
            {
                temp.A12B = new ObservableCollection<B>();
            }
            var x = temp.A12B.Find((w)=>w.X == temp.SelectedA1.X && w.Y == temp.SelectedA2.Y);
            if (x == null)
            {
                if (temp.SelectedA1 != null && temp.SelectedA2 != null)
                {
                    x = new B() { X = temp.SelectedA1.X, Y = temp.SelectedA2.Y };
                    temp.A12B.Add(x);
                }
            }
            else
            {
                temp.SelectedA12B = x;
                temp.SelectedA12B.AB = new ObservableCollection<Point>();
                temp.SelectedA12B.AB.Add(new Point() { X = temp.SelectedA2.X * 2, Y = temp.SelectedA1.Y * 2 });
                temp.SelectedA12B.AB.Add(new Point() { X = temp.SelectedA2.X * 4, Y = temp.SelectedA1.Y * 4 });
                temp.SelectedA12B.AB.Add(new Point() { X = temp.SelectedA2.X * 6, Y = temp.SelectedA1.Y * 6 });
            }
        }
    }
    public class B : PropertyNotifyBase
    {
        private int x;
        public int X
        {
            get => x;
            set
            {
                SetAndNotify(ref x, value);
            }
        }
        private int y;
        public int Y
        {
            get => y;
            set
            {
                SetAndNotify(ref y, value);
            }
        }
        private ObservableCollection<Point> ab;
        public ObservableCollection<Point> AB
        {
            get => ab;
            set { SetAndNotify(ref ab, value); }
        }
    }
    public class Point : PropertyNotifyBase
    {
        private int x;
        public int X
        {
            get => x;
            set
            {
                SetAndNotify(ref x, value);
            }
        }
        private int y;
        public int Y
        {
            get => y;
            set
            {
                SetAndNotify(ref y, value);
            }
        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Point()
        {

        }
    }
}
