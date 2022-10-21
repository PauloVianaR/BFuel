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

namespace BFuel.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostosRotasMaps : ContentPage
    {
        NoSQLService _service;
        IMongoCollection<GasStation> collection;

        public PostosRotasMaps()
        {
            InitializeComponent();

            GetLocation();
            LoadPins();

            pckFuels.SelectedIndex = 0;
        }

        private void SelectFuel(object sender, EventArgs e)
        {
            string pickeritem = ((Picker)sender).SelectedItem.ToString();

            switch (pickeritem)
            {
                case "Gasolina Comum":
                    FrameHeader.BackgroundColor = Color.FromHex("#429E42");
                    break;

                case "Gasolina Aditivada":
                    FrameHeader.BackgroundColor = Color.FromHex("#FF5BC65B");
                    break;

                case "Etanol":
                    FrameHeader.BackgroundColor = Color.FromHex("#B1A452");
                    break;

                case "Diesel":
                    FrameHeader.BackgroundColor = Color.FromHex("#FF7126");
                    break;

                case "Diesel S10":
                    FrameHeader.Background = Color.FromHex("#FFFF9761");
                    break;

                default:
                    FrameHeader.BackgroundColor = Color.FromHex("#429E42");
                    break;
            }
        }

        private void OpenListGasStations(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ListaPostosModal());
        }


        private async void GetLocation()
        {
            await Task.Delay(500);

            await Navigation.PushPopupAsync(new Load());

            try
            {
                Location location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Xamarin.Forms.Maps.Position position = new Xamarin.Forms.Maps.Position(location.Latitude, location.Longitude);
                    MapSpan mapSpanPosition = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1.800));
                    mapaCentral.MoveToRegion(mapSpanPosition);
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }

            await Navigation.PopAllPopupAsync();
        }

        private async void LoadPins(string fuel = "GASOLINA")
        {
            await Navigation.PushPopupAsync(new Load());

            try
            {
                MongoClient dbclient = new MongoClient(_service.dbconnection);
                IMongoDatabase database = dbclient.GetDatabase("bfueldb");
                collection = database.GetCollection<GasStation>("precos_postos_combustivel");
                IMongoQueryable<GasStation> queryableGS = collection.AsQueryable()
                    .Where(a => a.Municipio.Equals("BETIM") && a.Produto.Equals(fuel))
                    .OrderBy(a => a.Valor);

                List<GasStation> items = queryableGS.ToList();
                List<CustomPin> pinsList = new List<CustomPin>();

                foreach (GasStation gs in items)
                {
                    CustomPin pin = new CustomPin
                    {
                        Label = fuel + ": R$" + gs.Valor + "9",
                        Address = gs.Revenda,
                        Type = PinType.Place,
                        Position = new Xamarin.Forms.Maps.Position(gs.Latitude, gs.Longitude),
                        Name = gs.Bandeira,
                        Url = "http://xamarin.com/about/"
                    };

                    pinsList.Add(pin);
                }

                mapaCentral.CustomPins = pinsList;

                foreach (CustomPin pin in pinsList)
                {
                    mapaCentral.Pins.Add(pin);
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }

            await Navigation.PopAllPopupAsync();

        }
    }
}