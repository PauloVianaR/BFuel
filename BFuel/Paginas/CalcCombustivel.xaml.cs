using BFuel.Domain.Models;
using BFuel.Modelos;
using BFuel.Servicos;
using BFuel.Utility;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BFuel.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalcCombustivel : ContentPage
    {
        private const bool ValidationLocal = true;
        private decimal kmL = 10;

        public CalcCombustivel()
        {
            InitializeComponent();
        }

        private void ChangeLocalEntriesVisibility(object sender, ToggledEventArgs e)
        {
            if (switchOpcao.IsToggled)
            {
                stcLocalsEntries.IsVisible = true;
            }
            else
            {
                stckResults.IsVisible = false;
                entLocalDestino.Text = string.Empty;
                entLocalSaida.Text = string.Empty;
                entKmL.Text = string.Empty;
                stcLocalsEntries.IsVisible = false;
            }
        }

        private void UpdaterStringLocal(object sender, TextChangedEventArgs e)
        {
            ((Entry)sender).Text = e.NewTextValue.ToString().ToLower().TrimStart();

            if(e.NewTextValue.ToString().Contains("brasil"))
            {
                ((Entry)sender).Text = e.NewTextValue.ToString().Replace("brasil", "").TrimEnd();
            }
            else if (e.NewTextValue.ToString().Contains("brazil"))
            {
                ((Entry)sender).Text = e.NewTextValue.ToString().Replace("brazil", "").TrimEnd();
            }
        }

        private void DefinerKML(object sender, TextChangedEventArgs e)
        {
            kmL = e.NewTextValue.ToString() == string.Empty ? 10 : Convert.ToDecimal(e.NewTextValue.ToString());
        }

        private void BtnCalcularClicked(object sender, EventArgs e)
        {
            if(switchOpcao.IsToggled)
            {
                ComplexFlow();
            }
            else
            {
                SimpleFlow();
            }
        }

        #region SimpleFlow
        private void SimpleFlow()
        {
            try
            {
                decimal autonomia = Convert.ToDecimal(entAutonomia.Text);
                decimal precogas = Convert.ToDecimal(entPrecoGasolina.Text);
                decimal precoalcool = Convert.ToDecimal(entPrecoEtanol.Text);

                if (autonomia > 0 && precogas > 0 && precoalcool > 0)
                    CalculateCompensation(autonomia, precogas, precoalcool);
                else
                    ErrorMessage();
            }
            catch (Exception)
            {
                ErrorMessage();
            }
        }

        private void CalculateCompensation(decimal autonomia, decimal precogas, decimal precoalcool)
        {
            bool compensaAlcool = precogas * (autonomia / 100) <= precoalcool;

            if (compensaAlcool)
            {
                lblTxtMelhorOpcao.IsVisible = true;
                frameOpcao.IsVisible = true;
                lblCombustivelSelect.IsVisible = true;
                lblCombustivelSelect.Text = "Gasolina";
                frameOpcao.BackgroundColor = Color.FromHex("#54BF54");
            }
            else
            {
                lblTxtMelhorOpcao.IsVisible = true;
                frameOpcao.IsVisible = true;
                lblCombustivelSelect.IsVisible = true;
                lblCombustivelSelect.Text = "Etanol";
                frameOpcao.BackgroundColor = Color.FromHex("#B1A452");
            }
        }

        private void ErrorMessage(bool validationLocal = false)
        {
            string message = validationLocal ? "Verifique se a conexão com o WiFi ou 3G estão ativos e funcionando e tente novamente." : "Necessário preencher todos os campos obrigatórios para calcular";
            string title = validationLocal ? "Falha ao conectar com nossos serviços!" : "Falha!";

            DisplayAlert(title, message, "OK");

            lblCombustivelSelect.IsVisible = false;
            lblTxtMelhorOpcao.IsVisible = false;
            frameOpcao.IsVisible = false;
            stckResults.IsVisible = false;
        }

        #endregion;

        #region ComplexFlow
        private async void ComplexFlow()
        {
            await Navigation.PushPopupAsync(new Load());

            try
            {
                decimal autonomia = Convert.ToDecimal(entAutonomia.Text);
                decimal precogas = Convert.ToDecimal(entPrecoGasolina.Text);
                decimal precoalcool = Convert.ToDecimal(entPrecoEtanol.Text);

                if (autonomia > 0 && precogas > 0 && precoalcool > 0)
                {
                    CalculateCompensation(autonomia, precogas, precoalcool);
                    await CalculateInfoLocation(precogas, precoalcool);
                }
                else
                    ErrorMessage();
            }
            catch (Exception)
            {
                ErrorMessage();
            }
            finally
            {
                await Navigation.PopAllPopupAsync();
            }
        }
        

        private async Task CalculateInfoLocation(decimal precogas, decimal precoalcool)
        {
            decimal distancia = await GetDistanceBetween();
            decimal totalLitros;

            if(distancia != 0)
            {
                stckResults.IsVisible = true;

                totalLitros = distancia / kmL;

                spanGas.Text = "R$ " + Math.Round(totalLitros * precogas, 2) + " (" + Math.Round(totalLitros).ToString() + " L)";
                spanEtan.Text = "R$ " + Math.Round(totalLitros * precoalcool, 2) + " (" + Math.Round(totalLitros).ToString() + " L)";
            }
        }

        private async Task<decimal> GetDistanceBetween()
        {
            decimal distancia = 0;

            try
            {
                Location destino = new Location();
                Location saida = new Location();

                saida = await LocationRequest(entLocalSaida.Text);
                destino = await LocationRequest(entLocalDestino.Text);

                double distanciaDouble = CalculateDistance(saida, destino);

                distancia = Convert.ToDecimal(distanciaDouble);
            }
            catch(Exception)
            {
                ErrorMessage(ValidationLocal);
            }

            return distancia;
        }

        private async Task<Location> LocationRequest(string local)
        {
            Location_Service _service = new Location_Service();

            string localcompleto = local + ", Brazil";

            Location result = new Location();

            try
            {
                ResponseService<Locat> responseService = await _service.GetLocationByName(localcompleto);

                if (!responseService.IsSucess)
                {
                    if (responseService.StatusCode == 404)
                        await DisplayAlert("Erro!", "Não houveram resultados para as buscas dos locais informados", "OK");
                    else
                        await DisplayAlert("Ops", "Ocorreu um erro inesperado! Tente novamente mais tarde.", "OK");
                }
                else
                {
                    result.Latitude = responseService.Data.results[0].position.lat;
                    result.Longitude = responseService.Data.results[0].position.lon;
                }
            }
            catch (Exception)
            {
                ErrorMessage(ValidationLocal);
            }

            return result;
        }

        private double CalculateDistance(Location placeA, Location placeB)
        {
            return Location.CalculateDistance(placeA, placeB, DistanceUnits.Kilometers);
        }

        #endregion;
    }
}