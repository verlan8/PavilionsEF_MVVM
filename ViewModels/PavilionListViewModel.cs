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
    internal class PavilionListViewModel: ViewModelBase
    {
        #region ButtonContent

        private string _contentDeleteButton = "Удалить";
        public string contentDeleteButton
        {
            get => _contentDeleteButton;
        }

        private string _contentPavilionInterfaceButton = "Интерфейс Павильона";
        public string contentPavilionInterfaceButton
        {
            get => _contentPavilionInterfaceButton;
        }

        #endregion

        private ObservableCollection<PavilionListModel> _PavilionList;

        public ObservableCollection<PavilionListModel> PavilionList
        {
            get { return _PavilionList; }
            set
            {
                Set(ref _PavilionList, value);
            }
        }

        /*
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

                #endregion*/


        #region выбранный ТЦ
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Переход на Список Павильонов
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

        #endregion

        public PavilionListViewModel()
        {
            LoadData();
            DeleteCommand = new RelayCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
            PavilionInterfaceCommand = new RelayCommand(OnPavilionInterfaceCommandExecuted, CanPavilionInterfaceCommandExecute);
        }

        private void LoadData()
        {
            var db = new pavilionsDBEntities();
            PavilionList = new ObservableCollection<PavilionListModel>(
                db.pavilions.Join(
                    db.shopping_center, 
                    pav => pav.id_shopping_center,
                    shop => shop.id_shopping_center,
                    (shop, pav) => new { shop, pav})
                .Select(s => new PavilionListModel
                {
                    id_pavilion = s.shop. id_pavilion,
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
