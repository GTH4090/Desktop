using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSR2023.Models
{
    public partial class Employee
    {
        public int ClosedCount { get => this.Task.Where(el => el.StatusId == 3).Count(); }
        public int DeadLinedCount { get => this.Task.Where(el => el.Deadline <  DateTime.Now).Count(); }
    }
}
