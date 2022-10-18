using BFuel.Servicos;
using BFuel.Utility;
using BFuel.Domain.Models;
using BFuel.BFDomain.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Xamarin.Essentials;
using BFuel.Modelos;

namespace BFuel.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaPostosModal : ContentPage
    {
        NoSQLService _service;
        IMongoCollection<GasStation> collection;
        ObservableCollection<GasStation> cheapest_stations;

        public ListaPostosModal()
        {
            InitializeComponent();

            _service = new NoSQLService();
            cheapest_stations = new ObservableCollection<GasStation>();

            LoadAll();
        }

        private void BackMapsPage(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(); 
        }

        private async void LoadAll()
        {
            await Task.Delay(500);

            await GetGasStationsList();
        }
        private async void SearchNameOrFlag(object sender, EventArgs e)
        {
            cheapest_stations.Clear();

            await GetGasStationsList(((SearchBar)sender).Text);
        }
        private void NotFoundResponse()
        {
            DisplayAlert("Ops!", "Nenhum posto encontrado no momento. Tente novamente mais tarde", "OK");

            lblTextEmptyView.Text = "Nenhum posto encontrado";
        }

        #region SearchFlow
        private async Task GetGasStationsList(string search = "")
        {
            string _search = search.ToUpper().TrimStart().TrimEnd();

            await Navigation.PushPopupAsync(new Load());
            try
            {
                MongoClient dbclient = new MongoClient(_service.dbconnection);
                IMongoDatabase database = dbclient.GetDatabase("bfueldb");
                collection = database.GetCollection<GasStation>("precos_postos_combustivel");

                if(_search == string.Empty)
                {
                    GetGasStationsForFuel();
                }
                else
                {
                    switch (_search)
                    {
                        case "GASOLINA":
                            GetGasStationsForFuel();
                            break;

                        case "ETANOL":
                            GetGasStationsForFuel(_search);
                            break;

                        case "GASOLINA ADITIVADA":
                            GetGasStationsForFuel(_search);
                            break;

                        case "ADITIVADA":
                            GetGasStationsForFuel("GASOLINA " + _search);
                            break;

                        case "DIESEL":
                            GetGasStationsForFuel(_search);
                            break;

                        case "DIESEL S10":
                            GetGasStationsForFuel(_search);
                            break;

                        case "S10":
                            GetGasStationsForFuel("DIESEL " + _search);
                            break;

                        default:
                            GetGasStationsForFlag(_search);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                NotFoundResponse();
            }
            finally
            {
                await Navigation.PopAllPopupAsync();
            }
        }

        private void GetGasStationsForFlag(string flag)
        {
            string fuel = "GASOLINA";

            if (flag.Contains("ETANOL"))
            {
                fuel = "ETANOL";
            }
            else if (flag.Contains("ADITIVADA"))
            {
                fuel = "ADITIVADA";
            }
            else if (flag.Contains("DIESEL"))
            {
                fuel = "DIESEL";
            }

            IMongoQueryable<GasStation> queryableGS = collection.AsQueryable()
                .Where(a => a.Bandeira.Contains(flag) && a.Produto.Equals(fuel))
                .OrderBy(a => a.Valor)
                .Take(10);

            List<GasStation> items = queryableGS.ToList();

            if(items.Count == 0)
            {
                NotFoundResponse();
            }
            else
            {
                FillListofStations(items);
            }
        }

        private void GetGasStationsForFuel(string fuel = "GASOLINA")
        {
            IMongoQueryable<GasStation> queryableGS = collection.AsQueryable()
                .Where(a => a.Produto.Equals(fuel))
                .OrderBy(a => a.Valor)
                .Take(10);

            List<GasStation> items = queryableGS.ToList();

            FillListofStations(items);
        }

        private void FillListofStations(List<GasStation> items)
        {
            foreach (GasStation item in items) // convertendo de List pra ObservableCollection
            {
                cheapest_stations.Add(item);
            }

            items.Clear();

            ListOfGasStations.ItemsSource = cheapest_stations;
        }
        #endregion

        #region OpenMapFlow

        private async void HowToGo(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new Load());

            try
            {
                GasStation gs = (GasStation)ListOfGasStations.SelectedItem;

                string localstring = gs.Rua + ","
                        + gs.Numero.Replace("S/N", "") + ","
                        + gs.Bairro + ","
                        + gs.Complemento.Trim() + ","
                        + gs.Municipio + ","
                        + gs.Estado + ", Brasil";

                Location_Service _service = new Location_Service();
                Location result = new Location();

                ResponseService<Locat> responseService = await _service.GetLocationByName(localstring);

                if (!responseService.IsSucess)
                {
                    if (responseService.StatusCode == 404)
                    {
                        await DisplayAlert("Erro!", "Não houveram resultados para as buscas dos locais informados", "OK");
                        await Navigation.PopAllPopupAsync();
                    }
                    else
                    {
                        await DisplayAlert("Ops", "Ocorreu um erro inesperado! Tente novamente mais tarde.", "OK");
                        await Navigation.PopAllPopupAsync();
                    }

                }
                else
                {
                    result.Latitude = responseService.Data.results[0].position.lat;
                    result.Longitude = responseService.Data.results[0].position.lon;

                    await Navigation.PopAllPopupAsync();

                    await OpenMaps(result);
                }
            }
            catch (Exception ex)
            {
                await Navigation.PopAllPopupAsync();

                await DisplayAlert("Falha!", ex.Message, "OK");
            }
        }

        private async Task OpenMaps(Location coords)
        {
            MapLaunchOptions options = new MapLaunchOptions { Name = "Better Fuel", NavigationMode = NavigationMode.Default };

            try
            {
                await Map.OpenAsync(coords, options);
            }
            catch(Exception ex)
            {
                await DisplayAlert("Falha!", ex.Message, "OK");
            }
        }

        #endregion
    }
}