using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PavilionsEF.ViewModels.BaseClass;
using PavilionsEF.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PavilionsEF.Commands;
using System.Windows;
using System.Data;
using System.Data.SqlClient;

namespace PavilionsEF.ViewModels
{
    public class AdminViewModel: ViewModelBase
    {
        #region ButtonContent

        private string _contentDeleteButton = "Удалить";
        public string contentDeleteButton
        {
            get => _contentDeleteButton;
        }

        private string _contentEditShopCenter = "Изменить";
        public string contentEditShopCenter
        {
            get => _contentEditShopCenter;
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

        private string _contentTenantsButton = "Арендаторы";

        public string contentTenantsButton
        {
            get { return _contentTenantsButton; }
        }

        private string _contentRentalListButton = "Список аренд";

        public string contentRentalListButton
        {
            get { return _contentRentalListButton; }
        }


        private string _backToTenants = "Назад";

        public string BackToTenants
        {
            get { return _backToTenants; }
            set { _backToTenants = value; }
        }
        #endregion

        private pavilionsDBEntities db = new pavilionsDBEntities();

        #region поиск по фамилии

        private string _surname;
        public string Surname
        {
            get { return _surname; }
            set
            {
                Set(ref _surname, value);
                GetEmployees(value);
            }
        }
        #endregion

        #region Максимальная  по длине фамилия

        private int _maxSurname = GetMaxSurnameLength();

        public int MaxSurname
        {
            get { return _maxSurname; }
            set { Set(ref _maxSurname, value); }
        }
        #endregion

        #region Список сотрудников

        private ObservableCollection<EmployeeModel> _employeesList;

        public ObservableCollection<EmployeeModel> EmployeesList
        {
            get { return _employeesList; }
            set { Set(ref _employeesList, value); }
        }

        #endregion

        #region Список аренд
        private ObservableCollection<RentalListModel> _rentalList;

        public ObservableCollection<RentalListModel> RentalList
        {
            get { return _rentalList; }
            set { Set(ref _rentalList, value); }
        }

        #endregion

        #region Выбранный сотрудник
        private EmployeeModel _selectedEmployee;

        public EmployeeModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { Set(ref _selectedEmployee, value); }
        }
        #endregion

        #region выбранный сотрудник для изменения
        private employee _changeEmployee = new employee();

        public employee ChangeEmployee
        {
            get { return _changeEmployee; }
            set { Set(ref _changeEmployee, value); }
        }

        #endregion

        #region сотрудник для добавления
        private employee _addEmployee;

        public employee AddEmployee
        {
            get { return _addEmployee; }
            set { Set(ref _addEmployee, value); }
        }

        #endregion

        #region выбранный статус сотрудника

        private string _selectedEmployeeRole;

        public string SelectedEmployeeRole
        {
            get { return _selectedEmployeeRole; }
            set { Set(ref _selectedEmployeeRole, value); }
        }

        #endregion

        #region Роли

        private ObservableCollection<string> _roles = GetRoles();

        public ObservableCollection<string> Roles
        {
            get { return _roles; }
            set { Set (ref _roles, value); }
        }

        #endregion

        #region commands

        #region Удалить сотрудника
        public ICommand DeleteCommand { get; }

        private bool CanDeleteCommandExecute(object parametr)
        {
            return true;
        }

        //переход на страницу ИНТЕРФЕЙС ТЦ
        private void OnDeleteCommandExecuted(object parametr)
        {
            if (_selectedEmployee != null)
            {
                try
                {
                    //var db = new pavilionsDBEntities();
                    employee emp =
                        db.employees.Where(s => s.id_employee == SelectedEmployee.id_employee).FirstOrDefault();
                    emp.id_role = db.roles.Where(s => s.role_name == "Удален").Select(s => s.id_role).FirstOrDefault();
                    db.SaveChanges();
                    LoadData();
                    //EmployeesList.Remove(SelectedEmployee);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        #endregion

        #region Переход на изменение сотрудника
        public ICommand EditEmployeeCommand { get; }

        private bool CanEditEmployeeCommandExecute(object parametr)
        {
            return true;
        }

        //переход на страницу ИНТЕРФЕЙС ТЦ
        private void OnEditEmployeeCommandExecuted(object parametr)
        {

            if (_selectedEmployee != null)
            {
                ChangeEmployee = db.employees.Find(SelectedEmployee.id_employee);
                ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                    PageSelectViewModel.PageSelectViewModelState.EditEmp;
            }

        }
        #endregion

        #region Изменить сотрудника
        #endregion

        #region Переход на добавление сотрудника
        public ICommand AddEmployeeCommand { get; }

        private bool CanAddEmployeeCommandExecute(object parametr)
        {
            return true;
        }

        //переход на страницу ИНТЕРФЕЙС ТЦ
        private void OnAddEmployeeCommandExecuted(object parametr)
        {
            AddEmployee = new employee();
            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.EditEmp;

        }
        #endregion

        #region Добавить сотрудника
        public ICommand AddCommand { get; }

        private bool CanAddCommandExecute(object parametr)
        {
            return true;
        }

        //переход на страницу ИНТЕРФЕЙС ТЦ
        private void OnAddCommandExecuted(object parametr)
        {
            

        }
        #endregion

        #region Переход на интерфейс админа
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

        #region Переход на информацию о выбранном сотруднике

        public ICommand SelectedEmpCommand { get; }

        private bool CanSelectedEmpCommandExecute(object parametr)
        {
            return true;
        }

        private void OnSelectedEmpCommandExecuted(object parametr)
        {
            if (_selectedEmployee != null)
            {
                ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.SelectedEmp;
            }

        }
        #endregion

        #region Переход на список арендаторов
        public ICommand TenantsCommand { get; }

        private bool CanTenantsCommandExecute(object parametr)
        {
            return true;
        }

        private void OnTenantsCommandExecuted(object parametr)
        {
            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.Tenants;

        }
        #endregion

        #endregion

        /// <summary>
        /// АРЕНДАТОРЫ
        /// </summary>

        #region Список арендаторов

        private ObservableCollection<TenantModel> _tenantsList;

        public ObservableCollection<TenantModel> TenantsList
        {
            get { return _tenantsList; }
            set { Set(ref _tenantsList, value); }
        }

        #endregion

        #region Выбранный арендатор

        private TenantModel _selectedTenant;

        public TenantModel SelectedTenant
        {
            get { return _selectedTenant; }
            set { Set(ref _selectedTenant, value); }
        }

        #endregion

        #region commands Rent

        #region Переход на список аренд выбранного арендатора
        public ICommand RentalListCommand { get; }

        private bool CanRentalListCommandExecute(object parametr)
        {
            return true;
        }

        private void OnRentalListCommandExecute(object parametr)
        {
            if (_selectedTenant != null)
            {
                GetRentalListData();
                ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                PageSelectViewModel.PageSelectViewModelState.RentalList;
            }

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
                PageSelectViewModel.PageSelectViewModelState.SelectedTen;
            }

        }



        #endregion



        #endregion

        public AdminViewModel()
        {
            LoadData();
            //админ
            DeleteCommand = new RelayCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
            AdminInterfaceCommand = new RelayCommand(OnAdminInterfaceCommandExecuted, CanAdminInterfaceCommandExecute);
            EditEmployeeCommand = new RelayCommand(OnEditEmployeeCommandExecuted, CanEditEmployeeCommandExecute);
            AddEmployeeCommand = new RelayCommand(OnAddEmployeeCommandExecuted, CanAddEmployeeCommandExecute);
            AddCommand = new RelayCommand(OnAddCommandExecuted, CanAddCommandExecute);
            SelectedEmpCommand = new RelayCommand(OnSelectedEmpCommandExecuted, CanSelectedEmpCommandExecute);
            TenantsCommand = new RelayCommand(OnTenantsCommandExecuted, CanTenantsCommandExecute);

            //список аренд
            SelectedTenantCommand = new RelayCommand(OnSelectedTenantCommandExecute, CanSelectedTenantCommandExecute);
            RentalListCommand = new RelayCommand(OnRentalListCommandExecute, CanRentalListCommandExecute);
        }

        private static ObservableCollection<string> GetRoles()
        {
            var db = new pavilionsDBEntities();
            ObservableCollection<string> ListOfRoles = new ObservableCollection<string>(db.roles.Select(s => s.role_name));
            return ListOfRoles;
        }

        private static int GetMaxSurnameLength()
        {
            var db = new pavilionsDBEntities();
            string [] surnames = db.employees.Select(s => s.employee_surname).ToArray();
            int max = surnames.OrderByDescending(s => s.Length).Select(s => s.Length).FirstOrDefault();
            return max;
        }

        private void GetEmployees(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                LoadData();
            }
            else
            {
                EmployeesList = new ObservableCollection<EmployeeModel>(
                    db.employees.Where(s => s.employee_surname.Contains(inputString))
                    .Select(s => new EmployeeModel
                    {
                        id_employee = s.id_employee,
                        employee_surname = s.employee_surname,
                        employee_name = s.employee_name,
                        employee_middlename = s.employee_middlename,
                        employee_login = s.employee_login,
                        employee_password = s.employee_password,
                        id_role = s.id_role,
                        role_name = s.role.role_name,
                        telephone = s.telephone,
                        sex = s.sex,
                        employee_image = s.employee_image
                    }));
            }
        }

        ObservableCollection<RentalListModel> rentals = new ObservableCollection<RentalListModel>(); 

        private void GetRentalListData()
        {
            //SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-TJLQPJN\SQLEXPRESS;Initial Catalog = music_lover; Integrated Security = True");

            //вызвать хранимую процедуру через ADO
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-TJLQPJN\SQLEXPRESS;Initial Catalog = pavilionsDb; Integrated Security = True"))
            {
                conn.Open();
                string ProcName = "TenantsListOfLeases";
                DataTable dtRentalList = new DataTable();
                SqlCommand sqlCommand = new SqlCommand(ProcName, conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@id_tenant", SqlDbType.Int).Value = SelectedTenant.id_tenant;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    RentalListModel rentalListModel = new RentalListModel();
                    rentalListModel.shopping_center_name = reader[0].ToString();
                    rentalListModel.city = reader[1].ToString();
                    rentalListModel.id_pavilion = reader[2].ToString();
                    rentalListModel.start_of_lease = Convert.ToDateTime(reader[3]);
                    rentalListModel.end_of_lease = Convert.ToDateTime(reader[4]);
                    rentalListModel.rental_cost = Convert.ToDecimal(reader[5]);
                    rentalListModel.rent_status = reader[6].ToString();
                    rentals.Add(rentalListModel);
                }
                reader.Close();
                RentalList = rentals;

            }
        }

        private void LoadData()
        {
            EmployeesList = new ObservableCollection<EmployeeModel>(
                db.employees
                .Select(s => new EmployeeModel
                {
                    id_employee = s.id_employee,
                    employee_surname = s.employee_surname,
                    employee_name = s.employee_name,
                    employee_middlename = s.employee_middlename,
                    employee_login = s.employee_login,
                    employee_password = s.employee_password,
                    id_role = s.id_role,
                    role_name = s.role.role_name,
                    telephone = s.telephone,
                    sex = s.sex,
                    employee_image = s.employee_image
                }));

            TenantsList = new ObservableCollection<TenantModel>(
                db.tenants
                .Select(s => new TenantModel
                {
                    id_tenant = s.id_tenant,
                    tenant_name = s.tenant_name,
                    tenant_address = s.tenant_address,
                    telephone = s.telephone,
                }));
        }

    }
}
