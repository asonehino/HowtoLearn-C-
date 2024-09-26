using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _01_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //접속버튼을 눌렀다
            serialPort1.PortName = textBox1.Text; //포트번호
            serialPort1.BaudRate = int.Parse(textBox2.Text);
            serialPort1.Encoding = Encoding.UTF8;

            //시리얼 포트 개방
            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                //아두이노와 성공적으로 연결
                MessageBox.Show("연결에 성공했습니다.");
            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //아두이노로부터 데이터가 들어온게 감지되었다
            string text = serialPort1.ReadLine();

            string[] mydata = text.Split(',');

            //아두이노가 전송하는 데이터는 콤마를 기준으로 2개이다.
           
            if(mydata.Length == 2)
            {
                //온도
                textBox4.Text = mydata[0];
                //습도
                textBox5.Text = mydata[1];
            }

            richTextBox1.Text += text + '\n';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ON버튼 클릭 시
            //ESP32에게 '1'을 전송하기
            //SerialPort1.WriteLine
            serialPort1.Write("1");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            serialPort1.Write("0");
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
