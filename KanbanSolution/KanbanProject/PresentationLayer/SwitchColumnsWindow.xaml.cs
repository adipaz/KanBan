using KanbanProject.InterfaceLayer;
using KanbanProject.PresentationLayer.ViewModel;
using System.Windows;

namespace KanbanProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for SwitchColumnsWindow.xaml
    /// </summary>
    public partial class SwitchColumnsWindow : Window
    {
        UserController service;
        ColumnDataContext cdc;
        string boardName;

        public SwitchColumnsWindow(UserController service,string boardName)
        {
            InitializeComponent();
            this.boardName = boardName;
            this.service = service;
            this.cdc = new ColumnDataContext(this.service,boardName);
            this.DataContext = this.cdc;
        }

        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.cdc.ColumnName1 == null || this.cdc.ColumnName2 == null) // Columns were not picked 
            {
                MessageBox.Show("Please select two columns to switch");
            }
            else if (this.cdc.ColumnName1 == this.cdc.ColumnName2) // same column 
            {
                MessageBox.Show("Please select two different columns!");
            }

            else
            {
                this.service.Switch((string)this.cdc.ColumnName1.Content, (string)this.cdc.ColumnName2.Content,boardName);
                BoardWindow win = new BoardWindow(this.service,boardName);
                win.Show();
                this.Close();
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            BoardWindow win = new BoardWindow(this.service,boardName);
            win.Show();
            this.Close();
        }
    }
}
