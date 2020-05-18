using System.Windows;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Platform.WPF;

namespace IMSCovidTracker.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental", "SwipeView_Experimental");
            Xamarin.Forms.Forms.Init();

            LoadApplication(new IMSCovidTracker.App());
        }
    }
}
