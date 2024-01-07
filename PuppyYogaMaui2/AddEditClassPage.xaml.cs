using PuppyYogaMaui2.Models;
using PuppyYogaMaui2.Data;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PuppyYogaMaui2
{
    [QueryProperty(nameof(ClassId), "classId")]
    public partial class AddEditClassPage : ContentPage
    {
        private readonly PuppyYogaDatabase _database;
        private YogaClass _currentClass;
        private string _classId;
        public ObservableCollection<Instructor> Instructors { get; private set; }

        public string ClassId
        {
            set
            {
                _classId = Uri.UnescapeDataString(value ?? string.Empty);
                int.TryParse(_classId, out int classId);
                LoadClass(classId);
            }
        }

        public AddEditClassPage(PuppyYogaDatabase database)
        {
            InitializeComponent();
            _database = database;
            Instructors = new ObservableCollection<Instructor>();
            InstructorPicker.ItemsSource = Instructors;
            LoadInstructors();
        }

        public AddEditClassPage(PuppyYogaDatabase database, int classId) : this(database)
        {
            _classId = classId.ToString();
            LoadClass(classId);
            LoadInstructors();
        }

        private async void LoadClass(int classId)
        {
            _currentClass = await _database.GetClassAsync(classId);

            if (_currentClass != null)
            {
                NameEntry.Text = _currentClass.Name;
                DescriptionEntry.Text = _currentClass.Description;
                LocationEntry.Text = _currentClass.Location;
                scheduleDatePicker.Date = _currentClass.ScheduleDate;
                scheduleTimePicker.Time = _currentClass.ScheduleTime.TimeOfDay;
                PriceEntry.Text = _currentClass.Price.ToString();
                MaxCapacityEntry.Text = _currentClass.MaxCapacity.ToString();

                await LoadInstructors();

                var selectedInstructor = Instructors.FirstOrDefault(i => i.InstructorId == _currentClass.InstructorId);
                if (selectedInstructor != null)
                {
                    InstructorPicker.SelectedItem = selectedInstructor;
                }
                else
                {
                    Console.WriteLine($"Instructor with ID {_currentClass.InstructorId} not found!");
                }
            }
            else
            {
                Console.WriteLine($"Class with ID {classId} not found!");
            }
        }


        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _currentClass = _currentClass ?? new YogaClass();

            _currentClass.Name = NameEntry.Text;
            _currentClass.Description = DescriptionEntry.Text;
            _currentClass.Location = LocationEntry.Text;
            _currentClass.ScheduleDate = scheduleDatePicker.Date;
            _currentClass.ScheduleTime = new DateTime(scheduleTimePicker.Time.Ticks);
            _currentClass.Price = double.TryParse(PriceEntry.Text, out double price) ? price : 0;
            _currentClass.MaxCapacity = int.TryParse(MaxCapacityEntry.Text, out int maxCapacity) ? maxCapacity : 0;

            if (InstructorPicker.SelectedItem is Instructor selectedInstructor)
            {
                _currentClass.InstructorId = selectedInstructor.InstructorId;
            }

            var validationErrors = ValidateYogaClass(_currentClass);
            if (validationErrors.Any())
            {
                await DisplayAlert("Validation Error", validationErrors.First(), "OK");
                return;
            }

            int result = await _database.SaveClassAsync(_currentClass);

            if (result > 0)
            {
                await Shell.Current.GoToAsync("///ClassesPage");
            }
            else
            {
                Console.WriteLine("Failed to save or update class.");
            }
        }

        private List<string> ValidateYogaClass(YogaClass yogaClass)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(yogaClass.Name))
                errors.Add("Name is required.");
            if (string.IsNullOrWhiteSpace(yogaClass.Description))
                errors.Add("Description is required.");
            if (string.IsNullOrWhiteSpace(yogaClass.Location))
                errors.Add("Location is required.");
            if (yogaClass.ScheduleDate == default)
                errors.Add("Schedule Date is required.");
            if (yogaClass.ScheduleTime == default)
                errors.Add("Schedule Time is required.");
            if (yogaClass.Price <= 0)
                errors.Add("Price must be greater than 0.");
            if (yogaClass.MaxCapacity <= 0)
                errors.Add("Max Capacity must be greater than 0.");
            if (yogaClass.InstructorId <= 0)
                errors.Add("An Instructor must be selected.");

            return errors;
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///ClassesPage");
        }

        private async Task LoadInstructors()
        {
            Instructors.Clear();
            var instructorsList = await _database.GetInstructorsAsync();
            foreach (var instructor in instructorsList)
            {
                Instructors.Add(instructor);
            }
        }

    }
}
