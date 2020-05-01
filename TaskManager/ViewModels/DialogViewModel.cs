using System;
using System.Windows;
using TaskManager.Commands;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class DialogViewModel : ViewModel
    {       
        private bool dataEnabled;       
        private Visibility okVisible;
        private Visibility addVisible;
        private bool buttonEnabled;
        private DateTime dateDeadLine;
        private string taskName;
        private string taskComment;
        private Result taskResult;
        private bool taskDelete;
        private string labelText;

        public string DialogName { get; } = "Редактор завдання";
               
        public bool DataEnabled 
        { 
            get { return dataEnabled; }
            set 
            {
                dataEnabled = value;
                OnPropertyChanged(nameof(DataEnabled));
            }
        }       
        public Visibility OkVisible 
        {
            get {return okVisible; }
            set 
            {
                okVisible = value;
                OnPropertyChanged(nameof(OkVisible));
            }
        }
        public Visibility AddVisible 
        {
            get {return addVisible; }
            set 
            {
                addVisible = value;
                OnPropertyChanged(nameof(AddVisible));
            }
        }
        public bool ButtonEnabled
        {
            get { return buttonEnabled; }
            set
            {
                buttonEnabled = value;
                OnPropertyChanged(nameof(ButtonEnabled));
            }
        }
        public DateTime DateDeadLine 
        {
            get {return dateDeadLine; }
            set 
            {
                dateDeadLine = value;
                OnPropertyChanged(nameof(DateDeadLine));
            }
        }
        public string TaskName 
        {
            get { return taskName; }
            set 
            {
                taskName = value;
                OnPropertyChanged(nameof(TaskName));
            }
        }
        public string TaskComment 
        {
            get {return taskComment; }
            set 
            {
                taskComment = value;
                OnPropertyChanged(nameof(TaskComment));
            }
        }
        public Result TaskResult 
        {
            get {return taskResult; }
            set 
            { 
                taskResult = value;
                OnPropertyChanged(nameof(TaskResult));
            }
        }
        public bool TaskDelete 
        {
            get {return taskDelete; }
            set 
            {
                taskDelete = value;
                OnPropertyChanged(nameof(TaskDelete));
            }
        }
        public string LabelText
        {
            get { return labelText; }
            set
            {
                labelText = value;
                OnPropertyChanged(nameof(LabelText));
            }
        }

        public DialogViewModel()
        {
            DataEnabled = false;           
            AddVisible = Visibility.Visible;
            OkVisible = Visibility.Collapsed;
            TaskDelete = false;
            TaskResult = Result.No;
            LabelText = "Результат";
        }

        private Command okCommand;
        private Command deleteCommand;
        private Command addCommand;
        private Command readyCommand;

        public Command OkCommand => okCommand ?? (okCommand = new Command(obj=> 
        {
            TaskResult = Result.Ok;            
            if (obj is System.Windows.Window)
            {
                (obj as System.Windows.Window).Close();
            }
        }));
        public Command DeleteCommand => deleteCommand ?? (deleteCommand = new Command(obj=> 
        {
            TaskDelete = true;
            if (obj is System.Windows.Window)
            {
                (obj as System.Windows.Window).Close();
            }
        }));
        public Command AddCommand => addCommand ?? (addCommand = new Command(obj=> 
        {
            LabelText = "Опис завдання";
            AddVisible = Visibility.Collapsed;
            OkVisible = Visibility.Visible;
            DataEnabled = true;
            DateDeadLine = DateTime.Today;
            TaskName = "";
            TaskComment = "";
        }));
        public Command ReadyCommand => readyCommand ?? (readyCommand = new Command(obj=>
        {
            TaskResult = Result.Add;
            if (obj is System.Windows.Window)
            {
                (obj as System.Windows.Window).Close();
            }
        }));
    }
}
