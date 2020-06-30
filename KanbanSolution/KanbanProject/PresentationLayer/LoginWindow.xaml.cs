using KanbanProject.InterfaceLayer;
using KanbanProject.PresentationLayer.ViewModel;
using System.Windows;

namespace KanbanProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        UserController service;
        LoginWindowDataContext LDC;


        public LoginWindow()
        {
            InitializeComponent();
            this.service = new UserController();
            this.LDC = new LoginWindowDataContext();
            this.DataContext = this.LDC;
        }


        private void CreateAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccountWin = new CreateAccount();
            createAccountWin.Show();
            this.Close();

        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (service.Login(LDC.Email, LDC.Password) != null) // Successfull login
            {
                BoardsWindow boardsWin = new BoardsWindow(this.service);
                boardsWin.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("One of the details is not correct!!");
                this.LDC.Email = "";
                this.LDC.Password = "";
            }
        }

        private void CloseAppBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
