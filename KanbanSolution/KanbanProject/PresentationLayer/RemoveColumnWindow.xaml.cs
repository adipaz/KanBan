using KanbanProject.InterfaceLayer;
using KanbanProject.PresentationLayer.ViewModel;
using System.Windows;

namespace KanbanProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for RemoveColumnWindow.xaml
    /// </summary>
    public partial class RemoveColumnWindow : Window
    {
        UserController service;
        ColumnDataContext cdc;
        string boardName;

        public RemoveColumnWindow(UserController service,string boardName)
        {
            InitializeComponent();
            this.service = service;
            this.boardName = boardName;
            this.cdc = new ColumnDataContext(this.service,boardName);
            this.DataContext = this.cdc;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.cdc.ColumnName1 == null) // no column selected 
            {
                MessageBox.Show("Please select a column");
            }
            else
            {
                this.service.RemoveColumn((string)this.cdc.ColumnName1.Content,this.boardName);
                BoardWindow win = new BoardWindow(this.service,this.boardName); // return to board window 
                win.Show();
                this.Close();
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
