using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;




namespace ch06
{
    public partial class Form1 : Form
    {
        //전역으로 클래스선언
        MqttClient client;
        string clientId;
        //접속query
        string Conn = "Server=localhost;Database=ziu;Uid=root;Pwd=1234;";

        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //MQTT 연결끊기
            client.Disconnect(); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //MQTT 연결
            string BrokerAddress = "broker.emqx.io";
            client = new MqttClient(BrokerAddress);

            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // use a unique id as client id, each time we start the application
            clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            //클라이언트 완료 후 구독하기 좋은 지점
            //Subscribe Topic 추가
            client.Subscribe(new string[] { "nockanda/rfid" }, new byte[] { 0 });
            
            
        }

        //MQTT이벤트 핸들러
        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            //DO SOMETHING..!
            textBox1.Text = ReceivedMessage;

            //수신한 태그ID로 데이터베이스 조회
            //검색구문
            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                DataSet ds = new DataSet();
                string sql = $"select * from rfid where id = '{ReceivedMessage}';"; 
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "~");

                //데이터베이스에서 읽어온 갯수를 확인한다
                int cnt = ds.Tables[0].Rows.Count;

                if (cnt == 1)
                {
                    //결과출력
                    string name = ds.Tables[0].Rows[0]["name"].ToString();
                    string age = ds.Tables[0].Rows[0]["age"].ToString();
                    string gender = ds.Tables[0].Rows[0]["gender"].ToString();
                    string myclass = ds.Tables[0].Rows[0]["class"].ToString();

                    textBox2.Text = name;
                    textBox3.Text = age;
                    textBox4.Text = gender;
                    textBox5.Text = myclass;
                    textBox6.Text = "조회 성공";

                    string date = DateTime.Now.ToString();
                    
                    //login테이블에 로그 기록
                    conn.Open();
                    MySqlCommand msc = new MySqlCommand($"insert into login(id,name,date) values('{ReceivedMessage}','{name}','{date}');", conn);
                    msc.ExecuteNonQuery();
                }
                else
                {
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "조회 실패";
                }
            }

        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //종료버튼 눌렀다
            Application.Exit();
        }

        private void 등록하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //폼2를 모달리스로 띄운다
            Form2 fm2 = new Form2(textBox1.Text);
            fm2.ShowDialog();
        }

        private void 등록하기ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //폼2로 로그인
            Form2 fm2 = new Form2(textBox1.Text);
            fm2.ShowDialog();
        }
    }
}
