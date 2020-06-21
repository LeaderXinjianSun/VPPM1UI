using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BingLibrary.hjb.net;
using BingLibrary.hjb.file;
using System.IO;
using BingLibrary.hjb;
using System.Data;
using BingLibrary.Net.net;
using OfficeOpenXml;

namespace SXJLibrary
{
    public class EpsonRC90
    {
        #region 变量
        public bool[] Rc90In = new bool[200];
        public bool[] Rc90Out = new bool[200];
        public TcpIpClient IOReceiveNet = new TcpIpClient();
        public TcpIpClient TestSentNet = new TcpIpClient();
        public TcpIpClient TestReceiveNet = new TcpIpClient();
        public TcpIpClient TestReceiveNet1 = new TcpIpClient();
        string Ip = "192.168.1.2";
        public bool IOReceiveStatus = false, TestSendStatus = false, TestReceiveStatus = false, TestReceive1Status = false;
        string iniParameterPath = System.Environment.CurrentDirectory + "\\Parameter.ini";
        string iniFilepath = @"d:\test.ini";
        public string[] BordBarcode = new string[2] { "Null", "Null" };
        public ProducInfo[][] BarInfo = new ProducInfo[2][] { new ProducInfo[15], new ProducInfo[15] };
        public Tester[] YanmadeTester = new Tester[4];
        public UploadSoftwareStatus[] uploadSoftwareStatus = new UploadSoftwareStatus[4];
        public string[] TemporaryBordBarcode = new string[2] { "Null", "Null" };
        public string[][] sampleContent = new string[8][] { new string[4], new string[4], new string[4], new string[4], new string[4], new string[4], new string[4], new string[4] };
        public DateTime SamStart;
        public ExcelPackage Package;
        public ExcelWorksheet Worksheet;
        public bool MaterialFileStatus = false;
        #endregion
        #region 事件
        public delegate void PrintEventHandler(string ModelMessageStr);
        public event PrintEventHandler ModelPrint;
        #endregion
        #region 构造函数
        public EpsonRC90()
        {
            for (int i = 0; i < 4; i++)
            {
                YanmadeTester[i] = new Tester(i + 1);
                uploadSoftwareStatus[i] = new UploadSoftwareStatus(i + 1);
                uploadSoftwareStatus[i].ModelPrint += uploadprint;
                uploadSoftwareStatus[i].RecordPrint += RecordPrintOperate;
            }
            Ip = Inifile.INIGetStringValue(iniParameterPath, "EpsonRC90", "Ip", "192.168.1.2");

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    BarInfo[i][j] = new ProducInfo();
                    BarInfo[i][j].Barcode = "FAIL";
                    BarInfo[i][j].BordBarcode = "Null";
                    BarInfo[i][j].Status = 0;
                    BarInfo[i][j].TDate = DateTime.Now.ToString("yyyyMMdd");
                    BarInfo[i][j].TTime = DateTime.Now.ToString("HHmmss");
                }
            }
            SamStart = DateTime.Now;
            Run();
        }
        #endregion
        #region 机械手通讯      
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
                        bool r1 = await IOReceiveNet.Connect(Ip, 2000);
                        if (r1)
                        {
                            IOReceiveStatus = true;
                            ModelPrint("机械手IOReceiveNet连接");

                        }
                        else
                            IOReceiveStatus = false;
                    }
                }
                else
                { await Task.Delay(15000); }
            }
        }
        public async void checkTestSentNet()
        {
            while (true)
            {
                await Task.Delay(400);
                if (!TestSentNet.tcpConnected)
                {
                    await Task.Delay(1000);
                    if (!TestSentNet.tcpConnected)
                    {
                        bool r1 = await TestSentNet.Connect(Ip, 2002);
                        if (r1)
                        {
                            TestSendStatus = true;
                            ModelPrint("机械手TestSentNet连接");
                        }
                        else
                            TestSendStatus = false;
                    }
                }
                else
                {
                    await Task.Delay(15000);
                    TestSentNet.IsOnline();
                    if (!TestSentNet.tcpConnected)
                        ModelPrint("机械手TestSentNet断开");
                }
            }
        }
        public async void checkTestReceiveNet()
        {
            while (true)
            {
                await Task.Delay(400);
                if (!TestReceiveNet.tcpConnected)
                {
                    await Task.Delay(1000);
                    if (!TestReceiveNet.tcpConnected)
                    {
                        bool r1 = await TestReceiveNet.Connect(Ip, 2001);
                        if (r1)
                        {
                            TestReceiveStatus = true;
                            ModelPrint("机械手TestReceiveNet连接");
                        }
                        else
                            TestReceiveStatus = false;
                    }
                }
                else
                { await Task.Delay(15000); }
            }
        }
        public async void checkTestReceiveNet1()
        {
            while (true)
            {
                await Task.Delay(400);
                if (!TestReceiveNet1.tcpConnected)
                {
                    await Task.Delay(1000);
                    if (!TestReceiveNet1.tcpConnected)
                    {
                        bool r1 = await TestReceiveNet1.Connect(Ip, 2003);
                        if (r1)
                        {
                            TestReceive1Status = true;
                            ModelPrint("机械手TestReceiveNet1连接");
                        }
                        else
                            TestReceive1Status = false;
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
                        ModelPrint("机械手IOReceiveNet断开");
                    }
                    else
                    {
                        string[] strs = s.Split(',');
                        if (strs[0] == "IOCMD" && strs[1].Length == 200)
                        {
                            for (int i = 0; i < 200; i++)
                            {
                                Rc90Out[i] = strs[1][i] == '1' ? true : false;
                            }
                            string RsedStr = "";
                            for (int i = 0; i < 200; i++)
                            {
                                RsedStr += Rc90In[i] ? "1" : "0";
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
        private async void TestRevAnalysis()
        {
            while (true)
            {
                if (TestReceiveStatus == true)
                {
                    string s = await TestReceiveNet.ReceiveAsync();

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
                        TestReceiveNet.tcpConnected = false;
                        TestReceiveStatus = false;
                        ModelPrint("机械手TestReceiveNet断开");
                    }
                    else
                    {
                        ModelPrint("TestRev: " + s);
                        try
                        {
                            string[] strs = s.Split(';');
                            switch (strs[0])
                            {
                                case "ReleaseA"://产品号(0-14);产品1状态(3-5)
                                    SaveRelease(0, strs);
                                    break;
                                case "ReleaseB":
                                    SaveRelease(1, strs);
                                    break;
                                case "TestResultCount":
                                    TestResult tr = strs[1] == "OK" ? TestResult.Pass : TestResult.Ng;
                                    YanmadeTester[int.Parse(strs[2]) - 1].Update(tr);
                                    break;
                                case "Start":
                                    YanmadeTester[int.Parse(strs[1]) - 1].Start(TestFinishOperate);
                                    break;
                                case "Finish":
                                    YanmadeTester[int.Parse(strs[1]) - 1].TestResult = strs[2] == "1" ? TestResult.Pass : TestResult.Ng;
                                    YanmadeTester[int.Parse(strs[1]) - 1].TestStatus = TestStatus.Tested;
                                    if (MaterialFileStatus)
                                    {
                                        try
                                        {
                                            int index = int.Parse(strs[1]) - 1;
                                            Worksheet.Cells[index * 2 + 3, 6].Value = Convert.ToInt32(Worksheet.Cells[index * 2 + 3, 6].Value) + 1;
                                            Worksheet.Cells[index * 2 + 1 + 3, 6].Value = Convert.ToInt32(Worksheet.Cells[index * 2 + 1 + 3, 6].Value) + 1;
                                            Package.Save();
                                        }
                                        catch (Exception ex)
                                        {
                                            ModelPrint(ex.Message);
                                        }
                                    }
                                    break;
                                case "CheckSample":
                                    CheckSam();
                                    break;
                                case "PickNew":
                                    if (MaterialFileStatus)
                                    {
                                        try
                                        {
                                            Worksheet.Cells[11, 6].Value = Convert.ToInt32(Worksheet.Cells[11, 6].Value) + 1;
                                            Worksheet.Cells[12, 6].Value = Convert.ToInt32(Worksheet.Cells[12, 6].Value) + 1;
                                            Package.Save();
                                        }
                                        catch (Exception ex)
                                        {
                                            ModelPrint(ex.Message);
                                        }
                                    }
                                    break;
                                case "LinkNG":
                                    break;
                                case "StartSample":
                                    break;
                                case "EndClean":
                                    break;
                                case "CheckUploadStatus":
                                    string uploadrst = "OK";
                                    for (int i = 0; i < 4; i++)
                                    {
                                        if (!uploadSoftwareStatus[i].status)
                                        {
                                            uploadrst = "NG";
                                            break;
                                        }
                                    }
                                    await TestSentNet.SendAsync("UploadStatus;" + uploadrst);
                                    break;
                                case "CheckMaterial":
                                    if (MaterialFileStatus)
                                    {
                                        string material = "OK";
                                        for (int i = 3; i <= Worksheet.Dimension.End.Row; i++)
                                        {
                                            try
                                            {
                                                if (Convert.ToInt32(Worksheet.Cells[i, 6].Value) > Convert.ToInt32(Worksheet.Cells[i, 4].Value))
                                                {
                                                    ModelPrint((string)Worksheet.Cells[i, 1].Value + "," + (string)Worksheet.Cells[i, 3].Value + " 使用寿命到达上限");
                                                    material = "NG";
                                                    break;
                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(Worksheet.Cells[i, 6].Value) > Convert.ToInt32(Worksheet.Cells[i, 5].Value))
                                                    {
                                                        ModelPrint((string)Worksheet.Cells[i, 1].Value + "," + (string)Worksheet.Cells[i, 3].Value + " 使用寿命预警");
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ModelPrint(ex.Message);
                                            }

                                        }
                                        await TestSentNet.SendAsync("CheckMaterial;" + material);
                                        ModelPrint("CheckMaterial;" + material);
                                    }
                                    else
                                    {
                                        await TestSentNet.SendAsync("CheckMaterial;NG");
                                        ModelPrint("CheckMaterial;NG");
                                    }
                                    break;
                                default:
                                    ModelPrint("无效指令： " + s);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            ModelPrint(ex.Message);
                        }
                    }
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }
        private async void TestRev1Analysis()
        {
            while (true)
            {
                if (TestReceive1Status == true)
                {
                    string s = await TestReceiveNet1.ReceiveAsync();

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
                        TestReceiveNet1.tcpConnected = false;
                        TestReceive1Status = false;
                        ModelPrint("机械手TestReceiveNet1断开");
                    }
                    else
                    {
                        ModelPrint("TestRev1: " + s);
                        try
                        {
                            string[] strs = s.Split(';');
                            switch (strs[0])
                            {
                                case "Start":
                                    YanmadeTester[int.Parse(strs[1]) - 1].Start(TestFinishOperate);
                                    break;
                                case "Finish":
                                    YanmadeTester[int.Parse(strs[1]) - 1].TestResult = strs[2] == "1" ? TestResult.Pass : TestResult.Ng;
                                    YanmadeTester[int.Parse(strs[1]) - 1].TestStatus = TestStatus.Tested;
                                    break;
                                default:
                                    ModelPrint("无效指令： " + s);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            ModelPrint(ex.Message);
                        }
                    }
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }
        #endregion
        #region 功能函数
        void Run()
        {
            checkIOReceiveNet();
            checkTestSentNet();
            checkTestReceiveNet();
            checkTestReceiveNet1();
            IORevAnalysis();
            TestRevAnalysis();
            TestRev1Analysis();
        }
        async void CheckSam()
        {
            try
            {
                int ngItemCount = int.Parse(Inifile.INIGetStringValue(iniParameterPath, "Sample", "NGItemCount", "3"));
                int nGItemLimit = int.Parse(Inifile.INIGetStringValue(iniParameterPath, "Sample", "NGItemLimit", "99")); 
                string MNO = Inifile.INIGetStringValue(iniParameterPath, "BigData", "MACID", "X1621_1");
                Oracle oraDB = new Oracle("qddb04.eavarytech.com", "mesdb04", "ictdata", "ictdata*168");
                if (oraDB.isConnect())
                {
                    for (int i = 0; i < ngItemCount; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            string flexid = Inifile.INIGetStringValue(iniFilepath, "A", "id" + (j + 1).ToString(), "99999");
                            string ngitem = Inifile.INIGetStringValue(iniParameterPath, "Sample", "NGItem" + i.ToString(), "Null");
                            string stm = String.Format("Select * from fluke_data WHERE FL04 = '{0}' AND FL01 = '{1}' AND ITSDATE = '{2}' ORDER BY ITSDATE DESC, ITSTIME DESC", flexid, ngitem, DateTime.Now.ToString("yyyyMMdd"));
                            DataSet s = oraDB.executeQuery(stm);
                            DataTable dt = s.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                string datestr = (string)dt.Rows[0]["ITSDATE"];
                                string timestr = (string)dt.Rows[0]["ITSTIME"];
                                if (datestr.Length == 8 && (timestr.Length == 5 || timestr.Length == 6))
                                {
                                    if (timestr.Length == 5)
                                    {
                                        timestr = "0" + timestr;
                                    }
                                    string datetimestr = string.Empty;
                                    datetimestr = string.Format("{0}/{1}/{2} {3}:{4}:{5}", datestr.Substring(0, 4), datestr.Substring(4, 2), datestr.Substring(6, 2), timestr.Substring(0, 2), timestr.Substring(2, 2), timestr.Substring(4, 2));
                                    DateTime updatetime = Convert.ToDateTime(datetimestr);
                                    TimeSpan sp = updatetime - SamStart;
                                    if (sp.TotalSeconds > 0)
                                    {
                                        stm = String.Format("Select * from barsaminfo WHERE BARCODE = '{0}'", (string)dt.Rows[0]["BARCODE"]);
                                        DataSet s1 = oraDB.executeQuery(stm);
                                        DataTable dt1 = s1.Tables[0];
                                        if (dt1.Rows.Count > 0)
                                        {
                                            try
                                            {
                                                //插入样本记录
                                                string parnum = Inifile.INIGetStringValue(iniFilepath, "Other", "pn", "FHAPHS9");
                                                string tres = ngitem.Length > 20 ? ngitem.Substring(0, 20) : ngitem;
                                                stm = String.Format("INSERT INTO BARSAMREC (PARTNUM,SITEM,BARCODE,NGITEM,TRES,MNO,CDATE,CTIME,SR01) VALUES ('{0}','FLUKE','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", parnum, (string)dt.Rows[0]["BARCODE"], (string)dt1.Rows[0]["NGITEM"], tres, MNO, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"), flexid);                                                
                                                await Task.Run(()=> { oraDB.executeNonQuery(stm); });
                                                string filepath = "D:\\样本记录\\样本记录" + GetBanci() + ".csv";
                                                if (!Directory.Exists("D:\\样本记录"))
                                                {
                                                    Directory.CreateDirectory("D:\\样本记录");
                                                }
                                                if (!File.Exists(filepath))
                                                {
                                                    string[] heads = { "DateTime", "PARTNUM", "SITEM", "BARCODE", "NGITEM", "TRES", "MNO", "CDATE", "CTIME", "SR01" };
                                                    Csvfile.savetocsv(filepath, heads);
                                                }
                                                string[] conte = { System.DateTime.Now.ToString(), parnum, "FLUKE", (string)dt.Rows[0]["BARCODE"], (string)dt1.Rows[0]["NGITEM"], tres, MNO, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"), flexid };
                                                Csvfile.savetocsv(filepath, conte);
                                                stm = String.Format("Select * from BARSAMREC WHERE BARCODE = '{0}'", (string)dt.Rows[0]["BARCODE"]);
                                                DataSet samtimesds = await Task.Run<DataSet>(()=> { return oraDB.executeQuery(stm); }); 
                                                ModelPrint("插入样本记录 " + (string)dt.Rows[0]["BARCODE"] + " " + samtimesds.Tables[0].Rows.Count.ToString());
                                                if (samtimesds.Tables[0].Rows.Count > nGItemLimit)
                                                {
                                                    ModelPrint((string)dt.Rows[0]["BARCODE"] + "样本记录" + samtimesds.Tables[0].Rows.Count.ToString() + " > " + nGItemLimit.ToString());
                                                    sampleContent[i][j] = "Limit";
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ModelPrint(ex.Message);
                                            }
                                            if (((string)dt1.Rows[0]["NGITEM"]).ToUpper() == ngitem.ToUpper())
                                            {
                                                sampleContent[i][j] = "ok";
                                            }
                                            else
                                            {
                                                sampleContent[i][j] = (string)dt1.Rows[0]["NGITEM"];
                                            }
                                        }
                                        else
                                        {
                                            sampleContent[i][j] = "NoSam";
                                        }
                                    }
                                    else
                                    {
                                        sampleContent[i][j] = "NotNew";
                                    }
                                }
                                else
                                {
                                    ModelPrint("时间格式错误");
                                    sampleContent[i][j] = "Error";
                                }
                            }
                            else
                            {
                                sampleContent[i][j] = "NoRecord";
                            }
                        }

                    }
                    //回复样本结果
                    
                    bool resut = true;
                    for (int i = 0; i < ngItemCount; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (sampleContent[i][j] != "ok")
                            {
                                string resultString = "RestartSample;" + j.ToString() + ";" + i.ToString();
                                if (TestSendStatus)
                                {
                                    ModelPrint(resultString);
                                    await TestSentNet.SendAsync(resultString);
                                }
                                resut = false;
                            }
                        }
                    }
                    if (resut)
                    {
                        ModelPrint("EndSample");
                        await TestSentNet.SendAsync("EndSample");
                    }
                }
                else
                {
                    ModelPrint("样本查询Error:数据库连接失败");
                }
                oraDB.disconnect();
            }
            catch(Exception ex)
            {
                ModelPrint("样本查询Error:" + ex.Message);
            }
        }
        public void ResetBord(int index)
        {
            for (int i = 0; i < 15; i++)
            {
                BarInfo[index][i].Barcode = "FAIL";
                BarInfo[index][i].BordBarcode = BordBarcode[index];
                BarInfo[index][i].Status = 0;
                BarInfo[index][i].TDate = DateTime.Now.ToString("yyyyMMdd");
                BarInfo[index][i].TTime = DateTime.Now.ToString("HHmmss");
                string machinestr = Inifile.INIGetStringValue(iniParameterPath, "BigData", "MACID", "X1621_1");
                Mysql mysql = new Mysql();
                if (mysql.Connect())
                {
                    string stm = "INSERT INTO BARBIND (MACHINE,SCBARCODE,SCBODBAR,SDATE,STIME,PCSSER,RESULT) VALUES ('" + machinestr + "','" + BarInfo[index][i].Barcode + "','"
                                    + BordBarcode[index] + "','" + BarInfo[index][i].TDate + "','" + BarInfo[index][i].TTime + "','" + (i + 1).ToString() + "','" + BarInfo[index][i].Status.ToString() + "')";
                    mysql.executeQuery(stm);
                    mysql.executeQuery("COMMIT");
                }
                mysql.DisConnect();
            }
        }
        //放料到载盘；条码从夹爪转移到载盘
        async void SaveRelease(int _index,string[] rststr)
        {
            int index = int.Parse(rststr[1]);
            await Task.Run(() =>
            {
                Mysql mysql = new Mysql();
                if (mysql.Connect())
                {
                    string stm = "UPDATE BARBIND SET RESULT = '" + rststr[2] + "' WHERE SCBARCODE = '" + BarInfo[_index][index - 1].Barcode + "' AND SCBODBAR = '" + BarInfo[_index][index - 1].BordBarcode
                        + "' AND SDATE = '" + BarInfo[_index][index - 1].TDate + "' AND STIME = '" + BarInfo[_index][index - 1].TTime + "' AND PCSSER = '" + index.ToString() + "'";
                    mysql.executeQuery(stm); 
                }
                mysql.DisConnect();
            });
        }
        private void TestFinishOperate(int index)
        {
            uploadSoftwareStatus[index - 1].testerCycle = YanmadeTester[index - 1].TestSpan.ToString();
            uploadSoftwareStatus[index - 1].result = YanmadeTester[index - 1].TestResult == TestResult.Pass ? "PASS" : "FAIL";
            if (YanmadeTester[index - 1].TestSpan > 11 && uploadSoftwareStatus[index - 1].result == "PASS")
            {
                uploadSoftwareStatus[index - 1].StartCommand();
            }
            else
            {
                uploadSoftwareStatus[index - 1].StopCommand();
            }
        }
        private void uploadprint(string str)
        {
            ModelPrint(str);
        }
        private void RecordPrintOperate(int index, string bar, string rst, string cyc, bool isRecord)
        {
            SaveCSVfileRecord(DateTime.Now.ToString(), bar, rst, cyc + " s", index.ToString());
            if (isRecord && !Tester.IsInSampleMode && !Tester.IsInGRRMode)
            {
                if (YanmadeTester[index - 1].TestSpan > 5)
                {
                    YanmadeTester[index - 1].UpdateNormalWithTestTimes(rst);
                }
                else
                {
                        ModelPrint(bar + " 测试时间小于5秒，不纳入良率统计");
                }
            }
            else
            {
                if (!isRecord && !Tester.IsInSampleMode && !Tester.IsInGRRMode)
                {
                    ModelPrint(bar + " 测试次数大于3次，不纳入良率统计");
                }
            }
        }
        private void SaveCSVfileRecord(string TestTime, string Barcode, string TestResult, string TestCycleTime, string Index)
        {
            string filepath = "D:\\生产记录\\生产记录" + GetBanci() + ".csv";
            if (!Directory.Exists("D:\\生产记录"))
            {
                Directory.CreateDirectory("D:\\生产记录");
            }
            try
            {
                if (!File.Exists(filepath))
                {
                    string[] heads = { "Time", "Barcode", "Result", "Cycle", "Index" };
                    Csvfile.savetocsv(filepath, heads);
                }
                string[] conte = { TestTime, Barcode, TestResult, TestCycleTime, Index };
                Csvfile.savetocsv(filepath, conte);
            }
            catch (Exception ex)
            {
                ModelPrint(ex.Message);
            }
        }
        public string GetBanci()
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
        #endregion
    }
    public class ProducInfo
    {
        //条码 板条码 产品状态 日期 时间
        public string Barcode { set; get; }
        public string BordBarcode { set; get; }
        public int Status { set; get; }
        public string TDate { set; get; }
        public string TTime { set; get; }
    }
}
