using GalaSoft.MvvmLight;
using MediCloudDrive.Models;
using MediCloudDrive.Models.Interfaces;
using System.Diagnostics;

namespace MediCloudDrive.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MenuViewModel MenuModel { get; set; }

        public MainListViewModel ListViewModel { get; set; }

        public StatusViewModel StatusModel { get; set; }

        public MediCloudModel AppModel { get; private set; }

        public MainViewModel(MediCloudModel appModel)
        {
            Debug.Assert(appModel != null);
            this.AppModel = appModel;

            MenuModel = new MenuViewModel(this);
            ListViewModel = new MainListViewModel(this);
            StatusModel = new StatusViewModel(this);
        }
    }
}