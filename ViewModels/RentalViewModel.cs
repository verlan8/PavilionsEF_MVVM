using PavilionsEF.Commands;
using PavilionsEF.Models;
using PavilionsEF.ViewModels.BaseClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PavilionsEF.ViewModels
{
    public class RentalViewModel: ViewModelBase
    {

        #region ButtonContent

        private string _selectedSPButton = "Информация";

        public string SelectedSPButton
        {
            get { return _selectedSPButton; }
        }

        private string _pavilionListButton = "Список аренд";

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

        #region Список арендаторов

        private ObservableCollection<tenant> _tenantsList;

        public ObservableCollection<tenant> TenantsList
        {
            get { return _tenantsList; }
            set { Set(ref _tenantsList, value); }
        }

        #endregion


        #region Выбранный арендатор

        private tenant _selectedTenant;

        public tenant SelectedTenant
        {
            get { return _selectedTenant; }
            set { Set(ref _selectedTenant, value); }
        }

        #endregion

        #region commands

        #region Переход на список аренд выбранного арендатора
        public ICommand AdminInterfaceCommand { get; }

        private bool CanAdminInterfaceCommandExecute(object parametr)
        {
            return true;
        }

        //переход на страницу ИНТЕРФЕЙС ТЦ
        private void OnAdminInterfaceCommandExecuted(object parametr)
        {
            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.Admin;

        }
        #endregion

        #region Переход на информацию о выбранном арендаторе

        public ICommand SelectedTenantCommand { get; }

        private bool CanSelectedTenantCommandExecute(object parametr)
        {
            return true;
        }

        private void OnSelectedTenantCommandExecute(object parametr)
        {
            if (_selectedTenant != null)
            {
                ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.SelectedSP;
            }

        }

        #endregion



        #endregion


        public RentalViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            TenantsList = new ObservableCollection<tenant>(
                db.tenants
                .Select(s => new tenant
                {
                    id_tenant = s.id_tenant,
                    tenant_name = s.tenant_name,
                    tenant_address = s.tenant_address,
                    telephone = s.telephone,
                }));
        }
    }
}
