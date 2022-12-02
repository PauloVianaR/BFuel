using BFuel.Domain.Models;
using BFuel.Modelos;
using BFuel.Servicos;
using BFuel.Utility;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BFuel.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InicioHome : ContentPage
    {
        private SupplyService _service;
        private ObservableCollection<Supply> _listOfSupplies;
        private int _pageCount = 0;
        private int _listOfSuppliesFirstRequest;
        private int TotExpConfig;
        private int TotSuppConfig;
        public InicioHome()
        {
            InitializeComponent();

            _service = new SupplyService();

            LoadAll();
        }

        private async void LoadAll()
        {
            try
            {
                await LoadHistoric();
                LoadHeader();
            }
            catch (Exception)
            {
                await DisplayAlert("Ops", "Ocorreu um erro inesperado! Tente fazer o login novamente.", "OK");

                App.Current.Properties.Remove("User");
                await App.Current.SavePropertiesAsync();
                App.Current.MainPage = new MainPage();
            }
        }

        private async Task LoadHistoric()
        {
            User user = JsonConvert.DeserializeObject<User>(App.Current.Properties["User"].ToString());
            TotExpConfig = user.TotalExpenses_Config;
            TotSuppConfig = user.TotalSupplied_Config;

            ResponseService<List<Supply>> responseService = await _service.GetSupplies(user.Id);

            if (!responseService.IsSucess)
            {
                await DisplayAlert("Ops", "Ocorreu um erro inesperado! Tente novamente mais tarde", "OK");
            }
            else
            {
                DateTime dateCalc = GetDateTime(TotExpConfig);

                if(dateCalc < DateTime.UtcNow.AddHours(-3).AddMinutes(10))
                    _listOfSupplies = new ObservableCollection<Supply>(responseService.Data);
                else
                {
                    _listOfSupplies = new ObservableCollection<Supply>(responseService.Data.Where(sup =>
                        Convert.ToDateTime(sup.InsertedDate) <= dateCalc));
                }


                _listOfSuppliesFirstRequest = _listOfSupplies.Count();
                ListOfSupplies.ItemsSource = _listOfSupplies;
                _pageCount = 1;
                ListOfSupplies.RemainingItemsThreshold = 1;
            }

            actIndicator.IsRunning = false;
            actIndicator.IsVisible = false;

            if(_listOfSupplies.Count == 0)
            {
                lblInfoEmpView.Text = "Sem histórico ainda!";
            }
        }

        private void LoadHeader()
        {
            try
            {
                if (_listOfSupplies != null)
                {
                    double totPago = 0;
                    double totLitro = 0;

                    foreach (Supply supItem in _listOfSupplies)
                    {
                        totPago += supItem.TotalPaid;
                        totLitro += supItem.TotalLiters;
                    }

                    spanGastosTotais.Text = totPago.ToString();
                    spanTotAbastecido.Text = totLitro.ToString();
                }
            }
            catch(Exception ex)
            {
                DisplayAlert("Falha!", ex.Message, "OK");
            }
        }

        private async void InfinitySearch(object sender, EventArgs e)
        {
            ListOfSupplies.RemainingItemsThreshold = -1;
            _pageCount++;
            User user = JsonConvert.DeserializeObject<User>(App.Current.Properties["User"].ToString());

            ResponseService<List<Supply>> responseService = await _service.GetSupplies(user.Id, _pageCount);

            if (!responseService.IsSucess)
            {
                await DisplayAlert("Ops", "Ocorreu um erro inesperado! Tente novamente mais tarde", "OK");
                ListOfSupplies.RemainingItemsThreshold = 0;
            }
            else
            {
                var listOfSuppliesService = responseService.Data;
                foreach (var item in listOfSuppliesService)
                {
                    _listOfSupplies.Add(item);
                    LoadHeader();
                }
                if(_listOfSuppliesFirstRequest == listOfSuppliesService.Count)
                    ListOfSupplies.RemainingItemsThreshold = 1;
                else
                    ListOfSupplies.RemainingItemsThreshold = -1;
            }

        }

        private async void RefreshSearch(object sender, EventArgs e)
        {
            LoadAll();

            await Task.Delay(1000);

            ((RefreshView)sender).IsRefreshing = false;
        }

        private DateTime GetDateTime(int config)
        {
            DateTime dt_now = DateTime.UtcNow.AddHours(-3);
            DateTime newDate = dt_now;

            switch (config)
            {
                case 0:
                    newDate = dt_now;
                    break;

                case 1:
                    newDate = dt_now.AddMonths(-1);
                    break;

                case 2:
                    newDate = dt_now.AddDays(-7);
                    break;

                case 3:
                    newDate = dt_now.AddYears(-1);
                    break;

                default:
                    newDate = dt_now;
                    break;
            }

            return newDate;
        }
    }
}