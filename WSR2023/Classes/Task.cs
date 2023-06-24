using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSR2023.Models
{
    public partial class Task
    {
        public int SortNum { get
            {
                if(this.Deadline <= DateTime.Now && this.StatusId==2)
                {
                    return 1;
                }
                if (this.Deadline <= DateTime.Now && this.StatusId == 1)
                {
                    return 2;
                }
                if (this.Deadline > DateTime.Now && this.StatusId == 2)
                {
                    return 3;
                }
                if (this.Deadline > DateTime.Now && this.StatusId == 1)
                {
                    return 4;
                }
                else
                {
                    return 5;
                }

            } }
        public string DisplayColor { get
            {
                if(this.Deadline > DateTime.Now)
                {
                    return this.TaskStatus.ColorHex;
                }
                else
                {
                    return "#202020";
                }
            } }
        
    }
}
