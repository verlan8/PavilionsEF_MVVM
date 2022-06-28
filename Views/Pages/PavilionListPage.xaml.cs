using PavilionsEF.ViewModels;
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

namespace PavilionsEF.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для PavilionListPage.xaml
    /// </summary>
    public partial class PavilionListPage : Page
    {
        public PavilionListPage()
        {
            InitializeComponent();
            ViewModelManager viewModelManager = ViewModelManager.GetInstance();
            PavilionListPg.DataContext = viewModelManager.PavilionListViewModel;
            //PavilionListPg.DataContext = viewModelManager.ShoppingCentersViewModel;
        }
    }
}
