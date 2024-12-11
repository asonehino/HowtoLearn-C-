using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//네임스페이스추가
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace ch07
{
    public partial class Form1 : Form
    {
        //전역으로 클래스선언
        MqttClient client;
        string clientId;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //프로그램 실행한 뒤 1밀리 초 간격으로 뭐할래?
            if(serialPort1.IsOpen)
            {
                //데이터가 오면 받기
                if (serialPort1.BytesToRead>0)
                {
                    string data = serialPort1.ReadLine();
                    richTextBox1.Text += data + "\n";
                }
            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //모노에서는 안됨
            //string data = serialPort1.ReadLine();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //접속 버튼 클릭
            //윈도우면 실행 안함
            OperatingSystem os = System.Environment.OSVersion;
            if (os.Platform != PlatformID.Win32NT)
            {
                //라즈베리파이
                serialPort1.BaudRate = 115200;
                serialPort1.PortName = "/dev/ttyUSB0";

                serialPort1.Open();
            } 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //MQTT 브로커와 연결하는 부분
            string BrokerAddress = "broker.emqx.io";
            client = new MqttClient(BrokerAddress);

            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // use a unique id as client id, each time we start the application
            clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            //Subscribe Topic 추가
            client.Subscribe(new string[] { "bssm/data" }, new byte[] { 0 });


        }
        //MQTT이벤트 핸들러
        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            //DO SOMETHING..!
            richTextBox1.Text += ReceivedMessage + "\n";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //발행버튼
            //bssm/sensor
            client.Publish("bssm/sensor", Encoding.UTF8.GetBytes("hahaha"), 0, false);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.Disconnect();
        }
    }
}
