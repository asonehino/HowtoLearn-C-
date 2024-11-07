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


namespace ch04
{
    public partial class Form1 : Form
    {
        //접속query
        string Conn = "Server=localhost;Database=20241031;Uid=root;Pwd=hyeonho1101@;";


        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //버튼을 클릭했다
            //삽입구문
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Conn))
                {
                    conn.Open();
                    MySqlCommand msc = new MySqlCommand($"insert into user(name, age, gender) values ('{textBox1.Text}', {textBox2.Text}, '{textBox3.Text}');", conn);
                    msc.ExecuteNonQuery();
                }
                MessageBox.Show("데이터베이스에 추가했습니다.");
            }

            catch
            {
                MessageBox.Show("실패했습니다.");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //조회하기 클릭!
            //검색구문
            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                DataSet ds = new DataSet();
                string sql = "select * from user;";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "user");

                //ds라는데 읽어온 데이터가 있구나
                int len = ds.Tables[0].Rows.Count; //레코드갯수 

                richTextBox1.Text = "";

                for (int i=0; i<len; i++)
                {
                    string num = ds.Tables[0].Rows[i]["num"].ToString();
                    string name = ds.Tables[0].Rows[i]["name"].ToString();
                    string age = ds.Tables[0].Rows[i]["age"].ToString();
                    string gender = ds.Tables[0].Rows[i]["gender"].ToString();

                    richTextBox1.Text += $"{num}, {name}, {age}, {gender}\n";

                    //리스트뷰에 집어넣기
                    ListViewItem Ivi = new ListViewItem();
                    Ivi.Text = num;
                    Ivi.SubItems.Add(name);
                    Ivi.SubItems.Add(age);
                    Ivi.SubItems.Add(gender);
                    listView1.Items.Add(Ivi);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //조회버튼클릭
            //검색구문
            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                DataSet ds = new DataSet();
                string sql = $"select * from user where num = {textBox4.Text};";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "~");

                //읽어온 데이터 갯수
                int len = ds.Tables[0].Rows.Count; //레코드갯수 
                if (len == 0)
                {
                    //조회결과없음
                    MessageBox.Show("no");
                }
                else
                {
                    //조회가 되었음
                    string name = ds.Tables[0].Rows[0]["name"].ToString();
                    string age = ds.Tables[0].Rows[0]["age"].ToString();
                    string gender = ds.Tables[0].Rows[0]["gender"].ToString();
                    textBox5.Text = name;
                    textBox6.Text = age;
                    textBox7.Text = gender;
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //삭제구문
            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                conn.Open();
                MySqlCommand msc = new MySqlCommand($"delete from user where num = {textBox4.Text};", conn);
                {

                };
                msc.ExecuteNonQuery();
            }
            MessageBox.Show("삭제되었습니다.");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //수정구문
            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                conn.Open();
                MySqlCommand msc = new MySqlCommand($"update user set name = '{textBox5.Text}', age = {textBox5.Text}, gender = '{textBox6.Text}' where num = {textBox4.Text};", conn);

                msc.ExecuteNonQuery();
            }
            MessageBox.Show("수정되었습니다.");

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //삭제구문
            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                conn.Open();
                MySqlCommand msc = new MySqlCommand($"delete from user;", conn);
                {

                };
                msc.ExecuteNonQuery();
            }
            MessageBox.Show("삭제되었습니다.");
        }
    }
}
