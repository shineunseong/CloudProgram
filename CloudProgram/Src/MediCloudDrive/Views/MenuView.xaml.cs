using System.Diagnostics;
using System.Windows.Controls;

using MediCloudDrive.Models.Interfaces;
using MediCloudDrive.ViewModels;

namespace MediCloudDrive.Views
{
    /// <summary>
    /// MenuView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MenuView : UserControl, IDetailView
    {
        public MenuViewModel ViewModel { get; set; }

        public MenuView()
        {
            InitializeComponent();
        }

        public void SetInitViewModel(IMainViewModel MainViewModel)
        {
            Debug.Assert(MainViewModel is MainViewModel);

            ViewModel = (MainViewModel as MainViewModel).MenuModel;
            this.DataContext = ViewModel;
        }
    }
}