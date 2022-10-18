using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BFuel
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (App.Current.Properties.ContainsKey("User"))
            {
                MainPage = new Paginas.Inicial();
            }
            else
            {
                MainPage = new MainPage();
            }
        }

        public static void GoToMain(string msg = "")
        {
            App.Current.MainPage = new MainPage(msg);
        }

        protected override void OnStart() { }
        protected override void OnSleep() { }
        protected override void OnResume() { }
        
    }
}
