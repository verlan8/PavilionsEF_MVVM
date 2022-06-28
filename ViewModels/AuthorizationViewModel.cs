using PavilionsEF.Commands;
using PavilionsEF.Models;
using PavilionsEF.ViewModels.BaseClass;
using PavilionsEF.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace PavilionsEF.ViewModels
{

    public class AuthorizationViewModel : ViewModelBase
    {
        #region Labels
        private string _titleLabel = "Окно авторизации";
        public string titleLabel
        {
            get => _titleLabel;
            set => Set(ref _titleLabel, value);
        }

        private string _loginLabel = "Логин";
        public string loginLabel
        {
            get => _loginLabel;
        }

        private string _passwordLabel = "Пароль";
        public string passwordLabel
        {
            get => _passwordLabel;
        }

        private string _contentButton = "Авторизоваться";
        public string contentButton
        {
            get => _contentButton;
        }
        #endregion

        //менеджер с
        private string _login = "Adam@gmai.com";
        //админ
        //private string _login = "Elizor@gmai.com";
        public string login
        {
            get { return _login; }
            set
            {
                Set(ref _login, value);
            }
        }
        

        #region Captcha

        private Visibility _ChaptchaVisibility = Visibility.Collapsed;
        public Visibility ChaptchaVisibility 
        { 
            get => _ChaptchaVisibility; 
            set => Set(ref _ChaptchaVisibility, value); 
        }

        private string _captchaText = "";

        public string captchaText
        {
            get => _captchaText; 
            set => Set(ref _captchaText, value); 
        }

        private string _enterCaptcha = "";

        public string enterCaptcha
        {
            get { return _enterCaptcha; }
            set { Set(ref _enterCaptcha, value); }
        }

        
        #endregion

        public ICommand AuthorizationCommand { get; }

        private bool CanAuthorizationCommandExecute(object parametr)
        {
            return true;
        }

        private int counterOfClicks = 0;
        private bool flagCaptcha = false;
        private void OnAuthorizationCommandExecuted(object parametr)
        {
            //переход на нужную view по роли сотрудника
            var passwordBox = parametr as PasswordBox;
            string Password = passwordBox.Password;
            if (!string.IsNullOrEmpty(Password))
            {
                //проверка логина/пароля 
                if (Check(Password, login) == true && enterCaptcha == captchaText)
                {
                    flagCaptcha = true;
                    //переход на нужную view по роли сотрудника
                    //как переходить
                    switch (id_role)
                    {
                        //админ
                        case 1:
                            //AdminPage adminPage = new AdminPage();
                            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState = 
                            PageSelectViewModel.PageSelectViewModelState.Admin;

                            break;
                        //менеджер а
                        case 2:

                            break;
                        //менеджер с
                        case 3:
                            //ShoppingCentersPage shoppingCentersPage = new ShoppingCentersPage();
                            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
                            PageSelectViewModel.PageSelectViewModelState.ShoppingCenter;
                            break;
                        //удален
                        case 4:
                            MessageBox.Show("Сотрудник удален");
                            break;
                    }
                }
                counterOfClicks++;
                if (counterOfClicks > 0 && flagCaptcha == false)
                {
                    Captcha();
                    flagCaptcha = true;
                }
                flagCaptcha = false;
            }
        }


        public AuthorizationViewModel()
        {
            AuthorizationCommand = new RelayCommand(OnAuthorizationCommandExecuted, CanAuthorizationCommandExecute);
        }

        private bool flag = false;
        private int id_role = 0;
        private bool Check(string password, string login)
        {
            if (!String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(login))
            {
                try
                {
                    var db = new pavilionsDBEntities();
                    var userLogin = db.employees.Where(s => s.employee_login == login).Select(s => s.employee_login).FirstOrDefault();
                    var userPassword = db.employees.Where(s => s.employee_password == password).Select(s => s.employee_password).FirstOrDefault();
                    if (userLogin != null && userPassword != null)
                    {
                        flag = true;
                        id_role = db.employees.Where(s => s.employee_login == login).Select(s => s.id_role).FirstOrDefault();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            return flag;
        }

        private void Captcha()
        {
            ChaptchaVisibility = Visibility.Visible;
            string symbols = " ";
            symbols = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z,1,2,3,4,5,6,7,8,9,0";
            string[] symbolsArray = symbols.Split(',');
            string captcha = "";
            string captchaSymbol = "";
            Random rnd = new Random();
            for (int i = 0; i < 6; i++)
            {
                captchaSymbol = symbolsArray[(rnd.Next(0, symbolsArray.Length))];
                captcha += captchaSymbol;
            }
            captchaText = captcha;
        }
    }
}
