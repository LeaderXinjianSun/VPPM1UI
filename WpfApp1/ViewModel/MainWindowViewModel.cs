﻿using AvaryAPI;
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
using System.Data;
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
        private string alarmStatisticsPageVisibility;

        public string AlarmStatisticsPageVisibility
        {
            get { return alarmStatisticsPageVisibility; }
            set
            {
                alarmStatisticsPageVisibility = value;
                this.RaisePropertyChanged("AlarmStatisticsPageVisibility");
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

        private ObservableCollection<AlarmReportFormViewModel> alarmReportFormTester;

        public ObservableCollection<AlarmReportFormViewModel> AlarmReportFormTester
        {
            get { return alarmReportFormTester; }
            set
            {
                alarmReportFormTester = value;
                this.RaisePropertyChanged("AlarmReportFormTester");
            }
        }

        private ObservableCollection<AlarmReportFormViewModel> alarmReportFormFeeder;

        public ObservableCollection<AlarmReportFormViewModel> AlarmReportFormFeeder
        {
            get { return alarmReportFormFeeder; }
            set
            {
                alarmReportFormFeeder = value;
                this.RaisePropertyChanged("AlarmReportFormFeeder");
            }
        }

        private int alarmCout;
        public int AlarmCout
        {
            get { return alarmCout; }
            set
            {
                alarmCout = value;
                this.RaisePropertyChanged("AlarmCout");
            }
        }
        private int pcsCout;
        public int PcsCout
        {
            get { return pcsCout; }
            set
            {
                pcsCout = value;
                this.RaisePropertyChanged("PcsCout");
            }
        }
        private double passRadio;
        public double PassRadio
        {
            get { return passRadio; }
            set
            {
                passRadio = value;
                this.RaisePropertyChanged("PassRadio");
            }
        }
        private string version;
        public string Version
        {
            get { return version; }
            set
            {
                version = value;
                this.RaisePropertyChanged("Version");
            }        
        }
        private MachineStateViewModel machineStateA;
        public MachineStateViewModel MachineStateA
        {
            get { return machineStateA; }
            set
            {
                machineStateA = value;
                this.RaisePropertyChanged("MachineStateA");
            }
        }
        private MachineStateViewModel machineStateB;
        public MachineStateViewModel MachineStateB
        {
            get { return machineStateB; }
            set
            {
                machineStateB = value;
                this.RaisePropertyChanged("MachineStateB");
            }
        }
        private DateTime alarmSelectStartDate;

        public DateTime AlarmSelectStartDate
        {
            get { return alarmSelectStartDate; }
            set
            {
                alarmSelectStartDate = value;
                this.RaisePropertyChanged("AlarmSelectStartDate");
            }
        }
        private DateTime alarmSelectEndtDate;

        public DateTime AlarmSelectEndtDate
        {
            get { return alarmSelectEndtDate; }
            set
            {
                alarmSelectEndtDate = value;
                this.RaisePropertyChanged("AlarmSelectEndtDate");
            }
        }
        private DataTable alarmSelectFormDt;

        public DataTable AlarmSelectFormDt
        {
            get { return alarmSelectFormDt; }
            set
            {
                alarmSelectFormDt = value;
                this.RaisePropertyChanged("AlarmSelectFormDt");
            }
        }
        private ObservableCollection<AlarmReportFormViewModel> alarmStatictic;

        public ObservableCollection<AlarmReportFormViewModel> AlarmStatictic
        {
            get { return alarmStatictic; }
            set
            {
                alarmStatictic = value;
                this.RaisePropertyChanged("AlarmStatictic");
            }
        }
        private int totalAlarmCount;

        public int TotalAlarmCount
        {
            get { return totalAlarmCount; }
            set
            {
                totalAlarmCount = value;
                this.RaisePropertyChanged("TotalAlarmCount");
            }
        }
        private TimeSpan totalAlarmTimeSpan;

        public TimeSpan TotalAlarmTimeSpan
        {
            get { return totalAlarmTimeSpan; }
            set
            {
                totalAlarmTimeSpan = value;
                this.RaisePropertyChanged("TotalAlarmTimeSpan");
            }
        }
        private bool checkDbButtonIsEnabled;

        public bool CheckDbButtonIsEnabled
        {
            get { return checkDbButtonIsEnabled; }
            set
            {
                checkDbButtonIsEnabled = value;
                this.RaisePropertyChanged("CheckDbButtonIsEnabled");
            }
        }

        #endregion
        #region 方法绑定
        public DelegateCommand<object> MenuActionCommand { get; set; }
        public DelegateCommand BigDataPeramEditCommand { get; set; }
        public DelegateCommand AlarmReportFromExportCommand { get; set; }
        public DelegateCommand FuncCommand { get; set; }
        public DelegateCommand CheckAlarmFromDtCommand { get; set; }
        public DelegateCommand ExportAlarmCommand { get; set; }
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
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        int D301 = -1, D302 = -1;
        #endregion
        #region 构造函数
        public MainWindowViewModel()
        {
            MenuActionCommand = new DelegateCommand<object>(new Action<object>(this.MenuActionCommandExecute));
            BigDataPeramEditCommand = new DelegateCommand(new Action(this.BigDataPeramEditCommandExecute));
            AlarmReportFromExportCommand = new DelegateCommand(new Action(this.AlarmReportFromExportCommandExecute));
            FuncCommand = new DelegateCommand(new Action(this.FuncCommandExecute));
            CheckAlarmFromDtCommand = new DelegateCommand(new Action(this.CheckAlarmFromDtCommandExecute));
            ExportAlarmCommand = new DelegateCommand(new Action(this.ExportAlarmCommandExecute));
            fx5U.ConnectStateChanged += Fx5uConnectStateChanged;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            //dispatcherTimer.Tick += DispatcherTimer_Tick;
            //dispatcherTimer.Start();
            Init();
            Task.Run(() => { PLCRun(); });
            checkIOReceiveNet();
            IORevAnalysis();
            Run();
        }
        /// <summary>
        /// 计时器，每秒执行一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
//            项目 说明  优先级
//机台停机时间  待料 上料盘、下料盘传感器感应无料  0
//    换膜 剥膜失败报警、NG盘满 1
//    样本 样本测试    2
//    报警 测试机 测试机报警   3
//    故障 急停、开门、非运行流程 4
//    报警 上料机 上料机所有报警 5
//机台运行时间              6

            switch (D301)
            {
                case 1:
                    MachineStateA.DaiLiao += (double)1 / 60;
                    break;
                case 2:
                    MachineStateA.HuanMo += (double)1 / 60;
                    break;
                case 3:
                    MachineStateA.YangBen += (double)1 / 60;
                    break;
                case 4:
                    MachineStateA.TesterAlarm += (double)1 / 60;
                    break;
                case 5:
                    MachineStateA.Down += (double)1 / 60;
                    break;
                case 6:
                    MachineStateA.UploaderAlarm += (double)1 / 60;
                    break;
                case 7:
                    MachineStateA.Run += (double)1 / 60;
                    break;
                default:
                    break;
            }
            if (D301 > 0 && D301 < 8)
            {
                WriteToJson(MachineStateA, System.IO.Path.Combine(System.Environment.CurrentDirectory, "MachineStateA.json"));
            }
            switch (D302)
            {
                case 1:
                    MachineStateB.DaiLiao += (double)1 / 60;
                    break;
                case 2:
                    MachineStateB.HuanMo += (double)1 / 60;
                    break;
                case 3:
                    MachineStateB.YangBen += (double)1 / 60;
                    break;
                case 4:
                    MachineStateB.TesterAlarm += (double)1 / 60;
                    break;
                case 5:
                    MachineStateB.Down += (double)1 / 60;
                    break;
                case 6:
                    MachineStateB.UploaderAlarm += (double)1 / 60;
                    break;
                case 7:
                    MachineStateB.Run += (double)1 / 60;
                    break;
                default:
                    break;
            }
            if (D302 > 0 && D302 < 8)
            {
                WriteToJson(MachineStateB, System.IO.Path.Combine(System.Environment.CurrentDirectory, "MachineStateB.json"));
            }
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
                    AlarmStatisticsPageVisibility = "Collapsed";
                    break;
                case "1":
                    HomePageVisibility = "Collapsed";
                    AlarmReportFormPageVisibility = "Visible";
                    AlarmStatisticsPageVisibility = "Collapsed";
                    break;
                case "2":
                    HomePageVisibility = "Collapsed";
                    AlarmReportFormPageVisibility = "Collapsed";
                    AlarmStatisticsPageVisibility = "Visible";
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
            AlarmReportFormExecute();
            //return;
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
        private void FuncCommandExecute()
        {
            //var aa = fx5U.ReadDW("D6500");
            //var aa = 147 << 8;
            //WriteStatetoExcel(Path.Combine("D:\\报警记录", "VPP时间统计" + LastBanci + ".xlsx"));
        }
        private async void CheckAlarmFromDtCommandExecute()
        {
            //# INSERT INTO ldr_warn_data (WORKSTATION,MACID,WARNID,DETAILID,STARTTIME,SUPPLIER) VALUES ('VPP','VPP-03','M300','吸取失败了','2018-11-05 20:29:36','LDR')
            //# SELECT * FROM ldr_warn_data WHERE STARTTIME BETWEEN '2016-1-1 12:29:00' AND '2019-1-1 12:29:00' ORDER BY STARTTIME ASC
            //# UPDATE ldr_warn_data SET ENDTIME = NOW() WHERE WORKSTATION = 'VPP' AND MACID = 'VPP-03' AND STARTTIME = '2017-11-05 20:29:36'
            CheckDbButtonIsEnabled = false;
            try
            {
                AlarmSelectFormDt = await Task.Run<DataTable>(() => {
                    SXJ.Mysql mysql = new SXJ.Mysql();
                    if (mysql.Connect())
                    {
                        string stm = $"SELECT * FROM LDR_WARN_DATA WHERE WORKSTATION = '{TestStation}' AND MACID = '{MachineNumber}' AND STARTTIME BETWEEN '{AlarmSelectStartDate}' AND '{AlarmSelectEndtDate}' ORDER BY SYSDATETIME DESC";
                        DataSet ds = mysql.Select(stm);
                        mysql.DisConnect();
                        return ds.Tables["table0"];
                    }
                    mysql.DisConnect();
                    return null;
                });
                AlarmStatictic.Clear();
                TotalAlarmCount = 0;
                TotalAlarmTimeSpan = TimeSpan.Zero;
                if (AlarmSelectFormDt != null && AlarmSelectFormDt.Rows.Count > 0)
                {


                    for (int i = 0; i < AlarmSelectFormDt.Rows.Count; i++)
                    {
                        try
                        {
                            if (AlarmSelectFormDt.Rows[i]["STARTTIME"] != DBNull.Value && AlarmSelectFormDt.Rows[i]["ENDTIME"] != DBNull.Value)
                            {
                                var nowAlarm = AlarmStatictic.FirstOrDefault(s => s.Code == (string)AlarmSelectFormDt.Rows[i]["WARNID"]);
                                if (nowAlarm == null)
                                {
                                    AlarmReportFormViewModel newAlarm = new AlarmReportFormViewModel()
                                    {
                                        Code = (string)AlarmSelectFormDt.Rows[i]["WARNID"],
                                        Content = (string)AlarmSelectFormDt.Rows[i]["DETAILID"],
                                        Count = 1,
                                        TimeSpan = Convert.ToDateTime(AlarmSelectFormDt.Rows[i]["ENDTIME"]) - Convert.ToDateTime(AlarmSelectFormDt.Rows[i]["STARTTIME"])
                                    };

                                    AlarmStatictic.Add(newAlarm);
                                }
                                else
                                {
                                    nowAlarm.Count++;
                                    nowAlarm.TimeSpan += Convert.ToDateTime(AlarmSelectFormDt.Rows[i]["ENDTIME"]) - Convert.ToDateTime(AlarmSelectFormDt.Rows[i]["STARTTIME"]);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AddMessage(ex.Message);
                        }
                        
                    }

                    foreach (var item in AlarmStatictic)
                    {
                        TotalAlarmCount += item.Count;
                        TotalAlarmTimeSpan += item.TimeSpan;
                    }
                }
            }
            catch (Exception ex)
            {
                AddMessage(ex.Message);
            }
            CheckDbButtonIsEnabled = true;
        }

        private  void AlarmReportFormExecute()
        {
            try
            {
                AlarmReportFormTester = new ObservableCollection<AlarmReportFormViewModel>(AlarmReportForm.Where(s => s.Content.Contains("_测试机")));
                AlarmReportFormFeeder = new ObservableCollection<AlarmReportFormViewModel>(AlarmReportForm.Where(s => !s.Content.Contains("_测试机")));
            }
            catch (Exception)
            {

             
            }

        }

        private  void ExportAlarmCommandExecute()
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
                try
                {
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("机台报警统计"+DateTime.Now.ToString("yyyyMMddHHmmss"));
                        ws.Cells[1, 1].Value = "报警代码";
                        ws.Cells[1, 2].Value = "报警内容";
                        ws.Cells[1, 3].Value = "报警次数";
                        ws.Cells[1, 4].Value = "报警时长";
                        ws.Cells[1, 5].Value = MachineNumber;
                        ws.Cells[1, 6].Value = DateTime.Now.ToString();
                        for (int i = 0; i < AlarmStatictic.Count; i++)
                        {
                            ws.Cells[i + 2, 1].Value = AlarmStatictic[i].Code;
                            ws.Cells[i + 2, 2].Value = AlarmStatictic[i].Content;
                            ws.Cells[i + 2, 3].Value = AlarmStatictic[i].Count;
                            ws.Cells[i + 2, 4].Value = AlarmStatictic[i].TimeSpan.ToString();
                        }
                        ws.Cells[AlarmStatictic.Count + 2, 2].Value = "Total:";
                        ws.Cells[AlarmStatictic.Count + 2, 3].Value = TotalAlarmCount;
                        ws.Cells[AlarmStatictic.Count + 2, 4].Value = TotalAlarmTimeSpan.ToString();

                        ws = package.Workbook.Worksheets.Add("机台报警明细" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                        ws.Cells[1, 1].Value = "工站";
                        ws.Cells[1, 2].Value = "机台编号";
                        ws.Cells[1, 3].Value = "报警代码";
                        ws.Cells[1, 4].Value = "报警内容";
                        ws.Cells[1, 5].Value = "开始时间";
                        ws.Cells[1, 6].Value = "结束时间";
                        ws.Cells[1, 7].Value = "厂商代码";
                        ws.Cells[1, 8].Value = "系统时间";
                        ws.Cells[1, 9].Value = DateTime.Now.ToString();
                        for (int i = 0; i < AlarmSelectFormDt.Rows.Count; i++)
                        {
                            ws.Cells[i + 2, 1].Value = AlarmSelectFormDt.Rows[i]["WORKSTATION"];
                            ws.Cells[i + 2, 2].Value = AlarmSelectFormDt.Rows[i]["MACID"];
                            ws.Cells[i + 2, 3].Value = AlarmSelectFormDt.Rows[i]["WARNID"];
                            ws.Cells[i + 2, 4].Value = AlarmSelectFormDt.Rows[i]["DETAILID"];
                            ws.Cells[i + 2, 5].Value = AlarmSelectFormDt.Rows[i]["STARTTIME"].ToString();
                            ws.Cells[i + 2, 6].Value = AlarmSelectFormDt.Rows[i]["ENDTIME"].ToString();
                            ws.Cells[i + 2, 7].Value = AlarmSelectFormDt.Rows[i]["SUPPLIER"];
                            ws.Cells[i + 2, 8].Value = AlarmSelectFormDt.Rows[i]["SYSDATETIME"].ToString();
                        }

                        package.SaveAs(new FileInfo(dlg.FileName));
                    }

                }
                catch (Exception ex)
                {
                    AddMessage(ex.Message);
                }
            }
        }
        #endregion
        #region 自定义函数
        private void Init()
        {
            Version = "1.1007";
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
            AlarmStatisticsPageVisibility = "Collapsed";
            AlarmSelectStartDate = DateTime.Now;
            AlarmSelectEndtDate = DateTime.Now;
            AlarmStatictic = new ObservableCollection<AlarmReportFormViewModel>();
            CheckDbButtonIsEnabled = true;
            AlarmReportFormTester= new ObservableCollection<AlarmReportFormViewModel>();
            AlarmReportFormFeeder = new ObservableCollection<AlarmReportFormViewModel>();
            try
            {
                using (StreamReader reader = new StreamReader(System.IO.Path.Combine(System.Environment.CurrentDirectory, "AlarmReportForm.json")))
                {
                    string json = reader.ReadToEnd();
                    AlarmReportForm = JsonConvert.DeserializeObject<ObservableCollection<AlarmReportFormViewModel>>(json);
                    AlarmReportFormExecute();
                }
            }
            catch (Exception ex)
            {
                AlarmReportForm = new ObservableCollection<AlarmReportFormViewModel>();
                WriteToJson(AlarmReportForm, System.IO.Path.Combine(System.Environment.CurrentDirectory, "AlarmReportForm.json"));
                AddMessage(ex.Message);
            }

            //try
            //{
            //    using (StreamReader reader = new StreamReader(System.IO.Path.Combine(System.Environment.CurrentDirectory, "MachineStateA.json")))
            //    {
            //        string json = reader.ReadToEnd();
            //        MachineStateA = JsonConvert.DeserializeObject<MachineStateViewModel>(json);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MachineStateA = new MachineStateViewModel()
            //    {
            //        DaiLiao = 0,
            //        HuanMo = 0,
            //        YangBen = 0,
            //        TesterAlarm = 0,
            //        Down = 0,
            //        UploaderAlarm = 0,
            //        Run = 0
            //    };
            //    WriteToJson(MachineStateA, System.IO.Path.Combine(System.Environment.CurrentDirectory, "MachineStateA.json"));
            //    AddMessage(ex.Message);
            //}
            //try
            //{
            //    using (StreamReader reader = new StreamReader(System.IO.Path.Combine(System.Environment.CurrentDirectory, "MachineStateB.json")))
            //    {
            //        string json = reader.ReadToEnd();
            //        MachineStateB = JsonConvert.DeserializeObject<MachineStateViewModel>(json);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MachineStateB = new MachineStateViewModel()
            //    {
            //        DaiLiao = 0,
            //        HuanMo = 0,
            //        YangBen = 0,
            //        TesterAlarm = 0,
            //        Down = 0,
            //        UploaderAlarm = 0,
            //        Run = 0
            //    };
            //    WriteToJson(MachineStateB, System.IO.Path.Combine(System.Environment.CurrentDirectory, "MachineStateB.json"));
            //    AddMessage(ex.Message);
            //}
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
            int count1 = 0, oldMinute = -1, count2 = 0;
            string CurrentAlarmM1 = "", CurrentAlarmM2_1 = "", CurrentAlarmM2_2 = "", CurrentAlarmM3_1 = "", CurrentAlarmM3_2 = "";
            bool isRecord = false;
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
                #region 刷卡
                if (count1++ > 4)
                {
                    count1 = 0;
                   
                    await Task.Run(() =>
                    {
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
                    M300 = await Task<bool[]>.Run(() => {return fx5U.ReadMultiM("M3200", 800); }); 
                    if (M300 != null && StatusPLC)
                    {
                        for (int i = 0; i < AlarmList.Count; i++)
                        {
                            if (M300[i] != AlarmList[i].State && AlarmList[i].Content != "Null")
                            {
                                AlarmList[i].State = M300[i];

                                if (AlarmList[i].State)
                                {
                                    isRecord = false;
                                    if (i < 50)//上料机
                                    {
                                        if (CurrentAlarmM1 != AlarmList[i].Content)
                                        {
                                            CurrentAlarmM1 = AlarmList[i].Content;
                                            isRecord = true;
                                        }
                                    }
                                    else
                                    {
                                        if (i < 290)//撕膜A
                                        {
                                            if (CurrentAlarmM2_1 != AlarmList[i].Content)
                                            {
                                                CurrentAlarmM2_1 = AlarmList[i].Content;
                                                isRecord = true;
                                            }
                                        }
                                        else
                                        {
                                            if (i < 490)//撕膜B
                                            {
                                                if (CurrentAlarmM2_2 != AlarmList[i].Content)
                                                {
                                                    CurrentAlarmM2_2 = AlarmList[i].Content;
                                                    isRecord = true;
                                                }
                                            }
                                            else
                                            {
                                                if (i < 640)//贴膜A
                                                {
                                                    if (CurrentAlarmM3_1 != AlarmList[i].Content)
                                                    {
                                                        CurrentAlarmM3_1 = AlarmList[i].Content;
                                                        isRecord = true;
                                                    }
                                                }
                                                else //贴膜B
                                                {
                                                    if (CurrentAlarmM3_2 != AlarmList[i].Content)
                                                    {
                                                        CurrentAlarmM3_2 = AlarmList[i].Content;
                                                        isRecord = true;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    AlarmList[i].Start = DateTime.Now;
                                    AlarmList[i].End = DateTime.Now;
                                    AddMessage(AlarmList[i].Code + AlarmList[i].Content + "发生");

                                    if (isRecord)
                                    {
                                        var nowAlarm = AlarmReportForm.FirstOrDefault(s => s.Code == AlarmList[i].Code);
                                        if (nowAlarm == null)
                                        {
                                            AlarmReportFormViewModel newAlarm = new AlarmReportFormViewModel()
                                            {
                                                Code = AlarmList[i].Code,
                                                Content = AlarmList[i].Content,
                                                Count = 1,
                                                TimeSpan = AlarmList[i].End - AlarmList[i].Start
                                            };

                                            //Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
                                            //{

                                            AlarmReportForm.Add(newAlarm);
                                            //}));                                            
                                        }
                                        else
                                        {
                                            nowAlarm.Count++;
                                        }
                                        AlarmReportFormExecute();
                                        WriteToJson(AlarmReportForm, System.IO.Path.Combine(System.Environment.CurrentDirectory, "AlarmReportForm.json"));

                                        string banci = GetBanci();
                                        if (!File.Exists(Path.Combine("D:\\报警记录", "VPP报警记录" + banci + ".csv")))
                                        {
                                            string[] heads = new string[] { "时间", "内容" };
                                            Csvfile.savetocsv(Path.Combine("D:\\报警记录", "VPP报警记录" + banci + ".csv"), heads);
                                        }
                                        string[] conts = new string[] { AlarmList[i].Start.ToString(), AlarmList[i].Content };
                                        Csvfile.savetocsv(Path.Combine("D:\\报警记录", "VPP报警记录" + banci + ".csv"), conts);

                                        #region 上传
                                        await Task.Run(()=> {
                                            string Banci = (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20) ? "D" : "N";
                                            SXJ.Mysql mysql = new SXJ.Mysql();
                                            if (mysql.Connect())
                                            {
                                                string stm = "insert into TED_WARN_DATA (WORKSTATION,PARTNUM,MACID,LOADID,PETID,TDATE,TTIME,CLASS,WARNID,DETAILID,WARNNUM,FL01,FL02,FL03,FL04,FL05,FL06,FL07,FL08,FL09,FL10,SUPPLIER,WARNVER) value('" + TestStation + "','" + ProgramName + "','" + MachineNumber + "','" + MachineNumber + "','','" + DateTime.Now.ToString("yyyyMMdd") + "','" + DateTime.Now.ToString("HHmmss") + "','" + Banci + "','" + AlarmList[i].Content + "','','1','','','','','','','','','','','" + Supplier + "','" + WARNVER + "')";
                                                mysql.executeQuery(stm);
                                                stm = $"INSERT INTO LDR_WARN_DATA (WORKSTATION, MACID, WARNID, DETAILID, STARTTIME, SUPPLIER) VALUES ('{TestStation}', '{MachineNumber}', '{AlarmList[i].Code}', '{AlarmList[i].Content}', '{AlarmList[i].Start}', '{Supplier}')";
                                                mysql.executeQuery(stm);
                                            }
                                            mysql.DisConnect();
                                        });

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
                                        //nowAlarm.Count++;
                                        nowAlarm.TimeSpan += AlarmList[i].End - AlarmList[i].Start;
                                        AlarmReportFormExecute();
                                        WriteToJson(AlarmReportForm, System.IO.Path.Combine(System.Environment.CurrentDirectory, "AlarmReportForm.json"));
                                    }
                                    #region 上传
                                    await Task.Run(() => {
                                        SXJ.Mysql mysql = new SXJ.Mysql();
                                        if (mysql.Connect())
                                        {
                                            string stm = $"UPDATE LDR_WARN_DATA SET ENDTIME = '{AlarmList[i].End}' WHERE WORKSTATION = '{TestStation}' AND MACID = '{MachineNumber}' AND STARTTIME = '{AlarmList[i].Start}'";
                                            mysql.executeQuery(stm);
                                        }
                                        mysql.DisConnect();
                                    });

                                    #endregion

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
                        AlarmReportForm.Clear();
                        AlarmReportFormExecute();
                        WriteToJson(AlarmReportForm, System.IO.Path.Combine(System.Environment.CurrentDirectory, "AlarmReportForm.json"));
                        
                        //WriteStatetoExcel(Path.Combine("D:\\报警记录", "VPP时间统计" + LastBanci + ".xlsx"));
                        //MachineStateA.Clean();
                        //WriteToJson(MachineStateA, System.IO.Path.Combine(System.Environment.CurrentDirectory, "MachineStateA.json"));
                        //MachineStateB.Clean();
                        //WriteToJson(MachineStateB, System.IO.Path.Combine(System.Environment.CurrentDirectory, "MachineStateB.json"));

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
                #region 妥善率
                if (count2++ > 4)
                {
                    count2 = 0;
                    try
                    {
                        PcsCout = fx5U.ReadDW("D6500");
                        int _AlarmCount = 0;
                        foreach (var item in AlarmReportForm)
                        {
                            _AlarmCount += item.Count;
                        }
                        AlarmCout = _AlarmCount;
                        if (PcsCout == 0)
                        {
                            PassRadio = 100;
                        }
                        else
                        {
                            PassRadio = Math.Round((1 - (double)_AlarmCount / PcsCout) * 100, 1);
                        }
                        
                    }
                    catch { }
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
                    D301 = fx5U.ReadW("D301");
                    D302 = fx5U.ReadW("D302");
                }
                catch { System.Threading.Thread.Sleep(10000); }
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
                    var ws = package.Workbook.Worksheets.Add("上料机报警");
                    var ws1 = package.Workbook.Worksheets.Add("测试机报警");
                    ws.Cells[1, 1].Value = "ID";
                    ws.Cells[1, 2].Value = "报警内容";
                    ws.Cells[1, 3].Value = "报警次数";
                    ws.Cells[1, 4].Value = "报警时长(分钟)";
                    ws.Cells[1, 5].Value = DateTime.Now.ToString();
                    for (int i = 0; i < AlarmReportFormFeeder.Count; i++)
                    {
                        ws.Cells[i + 2, 1].Value = AlarmReportFormFeeder[i].Code;
                        ws.Cells[i + 2, 2].Value = AlarmReportFormFeeder[i].Content;
                        ws.Cells[i + 2, 3].Value = AlarmReportFormFeeder[i].Count;
                        ws.Cells[i + 2, 4].Value = Math.Round(AlarmReportFormFeeder[i].TimeSpan.TotalMinutes, 1);
                    }

                    ws1.Cells[1, 1].Value = "ID";
                    ws1.Cells[1, 2].Value = "报警内容";
                    ws1.Cells[1, 3].Value = "报警次数";
                    ws1.Cells[1, 4].Value = "报警时长(分钟)";
                    ws1.Cells[1, 5].Value = DateTime.Now.ToString();
                    for (int i = 0; i < AlarmReportFormTester.Count; i++)
                    {
                        ws1.Cells[i + 2, 1].Value = AlarmReportFormTester[i].Code;
                        ws1.Cells[i + 2, 2].Value = AlarmReportFormTester[i].Content;
                        ws1.Cells[i + 2, 3].Value = AlarmReportFormTester[i].Count;
                        ws1.Cells[i + 2, 4].Value = Math.Round(AlarmReportFormTester[i].TimeSpan.TotalMinutes, 1);
                    }


                    package.SaveAs(new FileInfo(filepath));
                }

            }
            catch (Exception ex)
            {
                AddMessage(ex.Message);
            }

        }
        private void WriteStatetoExcel(string filepath)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("MySheet");
                    ws.Cells[1, 1].Value = "A";
                    ws.Cells[1, 3].Value = DateTime.Now.ToString();
                    ws.Cells[2, 1].Value = "项目";
                    ws.Cells[2, 2].Value = "时间(单位min)";
                    ws.Cells[3, 1].Value = "待料";
                    ws.Cells[3, 2].Value = Math.Round(MachineStateA.DaiLiao, 1);
                    ws.Cells[4, 1].Value = "换膜";
                    ws.Cells[4, 2].Value = Math.Round(MachineStateA.HuanMo, 1);
                    ws.Cells[5, 1].Value = "样本";
                    ws.Cells[5, 2].Value = Math.Round(MachineStateA.YangBen, 1);
                    ws.Cells[6, 1].Value = "测试机报警";
                    ws.Cells[6, 2].Value = Math.Round(MachineStateA.TesterAlarm, 1);
                    ws.Cells[7, 1].Value = "故障";
                    ws.Cells[7, 2].Value = Math.Round(MachineStateA.Down, 1);
                    ws.Cells[8, 1].Value = "上料机报警";
                    ws.Cells[8, 2].Value = Math.Round(MachineStateA.UploaderAlarm, 1);
                    ws.Cells[9, 1].Value = "机台运行";
                    ws.Cells[9, 2].Value = Math.Round(MachineStateA.Run, 1);

                    ws.Cells[11, 1].Value = "B";
                    ws.Cells[11, 3].Value = DateTime.Now.ToString();
                    ws.Cells[12, 1].Value = "项目";
                    ws.Cells[12, 2].Value = "时间(单位min)";
                    ws.Cells[13, 1].Value = "待料";
                    ws.Cells[13, 2].Value = Math.Round(MachineStateB.DaiLiao, 1);
                    ws.Cells[14, 1].Value = "换膜";
                    ws.Cells[14, 2].Value = Math.Round(MachineStateB.HuanMo, 1);
                    ws.Cells[15, 1].Value = "样本";
                    ws.Cells[15, 2].Value = Math.Round(MachineStateB.YangBen, 1);
                    ws.Cells[16, 1].Value = "测试机报警";
                    ws.Cells[16, 2].Value = Math.Round(MachineStateB.TesterAlarm, 1);
                    ws.Cells[17, 1].Value = "故障";
                    ws.Cells[17, 2].Value = Math.Round(MachineStateB.Down, 1);
                    ws.Cells[18, 1].Value = "上料机报警";
                    ws.Cells[18, 2].Value = Math.Round(MachineStateB.UploaderAlarm, 1);
                    ws.Cells[19, 1].Value = "机台运行";
                    ws.Cells[19, 2].Value = Math.Round(MachineStateB.Run, 1);

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
