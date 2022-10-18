using BFuel.Domain.Models;
using BFuel.Modelos;
using BFuel.Servicos;
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

namespace BFuel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageCadastro : ContentPage
    {
        private RegisterService _service;
        public MainPageCadastro()
        {
            InitializeComponent();

            _service = new RegisterService();
        }

        private async void VoltarInicio(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void RealizarCadastro(object sender, EventArgs e)
        {
            string nome = entName.Text;
            string email = entEmail.Text;
            string senha = entSenha.Text;
            string confirmSenha = entConfirmSenha.Text;

            await Navigation.PushPopupAsync(new Load());

            if (senha != confirmSenha)
            {
                await Navigation.PopAllPopupAsync();

                await DisplayAlert("Erro!", "Senhas não coincidem", "OK");
            }
            else
                TaskRegister(nome, email, senha);
        }

        public async void TaskRegister(string nome,string email,string senha)
        {
            try
            {
                await _service.Register(nome, email, senha);

                string message = message = _service.Message;

                await DisplayAlert("Informação", message, "OK");
            }
            catch(Exception)
            {
                await DisplayAlert("Falha ao conectar com nossos serviços!", "Verifique se a conexão com o WiFi ou 3G está ativa e funcionando e tente novamente.", "OK");
            }
            finally
            {
                await Navigation.PopAllPopupAsync();
            }
        }
    }    
}