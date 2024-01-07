using Microsoft.Maui.Controls;
using System;
using PuppyYogaMaui2.Models;
using PuppyYogaMaui2.Data;
using System.Collections.ObjectModel;

namespace PuppyYogaMaui2
{
    public partial class ReservationsListPage : ContentPage
    {
        private PuppyYogaDatabase database;
        private ObservableCollection<Reservation> reservations;

        public ReservationsListPage()
        {
            InitializeComponent();
            database = (Application.Current as App)?.Services.GetService<PuppyYogaDatabase>();
            reservations = new ObservableCollection<Reservation>();
            ReservationsCollectionView.ItemsSource = reservations;
            LoadReservations();

            MessagingCenter.Subscribe<App>((App)Application.Current, "NewReservationAdded", (sender) =>
            {
                RefreshReservations(); 
            });
        }

        private async void RefreshReservations()
        {
            var reservationsList = await database.GetReservationsAsync();
            Device.BeginInvokeOnMainThread(() =>
            {
                reservations.Clear();
                foreach (var reservation in reservationsList)
                {
                    reservations.Add(reservation);
                }
            });
        }
        private async void LoadReservations()
        {
            try
            {
                var reservationsList = await database.GetReservationsAsync();
                foreach (var reservation in reservationsList)
                {
                    reservations.Add(reservation);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading reservations: {ex.Message}");
            }
        }

        private async void OnDeleteReservationTapped(object sender, EventArgs e)
        {
            if (sender is Label deleteLabel && deleteLabel.BindingContext is Reservation reservationToDelete)
            {
                var result = await DisplayAlert("Confirm Deletion", $"Do you want to delete the reservation '{reservationToDelete.CustomerName} {reservationToDelete.Email}{reservationToDelete.PhoneNumber}'?", "Yes", "No");

                if (result)
                {
                    int deleteResult = await database.DeleteReservationAsync(reservationToDelete);
                    if (deleteResult > 0)
                    {
                        reservations.Remove(reservationToDelete); // Remove from ObservableCollection
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete reservation.");
                    }
                }
            }
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App>((App)Application.Current, "NewReservationAdded");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshReservations(); 
        }
       
    }
}
