using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Telegram.Bot.Types.Enums;
using static UpbitAuto.UpbitAPI;

namespace UpbitAuto
{
    public partial class Form1 : Form
    {
        private List<Offer> OfferList = new List<Offer>();
        private List<OfferPack.OfferN> OfferListN = new List<OfferPack.OfferN>();
        private List<OfferPack.OfferNN> OfferListNN = new List<OfferPack.OfferNN>();
        private List<Mode.TModeList> TMode = new List<Mode.TModeList>();

        //string[] markets = null; //{ "KRW-BTC", "KRW-ETH", "KRW-NEO", "KRW-MTL", "KRW-LTC", "KRW-XRP", "KRW-ETC", "KRW-OMG", "KRW-SNT", "KRW-WAVES", "KRW-XEM", "KRW-QTUM", "KRW-LSK", "KRW-STEEM", "KRW-XLM", "KRW-ARDR", "KRW-KMD", "KRW-ARK", "KRW-STORJ", "KRW-GRS", "KRW-REP", "KRW-EMC2", "KRW-ADA", "KRW-SBD", "KRW-POWR", "KRW-BTG", "KRW-ICX", "KRW-EOS", "KRW-TRX", "KRW-SC", "KRW-GTO", "KRW-IGNIS", "KRW-ONT", "KRW-ZIL", "KRW-POLY", "KRW-ZRX", "KRW-SRN", "KRW-LOOM", "KRW-BCH", "KRW-ADX", "KRW-BAT", "KRW-IOST", "KRW-DMT", "KRW-RFR", "KRW-CVC", "KRW-IQ", "KRW-IOTA", "KRW-OST", "KRW-MFT", "KRW-ONG", "KRW-GAS", "KRW-UPP", "KRW-ELF", "KRW-KNC", "KRW-BSV", "KRW-THETA", "KRW-EDR", "KRW-QKC", "KRW-BTT", "KRW-MOC", "KRW-ENJ", "KRW-TFUEL", "KRW-MANA", "KRW-ANKR", "KRW-NPXS", "KRW-AERGO", "KRW-ATOM", "KRW-TT", "KRW-CRE", "KRW-SOLVE", "KRW-MBL", "KRW-TSHP", "KRW-WAXP", "KRW-HBAR", "KRW-MED", "KRW-MLK", "KRW-STPT", "KRW-ORBS", "KRW-VET", "KRW-CHZ", "KRW-PXL", "KRW-STMX", "KRW-DKA", "KRW-HIVE", "KRW-KAVA", "KRW-AHT", "KRW-SPND", "KRW-LINK", "KRW-XTZ", "KRW-BORA", "KRW-JST", "KRW-CRO", "KRW-TON", "KRW-SXP", "KRW-LAMB", "KRW-HUNT", "KRW-MARO", "KRW-PLA", "KRW-DOT", "KRW-SRM", "KRW-MVL", "KRW-PCI", "KRW-STRAX", "KRW-AQT", "KRW-BCHA", "KRW-GLM", "KRW-QTCON", "KRW-SSX", "KRW-META", "KRW-OBSR", "KRW-FCT2", "KRW-LBC", "KRW-CBK", "KRW-SAND" };

        int limteTime = 333;
        int bttime = Environment.TickCount;

        string nonce = Guid.NewGuid().ToString();

        private List<MarketInfo> GetMarketList(UpbitAPI U)
        {
            string marketAll = U.GetMarkets();
            //Console.WriteLine(marketAll);
            return JsonConvert.DeserializeObject<List<MarketInfo>>(marketAll);
        }

        private List<CoinInfo> GetTicker(UpbitAPI U, string CoinName)
        {

            try
            {
                Invoke((MethodInvoker)(() => toolStripStatusLabel4.Text = $"HB Upbit Bot / {limteTime}"));
                if (Environment.TickCount - bttime < limteTime)
                {
                    //Console.WriteLine("Sleep " + (limteTime - (Environment.TickCount - bttime)));
                    Thread.Sleep(limteTime - (Environment.TickCount - bttime));
                }

                //Console.WriteLine(CoinName);
                nonce = Guid.NewGuid().ToString();
                U.nonce = nonce;
                string marketAll = U.GetTicker(CoinName);
                //Console.WriteLine(marketAll);
                bttime = Environment.TickCount;
                if (!marketAll.Contains("Too many API requests.") && !marketAll.Contains("error"))
                {
                    //Console.WriteLine("GetTicker Too many API requests.");
                    return JsonConvert.DeserializeObject<List<CoinInfo>>(marketAll);
                }

                else
                {
                    LogWrite($"[{processID} - GetTicker]" + nonce + Environment.NewLine + "\t" + marketAll, "Log\\" + processID + "_Log.txt");
                    Console.WriteLine("GetTicker " + marketAll);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogWrite($"[{processID} - GetTicker]" + ex.Message, "Log\\" + processID + "_Log.txt");
                return null;
            }
        }



        private List<OrdersResponse> GetAllOrder(UpbitAPI U)
        {
            try
            {
                Invoke((MethodInvoker)(() => toolStripStatusLabel4.Text = $"HB Upbit Bot / {limteTime}"));
                if (Environment.TickCount - bttime < limteTime)
                {
                    //Console.WriteLine("Sleep " + (limteTime - (Environment.TickCount - bttime)));
                    Thread.Sleep(limteTime - (Environment.TickCount - bttime));
                }

                nonce = Guid.NewGuid().ToString();
                U.nonce = nonce;
                List<OrdersResponse> Teams;
                string marketAll = U.GetAllOrder();
                //Console.WriteLine(marketAll);
                bttime = Environment.TickCount;
                //LogWrite("Message: " + marketAll + Environment.NewLine, "Log\\MarketAll_" + processID + "_Log.txt");
                if (!marketAll.Contains("Too many API requests.") && !marketAll.Contains("error"))
                {
                    Teams = JsonConvert.DeserializeObject<List<OrdersResponse>>(marketAll);
                    return Teams;
                }
                else
                {
                    LogWrite($"[{processID} - GetAllOrder0]" + nonce + Environment.NewLine + "\t" + marketAll, "Log\\" + processID + "_Log.txt");
                    //Console.WriteLine($"[{processID} - GetOrder] {UUID}" + Environment.NewLine + "\t" + marketAll);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogWrite($"[{processID} - GetAllOrder1] " + ex.Message, "Log\\" + processID + "_Log.txt");
                return null;
            }
        }


        private OrdersResponse GetOrder(UpbitAPI U, string UUID)
        {
            try
            {
                Invoke((MethodInvoker)(() => toolStripStatusLabel4.Text = $"HB Upbit Bot / {limteTime}"));
                if (Environment.TickCount - bttime < limteTime)
                {
                    //Console.WriteLine("Sleep " + (limteTime - (Environment.TickCount - bttime)));
                    Thread.Sleep(limteTime - (Environment.TickCount - bttime));
                }
                nonce = Guid.NewGuid().ToString();
                U.nonce = nonce;
                string marketAll = U.GetOrder(UUID);
                //Console.WriteLine(marketAll);
                bttime = Environment.TickCount;


                OrdersResponse Teams;
                Teams = JsonConvert.DeserializeObject<OrdersResponse>(marketAll);


                if (!marketAll.Contains("Too many API requests.") && !marketAll.Contains("error"))
                {
                    //Console.WriteLine("GetOrder Too many API requests.");
                    return Teams;
                }
                else
                {
                    LogWrite($"[{processID} - GetOrder0] {nonce}" + Environment.NewLine + "\t" + marketAll, "Log\\" + processID + "_Log.txt");
                    //Console.WriteLine($"[{processID} - GetOrder] {UUID}" + Environment.NewLine + "\t" + marketAll);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogWrite($"[{processID} - GetOrder1]" + ex.Message, "Log\\" + processID + "_Log.txt");
                return null;
            }
        }


        private List<UserInfo> GetAccount()
        {
            try
            {
                Invoke((MethodInvoker)(() => toolStripStatusLabel4.Text = $"HB Upbit Bot / {limteTime}"));
                if (Environment.TickCount - bttime < limteTime)
                {
                    //Console.WriteLine("Sleep " + (limteTime - (Environment.TickCount - bttime)));
                    Thread.Sleep(limteTime - (Environment.TickCount - bttime));
                }
                nonce = Guid.NewGuid().ToString();
                U.nonce = nonce;
                string marketAll = U.GetAccount();
                bttime = Environment.TickCount;
                if (!marketAll.Contains("Too many API requests.") && !marketAll.Contains("error"))
                    return JsonConvert.DeserializeObject<List<UserInfo>>(marketAll);
                else
                {
                    LogWrite($"[{processID} - GetAccount]" + nonce + Environment.NewLine + "\t" + marketAll, "Log\\" + processID + "_Log.txt");
                    Console.WriteLine("GetAccount " + marketAll);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogWrite($"[{processID} - GetAccount]" + ex.Message, "Log\\" + processID + "_Log.txt");
                return null;
            }
        }

        private OrdersResponse MakeOrderB(string CoinName, string volume, string price, UpbitAPI.UpbitOrderType type)
        {
            try
            {
                Invoke((MethodInvoker)(() => toolStripStatusLabel4.Text = $"HB Upbit Bot / {limteTime}"));
                if (Environment.TickCount - bttime < limteTime)
                {
                    //Console.WriteLine("Sleep " + (limteTime - (Environment.TickCount - bttime)));
                    Thread.Sleep(limteTime - (Environment.TickCount - bttime));
                }
                nonce = Guid.NewGuid().ToString();
                U.nonce = nonce;
                string marketAll = U.MakeOrder(CoinName, UpbitAPI.UpbitOrderSide.bid, volume, price, type);
                //Console.WriteLine(marketAll);
                bttime = Environment.TickCount;
                if (!marketAll.Contains("Too many API requests.") && !marketAll.Contains("error"))
                {
                    return JsonConvert.DeserializeObject<OrdersResponse>(marketAll);
                }

                else
                {
                    LogWrite($"[{processID} - MakeOrderB]" + nonce + Environment.NewLine + "\t" + marketAll, "Log\\" + processID + "_Log.txt");
                    Console.WriteLine($"MakeOrderB {CoinName}, {volume}, {price}, {Maxprice}" + Environment.NewLine + "\t" + marketAll);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogWrite($"[{processID} - MakeOrderB]" + ex.Message, "Log\\" + processID + "_Log.txt");
                return null;
            }
        }


        private OrdersResponse MakeOrderS(string CoinName, decimal volume, string price, UpbitAPI.UpbitOrderType type)
        {
            try
            {
                Invoke((MethodInvoker)(() => toolStripStatusLabel4.Text = $"HB Upbit Bot / {limteTime}"));
                if (Environment.TickCount - bttime < limteTime)
                {
                    //Console.WriteLine("Sleep " + (limteTime - (Environment.TickCount - bttime)));
                    Thread.Sleep(limteTime - (Environment.TickCount - bttime));
                }

                nonce = Guid.NewGuid().ToString();
                U.nonce = nonce;
                string marketAll = U.MakeOrder(CoinName, UpbitAPI.UpbitOrderSide.ask, volume, price, type);
                //Console.WriteLine(marketAll);
                bttime = Environment.TickCount;
                if (!marketAll.Contains("Too many API requests.") && !marketAll.Contains("error"))
                {
                    //Console.WriteLine("MakeOrderB Too many API requests.");
                    return JsonConvert.DeserializeObject<OrdersResponse>(marketAll);
                }
                else
                {
                    LogWrite($"[{processID} - MakeOrderS]" + nonce + Environment.NewLine + "\t" + marketAll, "Log\\" + processID + "_Log.txt");
                    Console.WriteLine($"MakeOrderS {CoinName}, {volume}, {price}" + Environment.NewLine + "\t" + marketAll);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogWrite($"[{processID} - MakeOrderS]" + ex.Message, "Log\\" + processID + "_Log.txt");
                return null;
            }
        }


        private CancelResponse CancelOrder(string UUID)
        {
            try
            {
                Invoke((MethodInvoker)(() => toolStripStatusLabel4.Text = $"HB Upbit Bot / {limteTime}"));
                if (Environment.TickCount - bttime < limteTime)
                {
                    //Console.WriteLine("Sleep " + (limteTime - (Environment.TickCount - bttime)));
                    Thread.Sleep(limteTime - (Environment.TickCount - bttime));
                }

                nonce = Guid.NewGuid().ToString();
                U.nonce = nonce;
                string marketAll = U.CancelOrder(UUID);
                CancelResponse res;
                bttime = Environment.TickCount;
                if (!marketAll.Contains("Too many API requests.") && !marketAll.Contains("error"))
                {
                    res = JsonConvert.DeserializeObject<CancelResponse>(marketAll);
                    //Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} [주문 취소] {res.price}, {res.volume}, {res.executed_volume}");
                    return res;
                }
                else
                {
                    LogWrite($"[{processID} - CancelOrder]" + nonce + Environment.NewLine + "\t" + marketAll, "Log\\" + processID + "_Log.txt");
                    Console.WriteLine($"CancelOrder0 \t" + marketAll);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogWrite($"[{processID} - CancelOrder1]" + ex.Message, "Log\\" + processID + "_Log.txt");
                return null;
            }
        }


        //U.CancelOrder("주문 uuid")

        private List<Orderbook> GetOrderBook()
        {
            try
            {
                Invoke((MethodInvoker)(() => toolStripStatusLabel4.Text = $"HB Upbit Bot / {limteTime}"));
                if (Environment.TickCount - bttime < limteTime)
                {
                    //Console.WriteLine("Sleep " + (limteTime - (Environment.TickCount - bttime)));
                    Thread.Sleep(limteTime - (Environment.TickCount - bttime));
                }
                nonce = Guid.NewGuid().ToString();
                U.nonce = nonce;
                string marketAll = U.GetOrderbook(textBox10.Text);
                bttime = Environment.TickCount;
                if (!marketAll.Contains("Too many API requests.") && !marketAll.Contains("error"))
                    return JsonConvert.DeserializeObject<List<Orderbook>>(marketAll);
                else
                {
                    LogWrite($"[{processID} - GetOrderBook]" + nonce + Environment.NewLine + "\t" + marketAll, "Log\\" + processID + "_Log.txt");
                    Console.WriteLine("GetOrderBook " + marketAll);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogWrite($"[{processID} - GetOrderBook]" + ex.Message, "Log\\" + processID + "_Log.txt");
                return null;
            }
        }

        private List<OfferPack.CandlesResponse> GetCandles_Minute(string market, UpbitMinuteCandleType unit, DateTime date = default(DateTime), int count = 1)
        {
            try
            {
                Invoke((MethodInvoker)(() => toolStripStatusLabel4.Text = $"HB Upbit Bot / {limteTime}"));
                if (Environment.TickCount - bttime < limteTime)
                {
                    //Console.WriteLine("Sleep " + (limteTime - (Environment.TickCount - bttime)));
                    Thread.Sleep(limteTime - (Environment.TickCount - bttime));
                }
                nonce = Guid.NewGuid().ToString();
                U.nonce = nonce;
                string marketAll = U.GetCandles_Minute(market, unit, date, count);
                bttime = Environment.TickCount;
                if (!marketAll.Contains("Too many API requests.") && !marketAll.Contains("error"))
                    return JsonConvert.DeserializeObject<List<OfferPack.CandlesResponse>>(marketAll);
                else
                {
                    LogWrite($"[{processID} - GetCandles_Minute]" + nonce + Environment.NewLine + "\t" + marketAll, "Log\\" + processID + "_Log.txt");
                    Console.WriteLine("GetCandles_Minute " + marketAll);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogWrite($"[{processID} - GetCandles_Minute]" + ex.Message, "Log\\" + processID + "_Log.txt");
                return null;
            }
        }






        public static string encryption()
        {
            string[] alpabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "y", "z" };
            string[] number = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

            Random nalpabet = new Random();
            Random nnumber = new Random();
            string password;
            password = alpabet[nalpabet.Next(0, 24)] + alpabet[nalpabet.Next(0, 24)] + alpabet[nalpabet.Next(0, 24)];
            return password;
        }





        UpbitAPI U;
        string processID = null;


        private void button11_Click_1(object sender, EventArgs e)
        {
            try
            {
                string market = textBox2.Text;

                decimal Nper = Convert.ToDecimal(textBox3.Text);
                int Nprice = Convert.ToInt32(textBox4.Text);
                //String.Format("{0:#,0}", XX.Sprice)

                //Invoke((MethodInvoker)(() => dataGridView1.Rows[0].Cells[3].Value = String.Format("{0:#,0}", 1000)  ));
                //Invoke((MethodInvoker)(() => dataGridView1.Rows[1].Cells[4].Value = String.Format("{0:#,0}", 1000)  ));


                string[] addstr = { textBox2.Text, Nper.ToString(), Nprice.ToString(), "0", "0", "대기" };
                OfferListN.Add(new OfferPack.OfferN(OfferListN.Count, textBox2.Text, Nper, Nprice, false, null, false, 0, 0));
                dataGridView1.Rows.Add(addstr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Setting Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void jsonsplit(string str)
        {
            var abc = str.Split('{');
            foreach (var aa in abc)
            {
                var parse = aa.Split(',');
                foreach (var xx in parse)
                {

                    Console.WriteLine(xx);
                }
            }
        }

        string Accesskey = null;
        string Secretkey = null;

        private void button4_Click_2(object sender, EventArgs e)
        {
            Accesskey = textBox12.Text;
            Secretkey = textBox13.Text;

            U = new UpbitAPI(Accesskey, Secretkey);

            Properties.Settings.Default.AccesskeySetting = this.Accesskey;
            Properties.Settings.Default.SecretkeySetting = this.Secretkey;
            Properties.Settings.Default.Save();
            button6.Enabled = true;
        }



        private Telegram.Bot.TelegramBotClient Bot = new Telegram.Bot.TelegramBotClient("1690203305:AAGu_CIEo9IdkHOVDDnbo_kRDDsEBlzor2s");
        // init methods... 
        private async void telegramAPIAsync()
        { //Token Key 를 이용하여 봇을 가져온다. 

            //Bot 에 대한 정보를 가져온다. 
            var me = await Bot.GetMeAsync();
            //Bot 의 이름을 출력한다.
            Console.WriteLine("Hello my name is {0}", me.FirstName);

            //var message = e.Message;
        }

        private void setTelegramEvent()
        {
            Bot.OnMessage += Bot_OnMessage;       // 이벤트를 추가해줍니다. 


            Bot.StartReceiving();        // 이 함수가 실행이 되어야 사용자로부터 메세지를 받을 수 있습니다.
        }

        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            Console.WriteLine($"{message.Chat.Id}, {message.Text}");
            if (message == null || message.Type != MessageType.Text) return;

            if (message.Text.Contains("@ID"))
            {
                //MessageBox.Show("User Chat ID: " + message.Chat.Id); 
                await Bot.SendTextMessageAsync(message.Chat.Id, $"ChatID: {message.Chat.Id}\nBy {processID}");
                return;
            }
        }


        public Form1()
        {
            InitializeComponent();
            //panel3.Dock = DockStyle.Fill;

            mainView.CellValueChanged += new DataGridViewCellEventHandler(mainView_CellValueChanged);
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(Datagrid_CellValueChanged);
            processID = encryption();

            //await Bot
            telegramAPIAsync();
            //setTelegramEvent();      //위에 만든 소스에 이어서 추가해주세요.

            //Console.WriteLine(U.GetMarkets());

            // 당일 체결 내역 조회
            //Console.WriteLine(U.GetTicks("KRW-XRP", count: 2));

            // 현재가 정보 조회
            //Console.WriteLine(U.GetTicker("KRW-XRP"));

            // 시세 호가 정보(Orderbook) 조회
            //Console.WriteLine(U.GetOrderbook("KRW-BTC,KRW-ETH"));

            /*
            #region 자산
            // 자산 조회
            Console.WriteLine(U.GetAccount());
            #endregion

            #region 주문
            // 주문 가능 정보
            Console.WriteLine(U.GetOrderChance("KRW-BTC"));

            // 개별 주문 조회
            Console.WriteLine(U.GetOrder("주문 uuid"));

            // 주문 리스트 조회
            Console.WriteLine(U.GetAllOrder());

            // 주문하기
            Console.WriteLine(U.MakeOrder("KRW-BTC", UpbitAPI.UpbitOrderSide.bid, 0.001m, 5000000));

            // 주문 취소
            Console.WriteLine(U.CancelOrder("주문 uuid"));
            #endregion

            #region 시세 정보
            // 마켓 코드 조회
            Console.WriteLine(U.GetMarkets());

            // 캔들(분, 일, 주, 월) 조회
            Console.WriteLine(U.GetCandles_Minute("KRW-BTC", UpbitAPI.UpbitMinuteCandleType._1, to: DateTime.Now.AddMinutes(-2), count: 2));
            Console.WriteLine(U.GetCandles_Day("KRW-BTC", to: DateTime.Now.AddDays(-2), count: 2));
            Console.WriteLine(U.GetCandles_Week("KRW-BTC", to: DateTime.Now.AddDays(-14), count: 2));
            Console.WriteLine(U.GetCandles_Month("KRW-BTC", to: DateTime.Now.AddMonths(-2), count: 2));

            // 당일 체결 내역 조회
            Console.WriteLine(U.GetTicks("KRW-BTC", count: 2));

            // 현재가 정보 조회
            Console.WriteLine(U.GetTicker("KRW-BTC,KRW-ETH"));

            // 시세 호가 정보(Orderbook) 조회
            Console.WriteLine(U.GetOrderbook("KRW-BTC,KRW-ETH"));
            #endregion

            Console.ReadLine();
            */
        }

        //public decimal IntRound(decimal Value, int Digit)
        //{
        //    decimal Temp = Math.Pow(10.0, Digit);
        //    return Math.Round(Value * Temp) / Temp;
        //}






        void listBox1ADD(string str1)
        {
            Invoke((MethodInvoker)(() => listBox1.Items.Add(str1)));

            if (!isHover)
            {
                Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isHover)
                Clipboard.SetText(listBox1.GetItemText(listBox1.SelectedItem));
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isrun)
            {
                _switchThread.Abort();
                isrun = false;
            }

            Properties.Settings.Default.MarketSetting = textBox10.Text;
            Properties.Settings.Default.PMin = textBox15.Text;
            Properties.Settings.Default.PMax = textBox16.Text;
            Properties.Settings.Default.Save();
        }



        bool isHover = false;


        private void listBox1_MouseHover(object sender, EventArgs e)
        {
            isHover = true;
        }

        private void listBox1_MouseLeave(object sender, EventArgs e)
        {
            isHover = false;
        }

        private void mainView_MouseHover(object sender, EventArgs e)
        {
            isHover = true;
        }

        private void mainView_MouseLeave(object sender, EventArgs e)
        {
            isHover = false;
        }


        public void LogWrite(string message, string filename)
        {
            using (StreamWriter sw = File.AppendText(filename))
            {
                sw.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] {message}");
            }
        }


        Thread _switchThread;
        Thread _switchThread2;
        bool isrun = false;
        bool isrun2 = false;

        private void StartBtn_Click(object sender, EventArgs e)
        {
            if (!isrun)
            {
                _switchThread = new Thread(Start);
                _switchThread.Start();
                isrun = true;
                Setting.Enabled = false;
                StartBtn.Enabled = false;
                StopBtn.Enabled = true;
                bttime = Environment.TickCount;
            }


        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            if (isrun)
            {
                _switchThread.Abort();
                isrun = false;
                Setting.Enabled = true;
                StartBtn.Enabled = true;
                StopBtn.Enabled = false;
                Text = "매수 주문 취소 중";
                BuyAllCancel();
                Text = "매수 주문 취소 완료";
            }
        }


        private void button15_Click(object sender, EventArgs e)
        {
            if (!isrun2)
            {
                _switchThread2 = new Thread(StartNewVersion);
                _switchThread2.Start();
                isrun2 = true;
                bttime = Environment.TickCount;
                button15.Enabled = false;
            }
        }





        public void StartNewVersion()
        {
            foreach (var XX in OfferListN)
            {
                bool isOffer = true;
                while (isOffer)
                {
                    if (XX.buyUUID == null)
                    {
                        var buylist = MakeOrderB(XX.market, "", XX.Tprice.ToString(), UpbitOrderType.price);
                        if (buylist != null)
                        {
                            XX.buyUUID = buylist.uuid;
                            Invoke((MethodInvoker)(() => dataGridView1.CurrentCell = dataGridView1.Rows[XX.index].Cells[0]));
                            Invoke((MethodInvoker)(() => dataGridView1.ClearSelection()));
                            Invoke((MethodInvoker)(() => dataGridView1.Rows[XX.index].DefaultCellStyle.BackColor = Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(150)))), ((int)(((byte)(130)))))));
                            //Invoke((MethodInvoker)(() => dataGridView1.Rows[XX.index].Cells[3].Value = "매수 주문"));
                        }
                    }


                    if (XX.buyUUID != null && !XX.isbuy)
                    {
                        var buylist = GetOrder(U, XX.buyUUID);
                        if (buylist != null && buylist.state.Equals("cancel"))
                        {
                            Console.WriteLine(XX.market + ": trade Cnt: " + buylist.trades.Count);

                            decimal vol = 0; //체결 수량
                            decimal pri = 0; //체결 단가
                            foreach (var trade in buylist.trades)
                            {
                                pri = ((pri * vol) + (Convert.ToDecimal(trade.price) * Convert.ToDecimal(trade.volume))) / (vol + Convert.ToDecimal(trade.volume));
                                vol += Convert.ToDecimal(trade.volume);

                                XX.volume = vol;
                                //Console.WriteLine($"{unit2check(pri)}");
                            }

                            int res = (int)Math.Ceiling(Convert.ToDecimal(buylist.price) + Convert.ToDecimal(buylist.reserved_fee));


                            Invoke((MethodInvoker)(() => dataGridView1.Rows[XX.index].Cells[3].Value = String.Format("{0:#,0}", unit2checkR(pri)) + $"→ {res}"));
                            Invoke((MethodInvoker)(() => dataGridView1.Rows[XX.index].Cells[5].Value = "매수 완료"));
                            XX.Sprice = (decimal)unit2checkR(pri + (pri * XX.per / 100));
                            XX.isbuy = true;
                        }
                    }

                    if (XX.isbuy)
                    {
                        var selllist = MakeOrderS(XX.market, Convert.ToDecimal(XX.volume), XX.Sprice.ToString(), UpbitOrderType.limit);
                        if (selllist != null)
                        {
                            int One = (int)Math.Ceiling(XX.volume * XX.Sprice);
                            int res = One - (int)Math.Ceiling(One * 0.05 / 100);

                            Invoke((MethodInvoker)(() => dataGridView1.Rows[XX.index].Cells[4].Value = String.Format("{0:#,0}", XX.Sprice) + $"→ {One}→ {res}"));
                            //Invoke((MethodInvoker)(() => dataGridView1.Rows[XX.index].Cells[4].Value = XX.Sprice));
                            Invoke((MethodInvoker)(() => dataGridView1.Rows[XX.index].Cells[5].Value = "매도 주문"));
                            Invoke((MethodInvoker)(() => dataGridView1.Rows[XX.index].DefaultCellStyle.BackColor = Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))))));
                            //Invoke((MethodInvoker)(() => dataGridView1.BackColor = Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))))));
                            isOffer = false;
                            break;
                        }
                    }
                }
            }

            isrun2 = false;


        }





        public void BuyAllCancel()
        {
            for (int i = 0; i < OfferList.Count; i++)
            {
                Application.DoEvents();
                if (OfferList[i].isbuying && OfferList[i].isOfferB)
                {
                    var Cancel = CancelOrder(OfferList[i].BUUID);
                    OfferList[i].isbuying = false;
                    OfferList[i].isOfferB = false;
                    if (!isHover)
                    {
                        Invoke((MethodInvoker)(() => mainView.CurrentCell = mainView.Rows[OfferList[i].index].Cells[0]));
                        Invoke((MethodInvoker)(() => mainView.ClearSelection()));
                    }

                    Invoke((MethodInvoker)(() => mainView.Rows[OfferList[i].index].DefaultCellStyle.BackColor = SystemColors.Window));
                    Invoke((MethodInvoker)(() => toolStripStatusLabel3.BackColor = SystemColors.Window));
                    Invoke((MethodInvoker)(() => mainView.Rows[OfferList[i].index].Cells[4].Value = "주문 취소"));
                }
                Thread.Sleep(32);
            }
        }




        private void Setting_Click(object sender, EventArgs e)
        {
            //unit2check(decimal Value)
            OfferList.Clear();
            mainView.Rows.Clear();

            decimal StartP = Convert.ToDecimal(startPrice.Text);
            decimal EndP = Convert.ToDecimal(endPrice.Text);


            decimal Quantitye;
            decimal SQuantitye = 0;
            int price = Convert.ToInt32(Mony.Text); //호가 금액
            int totalPrice = 0;

            decimal buying = StartP;
            decimal selling;

            int listCnt = 0;


            while (buying < EndP)
            {
                Application.DoEvents();

                selling = unit2checkR(buying + (buying * Convert.ToDecimal(per.Text) / 100));
                if (buying == selling)
                {
                    MessageBox.Show("[" + per.Text + "%] 수치를 재설정 해 주세요.", "호가변화% 설정 에러");
                    break;
                }

                Quantitye = Math.Round((price - (price * 0.05m / 100)) / buying, 8);



                totalPrice += price;
                label28.Text = "리스트 총 금액: " + totalPrice + " KRW";

                decimal res = buying * Quantitye;
                decimal res2 = (selling * Quantitye) - ((selling * Quantitye) * 0.05m / 100);
                SQuantitye = Quantitye;

                if (checkBox4.Checked)
                {
                    SQuantitye = Math.Round((price + 1 + (price * 0.05m / 100)) / selling, 8);
                    res = buying * SQuantitye;
                    res2 = (selling * SQuantitye) - ((selling * SQuantitye) * 0.05m / 100);
                    //Console.WriteLine(SQuantitye);
                }
                decimal res3 = res2 - price;

                int view = (int)res2;
                int view2 = (int)res3;

                string[] addstr = { buying.ToString(), selling.ToString(), Convert.ToDecimal(Quantitye).ToString(), price.ToString(), "대기", view.ToString(), view2.ToString() + "↑ (0)" };
                OfferList.Add(new Offer(listCnt, buying, selling, Convert.ToDecimal(Quantitye), "", "", price, false, false, false, false, 0, view, view2, 10000000, 0, SQuantitye));
                mainView.Rows.Add(addstr);
                buying = selling;
                listCnt++;
                listNum.Text = "리스트수: " + listCnt;
                //Invoke((MethodInvoker)(() => mainView.CurrentCell = mainView.Rows[mainView.Rows.Count - 1].Cells[0]));
                Invoke((MethodInvoker)(() => mainView.ClearSelection()));
            }
        }


        int Maxprice = 0;

        public void Start()
        {
            string CoinName = textBox10.Text;


            decimal price = 0;

            int buyprice = 0;
            int contPrice = 0;
            decimal res = 0;

            int TotalB = 0;
            int TotalS = 0;
            Console.Clear();

            int sellCnt = 0;
            int buyCnt = 0;

            decimal accVol = 0;

            while (true)
            {
                try
                {
                    var Account = GetAccount();
                    if (Account != null)
                    {
                        foreach (var XX in Account)
                        {
                            if (XX.currency.Equals("KRW"))
                            {
                                Maxprice = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(XX.balance)));
                                Invoke((MethodInvoker)(() => textBox5.Text = Maxprice.ToString()));
                                break;
                            }
                        }
                    }

                    var Parse = GetTicker(U, CoinName);
                    if (Parse != null)
                    {
                        price = Convert.ToDecimal(Parse[0].trade_price);
                        Invoke((MethodInvoker)(() => this.Text = CoinName + " " + DateTime.Now.ToString("HH:mm:ss") + " / " + price + "KRW " + processID));
                    }
                    MACS MVCheck = MACK(CoinName); //이동평균선 조회


                    var Orderlist = GetAllOrder(U);
                    if (Orderlist == null)
                    {
                        continue;
                    }

                    foreach (Offer eventlist in OfferList)
                    {
                        int index = 9999;
                        
                        if (eventlist.isbuying && eventlist.isOfferB)
                        {

                            if (checkBox1.Checked)
                            {
                                if (eventlist.CancelPrice <= price)
                                {
                                    var Cancel = CancelOrder(eventlist.BUUID);
                                    if (Cancel != null)
                                    {
                                        eventlist.isbuying = false;
                                        eventlist.isOfferB = false;
                                        buyprice -= eventlist.price;
                                        
                                        Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].DefaultCellStyle.BackColor = SystemColors.Window));
                                        Invoke((MethodInvoker)(() => toolStripStatusLabel3.BackColor = SystemColors.Control));
                                        Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].Cells[4].Value = "주문 취소"));
                                        Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].Cells[0].Value = eventlist.buying));
                                        Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 192)));
                                        Invoke((MethodInvoker)(() => toolStripStatusLabel3.BackColor = Color.FromArgb(255, 255, 192)));
                                        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} [주문 취소], buyPrice: {eventlist.buying}, trade_price: {price}, Cancel: {eventlist.CancelPrice}");
                                        continue;
                                    }
                                }
                            }

                            index = Orderlist.FindIndex(item => item.uuid.Equals(eventlist.BUUID));
                            if (index == -1)
                            {
                                var buylist = GetOrder(U, eventlist.BUUID);
                                if (buylist != null)
                                {
                                    if (buylist.state.Equals("done"))
                                    {
                                        //매도
                                        if (V3sell(eventlist.index, eventlist.selling, eventlist.Svolume))
                                        {
                                            Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].Cells[4].Value = "매수완료"));
                                            listBox1ADD(DateTime.Now.ToString("HH:mm:ss") + " [매수 완료] -> " + eventlist.BUUID);
                                            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} [매수 완료] -> {eventlist.BUUID}");

                                            int tfund = 0;
                                            foreach (var pp in buylist.trades)
                                            {
                                                tfund += (int)Math.Round(Convert.ToDecimal(pp.funds));
                                            }

                                            eventlist.tbuy = ((int)Math.Round(Convert.ToDecimal(buylist.paid_fee)) + tfund);
                                            eventlist.isOfferB = false;

                                            contPrice += eventlist.price;
                                            TotalB += eventlist.tbuy;
                                            buyCnt++;

                                            Invoke((MethodInvoker)(() => label23.Text = "총 매수: " + TotalB + " KRW"));
                                            Invoke((MethodInvoker)(() => label30.Text = "매수 횟수: " + buyCnt));
                                            Invoke((MethodInvoker)(() => label31.Text = $"체결: {contPrice} KRW"));


                                            eventlist.isselling = true;

                                            if (!isHover)
                                            {
                                                Invoke((MethodInvoker)(() => mainView.CurrentCell = mainView.Rows[eventlist.index].Cells[0]));
                                                Invoke((MethodInvoker)(() => mainView.ClearSelection()));
                                            }

                                            Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].DefaultCellStyle.BackColor = Color.FromArgb(130, 150, 250)));
                                            Invoke((MethodInvoker)(() => toolStripStatusLabel3.BackColor = Color.FromArgb(130, 150, 250)));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].Cells[4].Value = "매수대기"));
                            }
                        }
                        
                        
                        if (eventlist.isselling && eventlist.isOfferS)
                        {
                            index = Orderlist.FindIndex(item => item.uuid.Equals(eventlist.SUUID));
                            if (index == -1)
                            {
                                var selllist = GetOrder(U, eventlist.SUUID);
                                if (selllist != null)
                                {
                                    if (selllist.state.Equals("done"))
                                    {
                                        Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].Cells[4].Value = "매도완료"));
                                        listBox1ADD(DateTime.Now.ToString("HH:mm:ss") + " [매도 완료] -> " + eventlist.SUUID);
                                        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} [매도 완료] -> {eventlist.SUUID}");

                                        eventlist.isbuying = false;
                                        eventlist.isselling = false;
                                        eventlist.isOfferB = false;
                                        eventlist.isOfferS = false;
                                        buyprice -= eventlist.price;
                                        contPrice -= eventlist.price;
                                        Invoke((MethodInvoker)(() => label31.Text = $"체결: {contPrice} KRW"));

                                        if (checkBox4.Checked)
                                        {
                                            accVol += (eventlist.volume - eventlist.Svolume);
                                            Console.WriteLine($"[매집] 누적: ≒ {accVol} {textBox10.Text.Split('-')[1]}");
                                        }

                                        int tfund = 0;
                                        foreach (var pp in selllist.trades)
                                        {
                                            tfund += (int)Math.Round(Convert.ToDecimal(pp.funds));
                                        }

                                        int Rsell = (tfund - (int)Math.Round(Convert.ToDecimal(selllist.paid_fee)));
                                        TotalS += eventlist.tbuy;
                                        eventlist.trade += 1;

                                        Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].Cells[6].Value = $"{eventlist.up}↑ ({eventlist.trade}) "));

                                        Invoke((MethodInvoker)(() => label24.Text = "총 매도: " + TotalS + " KRW"));
                                        res = res + Rsell - eventlist.tbuy;
                                        sellCnt++;
                                        Invoke((MethodInvoker)(() => label27.Text = "매도 횟수: " + sellCnt));
                                        Invoke((MethodInvoker)(() => label22.Text = "수익: " + res + " KRW"));

                                        if (!isHover)
                                        {
                                            Invoke((MethodInvoker)(() => mainView.CurrentCell = mainView.Rows[eventlist.index].Cells[0]));
                                            Invoke((MethodInvoker)(() => mainView.ClearSelection()));
                                        }
                                        Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].DefaultCellStyle.BackColor = Color.FromArgb(150, 250, 130)));
                                        Invoke((MethodInvoker)(() => toolStripStatusLabel3.BackColor = Color.FromArgb(150, 250, 130)));
                                        Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].Cells[0].Value = eventlist.buying));
                                    }
                                }
                            }
                            else
                            {
                                Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].Cells[4].Value = "매도대기"));
                            }
                        }





                        bool PriceCheck = false;

                        if (eventlist.index < OfferList.Count - 1)
                        {
                            if (eventlist.buying <= price && OfferList[eventlist.index + 1].buying > price)
                            {
                                PriceCheck = true;
                            }
                        }
                        else
                        {
                            if (eventlist.buying <= price)
                                PriceCheck = true;
                        }


                        if ((!eventlist.isbuying) && MVCheck.isbool && PriceCheck)
                        {
                            //매수주문
                            if (Maxprice >= eventlist.price)
                            {
                                if (V3buy(eventlist.index, eventlist.buying, eventlist.volume))
                                {
                                    eventlist.isbuying = true;
                                    buyprice += eventlist.price;
                                    Invoke((MethodInvoker)(() => label29.Text = $"매수: {buyprice} KRW"));

                                    Invoke((MethodInvoker)(() => mainView.CurrentCell = mainView.Rows[eventlist.index].Cells[0]));
                                    Invoke((MethodInvoker)(() => mainView.ClearSelection()));
                                    Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].DefaultCellStyle.BackColor = Color.FromArgb(250, 150, 130)));
                                    Invoke((MethodInvoker)(() => toolStripStatusLabel3.BackColor = Color.FromArgb(250, 150, 130)));

                                    int cancel = 0;
                                    Invoke((MethodInvoker)(() => cancel = (int)numericUpDown1.Value - 1));

                                    var Getorderbook = GetOrderBook();
                                    if (Getorderbook != null)
                                    {
                                        eventlist.CancelPrice = Convert.ToDecimal(Getorderbook[0].orderbook_units[cancel].ask_price);
                                        Invoke((MethodInvoker)(() => mainView.Rows[eventlist.index].Cells[0].Value = eventlist.buying + $" [{eventlist.CancelPrice}]"));
                                    }
                                    Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} [주문 확인 0-0], Cancel: {eventlist.CancelPrice}");

                                }
                            }
                        }

                        Invoke((MethodInvoker)(() => toolStripStatusLabel3.Text = $"[{DateTime.Now.ToString("HH:mm:ss")}] {MVCheck.temp1} / {MVCheck.temp2} / {MVCheck.temp3} / {MVCheck.isbool}"));
                        Invoke((MethodInvoker)(() => label21.Text = "매수 가능 금액: " + (Maxprice - eventlist.price) + "KRW"));
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine("[Main]\n" + ex.Message);
                }
            }
        }



        bool V3buy(int index, decimal price, decimal volume)
        {
            var buylist = MakeOrderB(textBox10.Text, volume.ToString(), price.ToString(), UpbitOrderType.limit);
            if (buylist != null)
            {
                OfferList[index].BUUID = buylist.uuid;
                OfferList[index].isOfferB = true;
                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} [매수 주문] {price}, {volume}, {buylist.uuid}");
                return true;
            }
            return false;
        }


        bool V3sell(int index, decimal price, decimal volume)
        {
            var selllist = MakeOrderS(textBox10.Text, volume, price.ToString(), UpbitOrderType.limit);
            if (selllist != null)
            {
                OfferList[index].SUUID = selllist.uuid;
                OfferList[index].isOfferS = true;
                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} [매도 주문] {price}, {volume}, {selllist.uuid}");
                return true;
            }
            return false;
        }





        public void NowPrice(string market, TextBox _tb)
        {
            var coinList = GetTicker(U, market);
            List<string> coins = new List<string>();
            if (coinList != null)
            {
                decimal Getprice = Convert.ToDecimal(coinList[0].trade_price);
                _tb.Text = Getprice.ToString();
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            NowPrice(textBox10.Text, textBox11);
        }





        private void mainView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {

                OfferList[e.RowIndex].selling = Convert.ToDecimal(mainView.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString());
                decimal price = Convert.ToDecimal(OfferList[e.RowIndex].price);
                decimal buying = Convert.ToDecimal(OfferList[e.RowIndex].buying);


                decimal selling = Convert.ToDecimal(OfferList[e.RowIndex].selling);


                decimal Quantitye = OfferList[e.RowIndex].volume;

                decimal res2 = (selling * Quantitye) - ((selling * Quantitye) * 0.05m / 100);
                decimal res3 = res2 - price;
                int predict = (int)res2;
                int up = (int)res3;

                OfferList[e.RowIndex].predict = predict;
                OfferList[e.RowIndex].up = up;

                mainView.Rows[e.RowIndex].Cells[2].Value = OfferList[e.RowIndex].volume;
                mainView.Rows[e.RowIndex].Cells[5].Value = predict;
                mainView.Rows[e.RowIndex].Cells[6].Value = up + "↑ (0)";


            }
            else if (e.ColumnIndex == 3) //호가금액
            {
                OfferList[e.RowIndex].price = Convert.ToInt32(mainView.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString());
                decimal price = Convert.ToDecimal(OfferList[e.RowIndex].price);
                decimal buying = Convert.ToDecimal(OfferList[e.RowIndex].buying);


                decimal selling = Convert.ToDecimal(OfferList[e.RowIndex].selling);


                OfferList[e.RowIndex].volume = Convert.ToDecimal(Math.Round((price - (price * 0.05m / 100)) / buying, 4));

                decimal Quantitye = OfferList[e.RowIndex].volume;

                decimal res2 = (selling * Quantitye) - ((selling * Quantitye) * 0.05m / 100);
                decimal res3 = res2 - price;
                int predict = (int)res2;
                int up = (int)res3;



                OfferList[e.RowIndex].predict = predict;
                OfferList[e.RowIndex].up = up;

                mainView.Rows[e.RowIndex].Cells[2].Value = OfferList[e.RowIndex].volume;
                mainView.Rows[e.RowIndex].Cells[5].Value = predict;
                mainView.Rows[e.RowIndex].Cells[6].Value = up + "↑ (0)";

                var result = (from priceSum in OfferList select priceSum).Sum(priceSum => priceSum.price);
                label28.Text = "리스트 총 금액: " + result + " KRW";

                //foreach (Offer eventlist in OfferList)
                //    Console.WriteLine(eventlist.volume);
            }
        }

        private void Datagrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            OfferListN[e.RowIndex].market = dataGridView1.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
            OfferListN[e.RowIndex].per = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells[1].FormattedValue.ToString());
            OfferListN[e.RowIndex].Tprice = Convert.ToInt32(Regex.Replace(dataGridView1.Rows[e.RowIndex].Cells[2].FormattedValue.ToString(), @"[^0-9]", ""));


        }




        private void textBox12_DoubleClick(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = "";
        }



        private void button6_Click_1(object sender, EventArgs e)
        {
            var Parse = GetAccount();
            if (Parse != null)
            {
                this.Text = $"보유 KRW: {Parse[0].balance} KRW";
            }
            else
            {
                MessageBox.Show("API키 오류");
            }
        }

        List<string> markets = new List<string>();
        AutoCompleteStringCollection sourceM = new AutoCompleteStringCollection();
        private void Form1_Load(object sender, EventArgs e)
        {

            textBox12.Text = Properties.Settings.Default.AccesskeySetting;
            textBox13.Text = Properties.Settings.Default.SecretkeySetting;
            textBox10.Text = Properties.Settings.Default.MarketSetting;
            textBox15.Text = Properties.Settings.Default.PMin;
            textBox16.Text = Properties.Settings.Default.PMax;


            Accesskey = textBox12.Text;
            Secretkey = textBox13.Text;


            if (!textBox12.Text.Equals("") && !textBox12.Text.Equals(""))
            {
                U = new UpbitAPI(Properties.Settings.Default.AccesskeySetting, Properties.Settings.Default.SecretkeySetting);
                button6.Enabled = true;
            }
            else
            {
                MessageBox.Show("Setting 항목에서 Api를 등록해주세요.");
                button6.Enabled = false;
            }

            string CoinName = textBox10.Text;
            var coinList = GetTicker(U, CoinName);
            List<string> coins = new List<string>();
            if (coinList != null)
            {
                decimal Getprice = Convert.ToDecimal(coinList[0].trade_price);
                textBox11.Text = Getprice.ToString();

                startPrice.Text = unit2checkR(Getprice - (Getprice * Convert.ToDecimal(textBox15.Text) / 100)).ToString();
                endPrice.Text = unit2checkR(Getprice + (Getprice * Convert.ToDecimal(textBox16.Text) / 100)).ToString();
            }



            sourceM.AddRange(new string[] { });

            var aa = GetMarketList(U);
            if (aa != null)
            {
                foreach (var xx in aa)
                {
                    if (xx.market.Contains("KRW"))
                    {
                        sourceM.Add(xx.market);
                        markets.Add(xx.market);
                    }
                }

                textBox2.AutoCompleteCustomSource = sourceM;
                Tmarket.AutoCompleteCustomSource = sourceM;
                textBox8.AutoCompleteCustomSource = sourceM;
            }

            //Console.WriteLine(U.GetTicker("KRW-MANA"));

            //Console.WriteLine(U.GetCandles_Minute("KRW-ZIL", UpbitAPI.UpbitMinuteCandleType._3, to: DateTime.Now, count: 10));

        }


        private void button7_Click_1(object sender, EventArgs e)
        {
            Console.WriteLine(U.GetOrder(textBox14.Text));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string CoinName = textBox10.Text;
            var coinList = GetTicker(U, CoinName);
            List<string> coins = new List<string>();
            if (coinList != null)
            {
                decimal Getprice = Convert.ToDecimal(coinList[0].trade_price);
                textBox11.Text = Getprice.ToString();

                startPrice.Text = unit2checkR(Getprice - (Getprice * Convert.ToDecimal(textBox15.Text) / 100)).ToString();


                endPrice.Text = unit2checkR(Getprice + (Getprice * Convert.ToDecimal(textBox16.Text) / 100)).ToString();
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            //1 x 2 / 100 = 0.02
            try
            {
                decimal val1 = Convert.ToDecimal(textBox100.Text);
                decimal val2 = Convert.ToDecimal(textBox101.Text);
                textBox102.Text = (val1 * val2 / 100).ToString();
            }
            catch (Exception ex)
            {
                textBox102.Text = "NaN";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //2 x 100 / 1 = 200
            try
            {
                decimal val1 = Convert.ToDecimal(textBox103.Text);
                decimal val2 = Convert.ToDecimal(textBox104.Text);
                textBox105.Text = (val2 * 100 / val1).ToString();
            }
            catch (Exception ex)
            {
                textBox105.Text = "NaN";
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //(2 - 1) / 1 x 100 = 100
            try
            {
                decimal val1 = Convert.ToDecimal(textBox106.Text);
                decimal val2 = Convert.ToDecimal(textBox107.Text);
                textBox108.Text = ((val2 - val1) / val1 * 100).ToString();
            }
            catch (Exception ex)
            {
                textBox108.Text = "NaN";
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            //1 - (1 x 2 / 100) = 0.98
            try
            {
                decimal val1 = Convert.ToDecimal(textBox109.Text);
                decimal val2 = Convert.ToDecimal(textBox110.Text);
                if (comboBox1.Text.Equals("증가"))
                {
                    textBox111.Text = (val1 + (val1 * val2 / 100)).ToString();
                }
                else
                {
                    textBox111.Text = (val1 - (val1 * val2 / 100)).ToString();
                }
            }
            catch (Exception ex)
            {
                textBox111.Text = "NaN";
            }

        }

        private void login()
        {
            if (textBox1.Text.Equals("!Rhwkdwod2@"))
            {
                panel3.Hide();
            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            login();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button9.Focus();
                login();
            }

        }

        private void textboxRemove(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void 초당5회ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Checked)
            {
                초당1회ToolStripMenuItem.Checked = false;
                초당2회ToolStripMenuItem.Checked = false;
                초당3회ToolStripMenuItem.Checked = false;
                초당4회ToolStripMenuItem.Checked = false;
                초당5회ToolStripMenuItem.Checked = false;

                switch (((ToolStripMenuItem)sender).Text)
                {
                    case "초당 1회":
                        limteTime = 1000;
                        break;
                    case "초당 2회":
                        limteTime = 500;
                        break;
                    case "초당 3회":
                        limteTime = 333;
                        break;
                    case "초당 4회":
                        limteTime = 250;
                        break;
                    case "초당 5회":
                        limteTime = 100;
                        break;
                    default:
                        limteTime = 333;
                        break;
                }
                ((ToolStripMenuItem)sender).Checked = true;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OfferListN.Clear();
            dataGridView1.Rows.Clear();
        }


        private void market_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox SelectTextBox = (TextBox)sender;
                SelectTextBox.Text = SelectTextBox.Text.ToUpper();
                SelectTextBox.Select(SelectTextBox.Text.Length, 0);
                return;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            NowPrice(Tmarket.Text, TtextBox1);
        }

        Thread _tracking;
        bool _trackingRun = false;
        private void Tbutton1_Click(object sender, EventArgs e)
        {
            if (!_trackingRun)
            {
                _trackingRun = true;
                _tracking = new Thread(Tracking);
                _tracking.Start();
                Tbutton1.Enabled = false;
                Tbutton2.Enabled = true;
            }

        }


        private void Tbutton2_Click(object sender, EventArgs e)
        {
            if (_trackingRun)
            {
                _trackingRun = false;
                _tracking.Abort();
                Tbutton1.Enabled = true;
                Tbutton2.Enabled = false;
            }
        }



        public decimal NowPrice(string market)
        {
            var coinList = GetTicker(U, market);
            if (coinList != null)
            {
                decimal Getprice = Convert.ToDecimal(coinList[0].trade_price);
                decimal Open = Convert.ToDecimal(coinList[0].opening_price);
                return Getprice;
            }
            return 0;
        }


        public struct MACS
        {
            public bool isbool;
            public decimal temp1;
            public decimal temp2;
            public decimal temp3;
        }

        public MACS MACK(string market)
        {
            //await Bot.SendTextMessageAsync(1331550349, $"{Tmarket.Text}\n매수: {dataGridView2.Rows[XX.index].Cells[2].Value} ({(int)XX.buyPrice})\n매도: {dataGridView2.Rows[XX.index].Cells[3].Value} ({(int)sellPrice})\n수익: {(int)(sellPrice - XX.buyPrice)}");
            MACS structMACK = new MACS();
            structMACK.isbool = false;
            if (!checkBox3.Checked)
            {
                structMACK.temp1 = 0;
                structMACK.temp2 = 0;
                structMACK.temp3 = 0;
                structMACK.isbool = true;
                return structMACK;
            }

            int ma1 = Convert.ToInt32(textBox19.Text);
            int ma2 = Convert.ToInt32(textBox20.Text);
            int ma3 = Convert.ToInt32(textBox21.Text);

            decimal temp1 = 0;
            decimal temp2 = 0;
            decimal temp3 = 0;
            string ivkStr = null;
            Invoke((MethodInvoker)(() => ivkStr = comboBox3.Text));
            UpbitAPI.UpbitMinuteCandleType tp;
            switch (ivkStr)
            {
                case "3분":
                    tp = UpbitMinuteCandleType._3;
                    break;
                case "5분":
                    tp = UpbitMinuteCandleType._5;
                    break;
                case "1분":
                default:
                    tp = UpbitMinuteCandleType._1;
                    break;
            }

            var Candles = GetCandles_Minute(market, tp, default, count: ma3);
            if (Candles != null)
            {

                for (int i = 0; i < ma3; i++)
                {
                    if (i < ma1) temp1 += Convert.ToDecimal(Candles[i].trade_price);
                    if (i < ma2) temp2 += Convert.ToDecimal(Candles[i].trade_price);
                    if (i < ma3) temp3 += Convert.ToDecimal(Candles[i].trade_price);
                }
                structMACK.temp1 = Math.Round(temp1 / ma1, 4);
                structMACK.temp2 = Math.Round(temp2 / ma2, 4);
                structMACK.temp3 = Math.Round(temp3 / ma3, 4);

            }

            //Console.WriteLine($"{temp1}, {temp2}");

            if ((structMACK.temp1 > structMACK.temp2) && (structMACK.temp2 > structMACK.temp3))
            {
                structMACK.isbool = true;
            }

            return structMACK;
        }

        public async void Tracking()
        {
            Console.Clear();
            TMode.Clear();
            decimal regPrice = Convert.ToDecimal(TtextBox1.Text);
            decimal fallPer = Convert.ToDecimal(TtextBox3.Text);
            decimal risePer = Convert.ToDecimal(TtextBox4.Text);
            decimal TargetPer = Convert.ToDecimal(TtextBox5.Text);

            decimal Min = unit2check(regPrice - (regPrice * fallPer / 100));
            decimal Max = unit2check(Min + (Min * risePer / 100));
            bool PointCheck = false;

            int index = 0;
            string buyUUID = null;
            bool isbuy = false;
            decimal volume = 0;
            decimal Sprice = 0;
            //TMode.Add(new Mode.TModeList(index, Tmarket.Text, false, ""));

            int UUIDcnt = 0;

            decimal buyPrice = 0;
            decimal sellPrice = 0;

            decimal Tbuy = 0;
            decimal Tsell = 0;
            decimal Tres = 0;

            List<decimal> log = new List<decimal>();
            int ListCnt = 0;

            decimal vol = 0; //체결 수량
            decimal pri = 0; //체결 단가

            string[] addData = { Tmarket.Text, regPrice.ToString(), "-", "-", "대기" };
            Invoke((MethodInvoker)(() => dataGridView2.Rows.Clear()));
            Invoke((MethodInvoker)(() => dataGridView2.Rows.Add(addData)));
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] New - Reg: {regPrice}, Min: {Min}, Max: {Max}");
            try
            {
                while (_trackingRun)
                {
                    decimal Price = Convert.ToDecimal(TtextBox2.Text);

                    decimal nowPrice = NowPrice(Tmarket.Text);
                    bool MVCheck = MACK(Tmarket.Text).isbool;
                    if (nowPrice == 0) continue;
                    Invoke((MethodInvoker)(() => this.Text = Tmarket.Text + " " + DateTime.Now.ToString("HH:mm:ss") + " " + nowPrice + " KRW " + processID));



                    if (regPrice < nowPrice && !PointCheck) //기준가 초과
                    {
                        Invoke((MethodInvoker)(() => dataGridView2.Rows[index].Cells[1].Value = $"{regPrice} → {nowPrice}"));
                        regPrice = nowPrice;
                        Min = unit2check(regPrice - (regPrice * fallPer / 100));
                        Max = unit2check(Min + (Min * risePer / 100));
                        Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] 기준가 초과 - Reg: {regPrice}, Min: {Min}, Max: {Max}");
                    }




                    //일정 호가 초과시 기준가 변경
                    if (Min >= nowPrice)
                    {
                        if (!PointCheck)
                        {
                            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] {Min}, {nowPrice}");
                        }

                        PointCheck = true;
                        if (Min > nowPrice)
                        {
                            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] nowPrice {nowPrice} : {Min}, {Max}");
                            decimal beforeMin = Min;

                            //매수점 이동
                            Min = nowPrice;
                            Max = unit2check(Min + (Min * risePer / 100));
                            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] 매수점 이동 {beforeMin} → {Min}, {Max}");
                            Invoke((MethodInvoker)(() => dataGridView2.Rows[index].Cells[4].Value = $"{beforeMin} → {Min}, {Max}"));
                        }
                    }




                    if (Max <= nowPrice && PointCheck && buyUUID == null && MVCheck)
                    {
                        //매수 주문
                        ListCnt = log.FindIndex(x => x == nowPrice);
                        if (ListCnt == -1)
                        {
                            var buylist = MakeOrderB(Tmarket.Text, "", Price.ToString(), UpbitOrderType.price);
                            if (buylist != null)
                            {
                                buyUUID = buylist.uuid;
                                listBox1ADD(DateTime.Now.ToString("HH:mm:ss") + " [매수 주문] -> " + buylist.uuid);

                                Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] 매수 주문: {nowPrice}");

                                Invoke((MethodInvoker)(() => dataGridView2.Rows[index].Cells[4].Value = "매수 주문"));
                                Invoke((MethodInvoker)(() => dataGridView2.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(250, 150, 130)));
                            }
                        }
                    }


                    if (buyUUID != null && !isbuy)
                    {
                        //매수 체결
                        var buylist = GetOrder(U, buyUUID);
                        if (buylist != null && (buylist.state.Equals("cancel") || buylist.state.Equals("done")))
                        {
                            listBox1ADD(DateTime.Now.ToString("HH:mm:ss") + " [매수 완료] -> " + buylist.uuid);
                            Console.WriteLine(Tmarket.Text + ": trade Cnt: " + buylist.trades.Count);
                            vol = 0;
                            pri = 0;
                            int tfund = 0;
                            foreach (var trade in buylist.trades)
                            {
                                pri = ((pri * vol) + (Convert.ToDecimal(trade.price) * Convert.ToDecimal(trade.volume))) / (vol + Convert.ToDecimal(trade.volume));
                                vol += Convert.ToDecimal(trade.volume);

                                volume = vol;
                                tfund += (int)Math.Round(Convert.ToDecimal(trade.funds));
                            }

                            Sprice = unit2check(pri + (pri * TargetPer / 100));
                            log.Add(unit2check(pri));
                            Invoke((MethodInvoker)(() => dataGridView2.Rows[index].Cells[2].Value = unit2check(pri)));

                            buyPrice = (Convert.ToDecimal(buylist.paid_fee) + tfund);
                            Tbuy += buyPrice;
                            isbuy = true;
                        }
                    }


                    if (isbuy)
                    {
                        var selllist = MakeOrderS(Tmarket.Text, volume, Sprice.ToString(), UpbitOrderType.limit);
                        if (selllist != null)
                        {
                            //매도 주문

                            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] 매도주문: {Sprice}");
                            Invoke((MethodInvoker)(() => dataGridView2.Rows[index].Cells[3].Value = Sprice));
                            Invoke((MethodInvoker)(() => dataGridView2.Rows[index].Cells[4].Value = $"매도 주문 {Min}, {Max}"));

                            //매매 정보 저장
                            TMode.Add(new Mode.TModeList(index, buyUUID, selllist.uuid, false, buyPrice, Sprice, pri));

                            Invoke((MethodInvoker)(() => dataGridView2.Rows[index].Cells[1].Value = $"{regPrice} → {nowPrice}"));
                            regPrice = nowPrice;
                            Min = unit2check(regPrice - (regPrice * fallPer / 100));
                            Max = unit2check(Min + (Min * risePer / 100));
                            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] New - Reg: {regPrice}, Min: {Min}, Max: {Max}");

                            //Invoke((MethodInvoker)(() => toolStripStatusLabel3.Text = $"[{DateTime.Now.ToString("HH:mm:ss")}] New - Min: {Min}, Max: {Max} {MVCheck}"));
                            Invoke((MethodInvoker)(() => dataGridView2.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(130, 150, 250)));

                            addData = new string[] { Tmarket.Text, regPrice.ToString(), "-", "-", "대기" };
                            Invoke((MethodInvoker)(() => dataGridView2.Rows.Add(addData)));

                            if (!isHover)
                            {
                                Invoke((MethodInvoker)(() => dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[0]));
                                Invoke((MethodInvoker)(() => dataGridView2.ClearSelection()));
                            }


                            PointCheck = false;
                            buyUUID = null;
                            isbuy = false;
                            volume = 0;
                            Sprice = 0;
                            index++;
                        }
                    }


                    //매도 정보 확인
                    //체결 확인이 될 경우 현재가를 기준가로 재설정
                    var Orderlist = GetAllOrder(U);
                    if (Orderlist != null)
                    {
                        foreach (var XX in TMode)
                        {
                            if (!XX.isSell)
                            {
                                UUIDcnt = Orderlist.FindIndex(item => item.uuid.Equals(XX.selluuid));
                                if (UUIDcnt == -1)
                                {
                                    var selllist = GetOrder(U, XX.selluuid);
                                    if (selllist != null)
                                    {
                                        if (selllist.state.Equals("done"))
                                        {
                                            listBox1ADD(DateTime.Now.ToString("HH:mm:ss") + " [매도 완료] -> " + XX.selluuid);

                                            XX.isSell = true;

                                            int tfund = 0;
                                            foreach (var pp in selllist.trades)
                                            {
                                                tfund += (int)Math.Round(Convert.ToDecimal(pp.funds));
                                            }

                                            sellPrice = (tfund - Convert.ToDecimal(selllist.paid_fee));
                                            Tsell += sellPrice;
                                            Tres += (int)(sellPrice - XX.buyPrice);

                                            log.Remove(XX.pri);

                                            //await Bot.SendTextMessageAsync(1331550349, $"{Tmarket.Text}\n매수: {dataGridView2.Rows[XX.index].Cells[2].Value} ({(int)XX.buyPrice})\n매도: {dataGridView2.Rows[XX.index].Cells[3].Value} ({(int)sellPrice})\n수익: {(int)(sellPrice - XX.buyPrice)}");
                                            Invoke((MethodInvoker)(() => dataGridView2.Rows[XX.index].Cells[2].Value = $"{dataGridView2.Rows[XX.index].Cells[2].Value} ({(int)XX.buyPrice})"));
                                            Invoke((MethodInvoker)(() => dataGridView2.Rows[XX.index].Cells[3].Value = $"{dataGridView2.Rows[XX.index].Cells[3].Value} ({(int)sellPrice})"));
                                            Invoke((MethodInvoker)(() => dataGridView2.Rows[XX.index].Cells[4].Value = $"수익: {(int)(sellPrice - XX.buyPrice)}"));
                                            Invoke((MethodInvoker)(() => dataGridView2.Rows[XX.index].DefaultCellStyle.BackColor = Color.FromArgb(150, 250, 130)));



                                            //매수점 이동
                                            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] 매도 체결 {nowPrice}");


                                            decimal beforeMin = Min;


                                            Invoke((MethodInvoker)(() => dataGridView2.Rows[XX.index].Cells[1].Value = $"{regPrice} → {nowPrice}"));
                                            regPrice = nowPrice;
                                            Min = unit2check(regPrice - (regPrice * fallPer / 100));
                                            Max = unit2check(Min + (Min * risePer / 100));
                                            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] New - Reg: {regPrice}, Min: {Min}, Max: {Max}");
                                            //Invoke((MethodInvoker)(() => toolStripStatusLabel3.Text = $"[{DateTime.Now.ToString("HH:mm:ss")}] 매수점 이동 {beforeMin} → {Min}, {Max} {MVCheck}"));
                                            PointCheck = false;

                                        }
                                    }
                                }
                            }

                        }
                    }

                    Invoke((MethodInvoker)(() => label40.Text = Min.ToString()));
                    Invoke((MethodInvoker)(() => label41.Text = Max + "  (" + decimal.Round(Price / Max, 8) + ")"));
                    Invoke((MethodInvoker)(() => label42.Text = String.Format("{0:#,0}", unit2check(Max + (Max * TargetPer / 100))) + " (≒" + String.Format("{0:#,0}", decimal.Round(decimal.Round(Price / Max, 8) * unit2check(Max + (Max * TargetPer / 100))) + ") KRW")));

                    Invoke((MethodInvoker)(() => label43.Text = $"총 매수: {(int)Tbuy} KRW"));
                    Invoke((MethodInvoker)(() => label45.Text = $"총 매도: {(int)Tsell} KRW"));
                    Invoke((MethodInvoker)(() => label44.Text = $"수익: {Tres} KRW"));

                    Invoke((MethodInvoker)(() => toolStripStatusLabel3.Text = $"[{DateTime.Now.ToString("HH:mm:ss")}] Min: {Min}, Max: {Max} / {PointCheck} / {MVCheck}"));
                }
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " 종료");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button13_Click_1(object sender, EventArgs e)
        {

            decimal Price = Convert.ToDecimal(TtextBox2.Text);
            decimal regPrice = Convert.ToDecimal(TtextBox1.Text);
            decimal fallPer = Convert.ToDecimal(TtextBox3.Text);
            decimal risePer = Convert.ToDecimal(TtextBox4.Text);
            decimal TargetPer = Convert.ToDecimal(TtextBox5.Text);


            decimal Min = unit2check(regPrice - (regPrice * fallPer / 100));
            decimal Max = unit2check(Min + (Min * risePer / 100));

            label40.Text = Min.ToString();
            label41.Text = Max + "  (" + decimal.Round(Price / Max, 8) + ")";
            label42.Text = String.Format("{0:#,0}", unit2check(Max + (Max * TargetPer / 100))) + " (≒" + String.Format("{0:#,0}", decimal.Round(decimal.Round(Price / Max, 8) * unit2check(Max + (Max * TargetPer / 100))) + ") KRW");
        }



        decimal unit2check(decimal num)
        {
            decimal res = 0;

            if (10 > num)
            {
                res = decimal.Ceiling(num * 100) / 100;
            }
            else if (100 > num)
            {
                res = decimal.Ceiling(num * 10) / 10;
            }
            else if (1000 > num)
            {
                res = decimal.Ceiling(num);
            }
            else if (10000 > num)
            {
                res = decimal.Ceiling(num * 2 * 0.1m) / 2 * 10;
            }
            else if (100000 > num)
            {
                res = decimal.Ceiling(num * 0.1m) * 10;
            }
            else if (500000 > num)
            {
                res = decimal.Ceiling(num * 2 * 0.01m) / 2 * 100;
            }
            else if (1000000 > num)
            {
                res = decimal.Ceiling(num * 0.01m) * 100;
            }
            else if (2000000 > num)
            {
                res = decimal.Ceiling(num * 2 * 0.001m) / 2 * 1000;
            }
            else
            {
                res = decimal.Ceiling(num * 0.001m) * 1000;
            }
            return res;
        }

        decimal unit2checkR(decimal num)
        {
            decimal res = 0;

            if (10 > num)
            {
                res = decimal.Round(num * 100) / 100;
            }
            else if (100 > num)
            {
                res = decimal.Round(num * 10) / 10;
            }
            else if (1000 > num)
            {
                res = decimal.Round(num);
            }
            else if (10000 > num)
            {
                res = decimal.Round(num * 2 * 0.1m) / 2 * 10;
            }
            else if (100000 > num)
            {
                res = decimal.Round(num * 0.1m) * 10;
            }
            else if (500000 > num)
            {
                res = decimal.Round(num * 2 * 0.01m) / 2 * 100;
            }
            else if (1000000 > num)
            {
                res = decimal.Round(num * 0.01m) * 100;
            }
            else if (2000000 > num)
            {
                res = decimal.Round(num * 2 * 0.001m) / 2 * 1000;
            }
            else
            {
                res = decimal.Round(num * 0.001m) * 1000;
            }
            return res;
        }

        decimal unit2checkT(decimal num)
        {
            decimal res = 0;

            if (10 > num)
            {
                res = decimal.Truncate(num * 100) / 100;
            }
            else if (100 > num)
            {
                res = decimal.Truncate(num * 10) / 10;
            }
            else if (1000 > num)
            {
                res = decimal.Truncate(num);
            }
            else if (10000 > num)
            {
                res = decimal.Truncate(num * 2 * 0.1m) / 2 * 10;
            }
            else if (100000 > num)
            {
                res = decimal.Truncate(num * 0.1m) * 10;
            }
            else if (500000 > num)
            {
                res = decimal.Truncate(num * 2 * 0.01m) / 2 * 100;
            }
            else if (1000000 > num)
            {
                res = decimal.Truncate(num * 0.01m) * 100;
            }
            else if (2000000 > num)
            {
                res = decimal.Truncate(num * 2 * 0.001m) / 2 * 1000;
            }
            else
            {
                res = decimal.Truncate(num * 0.001m) * 1000;
            }
            return res;
        }








        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //int ListCnt = 0;
            //List<decimal> log = new List<decimal>();
            //log.Add(1232.0m);



            //log.Add(1334.0000m);


            //decimal nowPrice = 1234;

            //ListCnt = log.FindIndex(x => x == nowPrice);
            //Console.WriteLine(ListCnt);


            //string[] aa = { "KRW-MANA", "KRW-MANA", "KRW-MANA", "KRW-MANA", "상태" };
            //dataGridView2.Rows.Add(aa);
            //Invoke((MethodInvoker)(() => dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[0]));
            //Invoke((MethodInvoker)(() => dataGridView2.ClearSelection()));


            //Console.WriteLine(unit2check(88881));
            //Console.WriteLine(unit2check(888020));

            //MakeOrderB("KRW-MANA", "", (1000.0000).ToString(), UpbitOrderType.price);
        }



        public long MillisecondsTimestamp(DateTime date)
        {
            DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(date.ToUniversalTime() - baseDate).TotalMilliseconds;
        }

        List<dbdata> k = new List<dbdata>();
        List<vol> v = new List<vol>();


        public void chartView(string market, double Min, double Max)
        {

            Invoke((MethodInvoker)delegate () {
                chart1.Series.Clear();
                Series price = new Series(market);
                chart1.Series.Add(price);
                chart1.Series[market].ChartType = SeriesChartType.Candlestick;
                chart1.Series[market]["OpenCloseStyle"] = "Triangle";
                chart1.Series[market]["ShowOpenClose"] = "Both";

                chart1.Series[market].Color = Color.Green;


                if (Max == Min)
                {
                    Max += 1;
                }

                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Max;
                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Min;

                chart1.Series[market]["PriceUpColor"] = "blue";
                chart1.Series[market]["PriceDownColor"] = "Red";


                //저 고 종 시
                for (int i = 0; i < k.Count; i++)
                {
                    chart1.Series[market].Points.AddXY(k[i].Datum, k[i].Hoog);
                    chart1.Series[market].Points[i].YValues[1] = k[i].Laag;
                    chart1.Series[market].Points[i].YValues[2] = k[i].PrijsOpen;
                    chart1.Series[market].Points[i].YValues[3] = k[i].PrijsGesloten;
                }


            });

        }

        public void chartSView(string market)
        {

            Invoke((MethodInvoker)delegate () {
                chart2.Series.Clear();
                Series price = new Series(market);
                chart2.Series.Add(price);
                //chart2.ChartAreas["ChartArea1"].AxisY.Maximum = Max + 1;
                //chart2.ChartAreas["ChartArea1"].AxisY.Minimum = Min - 1;
                chart2.Series[market].Color = Color.Green;


                for (int i = 0; i < k.Count; i++)
                {
                    chart2.Series[market].Points.Add(v[i].Vol);
                }
            });

        }



        private void button16_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add(textBox8.Text);
        }

        Thread _searchMarket;
        private void button17_Click(object sender, EventArgs e)
        {
            _searchMarket = new Thread(SearchMarket);
            _searchMarket.Start();
        }


        public void search(string MA)
        {




            string market = MA.ToString();
            Invoke((MethodInvoker)(() => label47.Text = "Search: " + market));
            var Candles = GetCandles_Minute(market, UpbitMinuteCandleType._3, default, 4);
            double Min;
            double Max;
            if (Candles != null)
            {
                //저가, 고가, 종가, 시가
                k.Clear();
                v.Clear();
                Max = double.Parse(Candles[0].high_price);
                Min = double.Parse(Candles[0].low_price);
                string res = null;


                Candles.Reverse();
                int i = 0;
                foreach (var XX in Candles)
                {
                    double trade_price = double.Parse(XX.trade_price);
                    double opening_price = double.Parse(XX.opening_price);
                    if (trade_price > opening_price)
                    {
                        res += i.ToString();
                    }

                    k.Add(new dbdata(XX.candle_date_time_kst, double.Parse(XX.low_price), double.Parse(XX.high_price), trade_price, opening_price));
                    v.Add(new vol(XX.candle_date_time_kst, double.Parse(XX.candle_acc_trade_volume)));
                    if (Min > double.Parse(XX.low_price)) Min = double.Parse(XX.low_price);
                    if (Max < double.Parse(XX.high_price)) Max = double.Parse(XX.high_price);
                    i++;
                }
                Console.WriteLine(res);
                if (res != null)
                {
                    if (res.Equals("0123") || res.Equals("023") || res.Equals("123") || res.Equals("23"))
                    {
                        Invoke((MethodInvoker)(() => listBox3.Items.Add(market)));
                        chartView(market, (double)unit2checkT(Convert.ToDecimal(Min)), (double)unit2check(Convert.ToDecimal(Max)));
                        chartSView(market);
                        //Console.WriteLine(market + ", " + res);
                    }
                    else
                    {
                        k.Clear();
                        v.Clear();
                    }
                }
            }
            Invoke((MethodInvoker)(() => label47.Text = "Search: End"));
        }

        List<SearchInfo> RSIList = new List<SearchInfo>();
        public async void SearchMarket()
        {
            RSIList.Clear();
            string result = string.Join(",", markets);


            while (bt28Run)
            {
                var Parse = GetTicker(U, result);
                if(Parse != null)
                {
                    var sort = from www in Parse orderby Convert.ToDecimal(www.acc_trade_price_24h) descending select www;
                    int index = 0;
                    int TickCount = Environment.TickCount;
                    int resTime = Environment.TickCount;
                    foreach (var XX in sort)
                    {
                        SearchStruct ss = TTT(XX.market);
                        //if (index > 20) break;

                        if (ss.rsi > 30 && ss.brsi < 30)
                        {
                            string newMarket = XX.market;
                            index = RSIList.FindIndex(item => item.market.Equals(XX.market));
                            if(index != -1)
                            {
                                if(TickCount - RSIList[index].ET > 180000)
                                {
                                    RSIList[index].switch01 = true;
                                    RSIList[index].BT = RSIList[index].ET;
                                    RSIList[index].ET = TickCount;
                                    RSIList[index].newMarket = XX.market + "\t" + ((Environment.TickCount - RSIList[index].BT) / 1000 / 60) + "분 전";
                                    Console.WriteLine($"{newMarket} {XX.market}: {ss.brsi}, {ss.rsi}");
                                    continue;
                                }
                                else
                                {
                                    RSIList[index].switch01 = false;
                                    Console.WriteLine($"{newMarket} {XX.market}: {ss.brsi}, {ss.rsi}");
                                    continue;
                                }
                            }
                

                            RSIList.Add(new SearchInfo(XX.market, true, TickCount, TickCount, newMarket));
                            Invoke((MethodInvoker)(() => Clipboard.SetText($"{XX.market.Split('-')[1]}")));
                            Console.WriteLine($"{newMarket} {XX.market}: {ss.brsi}, {ss.rsi}");
                        }
                        else
                        {
                            //RSIList.RemoveAll(a => a.market.Equals(XX.market));
                        }
                    }
                    string res = string.Join("\n", from name in RSIList where name.switch01 select name.newMarket);

                    if (res.Length > 0)
                    {
                        await Bot.SendTextMessageAsync(1331550349, $"{res}");
                        //resRSIList.Clear();
                    }

                }
            }
        }

        public struct SearchStruct
        {
            public bool isbool;
            public decimal brsi;
            public decimal rsi;
        }

        private void 제거ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                listBox2.Items.Remove(listBox2.SelectedItem.ToString());
            }
        }


        Thread chartShow;
        bool chartRun = false;
        string showMarket = null;

        private void listBox3_DoubleClick(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                showMarket = listBox3.SelectedItem.ToString();
                if (!chartRun)
                {
                    chartRun = true;
                    chartShow = new Thread(ChartShow);
                    chartShow.Start();
                }

            }
        }


        public void ChartShow()
        {
            while (chartRun)
            {
                string market = showMarket;
                //Invoke((MethodInvoker)(() => label47.Text = "Search: " + market));

                var Candles = GetCandles_Minute(market, UpbitMinuteCandleType._3, default, 4);
                double Min;
                double Max;
                if (Candles != null)
                {
                    //저가, 고가, 종가, 시가
                    k.Clear();
                    v.Clear();
                    Max = double.Parse(Candles[0].high_price);
                    Min = double.Parse(Candles[0].low_price);

                    Candles.Reverse();

                    foreach (var XX in Candles)
                    {
                        double trade_price = double.Parse(XX.trade_price);
                        double opening_price = double.Parse(XX.opening_price);

                        k.Add(new dbdata(XX.candle_date_time_kst, double.Parse(XX.low_price), double.Parse(XX.high_price), trade_price, opening_price));
                        v.Add(new vol(XX.candle_date_time_kst, double.Parse(XX.candle_acc_trade_volume)));
                        if (Min > double.Parse(XX.low_price)) Min = double.Parse(XX.low_price);
                        if (Max < double.Parse(XX.high_price)) Max = double.Parse(XX.high_price);
                    }

                    chartView(market, (double)unit2checkT(Convert.ToDecimal(Min)), (double)unit2check(Convert.ToDecimal(Max)));
                    chartSView(market);

                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (chartRun)
            {
                chartRun = false;
                chartShow.Abort();
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                string market = textBox17.Text;

                int Tprice = Convert.ToInt32(textBox7.Text);


                string[] addstr = { market, Tprice.ToString(), textBox9.Text, textBox18.Text, "대기" };
                OfferListNN.Add(new OfferPack.OfferNN(OfferListNN.Count, textBox17.Text, Convert.ToDecimal(textBox9.Text), Convert.ToDecimal(textBox18.Text), Tprice, false, null, false, 0, 0, false));
                dataGridView3.Rows.Add(addstr);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Setting Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (!isrun2)
            {
                //dataGridView3.Rows.Clear();
                _switchThread2 = new Thread(StartNNVersion);
                _switchThread2.Start();
                isrun2 = true;
                bttime = Environment.TickCount;
                button20.Enabled = false;
                button21.Enabled = true;
            }
        }




        public void StartNNVersion()
        {
            while (true)
            {
                try
                {
                    int index = OfferListNN.FindIndex(item => !item.isOffer);
                    if (index == -1) break;

                    foreach (var XX in OfferListNN)
                    {
                        if (!XX.isOffer)
                        {
                            //Console.WriteLine(XX.market);
                            if (XX.buyUUID == null)
                            {
                                decimal Quantitye = Math.Round((XX.Tprice - (XX.Tprice * 0.05m / 100)) / XX.BuyPrice, 8);
                                var buylist = MakeOrderB(XX.market, Quantitye.ToString(), XX.BuyPrice.ToString(), UpbitOrderType.limit);
                                if (buylist != null)
                                {
                                    XX.buyUUID = buylist.uuid;
                                    Invoke((MethodInvoker)(() => dataGridView3.CurrentCell = dataGridView3.Rows[XX.index].Cells[0]));
                                    Invoke((MethodInvoker)(() => dataGridView3.ClearSelection()));
                                    Invoke((MethodInvoker)(() => dataGridView3.Rows[XX.index].DefaultCellStyle.BackColor = Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(150)))), ((int)(((byte)(130)))))));
                                }
                            }


                            if (XX.buyUUID != null && !XX.isbuy)
                            {
                                var buylist = GetOrder(U, XX.buyUUID);
                                if (buylist != null && buylist.state.Equals("done"))
                                {
                                    Console.WriteLine(XX.market + ": trade Cnt: " + buylist.trades.Count);

                                    decimal vol = 0; //체결 수량
                                    decimal pri = 0; //체결 단가
                                    foreach (var trade in buylist.trades)
                                    {
                                        pri = ((pri * vol) + (Convert.ToDecimal(trade.price) * Convert.ToDecimal(trade.volume))) / (vol + Convert.ToDecimal(trade.volume));
                                        vol += Convert.ToDecimal(trade.volume);

                                        XX.volume = vol;
                                    }

                                    int res = (int)Math.Ceiling(Convert.ToDecimal(buylist.price) + Convert.ToDecimal(buylist.reserved_fee));


                                    Invoke((MethodInvoker)(() => dataGridView3.Rows[XX.index].Cells[4].Value = "매수 완료"));
                                    XX.isbuy = true;
                                }
                            }

                            if (XX.isbuy)
                            {
                                var selllist = MakeOrderS(XX.market, Convert.ToDecimal(XX.volume), XX.SellPrice.ToString(), UpbitOrderType.limit);
                                if (selllist != null)
                                {
                                    Invoke((MethodInvoker)(() => dataGridView3.Rows[XX.index].Cells[4].Value = "매도 주문"));
                                    Invoke((MethodInvoker)(() => dataGridView3.Rows[XX.index].DefaultCellStyle.BackColor = Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))))));
                                    XX.isOffer = true;
                                }
                            }
                        }


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            Console.WriteLine("FINISH");
            button20.Enabled = true;
            button21.Enabled = false;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (isrun2)
            {
                _switchThread2.Abort();
                isrun2 = false;
                BuyAllCancelN();
                button20.Enabled = true;
                button21.Enabled = false;
            }
        }

        public void BuyAllCancelN()
        {
            foreach (var XX in OfferListNN)
            {
                Application.DoEvents();
                if (!XX.isbuy && XX.buyUUID != null)
                {
                    //Console.WriteLine(XX.index + ", " + XX.buyUUID);
                    var Cancel = CancelOrder(XX.buyUUID);
                    //Invoke((MethodInvoker)(() => dataGridView3.CurrentCell = mainView.Rows[XX.index].Cells[0]));
                    //Invoke((MethodInvoker)(() => dataGridView3.ClearSelection()));
                    Invoke((MethodInvoker)(() => dataGridView3.Rows[XX.index].DefaultCellStyle.BackColor = SystemColors.Window));
                    Invoke((MethodInvoker)(() => dataGridView3.Rows[XX.index].Cells[4].Value = "주문 취소"));
                }
            }
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            OfferListNN.Clear();
            dataGridView3.Rows.Clear();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount =
                dataGridView3.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();

                for (int i = 0; i < selectedRowCount; i++)
                {
                    OfferListNN[dataGridView3.SelectedRows[i].Index].isOffer = true;
                    Invoke((MethodInvoker)(() => dataGridView3.Rows[dataGridView3.SelectedRows[i].Index].Cells[4].Value = "정지"));
                }

                //sb.Append("Total: " + selectedRowCount.ToString());
                //MessageBox.Show(sb.ToString(), "Selected Rows");
            }
        }




        Thread _ma;

        private void button25_Click(object sender, EventArgs e)
        {
            if (!isrun)
            {
                isrun = true;
                _ma = new Thread(MA);
                _ma.Start();
                button24.Enabled = true;
                button25.Enabled = false;
                button22.Enabled = false;
            }

        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (isrun)
            {
                isrun = false;
                _ma.Abort();
                button24.Enabled = false;
                button25.Enabled = true;
                button22.Enabled = true;
            }
        }

        List<MA> MovingAvg = new List<MA>();

        public async void MA()
        {
            //await Bot.SendTextMessageAsync(1331550349, $"{Tmarket.Text}\n매수: {dataGridView2.Rows[XX.index].Cells[2].Value} ({(int)XX.buyPrice})\n매도: {dataGridView2.Rows[XX.index].Cells[3].Value} ({(int)sellPrice})\n수익: {(int)(sellPrice - XX.buyPrice)}");
            int ma1 = 5;
            int ma2 = 15;
            int ma3 = 50;

            while (true)
            {
                int index = 0;
                foreach (var MA in MovingAvg)
                {
                    var Candles = GetCandles_Minute(MA.market, UpbitAPI.UpbitMinuteCandleType._1, date: DateTime.Now.AddMinutes(-1), count: ma3);
                    if (Candles != null)
                    {
                        decimal temp1 = 0;
                        decimal temp2 = 0;
                        decimal temp3 = 0;
                        for (int i = 0; i < ma3; i++)
                        {
                            if (i < ma1) temp1 += Convert.ToDecimal(Candles[i].trade_price);
                            if (i < ma2) temp2 += Convert.ToDecimal(Candles[i].trade_price);
                            if (i < ma3) temp3 += Convert.ToDecimal(Candles[i].trade_price);
                        }
                        MA.ma01 = Math.Round(temp1 / ma1, 4);
                        MA.ma02 = Math.Round(temp2 / ma2, 4);
                        MA.ma03 = Math.Round(temp3 / ma3, 4);
                        Invoke((MethodInvoker)(() => dataGridView4.Rows[index].Cells[1].Value = MA.ma01));
                        Invoke((MethodInvoker)(() => dataGridView4.Rows[index].Cells[2].Value = MA.ma02));
                        Invoke((MethodInvoker)(() => dataGridView4.Rows[index].Cells[3].Value = MA.ma03));
                    }

                    if (unit2checkT(MA.ma01) >= unit2checkT(MA.ma03) && MA.flag == 2)
                    {
                        MA.flag = 1;
                        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + MA.market + ": " + MA.ma01 + ", " + MA.ma03);
                        //await Bot.SendTextMessageAsync(1331550349, "[상승]\n" + MA.market + ": " + MA.ma01 + ", " + MA.ma03);
                        Invoke((MethodInvoker)(() => dataGridView4.Rows[index].Cells[5].Value = "[상승]"));
                    }

                    if (unit2checkT(MA.ma01) < unit2checkT(MA.ma03))
                    {
                        MA.flag = 2;
                        Invoke((MethodInvoker)(() => dataGridView4.Rows[index].Cells[5].Value = "[하락]"));
                    }
                    Invoke((MethodInvoker)(() => dataGridView4.Rows[index].Cells[4].Value = unit2checkR(NowPrice(MA.market))));
                    Invoke((MethodInvoker)(() => this.Text = MA.market + " " + MA.flag + " " + DateTime.Now.ToString("HH:mm:ss") + " / KRW " + processID));
                    index++;
                }
            }
        }


        private void button22_Click(object sender, EventArgs e)
        {
            listBox4.Items.Add(textBox24.Text);
            MovingAvg.Add(new MA(textBox24.Text, 0, 0, 0, 99999999, 0));

            string[] addstr = { textBox24.Text, "-", "-", "-", "0", "대기" };
            dataGridView4.Rows.Add(addstr);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            listBox4.Items.Clear();
            MovingAvg.Clear();
        }

        List<marketACC> macc = new List<marketACC>();
        private void button26_Click(object sender, EventArgs e)
        {

            foreach (var XX in sourceM)
            {
                var Parse = GetTicker(U, XX.ToString());
                if (Parse != null)
                {
                    foreach (var YY in Parse)
                    {
                        Application.DoEvents();
                        macc.Add(new marketACC(XX.ToString(), double.Parse(YY.acc_trade_price)));
                    }
                }
            }


            var result = from i in macc orderby i.acc_trade_price descending select i;

            foreach (var XX in result)
            {
                Console.WriteLine($"{XX.market}, {XX.acc_trade_price}");
            }

        }

        string[] selectMarket = { "KRW-ADA", "KRW-XLM", "KRW-XRP", "KRW-EOS", "KRW-MANA", "KRW-SNT", "KRW-XEM", "KRW-SXP" };

        private void button27_Click(object sender, EventArgs e)
        {
            foreach (var XX in selectMarket)
            {
                listBox4.Items.Add(XX);
                MovingAvg.Add(new MA(XX, 0, 0, 0, 99999999, 0));

                string[] addstr = { XX, "-", "-", "-", "0", "대기" };
                dataGridView4.Rows.Add(addstr);
            }

        }

        private void listBox4_MouseHover(object sender, EventArgs e)
        {
            isHover = true;

        }

        private void listBox4_MouseLeave(object sender, EventArgs e)
        {
            isHover = false;
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isHover)
                Clipboard.SetText(listBox4.GetItemText(listBox4.SelectedItem));
        }



        bool bt28Run = false;
        Thread aaa;

        private void button28_Click_1(object sender, EventArgs e)
        {
            if (!bt28Run)
            {
                bt28Run = true;
                button28.Text = "Stop";
                aaa = new Thread(SearchMarket);
                aaa.Start();
                Console.WriteLine("Start");
            }
            else
            {
                bt28Run = false;
                button28.Text = "Start";
                aaa.Abort();
                Console.WriteLine("Abort");
            }

        }
        
        public SearchStruct TTT(string market)
        {
            var Candles = GetCandles_Minute(market, UpbitMinuteCandleType._5, default, 100);
            decimal SU = 0;
            decimal SD = 0;

            decimal AUX = 0;
            decimal ADX = 0;

            decimal AUO = 0;
            decimal ADO = 0;


            decimal beforeTradePrice = 0;
            decimal BAUO = 0;
            decimal BADO = 0;
            decimal TradePrice = 0;

            bool ret = false;
            SearchStruct resualt;
            resualt.isbool = false;
            resualt.rsi = 100;
            resualt.brsi = 100;

            try
            {
                Candles.Reverse();

                if (Candles != null)
                {
                    for (int i = 0; i < 100; i++)
                    {
             
                        TradePrice = Convert.ToDecimal(Candles[i].trade_price);
                        //Console.WriteLine($"{i} {TradePrice} {market}");
                        if (i < 14 && i > 0)
                        {
                            if (TradePrice > beforeTradePrice)
                            {
                                SU += Math.Abs(TradePrice - beforeTradePrice);

                                AUX = SU / 14;
                                AUO = AUX;

                            }
                            else if (TradePrice < beforeTradePrice)
                            {
                                SD += Math.Abs(TradePrice - beforeTradePrice);

                                ADX = SD / 14;
                                ADO = ADX;
                            }

                        }
                        if (i > 13)
                        {
                            if (TradePrice > beforeTradePrice)
                            {
                                AUO = (BAUO * 13 + Math.Abs(TradePrice - beforeTradePrice)) / 14;
                                ADO = (BADO * 13) / 14;
                            }
                            else if (TradePrice < beforeTradePrice)
                            {
                                AUO = (BAUO * 13) / 14;
                                ADO = (BADO * 13 + Math.Abs(TradePrice - beforeTradePrice)) / 14;
                            }
                            else
                            {
                                AUO = (BAUO * 13 + 0) / 14;
                                ADO = (BADO * 13 + 0) / 14;
                            }
                            if(i == 98)
                            {
                                resualt.brsi = Math.Round(100 * AUO / (AUO + ADO), 4);
                            }
                            resualt.rsi = Math.Round(100 * AUO / (AUO + ADO), 4);

                        }
                        //Console.WriteLine($"{i}: {AUO}, {ADO}, {RSI} -- {AUX}, {ADX} -- {BAUO}, {BADO}");
                        BADO = ADO;
                        BAUO = AUO;
                        beforeTradePrice = TradePrice;
                    }
                }
                if (resualt.brsi < 30 && resualt.rsi > 30)
                {
                    resualt.isbool = true;
                }
                Invoke((MethodInvoker)(() => toolStripStatusLabel3.Text = $"[{DateTime.Now.ToString("HH:mm:ss")}] {market}: {resualt.rsi} -- {resualt.brsi}"));
                return resualt;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[{market}] ERROR: {ex.Message}");
                return resualt;
            }
            
        }







    }
}

class SearchInfo
{

    public string market;
    public bool switch01;
    public int ET;
    public int BT;
    public string newMarket;

    public SearchInfo(string marketA, bool switch01A, int ETA, int BTA, string newMarketA) 
    {
        market = marketA; switch01 = switch01A; ET = ETA; BT = BTA; newMarket = newMarketA;
    }
}


class marketACC
{
    public string market;
    public double acc_trade_price;
    public marketACC(string marketA, double acc_trade_priceA) { market = marketA; acc_trade_price = acc_trade_priceA; }
}



class dbdata
{
    public string Datum;
    public double Hoog;
    public double Laag;
    public double PrijsOpen;
    public double PrijsGesloten;
    public dbdata(string d, double h, double l, double o, double c) { Datum = d; Hoog = h; Laag = l; PrijsOpen = o; PrijsGesloten = c; }
}

class vol
{
    public string Datum;
    public double Vol;
    public vol(string d, double v) { Datum = d; Vol = v; }
}

class MA
{
    public string market;
    public decimal ma01;
    public decimal ma02;
    public decimal ma03;
    public int beforeTime;
    public int flag;
    public MA(string marketA, decimal ma01A, decimal ma02A, decimal ma03A, int beforeTimeA, int flagA) { market = marketA; ma01 = ma01A; ma02 = ma02A; ma03 = ma03A; beforeTime = beforeTimeA; flag = flagA; }
}



//2021.02.08 20:10 -> 3,349,738 시작





//- bid : 매수
//- ask : 매도

/*
 *  최소 호가 (이상)	최대 호가 (미만)	주문 가격 단위 (원)
    2,000,000	                     	    1,000
    1,000,000	        2,000,000         	500
    500,000	            1,000,000           100
    100,000	            500,000             50
    10,000	            100,000             10
    1,000	            10,000              5
    100	                1,000               1
    10	                100	                0.1
    0	                10                  0.01
 *
 */