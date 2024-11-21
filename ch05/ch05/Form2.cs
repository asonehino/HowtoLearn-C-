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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ch05
{
    public partial class Form2 : Form
    {
        //접속query
        string Conn = "Server=localhost;Database=ziu;Uid=root;Pwd=1234;";
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //조회버튼클릭
            //검색구문
            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                DataSet ds = new DataSet();
                string sql = "select * from sensor_table order by num desc limit 30";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "~");

                //읽어온 데이터의 갯수
                int len = ds.Tables[0].Rows.Count;

                //차트 뽀인뜨 클리어
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();

                for (int i = 0; i < len; i++)
                {
                    string num = ds.Tables[0].Rows[i]["num"].ToString();
                    string temp = ds.Tables[0].Rows[i]["temp"].ToString();
                    string humi = ds.Tables[0].Rows[i]["humi"].ToString();
                    string date = ds.Tables[0].Rows[i]["date"].ToString();

                    //차트에 데이터 추가하기
                    chart1.Series[0].Points.AddY(temp);
                    chart1.Series[1].Points.AddY(humi);
                }
            }
        }
    }
}
