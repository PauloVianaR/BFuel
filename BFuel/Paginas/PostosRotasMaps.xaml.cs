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

namespace BFuel.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostosRotasMaps : ContentPage
    {
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

            await Navigation.PushPopupAsync(new Load());

            try
            {
                Location location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Position position = new Position(location.Latitude, location.Longitude);
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

        private void LoadPins()
        {
            CustomPin pin = new CustomPin
            {
                Label = "Gasolina: R$4,799",
                Address = "Posto Ipiranga da Mata",
                Type = PinType.Place,
                Position = new Position(-19.95264826794483, -44.18626487278566),
                Name = "Xamarin",
                Url = "http://xamarin.com/about/"
            };
            CustomPin pinoteste2 = new CustomPin
            {
                Label = "Igreja pentecostal",
                Address = "Igreja na rua Amelia Augusta",
                Type = PinType.Place,
                Position = new Position(-19.953259907720945, -44.18657108418384),
                Name = "teste",
                Url = "http://xamarin.com/about/"
            };

            mapaCentral.CustomPins = new List<CustomPin> { pin };

            mapaCentral.Pins.Add(pin);
        }
    }
}