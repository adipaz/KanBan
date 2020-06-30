using System;

namespace KanbanProject.DataAccessLayer
{
    public class DALTask
    {
        private string creationDate;
        private string dueDate;
        private string title;
        private string description;


        //constructor
        public DALTask(string creationDate, string dueDate, string title, string description)
        {
            this.creationDate = creationDate;
            this.dueDate = dueDate;
            this.title = title;
            this.description = description;
        }

        //getters

        /// <returns>creation date</returns>
        public string GetCreationDate() { return this.creationDate; }
        /// <returns>due date</returns>
        public string GetDueDate() { return this.dueDate; }
        /// <returns>title</returns>
        public string GetTitle() { return this.title; }
        /// <returns>description</returns>
        public string GetDescription() { return this.description; }

        

    }
}
