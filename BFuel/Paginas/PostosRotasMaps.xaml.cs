using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Extensions;
using BFuel.Utility;
using System.Reflection;
using BFuel.CustomViews;
using MongoDB.Driver;
using BFuel.BFDomain.Models;
using BFuel.Servicos;
using MongoDB.Driver.Linq;
using BFuel.Modelos;
using BFuel.Domain.Models;
using System.Collections.ObjectModel;

namespace BFuel.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostosRotasMaps : ContentPage
    {
        NoSQLService _service = new NoSQLService();
        IMongoCollection<GasStation> collection;
        Location currentLocation;
        bool allowPickerChanges = false;
        String currentCity;

        public PostosRotasMaps()
        {
            InitializeComponent();

            pckFuels.SelectedIndex = 0;

            LoadAll();
        }

        private async void LoadAll()
        {
            await LoadMap();
            await GetCurrentCity();
            await Task.Delay(1500);
            await LoadPins();
            allowPickerChanges = true;
        }

        private async void SelectFuel(object sender, EventArgs e)
        {
            string pickeritem = ((Picker)sender).SelectedItem.ToString();

            if (allowPickerChanges)
            {
                switch (pickeritem)
                {
                    case "Gasolina Comum":
                        FrameHeader.BackgroundColor = Color.FromHex("#429E42");
                        await LoadPins();
                        break;

                    case "Gasolina Aditivada":
                        FrameHeader.BackgroundColor = Color.FromHex("#FF5BC65B");
                        await LoadPins("GASOLINA ADITIVADA");
                        break;

                    case "Etanol":
                        FrameHeader.BackgroundColor = Color.FromHex("#B1A452");
                        await LoadPins("ETANOL");
                        break;

                    case "Diesel":
                        FrameHeader.BackgroundColor = Color.FromHex("#FF7126");
                        await LoadPins("DIESEL");
                        break;

                    case "Diesel S10":
                        FrameHeader.Background = Color.FromHex("#FFFF9761");
                        await LoadPins("DIESEL S10");
                        break;

                    default:
                        FrameHeader.BackgroundColor = Color.FromHex("#429E42");
                        break;
                }
            }            
        }

        private void OpenListGasStations(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ListaPostosModal());
        }

        private async Task GetCurrentCity()
        {
            currentCity = "BETIM";
            // TO DO
            /*
            Location_Service _service = new Location_Service();

            Location pos = await Geolocation.GetLastKnownLocationAsync();

            await Navigation.PushPopupAsync(new Load());

            try
            {
                ResponseService<GoogleLocat> responseService = await _service.GetLocationByCoords(pos.Latitude,pos.Longitude);

                if (!responseService.IsSucess)
                {
                    if (responseService.StatusCode == 404)
                        await DisplayAlert("Erro!", "Não houveram resultados para as buscas dos locais informados", "OK");
                    else
                        await DisplayAlert("Ops", "Ocorreu um erro inesperado! Tente novamente mais tarde.", "OK");
                }
                else
                {
                    currentCity = responseService.Data.results[0].adress_components[1].short_name.ToUpper();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }

            await Navigation.PopAllPopupAsync();
            */
        }

        private async Task LoadMap()
        {
            await Navigation.PushPopupAsync(new Load());

            try
            {
                currentLocation = await Geolocation.GetLastKnownLocationAsync();

                if (currentLocation != null)
                {
                    Xamarin.Forms.Maps.Position position = new Xamarin.Forms.Maps.Position(currentLocation.Latitude, currentLocation.Longitude);
                    MapSpan mapSpanPosition = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1.600));
                    mapaCentral.MoveToRegion(mapSpanPosition);
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }

            try
            {
                await Navigation.PopAllPopupAsync();
            }
            catch (Exception) { }
        }

        private async Task LoadPins(string fuel = "GASOLINA")
        {
            await Navigation.PushPopupAsync(new Load());

            if (mapaCentral.Pins.Count > 0)
            {
                mapaCentral.Pins.Clear();
                mapaCentral.CustomPins.Clear();
            }

            try
            {
                MongoClient dbclient = new MongoClient(_service.dbconnection);
                IMongoDatabase database = dbclient.GetDatabase("bfueldb");
                collection = database.GetCollection<GasStation>("precos_postos_combustivel");
                IMongoQueryable<GasStation> queryableGS = collection.AsQueryable()
                    .Where(a => a.Municipio.Equals(currentCity.ToUpper()) && a.Produto.Equals(fuel))
                    .OrderBy(a => a.Valor);

                List<GasStation> items = queryableGS.ToList();
                ObservableCollection<CustomPin> pinsList = new ObservableCollection<CustomPin>();

                if(items.Count < 1)
                {
                    throw new Exception("Nenhum posto localizado para esse tipo de combustível!");
                }

                await Task.Delay(1000);

                foreach (GasStation gs in items)
                {
                    CustomPin pin = new CustomPin
                    {
                        Label = fuel + ": R$" + gs.Valor + "9",
                        Address = gs.Revenda,
                        Type = PinType.Place,
                        Position = new Xamarin.Forms.Maps.Position(gs.Latitude, gs.Longitude),
                        Name = gs.Bandeira,
                        Url = "https://www.google.com.br/maps/dir/" 
                            + currentLocation.Latitude.ToString() 
                            + "," + currentLocation.Longitude.ToString() 
                            + "/" + gs.Latitude.ToString() 
                            + "," + gs.Longitude.ToString()
                    };
                    pinsList.Add(pin);
                }
                mapaCentral.CustomPins = pinsList;

                foreach (CustomPin pin in pinsList)
                {
                    mapaCentral.Pins.Add(pin);
                }

                await Task.Delay(2000);
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("posto"))
                    await DisplayAlert("Ops", ex.Message, "OK");
                else
                    await DisplayAlert("Ops", "Nenhum posto encontrado no momento... \nTente novamente mais tarde!", "OK");

                if(mapaCentral.Pins.Count > 0)
                {
                    mapaCentral.Pins.Clear();
                    mapaCentral.CustomPins.Clear();
                }
            }

            try
            {
                await Navigation.PopAllPopupAsync();
            }
            catch (Exception) { }
        }
    }
}