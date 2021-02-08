using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;

namespace NFC_Reader_WPF
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Process();
        }
        private void txtWriteNFC_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Task.Run(async () =>
            //        {
            //            using (var client = new HttpClient())
            //            {
            //                var MyIni = new IniFile("config.ini");
            //                string url = MyIni.Read("url", "config").ToString();
            //                string token = MyIni.Read("token", "config").ToString();
                            
            //                string date = DateTime.Now.ToString("yyyy-MM-dd");
            //                var request = new HttpRequestMessage(HttpMethod.Get, url + "api/books?date=" + date);
            //                request.Headers.Add("ContentType", "application/json");
            //                request.Headers.Add("Authorization", "Bearer " + token);

            //                var response = await client.SendAsync(request);
            //                string a = await response.Content.ReadAsStringAsync();
            //                var content = JsonConvert.DeserializeObject<List<PUT1Bien.MyArray>>(a);

            //                for(int i =0; i<content.Count; i++)
            //                {
            //                    if(content[i].card !=null)
            //                    {
            //                       if( content[i].card.cid!=null)
            //                        {
            //                            if(content[i].card.cid.ToString() == MyValue.ToString())
            //                            {
            //                                TXTNAME = content[i].card.name.ToString();
            //                                TXTKANA = content[i].card.kana.ToString();

            //                                TXTDATE = content[i].card.birthday.ToString();
            //                                TXTGENDER = content[i].card.gender.ToString();
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        });
            //}catch(Exception ex)
            //{
            //    MessageBox.Show("Error! \n" + ex);
            //}
        }
    }
   

}
