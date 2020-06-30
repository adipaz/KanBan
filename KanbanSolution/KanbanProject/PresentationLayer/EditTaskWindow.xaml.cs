using KanbanProject.InterfaceLayer;
using KanbanProject.InterfaceLayer.DataObjects;
using KanbanProject.PresentationLayer.ViewModel;
using System;
using System.Windows;

namespace KanbanProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        UserController service;
        TaskWindowDataContext twdc;
        string state;
        string creationTime;
        string boardName;

        public EditTaskWindow(UserController service, DateTime dueDate, string title, string description, string creationTime, string state,string boardName)
        {
            InitializeComponent();
            this.boardName = boardName;
            this.service = service;
            this.twdc = new TaskWindowDataContext();
            this.DataContext = this.twdc;
            this.twdc.DueDate = dueDate;
            this.twdc.Title = title;
            this.twdc.Description = description;
            this.creationTime = creationTime;
            this.state = state;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckDueDate()) // Due date is valid 
            {
                if (!twdc.Title.Equals("") && !twdc.Description.Equals(""))
                {
                    InterfaceLayerTask taskToEdit = new InterfaceLayerTask(creationTime, twdc.DueDate.ToString(), twdc.Description, twdc.Title, state);
                    this.service.EditTask(taskToEdit,boardName);
                    BoardWindow win = new BoardWindow(this.service,boardName);
                    win.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Task title and/or description can not be empty!");
                }
            }
            else
            {
                MessageBox.Show("invalid date!");
            }

        }


        private bool CheckDueDate()
        {
            DateTime toCheck = twdc.DueDate;
            DateTime now = DateTime.Now;
            if (toCheck.CompareTo(now) >= 0) { return true; }
            else return false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BoardWindow win = new BoardWindow(this.service,boardName);
            win.Show();
            this.Close();
        }
    }
}
