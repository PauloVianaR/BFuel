using BFuel.BFDomain.Models;
using BFuel.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BFuel.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Configs : ContentPage
    {
        public Configs()
        {
            InitializeComponent();

            pckGastosConfig.SelectedIndex = 0;
            pckGastosLitros.SelectedIndex = 0;
        }

        private void FazerLoggout(object sender, EventArgs e)
        {
            App.Current.Properties.Remove("User");
            App.Current.SavePropertiesAsync();

            var pginicial = new MainPage();

            App.Current.MainPage = pginicial;
        }

        private async void AlterarSenha(object sender, EventArgs e)
        {
            User user = JsonConvert.DeserializeObject<User>(App.Current.Properties["User"].ToString());

            if (user.UserType == (int)UserTypes.Types.Google || user.UserType == (int)UserTypes.Types.Facebook)
            {
                await DisplayAlert("Ops", "Se você fez o login com uma conta Google ou Facebook não será possível altera a senha!", "OK");
            }
            else
                await DisplayAlert("Teste", user.UserType.ToString(), "OK");
        }
    }
}