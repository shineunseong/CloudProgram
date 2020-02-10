using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using MediCloudDrive.Models.Interfaces;
using MediCloudDrive.ViewModels;

namespace MediCloudDrive.Views
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : UserControl, IDetailView
    {
        private OptionView optionView = null;

        public MainViewModel ViewModel { get; set; }

        public MainView()
        {
            InitializeComponent();
        }

        public void SetInitViewModel(IMainViewModel MainViewModel)
        {
            Debug.Assert(MainViewModel is MainViewModel);

            ViewModel = MainViewModel as MainViewModel;
            this.DataContext = ViewModel;

            this.menuview.SetInitViewModel(MainViewModel);
            this.listview.SetInitViewModel(MainViewModel);
            this.statusview.SetInitViewModel(MainViewModel);

            if (true == string.IsNullOrEmpty(ViewModel.AppModel.S3Info.StorageName) ||
                true == string.IsNullOrEmpty(ViewModel.AppModel.AppSetting.CompanyID))
            {
                // 옵션화면으로 바로 전환해야함
                ShowOptionView();
            }
        }

        public void ShowOptionView()
        {
            optionGrid.Children.Clear();
            optionView = null;
            optionView = new OptionView();
            optionView.SetOptionModel(ViewModel);

            ScrollViewer scrView = new ScrollViewer()
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                Content = optionView
            };

            optionGrid.Children.Add(scrView);
            optionGrid.Visibility = System.Windows.Visibility.Visible;
        }

        public void CloseOptionView()
        {
            optionGrid.Visibility = System.Windows.Visibility.Hidden;
            optionGrid.Children.Clear();
            optionView = null;
        }

        private void UserControl_PreviewDrop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var item in files)
                {
                    Debug.Assert(File.Exists(item), "파일이 존재하지 않습니다");

                    ViewModel.ListViewModel.AddFile(item);
                }

                ViewModel.ListViewModel.SendProcess();
            }
        }
    }
}