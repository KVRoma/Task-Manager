using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class Task
    {
        public int Id { get; set; }
        public DateTime DateDeadLine { get; set; }
        public DateTime DateCreation { get; set; }
        public string TaskName { get; set; }
        public string TaskComment { get; set; }
        public Result TaskResult { get; set; }
        public string TextDateDeadLine
        {
            get { return DateDeadLine.ToShortDateString(); }
        }
    }
}
