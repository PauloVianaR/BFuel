using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.Maps;

namespace BFuel.CustomViews
{
    public class CustomMap : Map
    {
        public ObservableCollection<CustomPin> CustomPins { get; set; }
    }
}
