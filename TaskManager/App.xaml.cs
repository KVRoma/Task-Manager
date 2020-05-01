using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TaskManager.ViewModels;
using TaskManager.Views;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();
        //MainViewModel mainViewModel;
        static public TaskbarIcon notifyIcon;

        public App()
        {         
            displayRootRegistry.RegisterWindowType<MainViewModel, MainView>();
            displayRootRegistry.RegisterWindowType<DialogViewModel, DialogView>();
            displayRootRegistry.RegisterWindowType<ReklamaViewModel, ReklamaView>();
            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            //mainViewModel = new MainViewModel();
            //await displayRootRegistry.ShowModalPresentation(mainViewModel);
            //Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose();
            displayRootRegistry.UnregisterWindowType<MainViewModel>();
            displayRootRegistry.UnregisterWindowType<DialogViewModel>();
            displayRootRegistry.UnregisterWindowType<ReklamaViewModel>();
            base.OnExit(e);
        }
    }
}
