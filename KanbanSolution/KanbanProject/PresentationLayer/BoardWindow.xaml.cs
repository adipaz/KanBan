using KanbanProject.InterfaceLayer;
using KanbanProject.InterfaceLayer.DataObjects;
using KanbanProject.PresentationLayer.ViewModel;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Linq;

namespace KanbanProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        BoardWindowDataContext bwdc;
        UserController service;
        string boardName;

        public BoardWindow(UserController service,string boardName)
        {
            InitializeComponent();
            this.boardName = boardName;
            this.bwdc = new BoardWindowDataContext(service,boardName);
            this.DataContext = this.bwdc;
            this.service = service;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow TaskWin = new TaskWindow(this.service,boardName);
            TaskWin.Show();
            this.Close();
        }

        private void AddColumnButton_Click(object sender, RoutedEventArgs e)
        {
            AddColumnWindow AddColWin = new AddColumnWindow(this.service,this.boardName);
            AddColWin.Show();
            this.Close();
        }

        private void RemoveColumnButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveColumnWindow RemoveColWin = new RemoveColumnWindow(this.service,this.boardName);
            RemoveColWin.Show();
            this.Close();
        }

        private void MoveTaskButton_Click(object sender, RoutedEventArgs e) // move task to next column
        {
            if (this.bwdc.Selected != null)
            {
                InterfaceLayerTask taskToMove = new InterfaceLayerTask(this.bwdc.Selected.CreationTime, this.bwdc.Selected.Duedate, this.bwdc.Selected.Description, this.bwdc.Selected.Title, this.bwdc.Selected.State);
                string output = this.service.ChangeState(taskToMove,this.boardName);
                if (output.Equals("limit")) // movement not allowed 
                    MessageBox.Show("Movement not allowed! [ limitation ]");
                else if (output.Equals("last"))
                    MessageBox.Show("Movement not allowed! [ last column ]");
                else
                    this.bwdc.SetIB(this.service.GetIBoard(this.boardName));
            }
            else
                System.Windows.MessageBox.Show("No task selected");
        }

        private void SwitchColumnsButton_Click(object sender, RoutedEventArgs e) // switch columns 
        {
            SwitchColumnsWindow SwitchColsWin = new SwitchColumnsWindow(this.service,boardName);
            SwitchColsWin.Show();
            this.Close();
        }

        private void RemoveTaskButton_Click(object sender, RoutedEventArgs e) // remove task 
        {
            if (this.bwdc.Selected != null)
            {
                InterfaceLayerTask taskToRemove = new InterfaceLayerTask(this.bwdc.Selected.CreationTime, this.bwdc.Selected.Duedate, this.bwdc.Selected.Description, this.bwdc.Selected.Title, this.bwdc.Selected.State);
                this.service.RemoveTask(taskToRemove,this.boardName);
                this.bwdc.SetIB(this.service.GetIBoard(this.boardName));
            }
            else
                MessageBox.Show("No task selected");
        }

        private void limitBtn_Click(object sender, RoutedEventArgs e) // limit columns 
        {
            LimitColumnWindow win = new LimitColumnWindow(this.service,boardName);
            win.Show();
            this.Close();

        }

        private void unlimitBtn_Click(object sender, RoutedEventArgs e) // unlimit columns 
        {
            UnlimitColumnWindow win = new UnlimitColumnWindow(service,boardName);
            win.Show();
            this.Close();
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e) // logout 
        {
            // ask user if he wants to exit 
            DialogResult dialog = System.Windows.Forms.MessageBox.Show("Are you sure you want to log out?\n", "Log Out", MessageBoxButtons.YesNo);

            if (dialog == System.Windows.Forms.DialogResult.Yes)
            {
                LoginWindow loginWin = new LoginWindow(); // return to login window 
                loginWin.Show();
                this.Close();
            }
        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e) // edit task
        {
            if (this.bwdc.Selected != null)
            {
                EditTaskWindow EditWin = new EditTaskWindow(this.service, Convert.ToDateTime(this.bwdc.Selected.Duedate), this.bwdc.Selected.Title, this.bwdc.Selected.Description, this.bwdc.Selected.CreationTime, this.bwdc.Selected.State,boardName);
                EditWin.Show();
                this.Close();
            }
            else
            {
                System.Windows.MessageBox.Show("No task selected");
            }
        }

        private void SortByDueDateButton_Click(object sender, RoutedEventArgs e) // sory by due date 
        {
            ObservableCollection<BoardWindowRow> SortedByDueDate = new ObservableCollection<BoardWindowRow>(this.bwdc.Tasks.OrderBy(x => this.service.GetIndexOfColumn(this.boardName,x.State)).ThenBy(x => x.Duedate));
            this.bwdc.Tasks = SortedByDueDate;
        }

        private void CreationTimeButton_Click(object sender, RoutedEventArgs e) // sort by creation date 
        {
            ObservableCollection<BoardWindowRow> SortedByCreationTime = new ObservableCollection<BoardWindowRow>(this.bwdc.Tasks.OrderBy(x => this.service.GetIndexOfColumn(this.boardName,x.State)).ThenBy(x => x.CreationTime));
            this.bwdc.Tasks = SortedByCreationTime;
        }

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void FilterBtn_Click(object sender, RoutedEventArgs e) // filter button 
        {
            this.bwdc.UpdateFilter();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            BoardsWindow boardsWindow = new BoardsWindow(service);
            boardsWindow.Show();
            this.Close();
        }
    }
}
