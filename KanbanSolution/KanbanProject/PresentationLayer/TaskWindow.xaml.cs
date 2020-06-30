using KanbanProject.InterfaceLayer;
using KanbanProject.InterfaceLayer.DataObjects;
using KanbanProject.PresentationLayer.ViewModel;
using System;
using System.Windows;

namespace KanbanProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        TaskWindowDataContext twdc;
        UserController service;
        string boardName;

        public TaskWindow(UserController service, string boardName)
        {
            InitializeComponent();
            this.boardName = boardName;
            this.twdc = new TaskWindowDataContext();
            this.DataContext = this.twdc;
            this.service = service;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.service.IsBoardEmpty(boardName))
            {
                if (CheckDueDate())
                {
                    string output = service.CreateTask(twdc.DueDate.ToString(), twdc.Title, twdc.Description,boardName);
                    if (output.Equals("")) // Successfull 
                    {
                        BoardWindow win = new BoardWindow(this.service,boardName);
                        win.Show();
                        this.Close();
                    }
                    else if (output.Equals("limit"))
                    {
                        MessageBox.Show("Limitation Error!!");
                    }
                    else if (output.Equals("details"))
                    {
                        MessageBox.Show("One of the details is not correct(Title[max. 50 chars] and description[max. 300 chars] should not be empty!");
                    }
                    else
                    {
                        MessageBox.Show("Board is Empty");
                    }
                }
                else
                {
                    MessageBox.Show("invalid date!");
                }
            }
            else
            {
                MessageBox.Show("Empty board");
            }
        }

        private bool CheckDueDate()
        {
            DateTime toCheck = twdc.DueDate;
            DateTime now = DateTime.Now;
            if (toCheck.CompareTo(now) >= 0) { return true; }
            else return false;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            BoardWindow win = new BoardWindow(this.service,boardName);
            win.Show();
            this.Close();
        }
    }
}
