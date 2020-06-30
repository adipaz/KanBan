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
    /// Interaction logic for BoardsWindow.xaml
    /// </summary>
    public partial class BoardsWindow : Window
    {
        BoardsWindowDataContext bwdc;
        UserController service;

        public BoardsWindow(UserController service)
        {
            InitializeComponent();
            this.service = service;
            this.bwdc = new BoardsWindowDataContext(service);
            this.DataContext = this.bwdc;
        }

        private void ViewBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.bwdc.BoardName != null)
            {
                BoardWindow boardWindow = new BoardWindow(this.service, (string)this.bwdc.BoardName.Content);
                boardWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a board");
            }
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.bwdc.BoardName != null)
            {
                this.service.RemoveBoard((string)this.bwdc.BoardName.Content);
                MessageBox.Show("Board deleted successfully");
                BoardsWindow boardsWindow = new BoardsWindow(service);
                boardsWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a board");
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.bwdc.NewBoardName != null&&!this.bwdc.NewBoardName.Equals(""))
            {
                if (this.service.AddBoard(this.bwdc.NewBoardName)){
                    MessageBox.Show("Board added successfully");
                    this.bwdc = new BoardsWindowDataContext(service);
                    BoardsWindow boardsWindow = new BoardsWindow(service);
                    boardsWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Board with this name is already exists");
                }
            }
            else
            {
                MessageBox.Show("Please select a boardName");
            }
        }
    }
}
