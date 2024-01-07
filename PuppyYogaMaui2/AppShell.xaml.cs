using PuppyYogaMaui2;
namespace PuppyYogaMaui2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(GalleryPage), typeof(GalleryPage));
            Routing.RegisterRoute(nameof(ReservationsListPage), typeof(ReservationsListPage));

            Routing.RegisterRoute(nameof(AddEditClassPage), typeof(AddEditClassPage));
            Routing.RegisterRoute(nameof(AddEditInstructorPage), typeof(AddEditInstructorPage));

            Routing.RegisterRoute(nameof(AddClassFeedbackPage), typeof(AddClassFeedbackPage));

            Routing.RegisterRoute(nameof(AddInstructorFeedbackPage), typeof(AddInstructorFeedbackPage));
        }
    }
}
