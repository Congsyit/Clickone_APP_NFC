using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Reactive.Bindings;

namespace NFC_Reader_WPF
{
    class status
    {
        public string status1 { get; set; }
    }
    public class Process
    {
        public ReactiveProperty<string> TXTNAME { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ValueQRCode { get; set; }
        public ReactiveProperty<string> TXTKANA { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> TXTDATE { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> TXTGENDER { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> TXTNFCCode { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> TXT_ID { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<int> CurrentProgress { get; set; }
        public ReactiveProperty<object> IsProgressBarVisible { get; set; } = new ReactiveProperty<object>();

        string BienPUT;
        string _id = "";
        string NFC_code = "";
        bool checkID = false;
        string memo;

        public ReactiveCommand LoadInfo { get; set; } = new ReactiveCommand();
        public ReactiveCommand WriteNFC { get; set; } = new ReactiveCommand();
        public ReactiveCommand BtnReadNFC { get; set; } = new ReactiveCommand();

        public Process()
        {
            TXTNAME = new ReactiveProperty<string>();
            ValueQRCode = new ReactiveProperty<string>();
            TXTKANA = new ReactiveProperty<string>();
            TXTDATE = new ReactiveProperty<string>();
            TXTGENDER = new ReactiveProperty<string>();
            TXTNFCCode = new ReactiveProperty<string>();
            TXT_ID = new ReactiveProperty<string>();

            LoadInfo = new ReactiveCommand();
            WriteNFC = new ReactiveCommand();
            BtnReadNFC = new ReactiveCommand();

            CurrentProgress = new ReactiveProperty<int>();
            IsProgressBarVisible = new ReactiveProperty<object>();
            IsProgressBarVisible.Value = Visibility.Visible;

            LoadInfo.Subscribe(() =>
                {
                    getdata();
                });
            WriteNFC.Subscribe(() =>
            {
                writeNFCCode();

            });

            BtnReadNFC.Subscribe(() =>
            {
                ReadNFCCode();
            });
        }
        public void getdata()
        {
            // chuyen huong process bar the hien tien trinh cua ung dung theo thoi gian thuc
            // Home 
            if (ValueQRCode.Value == "" || ValueQRCode.Value ==null)
            {
                MessageBox.Show("患者番号を入力してください!  \n\n患者番号がすでに入力されている場合、患者が今日の予定であるかどうかを確認してください。", "システムからの通知！", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    Task.Run(async () =>
                        {
                            using (var client = new HttpClient())
                            {
                                CurrentProgress.Value = 0;
                                var MyIni = new IniFile("config.ini");
                                string url = MyIni.Read("url", "config").ToString();
                                string token = MyIni.Read("token", "config").ToString();
                                CurrentProgress.Value = 10;
                                string date = DateTime.Now.ToString("yyyy-MM-dd");
                                var request = new HttpRequestMessage(HttpMethod.Get, url + "api/books?date=" + date);
                                CurrentProgress.Value = 20;
                                request.Headers.Add("ContentType", "application/json");
                                request.Headers.Add("Authorization", "Bearer " + token);
                                CurrentProgress.Value = 30;
                                var response = await client.SendAsync(request);
                                CurrentProgress.Value = 40;
                                string a = await response.Content.ReadAsStringAsync();
                                var content = JsonConvert.DeserializeObject<List<PUT1Bien.MyArray>>(a);
                                CurrentProgress.Value = 50;
                                for (int i = 0; i < content.Count; i++)
                                {
                                    if (content[i].card != null)
                                    {
                                        if (content[i].card.cid != null)
                                        {
                                            if (content[i].card.cid.ToString() == ValueQRCode.Value.ToString())
                                            {
                                                memo = content[i].card.memo.ToString();
                                                checkID = true;
                                                CurrentProgress.Value = 100;
                                                TXTNAME.Value = content[i].card.name.ToString();
                                                TXTKANA.Value = content[i].card.kana.ToString();
                                                TXTDATE.Value = content[i].card.birthday.ToString();
                                                TXTGENDER.Value = content[i].card.gender.ToString();
                                                TXT_ID.Value = content[i].card._id.ToString();
                                                _id = content[i].card._id.ToString();
                                            } 
                                        }
                                    }
                                }
                                if (checkID == false)
                                {
                                    TXTNAME.Value = string.Empty;
                                    TXTKANA.Value = string.Empty;
                                    TXTDATE.Value = string.Empty;
                                    TXTGENDER.Value = string.Empty;
                                    TXT_ID.Value = string.Empty;
                                    MessageBox.Show("申し訳ありませんが、患者は今日訪問する予定はありません！\n\n再度確認してください！", "システムからの通知！", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }

                                checkID = false;
                            }
                        });
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show("Error! \n" + ex);
                }
            }
            
        }
        public void writeNFCCode()
        {
            
            if (NFC_code == "")
            {
                MessageBox.Show("NFCコードをまだ読んでいないので、リーダーにNFCコードを置いて、\n\n[NFCコードを読む]ボタンを押してください。", "システムからの通知！", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                bool kiemtrachuoi = memo.Contains("(NFC)");
                TXTNFCCode.Value = NFC_code;
                try
                {
                    Task.Run(async () =>
                        {
                            using (var client = new HttpClient())
                            {
                                
                                if(memo == null || memo =="" )
                                {
                                    BienPUT = "(NFC)"+NFC_code+"(end)";//kiem tra xem benh nhan da co ma NFC chua, co roi thi sua lai ma moi, chua co thi them ma moi
                                }else if(kiemtrachuoi == true)
                                {
                                    var resuilt = MessageBox.Show("患者はすでにNFCコードを持っていますが、患者のNFCコードを既存のコードに置き換えたいですか？", "システムからの通知！", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                    if (resuilt == MessageBoxResult.Yes)
                                    {
                                        int vitridautien = memo.IndexOf("(NFC)");
                                        memo = memo.Substring(0, vitridautien); //kiem tra co thi xoa
                                        BienPUT = memo+"(NFC)"+NFC_code+"(end)";
                                        
                                    }else
                                    {
                                        BienPUT = memo;
                                    }
                                }
                                else
                                {
                                    BienPUT = memo + "\n\n(NFC)" + NFC_code + "(end)";
                                }
                                //MessageBox.Show("" + BienPUT);
                                var MyIni = new IniFile("config.ini");
                                string url = MyIni.Read("url", "config").ToString();
                                string token = MyIni.Read("token", "config").ToString();

                                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                                Hashtable hashtable = new Hashtable();
                                var parameters = new Hashtable
                                {
                                    ["memo"] = BienPUT
                                };

                                var json = JsonConvert.SerializeObject(parameters);
                                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                                var response = await client.PutAsync(url + "api/cards/" + TXT_ID.Value, httpContent);
                                if (response.IsSuccessStatusCode)
                                {
                                    MessageBox.Show("NFCコードを患者 '" + ValueQRCode.Value + " 'に正常に更新しました!", "システムからの通知！", MessageBoxButton.OK, MessageBoxImage.Information);
                                    TXTDATE.Value = string.Empty;
                                    TXTGENDER.Value = string.Empty;
                                    TXTKANA.Value = string.Empty;
                                    TXTNAME.Value = string.Empty;
                                    TXTNFCCode.Value = string.Empty;
                                    TXT_ID.Value = string.Empty;
                                    ValueQRCode.Value = string.Empty;
                                }
                                else
                                {
                                    MessageBox.Show("NFCコードを患者' " + ValueQRCode.Value + " 'に更新できませんでした！!", "システムからの通知！", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }

                        }).Wait();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public string ReadNFCCode()
        {
            string IDcard;
           
            IntPtr hContext = IntPtr.Zero;
            // ##################################################
            // 1. SCardEstablishContext
            // ##################################################
            var Myini = new IniFile();
            string QRcodeClean = Myini.Read("clean", "configuserup").ToString();
            uint ret = nfcapi.SCardEstablishContext(pcsc_const.SCARD_SCOPE_USER, IntPtr.Zero, IntPtr.Zero, out hContext);
            uint pcchReaders = 0;
            // NFCリーダの文字列バッファのサイズを取得
            ret = nfcapi.SCardListReaders(hContext, null, null, ref pcchReaders);
            var readerName = "";
            IntPtr hCard = IntPtr.Zero;

            SCardEstablishContext();
            SCardListReaders();
            SCardConnect();
            //SCardTransmit();

            void SCardEstablishContext()
            {
                Console.WriteLine("***** 1. SCardEstablishContext *****");
                if (ret != pcsc_const.SCARD_S_SUCCESS)
                {
                    string message;
                    switch (ret)
                    {
                        case pcsc_const.SCARD_E_NO_SERVICE:
                            message = "サービスが起動されていません。";
                            break;
                        default:
                            message = "サービスに接続できません。code = " + ret;
                            break;
                    }
                    throw new ApplicationException(message);
                }
                if (hContext == IntPtr.Zero)
                {
                    throw new ApplicationException("コンテキストの取得に失敗しました。");
                }
                Console.WriteLine("　サービスに接続しました。");
            }

            // ##################################################
            // 2. SCardListReaders
            // ##################################################

            void SCardListReaders()
            {
                Console.WriteLine("***** 2. SCardListReaders *****");
                if (ret != pcsc_const.SCARD_S_SUCCESS)
                {
                    // 検出失敗
                    throw new ApplicationException("NFCリーダを確認できません");
                }
                // NFCリーダの文字列を取得
                byte[] mszReaders = new byte[pcchReaders * 2]; // 1文字2byte
                ret = nfcapi.SCardListReaders(hContext, null, mszReaders, ref pcchReaders);
                if (ret != pcsc_const.SCARD_S_SUCCESS)
                {
                    //検出失敗
                    throw new ApplicationException("NFCリーダの取得に失敗しました。");
                }
                UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
                string readerNameMultiString = unicodeEncoding.GetString(mszReaders);
                // 認識したNDCリーダの最初の1台を使用
                int nullindex = readerNameMultiString.IndexOf((char)0);
                readerName = readerNameMultiString.Substring(0, nullindex);
                Console.WriteLine("　NFCリーダを検出しました。 " + readerName);
            }
            // ##################################################
            // 3. SCardConnect
            // ##################################################
            //
            void SCardConnect()
            {
                //Console.WriteLine("***** 3. SCardConnect *****");
                do
                {
                    //Console.WriteLine("***** 3. SCardConnect *****");
                    //IntPtr hCard = IntPtr.Zero;
                    IntPtr activeProtocol = IntPtr.Zero;
                    ret = nfcapi.SCardConnect(hContext, readerName, pcsc_const.SCARD_SHARE_SHARED, pcsc_const.SCARD_PROTOCOL_T1, ref hCard, ref activeProtocol);
                } while (ret != pcsc_const.SCARD_S_SUCCESS);
                //Console.WriteLine("　カードに接続しました。");
                SCardTransmit();
            }

            void SCardTransmit()   
            {
                //Console.WriteLine("***** 4. SCardTransmit *****");//
                uint maxRecvDataLen = 256;
                var recvBuffer = new byte[maxRecvDataLen + 2];
                var sendBuffer = new byte[] { 0xff, 0xca, 0x00, 0x00, 0x00 };  // ← IDmを取得するコマンド

                nfcapi.SCARD_IO_REQUEST ioRecv = new nfcapi.SCARD_IO_REQUEST();
                ioRecv.cbPciLength = 255;

                int pcbRecvLength = recvBuffer.Length;
                int cbSendLength = sendBuffer.Length;

                IntPtr handle = nfcapi.LoadLibrary("Winscard.dll");
                IntPtr pci = nfcapi.GetProcAddress(handle, "g_rgSCardT1Pci");
                nfcapi.FreeLibrary(handle);

                ret = nfcapi.SCardTransmit(hCard, pci, sendBuffer, cbSendLength, ioRecv, recvBuffer, ref pcbRecvLength);
                //if (ret != pcsc_const.SCARD_S_SUCCESS)
                //{
                //    throw new ApplicationException("NFCカードへの送信に失敗しました。code = " + ret);
                //}
                do
                {
                    ret = nfcapi.SCardTransmit(hCard, pci, sendBuffer, cbSendLength, ioRecv, recvBuffer, ref pcbRecvLength);
                    if (ret != pcsc_const.SCARD_S_SUCCESS)
                        SCardConnect();
                } while (ret != pcsc_const.SCARD_S_SUCCESS);
                // 受信データからIDmを抽出する
                // recvBuffer = IDm + SW1 + SW2 (SW = StatusWord)
                // SW1 = 0x90 (144) SW1 = 0x00 (0) で正常だが、ここでは見ていない
                string cardId = BitConverter.ToString(recvBuffer, 0, pcbRecvLength);
                //Console.WriteLine("　カードからデータを取得しました。");
                //MessageBox.Show("　【IDm】：" + cardId);
                Console.Beep(1220, 100);
                int milisecond = 1000;

                IDcard = cardId;
                if (IDcard == "")
                {
                    SCardConnect();
                }
                else
                {
                    TXTNFCCode.Value = IDcard;
                    NFC_code = IDcard;
                    //Thread.Sleep(milisecond);
                    //IDcard = "";
                    //SCardConnect();
                }
                
            }
            return IDcard;
        }


    }
}
