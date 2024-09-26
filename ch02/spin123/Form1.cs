using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace spin123
{
    public partial class Form1 : Form
    {
        MqttClient client;
        string clientId;

        public Form1()
        {
            InitializeComponent();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //아두이노가 뭔갈 보냈다
            string data = serialPort1.ReadLine();

            richTextBox1.Text += data + "\n";

            //esp23가 보낸 csv형식의 데이터를 쪼개서 textbox3와 texbox4에 넣는다
            string[] data2 = data.Split(',');
            if(data2.Length == 2)
            {
                //데이터가 조도센서와 가변저항값 2개인게 확실하면
                textBox3.Text = data2[0];
                textBox4.Text = data2[1];
                //숫자로 바꿔서 바늘으르 움직인다
                aGauge1.Value = int.Parse(data2[0]);
                aGauge2.Value = int.Parse(data2[1]);
            }

            //크로스 쓰레드 문제르 ㄹ해결하기 위한 해법
            //this.Invoke(new MethodInvoker(delegate ()
            //{
            //    richTextBox1.Text = data + "\n";
            //}

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //접속버튼을 눌렀다
            serialPort1.PortName = textBox1.Text;
            serialPort1.BaudRate = int.Parse(textBox2.Text);
            //esp32와 연결
            serialPort1.Open();
            //esp가 연결되었나?
            if (serialPort1.IsOpen)
            {
                //접속 성공한 경우
                MessageBox.Show("연결됨");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //사용자가 프로그램을 실행해서 사용할 준비가 완료됨
            //mqtt 브로커와 접속


            string BrokerAddress = "broker.emqx.io";
            client = new MqttClient(BrokerAddress);
            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // use a unique id as client id, each time we start the application
            clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            //접속완료 (구독)
            string[] mytopic = {"bssm/asonehino"};
            byte[] myqos = { 0 };
            client.Subscribe(mytopic, myqos); 
        }

        //mqtt 이벤트 핸들러
        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            //DO SOMETHING..!
            richTextBox1.Text += ReceivedMessage + "\n";
            string[] data2 = ReceivedMessage.Split(',');
            if (data2.Length == 2)
            {
                //데이터가 조도센서와 가변저항값 2개인게 확실하면
                textBox3.Text = data2[0];
                textBox4.Text = data2[1];
                //숫자로 바꿔서 바늘으르 움직인다
                aGauge1.Value = int.Parse(data2[0]);
                aGauge2.Value = int.Parse(data2[1]);
            }
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //사용자가 프로그램을 종료하기 직전에 발생
            //mqtt 브로커와 접속 끊기
            client.Disconnect();
        }
    }
}
