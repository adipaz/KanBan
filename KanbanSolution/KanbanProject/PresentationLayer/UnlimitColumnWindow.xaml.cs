using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using KanbanProject.InterfaceLayer;
using KanbanProject.PresentationLayer.ViewModel;

namespace KanbanProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for UnlimitColumnWindow.xaml
    /// </summary>
    public partial class UnlimitColumnWindow : Window
    {
        UserController service;
        ColumnDataContext cdc;
        string boardName;

        public UnlimitColumnWindow(UserController service,string boardName)
        {
            InitializeComponent();
            this.boardName = boardName;
            this.service = service;
            this.cdc = new ColumnDataContext(this.service,boardName);
            this.DataContext = this.cdc;
        }

        private void unlimitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.cdc.ColumnName1 == null) // no column selcted
            {
                MessageBox.Show("Please select column to limit");
            }
            else
            {
                this.service.Unlimit((string)this.cdc.ColumnName1.Content,boardName);
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
