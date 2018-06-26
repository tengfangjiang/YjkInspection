﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Timers;
using System.IO;

namespace YjkInspectClient
{
    public partial class MainForm : Form
    {
        private bool isClose = true;
        private int iRetUSB = 0, iRetCOM = 0;
        private PeopleInfo people = new PeopleInfo();
        private System.Timers.Timer _readCardInfoTimer; // 定时处理任务
        //更新窗体委托
        private loghandler loghandlers;

        private const int TYPE_STATUS_LABEL = 1; // 更新状态栏
        private const int TYPE_FILL_DATA = 2; // 更新数据填充
        private const int TYPE_WRITE_HISTORY = 3; // 更新历史纪录（写文件）
        private const int TYPE_WRITE_ID = 4; // 更新 当前 ID (写文件）

        private const string HISTORY_DIR = "HistoryRecord";

        private int serialId = 1;

        public delegate void loghandler(string Str, int msgType);

        public MainForm()
        {
            InitializeComponent();
        }

        #region 示例代码
        private void button1_Click(object sender, EventArgs e)
        {
            TSCSDK.openport("Gprinter  GP-3120TN");                                           //Open specified printer driver
                                                                                              // setup("35", "25", "4", "8", "0", "1", "0");                          　　 //Setup the media size and sensor type info            
            TSCSDK.sendcommand("SIZE 30mm,37mm");
            TSCSDK.clearbuffer();
            TSCSDK.sendcommand("SET PEEL ON ");
            TSCSDK.sendcommand(" SET   TEAR   ON ");
            TSCSDK.sendcommand(" SET   STRIPPER   ON ");
            // sendcommand("HOME");
            //Clear image buffer
            //  barcode("10", "50", "128", "50", "1", "0", "2", "2", "010101"); //Drawing barcode
            // printerfont("30", "25", "3", "0", "1", "1", "158528552　");        //Drawing printer font
            TSCSDK.windowsfont(25, 2, 38, 0, 2, 0, "ARIAL", "贵阳机场中转");
            TSCSDK.windowsfont(42, 38, 35, 0, 2, 0, "ARIAL", "电子消费券");
            TSCSDK.windowsfont(20, 65, 30, 0, 2, 0, "ARIAL", "欢迎选择贵阳中转");

            TSCSDK.windowsfont(15, 90, 30, 0, 2, 0, "ARIAL", "凭密码:12345678"); //我们提供价值48元就餐券
            TSCSDK.windowsfont(15, 115, 24, 0, 0, 0, "ARIAL", "到以下地点消费免48元");  //凭消费密码:1213456789
            TSCSDK.windowsfont(12, 140, 24, 0, 0, 0, "ARIAL", "1.雅品快港自助餐(连廊)");
            TSCSDK.windowsfont(12, 165, 24, 0, 0, 0, "ARIAL", "2.仙姆莱克咖啡(C指廊)");
            TSCSDK.windowsfont(12, 190, 24, 0, 0, 0, "ARIAL", "3.欧迪咖啡厅(C指廊)");
            TSCSDK.windowsfont(12, 215, 22, 0, 0, 0, "ARIAL", "4.雅品茶餐厅(二楼D岛旁)");
            //  windowsfont(180, 175, 24, 0, 0, 0, "ARIAL", "121345678");
            TSCSDK.barcode("40", "240", "128", "30", "1", "0", "2", "2", "12345678");
            //  windowsfont(30, 200, 24, 0, 0, 0, "ARIAL", "4.雅品茶餐厅(二楼D岛旁)");

            //   sendcommand(" SET   STRIPPER   ON ");
            TSCSDK.printlabel("1", "1");                                                    //Print labels
            TSCSDK.closeport();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TSCSDK.openport("Gprinter  GP-1324D");                                           //Open specified printer driver

            TSCSDK.sendcommand("SIZE 60 mm,40 mm");
            TSCSDK.sendcommand("GAP 3");
            TSCSDK.sendcommand("DENSITY 6");
            TSCSDK.sendcommand("SPEED 5");
            TSCSDK.sendcommand("REFERENCE 10,10");
            TSCSDK.sendcommand("CLS");
            //sendcommand("BAR 20,16,1600,5");
            TSCSDK.sendcommand("TEXT 20,16 ,'1',90,2,2,'NO:4872'");
            TSCSDK.barcode("50", "1", "128", "30", "1", "0", "3", "3", "121345678");

            TSCSDK.sendcommand("PRINT 1");
            TSCSDK.closeport();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TSCSDK.openport("Gprinter GP-3120TN");
            TSCSDK.sendcommand("SET PEEL ON ");
            TSCSDK.closeport();
        }
        #endregion

        private void btn_print_Click(object sender, EventArgs e)
        {
            people.addTime = Convert.ToString(DateTime.Now);
            // printBoxCode(people);
            addListItem(people);
            onevent("", TYPE_WRITE_HISTORY);
            serialId++;
            people.serialId = Convert.ToString(serialId);
        }

        private void addListItem(PeopleInfo toAdd)
        {
            lv_print_history.BeginUpdate();
            ListViewItem lvi = new ListViewItem();
            lvi.Text = toAdd.name;
            lvi.SubItems.Add(toAdd.serialId);
            lvi.SubItems.Add(toAdd.addTime);
            lvi.Tag = toAdd.Clone();
            lv_print_history.Items.Add(lvi);
            lv_print_history.EndUpdate();
        }

        private void addListItem(List<PeopleInfo> toAdd)
        {
            lv_print_history.BeginUpdate();
            lv_print_history.Items.Clear();
            foreach (PeopleInfo item in toAdd)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = item.name;
                lvi.SubItems.Add(item.serialId);
                lvi.SubItems.Add(item.addTime);
                lvi.Tag = item.Clone();
                lv_print_history.Items.Add(lvi);
            }
            
            lv_print_history.EndUpdate();
        }

        private void printBoxCode(String boxCode, String barCode, String name, String idCard, String age, String sex, String number)
        {
            TSCSDK.openport("Gprinter  GP-1324D");
            TSCSDK.sendcommand("SIZE 60 mm,40 mm");
            //Open specified printer driver
            //setup("30", "25", "4", "8", "0", "0", "0");                          　　 //Setup the media size and sensor type info            

            TSCSDK.clearbuffer();
            TSCSDK.sendcommand("CLS");

            TSCSDK.sendcommand("QRCODE 300,30,L,6,A,0,\"" + boxCode + "\"");// 打印二维码
            TSCSDK.printerfont("20", "30", "TSS24.BF2", "0", "1", "1", "姓  名: " + name);//姓  名
            TSCSDK.printerfont("20", "65", "TSS24.BF2", "0", "1", "1", "年  龄: " + age + "岁");//年  龄
            TSCSDK.printerfont("20", "100", "TSS24.BF2", "0", "1", "1", "性  别: " + sex);//性  别
            TSCSDK.printerfont("20", "135", "TSS24.BF2", "0", "1", "1", "流水号：" + number);
            TSCSDK.barcode("20", "190", "128", "50", "0", "0", "2", "2", barCode);
            TSCSDK.printerfont("20", "260", "TSS24.BF2", "0", "1", "1", "" + idCard);
            TSCSDK.printlabel("1", "1");                                                    //Print labels
            TSCSDK.closeport();
            toolStripStatusLabel1.Text = "打印";
        }

        private void printBoxCode(PeopleInfo printPeople)
        {
            printBoxCode(printPeople.name, printPeople.cardId, printPeople.name, printPeople.cardId, printPeople.age, printPeople.sex, printPeople.serialId);
        }

        /// <summary>
        /// 初始化当天记录和ID
        /// </summary>
        private void initRecordAndId(string fileName)
        {
            bool isToday = string.IsNullOrEmpty(fileName);
            string historyPath = isToday ? HISTORY_DIR + "/" + DateTime.Now.ToString("yy-MM-dd") + "_history.txt" : HISTORY_DIR + "/" + fileName;
            if (File.Exists(historyPath))
            {
                List<PeopleInfo> infos = new List<PeopleInfo>();
                StreamReader sr = new StreamReader(historyPath, Encoding.UTF8);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    infos.Add(new PeopleInfo(line));
                }
                if (isToday && !string.IsNullOrEmpty(infos.Last().serialId))
                {
                    serialId = Convert.ToInt32(infos.Last().serialId);
                }

                addListItem(infos);
            }
        }

        /// <summary>
        /// 初始化 读卡器
        /// </summary>
        private void cvrInit()
        {
            try
            {
                int iPort;
                for (iPort = 1001; iPort <= 1016; iPort++)
                {
                    iRetUSB = CVRSDK.CVR_InitComm(iPort);
                    if (iRetUSB == 1)
                    {
                        break;
                    }
                }
                if (iRetUSB != 1)
                {
                    for (iPort = 1; iPort <= 4; iPort++)
                    {
                        iRetCOM = CVRSDK.CVR_InitComm(iPort);
                        if (iRetCOM == 1)
                        {
                            break;
                        }
                    }
                }

                if ((iRetCOM == 1) || (iRetUSB == 1))
                {
                    isClose = false;
                    this.toolStripStatusLabel1.Text = "CVRSDK 初始化成功！";
                }
                else
                {
                    this.toolStripStatusLabel1.Text = "CVRSDK 初始化失败！";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        private void readData()
        {
            try
            {
                if ((iRetCOM == 1) || (iRetUSB == 1))
                {

                    int authenticate = CVRSDK.CVR_Authenticate();
                    if (authenticate == 1)
                    {
                        int readContent = CVRSDK.CVR_Read_Content(4);
                        if (readContent == 1)
                        {
                            onevent("读卡操作成功", TYPE_STATUS_LABEL);
                            onevent("", TYPE_FILL_DATA);
                        }
                        else
                        {
                            onevent("读卡操作失败", TYPE_STATUS_LABEL);
                        }
                    }
                    else
                    {
                        onevent("未放卡或卡片放置不正确", TYPE_STATUS_LABEL);
                    }
                }
                else
                {
                    onevent("初始化失败", TYPE_STATUS_LABEL);
                }
            }
            catch (Exception ex)
            {
                onevent(ex.ToString(), TYPE_STATUS_LABEL);
            }
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        public void FillData(PeopleInfo toSavePeople)
        {
            toSavePeople.clearInfo();
            try
            {
                byte[] name = new byte[30];
                int length = 30;
                CVRSDK.GetPeopleName(ref name[0], ref length);
                //MessageBox.Show();
                byte[] number = new byte[30];
                length = 36;
                CVRSDK.GetPeopleIDCode(ref number[0], ref length);
                byte[] people = new byte[30];
                length = 3;
                CVRSDK.GetPeopleNation(ref people[0], ref length);
                byte[] validtermOfStart = new byte[30];
                length = 16;
                CVRSDK.GetStartDate(ref validtermOfStart[0], ref length);
                byte[] birthday = new byte[30];
                length = 16;
                CVRSDK.GetPeopleBirthday(ref birthday[0], ref length);
                byte[] address = new byte[30];
                length = 70;
                CVRSDK.GetPeopleAddress(ref address[0], ref length);
                byte[] validtermOfEnd = new byte[30];
                length = 16;
                CVRSDK.GetEndDate(ref validtermOfEnd[0], ref length);
                byte[] signdate = new byte[30];
                length = 30;
                CVRSDK.GetDepartment(ref signdate[0], ref length);
                byte[] sex = new byte[30];
                length = 3;
                CVRSDK.GetPeopleSex(ref sex[0], ref length);

                byte[] samid = new byte[32];
                CVRSDK.CVR_GetSAMID(ref samid[0]);
                toSavePeople.sex = Encoding.GetEncoding("GB2312").GetString(sex).Replace("\0", "").Trim();
                toSavePeople.age = Convert.ToString(GetAgeByBirthdate(Convert.ToDateTime(Encoding.GetEncoding("GB2312").GetString(birthday).Replace("\0", "").Trim())));
                toSavePeople.cardId = Encoding.GetEncoding("GB2312").GetString(number).Replace("\0", "").Trim();
                toSavePeople.name = Encoding.GetEncoding("GB2312").GetString(name).Replace("\0", "").Trim();
                toSavePeople.serialId = Convert.ToString(serialId);// DateTime.Now.ToString("yyyyMMddHHmm");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            initRecordAndId(null);
            // 读卡器初始化
            cvrInit();
            // 空间与数据绑定
            tb_name.DataBindings.Add("Text", people, "name", false, DataSourceUpdateMode.OnPropertyChanged);
            tb_sex.DataBindings.Add("Text", people, "sex", false, DataSourceUpdateMode.OnPropertyChanged);
            tb_age.DataBindings.Add("Text", people, "age", false, DataSourceUpdateMode.OnPropertyChanged);
            tb_card_id.DataBindings.Add("Text", people, "cardId", false, DataSourceUpdateMode.OnPropertyChanged);
            tb_serial.DataBindings.Add("Text", people, "serialId", false, DataSourceUpdateMode.OnPropertyChanged);

            //声明委托
            loghandlers = new loghandler(PrintLog);

            // 循环读卡
            _readCardInfoTimer = new System.Timers.Timer();
            _readCardInfoTimer.Elapsed += new ElapsedEventHandler(timeSendSms_Tick);
            _readCardInfoTimer.Interval = 1000;
            _readCardInfoTimer.Start();
        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            //people.clearInfo();
            //string random = Convert.ToString(DateTime.Now.Millisecond % 12);
            //people.name = "name"+ random;
            //people.age = random;
            //people.sex = DateTime.Now.Millisecond % 2 == 0 ? "男" : "女";
            //people.serialId = Convert.ToString(serialId);
            //people.cardId = "3332221987110823" + random;
            if (!isClose)
            {
                // 初始化成功后读取数据
                readData();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClose)
            {
                try
                {
                    CVRSDK.CVR_CloseComm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void lv_print_history_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection indexes = this.lv_print_history.SelectedIndices;
                if (indexes.Count > 0)
                {
                    (lv_print_history.Items[indexes[0]].Tag as PeopleInfo).copyTo(people);
                    //
                    //string sPartNo = this.lv_print_history.Items[index].SubItems[0].Text;//获取第一列的值  
                    //string sPartName = this.lv_print_history.Items[index].SubItems[1].Text;//获取第二列的值  
                    //toolStripStatusLabel1.Text = sPartNo + sPartName + lv_print_history.Items[index].Tag;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败！\n" + ex.Message, "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        /// <summary>
        /// 跨线程操作
        /// </summary>
        /// <param name="Str">内容</param>
        /// <param name="msgType">类型</param>
        public void onevent(string Str, int msgType)
        {
            this.Invoke(loghandlers, new object[] { Str, msgType });
        }

        /// <summary>
        /// 状态显示
        /// </summary>
        /// <param name="msg"></param>
        public void PrintLog(string msg, int msgType)
        {
            try
            {
                switch (msgType)
                {
                    case TYPE_STATUS_LABEL:
                        // 更新状态栏
                        toolStripStatusLabel1.Text = msg;
                        break;
                    case TYPE_FILL_DATA:
                        // 填充数据（界面）
                        FillData(people);
                        break;
                    case TYPE_WRITE_HISTORY:
                        // 保存打印记录
                        WriteHistory(people, DateTime.Now);
                        break;
                    case TYPE_WRITE_ID:
                        // 保存流水号
                        string idPath = "id.txt";
                        WriteFile(idPath, DateTime.Now.ToString("yy-MM-dd") + "," + Convert.ToString(serialId), false);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ee)
            {
                System.Console.WriteLine(ee.Message);
            }
        }

        /// <summary>
        /// 打印记录
        /// </summary>
        /// <param name="toSavePeople">记录（个人信息）</param>
        /// <param name="dt">打印时间</param>
        public void WriteHistory(PeopleInfo toSavePeople, DateTime dt)
        {
            string historyPath = HISTORY_DIR + "/" + DateTime.Now.ToString("yy-MM-dd") + "_history.txt";
            WriteFile(historyPath, toSavePeople.ToString(), true);
        }

        public void WriteFile(String filePath, String toWriteText, bool append)
        {
            try
            {
                if (!Directory.Exists(HISTORY_DIR))
                {
                    Directory.CreateDirectory(HISTORY_DIR);
                }
                if (File.Exists(filePath))
                {
                    StreamWriter sw = new StreamWriter(filePath, append);
                    sw.Write(toWriteText + "\r\n");

                    sw.Flush(); sw.Close();
                    sw.Dispose();
                }
                else
                {

                    StreamWriter sw = new StreamWriter(filePath, false);
                    sw.Write(toWriteText + "\r\n");

                    sw.Flush(); sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
            }
        }

        //任务thread--非定时--定时任务单独处理
        private void timeSendSms_Tick(object sender, EventArgs e)
        {
            _readCardInfoTimer.Enabled = false;
            //btn_read.Enabled = false;

            if (!isClose)
            {
                readData();
            }

            //btn_read.Enabled = true;
            _readCardInfoTimer.Enabled = true;
        }

        public int GetAgeByBirthdate(DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }
    }
}
