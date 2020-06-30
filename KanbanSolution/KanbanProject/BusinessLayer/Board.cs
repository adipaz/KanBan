using KanbanProject.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KanbanProject.BusinessLayer
{
    public class Board
    {
        private Dictionary<int, Column> columns;

        // Private attributes for update - Data access layer 
        private DataBase dataBase;
        private int count;

        public Board() // Ctor 
        {
            
            this.columns = new Dictionary<int, Column>();
            this.count = this.columns.Count;
        }

        public void SetDataBase(DataBase dataBase)
        {
            this.dataBase = dataBase;
        }

        public Column GetLeftest() // Return leftest ( first ) column 
        {
            foreach (KeyValuePair<int, Column> pair in this.columns)
                if (pair.Value.GetIsleftest())
                    return pair.Value;
            return null;
        }

        public Column GetRightest() // Return rightest ( last ) column
        {
            foreach (KeyValuePair<int, Column> pair in this.columns)
                if (pair.Value.GetIsRightet())
                    return pair.Value;
            return null;
        }

        public Dictionary<int, Column> GetDictionary() // return dictionarty of columns 
        {
            return this.columns;
        }

        public Column GetColumnByIndex(int index) // Return column of dictionary by index
        {
            return this.columns[index];
        }

        public Column GetColumnByName(string name) // Return column of dictionary by column name
        {
            int currentIndex = this.columns.FirstOrDefault(x => ((Column)x.Value).GetColumnName().Equals(name)).Key;
            return GetColumnByIndex(currentIndex);
        }

        public int GetIndexOfColumn(Column c) // Return index of column in dictionary by column object 
        {
            int index = this.columns.FirstOrDefault(x => ((Column)x.Value).GetColumnName().Equals(c.GetColumnName())).Key;
            return index;
        }

        public int GetIndexByColumnName(string columnName) // Return index of column by column name 
        {
            int index = this.columns.FirstOrDefault(x => ((Column)x.Value).GetColumnName().Equals(columnName)).Key;
            return index;
        }

        public int GetCount() // number of columns 
        {
            return this.count;
        }

        public int GetColumnKey(string columnName) // Return column key by column name 
        {
            return this.columns.FirstOrDefault(x => ((Column)x.Value).GetColumnName().Equals(columnName)).Key;
        }


        /// <summary>
        /// This function is responsible for changing a state of a given Task 
        /// </summary>
        /// <param name="task"> Task instance to be moved </param>
        public bool ChangeState(Task task,string boardName,string email)
        {
            string currCol = task.GetState(); // string that represents current state or column, for example : "backlog"
            // Get index of current column
            int currentIndex = this.columns.FirstOrDefault(x => ((Column)x.Value).GetColumnName().Equals(currCol)).Key;
            Column currentCol = this.columns[currentIndex];

            if (!(currentIndex == this.columns.Keys.Last()))  // Checking whether the state changing occurs in the next column 
            {
                int movingIndex = currentIndex + 1;
                Column movingCol = this.columns[movingIndex];

                if ((movingCol.GetIsLimited() && movingCol.GetTasks().Count < movingCol.GetLim()) || (!movingCol.GetIsLimited()))
                {
                    task.SetState(movingCol.GetColumnName());
                    movingCol.AddTaskToColumn(task, movingIndex,boardName,email);
                    currentCol.RemoveTask(task, currentIndex,boardName,email);
                    this.dataBase.MoveTask(email, boardName, Convert.ToString(task.GetCreationTime()), Convert.ToString(task.GetDueDate()), currCol, task.GetTitle(), task.GetDescription(), movingCol.GetColumnName());
                    return true;
                }
                else
                {
                    Logger.GetLogger().Error("changing state couldn't been preformed.Limitation error!!");
                    return false;
                }
            }
            else
            {
                Logger.GetLogger().Error("Can't move task from last column");
                return false;
            }
        }

        /// <summary>
        /// The function is responsible for switching 2 columns
        /// </summary>
        /// <param name="toSwitch"> Column to switch </param>
        /// <param name="toSwitchIndex"> Index to switch </param>
        public void SwitchColumns(Column toSwitch, int toSwitchIndex,string boardName,string email)
        {
            Column tmp = this.columns[toSwitchIndex];
            int currentIndex = this.columns.FirstOrDefault(x => ((Column)x.Value).GetColumnName().Equals(toSwitch.GetColumnName())).Key;
            this.columns[toSwitchIndex] = toSwitch;
            this.columns[currentIndex] = tmp;
            if (currentIndex == 1) { tmp.SetLeftest(true); toSwitch.SetLeftest(false); }
            if (currentIndex == this.columns.Count) { tmp.SetRightest(true); toSwitch.SetRightest(false); }
            if (toSwitchIndex == 1) { tmp.SetLeftest(false); toSwitch.SetLeftest(true); }
            if (toSwitchIndex == this.columns.Count) { tmp.SetRightest(false); toSwitch.SetRightest(true); }

            // Update data
            this.dataBase.SwitchColumns(email, boardName, toSwitch.GetColumnName(), tmp.GetColumnName());
        }

        /// <summary>
        /// Function adds column to dictionary of columns 
        /// </summary>
        /// <param name="c"> Column to add </param>
        /// <returns> Successfull addition ? true : false </returns>
        public bool AddColumn(Column c,string boardName,string email)
        {
            if (c.GetColumnName() == "" || c.GetColumnName() == null) // empty string column or null 
            {
                Logger.GetLogger().Error("Invalid name of column");
                return false;
            }
            else if (this.columns.FirstOrDefault(x => ((Column)x.Value).GetColumnName().Equals(c.GetColumnName())).Value != null)
            {
                // Column with the same name is already exists
                Logger.GetLogger().Error("Board already contains this column");
                return false;
            }
            else
            {
                c.SetDataBase(this.dataBase);
                if (this.columns.Count > 0)
                {
                    this.columns[this.columns.Count].SetRightest(false);
                }
                if (this.columns.Count == 0)
                {
                    c.SetLeftest(true);
                }
                this.columns.Add(this.columns.Count + 1, c);
                c.SetRightest(true);
                this.count = this.columns.Count;

                // Update data
                this.dataBase.AddColumn(email, boardName, c.GetColumnName());
                return true;
            }
        }

        public void AddColumnToBoardRestore(Column c) // Addition of column restoring board after login
        {
            if (this.columns.Count > 0)
                this.columns[this.columns.Count].SetRightest(false);
            this.columns.Add(this.columns.Count + 1, c);
            c.SetRightest(true);
            this.count = this.columns.Count;
        }

        /// <summary>
        /// Fuction removes column from dictionary 
        /// </summary>
        /// <param name="c"> Column to remove </param>
        public void RemoveColumn(Column c,string boardName,string email)
        {
            if (this.columns.Count > 0)
            {
                int currentIndex = this.columns.FirstOrDefault(x => ((Column)x.Value).GetColumnName().Equals(c.GetColumnName())).Key;
                this.columns[1].SetLeftest(false);
                this.columns[this.columns.Count].SetRightest(false);


                for (int i = currentIndex; i < this.columns.Count; i++) // move all columns after current index to close gap
                {
                    this.columns[i] = this.columns[i + 1];
                }

                this.columns.Remove(this.columns.Count);

                if (this.columns.Count > 0)
                {
                    this.columns[1].SetLeftest(true);
                    this.columns[this.columns.Count].SetRightest(true);
                }
                this.count = this.columns.Count;

                // Update data
                this.dataBase.RemoveColumn(email, boardName, c.GetColumnName());

            }
            else
            {
                Logger.GetLogger().Error("There are no more columns in the board");

            }
        }

        public bool IsEmpty() // Returns boolen statement which declares whether the dictionary of columns is empty 
        {
            return this.columns.Count == 0;
        }
    }
}
