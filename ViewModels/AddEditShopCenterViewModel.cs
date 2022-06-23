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
    public class AddEditShopCenterViewModel: ViewModelBase
    {
        #region Labels
        private string _titleAddEditSPLabel = "Создание/редактирование ТЦ";
        public string titleAddEditSPLabel
        {
            get {  return _titleAddEditSPLabel;}
        }
        private string _contentDeleteButton = "Удалить";
        public string contentDeleteButton
        {
            get => _contentDeleteButton;
        }

        private string _contentPavilionList = "Список Павильонов";
        public string contentPavilionList
        {
            get => _contentPavilionList;
        }
        #endregion

        //использование ObservableCollection вместо List, т.к. имеется встроенные
        //методы сканирования изменений в листе
        private ObservableCollection<PavilionListModel> _PavilionList;

        public ObservableCollection<PavilionListModel> PavilionList
        {
            get { return _PavilionList; }
            set
            {
                Set(ref _PavilionList, value);
            }
        }

        #region commands

        #region Переход на Список Павильонов
        public ICommand PavilionListCommand { get; }

        private bool CanPavilionListCommandExecute(object parametr)
        {
            return true;
        }

        //переход на страницу СПИСОК ПАВИЛЬОНОВ
        private void OnPavilionListCommandExecuted(object parametr)
        {
            ViewModelManager.GetInstance().pageSelectViewModel.pageSelectViewModelState =
            PageSelectViewModel.PageSelectViewModelState.PavilionList;
        }
        #endregion

        #endregion

        public AddEditShopCenterViewModel()
        {
            PavilionListCommand = new RelayCommand(OnPavilionListCommandExecuted, CanPavilionListCommandExecute);
        }
    }
}
