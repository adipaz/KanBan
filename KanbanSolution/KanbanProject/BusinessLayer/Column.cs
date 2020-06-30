using KanbanProject.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KanbanProject.BusinessLayer
{
    public class Column
    {
        private string colName;

        private List<Task> tasksList;

        private bool isLimitedList = false;
        private int lim;

        private bool leftestColumn = false;
        private bool rightestColumn = false;


        // Private attributes for update - Data access layer 
        private DataBase dataBase;

        public Column(string columnName) // Ctor
        {
            this.colName = columnName;
            tasksList = new List<Task>();
        }

        public string GetColumnName()
        {
            return this.colName;
        }

        public List<Task> GetTasks()
        {
            return this.tasksList;
        }

        public DataBase GetDataBase()
        {
            return this.dataBase;
        }

        public void SetDataBase(DataBase dataBase)
        {
            this.dataBase = dataBase;
        }

        public int GetLim()
        {
            return this.lim;
        }

        public bool GetIsLimited()
        {
            return this.isLimitedList;
        }

        public bool GetIsleftest()
        {
            return this.leftestColumn;
        }

        public bool GetIsRightet()
        {
            return this.rightestColumn;
        }

        public void SetLeftest(bool leftestColumn)
        {
            this.leftestColumn = leftestColumn;
        }

        public void SetRightest(bool rightestColumn)
        {
            this.rightestColumn = rightestColumn;
        }

        public void SetIsLimited(bool isLimited)
        {
            this.isLimitedList = isLimited;
        }

        /// <summary>
        /// This function is responsible for creating a new task 
        /// </summary>
        /// <param name="creationTime"> Creation time attribute of Task </param>
        /// <param name="dueDate"> Due date attribute of Task </param>
        /// <param name="title"> Title attribute of Task </param>
        /// <param name="description"> Description attribute of Task </param>
        public bool CreateTask(DateTime creationTime, DateTime dueDate, String title, String description,string boardName,string email)
        {
            /*
             * First and foremost - check if the addition made to leftest column ! 
             * 2 cases of addition : 1. The list is not limited - add Task straight to leftest colmumn.
             *                       2. The list is limited - the adding is priored by check.
             */
            if (this.leftestColumn &&
               ((!this.isLimitedList) || (this.isLimitedList && this.tasksList.Count < this.lim)))
            {
                Task t = new Task(dueDate, title, description, creationTime, this.colName);
                this.AddTaskToColumn(t, 1,boardName,email);
                return true;
            }
            else
            {
                Logger.GetLogger().Error("Creating Task Failed!! " + title);
                return false;
            }
        }

        /// <summary>
        /// This function is reponsible for adding Task instance to a column in the Board restor proccess(Login)
        /// </summary>
        /// <param name="task"> Task to be added to this column </param>
        public void AddTaskToColumnRestore(Task task)
        {
            this.tasksList.Add(task);
        }

        /// <summary>
        ///  This function is reponsible for adding Task instance to a column 
        /// </summary>
        /// <param name="task"> Task to be added to this column </param>
        public void AddTaskToColumn(Task task, int columnNumber,string boardName,string email)
        {
            if (this.rightestColumn) { task.SetInRightest(true); }
            this.tasksList.Add(task);

            // Update data
            this.dataBase.AddTask(email, boardName,Convert.ToString(task.GetCreationTime()),Convert.ToString( task.GetDueDate()), task.GetState(), task.GetTitle(), task.GetDescription());
        }

        /// <summary>
        /// This function is responsible for removing a task from this column 
        /// </summary>
        /// <param name="task"> Task to be removed from this column </param>
        public void RemoveTask(Task task, int columnNumber,string boardName,string email)
        {
            this.tasksList.Remove(tasksList.Find(x => x.GetTitle().Equals(task.GetTitle())));

            // Update data
            this.dataBase.RemoveTask(email, boardName, Convert.ToString(task.GetCreationTime()), Convert.ToString(task.GetDueDate()), this.colName, task.GetTitle(), task.GetDescription());
        }

        /// <summary>
        /// This function is responsible for limiting the number of task in this certain column
        /// </summary>
        /// <param name="limitNum"> string representing the number of Tasks in this column, sent from GUI </param>
        public void Limit(int limitNum)
        {
            this.isLimitedList = true; // The list is limited from now on

            if (this.tasksList.Count > limitNum) // List size is bigger than wanted limitation
            {
                Logger.GetLogger().Error("Wanted limitation can't be obatained!!(ammount of tasks is bigger than desired limit)");
                return;
            }
            else
            {
                this.lim = limitNum;
                List<Task> limitedTasksList = new List<Task>(this.lim); // Create new list with capacity limit 
                foreach (Task t in this.tasksList) // Copy content of old list to new limited one 
                {
                    limitedTasksList.Add(t);
                }
                this.tasksList = limitedTasksList;
            }
        }


        public void SortByCreationTime()
        {
            this.tasksList.OrderBy(x => ((Task)x).GetCreationTime());
        }

        public void RemoveByCreationTime(string creationTime, int colNum,string boardName,string email)
        {
            foreach(Task t in this.tasksList)
            {
                if (t.GetCreationTime().ToString().Equals(creationTime))
                {
                    Task oldTask = t;
                    this.RemoveTask(oldTask, colNum,boardName,email);
                    break;
                }
            }
        }
    }
}
