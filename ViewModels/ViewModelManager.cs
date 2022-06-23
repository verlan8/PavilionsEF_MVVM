namespace PavilionsEF.ViewModels
{
    internal class ViewModelManager
    {
        public MainViewModel MainViewModel { get; } = new MainViewModel();

        public ShoppingCentersViewModel ShoppingCentersViewModel {  get; } = new ShoppingCentersViewModel();

        public AddEditShopCenterViewModel AddEditShopCenterViewModel { get; } = new AddEditShopCenterViewModel();
        public PageSelectViewModel pageSelectViewModel { get; } = new PageSelectViewModel();

        static private ViewModelManager viewModelManager = null;
        static public ViewModelManager GetInstance()
        {
            if (viewModelManager == null) viewModelManager = new ViewModelManager();
            return viewModelManager;
        }

    }
}