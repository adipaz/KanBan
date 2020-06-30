using KanbanProject.BusinessLayer;
using KanbanProject.InterfaceLayer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace KanbanProject.PresentationLayer.ViewModel
{
    class BoardsWindowDataContext : INotifyPropertyChanged
    {
        ObservableCollection<ComboBoxItem> boardsitems;
        public ObservableCollection<ComboBoxItem> BoardsItems
        {
            get
            {
                return boardsitems;
            }
            set
            {
                boardsitems = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("boardsitems"));
            }
        }

        ComboBoxItem boardName;
        public ComboBoxItem BoardName
        {
            get
            {
                return boardName;
            }
            set
            {
                boardName = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("boardName"));
            }
        }

        string newBoardName = "";
        public string NewBoardName
        {
            get
            {
                return newBoardName;
            }
            set
            {
                newBoardName = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("newBoardName"));
            }
        }

        UserController service;

        public BoardsWindowDataContext(UserController service)
        {
            this.service = service;
            this.BoardsItems=new ObservableCollection<ComboBoxItem>();
            this.service.InitializeBoards(this.BoardsItems);
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
