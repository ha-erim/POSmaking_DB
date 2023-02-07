using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace finalproject
{
    public partial class Form8 : MetroForm
    {
        public Form8()
        {
            InitializeComponent();
        }

        string product;
        DataTable review_table;

        public void ReceiveData(string pro_name)
        {
            product = productTableAdapter1.FindPId(pro_name);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            string[] num_title = listBox1.SelectedItem.ToString().Split('\t');
            string r_num = num_title[0];
            DataRow[] datarow = review_table.Select("[r_num] = '" + r_num + "'");
            DataRow dr = datarow[0];

            textBox1.Text = "작성자 : " + dr["C_ID"] + "\r\n" + "구매 장소 : " + dr["m_num"] + "\r\n" + "제목 : " + dr["title"].ToString() + "\r\n\r\n" + "내용 : " +  dr["text"];

        }

        private void Form8_Load(object sender, EventArgs e)
        {
            reviewTableAdapter1.ShowReview(dataSet11.REVIEW, product);
            review_table = dataSet11.Tables["REVIEW"];

            if(review_table.Rows.Count == 0)
            {
                MessageBox.Show("아직 등록된 리뷰가 없습니다.");
                this.Close();
            }
            else
            {
                foreach (DataRow dr in review_table.Rows)
                {
                    listBox1.Items.Add(dr["r_num"].ToString() + "\t" + dr["title"].ToString());
                }
            }
            
        }
    }
}
