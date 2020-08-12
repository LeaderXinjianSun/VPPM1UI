using AvaryAPI;
using BingLibrary.hjb;
using BingLibrary.hjb.file;
using BingLibrary.hjb.net;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using WpfApp1.View;
using 读写器530SDK;

namespace WpfApp1.ViewModel
{
    class MainWindowViewModel : NotificationObject
    {
        #region 属性
        private string messageStr;

        public string MessageStr
        {
            get { return messageStr; }
            set
            {
                messageStr = value;
                this.RaisePropertyChanged("MessageStr");
            }
        }
        private bool statusPLC;

        public bool StatusPLC
        {
            get { return statusPLC; }
            set
            {
                statusPLC = value;
                this.RaisePropertyChanged("StatusPLC");
            }
        }
        /// <summary>
        /// 测试工站                  
        /// </summary>
        private string testStation;

        public string TestStation
        {
            get { return testStation; }
            set
            {
                testStation = value;
                this.RaisePropertyChanged("TestStation");
            }
        }


        /// <summary>
        /// 厂商代码
        /// </summary>
        private string supplier;

        public string Supplier
        {
            get { return supplier; }
            set
            {
                supplier = value;
                this.RaisePropertyChanged("Supplier");
            }
        }
        /// <summary>
        /// 机台编号                  
        /// </summary>
        private string machineNumber;

        public string MachineNumber
        {
            get { return machineNumber; }
            set
            {
                machineNumber = value;
                this.RaisePropertyChanged("MachineNumber");
            }
        }
        private string bigDataPeramEdit;

        public string BigDataPeramEdit
        {
            get { return bigDataPeramEdit; }
            set
            {
                bigDataPeramEdit = value;
                this.RaisePropertyChanged("BigDataPeramEdit");
            }
        }
        /// <summary>
        /// 料号或者程序名称
        /// </summary>
        private string programName;

        public string ProgramName
        {
            get { return programName; }
            set
            {
                programName = value;
                this.RaisePropertyChanged("ProgramName");
            }
        }

        private bool bigDataEditIsReadOnly;

        public bool BigDataEditIsReadOnly
        {
            get { return bigDataEditIsReadOnly; }
            set
            {
                bigDataEditIsReadOnly = value;
                this.RaisePropertyChanged("BigDataEditIsReadOnly");
            }
        }
        private string wARNVER;

        public string WARNVER
        {
            get { return wARNVER; }
            set
            {
                wARNVER = value;
                this.RaisePropertyChanged("WARNVER");
            }
        }
        private string homePageVisibility;

        public string HomePageVisibility
        {
            get { return homePageVisibility; }
            set
            {
                homePageVisibility = value;
                this.RaisePropertyChanged("HomePageVisibility");
            }
        }
        private string alarmReportFormPageVisibility;

        public string AlarmReportFormPageVisibility
        {
            get { return alarmReportFormPageVisibility; }
            set
            {
                alarmReportFormPageVisibility = value;
                this.RaisePropertyChanged("AlarmReportFormPageVisibility");
            }
        }
        private ObservableCollection<AlarmReportFormViewModel> alarmReportForm;

        public ObservableCollection<AlarmReportFormViewModel> AlarmReportForm
        {
            get { return alarmReportForm; }
            set
            {
                alarmReportForm = value;
                this.RaisePropertyChanged("AlarmReportForm");
            }
        }

        #endregion
        #region 方法绑定
        public DelegateCommand<object> MenuActionCommand { get; set; }
        public DelegateCommand BigDataPeramEditCommand { get; set; }
        public DelegateCommand AlarmReportFromExportCommand { get; set; }
        #endregion
        #region 变量
        private string iniParameterPath = System.Environment.CurrentDirectory + "\\Parameter.ini";
        SXJ.Fx5u fx5U = new SXJ.Fx5u("192.168.1.10", 3002);
        bool[] PLCIN, PLCOUT;
        public bool[] Rc90In;
        public bool[] Rc90Out;
        public TcpIpClient IOReceiveNet = new TcpIpClient();
        public bool IOReceiveStatus = false;
        CReader reader = new CReader();
        bool[] M300;
        string LastBanci;
        List<SXJ.AlarmData> AlarmList = new List<SXJ.AlarmData>();
        #endregion
        #region 构造函数
        public MainWindowViewModel()
        {
            MenuActionCommand = new DelegateCommand<object>(new Action<object>(this.MenuActionCommandExecute));
            BigDataPeramEditCommand = new DelegateCommand(new Action(this.BigDataPeramEditCommandExecute));
            AlarmReportFromExportCommand = new DelegateCommand(new Action(this.AlarmReportFromExportCommandExecute));
            fx5U.ConnectStateChanged += Fx5uConnectStateChanged;
            Init();
            Task.Run(() => { PLCRun(); });
            checkIOReceiveNet();
            IORevAnalysis();
            Run();
        }
        #endregion
        #region 方法绑定函数
        private void MenuActionCommandExecute(object p)
        {
            switch (p.ToString())
            {
                case "0":
                    HomePageVisibility = "Visible";
                    AlarmReportFormPageVisibility = "Collapsed";
                    break;
                case "1":
                    HomePageVisibility = "Collapsed";
                    AlarmReportFormPageVisibility = "Visible";
                    break;
                default:
                    break;
            }
        }
        private void BigDataPeramEditCommandExecute()
        {
            if (BigDataEditIsReadOnly)
            {
                BigDataEditIsReadOnly = false;
                BigDataPeramEdit = "Save";
            }
            else
            {
                Inifile.INIWriteValue(iniParameterPath, "System", "TestStation", TestStation);
                Inifile.INIWriteValue(iniParameterPath, "System", "Supplier", Supplier);
                Inifile.INIWriteValue(iniParameterPath, "System", "MachineNumber", MachineNumber);
                Inifile.INIWriteValue(iniParameterPath, "System", "ProgramName", ProgramName);
                Inifile.INIWriteValue(iniParameterPath, "System", "WARNVER", WARNVER);
                BigDataEditIsReadOnly = true;
                BigDataPeramEdit = "Edit";
                AddMessage("大数据参数保存");
            }
        }
        private void AlarmReportFromExportCommandExecute()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //dlg.FileName = "AlarmReport"; // Default file name
            //dlg.DefaultExt = ".xlsx"; // Default file extension
            dlg.Filter = "Text Files(*.xlsx)|*.xlsx|All(*.*)|*"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                WriteAlarmtoExcel(dlg.FileName);
            }
        }
        #endregion
        #region 自定义函数
        private void Init()
        {
            MessageStr = "";
            BigDataEditIsReadOnly = true;
            BigDataPeramEdit = "Edit";
            PLCIN = new bool[100];
            PLCOUT = new bool[100];
            Rc90In = new bool[100];
            Rc90Out = new bool[100];
            StatusPLC = true;
            TestStation = Inifile.INIGetStringValue(iniParameterPath, "System", "TestStation", "NA");
            Supplier = Inifile.INIGetStringValue(iniParameterPath, "System", "Supplier", "NA");
            MachineNumber = Inifile.INIGetStringValue(iniParameterPath, "System", "MachineNumber", "NA");
            ProgramName = Inifile.INIGetStringValue(iniParameterPath, "System", "ProgramName", "NA");
            WARNVER = Inifile.INIGetStringValue(iniParameterPath, "System", "WARNVER", "NA");
            LastBanci = Inifile.INIGetStringValue(iniParameterPath, "Summary", "LastBanci", "null");
            HomePageVisibility = "Visible";
            AlarmReportFormPageVisibility = "Collapsed";

            try
            {
                using (StreamReader reader = new StreamReader(System.IO.Path.Combine(System.Environment.CurrentDirectory, "AlarmReportForm.json")))
                {
                    string json = reader.ReadToEnd();
                    AlarmReportForm = JsonConvert.DeserializeObject<ObservableCollection<AlarmReportFormViewModel>>(json);
                }
            }
            catch (Exception ex)
            {
                AlarmReportForm = new ObservableCollection<AlarmReportFormViewModel>();
                WriteToJson(AlarmReportForm, System.IO.Path.Combine(System.Environment.CurrentDirectory, "AlarmReportForm.json"));
                AddMessage(ex.Message);
            }
            #region 报警文档
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string alarmExcelPath = Path.Combine(System.Environment.CurrentDirectory, "VPP报警.xlsx");
                if (File.Exists(alarmExcelPath))
                {

                    FileInfo existingFile = new FileInfo(alarmExcelPath);
                    using (ExcelPackage package = new ExcelPackage(existingFile))
                    {
                        // get the first worksheet in the workbook
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        for (int i = 1; i <= worksheet.Dimension.End.Row; i++)
                        {
                            SXJ.AlarmData ad = new SXJ.AlarmData();
                            ad.Code = worksheet.Cells["A" + i.ToString()].Value == null ? "Null" : worksheet.Cells["A" + i.ToString()].Value.ToString();
                            ad.Content = worksheet.Cells["B" + i.ToString()].Value == null ? "Null" : worksheet.Cells["B" + i.ToString()].Value.ToString();
                            ad.Type = worksheet.Cells["C" + i.ToString()].Value == null ? "Null" : worksheet.Cells["C" + i.ToString()].Value.ToString();
                            ad.Start = DateTime.Now;
                            ad.End = DateTime.Now;
                            ad.State = false;
                            AlarmList.Add(ad);
                        }
                        AddMessage("读取到" + worksheet.Dimension.End.Row.ToString() + "条报警");
                    }
                }
                else
                {
                    AddMessage("VPP报警.xlsx 文件不存在");
                }
            }
            catch (Exception ex)
            {
                AddMessage(ex.Message);
            }
            #endregion
        }
        private void WriteToJson(object p, string path)
        {
            try
            {
                using (FileStream fs = File.Open(path, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, p);
                }
            }
            catch (Exception ex)
            {
                AddMessage(ex.Message);
            }
        }
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
                await Task.Delay(50);
                try
                {
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
                                string RsedStr = "";
                                try
                                {
                                    for (int i = 0; i < 100; i++)
                                    {
                                        PLCOUT[i] = strs[1][i] == '1' ? true : false;
                                    }

                                    for (int i = 0; i < 100; i++)
                                    {
                                        RsedStr += PLCIN[i] ? "1" : "0";
                                    }
                                }
                                catch (Exception)
                                {


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
                catch { }
            }
        }
        async void Run()
        {
            int count1 = 0, oldMinute = -1;
            string CurrentAlarm = "";
            string MODE = "1";
            int CardStatus = 1, cardret = 1;
            if (!Directory.Exists("D:\\报警记录"))
            {
                Directory.CreateDirectory("D:\\报警记录");
            }

            //Task.Run(() =>
            //{
            //    while (true)
            //    {


            //        Thread.Sleep(50);
            //        #region 互刷
            //        try
            //        {
            //            //for (int i = 0; i < 100; i++)
            //            //{
            //            PLCOUT = Rc90In;
            //            Rc90Out = PLCIN;
            //            // }
            //        }
            //        catch { }
            //        #endregion
            //    }
            //});




            //await Task.Run(() => {
                while (true)
                {
                    await Task.Delay(100);
                    //Thread.Sleep(100);
                    if (count1++ > 4)
                    {
                        count1 = 0;
                        #region 刷卡
                        await Task.Run(() => {
                        try
                        {
                            byte[] buf = new byte[256];//用来存储卡信息的buff
                            byte[] snr = 读写器530SDK.CPublic.CharToByte("FF FF FF FF FF FF");//应该是一种读码格式，照抄即可。

                            if (IntPtr.Zero == reader.GetHComm())
                            {
                                string COM = Inifile.INIGetStringValue(iniParameterPath, "读卡器", "COM", "COM19").Replace("COM", "");
                                reader.OpenComm(int.Parse(COM), 9600);
                                MODE = Inifile.INIGetStringValue(iniParameterPath, "读卡器", "MODE", "3");
                            }

                            //刷卡；若刷到卡返回0，没刷到回1。
                            CardStatus = reader.MF_Read(0, byte.Parse(MODE), 0, 1, ref snr[0], ref buf[0]);
                            //采用上升沿信号，防止卡放在读卡机上，重复执行查询动作。寄卡放一次，才查询一次，要再查询，需要重新刷卡。
                            if (cardret != CardStatus)
                            {
                                cardret = CardStatus;
                                if (CardStatus == 0)//刷到卡了
                                {
                                    string barcode = getCardSN(new byte[] { buf[2], buf[3], buf[4], buf[5] });
                                    AddMessage("刷卡 " + barcode);
                                    string workNo = "";
                                    bool res1 = new CardVerify().checkOperatorAbility(barcode, ref workNo);
                                    string EnableRun = res1 ? "Y" : "N";
                                    string strOperatorSN = new CardVerify().getOperatorNumber(barcode);
                                    AddMessage("人员: " + strOperatorSN + " 工号:" + workNo + " 权限:" + EnableRun);
                                    SXJ.Mysql mysql = new SXJ.Mysql();
                                    if (mysql.Connect())
                                    {
                                        string stm = "insert into TED_CARD_DATA （TestStation,MachineNumber,TestDate,TestTime,CardNumber,WorkerNumber,EnableRun,ErrMessage,Supplier,SystemDate,SystemTime) value('" + TestStation + "','" + MachineNumber + "','" + DateTime.Now.ToString("yyyyMMdd") + "','" + DateTime.Now.ToString("HHmmss") + "','" + barcode + "','" + workNo + "','" + EnableRun + "','NA','" + Supplier + "','" + DateTime.Now.ToString("yyyyMMdd") + "','" + DateTime.Now.ToString("HHmmss") + "')";
                                        mysql.executeQuery(stm);
                                    }
                                    mysql.DisConnect();
                                    fx5U.SetMultiM("M401", new bool[2] { false, false });
                                    if (res1)
                                    {
                                        fx5U.SetM("M401", true);
                                    }
                                    else
                                    {
                                        fx5U.SetM("M402", true);
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            reader.CloseComm();
                            AddMessage(ex.Message);
                        }
                        });
                    }
                    #endregion
                    #region 报警记录
                    // await Task.Run(()=> {
                    try
                    {
                        //读报警
                        M300 = fx5U.ReadMultiM("M3200", 800);
                        if (M300 != null && StatusPLC)
                        {
                            for (int i = 0; i < AlarmList.Count; i++)
                            {
                                if (M300[i] != AlarmList[i].State && AlarmList[i].Content != "Null")
                                {
                                    AlarmList[i].State = M300[i];
                                    if (AlarmList[i].State)
                                    {
                                        AlarmList[i].Start = DateTime.Now;
                                        AlarmList[i].End = DateTime.Now;
                                        AddMessage(AlarmList[i].Code + AlarmList[i].Content + "发生");
                                        var nowAlarm = AlarmReportForm.FirstOrDefault(s => s.Code == AlarmList[i].Code);
                                        if (nowAlarm == null)
                                        {
                                            AlarmReportFormViewModel newAlarm = new AlarmReportFormViewModel()
                                            {
                                                Code = AlarmList[i].Code,
                                                Content = AlarmList[i].Content,
                                                Count = 0,
                                                TimeSpan = AlarmList[i].End - AlarmList[i].Start
                                            };

                                            //Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
                                            //{
                                            
                                                AlarmReportForm.Add(newAlarm);
                                            //}));
                                            WriteToJson(AlarmReportForm, System.IO.Path.Combine(System.Environment.CurrentDirectory, "AlarmReportForm.json"));
                                        }
                                        if (CurrentAlarm != AlarmList[i].Content)
                                        {
                                            string banci = GetBanci();
                                            if (!File.Exists(Path.Combine("D:\\报警记录", "VPP报警记录" + banci + ".csv")))
                                            {
                                                string[] heads = new string[] { "时间", "内容" };
                                                Csvfile.savetocsv(Path.Combine("D:\\报警记录", "VPP报警记录" + banci + ".csv"), heads);
                                            }
                                            string[] conts = new string[] { AlarmList[i].Start.ToString(), AlarmList[i].Content };
                                            Csvfile.savetocsv(Path.Combine("D:\\报警记录", "VPP报警记录" + banci + ".csv"), conts);
                                            CurrentAlarm = AlarmList[i].Content;
                                            #region 上传
                                            string Banci = (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20) ? "D" : "N";
                                            SXJ.Mysql mysql = new SXJ.Mysql();
                                            if (mysql.Connect())
                                            {
                                                string stm = "insert into TED_WARN_DATA (WORKSTATION,PARTNUM,MACID,LOADID,PETID,TDATE,TTIME,CLASS,WARNID,DETAILID,WARNNUM,FL01,FL02,FL03,FL04,FL05,FL06,FL07,FL08,FL09,FL10,SUPPLIER,WARNVER) value('" + TestStation + "','" + ProgramName + "','" + MachineNumber + "','" + MachineNumber + "','','" + DateTime.Now.ToString("yyyyMMdd") + "','" + DateTime.Now.ToString("HHmmss") + "','" + Banci + "','" + AlarmList[i].Content + "','','1','','','','','','','','','','','" + Supplier + "','" + WARNVER + "')";
                                                mysql.executeQuery(stm);
                                            }
                                            mysql.DisConnect();
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        AlarmList[i].End = DateTime.Now;
                                        AddMessage(AlarmList[i].Code + AlarmList[i].Content + "解除");
                                        var nowAlarm = AlarmReportForm.FirstOrDefault(s => s.Code == AlarmList[i].Code);
                                        if (nowAlarm != null)
                                        {
                                            nowAlarm.Count++;
                                            nowAlarm.TimeSpan += AlarmList[i].End - AlarmList[i].Start;
                                            WriteToJson(AlarmReportForm, System.IO.Path.Combine(System.Environment.CurrentDirectory, "AlarmReportForm.json"));
                                        }
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //});

                    #endregion
                    #region 换班
                    if (LastBanci != GetBanci())
                    {
                        try
                        {
                            WriteAlarmtoExcel(Path.Combine("D:\\报警记录", "VPP报警统计" + LastBanci + ".xlsx"));
                            //Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => {
                                AlarmReportForm.Clear();
                           // }));


                            WriteToJson(AlarmReportForm, System.IO.Path.Combine(System.Environment.CurrentDirectory, "AlarmReportForm.json"));
                            LastBanci = GetBanci();
                            Inifile.INIWriteValue(iniParameterPath, "Summary", "LastBanci", LastBanci);
                            AddMessage(LastBanci + " 换班数据清零");
                        }
                        catch (Exception ex)
                        {
                            AddMessage(ex.Message);
                        }
                    }
                    #endregion

                    await Task.Run(() =>
                    {
                    if (DateTime.Now.Minute != oldMinute)
                    {
                        oldMinute = DateTime.Now.Minute;

                        #region 心跳
                        try
                        {
                            int item = fx5U.ReadW("D300");
                            string Status = "";
                            switch (item)
                            {
                                case 1:
                                    Status = "R";
                                    break;
                                case 2:
                                    Status = "H";
                                    break;
                                case 3:
                                    Status = "A";
                                    break;
                                default:
                                    break;
                            }
                            SXJ.Mysql mysql = new SXJ.Mysql();
                            if (mysql.Connect())
                            {
                                string stm = "insert into TED_HEART_DATA (TestStation,MachineNumber,TestDate,TestTime,AlarmCode,Status,ProgramName,Barcode,SystemDate,SystemTime,SUPPLIER) value('" + TestStation + "','" + MachineNumber + "','" + DateTime.Now.ToString("yyyyMMdd") + "','" + DateTime.Now.ToString("HHmmss") + "','NA','" + Status + "','" + ProgramName + "','NA','" + DateTime.Now.ToString("yyyyMMdd") + "','" + DateTime.Now.ToString("HHmmss") + "','" + Supplier + "')";
                                mysql.executeQuery(stm);
                            }
                            mysql.DisConnect();
                            AddMessage("上传心跳:" + Status);
                        }
                        catch (Exception ex)
                        {
                            AddMessage(ex.Message);
                        }

                        #endregion
                    }
                    });
                }

            //});
            
        }
        void PLCRun()
        {
            while (true)
            {
                /*
                 * 
                 */
                System.Threading.Thread.Sleep(100);
                try
                {
                    //读PLC
                    PLCIN = fx5U.ReadMultiM("M2300", 100);
                    //写PLC
                    fx5U.SetMultiM("M2200", PLCOUT);
                    fx5U.SetM("M400", true);
                }
                catch { }
            }
        }
        private void AddMessage(string str)
        {
            string[] s = MessageStr.Split('\n');
            if (s.Length > 1000)
            {
                MessageStr = "";
            }
            if (MessageStr != "")
            {
                MessageStr += "\n";
            }
            MessageStr += System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + str;
        }
        void Fx5uConnectStateChanged(object sender, bool e)
        {
            StatusPLC = e;
        }
        private string getCardSN(byte[] buf2)
        {
            string str1 = "";
            try
            {
                if (buf2.Length > 0)
                {
                    int sum = 0;
                    string strCard = "";
                    int count = buf2.Length - 1;
                    while (buf2[count] == 0x00) { count--; };
                    for (int i = 0; i <= count; i++)
                    {
                        strCard += string.Format("{0:X2} ", buf2[i]);
                        sum += buf2[i] << i * 8;
                    }
                    str1 = sum.ToString("0000000000"); //0756267432
                }
            }
            catch { }

            return str1;
        }
        private string GetBanci()
        {
            string rs = "";
            if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20)
            {
                rs += DateTime.Now.ToString("yyyyMMdd") + "_D";
            }
            else
            {
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 8)
                {
                    rs += DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "_N";
                }
                else
                {
                    rs += DateTime.Now.ToString("yyyyMMdd") + "_N";
                }
            }
            return rs;
        }
        private void WriteAlarmtoExcel(string filepath)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("MySheet");
                    ws.Cells[1, 1].Value = "ID";
                    ws.Cells[1, 2].Value = "报警内容";
                    ws.Cells[1, 3].Value = "报警次数";
                    ws.Cells[1, 4].Value = "报警时长(分钟)";
                    ws.Cells[1, 5].Value = DateTime.Now.ToString();
                    for (int i = 0; i < AlarmReportForm.Count; i++)
                    {
                        ws.Cells[i + 2, 1].Value = AlarmReportForm[i].Code;
                        ws.Cells[i + 2, 2].Value = AlarmReportForm[i].Content;
                        ws.Cells[i + 2, 3].Value = AlarmReportForm[i].Count;
                        ws.Cells[i + 2, 4].Value = Math.Round(AlarmReportForm[i].TimeSpan.TotalMinutes, 1);
                    }
                    package.SaveAs(new FileInfo(filepath));
                }

            }
            catch (Exception ex)
            {
                AddMessage(ex.Message);
            }

        }
        #endregion
    }
}
