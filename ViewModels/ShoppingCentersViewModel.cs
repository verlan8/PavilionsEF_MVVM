using PavilionsEF.Commands;
using PavilionsEF.Models;
using PavilionsEF.ViewModels.BaseClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PavilionsEF.ViewModels
{
    public class ShoppingCentersViewModel : ViewModelBase
    {
        #region ButtonContent

        private string _contentDeleteButton = "Удалить";
        public string contentDeleteButton
        {
            get => _contentDeleteButton;
        }

        private string _contentAddEditShopCenter = "Интерфейс ТЦ";
        public string contentAddEditShopCenter
        {
            get => _contentAddEditShopCenter;
        }

        private string _pavilionListButton = "Список Павильонов";

        public string PavilionListButton
        {
            get { return _pavilionListButton; }
        }


        #endregion

        //использование ObservableCollection вместо List, т.к. имеется встроенные
        //методы сканирования изменений в листе
        private ObservableCollection<ShoppingCenterModel> _ShoppingCenters;

        public ObservableCollection<ShoppingCenterModel> ShoppingCenters
        {
            get { return _ShoppingCenters; }
            set 
            { 
                Set(ref _ShoppingCenters, value); 
            }
        }

        #region выбранный ТЦ
        private ShoppingCenterModel _selectedShoppingCenterModel;
        /// <summary>
        /// Выбранный ТЦ
        /// </summary>
        public ShoppingCenterModel SelectedShoppingCenter
        {
            get => _selectedShoppingCenterModel;
            set
            {
                if (_selectedShoppingCenterModel != value)
                {
                    Set(ref _selectedShoppingCenterModel, value);
                }
            }
        }
        
        private bool _isSelected = false;
        /// <summary>
        /// ТЦ выбран
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }
        #endregion

        #region статусы

        private string AllStatuses = "Все";
        private ObservableCollection<string> _Statuses = GetStatuses();

        public ObservableCollection<string> Statuses
        {
            get { return _Statuses; }
            set { Set(ref _Statuses, value); }
        }

        private string _selectedStatus;
        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set 
            {
                if (_selectedStatus != value)
                {
                    SelectedCity = null;
                    Set(ref _selectedStatus, value);
                    if (AllStatuses == value)
                    {
                        LoadData();
                    }
                    else
                    {
                        ShowSPWithSelectedStatus(SelectedStatus);
                    }
                    
                } 
            }
        }

        #endregion

        #region города
        private ObservableCollection<string> _cities = GetCities();

        public ObservableCollection<string> Cities
        {
            get { return _cities; }
            set { Set(ref _cities, value); }
        }

        private string _selectedCity;

        public string SelectedCity
        {
            get { return _selectedCity; }
            set 
            { 
                Set (ref _selectedCity, value);
                ShowSPWIthSelectedCity(SelectedCity);
            }
        }

        #endregion


        #region commands

        #region  Переход на СПИСОК ПАВИЛЬОНОВ

        public ICommand PavilionListCommand { get; }

        private bool CanPavilionListCommandExecute(object parametr)
        {
            return true;
        }

        private void OnPavilionListCommandExecuteed(object parametr)
        {
            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState = 
                PageSelectViewModel.PageSelectViewModelState.PavilionList;
        }

        #endregion

        #region Удалить ТЦ
        public ICommand DeleteCommand { get; }

        private bool CanDeleteCommandExecute(object parametr)
        {
            return true;
        }
        private void OnDeleteCommandExecuted(object parametr)
        {
            try
            {
                var db = new pavilionsDBEntities();
                shopping_center shoppingCenter = (
                    db.shopping_center.Where(s => s.id_shopping_center == SelectedShoppingCenter.id_shopping_center).FirstOrDefault());
                shoppingCenter.id_status = (db.statuses.Where(s => s.status_name == "Удален").Select(s=>s.id_status).FirstOrDefault());
                db.SaveChanges();
                ShoppingCenters.Remove(SelectedShoppingCenter);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Переход на ИНТЕРФЕЙС ТЦ
        public ICommand AddEditShopCenterCommand { get; }

        private bool CanAddEditShopCenterCommandExecute(object parametr)
        {
            return true;
        }

        //переход на страницу ИНТЕРФЕЙС ТЦ
        private void OnAddEditShopCenterCommandExecuted(object parametr)
        {
            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.AddEditSP;
        }
        #endregion

        #endregion

        

        public ShoppingCentersViewModel()
        {
            LoadData();
            DeleteCommand = new RelayCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
            AddEditShopCenterCommand = new RelayCommand(OnAddEditShopCenterCommandExecuted, CanAddEditShopCenterCommandExecute);
        }

        private void ShowSPWIthSelectedCity(string selectedCity)
        {
            var db = new pavilionsDBEntities();
            ShoppingCenters.Clear();
            ShoppingCenters = new ObservableCollection<ShoppingCenterModel>(
                db.shopping_center.Where(s => s.city == selectedCity)
                .Select(s => new ShoppingCenterModel
                {
                    id_shopping_center = s.id_shopping_center,
                    shopping_center_name = s.shopping_center_name,
                    status_name = s.status.status_name,
                    status_id = s.status.id_status,
                    pavilions_quantity = s.pavilions_quantity,
                    city = s.city,
                    cost = s.cost,
                    number_of_storeys = s.number_of_storeys,
                    value_added_factor = s.value_added_factor,
                    shopping_center_image = s.shopping_center_image
                }).OrderBy(s => new { s.city, s.status_name }));
        }

        private static ObservableCollection<string> GetCities()
        {
            var db = new pavilionsDBEntities();
            return new ObservableCollection<string>(db.shopping_center.Select(s => s.city).Distinct());
        }

        private void ShowSPWithSelectedStatus(string status)
        {
            var db = new pavilionsDBEntities();
            ShoppingCenters.Clear();
            ShoppingCenters = new ObservableCollection<ShoppingCenterModel>(
                db.shopping_center.Where(s => s.status.status_name == status)
                .Select(s => new ShoppingCenterModel
                {
                    id_shopping_center = s.id_shopping_center,
                    shopping_center_name = s.shopping_center_name,
                    status_name = s.status.status_name,
                    status_id = s.status.id_status,
                    pavilions_quantity = s.pavilions_quantity,
                    city = s.city,
                    cost = s.cost,
                    number_of_storeys = s.number_of_storeys,
                    value_added_factor = s.value_added_factor,
                    shopping_center_image = s.shopping_center_image
                }).OrderBy(s => new { s.city, s.status_name }));
        }

        private static ObservableCollection<string> GetStatuses()
        {
            var db = new pavilionsDBEntities();
            ObservableCollection<string> ListOfStatuses = new ObservableCollection<string>(db.statuses.Select(s => s.status_name));
            ListOfStatuses.Add("Все");
            return ListOfStatuses;
        }
        private void LoadData()
        {
            var db = new pavilionsDBEntities();
            ShoppingCenters = new ObservableCollection<ShoppingCenterModel>(
                db.shopping_center.Where(s => s.id_status != 3)
                .Join(db.statuses,
                    shop => shop.id_status,
                    status => status.id_status,
                    (shop, status) => new { shop, status })
                .Select(s => new ShoppingCenterModel {
                    id_shopping_center = s.shop.id_shopping_center,
                    shopping_center_name = s.shop.shopping_center_name,
                    status_name = s.status.status_name,
                    status_id = s.status.id_status,
                    pavilions_quantity = s.shop.pavilions_quantity,
                    city = s.shop.city,
                    cost = s.shop.cost,
                    number_of_storeys = s.shop.number_of_storeys,
                    value_added_factor = s.shop.value_added_factor,
                    shopping_center_image = s.shop.shopping_center_image
                }).OrderBy(s => new { s.city, s.status_name }));

        }
    }
}
