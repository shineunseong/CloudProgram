using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using log4net;
using MahApps.Metro.Controls;

using MediCloudDrive.Models;
using MediCloudDrive.ViewModels;

namespace MediCloudDrive
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public NotifyIcon notify;

        public MainViewModel ViewModel { get; set; }

        private ILog log = LogManager.GetLogger("RollingFile");

        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetInitView(MediCloudModel appModel)
        {
            // 인자로 app.cs에서 모델을 받아와서 초기값을 설정해준다
            ViewModel = new MainViewModel(appModel);
            mainview.SetInitViewModel(ViewModel);

            System.Windows.Application.Current.MainWindow = this;
        }

        // x 버튼으로 종료시를 위한 오버라이드
        protected override void OnClosing(CancelEventArgs e)
        {
            // 프로그램 종료에 대한부분
            if (true)
            {
                e.Cancel = true;
                this.Hide();
            }
            base.OnClosing(e);
        }

        // ESC 버튼으로 창닫기
        private void OnCloseExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ViewModel.AppModel.AppSetting.IsESCClose == true)
            {
                this.Hide();
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 윈도 시작시 시작프로그램에 설정
            SetRegWinStartup();

            // 트레이 아이콘 마우스 오른쪽 메뉴
            SetContextMenu();
        }

        private void SetRegWinStartup()
        {
        }

        private void SetContextMenu()
        {
            try
            {
                System.Windows.Forms.ContextMenu menu = new ContextMenu();
                notify = new NotifyIcon
                {
                    Icon = Properties.Resources.Mediage,
                    Visible = true,
                    ContextMenu = menu,
                    Text = "MediAge - Cloud Drive"
                };
                notify.DoubleClick += (click_sender, click_e) => { ReShowWindow(); };

                var menu1 = new MenuItem()
                {
                    Index = 0,
                    Text = "열기"
                };
                menu1.Click += (click_sender, click_e) => { ReShowWindow(); };
                menu.MenuItems.Add(menu1);
                menu.MenuItems.Add(new MenuItem()
                {
                    Index = 1,
                    Text = "MediAge - Report Cloud Drive",
                    Enabled = false
                });
                menu.MenuItems.Add(new MenuItem("-"));  //seperator
                var menu2 = new MenuItem()
                {
                    Index = 3,
                    Text = "프로그램 종료"
                };
                menu2.Click += (click_sender, click_e) => { ExitApplication(); };
                menu.MenuItems.Add(menu2);
            }
            catch (Exception ex)
            {
                // log에 저장
                Debug.Assert(false, ex.Message);
                Debug.WriteLine(ex.Message);
            }
        }

        private void ReShowWindow()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Visibility = Visibility.Visible;
        }

        private void ExitApplication()
        {
            notify.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        private void mainview_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}