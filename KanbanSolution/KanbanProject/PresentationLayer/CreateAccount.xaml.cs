using KanbanProject.InterfaceLayer;
using KanbanProject.PresentationLayer.ViewModel;
using System.Windows;

namespace KanbanProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {
        RegisterDataContext rdc;

        public CreateAccount()
        {
            InitializeComponent();
            this.rdc = new RegisterDataContext();
            this.DataContext = this.rdc;

        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserController service = new UserController();

            if (this.rdc.Password.Equals(this.rdc.PasswordRep))
            {
                if (service.Register(this.rdc.Email, this.rdc.Password) != null)
                {

                    LoginWindow loginWin = new LoginWindow();
                    MessageBox.Show("Registered succesfully");
                    loginWin.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Registration could not been made!!\n1.Check if you Already Registered with same Email\n2.Check input Email and password\n[notice : password must be 4-20 chars long, including letter, Cap letter and a number]");
                }
            }
            else
            {
                this.rdc.Password = "";
                this.rdc.PasswordRep = "";
                MessageBox.Show("the typed passwords are not the same!");
            }
        }


        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow win = new LoginWindow();
            win.Show();
            this.Close();
        }
    }
}
