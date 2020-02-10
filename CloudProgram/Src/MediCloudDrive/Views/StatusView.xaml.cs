using System.Diagnostics;
using System.Windows.Controls;

using MediCloudDrive.Models.Interfaces;
using MediCloudDrive.ViewModels;

namespace MediCloudDrive.Views
{
    /// <summary>
    /// StatusView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StatusView : UserControl, IDetailView
    {
        public StatusViewModel ViewModel { get; set; }

        public StatusView()
        {
            InitializeComponent();
        }

        public void SetInitViewModel(IMainViewModel MainViewModel)
        {
            Debug.Assert(MainViewModel is MainViewModel);

            ViewModel = (MainViewModel as MainViewModel).StatusModel;
            this.DataContext = ViewModel;
        }
    }
}