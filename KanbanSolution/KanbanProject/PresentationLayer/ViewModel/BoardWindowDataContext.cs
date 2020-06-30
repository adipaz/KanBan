using KanbanProject.InterfaceLayer;
using KanbanProject.InterfaceLayer.DataObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace KanbanProject.PresentationLayer.ViewModel
{
    class BoardWindowDataContext : INotifyPropertyChanged
    {
        private string limit;
        public string Limit
        {
            get { return limit; }
            set
            {
                limit = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Limit"));
            }
        }

        string searchTerm = "";
        public string SearchTerm
        {
            get
            {
                return searchTerm;
            }
            set
            {
                searchTerm = value;
                //UpdateFilter();
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SearchTerm"));
            }
        }

        private BoardWindowRow selected;
        public BoardWindowRow Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Selected"));
            }
        }

        private ICollectionView gridView;
        public ICollectionView GridView
        {
            get
            {
                return gridView;
            }
            set
            {
                gridView = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GridView"));
            }
        }

        private ObservableCollection<BoardWindowRow> tasks;
        public ObservableCollection<BoardWindowRow> Tasks
        {
            get
            {
                return tasks;
            }
            set
            {
                tasks = value;
                UpdateFilter();
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Tasks"));
            }
        }

        public void UpdateFilter()
        {
            CollectionViewSource cvs = new CollectionViewSource() { Source = tasks };
            ICollectionView cv = cvs.View;
            cv.Filter = o =>
            {
                BoardWindowRow p = o as BoardWindowRow;
                return (p.Description.ToUpper().Contains(SearchTerm.ToUpper()) || p.Title.ToUpper().Contains(SearchTerm.ToUpper()));
            };
            GridView = cv;
        }

        UserController service;
        InterfaceLayerBoard Ib;
        string boardName;

        public BoardWindowDataContext(UserController service,string boardName)
        {
            this.service = service;
            this.boardName = boardName;
            this.Ib = this.service.GetIBoard(boardName);
            ShowTheard();
        }

        public void SetIB(InterfaceLayerBoard newIB)
        {
            this.Ib = newIB;
            ShowTheard();
        }

        void ShowTheard()
        {
            //string BoardName = "My Board";
            ObservableCollection<BoardWindowRow> rows = new ObservableCollection<BoardWindowRow>();
            foreach (InterfaceLayerColumn Ic in this.Ib.Columns.ToArray())
            {
                foreach (InterfaceLayerTask task in Ic.Tasks.ToArray())
                {
                    rows.Add(new BoardWindowRow(task));
                }
            }
            Tasks = rows;

        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
