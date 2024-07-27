using Seoul_Stay.Entities;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Management.xaml
    /// </summary>
    public partial class Management : Window
    {
        public Management()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new Session1Entities())
            {
                if (ownerManagerTab.IsSelected)
                {
                    long userId = Properties.Settings.Default.UserID;
                    var listings = context.Items.Where(i => i.UserID == userId).Select(i => new ListingItem
                    {
                        Title = i.Title,
                        Capacity = i.Capacity,
                        Area = i.Area.Name,
                        Type = i.ItemType.Name
                    }).ToList();

                    /*travelerDataGrid.ItemsSource = listings;*/
                    ownerDataGrid.ItemsSource = listings;
                    /*statusText.Text = $"{listings.Count} items found.";*/
                } 
                else if (travelerTab.IsSelected)
                {
                    /*long userId = Properties.Settings.Default.UserID;*/
                    var listings = context.Items.Select(i => new ListingItem
                    {
                        Title = i.Title,
                        Capacity = i.Capacity,
                        Area = i.Area.Name,
                        Type = i.ItemType.Name
                    }).ToList();

                    travelerDataGrid.ItemsSource = listings;
                    /*ownerDataGrid.ItemsSource = listings;*/
                    statusText.Text = $"{listings.Count} items found.";
                }


            }
        }

            private void exitBtn_Click(object sender, RoutedEventArgs e)
            {
                Environment.Exit(1);
            }

            private void logoutBtn_Click(object sender, RoutedEventArgs e)
            {
                Welcome welcome = new Welcome();
                welcome.Show();
                this.Close();
            }

            private void searchBox_TextChanged(object sender,System.Windows.Controls.TextChangedEventArgs e)
            {

            TextBox textBox = sender as TextBox;
            string filter = textBox.Text.ToLower();

            using (var context = new Session1Entities())
            {
                var filteredListings = context.Items.
                    Where(i => i.Title.ToLower().Contains(filter) || 
                    i.Area.Name.ToLower().Contains(filter) ||
                    i.ItemType.Name.ToLower().Contains(filter))
                    .Select(i => new ListingItem
                    {
                        Title = i.Title,
                        Capacity = i.Capacity,
                        Area = i.Area.Name,
                        Type= i.ItemType.Name
                    }).ToList();

                travelerDataGrid.ItemsSource = filteredListings;
                /*ownerDataGrid.ItemsSource = filteredListings;*/
                statusText.Text = $"{filteredListings.Count} items found.";
            }

            }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddEditListing addEdit = new AddEditListing(false);
            addEdit.Show();
            this.Close();
        }

       


        /*private class SelectedItemType
        {
            public string Title { get; set; }
            public int Capacity { get; set; }   
            public string Area { get; set; }
            public string Type { get; set; }
        }*/

       


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("It works");
            var selectedItem = ownerDataGrid.SelectedItem as ListingItem;
            if (selectedItem != null)
            {
                /*var item = (SelectedItemType)selectedItem;*/
                using (var context = new Session1Entities())
                {
                    var itemToEdit = context.Items
                        .FirstOrDefault(i => i.Title == selectedItem.Title &&
                        i.Capacity == selectedItem.Capacity &&
                        i.Area.Name == selectedItem.Area &&
                        i.ItemType.Name == selectedItem.Type &&
                        i.UserID == Properties.Settings.Default.UserID
                      
                        );

                    if (itemToEdit != null)
                    {
                        AddEditListing editListing = new AddEditListing(true, itemToEdit);
                        editListing.Show();
                        this.Close();
                    }
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
            if (travelerTab.IsSelected)
            {
                statusText.Visibility = Visibility.Visible;
            } else
            {
                statusText.Visibility = Visibility.Hidden;
            }
        }
    }
    } 

