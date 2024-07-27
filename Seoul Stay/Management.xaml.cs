using Seoul_Stay.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Seoul_Stay
{
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

                    ownerDataGrid.ItemsSource = listings;
                }
                else if (travelerTab.IsSelected)
                {
                    var listings = context.Items.Select(i => new ListingItem
                    {
                        Title = i.Title,
                        Capacity = i.Capacity,
                        Area = i.Area.Name,
                        Type = i.ItemType.Name
                    }).ToList();

                    travelerDataGrid.ItemsSource = listings;
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

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
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
                        Type = i.ItemType.Name
                    }).ToList();

                travelerDataGrid.ItemsSource = filteredListings;
                statusText.Text = $"{filteredListings.Count} items found.";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddEditListing addEdit = new AddEditListing(false);
            addEdit.Show();
            this.Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit button clicked");

            if (ownerDataGrid.SelectedItem is ListingItem selectedItem)
            {
                MessageBox.Show($"Selected item: {selectedItem.Title}");

                using (var context = new Session1Entities())
                {
                    var itemToEdit = context.Items
                        .FirstOrDefault(i => i.Title == selectedItem.Title &&
                                             i.Capacity == selectedItem.Capacity &&
                                             i.Area.Name == selectedItem.Area &&
                                             i.ItemType.Name == selectedItem.Type &&
                                             i.UserID == Properties.Settings.Default.UserID);

                    if (itemToEdit != null)
                    {
                        MessageBox.Show("Item found. Opening edit window.");
                        AddEditListing editListing = new AddEditListing(true, itemToEdit);
                        editListing.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Item not found in the database.");
                    }
                }
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }
    }
}
