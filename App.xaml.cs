using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NFC_Reader_WPF
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Mainn(object sender, StartupEventArgs e)
        {
            MessageBox.Show("hello");
            startApp();
            string a = QRcodeRead();
            string b = NFCcodeRead();

            //((MainWindow)Current.MainWindow).txtQRcode.Text = a;
            //((MainWindow)Current.MainWindow).txtNFC_Code.Text = b;
        }
        protected void startApp ()
        {
            this.InitializeComponent();
            this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
        }
        private string QRcodeRead()
        {
            return "3004K";
        }
        private string NFCcodeRead()
        {
            return "04-15-91-3A-2B-00-20-90-00";
        }
    }
}
