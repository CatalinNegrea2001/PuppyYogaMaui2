using PuppyYogaMaui2.Models;
using PuppyYogaMaui2.Data;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System;
using System.Linq;

namespace PuppyYogaMaui2
{
    public partial class ClassesPage : ContentPage
    {
        private readonly PuppyYogaDatabase _database;

        public ObservableCollection<YogaClass> Classes { get; set; }

        public ClassesPage()
        {
            InitializeComponent();
            _database = MauiProgram.CreateMauiApp().Services.GetService<PuppyYogaDatabase>();
            Classes = new ObservableCollection<YogaClass>();
            ClassesCollectionView.ItemsSource = Classes;
            this.BindingContext = this;
            LoadClasses();
        }

        public ClassesPage(PuppyYogaDatabase database)
        {
            InitializeComponent();
            _database = database;
            Classes = new ObservableCollection<YogaClass>();
            ClassesCollectionView.ItemsSource = Classes;
            LoadClasses();
        }

        private async void LoadClasses()
        {
            var classesList = await _database.GetClassesAsync();
            var instructorsList = await _database.GetInstructorsAsync(); 

            Classes.Clear(); 

            foreach (var yogaClass in classesList)
            {
                // Find the instructor for each class
                var instructor = instructorsList.FirstOrDefault(i => i.InstructorId == yogaClass.InstructorId);
                yogaClass.InstructorName = instructor?.FullName ?? "No Instructor Assigned";
                Classes.Add(yogaClass);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadClasses(); 
        }

        private async void OnAddClassClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditClassPage(_database));
        }

        private async void OnClassTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is YogaClass selectedClass)
            {
                await Navigation.PushAsync(new AddEditClassPage(_database, selectedClass.Id));
            }
        }

        private async void OnDeleteClassTapped(object sender, EventArgs e)
        {
            if (sender is Label deleteLabel && deleteLabel.BindingContext is YogaClass classToDelete)
            {
                var result = await DisplayAlert("Confirm Deletion", $"Do you want to delete the class '{classToDelete.Name}'?", "Yes", "No");

                if (result)
                {
                    int deleteResult = await _database.DeleteClassAsync(classToDelete);

                    if (deleteResult > 0)
                    {
                        Classes.Remove(classToDelete); 
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete class.");
                    }
                }
            }
        }

        private async void OnReserveButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is YogaClass selectedClass)
            {
                await Navigation.PushAsync(new ReserveClassPage(selectedClass));
            }
        }
        private async void OnViewFeedbackClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is YogaClass selectedClass)
            {
                await Navigation.PushAsync(new ClassFeedbackPage(selectedClass, _database));
            }
        }
        private async void OnAddFeedbackClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is YogaClass selectedClass)
            {
                await Navigation.PushAsync(new AddClassFeedbackPage(selectedClass, _database));
            }
        }

       
    }
}
