using System.Windows;
using Xamarin.Forms;
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

            Xamarin.Forms.Forms.Init();

            LoadApplication(new IMSCovidTracker.App());
        }
    }
}
