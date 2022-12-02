using BFuel.BFDomain.Models;
using BFuel.Domain.Models;
using BFuel.Utility;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
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
        User user;
        public Configs()
        {
            InitializeComponent();

            user = JsonConvert.DeserializeObject<User>(App.Current.Properties["User"].ToString());

            pckGastosConfig.SelectedIndex = user.TotalExpenses_Config > 0 ? user.TotalExpenses_Config : 0;
            pckGastosLitros.SelectedIndex = user.TotalSupplied_Config > 0 ? user.TotalSupplied_Config : 0;
        }

        private void FazerLoggout(object sender, EventArgs e)
        {
            App.Current.Properties.Remove("User");
            App.Current.SavePropertiesAsync();
            App.Current.MainPage = new MainPage();
        }

        private async void AlterarSenha(object sender, EventArgs e)
        {

            if (user.UserType == (int)UserTypes.Types.Google || user.UserType == (int)UserTypes.Types.Facebook)
            {
                await DisplayAlert("Ops", "Se você fez o login com uma conta Google ou Facebook não será possível alterar a senha!", "OK");
            }
            else
                await DisplayAlert("Aviso", "Funcionalidade indisponível no momento(WIP)", "OK");
        }

        private async void ChangeConfigs(object sender, EventArgs e)
        {
            int index = ((Picker)sender).SelectedIndex;

            await Navigation.PushPopupAsync(new Load());

            App.Current.Properties.Remove("User");
            user.TotalSupplied_Config = index;
            user.TotalExpenses_Config = index;
            App.Current.Properties.Add("User", JsonConvert.SerializeObject(user));
            await App.Current.SavePropertiesAsync();

            if (pckGastosConfig.SelectedIndex != index)
                pckGastosConfig.SelectedIndex = index;
            else if (pckGastosLitros.SelectedIndex != index)
                pckGastosLitros.SelectedIndex = index;

            await Navigation.PopAllPopupAsync();
        }
    }
}