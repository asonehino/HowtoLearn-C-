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

namespace ch03
{
    public partial class Form1 : Form
    {
        MqttClient client;
        string clientId;
        
        Dictionary<string, string> mydic = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //MQTT 브로커와 연결하는 부분
            string BrokerAddress = "broker.emqx.io";
            client = new MqttClient(BrokerAddress);

            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // use a unique id as client id, each time we start the application
            clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            //연결완료지점 = 구독하는 지점
            //bssm/#은 bssm/로 시작하는 거 모두 받기
            client.Subscribe(new string[] { "bssm/#" }, new byte[] { 0 });
        }

        //MQTT이벤트 핸들러
        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            richTextBox1.Text += "[" + e.Topic + "]"  + ReceivedMessage + "\n";

            //지금 날라온 토픽을 사전에서 검색한다
            //검색결과 사전에 있다면 value를 업데이트
            //검색결과 사전에 없다면 최초로 온 메시지이므로 추가
            if (mydic.ContainsKey(e.Topic))
            {
                //이미 받은 적이 있으면
                mydic[e.Topic] = ReceivedMessage;
            }
            else
            {
                //최초로 수신한 토픽
                mydic.Add(e.Topic, ReceivedMessage);
            }
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //MQTT 연결 종료
            client.Disconnect();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //타이머가 1초 간격으로 무엇을 하냐?
            //딕셔너리안에 있는 내용을 몽땅 listview1에 출력
            //기존내용을 다 삭제하고 새로운 내용을 추가
            listView1.Items.Clear();
            foreach (KeyValuePair<string, string> entry in mydic)
            {
                //entry.Key
                //entry.Value
                //리스트 뷰의 데이터를 한 레코드에 집어넣는 자료형 만들기
                ListViewItem lvi = new ListViewItem();
                lvi.Text = entry.Key; //토픽
                lvi.SubItems.Add(entry.Value); //페이로더

                listView1.Items.Add(lvi);
            }
        }
    }
}
