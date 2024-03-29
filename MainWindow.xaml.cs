﻿using PavilionsEF.ViewModels;
using PavilionsEF.Views.Pages;
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

namespace PavilionsEF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// Прокофьев Иван ПКС-305
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModelManager viewModelManager = ViewModelManager.GetInstance();
        public MainWindow()
        {

            InitializeComponent();
            //MainFrame.DataContext = viewModelManager.pageSelectViewModel;
            viewModelManager.pageSelectViewModel.Listeners += (page) =>
            {
                Page pageTo = null;
                switch(page)
                {
                    case PageSelectViewModel.PageSelectViewModelState.Authorization:
                        pageTo = new AuthorizationPage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.Admin:
                        pageTo = new AdminPage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.ShoppingCenter:
                        pageTo = new ShoppingCentersPage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.AddSP:
                        pageTo = new AddShoppingCenterPage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.PavilionInterface:
                        pageTo = new PavilionInterfacePage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.PavilionList:
                        pageTo = new PavilionListPage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.SelectedSP:
                        pageTo = new SelectedSPPage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.EditSP:
                        pageTo = new EditShoppingCenterPage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.SelectedPav:
                        pageTo = new SelectedPavilionPage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.AddPav:
                        pageTo = new AddPavilionPage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.EditPav:
                        pageTo = new EditPavilionPage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.SelectedEmp:
                        pageTo = new SelectedEmployeePage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.AddEmp:
                        pageTo = new AddEmployeePage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.EditEmp:
                        pageTo = new EditEmployeePage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.Tenants:
                        pageTo = new TenantsPage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.SelectedTen:
                        pageTo = new SelectedTenantPage();
                        break;
                    case PageSelectViewModel.PageSelectViewModelState.RentalList:
                        pageTo = new RentalListPage();
                        break;
                }
                MainFrame.NavigationService.Navigate(pageTo);
            };
        }

        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            viewModelManager.pageSelectViewModel.AfterLoad();
        }
    }
}
