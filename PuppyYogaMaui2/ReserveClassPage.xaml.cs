using Microsoft.Maui.Controls;
using System;
using PuppyYogaMaui2.Models;
using PuppyYogaMaui2.Data;
using System.Threading.Tasks;

namespace PuppyYogaMaui2
{
    public partial class ReserveClassPage : ContentPage
    {
        private YogaClass selectedClass;
        private PuppyYogaDatabase database;

        public ReserveClassPage(YogaClass selectedClass)
        {
            InitializeComponent();
            this.selectedClass = selectedClass;
            this.BindingContext = new Reservation();

            database = (Application.Current as App)?.Services.GetService<PuppyYogaDatabase>();

            ClassName.Text = selectedClass.Name;
            ClassDate.Text = $"Data: {selectedClass.ScheduleDate:dd/MM/yyyy}";
            ClassTime.Text = $"Ora: {selectedClass.ScheduleTime:HH:mm}";
        }

        private async void OnReserveClassClicked(object sender, EventArgs e)
        {
            string name = NameEntry.Text;
            string phone = PhoneEntry.Text;
            string email = EmailEntry.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Error", "All fields must be filled.", "OK");
                return;
            }

            var reservation = new Reservation
            {
                YogaClassId = selectedClass.Id,
                CustomerName = name,
                PhoneNumber = PhoneEntry.Text,
                Email = EmailEntry.Text,
                ClassName = selectedClass.Name,
                ClassDate = selectedClass.ScheduleDate,
                ClassTime = selectedClass.ScheduleTime
            };

            if (!ValidationUtils.IsValidPhoneNumber(reservation.PhoneNumber))
            {
                await DisplayAlert("Error", "Invalid phone number. It must be 10 digits.", "OK");
                return;
            }

            if (!ValidationUtils.IsValidEmail(reservation.Email))
            {
                await DisplayAlert("Error", "Invalid email address.", "OK");
                return;
            }

            bool isSaved = await SaveReservation(reservation);
            if (isSaved)
            {
                await DisplayAlert("Success", "Your reservation has been successfully recorded!", "OK");
                await Navigation.PopAsync();
                MessagingCenter.Send<App>((App)Application.Current, "NewReservationAdded");
            }
            else
            {
                await DisplayAlert("Error", "There was a problem saving the reservation.", "OK");
            }
        }


        private async Task<bool> SaveReservation(Reservation reservation)
        {
            try
            {
                bool isSaved = await database.SaveReservationAsync(reservation);
                if (isSaved)
                {
                    MessagingCenter.Send<App>((App)Application.Current, "NewReservationAdded");
                    return true;
                }
                else
                {
                    Console.WriteLine("Rezervarea nu a fost salvată.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving reservation: {ex.Message}");
                return false;
            }
        }
    }
}
