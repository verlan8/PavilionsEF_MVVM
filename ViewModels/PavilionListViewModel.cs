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
    internal class PavilionListViewModel : ViewModelBase
    {
        #region ButtonContent

        private string _contentDeleteButton = "Удалить";
        public string contentDeleteButton
        {
            get => _contentDeleteButton;
        }

        private string _contentEditPavilion = "Изменить";
        public string contentEditPavilion
        {
            get => _contentEditPavilion;
        }

        private string _contentAddPavilion = "Добавить";
        public string contentAddPavilion
        {
            get => _contentAddPavilion;
        }

        private string _contentPavilionInterfaceButton = "Интерфейс Павильона";
        public string contentPavilionInterfaceButton
        {
            get => _contentPavilionInterfaceButton;
        }

        private string _selectedPavilionButton = "Информация";

        public string SelectedPavilionButton
        {
            get { return _selectedPavilionButton; }
        }

        private string _backToPavilionList = "Назад";

        public string BackToPavilionList
        {
            get { return _backToPavilionList; }
            set { _backToPavilionList = value; }
        }

        #endregion

        #region Список ТЦ
        private ObservableCollection<ShoppingCenterModel> _ShoppingCenters;

        public ObservableCollection<ShoppingCenterModel> ShoppingCenters
        {
            get { return _ShoppingCenters; }
            set
            {
                Set(ref _ShoppingCenters, value);
            }
        }
        #endregion


        #region Список павильонов
        private ObservableCollection<PavilionListModel> _PavilionList;

        public ObservableCollection<PavilionListModel> PavilionList
        {
            get { return _PavilionList; }
            set
            {
                Set(ref _PavilionList, value);
            }
        }
        #endregion

        #region Названия тц
        private ObservableCollection<string> _ShoppingCentersNames = GetShoppingCentersNames();

        public ObservableCollection<string> ShoppingCentersNames
        {
            get { return _ShoppingCentersNames; }
            set { Set(ref _ShoppingCentersNames, value); }
        }
        #endregion

        private string _selectedPavilionStatus;

        public string SelectedPavilionStatus
        {
            get { return _selectedPavilionStatus; }
            set { Set(ref _selectedPavilionStatus, value); }
        }

        private string _selectedShoppingCenterName;

        public string SelectedShoppingCenterName
        {
            get { return _selectedShoppingCenterName; }
            set { Set(ref _selectedShoppingCenterName, value); }
        }

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
                    Set(ref _selectedStatus, value);
                    if (AllStatuses == value)
                    {
                        LoadData();
                        SelectedFloor = AllFloors;
                    }
                    else
                    {
                        SelectValidation();
                    }

                }
            }
        }

        #endregion

        #region этажи
        private string AllFloors = "Все";
        private ObservableCollection<string> _floors = GetFloors();

        public ObservableCollection<string> Floors
        {
            get { return _floors; }
            set { Set(ref _floors, value); }
        }

        private string _selectedFloor;

        public string SelectedFloor
        {
            get { return _selectedFloor; }
            set
            {
                Set(ref _selectedFloor, value);
                SelectValidation();
            }
        }

        #endregion


        #region павильон для изменения
        private pavilion _editPavilion;
        /// <summary>
        /// Выбранный ТЦ
        /// </summary>
        public pavilion EditPavilion
        {
            get => _editPavilion;
            set
            {
                Set(ref _editPavilion, value);
            }
        }
        #endregion


        #region выбранный павильон
        private PavilionListModel _selectedPavilion;
        /// <summary>
        /// Выбранный ТЦ
        /// </summary>
        public PavilionListModel SelectedPavilion
        {
            get => _selectedPavilion;
            set
            {
                Set(ref _selectedPavilion, value);
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


        #region commands

        #region Удалить Павилион
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
                pavilion pavilion = (
                    db.pavilions.Where(s => s.id_pavilion == SelectedPavilion.id_pavilion
                        && s.id_shopping_center == SelectedPavilion.id_shopping_center).FirstOrDefault());
                PavilionList.Remove(SelectedPavilion);
                //db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Добавить Павильон
        public ICommand AddCommand { get; }

        private bool CanAddCommandExecute(object parametr)
        {
            return true;
        }

        private void OnAddCommandExecuted(object parametr)
        {
            //проверка параметров
            if (string.IsNullOrWhiteSpace(EditPavilion.id_pavilion) || SelectedPavilionStatus == null
                || string.IsNullOrWhiteSpace(SelectedShoppingCenterName))
            {
                MessageBox.Show("Неправильно введены строковые параметры");
            }
            else if (EditPavilion.cost_per_square_meter < 0 || EditPavilion.pavilion_floor < 0
                || EditPavilion.pavilion_square < 0 || EditPavilion.value_added_factor <= 0)
            {
                MessageBox.Show("Неправильно введены числовые параметры");
            }
            else
            {
                try
                {
                    //добавление
                    var db = new pavilionsDBEntities();
                    EditPavilion.pavilion_status = db.pavilions
                        .Where(s => s.pavilionStatus.pavilionstatus_name == SelectedPavilionStatus)
                        .Select(s => s.pavilionStatus.id_status).FirstOrDefault();
                    EditPavilion.id_shopping_center = db.shopping_center.Where(s => s.shopping_center_name == SelectedShoppingCenterName)
                        .Select(s => s.id_shopping_center).FirstOrDefault();
                    db.pavilions.Add(EditPavilion);
                    db.SaveChanges();
                    LoadData();
                    ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                        PageSelectViewModel.PageSelectViewModelState.PavilionList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }
        #endregion

        #region Переход на Интерфейс Павильонов
        public ICommand PavilionInterfaceCommand { get; }

        private bool CanPavilionInterfaceCommandExecute(object parametr)
        {
            return true;
        }

        //переход на страницу ИНТЕРФЕЙС ПАВИЛЬОНА
        private void OnPavilionInterfaceCommandExecuted(object parametr)
        {
            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
            PageSelectViewModel.PageSelectViewModelState.PavilionInterface;
        }
        #endregion

        #region Переход на Список Павильонов

        public ICommand PavilionListCommand { get; }

        private bool CanPavilionListCommandExecute(object parametr)
        {
            return true;
        }

        private void OnPavilionListCommandExecute(object parametr)
        {
            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.PavilionList;
        }
        #endregion

        #region Переход на Добавление Павильон

        public ICommand AddPavilionListCommand { get; }

        private bool CanAddPavilionListCommandExecute(object parametr)
        {
            return true;
        }

        private void OnAddPavilionListCommandExecute(object parametr)
        {
            Statuses.Remove(AllStatuses);
            //SelectedPavilion = new PavilionListModel();
            EditPavilion = new pavilion();
            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.AddPav;
        }
        #endregion

        #region Переход на Изменение Павильонов

        public ICommand EditPavilionListCommand { get; }

        private bool CanEditPavilionListCommandExecute(object parametr)
        {
            return true;
        }

        private void OnEditPavilionListCommandExecute(object parametr)
        {
            Statuses.Remove(AllStatuses);
            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.EditPav;
        }
        #endregion

        #region Переход на информацию о выбранном павильоне

        public ICommand SelectedPavilionCommand { get; }

        private bool CanSelectedPavilionCommandExecute(object parametr)
        {
            return true;
        }

        private void OnSelectedPavilionCommandExecute(object parametr)
        {
            if (_selectedPavilion != null)
            {
                ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.SelectedPav;
            }

        }
        #endregion

        #endregion

        public PavilionListViewModel()
        {
            LoadData();
            UpdateStatuses();
            DeleteCommand = new RelayCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
            PavilionInterfaceCommand = new RelayCommand(OnPavilionInterfaceCommandExecuted, CanPavilionInterfaceCommandExecute);
            PavilionListCommand = new RelayCommand(OnPavilionListCommandExecute, CanPavilionListCommandExecute);
            AddPavilionListCommand = new RelayCommand(OnAddPavilionListCommandExecute, CanAddPavilionListCommandExecute);
            EditPavilionListCommand = new RelayCommand(OnEditPavilionListCommandExecute, CanEditPavilionListCommandExecute);
            SelectedPavilionCommand = new RelayCommand(OnSelectedPavilionCommandExecute, CanSelectedPavilionCommandExecute);
            AddCommand = new RelayCommand(OnAddCommandExecuted, CanAddCommandExecute);
        }


        // 0 0 0
        // 1 0 0 
        // 0 1 0
        // 0 0 1
        // 1 1 0
        // 0 1 1
        // 1 0 1
        // 1 1 1

        private void UpdateStatuses()
        {
            Statuses = GetStatuses();
        }

        private void ShowSPWIthSelectedFloorAndStatus(string selectedFloor, string status)
        {
            var db = new pavilionsDBEntities();
            PavilionList.Clear();
            int floor = Convert.ToInt32(selectedFloor);
            PavilionList = new ObservableCollection<PavilionListModel>(
                db.pavilions.Where(s => s.pavilionStatus.pavilionstatus_name == _selectedStatus && s.pavilion_floor == floor)
                .Select(s => new PavilionListModel
                {
                    id_pavilion = s.id_pavilion,
                    id_shopping_center = s.id_shopping_center,
                    shopping_center_name = s.shopping_center.shopping_center_name,
                    shopping_center_status_name = s.shopping_center.status.status_name,
                    floor = s.pavilion_floor,
                    pavilion_status = s.pavilion_status,
                    pavilion_status_name = s.pavilionStatus.pavilionstatus_name,
                    pavilion_square = s.pavilion_square,
                    cost_per_square_meter = s.cost_per_square_meter,
                    value_added_factor = s.value_added_factor
                }));
        }

        private void SelectValidation()
        {
            var db = new pavilionsDBEntities();
            PavilionList.Clear();
            if ((_selectedStatus == AllStatuses || _selectedStatus == null)
                 && (_selectedFloor == AllFloors || _selectedFloor == null))
            {
                LoadData();
            }
            else
            {
                if ((_selectedFloor == AllFloors || _selectedFloor == null)
                     && _selectedStatus != AllStatuses)
                {
                    ShowSPWithSelectedStatus(_selectedStatus);
                }
                else
                {
                    if ((_selectedStatus == AllStatuses || _selectedStatus == null)
                    && _selectedFloor != AllFloors)
                    {
                        ShowSPWIthSelectedFloor(_selectedFloor);
                    }
                    else
                    {
                        ShowSPWIthSelectedFloorAndStatus(_selectedFloor, _selectedStatus);
                    }
                }
            }
        }


        private void ShowSPWIthSelectedFloor(string selectedFloor)
        {
            var db = new pavilionsDBEntities();
            PavilionList.Clear();
            int floor = Convert.ToInt32(selectedFloor);
            PavilionList = new ObservableCollection<PavilionListModel>(
                db.pavilions.Where(s => s.pavilion_floor == floor)
                .Select(s => new PavilionListModel
                {
                    id_pavilion = s.id_pavilion,
                    id_shopping_center = s.id_shopping_center,
                    shopping_center_name = s.shopping_center.shopping_center_name,
                    shopping_center_status_name = s.shopping_center.status.status_name,
                    floor = s.pavilion_floor,
                    pavilion_status = s.pavilion_status,
                    pavilion_status_name = s.pavilionStatus.pavilionstatus_name,
                    pavilion_square = s.pavilion_square,
                    cost_per_square_meter = s.cost_per_square_meter,
                    value_added_factor = s.value_added_factor
                }));
        }

        private static ObservableCollection<string> GetFloors()
        {
            var db = new pavilionsDBEntities();
            ObservableCollection<string> ListOfFloors = new ObservableCollection<string>(db.pavilions.Select(s => s.pavilion_floor.ToString()).Distinct());
            ListOfFloors.Add("Все");
            return ListOfFloors;
        }

        private void ShowSPWithSelectedStatus(string status)
        {
            var db = new pavilionsDBEntities();
            PavilionList.Clear();
            PavilionList = new ObservableCollection<PavilionListModel>(
                db.pavilions.Where(s => s.pavilionStatus.pavilionstatus_name == status)
                .Select(s => new PavilionListModel
                {
                    id_pavilion = s.id_pavilion,
                    id_shopping_center = s.id_shopping_center,
                    shopping_center_name = s.shopping_center.shopping_center_name,
                    shopping_center_status_name = s.shopping_center.status.status_name,
                    floor = s.pavilion_floor,
                    pavilion_status = s.pavilion_status,
                    pavilion_status_name = s.pavilionStatus.pavilionstatus_name,
                    pavilion_square = s.pavilion_square,
                    cost_per_square_meter = s.cost_per_square_meter,
                    value_added_factor = s.value_added_factor
                }));
        }

        private static ObservableCollection<string> GetStatuses()
        {
            var db = new pavilionsDBEntities();
            ObservableCollection<string> ListOfStatuses = new ObservableCollection<string>(db.pavilionStatuses.Select(s => s.pavilionstatus_name));
            ListOfStatuses.Add("Все");
            return ListOfStatuses;
        }

        private static ObservableCollection<string> GetShoppingCentersNames()
        {
            var db = new pavilionsDBEntities();
            ObservableCollection<string> ListOfShoppingCentersNames = 
                new ObservableCollection<string>(db.shopping_center.Where(s => s.status.status_name != "Удален")
                .Select(s => s.shopping_center_name).Distinct());
            return ListOfShoppingCentersNames;
        }

        private void LoadData()
        {
            var db = new pavilionsDBEntities();
            PavilionList = new ObservableCollection<PavilionListModel>(
                db.pavilions.Join(
                    db.shopping_center,
                    pav => pav.id_shopping_center,
                    shop => shop.id_shopping_center,
                    (shop, pav) => new { shop, pav })
                .Select(s => new PavilionListModel
                {
                    id_pavilion = s.shop.id_pavilion,
                    id_shopping_center = s.shop.id_shopping_center,
                    shopping_center_name = s.pav.shopping_center_name,
                    shopping_center_status_name = s.pav.status.status_name,
                    floor = s.shop.pavilion_floor,
                    pavilion_status = s.shop.pavilion_status,
                    pavilion_status_name = s.shop.pavilionStatus.pavilionstatus_name,
                    pavilion_square = s.shop.pavilion_square,
                    cost_per_square_meter = s.shop.cost_per_square_meter,
                    value_added_factor = s.shop.value_added_factor
                }));
        }
    }
}
