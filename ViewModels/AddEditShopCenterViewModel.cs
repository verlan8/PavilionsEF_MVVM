using PavilionsEF.ViewModels.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public AddEditShopCenterViewModel()
        {

        }
    }
}
