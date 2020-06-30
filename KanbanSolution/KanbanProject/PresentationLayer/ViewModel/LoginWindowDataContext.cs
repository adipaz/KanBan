using System.ComponentModel;

namespace KanbanProject.PresentationLayer.ViewModel
{
    public class LoginWindowDataContext : INotifyPropertyChanged
    {
        string email = "";
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("email"));
            }
        }

        string password = "";
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("password"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;



        public LoginWindowDataContext()
        {

        }
    }
}
