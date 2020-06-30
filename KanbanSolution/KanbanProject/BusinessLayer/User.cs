using KanbanProject.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KanbanProject.BusinessLayer
{
    public class User
    {
        // System fields
        private string email;
        private string password;
        private Dictionary<string, Board> boards;

        private DataBase dataBase;

        private const int MIN_PASS_LENGTH = 4;
        private const int MAX_PASS_LENGTH = 20;

        public User(string email, string password) // ctor
        {
            this.email = email;
            this.password = password;
            this.dataBase = new DataBase();
        }

        public string GetEmail()
        {
            return email;
        }
        public Dictionary<string,Board> GetBoards()
        {
            return this.boards;
        }

        public void SetEmail(string email)
        {
            this.email = email;
        }

        public string GetPassword()
        {
            return password;
        }

        public Board GetBoard(String name)
        {
            return this.boards[name];
        }

        public bool AddBoard(string boardName)
        {
            if (boards.ContainsKey(boardName))
            {
                Logger.GetLogger().Error("There is a board with this name");
                return false;
            }
            else if (boardName == "" || boardName == null) { 
                Logger.GetLogger().Error("invalid board name");
                return false;
            }
            else
            {

                Board newB = new Board();
                newB.SetDataBase(dataBase);
                //initializes the new board
                newB.AddColumnToBoardRestore(new Column("backLog"));
                newB.GetDictionary()[1].SetDataBase(this.dataBase);
                newB.AddColumnToBoardRestore(new Column("inProgress"));
                newB.GetDictionary()[2].SetDataBase(this.dataBase);
                newB.AddColumnToBoardRestore(new Column("done"));
                newB.GetDictionary()[3].SetDataBase(this.dataBase);
                newB.GetDictionary()[1].SetLeftest(true);
                newB.GetDictionary()[newB.GetDictionary().Count].SetRightest(true);

                this.boards.Add(boardName, newB);
                //Update data
                this.dataBase.AddBoard(this.email, boardName);
                return true;
            }
        }

        public void RemoveBoard(string boardName)
        {
            if (boardName == null)
            {
                Logger.GetLogger().Error("invalid board name");
                return;
            }
            else
            {
                this.boards.Remove(boardName);
                //Updae data
                this.dataBase.RemoveBoard(this.email, boardName);
            }
        }

        /// <summary>
        /// This function is responsible for the Login proccess 
        /// </summary>
        /// <param name="boardDataFile"> Board data file for restore </param>
        public void Login()
        {
            DALBoards db = this.dataBase.GetUserDALBoards(this.email);
            foreach(KeyValuePair<string,BoardData> pair in db.GetBoards().ToList())
            {
                if (this.boards == null)
                    this.boards = new Dictionary<string, Board>();
                this.boards.Add(pair.Key, ConvertBDtoBoard(pair.Value));
            }
            if (this.boards == null)
                this.boards = new Dictionary<string, Board>();
            Initialization();
        }

        private Board ConvertBDtoBoard(BoardData bd)
        {
            Board b = new Board();
            b.SetDataBase(dataBase);
            foreach (KeyValuePair<int, DALColumn> dc in bd.getDictionary().ToList())
            {
                Column c = new Column(dc.Value.GetTitle());
                b.AddColumnToBoardRestore(c);
                foreach (DALTask dt in dc.Value.GetColumn())
                {
                    Task t = new Task(Convert.ToDateTime(dt.GetDueDate()), dt.GetTitle(), dt.GetDescription(), Convert.ToDateTime(dt.GetCreationDate()), dc.Value.GetTitle());
                    c.AddTaskToColumnRestore(t);
                }
            }
            if (b.GetCount() > 0)
            {
                b.GetColumnByIndex(1).SetLeftest(true);
                b.GetColumnByIndex(b.GetCount()).SetRightest(true);
            }
            return b;
        }

        /// <summary>
        /// This function is responsible for the Registration proccess 
        /// </summary>
        /// <param name="boardDataFile"> Board data file for board saving </param>
        public void Register()
        {
            this.dataBase.AddUser(this.email,this.password);
            this.boards = new Dictionary<string, Board>();
            Initialization();
        }

        

        /// <summary>
        /// Init function, used to init attributes in board columns 
        /// </summary>
        private void Initialization()
        {
            foreach (KeyValuePair<string, Board> pair in this.boards)
            {
                foreach (KeyValuePair<int, Column> c in pair.Value.GetDictionary())
                {
                    c.Value.SetDataBase(this.dataBase);
                }
            }
        }

        /// <summary>
        /// Logout function
        /// </summary>
        public void Logout()
        {
            Logger.GetLogger().Debug(this.email + " Logged out successfully!");
        }




        /// <summary>
        /// This function is responsible for checking if the user password is valid 
        /// </summary>
        /// <param name="password"> user password string </param>
        /// <returns> True - password is valid, False otherwise </returns>
        public static bool IsPasswordlValid(string password)
        {
            if (password == null)
            {
                Logger.GetLogger().Error("Password is null");
                return false;
            }

            if (password.Length < MIN_PASS_LENGTH || password.Length > MAX_PASS_LENGTH) // A user password must be in length of 4 to 20 characters
            {
                Logger.GetLogger().Error("Password Length does not match requirements");
                return false;
            }
            bool upper = false, lower = false, digit = false;

            foreach (char c in password) // A user password must include at least one capital character, one small character and a number
            {
                if (Char.IsUpper(c)) { upper = true; }
                if (Char.IsLower(c)) { lower = true; }
                if (Char.IsDigit(c)) { digit = true; }
            }
            if (upper && lower && digit) { return true; }
            else { Logger.GetLogger().Error("Password Doesn't Inlucde one or more - Capital letter, small letter, digit"); return false; }
        }

        /// <summary>
        /// This function is responsible for checking if the user email is valid 
        /// </summary>
        /// <param name="email"> user email string </param>
        /// <returns> True - email is valid, False otherwise </returns>
        public static bool IsEmailValid(string email)
        {
            if (email == null)
            {
                Logger.GetLogger().Error("Email is null");
                return false;
            }


            // Usage of Regesx 
            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail) { return true; }
            else { Logger.GetLogger().Error("Invalid Email"); return false; }
        }

        /// Check if valid Login details ( email & password ) 
        public static bool IsValidLogin(string email, string password)
        {
            return IsEmailValid(email) && IsPasswordlValid(password);
        }

        /// Check if valid Registration details ( email & password ) 
        public static bool IsValidRegistration(string email, string password)
        {
            return IsEmailValid(email) && IsPasswordlValid(password);
        }

        /// <summary>
        /// This function checks if the user with certain email and password is already in the users list ( File ) 
        /// </summary>
        /// <param name="email"> user email </param>
        /// <param name="password"> user password </param>
        /// <param name="userfile"> File to check existance </param>
        /// <returns> True if user already exists, False otherwise </returns>
        public static bool IsExist(string email, string password)
        {
            DataBase dataBase = new DataBase();
            return dataBase.IsExist(email, password);
        }

    }
}
