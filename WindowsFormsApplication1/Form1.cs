using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openport("Gprinter  GP-3120TN");                                           //Open specified printer driver
                                                                                       // setup("35", "25", "4", "8", "0", "1", "0");                          　　 //Setup the media size and sensor type info            
            sendcommand("SIZE 30mm,37mm");
            clearbuffer();
            sendcommand("SET PEEL ON ");
            sendcommand(" SET   TEAR   ON ");
            sendcommand(" SET   STRIPPER   ON ");
            // sendcommand("HOME");
            //Clear image buffer
            //  barcode("10", "50", "128", "50", "1", "0", "2", "2", "010101"); //Drawing barcode
            // printerfont("30", "25", "3", "0", "1", "1", "158528552　");        //Drawing printer font
            windowsfont(25, 2, 38, 0, 2, 0, "ARIAL", "贵阳机场中转");
            windowsfont(42, 38, 35, 0, 2, 0, "ARIAL", "电子消费券");
            windowsfont(20, 65, 30, 0, 2, 0, "ARIAL", "欢迎选择贵阳中转");

            windowsfont(15, 90, 30, 0, 2, 0, "ARIAL", "凭密码:12345678"); //我们提供价值48元就餐券
            windowsfont(15, 115, 24, 0, 0, 0, "ARIAL", "到以下地点消费免48元");  //凭消费密码:1213456789
            windowsfont(12, 140, 24, 0, 0, 0, "ARIAL", "1.雅品快港自助餐(连廊)");
            windowsfont(12, 165, 24, 0, 0, 0, "ARIAL", "2.仙姆莱克咖啡(C指廊)");
            windowsfont(12, 190, 24, 0, 0, 0, "ARIAL", "3.欧迪咖啡厅(C指廊)");
            windowsfont(12, 215, 22, 0, 0, 0, "ARIAL", "4.雅品茶餐厅(二楼D岛旁)");
            //  windowsfont(180, 175, 24, 0, 0, 0, "ARIAL", "121345678");
            barcode("40", "240", "128", "30", "1", "0", "2", "2", "12345678");
            //  windowsfont(30, 200, 24, 0, 0, 0, "ARIAL", "4.雅品茶餐厅(二楼D岛旁)");

            //   sendcommand(" SET   STRIPPER   ON ");
            printlabel("1", "1");                                                    //Print labels
            closeport();
        }

        [DllImport("TSCLIB.dll", EntryPoint = "about")]
        public static extern int about();

        [DllImport("TSCLIB.dll", EntryPoint = "openport")]
        public static extern int openport(string printername);

        [DllImport("TSCLIB.dll", EntryPoint = "barcode")]
        public static extern int barcode(string x, string y, string type,
                    string height, string readable, string rotation,
                    string narrow, string wide, string code);

        [DllImport("TSCLIB.dll", EntryPoint = "clearbuffer")]
        public static extern int clearbuffer();

        [DllImport("TSCLIB.dll", EntryPoint = "closeport")]
        public static extern int closeport();

        [DllImport("TSCLIB.dll", EntryPoint = "downloadpcx")]
        public static extern int downloadpcx(string filename, string image_name);

        [DllImport("TSCLIB.dll", EntryPoint = "formfeed")]
        public static extern int formfeed();

        [DllImport("TSCLIB.dll", EntryPoint = "nobackfeed")]
        public static extern int nobackfeed();

        [DllImport("TSCLIB.dll", EntryPoint = "printerfont")]
        public static extern int printerfont(string x, string y, string fonttype,
                        string rotation, string xmul, string ymul,
                        string text);

        [DllImport("TSCLIB.dll", EntryPoint = "printlabel")]
        public static extern int printlabel(string set, string copy);

        [DllImport("TSCLIB.dll", EntryPoint = "sendcommand")]
        public static extern int sendcommand(string printercommand);

        [DllImport("TSCLIB.dll", EntryPoint = "setup")]
        public static extern int setup(string width, string height,
                  string speed, string density,
                  string sensor, string vertical,
                  string offset);

        [DllImport("TSCLIB.dll", EntryPoint = "windowsfont")]
        public static extern int windowsfont(int x, int y, int fontheight,
                        int rotation, int fontstyle, int fontunderline,
                        string szFaceName, string content);

        private void button2_Click(object sender, EventArgs e)
        {
            openport("Gprinter  GP-1324D");                                           //Open specified printer driver

            sendcommand("SIZE 60 mm,40 mm");
            sendcommand("GAP 3");
            sendcommand("DENSITY 6");
            sendcommand("SPEED 5");
            sendcommand("REFERENCE 10,10");
            sendcommand("CLS");
            //sendcommand("BAR 20,16,1600,5");
            sendcommand("TEXT 20,16 ,'1',90,2,2,'NO:4872'");
            barcode("50", "1", "128", "30", "1", "0", "3", "3", "121345678");

            sendcommand("PRINT 1");
            closeport();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openport("Gprinter GP-3120TN");

            sendcommand("SET PEEL ON ");
            closeport();
        }

        private void button4_Click(object sender, EventArgs e)
        {
           printBoxCode("李四", "34062119890615456x", "张三依依", "340621********198x", "60", "男", "201806190101");

        }

        private void printBoxCode(String boxCode, String barCode, String name, String idCard, String age, String sex, String number)
        {
            openport("Gprinter  GP-1324D");
            sendcommand("SIZE 60 mm,40 mm");
            //Open specified printer driver
            //setup("30", "25", "4", "8", "0", "0", "0");                          　　 //Setup the media size and sensor type info            

            clearbuffer();
            sendcommand("CLS");

            sendcommand("QRCODE 300,30,L,6,A,0,\"" + boxCode + "\"");// 打印二维码
            printerfont("20", "30", "TSS24.BF2", "0", "1", "1", "姓  名: " + name);//姓  名
            printerfont("20", "65", "TSS24.BF2", "0", "1", "1", "年  龄: " + age + "岁");//年  龄
            printerfont("20", "100", "TSS24.BF2", "0", "1", "1", "性  别: " + sex);//性  别
            printerfont("20", "135", "TSS24.BF2", "0", "1", "1", "流水号：" + number);
            barcode("20", "190", "128", "50", "0", "0", "2", "2", barCode);
            printerfont("20", "260", "TSS24.BF2", "0", "1", "1", "" + idCard);
            printlabel("1", "1");                                                    //Print labels
            closeport();
            toolStripStatusLabel1.Text = "正在打印...";
        }
    }
}
