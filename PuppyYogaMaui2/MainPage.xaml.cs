using Microsoft.Maui.Controls;
using System;

namespace PuppyYogaMaui2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnBookClassClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///ClassesPage");
        }

        private async void OnAboutUsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AboutPage");
        }

        private async void OnGalleryClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("GalleryPage");
        }
    }
}
