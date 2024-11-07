using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//네임스페이스 추가
using MySql.Data.MySqlClient;


namespace ch05
{
    public partial class Form1 : Form
    {
        //접속query
        string Conn = "Server=localhost;Database=ziu;Uid=root;Pwd=1234;";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //포트번호, 통신속도
            serialPort1.PortName = textBox1.Text;
            serialPort1.BaudRate = 115200;

            serialPort1.Open(); //esp와 연결

            if (serialPort1.IsOpen)
            {
                MessageBox.Show("연결완료");
            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //데이터가 왔다
            string data = serialPort1.ReadLine();
            richTextBox1.Text += data + "\n";

            string[] sensor = data.Split(',');

            if (sensor.Length == 2)
            {
                string temp = sensor[0];
                string humi = sensor[1];
                string date = DateTime.Now.ToString();

                using (MySqlConnection conn = new MySqlConnection(Conn))
                {
                    conn.Open();
                    MySqlCommand msc = new MySqlCommand($"insert into sensor_table(temp, humi, date) values({temp},{humi},'{date}');", conn);
                    msc.ExecuteNonQuery();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //검색구문
            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                DataSet ds = new DataSet();
                string sql = "select * from sensor_table";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "~");

                //읽어온 데이터의 갯수
                int len = ds.Tables[0].Rows.Count;

                listView1.Items.Clear();

                for (int i = 0; i<len; i++)
                {
                    string num = ds.Tables[0].Rows[i]["num"].ToString();
                    string temp = ds.Tables[0].Rows[i]["temp"].ToString();
                    string humi = ds.Tables[0].Rows[i]["humi"].ToString();
                    string date = ds.Tables[0].Rows[i]["date"].ToString();

                    //하나의 레코드를 지정하고listview에 대입한다.
                    ListViewItem Ivi = new ListViewItem();
                    Ivi.Text = num;
                    Ivi.SubItems.Add(temp);
                    Ivi.SubItems.Add(humi);
                    Ivi.SubItems.Add(date);

                    listView1.Items.Add(Ivi);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //검색구문
            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                DataSet ds = new DataSet();
                string sql = "select * from sensor_table order by date desc limit 10";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "~");

                //읽어온 데이터의 갯수
                int len = ds.Tables[0].Rows.Count;

                listView1.Items.Clear();

                for (int i = 0; i < len; i++)
                {
                    string num = ds.Tables[0].Rows[i]["num"].ToString();
                    string temp = ds.Tables[0].Rows[i]["temp"].ToString();
                    string humi = ds.Tables[0].Rows[i]["humi"].ToString();
                    string date = ds.Tables[0].Rows[i]["date"].ToString();

                    //하나의 레코드를 지정하고listview에 대입한다.
                    ListViewItem Ivi = new ListViewItem();
                    Ivi.Text = num;
                    Ivi.SubItems.Add(temp);
                    Ivi.SubItems.Add(humi);
                    Ivi.SubItems.Add(date);

                    listView1.Items.Add(Ivi);
                }
            }
        }
    }
}
