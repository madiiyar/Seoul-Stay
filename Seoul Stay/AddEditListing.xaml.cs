using Seoul_Stay.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Seoul_Stay
{
    /// <summary>
    /// Interaction logic for AddEditListing.xaml
    /// </summary>
    public partial class AddEditListing : Window
    {

        private readonly bool _isEditMode;
        private readonly Seoul_Stay.Entities.Item _itemToEdit;

        public ObservableCollection<AmenityItem> Amenities { get; set; }
       

        public AddEditListing(bool isEditMode, Seoul_Stay.Entities.Item item = null)
        {
            InitializeComponent();
            _isEditMode = isEditMode;
            _itemToEdit = item;

            LoadAmenities();
            LoadAreas();
            LoadItemTypes();
            if (_isEditMode && _itemToEdit != null)
            {
                InitializeFieldsForEditing();
                closeFinishButton.Content = "Close";
                nextButton.Visibility = Visibility.Hidden;
            }
            else
            {
                closeFinishButton.Content = "Finish";
                nextButton.Visibility= Visibility.Visible;
            }
        }

        private void InitializeFieldsForEditing()
        {

            titleTextBox.Text = _itemToEdit.Title;
            capacityTextBox.Text = _itemToEdit.Capacity.ToString();
            bedsTextBox.Text = _itemToEdit.NumberOfBeds.ToString();
            bedroomsTextBox.Text = _itemToEdit.NumberOfBedrooms.ToString();
            bathroomsTextBox.Text = _itemToEdit.NumberOfBathrooms.ToString();
            approxAddressTextBox.Text = _itemToEdit.ApproximateAddress;
            exactAddressTextBox.Text = _itemToEdit.ExactAddress;
            descriptionTextBox.Text = _itemToEdit.Description;
            hostRulesTextBox.Text = _itemToEdit.HostRules;
            minNightsTextBox.Text = _itemToEdit.MinimumNights.ToString();
            maxNightsTextBox.Text = _itemToEdit.MaximumNights.ToString();
            typeComboBox.SelectedValue = _itemToEdit.ItemTypeID;
            areaComboBox.SelectedValue= _itemToEdit.AreaID;

        }

        private void LoadAmenities()
        {
            using (var context = new Session1Entities())
            {
                var allAmenities = context.Amenities.Select(a => new AmenityItem{
                    ID = (int)a.ID,
                    Name = a.Name,
                    IsSelected = false
                }).ToList();

                if (_isEditMode && _itemToEdit != null)
                {
                    var selectedAmenityIds = context.ItemAmenities.
                        Where(ia => ia.ItemID == _itemToEdit.ID).
                        Select(ia => (int)ia.AmenityID).
                        ToList();

                    foreach (var amenity in allAmenities)
                    {
                        if (selectedAmenityIds.Contains(amenity.ID))
                        {
                            amenity.IsSelected = true;
                        }
                    }
                }
                Amenities = new ObservableCollection<AmenityItem>(allAmenities);
                amenitiesDataGrid.ItemsSource = Amenities;
            }
        }

        private void LoadItemTypes()
        {
            using (var context = new Session1Entities())
            {
                var itemTypes = context.ItemTypes.ToList();
                typeComboBox.ItemsSource = itemTypes;
                typeComboBox.DisplayMemberPath = "Name";
                typeComboBox.SelectedValuePath = "ID";
            }
        }




        private void LoadAreas()
        {
            using (var context = new Session1Entities())
            {
                var areas = context.Areas.Select(a => new
                {
                    a.ID,
                    a.Name
                }).ToList();

                areaComboBox.ItemsSource = areas;
                areaComboBox.DisplayMemberPath = "Name";
                areaComboBox.SelectedValuePath = "ID";
            }
        }


        private void closeFinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isEditMode)
            {
                using (var context = new Session1Entities())
                {
                    var itemToUpdate = context.Items.FirstOrDefault(i => i.ID == _itemToEdit.ID);
                    if (itemToUpdate != null)
                    {
                        itemToUpdate.Title = titleTextBox.Text;
                        itemToUpdate.Capacity = int.Parse(capacityTextBox.Text);
                        itemToUpdate.NumberOfBeds = int.Parse(bedsTextBox.Text);
                        itemToUpdate.NumberOfBedrooms = int.Parse(bedroomsTextBox.Text);
                        itemToUpdate.NumberOfBathrooms = int.Parse(bathroomsTextBox.Text);
                        itemToUpdate.ApproximateAddress = approxAddressTextBox.Text;
                        itemToUpdate.ExactAddress = exactAddressTextBox.Text;
                        itemToUpdate.Description = descriptionTextBox.Text;
                        itemToUpdate.HostRules = hostRulesTextBox.Text;
                        itemToUpdate.MinimumNights = int.Parse(minNightsTextBox.Text);
                        itemToUpdate.MaximumNights = int.Parse(maxNightsTextBox.Text);
                        itemToUpdate.ItemTypeID = (long)typeComboBox.SelectedValue;
                        itemToUpdate.AreaID = (long)areaComboBox.SelectedValue;


                        var existingAmenities = context.ItemAmenities.Where(ia => ia.ItemID == itemToUpdate.ID).ToList();
                        context.ItemAmenities.RemoveRange(existingAmenities);


                        foreach (var amenity in Amenities.Where(a => a.IsSelected))
                        {
                            context.ItemAmenities.Add(new ItemAmenity
                            {
                                ItemID = itemToUpdate.ID,
                                AmenityID = amenity.ID,
                            });

                        }


                        context.SaveChanges();
                    }
                }
            }
            else
            {
                using(var context = new Session1Entities())
                {
                    var newItem = new Seoul_Stay.Entities.Item
                    {
                        Title = titleTextBox.Text,
                        Capacity = int.Parse(capacityTextBox.Text),
                        NumberOfBeds = int.Parse(bedsTextBox.Text),
                        NumberOfBedrooms = int.Parse(bedroomsTextBox.Text),
                        NumberOfBathrooms = int.Parse(bathroomsTextBox.Text),
                        ApproximateAddress = approxAddressTextBox.Text,
                        ExactAddress = exactAddressTextBox.Text,
                        Description = descriptionTextBox.Text,
                        HostRules = hostRulesTextBox.Text,
                        MinimumNights = int.Parse(minNightsTextBox.Text),
                        MaximumNights = int.Parse(maxNightsTextBox.Text),
                        ItemTypeID = (long)typeComboBox.SelectedValue,
                        AreaID = (long)areaComboBox.SelectedValue,
                        UserID = Properties.Settings.Default.UserID,
                        GUID = Guid.NewGuid()

                    };

                    context.Items.Add(newItem);
                    context.SaveChanges();


                    foreach (var amenity in Amenities.Where(a => a.IsSelected))
                    {
                        context.ItemAmenities.Add(new ItemAmenity
                        {
                            ItemID = newItem.ID,
                            AmenityID = amenity.ID
                        });
                    }
                    context.SaveChanges();
                }
                
            }
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
