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

        private void AlterarSenha(object sender, EventArgs e)
        {
            DisplayAlert("Falha (WIP)", "Funcionalidade não disponível no momento (Work in progress)", "OK");
        }
    }
}