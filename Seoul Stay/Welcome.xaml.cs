using Seoul_Stay.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Seoul_Stay
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        public Welcome()
        {
            InitializeComponent();

            // Load settings
            if (Properties.Settings.Default.KeepMeSignedIn)
            {
                employeeField.Text = Properties.Settings.Default.employee;
                usernameField.Text = Properties.Settings.Default.username;
                passwordField.Password = Properties.Settings.Default.password;
                keepMeSignedIn.IsChecked = true;
            }
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameField.Text;
            string password = passwordField.Password;
            string employee = employeeField.Text;

            using (var context = new Session1Entities())
            {
                User user = null; // Объявление и инициализация переменной user значением null

                if (!string.IsNullOrEmpty(employee) && !string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("Пожалуйста, заполните только одно поле: либо Employee, либо Username.");
                    return;
                }
                else if (!string.IsNullOrEmpty(employee))
                {
                    user = context.Users
                        .FirstOrDefault(x => x.Username == employee && x.Password == password);
                }
                else if (!string.IsNullOrEmpty(username))
                {
                    user = context.Users
                        .FirstOrDefault(x => x.Username == username && x.Password == password);
                }
                else
                {
                    MessageBox.Show("Пожалуйста, заполните необходимые поля.");
                    return;
                }

                if (user != null)
                {
                    Properties.Settings.Default.UserID = user.ID;
                    Properties.Settings.Default.Save();

                    if (keepMeSignedIn.IsChecked == true)
                    {
                        Properties.Settings.Default.username = username;
                        Properties.Settings.Default.employee = employee;
                        Properties.Settings.Default.password = password;
                        Properties.Settings.Default.KeepMeSignedIn = true;
                    }
                    else
                    {
                        Properties.Settings.Default.username = string.Empty;
                        Properties.Settings.Default.username = string.Empty;
                        Properties.Settings.Default.password = string.Empty;
                        Properties.Settings.Default.KeepMeSignedIn = false;
                    }
                    Properties.Settings.Default.Save();

                    Management management = new Management();
                    management.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверное имя пользователя или пароль");
                }
            }
        }


        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            Create_account create = new Create_account();
            create.Show();
            this.Close();
        }

        private void showPassword_Checked(object sender, RoutedEventArgs e)
        {
            passwordFieldText.Visibility = Visibility.Visible;
            passwordField.Visibility = Visibility.Hidden;
            passwordFieldText.Text = passwordField.Password;
        }

        private void showPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            passwordFieldText.Visibility = Visibility.Hidden;
            passwordField.Visibility = Visibility.Visible;
            passwordField.Password = passwordFieldText.Text;
        }
    }
}
