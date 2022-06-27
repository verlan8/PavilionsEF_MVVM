using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PavilionsEF.ViewModels;

namespace PavilionsEF.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для TenantsPage.xaml
    /// </summary>
    public partial class TenantsPage : Page
    {
        public TenantsPage()
        {
            InitializeComponent();
            ViewModelManager viewModelManager = ViewModelManager.GetInstance();
            Ten.DataContext = viewModelManager.AdminViewModel;
        }
    }
}
