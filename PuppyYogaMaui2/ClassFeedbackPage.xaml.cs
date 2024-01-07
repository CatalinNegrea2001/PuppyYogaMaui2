using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using PuppyYogaMaui2.Models;
using PuppyYogaMaui2.Data;
using System.Linq;

namespace PuppyYogaMaui2
{
    public partial class ClassFeedbackPage : ContentPage
    {
        private YogaClass _class;
        private PuppyYogaDatabase _database;
        public ObservableCollection<ClassFeedback> FeedbackList { get; set; }

        public ClassFeedbackPage(YogaClass yogaClass, PuppyYogaDatabase database)
        {
            InitializeComponent();
            _class = yogaClass;
            _database = database;
            FeedbackList = new ObservableCollection<ClassFeedback>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var feedbacks = await _database.GetClassFeedbackAsync(_class.Id);
            FeedbackList.Clear();
            foreach (var feedback in feedbacks)
            {
                FeedbackList.Add(feedback);
            }
        }

        private async void OnFeedbackSelected(object sender, SelectionChangedEventArgs e)
        {
            var feedback = e.CurrentSelection.FirstOrDefault() as ClassFeedback;
            if (feedback != null)
            {
                
                await Navigation.PushAsync(new AddClassFeedbackPage(_class, _database, feedback));
            }
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
