using System;

namespace KanbanProject.BusinessLayer
{
    public class Task
    {
        private DateTime creationTime;
        private DateTime dueDate;
        private string title;
        private string description;
        private string state;
        private bool isInRightest = false;

        private const int TITLE_MAX_LENGTH = 50;
        private const int DESCRIPTION_MAX_LENGTH = 300;


        public Task(DateTime dueDate, string title, string description, DateTime creationTime, string state) // Ctor 
        {
            this.dueDate = dueDate;
            this.title = title;
            this.description = description;
            this.creationTime = creationTime;
            this.state = state;
        }

        public DateTime GetCreationTime()
        {
            return creationTime;
        }

        public DateTime GetDueDate()
        {
            return dueDate;

        }

        public string GetTitle()
        {
            return title;
        }

        public string GetDescription()
        {
            return description;
        }

        public string GetState()
        {
            return state;
        }

        public bool GetInRightest()
        {
            return this.isInRightest;
        }

        public void SetInRightest(bool inRightest)
        {
            this.isInRightest = inRightest;
        }

        public void SetDueDate(DateTime dueDate)
        {
            if (!(this.isInRightest))
            {
                this.dueDate = dueDate;
            }
            else
            {
                Logger.GetLogger().Error("Task in done column can't be changed by the user!");
            }
        }

        public void SetTitle(string title)
        {
            if (!(this.isInRightest))

            {
                this.title = title;
            }
            else
            {
                Logger.GetLogger().Error("Task in done column can't be changed by the user!");
            }
        }

        public void SetDescription(string description)
        {
            if (!(this.isInRightest))

            {
                this.description = description;
            }
            else
            {
                Logger.GetLogger().Error("Task in done column can't be changed by the user!");
            }
        }

        public void SetState(string state)
        {
            if (!(this.isInRightest))
            {
                this.state = state;
            }
            else
            {
                Logger.GetLogger().Error("Task in done column can't be changed by the user!");
            }
        }

        /// <summary>
        /// This function is responsible for checking if the Task title is valid 
        /// </summary>
        /// <param name="title"> Task title string </param>
        /// <returns> True - title is valid, False otherwise </returns>
        public static bool IsValidTitle(string title)
        {
            if (title == null)
            {
                Logger.GetLogger().Error("Title can't be null"); return false;
            }
            else if (title.Length > 0 && title.Length <= TITLE_MAX_LENGTH) // A title (max. 50 characters, not empty)
                return true;

            else { Logger.GetLogger().Error("Title length must be between 1-50"); return false; }
        }

        /// <summary>
        /// This function is responsible for checking if the Task description is valid 
        /// </summary>
        /// <param name="des"> Task description string </param>
        /// <returns> True - description is valid, False otherwise </returns>
        public static bool IsValidDescription(string des)
        {
            if (des == null)
            {
                Logger.GetLogger().Error("Description can't be null"); return false;
            }
            else if (des.Length <= DESCRIPTION_MAX_LENGTH && des.Length > 0) // A description(max. 300 characters, optional)
                return true;

            else { Logger.GetLogger().Error("Descrition length must be between 0-300"); return false; }
        }

    }
}
