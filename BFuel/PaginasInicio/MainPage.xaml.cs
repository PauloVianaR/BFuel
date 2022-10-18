using BFuel.Domain.Models;
using BFuel.Modelos;
using BFuel.Servicos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using BFuel.Utility;

namespace BFuel
{
    public partial class MainPage : ContentPage
    {
        private UserService _userservice;
        private RegisterService _registerservice;
        private readonly IGoogleManager _googleManager;
        public MainPage(string message = "")
        {
            InitializeComponent();

            _userservice = new UserService();
            _googleManager = DependencyService.Get<IGoogleManager>();
            _registerservice = new RegisterService();

            if (message != string.Empty)
                DisplayAlert("Ops!", "Falha ao tentar realizar o login. Confirme seus dados e tente novamente!", "OK");
        }

        private async Task LoginMaster(string email, string senha)
        {
            await Navigation.PushPopupAsync(new Load());

            try
            {
                ResponseService<User> responseService = await _userservice.GetUser(email, senha);

                if (!responseService.IsSucess)
                {
                    if (responseService.StatusCode == 404)
                        await DisplayAlert("Erro!", "Nenhum usuário encontrado", "OK");
                    else
                        await DisplayAlert("Ops", "Ocorreu um erro inesperado! Tente novamente mais tarde.", "OK");
                }
                else
                {
                    App.Current.Properties.Add("User", JsonConvert.SerializeObject(responseService.Data));
                    await App.Current.SavePropertiesAsync();
                    App.Current.MainPage = new Paginas.Inicial();
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Falha ao conectar com nossos serviços!", "Verifique se a conexão com o WiFi ou 3G está ativa e funcionando e tente novamente.", "OK");
            }
            finally
            {
                await Navigation.PopAllPopupAsync();
            }
        }

        private async void LoginDefault(object sender, EventArgs e)
        {
            string email = entEmail.Text;
            string senha = entSenha.Text;

            await LoginMaster(email, senha);
        }

        private async void SignIn(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPageCadastro());
        }

        private void LoginGoogle(object sender, EventArgs e)
        {
            _googleManager.Login(OnLoginComplete);
        }

        private async void OnLoginComplete(GoogleUser googleUser, string message)
        {
            string email = googleUser.Email;
            string senha = googleUser.Password;
            string nome = googleUser.Name;

            if(googleUser != null)
            {
                ResponseService<User> responseService = await _userservice.GetUser(email, senha);

                if (!responseService.IsSucess)
                {
                    if(responseService.StatusCode == 404)
                    {
                        await Navigation.PushPopupAsync(new Load());
                        await _registerservice.Register(nome, email, senha);
                        string messagex = _registerservice.Message;
                        await Navigation.PopAllPopupAsync();

                        await DisplayAlert("Informação", messagex, "OK");
                    }
                    else
                        await DisplayAlert("Ops", "Ocorreu um erro inesperado! Tente novamente mais tarde.", "OK");                   
                }
                else
                {
                    await LoginMaster(email, senha);
                }

            }
            else
            {
                await DisplayAlert("Ops!", message, "OK");

                App.GoToMain("Erro");
            }
        }

        private void LoginFacebook(object sender, EventArgs e)
        {
            App.Current.MainPage = new PaginasInicio.LoginFacebookPage();
        }
    }
}
