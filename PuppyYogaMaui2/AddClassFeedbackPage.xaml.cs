using Microsoft.Maui.Controls;
using System;
using PuppyYogaMaui2.Models;
using PuppyYogaMaui2.Data;

namespace PuppyYogaMaui2
{
    public partial class AddClassFeedbackPage : ContentPage
    {
        private YogaClass _class;
        private PuppyYogaDatabase _database;
        private ClassFeedback _existingFeedback;

        public AddClassFeedbackPage(YogaClass yogaClass, PuppyYogaDatabase database, ClassFeedback existingFeedback = null)
        {
            InitializeComponent();
            _class = yogaClass;
            _database = database;
            _existingFeedback = existingFeedback;

            if (_existingFeedback != null)
            {
                RatingEntry.Text = _existingFeedback.Rating.ToString();
                CommentEditor.Text = _existingFeedback.Comment;
                SubmitButton.Text = "Update Feedback";
            }
        }

        private async void OnSubmitFeedbackClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RatingEntry.Text) || string.IsNullOrWhiteSpace(CommentEditor.Text))
            {
                await DisplayAlert("Error", "Both rating and comment are required.", "OK");
                return;
            }

            if (!int.TryParse(RatingEntry.Text, out int rating) || rating < 1 || rating > 5)
            {
                await DisplayAlert("Error", "Rating must be a number between 1 and 5.", "OK");
                return;
            }

            if (_existingFeedback != null)
            {
                _existingFeedback.Rating = rating;
                _existingFeedback.Comment = CommentEditor.Text;
                _existingFeedback.Date = DateTime.UtcNow;
                await _database.UpdateClassFeedbackAsync(_existingFeedback);
                await DisplayAlert("Success", "Your feedback has been updated.", "OK");
            }
            else
            {
                var newFeedback = new ClassFeedback
                {
                    YogaClassId = _class.Id,
                    Rating = rating,
                    Comment = CommentEditor.Text,
                    Date = DateTime.UtcNow
                };
                await _database.SaveClassFeedbackAsync(newFeedback);
                await DisplayAlert("Success", "Your feedback has been submitted.", "OK");
            }

            try
            {
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare de Navigare", "Nu s-a putut naviga înapoi: " + ex.Message, "OK");
            }
        }
    }
}
