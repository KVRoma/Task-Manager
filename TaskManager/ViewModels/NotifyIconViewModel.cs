using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using TaskManager.Commands;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class NotifyIconViewModel : ViewModel
    {
        DisplayRootRegistry displayRootRegistry = (Application.Current as App).displayRootRegistry;
        MainViewModel mainViewModel; // = new MainViewModel();
        TaskManagerContext db;        

        private DispatcherTimer timer;
       
        private string infoToDay;
        private string infoNextDay;
        private string infoWarning;
        private string info;
        private int countHours;

        public string Timestamp
        {
            get 
            {                
                return DateTime.Now.ToLongTimeString(); 
            }
        }

        public string InfoToDay
        {
            get { return infoToDay; }
            set
            {
                infoToDay = value;
                OnPropertyChanged(nameof(InfoToDay));
            }
        }

        public string InfoNextDay
        {
            get { return infoNextDay; }
            set
            {
                infoNextDay = value;
                OnPropertyChanged(nameof(InfoNextDay));
            }
        }

        public string InfoWarning
        {
            get { return infoWarning; }
            set
            {
                infoWarning = value;
                OnPropertyChanged(nameof(InfoWarning));
            }
        }

        public NotifyIconViewModel()
        {            
            timer = new DispatcherTimer(TimeSpan.FromHours(1), DispatcherPriority.Normal, OnTimerTick, Application.Current.Dispatcher);
            db = new TaskManagerContext();
            db.Tasks.Load();
            mainViewModel = new MainViewModel(db);
            countHours = 2;
        }



        private void OnTimerTick(object sender, EventArgs e)
        {
            countHours++;
            InfoToDay = "";
            InfoNextDay = "";
            InfoWarning = "";
            info = "";
            Application.Current.Dispatcher.BeginInvoke(new Action(() => OnPropertyChanged(nameof(Timestamp))));
            var firstDate = db.Tasks.Where(i => i.TaskResult == Result.No).OrderBy(d=>d.DateDeadLine);
            foreach (var item in firstDate)
            {
                
                if (DateTime.Today.Subtract(item.DateDeadLine).Days == 0)
                {
                    InfoToDay += "Сьогодні термін: " + item.TaskName + Environment.NewLine;
                }
                if (DateTime.Today.Subtract(item.DateDeadLine).Days == -1)
                {
                    InfoNextDay += "Виконати завтра: " + item.TaskName + Environment.NewLine;
                }
                if (DateTime.Today.Subtract(item.DateDeadLine).Days > 0)
                {
                    InfoWarning += "ВИЙШОВ ТЕРМІН !!!: " + item.TaskName + Environment.NewLine;
                }
            }

           info = InfoWarning;

            if (countHours == 3)
            {
                info += InfoToDay + InfoNextDay;
                countHours = 0;
            }
            App.notifyIcon.ShowBalloonTip("Нагадування !!!",info, BalloonIcon.Info);
        }

        private Command _showWindowCommand;
        private Command _hideWindowCommand;
        private Command _exitAppCommand;


        public Command ShowWindowCommand => _showWindowCommand ?? (_showWindowCommand = new Command(obj => 
        {
            try
            {
                displayRootRegistry.ShowPresentation(mainViewModel);
            }
            catch 
            {               
            }
        }));

        public Command HideWindowCommand => _hideWindowCommand ?? (_hideWindowCommand = new Command(obj => 
        {
            try
            {
                displayRootRegistry.HidePresentation(mainViewModel);
            }
            catch
            {               
            }
        }));

        public Command ExitAppCommand => _exitAppCommand ?? (_exitAppCommand = new Command(obj => 
        {
            Application.Current.Shutdown();
        }));
    }
}
