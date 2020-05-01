using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using TaskManager.Commands;
using TaskManager.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace TaskManager.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private TaskManagerContext db;
        private string textDate;
        private Models.Task taskSelect;
        private IEnumerable<Models.Task> tasks;
        
        public string NameWindow { get; } = "Task Manager";
        public string Autor { get; } =  " © <Kuchinik & Co.>, 2019";


        public string TextDate
        {
            get { return textDate; }
            set
            {
                textDate = value;
                OnPropertyChanged(nameof(TextDate));
            }
        }
        public Models.Task TaskSelect
        {
            get { return taskSelect; }
            set
            {
                taskSelect = value;
                OnPropertyChanged(nameof(TaskSelect));
            }
        }
        public IEnumerable<Models.Task> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }


        private Command _openDialogCommand;
        private Command _reklamaCommand;
        private Command _exportExcelCommand;
        private Command _hideCommand;

        public Command OpenDialogCommand => _openDialogCommand ?? (_openDialogCommand = new Command(async obj =>
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            var dialogViewModel = new DialogViewModel
            {
                DateDeadLine = TaskSelect?.DateDeadLine ?? DateTime.Today,
                TaskName = TaskSelect?.TaskName ?? null,
                TaskComment = "",
                ButtonEnabled = (TaskSelect != null) ? true : false
            };
            await displayRootRegistry.ShowModalPresentation(dialogViewModel);

            if (dialogViewModel.TaskDelete)
            {
                db.Tasks.Remove(TaskSelect);
                db.SaveChanges();
            }
            if (dialogViewModel.TaskResult == Result.Add)
            {
                Models.Task item = new Models.Task()
                {
                    DateCreation = DateTime.Today,
                    DateDeadLine = dialogViewModel.DateDeadLine,
                    TaskName = dialogViewModel.TaskName,
                    TaskComment = dialogViewModel.TaskComment,
                    TaskResult = Result.No
                };
                db.Tasks.Add(item);
                db.SaveChanges();
            }
            if (dialogViewModel.TaskResult == Result.Ok)
            {
                TaskSelect.TaskResult = dialogViewModel.TaskResult;
                if (dialogViewModel.TaskComment != "")
                {
                    TaskSelect.TaskComment += Environment.NewLine + "Коментар про виконання:" + Environment.NewLine + dialogViewModel.TaskComment;
                }
                TaskSelect.DateCreation = DateTime.Today;
                db.Entry(TaskSelect).State = EntityState.Modified;
                db.SaveChanges();
            }
            Tasks = db.Tasks.Local.ToBindingList().Where(r=>r.TaskResult == Result.No).OrderBy(t => t.DateDeadLine);
        }));

        public Command ReklamaCommand => _reklamaCommand ?? (_reklamaCommand = new Command(async obj => 
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            var reklamaViewModel = new ReklamaViewModel();
            await displayRootRegistry.ShowModalPresentation(reklamaViewModel);
        }));

        public Command HideCommand => _hideCommand ?? (_hideCommand = new Command(obj => 
        {
            try
            {
                var displayRootRegistry = (Application.Current as App).displayRootRegistry;
                displayRootRegistry.HidePresentation(this);
            }
            catch
            {
            }
        }));

        public Command ExportExcelCommand => _exportExcelCommand ?? (_exportExcelCommand = new Command(obj => 
        {
            var task = db.Tasks.Where(a => a.Id > 0).OrderBy(t=>t.DateDeadLine);             // вибираємо всі завдання
            string[,] result = new string[task.Count()+1, 6];                                  // створюємо двовимірний масив

            result[0, 0] = "Id";
            result[0, 1] = "Дата створення, або фактичного виконання";
            result[0, 2] = "Дата необхідного виконання";
            result[0, 3] = "Назва";
            result[0, 4] = "Текст завдання";
            result[0, 5] = "Результат";
            int count = 1;


            foreach (var item in task)
            {                
                result[count, 0] = item.Id.ToString();
                result[count, 1] = item.DateCreation.ToShortDateString();
                result[count, 2] = item.DateDeadLine.ToShortDateString();
                result[count, 3] = item.TaskName;
                result[count, 4] = item.TaskComment;
                result[count, 5] = ((Result)item.TaskResult).ToString();               
                count++;
            }

            Excel.Application ExcelApp = new Excel.Application();     // Створюємо додаток Excel
            Excel.Workbook ExcelWorkBook;                             // Створюємо книгу Excel
            Excel.Worksheet ExcelWorkSheet;                           // Створюємо лист Excel
            Excel.Range range;                                        // Створюємо діапазон Excel
            Excel.Range start;                                        // Створюємо діапазон для передачі початкової позиції
            Excel.Range end;                                          // Створюємо діапазон для передачі кінцевої позиції

            try
            {
                ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);                  // Створюємо новий файл Excel                
                ExcelWorkSheet = (Excel.Worksheet)ExcelWorkBook.Sheets[1];                                // Створюємо новий Лист Excel

                start = (Excel.Range)ExcelWorkSheet.Cells[1, 1];                                           // Вказуємо початкову позицію
                end = (Excel.Range)ExcelWorkSheet.Cells[result.GetLength(0), result.GetLength(1)];         // Вказуємо кінцеву позицію
                range = ExcelWorkSheet.get_Range(start, end);                                              // Виділяємо діапазон                
                range.Value = result;                                                                       // виводим наш масив в діапазон        

                ExcelApp.Visible = true;            // Робим Excel видимим, щоб користувач міг його закрити
                ExcelApp.UserControl = true;        // Передаємо керування користувачу
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }));

        public MainViewModel(TaskManagerContext context)
        {
            db = context;
            Tasks = db.Tasks.Local.ToBindingList().Where(r=>r.TaskResult == Result.No).OrderBy(t=>t.DateDeadLine);            
        }

        
    }
}
