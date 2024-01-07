using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using PuppyYogaMaui2.Models;
using PuppyYogaMaui2.Data;
namespace PuppyYogaMaui2
{
    public partial class InstructorFeedbackPage : ContentPage
    {
        private Instructor _instructor;
        private PuppyYogaDatabase _database;
        public ObservableCollection<InstructorFeedback> FeedbackList { get; set; }

        public InstructorFeedbackPage(Instructor instructor, PuppyYogaDatabase database)
        {
            InitializeComponent();
            _instructor = instructor;
            _database = database;
            FeedbackList = new ObservableCollection<InstructorFeedback>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var feedbacks = await _database.GetInstructorFeedbackAsync(_instructor.InstructorId);
            FeedbackList.Clear();
            foreach (var feedback in feedbacks)
            {
                FeedbackList.Add(feedback);
            }
        }
        private async void OnFeedbackSelected(object sender, SelectionChangedEventArgs e)
        {
            var feedback = e.CurrentSelection.FirstOrDefault() as InstructorFeedback;
            if (feedback != null)
            {
                await Navigation.PushAsync(new AddInstructorFeedbackPage(_instructor, _database, feedback));
            }
           ((CollectionView)sender).SelectedItem = null;
        }


    }

}
