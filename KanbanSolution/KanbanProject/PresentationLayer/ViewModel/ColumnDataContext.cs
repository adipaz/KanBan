using KanbanProject.BusinessLayer;
using KanbanProject.InterfaceLayer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace KanbanProject.PresentationLayer.ViewModel
{
    class ColumnDataContext : INotifyPropertyChanged
    {
        string columnname = "";
        public string ColumnName
        {
            get
            {
                return columnname;
            }
            set
            {
                columnname = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("columnName"));
            }
        }

        ObservableCollection<ComboBoxItem> cbitems;
        public ObservableCollection<ComboBoxItem> CBItems
        {
            get
            {
                return cbitems;
            }
            set
            {
                cbitems = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("cbitems"));
            }
        }

        ComboBoxItem columnname1;
        public ComboBoxItem ColumnName1
        {
            get
            {
                return columnname1;
            }
            set
            {
                columnname1 = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("columnname1"));
            }
        }

        string lim;
        public string Lim
        {
            get
            {
                return lim;
            }
            set
            {
                lim = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("lim"));
            }
        }

        ComboBoxItem columnname2;
        public ComboBoxItem ColumnName2
        {
            get
            {
                return columnname2;
            }
            set
            {
                columnname2 = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("columnname2"));
            }
        }

        UserController service;
        string boardName;

        public ColumnDataContext(UserController service,string boardName)
        {
            this.service = service;
            this.CBItems = new ObservableCollection<ComboBoxItem>();
            this.boardName = boardName;
            this.service.InitializeColumns(this.CBItems, this.boardName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
