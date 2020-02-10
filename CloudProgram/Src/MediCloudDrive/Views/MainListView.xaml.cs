using System.Diagnostics;
using System.Windows.Controls;

using MediCloudDrive.Models.Interfaces;
using MediCloudDrive.ViewModels;

namespace MediCloudDrive.Views
{
    /// <summary>
    /// MainListView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainListView : UserControl, IDetailView
    {
        public MainListViewModel ViewModel { get; set; }

        public MainListView()
        {
            InitializeComponent();
        }

        public void SetInitViewModel(IMainViewModel MainViewModel)
        {
            Debug.Assert(MainViewModel is MainViewModel);

            ViewModel = (MainViewModel as MainViewModel).ListViewModel;
            this.DataContext = ViewModel;
        }
    }
}