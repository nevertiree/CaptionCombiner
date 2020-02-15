using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaptionCombiner
{
    /// <summary>
    /// CaptionCombinerHome.xaml 的交互逻辑
    /// </summary>
    public partial class CaptionCombinerHome : Page
    {
        public CaptionCombinerHome()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 选择文件和文件夹对话框
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff",
                Multiselect = true,
                Title = "选择需要凭借的视频截图"
            };
            var result = openFileDialog.ShowDialog();

            NavigationService.GetNavigationService(this).Navigate(new SortingPage(openFileDialog.FileNames));
        }
    }
}
