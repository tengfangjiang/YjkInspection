using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace YjkInspectClient
{
    public partial class MainForm : Form
    {
        bool isClose = false;
        int iRetUSB = 0, iRetCOM = 0;
        PeopleInfo people = new PeopleInfo();
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
            printBoxCode(people);
            addListItem(people);
            // printBoxCode("李四", "34062119890615456x", "张三依依", "340621********198x", "60", "男", "201806190101");
        }

        private void addListItem(PeopleInfo toAdd)
        {
            lv_print_history.BeginUpdate();
            ListViewItem lvi = new ListViewItem();
            lvi.Text = toAdd.name;
            lvi.SubItems.Add(people.serialId);
            lvi.SubItems.Add(people.addTime);
            lv_print_history.Items.Add(lvi);
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
            toolStripStatusLabel1.Text = "正在打印...";
        }

        private void printBoxCode(PeopleInfo printPeople)
        {
            printBoxCode(printPeople.name, printPeople.cardId, printPeople.name, printPeople.cardId, printPeople.age, printPeople.sex, printPeople.serialId);
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
                            this.toolStripStatusLabel1.Text = "读卡操作成功！";
                            FillData(people);
                            initViewByPeopleInfo(people);
                        }
                        else
                        {
                            this.toolStripStatusLabel1.Text = "读卡操作失败！";
                        }
                    }
                    else
                    {
                        MessageBox.Show("未放卡或卡片放置不正确");
                    }
                }
                else
                {
                    MessageBox.Show("初始化失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                pb_head.ImageLocation = Application.StartupPath + "\\zp.bmp";
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
                toSavePeople.serialId = DateTime.Now.ToString("yyyyMMddHHmm");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void initViewByPeopleInfo(PeopleInfo info)
        {
            tb_sex.Text = info.sex;
            tb_age.Text = info.age;
            tb_card_id.Text = info.cardId;
            tb_name.Text = info.name;

            tb_serial.Text = info.serialId;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cvrInit();
            tb_name.DataBindings.Add("Text", people, "name", false, DataSourceUpdateMode.OnPropertyChanged);
            tb_sex.DataBindings.Add("Text", people, "sex", false, DataSourceUpdateMode.OnPropertyChanged);
            tb_age.DataBindings.Add("Text", people, "age", false, DataSourceUpdateMode.OnPropertyChanged);
            tb_card_id.DataBindings.Add("Text", people, "cardId", false, DataSourceUpdateMode.OnPropertyChanged);
            tb_serial.DataBindings.Add("Text", people, "serialId", false, DataSourceUpdateMode.OnPropertyChanged);

            ColumnHeader ch = new ColumnHeader();
            ch.Text = "姓名";
            ch.Width = 77;
            ch.TextAlign = HorizontalAlignment.Left;
            lv_print_history.Columns.Add(ch);
            //lv_print_history.Columns.Add(ch);
            ColumnHeader ch1 = new ColumnHeader();
            ch1.Text = "流水号";
            ch1.Width = 77;
            ch1.TextAlign = HorizontalAlignment.Left;
            lv_print_history.Columns.Add(ch1);
            ColumnHeader ch2 = new ColumnHeader();
            ch2.Text = "添加时间";
            ch2.Width = 77;
            ch2.TextAlign = HorizontalAlignment.Left;
            lv_print_history.Columns.Add(ch2);

        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            readData();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClose)
            {
                try
                {
                    CVRSDK.CVR_CloseComm();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
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
