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

namespace BFuel.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostosRotasMaps : ContentPage
    {
        public PostosRotasMaps()
        {
            InitializeComponent();

            GetLocation();

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

                    LoadPins();
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
            Pin pinoteste = new Pin
            {
                Label = "Bom de boca",
                Address = "Restaurante na rua Amelia Augusta",
                Type = PinType.Place,
                Position = new Position(-19.95264826794483, -44.18626487278566)
            };
            Pin pinoteste2 = new Pin
            {
                Label = "Igreja pentecostal",
                Address = "Igreja na rua Amelia Augusta",
                Type = PinType.Place,
                Position = new Position(-19.953259907720945, -44.18657108418384)
            };

            mapaCentral.Pins.Add(pinoteste);
            mapaCentral.Pins.Add(pinoteste2);
        }
    }
}