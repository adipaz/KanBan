using KanbanProject.BusinessLayer;
using KanbanProject.InterfaceLayer.DataObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace KanbanProject.InterfaceLayer
{
    public class UserController
    {
        // Public attributes for testing purpose 
        private User user;



        public Board GetUserBoards(User user,string boardName)
        {
            return user.GetBoard(boardName);
        }

        public User GetUser()
        {
            return this.user;
        }

        /// <summary>
        /// This function is responsible for the login proccess, connects between the GUI(presentation layer) and business/logic layer 
        /// </summary>
        /// <param name="email"> email of the user, inserted in the GUI </param>
        /// <param name="password"> password of the user, inserted in the GUI </param>
        /// <returns> The function returns a User object created if the login details are correct, null otherwise </returns>
        public User Login(string email, string password) // Factory function 
        {
            if (User.IsValidLogin(email, password) && User.IsExist(email, password))
            {
                Logger.GetLogger().Debug("successful login : " + email);

                // Login  
                this.user = new User(email, password);
                user.Login(); // Delegate to User class method 
                return user; // Return created user
            }
            else
            {
                Logger.GetLogger().Error("unsuccessful login");
                return null;
            }
        }

        /// <summary>
        /// This function is responsible for the registration proccess, connects between the GUI(presentation layer) and business/logic layer
        /// </summary>
        /// <param name="email"> email of the user, inserted in the GUI </param>
        /// <param name="password"> password of the user, inserted in the GUI </param>
        /// <returns> The function returns a User object created if the login details are valid, null otherwise </returns>
        public User Register(string email, string password)
        {
            if (User.IsValidRegistration(email, password) && (!(User.IsExist(email, password))))
            {
                Logger.GetLogger().Debug("successful registration : " + email);

                // Create and register new user in System 
                this.user = new User(email, password);
                user.Register();
                return user;
            }
            else
            {
                Logger.GetLogger().Error("unsuccessful registration : " + email);
                return null;
            }
        }

        /// <summary>
        /// This function is responsible for the Logout proccess
        /// /// </summary>
        /// <param name="user"> Current user that uses the system, to be logged out </param>
        public static void Logout(User user)
        {
            user.Logout();
        }

        /// <summary>
        /// This function is responsible for creating a new task, connects between the GUI(presentation layer) and business/logic layer
        /// </summary>
        /// <param name="dueDate"> Due date attribute of Task instance </param>
        /// <param name="creationTime"> Creation time attribute of Task instance </param>
        /// <param name="title"> Title attribute of Task instance </param>
        /// <param name="description"> Description attribute of Task instance </param>
        /// <param name="u"> Current user </param>
        public string CreateTask(string dueDate, string title, string description,string boardName)
        {
            if (!this.IsBoardEmpty(boardName))
            {

                if (Task.IsValidTitle(title) && Task.IsValidDescription(description))
                {
                    DateTime now = DateTime.Now;
                    string state = this.user.GetBoard(boardName).GetLeftest().GetColumnName();
                    //this.user.GetBoard().GetLeftest().AddTaskToColumn(new Task(Convert.ToDateTime(dueDate),title,description,now,state),this.GetUser().GetBoard().GetIndexOfColumn(this.user.GetBoard().GetLeftest()) );
                    if (this.user.GetBoard(boardName).GetLeftest().CreateTask(now, Convert.ToDateTime(dueDate), title, description,boardName,this.user.GetEmail()))
                    {
                        return "";
                    }
                    else
                    {
                        Logger.GetLogger().Error("Creating task Failed");
                        return "limit";
                    }

                }
                else
                {
                    Logger.GetLogger().Error("Invalid task details");
                    return "details";
                }
            }
            else
            {
                Logger.GetLogger().Error("Board is Empty");
                return "empty";
            }
        }




        /// <summary>
        /// This function is responsible for removing a task, connects between the GUI(presentation layer) and business/logic layer
        /// </summary>
        /// <param name="task"> Task object to be removed </param>
        /// <param name="u"> Current user </param>
        public void RemoveTask(InterfaceLayerTask task,string boardName)
        {
            string column = task.State; // Figure out column 
            Board board = this.user.GetBoard(boardName);
            Task taskToRemove = new Task(Convert.ToDateTime(task.DueDate), task.Title, task.Description, Convert.ToDateTime(task.CreationTime), task.State);
            this.user.GetBoard(boardName).GetColumnByName(column).RemoveTask(taskToRemove, board.GetIndexOfColumn(board.GetColumnByName(column)),boardName,this.user.GetEmail());
        }

        /// <summary>
        /// This function is responsible for changing state of a task, connects between the GUI(presentation layer) and business/logic layer
        /// </summary>
        /// <param name="task"> Task object to be moved to other column </param>
        /// <param name="u"> Current user </param>
        public string ChangeState(InterfaceLayerTask task,string boardName)
        {
            Board board = this.user.GetBoard(boardName);
            Task taskToMove = new Task(Convert.ToDateTime(task.DueDate), task.Title, task.Description, Convert.ToDateTime(task.CreationTime), task.State);
            if (this.user.GetBoard(boardName).GetIndexOfColumn(this.user.GetBoard(boardName).GetRightest()) > this.user.GetBoard(boardName).GetColumnKey(task.State))
            {
                if (board.ChangeState(taskToMove,boardName,this.user.GetEmail()))
                {
                    return "";
                }
                else
                {
                    Logger.GetLogger().Error("Limitation Error. Movement not allowed!!");
                    return "limit";
                }
            }
            else // Last column
            {
                Logger.GetLogger().Error("Given task is found in last column. Movement not allowed!!");
                return "last";
            }
        }

        /// <summary>
        /// This function is responsible for limiting number of tasks in all columns, connects between GUI and business/logic layer 
        /// </summary>
        /// <param name="limit"> limit num</param>
        public void Limit(int limit, string column,string boardName)
        {
            Board board = this.user.GetBoard(boardName);
            // Delegation
            foreach (Column c in board.GetDictionary().Values)
            {
                if(c.GetColumnName().Equals(column))
                    c.Limit(limit);
            }
        }

        public void Unlimit(string column,string boardName)
        {
            Board board = this.user.GetBoard(boardName);
            // Delegation
            foreach (Column c in board.GetDictionary().Values)
            {
                if (c.GetColumnName().Equals(column))
                    c.SetIsLimited(false);
            }
        }

        public bool AddColumn(string columnName,string boardName)
        {
            if (this.user.GetBoard(boardName).AddColumn(new Column(columnName),boardName,this.user.GetEmail()))
                return true;
            else
                return false;
        }

        public void RemoveColumn(string columnName,string boardName)
        {
            this.user.GetBoard(boardName).RemoveColumn(this.user.GetBoard(boardName).GetColumnByName(columnName),boardName,this.user.GetEmail());
        }

        public void Switch(string column1, string column2,string boardName)
        {
            this.user.GetBoard(boardName).SwitchColumns(this.user.GetBoard(boardName).GetColumnByName(column1), this.user.GetBoard(boardName).GetColumnKey(column2),boardName,this.user.GetEmail());
        }

        public InterfaceLayerBoard GetIBoard(string boardName)
        {
            List<InterfaceLayerColumn> columns = new List<InterfaceLayerColumn>();

            Board b = this.user.GetBoard(boardName);

            foreach (KeyValuePair<int, Column> pair in b.GetDictionary())
            {
                Column c = pair.Value;

                List<InterfaceLayerTask> tasks = new List<InterfaceLayerTask>();

                foreach (Task t in c.GetTasks())
                {
                    InterfaceLayerTask It = new InterfaceLayerTask(t.GetCreationTime().ToString(), t.GetDueDate().ToString(), t.GetDescription(), t.GetTitle(), t.GetState());
                    tasks.Add(It);

                }

                InterfaceLayerColumn Ic = new InterfaceLayerColumn(tasks);
                columns.Add(Ic);

            }

            return new InterfaceLayerBoard(columns);

        }

        public void EditTask(InterfaceLayerTask task,string boardName)
        {
            int colNum = this.user.GetBoard(boardName).GetColumnKey(task.State);
            this.user.GetBoard(boardName).GetColumnByName(task.State).RemoveByCreationTime(task.CreationTime, colNum,boardName,this.user.GetEmail());
            Task newTask = new Task(Convert.ToDateTime(task.DueDate), task.Title, task.Description, Convert.ToDateTime(task.CreationTime), task.State);
            this.user.GetBoard(boardName).GetColumnByName(task.State).AddTaskToColumn(newTask, colNum,boardName,this.user.GetEmail());
        }

        public bool IsBoardEmpty(string boardName)
        {
            return (this.user.GetBoard(boardName).IsEmpty());
        }

        public bool AddBoard(string boardName)
        {
            return this.user.AddBoard(boardName);
        }

        public void RemoveBoard(string boardName)
        {
            this.user.RemoveBoard(boardName);
        }

        public void InitializeBoards(ObservableCollection<ComboBoxItem> boardsItems)
        {
            foreach (KeyValuePair<string, Board> pair in this.user.GetBoards())
                boardsItems.Add(new ComboBoxItem { Content = pair.Key });
        }

        public void InitializeColumns(ObservableCollection<ComboBoxItem> cBItems, string boardName)
        {
            foreach (KeyValuePair<int, Column> pair in this.user.GetBoard(boardName).GetDictionary())
                cBItems.Add(new ComboBoxItem { Content = pair.Value.GetColumnName() });
        }

        public int GetIndexOfColumn(string boardName,string state)
        {
            return (this.user.GetBoard(boardName).GetIndexByColumnName(state));
        }

    }
}
