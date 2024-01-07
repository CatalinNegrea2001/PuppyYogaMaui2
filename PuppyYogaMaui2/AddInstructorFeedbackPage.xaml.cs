using Microsoft.Maui.Controls;
using System;
using PuppyYogaMaui2.Models;
using PuppyYogaMaui2.Data;

namespace PuppyYogaMaui2
{
    public partial class AddInstructorFeedbackPage : ContentPage
    {
        private Instructor _instructor;
        private PuppyYogaDatabase _database;
        private InstructorFeedback _existingFeedback;

        public AddInstructorFeedbackPage(Instructor instructor, PuppyYogaDatabase database, InstructorFeedback existingFeedback = null)
        {
            InitializeComponent();
            _instructor = instructor;
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

            string comment = CommentEditor.Text;

            if (_existingFeedback != null)
            {
                _existingFeedback.Rating = rating;
                _existingFeedback.Comment = comment;
                _existingFeedback.Date = DateTime.UtcNow; 

                await _database.UpdateInstructorFeedbackAsync(_existingFeedback);
                await DisplayAlert("Success", "Your feedback has been updated.", "OK");
            }
            else
            {
                var newFeedback = new InstructorFeedback
                {
                    InstructorId = _instructor.InstructorId,
                    Rating = rating,
                    Comment = comment,
                    Date = DateTime.UtcNow
                };

                await _database.SaveInstructorFeedbackAsync(newFeedback);
                await DisplayAlert("Success", "Your feedback has been submitted.", "OK");
            }

            await Navigation.PopAsync();
        }
    }
}
