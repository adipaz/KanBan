using System;
using System.ComponentModel;

namespace KanbanProject.PresentationLayer.ViewModel
{
    class TaskWindowDataContext : INotifyPropertyChanged
    {
        string title = "";
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
            }
        }

        string description = "";
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        DateTime dueDate = DateTime.Now;
        public DateTime DueDate
        {
            get
            {
                return dueDate;
            }
            set
            {
                dueDate = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DueDate"));
            }
        }


        public TaskWindowDataContext()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
