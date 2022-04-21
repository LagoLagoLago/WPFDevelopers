using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace WPFDevelopers.Samples.ExampleViews
{
    /// <summary>
    /// CutImageExample.xaml 的交互逻辑
    /// </summary>
    public partial class CutImageExample : UserControl
    {
        public CutImageExample()
        {
            InitializeComponent();
            this.btnImport.Click += OnImportClickHandler;
            this.btnSave.Click += BtnSave_Click;
        }
        private void OnImportClickHandler(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "图像文件(*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;";
            if (openFileDialog.ShowDialog() == true)
            {

                using (var reader = new BinaryReader(File.Open(openFileDialog.FileName, FileMode.Open)))
                {
                    try
                    {
                        var fileInfo = new FileInfo(openFileDialog.FileName);
                        var bytes = reader.ReadBytes((int)fileInfo.Length);
                        reader.Close();
                        var bitmapImage = new BitmapImage
                        {
                            CacheOption = BitmapCacheOption.OnLoad
                        };
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = new MemoryStream(bytes);
                        bitmapImage.EndInit();
                        cutCustoms.ImageSource = bitmapImage;
                    }
                    catch (Exception ex)
                    {
                        //JACommonUtils.ShowException(ex);
                    }
                }

                //cutCustoms.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName));
                btnSave.IsEnabled = true;
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.jpg";
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "image file|*.jpg";

            if (dlg.ShowDialog() == true)
            {
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create((BitmapSource)image2.ImageSource));
                using (var fs = System.IO.File.OpenWrite(dlg.FileName))
                {
                    pngEncoder.Save(fs);
                    fs.Dispose();
                    fs.Close();
                }
            }

        }
    }
}
