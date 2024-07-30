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
        private List<Item> _allItems;
        private List<Item> _allItems2;
        private Session1Entities _context;

        public Management()
        {
            InitializeComponent();
            _context = new Session1Entities();
            LoadData();

          

            
        }

        private void LoadData()
        {
            _allItems = _context.Items.Include("Area").Include("ItemType").ToList();
            _allItems2 = _context.Items.Include("Area").Include("ItemType").
                Where(a => a.UserID == Properties.Settings.Default.UserID).ToList();
            DataGridTraveller.ItemsSource = _allItems;
            DataGridOwnerManager.ItemsSource = _allItems2;
            UpdateItemCountText(DataGridTraveller.Items.Count);
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.KeepMeSignedIn = false;
            Properties.Settings.Default.username = string.Empty;
            Properties.Settings.Default.password = string.Empty;
            Properties.Settings.Default.Save();
            var welcomeWindow = new Welcome();
            welcomeWindow.Show();
            this.Close();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnAddListing_Click(object sender, RoutedEventArgs e)
        {
            var addwindow = new AddEditListing(false);
            addwindow.Show();
            this.Close();

        }

        private void BtnEditClick_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.DataContext as Item;
            if (item != null)
            {
                var welcomeWindow = new AddEditListing(true, item);
                welcomeWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("ERROR");
            }
        }

        private void TabItemOwnerManager_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UpdateItemCountText(DataGridOwnerManager.Items.Count);
        }

        private void TabItemTraveller_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UpdateItemCountText(DataGridTraveller.Items.Count);
        }

        private void SearchTextBoxTraveller_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = SearchTextBoxTraveller.Text.ToLower();
            var filteredItems = _allItems.Where(item =>
                item.Title.ToLower().Contains(filterText) ||
                item.Area.Name.ToLower().Contains(filterText) ||
                item.ItemType.Name.ToLower().Contains(filterText)).ToList();

            DataGridTraveller.ItemsSource = filteredItems;
            
            UpdateItemCountText(filteredItems.Count);
        }

        private void UpdateItemCountText(int count)
        {
            ItemsFoundText.Text = $"{count} items found";
        }
    }
}