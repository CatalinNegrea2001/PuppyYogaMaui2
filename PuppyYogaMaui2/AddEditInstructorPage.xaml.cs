using PuppyYogaMaui2.Models;
using PuppyYogaMaui2.Data;
using Microsoft.Maui.Controls;
using System;

namespace PuppyYogaMaui2
{
    [QueryProperty(nameof(InstructorId), "instructorId")]
    public partial class AddEditInstructorPage : ContentPage
    {
        private readonly PuppyYogaDatabase _database;
        private Instructor _currentInstructor;
        private int _instructorId;

        public int InstructorId
        {
            set
            {
                _instructorId = value;
                LoadInstructor(_instructorId);
            }
        }

        public AddEditInstructorPage(PuppyYogaDatabase database, int instructorId = 0)
        {
            InitializeComponent();
            _database = database ?? throw new ArgumentNullException(nameof(database), "A valid database must be provided.");

            if (instructorId != 0)
            {
                _instructorId = instructorId;
                LoadInstructor(instructorId);
            }
        }

        private async void LoadInstructor(int instructorId)
        {
            if (_database == null)
            {
                throw new InvalidOperationException("Database has not been initialized.");
            }

            _currentInstructor = await _database.GetInstructorAsync(instructorId);

            if (_currentInstructor != null)
            {
                FirstNameEntry.Text = _currentInstructor.FirstName;
                LastNameEntry.Text = _currentInstructor.LastName;
                BioEditor.Text = _currentInstructor.Bio;
            }
            else
            {
                Console.WriteLine($"Instructor with ID {instructorId} not found!");
            }
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            if (_database == null)
            {
                throw new InvalidOperationException("Database has not been initialized.");
            }

            if (_currentInstructor == null)
            {
                _currentInstructor = new Instructor(); 
            }

            _currentInstructor.FirstName = FirstNameEntry.Text;
            _currentInstructor.LastName = LastNameEntry.Text;
            _currentInstructor.Bio = BioEditor.Text;

            var validationErrors = ValidateInstructor(_currentInstructor);
            if (validationErrors.Any())
            {
                await DisplayAlert("Validation Error", validationErrors.First(), "OK");
                return;
            }

            await _database.SaveInstructorAsync(_currentInstructor);

            await Shell.Current.GoToAsync("///InstructorsPage");
        }

        private List<string> ValidateInstructor(Instructor instructor)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(instructor.FirstName))
                errors.Add("First name is required.");

            if (string.IsNullOrWhiteSpace(instructor.LastName))
                errors.Add("Last name is required.");

            if (string.IsNullOrWhiteSpace(instructor.Bio))
                errors.Add("Bio is required and cannot be empty.");

            if (instructor.Bio != null && instructor.Bio.Length > 500) 
                errors.Add("Bio must be less than 500 characters.");
           
            return errors;
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///InstructorsPage");
        }
    }
}
