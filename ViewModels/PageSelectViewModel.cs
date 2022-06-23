using System;
using System.Collections.Generic;

namespace PavilionsEF.ViewModels
{
    internal class PageSelectViewModel
    {
        public enum PageSelectViewModelState
        {
            Authorization,Admin,PavilionInterface, PavilionList, ShoppingCenter, AddEditSP
        };



        public delegate void ListenerType(PageSelectViewModelState pageSelectViewModelState);

        public event ListenerType Listeners;

        //        private List<ListenerType> listListeners = new List<System.Func<Void,PageSelectViewModelState>>();


        //      public void RegisterListener(System.Func<Void,PageSelectViewModelState> listener)
        //      {
        //          listListeners.Add(listener);
        //      }


        private PageSelectViewModelState pageSelectViewModelStateField;
        public PageSelectViewModelState pageSelectViewModelState { get => pageSelectViewModelStateField; set { pageSelectViewModelStateField = value;
                Listeners.Invoke(value); } }
        

        public void AfterLoad()
        {
            pageSelectViewModelState = PageSelectViewModel.PageSelectViewModelState.Authorization;
        }
            
    }
}