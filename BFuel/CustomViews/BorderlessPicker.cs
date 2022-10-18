using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BFuel.CustomViews
{
    public class BorderlessPicker : Picker
    {
        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(int), typeof(BorderlessPicker), 0);

        public int BorderThickness
        {
            get => (int)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }
    }
}
