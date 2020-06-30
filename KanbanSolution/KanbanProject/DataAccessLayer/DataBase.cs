using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace KanbanProject.DataAccessLayer
{
    public class DataBase
    {
        private const string USERSTABLE = "Users";
        private const string COLUMNSTABLE = "Columns";
        private const string BOARDSTABLE = "Boards";
        private const string TASKSTABLE = "Tasks";

        private static string database_name = "KanbanDataBase.sqlite";
        private static string connection_string = $"Data Source={database_name};Version=3";
        private SQLiteConnection connection = new SQLiteConnection(connection_string);
        private SQLiteCommand command;


        public DataBase()
        {
            if (!File.Exists(database_name))
            {
                try
                {
                    SQLiteConnection.CreateFile(database_name);
                    connection.Open();
                    string createUsersTable = "create table " + USERSTABLE + " (email varchar(50),password varchar(20))";
                    command = new SQLiteCommand(createUsersTable, connection);
                    command.ExecuteNonQuery();
                    string createTasksTable = "create table " + TASKSTABLE + " (email varchar(50),boardName varchar(20),creationTime varchar(20),dueDate varchar(20),title varchar(50),description varchar(300),columnName varchar(20))";
                    command = new SQLiteCommand(createTasksTable, connection);
                    command.ExecuteNonQuery();
                    string createColumnsTable = "create table " + COLUMNSTABLE + " (email varchar(50),boardName varchar(20),columnName varchar(20))";
                    command = new SQLiteCommand(createColumnsTable, connection);
                    command.ExecuteNonQuery();
                    string createBoardsTable = "create table " + BOARDSTABLE + " (email varchar(50),boardName varchar(20))";
                    command = new SQLiteCommand(createBoardsTable, connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error");
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool IsExist(string email, string password)
        {
            try
            {
                connection.Open();
                string sql = "select * from " + USERSTABLE;
                command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    if (reader["email"].ToString().Equals(email) && reader["password"].ToString().Equals(password))
                        return true;
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return false;
        }

        public void AddUser(string email, string password)
        {
            try
            {
                connection.Open();
                command = new SQLiteCommand(null, connection);
                //Adds user to userstable
                command.CommandText = "INSERT INTO " + USERSTABLE + " (email,password) " + "VALUES (@email, @password)";
                SQLiteParameter email_param = new SQLiteParameter(@"email", email);
                SQLiteParameter password_param = new SQLiteParameter(@"password", password);
                command.Parameters.Add(email_param);
                command.Parameters.Add(password_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();

                //Adds user's deafult board to boardstable
                command = new SQLiteCommand(null, connection);
                command.CommandText = "INSERT INTO " + BOARDSTABLE + " (email,boardName) " + "VALUES (@email, @boardName)";
                SQLiteParameter boardName_param = new SQLiteParameter(@"boardName", "deafult board");
                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();

                //Initializes user's deafult board in columnstable
                CreateDeafultBoard(email, "backlog", "deafult board");
                CreateDeafultBoard(email, "in progress", "deafult board");
                CreateDeafultBoard(email, "done", "deafult board");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        private void CreateDeafultBoard(string email, string column, string boardName)
        {
            command = new SQLiteCommand(null, connection);
            command.CommandText = "INSERT INTO " + COLUMNSTABLE + " (email,boardName,columnName) " + "VALUES (@email, @boardName, @columnName)";
            SQLiteParameter email_param = new SQLiteParameter(@"email", email);
            SQLiteParameter boardName_param = new SQLiteParameter(@"boardName", boardName);
            SQLiteParameter columnName_param = new SQLiteParameter(@"columnName", column);
            command.Parameters.Add(email_param);
            command.Parameters.Add(boardName_param);
            command.Parameters.Add(columnName_param);
            command.Prepare();
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void AddTask(string email, string boardName, string creationTime, string dueDate, string columnName, string title, string description)
        {
            try
            {
                connection.Open();
                //inserting into the task table
                command = new SQLiteCommand(null, connection);
                command.CommandText = "INSERT INTO " + TASKSTABLE + " (email,boardName,creationTime,dueDate,title,description,columnName) " + "VALUES (@email, @boardName, @creationTime, @dueDate, @title, @description, @columnName)";
                SQLiteParameter email_param = new SQLiteParameter(@"email", email);
                SQLiteParameter boardName_param = new SQLiteParameter(@"boardName", boardName);
                SQLiteParameter creationTime_param = new SQLiteParameter(@"creationTime", creationTime);
                SQLiteParameter dueDate_param = new SQLiteParameter(@"dueDate", dueDate);
                SQLiteParameter title_param = new SQLiteParameter(@"title", title);
                SQLiteParameter description_param = new SQLiteParameter(@"description", description);
                SQLiteParameter columnName_param = new SQLiteParameter(@"columnName", columnName);
                command.Parameters.Add(email_param);
                command.Parameters.Add(creationTime_param);
                command.Parameters.Add(dueDate_param);
                command.Parameters.Add(title_param);
                command.Parameters.Add(description_param);
                command.Parameters.Add(columnName_param);
                command.Parameters.Add(boardName_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }


        public void AddColumn(string email, string boardName, string columnName)
        {
            try
            {
                connection.Open();
                //inserting into the columns' table
                command = new SQLiteCommand(null, connection);
                command.CommandText = "INSERT INTO " + COLUMNSTABLE + " (email,boardName,columnName) " + "VALUES (@email, @boardName,@columnName)";
                SQLiteParameter email_param = new SQLiteParameter(@"email", email);
                SQLiteParameter boardName_param = new SQLiteParameter(@"boardName", boardName);
                SQLiteParameter columnName_param = new SQLiteParameter(@"columnName", columnName);

                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Parameters.Add(columnName_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        public void AddBoard(string email, string boardName)
        {
            try
            {
                connection.Open();
                //inserting into the boards' table
                command = new SQLiteCommand(null, connection);
                command.CommandText = "INSERT INTO " + BOARDSTABLE + " (email,boardName) " + "VALUES (@email, @boardName)";
                SQLiteParameter email_param = new SQLiteParameter(@"email", email);
                SQLiteParameter boardName_param = new SQLiteParameter(@"boardName", boardName);
                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();

                //Adds deafult columns to board
                CreateDeafultBoard(email, "backlog", boardName);
                CreateDeafultBoard(email, "in progress", boardName);
                CreateDeafultBoard(email, "done", boardName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        public void RemoveTask(string email, string boardName, string creationTime, string dueDate, string columnName, string title, string description)
        {
            try
            {
                connection.Open();

                command = new SQLiteCommand(null, connection);
                command.CommandText = "DELETE FROM " + TASKSTABLE + " WHERE email= @email AND boardName= @boardName AND creationTime= @creationTime AND dueDate= @dueDate AND columnName= @columnName AND title= @title AND description= @description";
                SQLiteParameter email_param = new SQLiteParameter(@"email", email);
                SQLiteParameter boardName_param = new SQLiteParameter(@"boardName", boardName);
                SQLiteParameter creationTime_param = new SQLiteParameter(@"creationTime", creationTime);
                SQLiteParameter dueDate_param = new SQLiteParameter(@"dueDate", dueDate);
                SQLiteParameter columnName_param = new SQLiteParameter(@"columnName", columnName);
                SQLiteParameter title_param = new SQLiteParameter(@"title", title);
                SQLiteParameter description_param = new SQLiteParameter(@"description", description);
                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Parameters.Add(creationTime_param);
                command.Parameters.Add(dueDate_param);
                command.Parameters.Add(columnName_param);
                command.Parameters.Add(title_param);
                command.Parameters.Add(description_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        public void RemoveColumn(string email, string boardName, string columnName)
        {
            try
            {
                connection.Open();
                //first of all remove all tasks of the column
                command = new SQLiteCommand(null, connection);
                command.CommandText = "DELETE FROM " + TASKSTABLE + " WHERE email = @email AND boardName= @boardName AND columnName= @columnName ";
                SQLiteParameter email_param = new SQLiteParameter(@"email", email);
                SQLiteParameter boardName_param = new SQLiteParameter(@"boardName", boardName);
                SQLiteParameter columnName_param = new SQLiteParameter(@"columnName", columnName);
                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Parameters.Add(columnName_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();

                //now remove the column from the columns' table
                command = new SQLiteCommand(null, connection);
                command.CommandText = "DELETE FROM " + COLUMNSTABLE + " WHERE email = @email AND boardName= @boardName AND columnName= @columnName ";
                email_param = new SQLiteParameter(@"email", email);
                boardName_param = new SQLiteParameter(@"boardName", boardName);
                columnName_param = new SQLiteParameter(@"columnName", columnName);
                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Parameters.Add(columnName_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        public void RemoveBoard(string email, string boardName)
        {
            try
            {
                connection.Open();
                //first of all remove all tasks of the column
                command = new SQLiteCommand(null, connection);
                command.CommandText = "DELETE FROM " + TASKSTABLE + " WHERE email = @email AND boardName= @boardName ";
                SQLiteParameter email_param = new SQLiteParameter(@"email", email);
                SQLiteParameter boardName_param = new SQLiteParameter(@"boardName", boardName);
                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();

                // then remove the column from the columns' table
                command = new SQLiteCommand(null, connection);
                command.CommandText = "DELETE FROM " + COLUMNSTABLE + " WHERE email = @email AND boardName= @boardName ";
                email_param = new SQLiteParameter(@"email", email);
                boardName_param = new SQLiteParameter(@"boardName", boardName);
                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();

                //now remove the board from the boards' table
                command = new SQLiteCommand(null, connection);
                command.CommandText = "DELETE FROM " + BOARDSTABLE + " WHERE email = @email AND boardName= @boardName ";
                email_param = new SQLiteParameter(@"email", email);
                boardName_param = new SQLiteParameter(@"boardName", boardName);
                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        public void MoveTask(string email, string boardName, string creationTime, string dueDate, string columnName, string title, string description,string targetColumn)
        {
            try
            {
                connection.Open();

                command = new SQLiteCommand(null, connection);
                command.CommandText ="UPDATE "+TASKSTABLE+ " SET columnName= @targetColumn WHERE email= @email AND boardName= @boardName AND creationTime= @creationTime AND dueDate= @dueDate AND columnName= @columnName AND title= @title AND description= @description";
                SQLiteParameter email_param = new SQLiteParameter(@"email", email);
                SQLiteParameter boardName_param = new SQLiteParameter(@"boardName", boardName);
                SQLiteParameter creationTime_param = new SQLiteParameter(@"creationTime", creationTime);
                SQLiteParameter dueDate_param = new SQLiteParameter(@"dueDate", dueDate);
                SQLiteParameter columnName_param = new SQLiteParameter(@"columnName", columnName);
                SQLiteParameter title_param = new SQLiteParameter(@"title", title);
                SQLiteParameter description_param = new SQLiteParameter(@"description", description);
                SQLiteParameter targetColumn_param = new SQLiteParameter(@"targetColumn", targetColumn);
                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Parameters.Add(creationTime_param);
                command.Parameters.Add(dueDate_param);
                command.Parameters.Add(columnName_param);
                command.Parameters.Add(title_param);
                command.Parameters.Add(description_param);
                command.Parameters.Add(targetColumn_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        public void SwitchColumns(string email, string boardName, string column1, string column2)
        {
            try
            {
                connection.Open();
                //updates columnName of column1 to '-1'
                command = new SQLiteCommand(null, connection);
                command.CommandText = "UPDATE " + COLUMNSTABLE + " SET columnName= @temp WHERE email= @email AND boardName= @boardName AND columnName= @column1 ";
                SQLiteParameter temp_param = new SQLiteParameter(@"temp", "-1");
                SQLiteParameter email_param = new SQLiteParameter(@"email", email);
                SQLiteParameter boardName_param = new SQLiteParameter(@"boardName", boardName);
                SQLiteParameter column1_param = new SQLiteParameter(@"column1", column1);
                command.Parameters.Add(temp_param);
                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Parameters.Add(column1_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();

                //updates columnName of column2 to column1
                command = new SQLiteCommand(null, connection);
                command.CommandText = "UPDATE " + COLUMNSTABLE + " SET columnName= @column1 WHERE email= @email AND boardName= @boardName AND columnName= @column2 ";
                SQLiteParameter column2_param = new SQLiteParameter(@"column2", column2);
                command.Parameters.Add(column1_param);
                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Parameters.Add(column2_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();

                //updates columnName of column1 to column2
                command = new SQLiteCommand(null, connection);
                command.CommandText = "UPDATE " + COLUMNSTABLE + " SET columnName= @column2 WHERE email= @email AND boardName= @boardName AND columnName= @temp ";
                command.Parameters.Add(column2_param);
                command.Parameters.Add(email_param);
                command.Parameters.Add(boardName_param);
                command.Parameters.Add(temp_param);
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        public DALBoards GetUserDALBoards(string email)
        {
            try
            {
                connection.Open();
                command = new SQLiteCommand(null, connection);
                command.CommandText = "SELECT boardName FROM " + BOARDSTABLE + " WHERE email= @email";
                SQLiteParameter email_param = new SQLiteParameter(@"email", email);
                command.Parameters.Add(email_param);
                SQLiteDataReader reader = command.ExecuteReader();
                DALBoards output = new DALBoards(email);
                string boardName;
                while (reader.Read())
                {
                    boardName = reader["boardName"].ToString();
                    output.AddBoard(boardName);
                    InitializedBoardsColumns(email, boardName, output);
                }
                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return null;
        }

        private void InitializedBoardsColumns(string email, string boardName, DALBoards dalBoards)
        {
            SQLiteCommand c = new SQLiteCommand(null, connection);
            c.CommandText = "SELECT columnName FROM " + COLUMNSTABLE + " WHERE email= @email AND boardName= @boardName";
            SQLiteParameter email_param = new SQLiteParameter(@"email", email);
            SQLiteParameter boardName_param = new SQLiteParameter(@"boardName", boardName);
            c.Parameters.Add(email_param);
            c.Parameters.Add(boardName_param);
            SQLiteDataReader reader = c.ExecuteReader();
            string columnName;
            while (reader.Read())
            {
                columnName = reader["columnName"].ToString();
                dalBoards.GetBoards()[boardName].AddColumn(columnName);
                InitializedColumnsTasks(email, boardName, columnName, dalBoards);
            }
        }

        private void InitializedColumnsTasks(string email, string boardName, string columnName, DALBoards dalBoards)
        {
            SQLiteCommand c = new SQLiteCommand(null, connection);
            c.CommandText = "SELECT creationTime,dueDate,title,description FROM " + TASKSTABLE + " WHERE email= @email AND boardName= @boardName AND columnName= @columnName";
            SQLiteParameter email_param = new SQLiteParameter(@"email", email);
            SQLiteParameter boardName_param = new SQLiteParameter(@"boardName", boardName);
            SQLiteParameter columnName_param = new SQLiteParameter(@"columnName", columnName);
            c.Parameters.Add(email_param);
            c.Parameters.Add(boardName_param);
            c.Parameters.Add(columnName_param);
            SQLiteDataReader reader = c.ExecuteReader();
            while (reader.Read())
            {
                DALTask dalTask = new DALTask(reader["creationTime"].ToString(), reader["dueDate"].ToString(), reader["title"].ToString(), reader["description"].ToString());
                dalBoards.GetBoards()[boardName].GetColumnByName(columnName).Add(dalTask);
            }
        }



    }
}
