using Seoul_Stay.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Seoul_Stay
{
    /// <summary>
    /// Interaction logic for Create_account.xaml
    /// </summary>
    public partial class Create_account : Window
    {
        public Create_account()
        {
            InitializeComponent();
        }

        

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string userna = username.Text;
            string name = fullname.Text;
            DateTime? birth = birthday.SelectedDate;
            string pass = passsword.Password;
            int fam = int.Parse(numberFamily.Text);
            bool gen = true;
            if (maleRadio.IsChecked == true)
            {
                 gen = true;
            }
            else
            {
                gen = false;
            }

            if (username.Text == string.Empty || fullname.Text == string.Empty || passsword.Password == string.Empty || passswordRetype.Password == string.Empty || birthday == null || (maleRadio.IsChecked == false && femaleRadio.IsChecked == false))
            {
                MessageBox.Show("Please fill all things or your passwords are different or agree with Terms");

            }

            else if (passsword.Password != passswordRetype.Password)
            {
                MessageBox.Show("Your passwords are different");
            }
            else if (passsword.Password.Length < 5)
            {
                MessageBox.Show("Your password needs to be 5 symbols or higher");
            }
            else if (termsCheck.IsChecked == false)
            {
                MessageBox.Show("Do you agree with Terms and Conditions");
            }
            else
            {

                using (var context = new Session1Entities())
                {
                    var existingUser = context.Users.FirstOrDefault(x => x.Username == userna);
                    if (existingUser != null)
                    {
                        MessageBox.Show("Error Message", "Username already existed", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var user = new User
                    {
                        Username = userna,
                        Password = pass,
                        UserTypeID = 2,
                        FullName = name,
                        BirthDate = birth.Value,
                        FamilyCount = fam,
                        Gender = gen,
                        GUID = Guid.NewGuid()

                    };

                    context.Users.Add(user);
                    context.SaveChanges();

                    Properties.Settings.Default.UserID = user.ID;
                    Properties.Settings.Default.Save();
                }

                MessageBox.Show("User added successfully!");
                Management management = new Management();
                management.Show();
                this.Close();
            }
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string termsFilePath = "C:\\Users\\amadi\\Documents\\WorldSkills\\Seoul Stay\\Seoul Stay\\photos\\Terms.txt";
                if (File.Exists(termsFilePath))
                {
                    string terms = File.ReadAllText(termsFilePath);
                    MessageBox.Show(terms, "Terms and Conditions");
                    termsCheck.IsEnabled = true;
                } else
                {
                    MessageBox.Show("Terms and Conditions file not found.", "Error");
                }
            } catch  (Exception ex)
            { MessageBox.Show("An error occurred while reading the Terms and Conditions: " + ex.Message, "Error"); }

            }

        

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Welcome welcome = new Welcome();
            welcome.Show();
            this.Close();
        }
    }
    }
        
