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
        public MainForm()
        {
            InitializeComponent();
        }

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

        private void button4_Click(object sender, EventArgs e)
        {
           printBoxCode("李四", "34062119890615456x", "张三依依", "340621********198x", "60", "男", "201806190101");

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
    }
}
