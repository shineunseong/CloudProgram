using System.Text.RegularExpressions;
using System.Windows.Controls;
using MediCloudDrive.Biz;
using MediCloudDrive.ViewModels;

namespace MediCloudDrive.Views
{
    /// <summary>
    /// OptionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OptionView : UserControl
    {
        public OptionViewModel ViewModel { get; set; }

        private MediAES aes = new MediAES();

        public OptionView()
        {
            InitializeComponent();
        }

        public void SetOptionModel(MainViewModel parentModel)
        {
            this.DataContext = ViewModel = new OptionViewModel(parentModel);

            if (false == string.IsNullOrEmpty(ViewModel.CloudInfoModel.PrivateKey))
            {
                psBox.Password = aes.GetDeCryptString(ViewModel.CloudInfoModel.PrivateKey);
            }

            ViewModel.GetCloudKey = GetCloudPrivateKey;
        }

        public void GetCloudPrivateKey()
        {
            if (string.IsNullOrEmpty(psBox.Password)) return;

            ViewModel.CloudInfoModel.PrivateKey = aes.GetEncryptString(psBox.Password);
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsNumeric(e.Text);
        }

        private bool IsNumeric(string source)
        {
            Regex regex = new Regex("[^0-9.-]+");

            return !regex.IsMatch(source);
        }
    }
}