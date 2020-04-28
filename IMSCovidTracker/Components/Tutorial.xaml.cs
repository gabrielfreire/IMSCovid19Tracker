using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMSCovidTracker.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tutorial : Frame
    {
        public Tutorial(Rectangle position)
        {
            InitializeComponent();

            AbsoluteLayout.SetLayoutBounds(countryWidgetInfo, position);
            AbsoluteLayout.SetLayoutFlags(countryWidgetInfo, AbsoluteLayoutFlags.All);
        }
    }
}