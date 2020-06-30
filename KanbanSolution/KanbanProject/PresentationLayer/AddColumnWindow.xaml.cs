using KanbanProject.InterfaceLayer;
using KanbanProject.PresentationLayer.ViewModel;
using System.Windows;


namespace KanbanProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for AddColumnWindow.xaml
    /// </summary>
    public partial class AddColumnWindow : Window
    {
        ColumnDataContext cdc;
        UserController service;
        string boardName;

        public AddColumnWindow(UserController service,string boardName)
        {
            InitializeComponent();
            this.boardName = boardName;
            this.service = service;
            this.cdc = new ColumnDataContext(this.service,boardName);
            this.DataContext = this.cdc;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            if (service.AddColumn(this.cdc.ColumnName,this.boardName)) // Successfull adition 
            {
                BoardWindow win = new BoardWindow(this.service,this.boardName); // return to board window 
                win.Show();
                this.Close();
            }
            else if (this.cdc.ColumnName == "" || this.cdc.ColumnName == null)
            {
                MessageBox.Show("The column's name is invalid");
            }
            else
            {
                MessageBox.Show("This column already exists in the board!");
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            BoardWindow win = new BoardWindow(this.service,this.boardName);
            win.Show();
            this.Close();
        }
    }
}
