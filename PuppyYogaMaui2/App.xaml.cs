using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;
using System;

namespace PuppyYogaMaui2
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }

        public App(IServiceProvider services)
        {
            InitializeComponent();
            Services = services;
            MainPage = new AppShell();
        }
        public Shell Shell => MainPage as Shell;

    }
}
