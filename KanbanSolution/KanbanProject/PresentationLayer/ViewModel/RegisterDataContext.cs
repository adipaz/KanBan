using System.ComponentModel;

namespace KanbanProject.PresentationLayer.ViewModel
{
    class RegisterDataContext : INotifyPropertyChanged
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


        string passwordRep = "";
        public string PasswordRep
        {
            get
            {
                return passwordRep;
            }
            set
            {
                passwordRep = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("passwordRep"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public RegisterDataContext()
        {

        }

    }
}
