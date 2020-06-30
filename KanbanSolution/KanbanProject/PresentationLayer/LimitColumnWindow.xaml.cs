using System;
using System.Windows;
using KanbanProject.InterfaceLayer;
using KanbanProject.PresentationLayer.ViewModel;

namespace KanbanProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for LimitColumnWindow.xaml
    /// </summary>
    public partial class LimitColumnWindow : Window
    {
        UserController service;
        ColumnDataContext cdc;
        string boardName;

        public LimitColumnWindow(UserController service,string boardName)
        {
            InitializeComponent();
            this.boardName = boardName;
            this.service = service;
            this.cdc = new ColumnDataContext(this.service,boardName);
            this.DataContext = this.cdc;
        }

        private void limitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.cdc.ColumnName1 == null) // no column selcted
            {
                MessageBox.Show("Please select column to limit");   
            }
            int lim;
            bool isNumeric = int.TryParse(cdc.Lim, out lim); // check is numeric
            if (String.IsNullOrEmpty(cdc.Lim) || !isNumeric)
            {
                MessageBox.Show("Please select a limitation number");
            }
            else
            {
                this.service.Limit(Convert.ToInt32(cdc.Lim), (string)this.cdc.ColumnName1.Content,boardName);
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
