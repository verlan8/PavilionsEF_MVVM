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

        private string _contentAddEditShopCenter = "Изменить";
        public string contentAddEditShopCenter
        {
            get => _contentAddEditShopCenter;
        }

        private string _contentAddShopCenter = "Добавить";
        public string contentAddShopCenter
        {
            get => _contentAddShopCenter;
        }

        private string _selectedSPButton = "Информация";

        public string SelectedSPButton
        {
            get { return _selectedSPButton; }
        }

        private string _pavilionListButton = "Список Павильонов";

        public string PavilionListButton
        {
            get { return _pavilionListButton; }
        }

        private string _backToShoppingCenter = "Назад";

        public string BackToShoppingCenter
        {
            get { return _backToShoppingCenter; }
            set { _backToShoppingCenter = value; }
        }


        #endregion

        private pavilionsDBEntities db = new pavilionsDBEntities();

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

        private shopping_center _editShoppingCenter;
        /// <summary>
        /// Выбранный ТЦ
        /// </summary>
        public shopping_center EditShoppingCenter
        {
            get => _editShoppingCenter;
            set
            {
                if (_editShoppingCenter != value)
                {
                    Set(ref _editShoppingCenter, value);
                }
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
        #endregion

        #region выбранный тц для изменения
        private shopping_center _changeShoppingCenter = new shopping_center();

        public shopping_center ChangeShoppingCenter
        {
            get { return _changeShoppingCenter; }
            set { Set (ref _changeShoppingCenter, value); }
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

        private string _selectedSPStatus;

        public string SelectedSPStatus
        {
            get { return _selectedSPStatus; }
            set { Set(ref _selectedSPStatus, value); }
        }


        private string _selectedStatus;
        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                if (_selectedStatus != value)
                {
                    //SelectedCity = null;
                    Set(ref _selectedStatus, value);
                    SelectValidation();
                    /*if (AllStatuses == value)
                    {
                        LoadData();

                    }
                    else
                    {
                        SelectValidation();
                        //ShowSPWithSelectedStatus(SelectedStatus);
                    }*/

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
                Set(ref _selectedCity, value);
                SelectValidation();
                //ShowSPWIthSelectedCity(SelectedCity);
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
            /*if (_selectedShoppingCenterModel != null)
            {
                ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.PavilionList;
            }*/
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
            if (_selectedShoppingCenterModel != null)
            {
                try
                {
                    //var db = new pavilionsDBEntities();
                    shopping_center shoppingCenter =
                        db.shopping_center.Where(s => s.id_shopping_center == SelectedShoppingCenter.id_shopping_center).FirstOrDefault();
                    shoppingCenter.id_status = db.statuses.Where(s => s.status_name == "Удален").Select(s => s.id_status).FirstOrDefault();
                    db.SaveChanges();
                    ShoppingCenters.Remove(SelectedShoppingCenter);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }  
        }
        #endregion

        #region Добавить ТЦ
        public ICommand AddCommand { get; }

        private bool CanAddCommandExecute(object parametr)
        {
            return true;
        }

        private void OnAddCommandExecuted(object parametr)
        {
            //проверка параметров
            if (string.IsNullOrWhiteSpace(SelectedSPStatus) || SelectedSPStatus == null
                || string.IsNullOrWhiteSpace(EditShoppingCenter.city))
            {
                MessageBox.Show("Неправильно введены строковые параметры");
            }
            else if (EditShoppingCenter.pavilions_quantity < 0 || EditShoppingCenter.cost < 0 
                || EditShoppingCenter.number_of_storeys < 0 || EditShoppingCenter.value_added_factor <= 0)
            {
                MessageBox.Show("Неправильно введены числовые параметры");
            }
            else
            {
                try
                {
                    //добавление
                    //var db = new pavilionsDBEntities();
                    EditShoppingCenter.id_status = db.shopping_center
                        .Where(s => s.status.status_name == SelectedSPStatus)
                        .Select(s => s.status.id_status).FirstOrDefault();
                    EditShoppingCenter.shopping_center_image = null;
                    EditShoppingCenter.id_shopping_center = db.shopping_center.OrderByDescending(s => s.id_shopping_center)
                        .Select(s => s.id_shopping_center).FirstOrDefault()+1;
                    db.shopping_center.Add(EditShoppingCenter);
                    db.SaveChanges();
                    LoadData();
                    UpdateStatuses();
                    ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState = 
                        PageSelectViewModel.PageSelectViewModelState.ShoppingCenter;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion

        #region Переход на добавление ТЦ
        public ICommand AddShopCenterCommand { get; }

        private bool CanAddShopCenterCommandExecute(object parametr)
        {
            return true;
        }

        //переход на страницу ИНТЕРФЕЙС ТЦ
        private void OnAddShopCenterCommandExecuted(object parametr)
        {
            //SelectedShoppingCenter = new ShoppingCenterModel();
            Statuses.Remove(AllStatuses);
            EditShoppingCenter = new shopping_center();
            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.AddSP;
        }
        #endregion

        #region Изменить ТЦ
        public ICommand ChangeCommand { get; }

        private bool CanChangeCommandExecute(object parametr)
        {
            return true;
        }

        private void OnChangeCommandExecuted(object parametr)
        {
            //изменение
            //проверка параметров
            if (string.IsNullOrWhiteSpace(SelectedSPStatus) || SelectedSPStatus == null
                || string.IsNullOrWhiteSpace(ChangeShoppingCenter.city))
            {
                MessageBox.Show("Неправильно введены строковые параметры");
            }
            else if (ChangeShoppingCenter.pavilions_quantity < 0 || ChangeShoppingCenter.cost < 0
                || ChangeShoppingCenter.number_of_storeys < 0 || ChangeShoppingCenter.value_added_factor <= 0)
            {
                MessageBox.Show("Неправильно введены числовые параметры");
            }
            else
            {
                try
                {
                    //изменение
                    //var db = new pavilionsDBEntities();
                    ChangeShoppingCenter.id_status = db.shopping_center
                        .Where(s => s.status.status_name == SelectedSPStatus)
                        .Select(s => s.status.id_status).FirstOrDefault();
                    ChangeShoppingCenter.shopping_center_image = null;
                    ChangeShoppingCenter.id_shopping_center = db.shopping_center.OrderByDescending(s => s.id_shopping_center)
                        .Select(s => s.id_shopping_center).FirstOrDefault();
                    db.SaveChanges();
                    LoadData();
                    UpdateStatuses();
                    ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                        PageSelectViewModel.PageSelectViewModelState.ShoppingCenter;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }
        #endregion

        #region Переход на изменение ТЦ
        public ICommand AddEditShopCenterCommand { get; }

        private bool CanAddEditShopCenterCommandExecute(object parametr)
        {
            return true;
        }

        //переход на страницу ИНТЕРФЕЙС ТЦ
        private void OnAddEditShopCenterCommandExecuted(object parametr)
        {
            
            if (_selectedShoppingCenterModel != null)
            {
                Statuses.Remove(AllStatuses);
                //var db = new pavilionsDBEntities();
                ChangeShoppingCenter = db.shopping_center.Find(SelectedShoppingCenter.id_shopping_center);
                SelectedSPStatus = _selectedShoppingCenterModel.status_name;
                ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState = 
                    PageSelectViewModel.PageSelectViewModelState.EditSP;
            }
           
        }
        #endregion

        #region Переход на информацию о выбранном тц

        public ICommand SelectedSPCommand { get; }

        private bool CanSelectedSPCommandExecute(object parametr)
        {
            return true;
        }

        private void OnSelectedSPCommandExecute(object parametr)
        {
            if (_selectedShoppingCenterModel != null)
            {
                ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.SelectedSP;
            }

        }
        #endregion

        #region Переход на Список ТЦ

        public ICommand SPCommand { get; }

        private bool CanSPCommandExecute(object parametr)
        {
            return true;
        }

        private void OnSPCommandExecute(object parametr)
        {
            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.ShoppingCenter;
        }
        #endregion

        #endregion

        public ShoppingCentersViewModel()
        {
            LoadData();
            UpdateStatuses();
            DeleteCommand = new RelayCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
            AddEditShopCenterCommand = new RelayCommand(OnAddEditShopCenterCommandExecuted, CanAddEditShopCenterCommandExecute);
            PavilionListCommand = new RelayCommand(OnPavilionListCommandExecuteed, CanPavilionListCommandExecute);
            SPCommand = new RelayCommand(OnSPCommandExecute, CanSPCommandExecute);
            SelectedSPCommand = new RelayCommand(OnSelectedSPCommandExecute, CanSelectedSPCommandExecute);
            ChangeCommand = new RelayCommand(OnChangeCommandExecuted, CanChangeCommandExecute);
            AddCommand = new RelayCommand(OnAddCommandExecuted, CanAddCommandExecute);
            AddShopCenterCommand = new RelayCommand(OnAddShopCenterCommandExecuted, CanAddShopCenterCommandExecute);
        }

        private void SelectValidation()
        {
            //var db = new pavilionsDBEntities();
            ShoppingCenters.Clear();
            if ((_selectedStatus == AllStatuses || _selectedStatus == null)
                 && (_selectedCity == AllStatuses || _selectedCity == null))
            {
                LoadData();
            }
            else
            {
                if ((_selectedCity == AllStatuses || _selectedCity == null)
                     && _selectedStatus != AllStatuses)
                {
                    ShowSPWithSelectedStatus(_selectedStatus);
                }
                else
                {
                    if ((_selectedStatus == AllStatuses || _selectedStatus == null)
                    && _selectedCity != AllStatuses)
                    {
                        ShowSPWIthSelectedCity(_selectedCity);
                    }
                    else
                    {
                        ShowSPWIthSelectedCityAndStatus(_selectedCity, _selectedStatus);
                    }
                }
            }
        }



        private void ShowSPWIthSelectedCityAndStatus(string selectedCity, string selectedStatus)
        {
            //var db = new pavilionsDBEntities();
            ShoppingCenters.Clear();
            ShoppingCenters = new ObservableCollection<ShoppingCenterModel>(
                db.shopping_center.Where(s => s.city == selectedCity && s.status.status_name == selectedStatus)
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

        private void ShowSPWIthSelectedCity(string selectedCity)
        {
            //var db = new pavilionsDBEntities();
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
            ObservableCollection<string> ListOfCiies = new ObservableCollection<string>(db.shopping_center.Where(s => s.status.status_name != "Удален").Select(s => s.city).Distinct());
            ListOfCiies.Add("Все");
            return ListOfCiies;
        }

        private void ShowSPWithSelectedStatus(string status)
        {
            //var db = new pavilionsDBEntities();
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

        private void UpdateStatuses()
        {
            Statuses = GetStatuses();
            Cities = GetCities();
        }

        private void LoadData()
        {
            //var db = new pavilionsDBEntities();
            ShoppingCenters = new ObservableCollection<ShoppingCenterModel>(
                db.shopping_center.Where(s => s.id_status != 3)
                .Join(db.statuses,
                    shop => shop.id_status,
                    status => status.id_status,
                    (shop, status) => new { shop, status })
                .Select(s => new ShoppingCenterModel
                {
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
