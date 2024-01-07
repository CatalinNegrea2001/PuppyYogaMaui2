using PuppyYogaMaui2.Models;
using PuppyYogaMaui2.Data;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System;

namespace PuppyYogaMaui2
{
    public partial class InstructorsPage : ContentPage
    {
        private PuppyYogaDatabase _database;
        public ObservableCollection<Instructor> Instructors { get; set; }

        public InstructorsPage()
        {
            InitializeComponent();
            _database = MauiProgram.CreateMauiApp().Services.GetService<PuppyYogaDatabase>();
            Instructors = new ObservableCollection<Instructor>();
            InstructorsCollectionView.ItemsSource = Instructors;
            this.BindingContext = this;
            LoadInstructors();
        }

        public InstructorsPage(PuppyYogaDatabase database)
        {
            InitializeComponent();
            _database = database;
            Instructors = new ObservableCollection<Instructor>();
            InstructorsCollectionView.ItemsSource = Instructors;
            LoadInstructors();
        }

        private async void LoadInstructors()
        {
            var instructorsList = await _database.GetInstructorsAsync();
            Instructors.Clear();
            foreach (var instructor in instructorsList)
            {
                Instructors.Add(instructor);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadInstructors();
        }

        private async void OnAddInstructorClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditInstructorPage(_database));
        }

        private async void OnDeleteInstructorTapped(object sender, EventArgs e)
        {
            if (sender is Label deleteLabel && deleteLabel.BindingContext is Instructor instructorToDelete)
            {
                var result = await DisplayAlert("Confirm Deletion", $"Do you want to delete the instructor '{instructorToDelete.FirstName} {instructorToDelete.LastName}'?", "Yes", "No");

                if (result)
                {
                    int deleteResult = await _database.DeleteInstructorAsync(instructorToDelete);
                    if (deleteResult > 0)
                    {
                        Instructors.Remove(instructorToDelete);
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete instructor.");
                    }
                }
            }
        }

        private async void OnInstructorTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is Instructor selectedInstructor)
            {
                await Navigation.PushAsync(new AddEditInstructorPage(_database, selectedInstructor.InstructorId));
            }
        }

        private async void OnViewFeedbackClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Instructor selectedInstructor)
            {
                await Navigation.PushAsync(new InstructorFeedbackPage(selectedInstructor, _database));
            }
        }


        private async void OnAddFeedbackClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Instructor selectedInstructor)
            {
                await Navigation.PushAsync(new AddInstructorFeedbackPage(selectedInstructor, _database));
            }
        }
        


    }
}
