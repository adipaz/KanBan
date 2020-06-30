using System;
using System.Collections.Generic;

namespace KanbanProject.DataAccessLayer
{
    public class DALColumn
    {
        private string title;
        private List<DALTask> column;

        public DALColumn(string title)
        {
            this.title = title;
            this.column = new List<DALTask>();
        }
        
        public void Add(DALTask task)
        {
            this.column.Add(task);
        }

        public string GetTitle()
        {
            return this.title;
        }

        public List<DALTask> GetColumn()
        {
            return this.column;
        }
        

        
    }
}
