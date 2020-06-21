using BingLibrary.hjb.net;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using SXJLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModel
{
    class MainWindowViewModel : NotificationObject
    {
        #region 属性

        private string mytext;

        public string Mytext
        {
            get { return mytext; }
            set
            {
                mytext = value;
                this.RaisePropertyChanged("Mytext");
            }
        }

        #endregion
        #region 方法绑定
        public DelegateCommand B1Command { get; set; }
        #endregion
        #region 变量
        Fx5u fx5U = new Fx5u("192.168.1.10", 3002);
        bool[] PLCIN, PLCOUT;
        //bool[] RC90IN, RC90OUT;
        public bool[] Rc90In = new bool[100];
        public bool[] Rc90Out;
        public TcpIpClient IOReceiveNet = new TcpIpClient();
        public bool IOReceiveStatus = false;
        #endregion
        #region 构造函数
        public MainWindowViewModel()
        {
            //Mytext= "whx";
            // mytext = "123";
            PLCIN = new bool[100];
            PLCOUT = new bool[100];
            Rc90Out = new bool[100];
            Rc90Out = new bool[100];
            B1Command = new DelegateCommand(new Action(B1CommandExecute));

            Task.Run(() => { PLCRun(); });
            checkIOReceiveNet();
            IORevAnalysis();
            Run();
        }
        #endregion
        #region 方法绑定函数
        private void B1CommandExecute()
        {
            Mytext = Guid.NewGuid().ToString();

        }
        #endregion
        #region 自定义函数
        public async void checkIOReceiveNet()
        {
            while (true)
            {
                await Task.Delay(400);
                if (!IOReceiveNet.tcpConnected)
                {
                    await Task.Delay(1000);
                    if (!IOReceiveNet.tcpConnected)
                    {
                        bool r1 = await IOReceiveNet.Connect("192.168.1.5", 2000);
                        if (r1)
                        {
                            IOReceiveStatus = true;
                            // ModelPrint("机械手IOReceiveNet连接");

                        }
                        else
                            IOReceiveStatus = false;
                    }
                }
                else
                { await Task.Delay(15000); }
            }
        }

        private async void IORevAnalysis()
        {
            while (true)
            {
                //await Task.Delay(100);
                if (IOReceiveStatus == true)
                {
                    string s = await IOReceiveNet.ReceiveAsync();

                    string[] ss = s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        s = ss[0];

                    }
                    catch
                    {
                        s = "error";
                    }

                    if (s == "error")
                    {
                        IOReceiveNet.tcpConnected = false;
                        IOReceiveStatus = false;
                        //ModelPrint("机械手IOReceiveNet断开");
                    }
                    else
                    {
                        string[] strs = s.Split(',');
                        if (strs[0] == "IOCMD" && strs[1].Length == 100)
                        {
                            for (int i = 0; i < 100; i++)
                            {
                                Rc90In[i] = strs[1][i] == '1' ? true : false;
                            }
                            string RsedStr = "";
                            for (int i = 0; i < 100; i++)
                            {
                                RsedStr += Rc90Out[i] ? "1" : "0";
                            }
                            await IOReceiveNet.SendAsync(RsedStr);
                            //ModelPrint("IOSend " + RsedStr);
                            //await Task.Delay(1);
                        }
                        //ModelPrint("IORev: " + s);
                    }
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }





#pragma warning disable CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        async void Run()
#pragma warning restore CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        {

            while (true)
            {
                await Task.Delay(100);
                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        
                        PLCOUT[i] = Rc90In[i];
                        Rc90Out[i] = PLCIN[i];

                    }
                }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
                catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
                {

                }

            }
        }

        void PLCRun()
        {
            while (true)
            {
                /*
                 * 
                 */
                System.Threading.Thread.Sleep(100);
                //Mytext = Guid.NewGuid().ToString();
                //读PLC
                PLCIN = fx5U.ReadMultiM("M2300", 100);
                //写PLC
                fx5U.SetMultiM("M2200", PLCOUT);
            }
        }
        #endregion
    }
}
