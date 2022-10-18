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

namespace BFuel.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Abastecer : ContentPage
    {
        private SupplyService _service;
        public Abastecer()
        {
            InitializeComponent();

            _service = new SupplyService();
        }

        private double totPago = 0;
        private double valorLitro = 0;
        private string totalLitros = "";
        private string combustivel = "";
        private string localPosto = "";

        private void EntPrecoLitro_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcTotalLitros();
        }

        private void EntTotalPago_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcTotalLitros();
        }

        private void CalcTotalLitros()
        {
            try
            {
                totPago = Convert.ToDouble(entTotalPago.Text);
                valorLitro = Convert.ToDouble(entPrecoLitro.Text);

                if (totPago != 0 && valorLitro != 0)
                {
                    totalLitros = lblTotalLitros.Text = (Math.Round(totPago / valorLitro, 2)).ToString();
                    lblTotalLitros.Text = totalLitros + " L";
                }
            }
            catch (Exception)
            {
                lblTotalLitros.Text = "0";
                valorLitro = 0;
                totPago = 0;
            }
        }

        private void SelecionaCombustivel(object sender, EventArgs e)
        {
            combustivel = pckCombustivel.SelectedItem.ToString();
        }

        private void EntLocalPosto_TextChanged(object sender, TextChangedEventArgs e)
        {
            localPosto = entLocalPosto.Text;
        }

        private async void InserirAbastecimento(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new Load());

            User user = JsonConvert.DeserializeObject<User>(App.Current.Properties["User"].ToString());

            DateTime dataHora = DateTime.UtcNow.AddHours(-3);

            Supply sup = new Supply()
            {
                TotalPaid = totPago,
                TotalLiters = totalLitros != "" ? Convert.ToDouble(totalLitros) : 0,
                LiterPrice = valorLitro,
                LocalName = localPosto,
                FuelName = combustivel,
                InsertedDate = dataHora,
                UserId = user.Id
            };

            ResponseService<Supply> responseService = await _service.AddSupply(sup);

            if (!responseService.IsSucess)
            {
                if (responseService.StatusCode == 400)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var dicKey in responseService.Errors)
                    {
                        foreach (var message in dicKey.Value)
                        {
                            sb.AppendLine(message);
                        }
                    }

                    await DisplayAlert("Erro!", sb.ToString(), "OK"); ;
                }

                else
                    await DisplayAlert("Ops", "Ocorreu um erro inesperado! Tente novamente mais tarde", "OK");
            }
            else
            {
                await DisplayAlert("Concluído", "Abastecimento inserido com sucesso!", "OK");
                valorLitro = 0;
                totPago = 0;
                totalLitros = "";
                combustivel = "";
                localPosto = "";
                entLocalPosto.Text = "";
                entPrecoLitro.Text = "";
                entTotalPago.Text = "";
            }

            await Navigation.PopAllPopupAsync();
        }

    }
}