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
    public partial class Form7 : MetroForm
    {
        public Form7()
        {
            InitializeComponent();
        }
        DataTable review;
        private void Form7_Load(object sender, EventArgs e)
        {
            reviewTableAdapter1.Fill(dataSet11.REVIEW);
            productTableAdapter1.Fill(dataSet11.PRODUCT);

            review = dataSet11.Tables["REVIEW"];
            if(writed == true)
            {
                //product = 상품이름
                //p,c,text,m,title,t_id
                string p_id = productTableAdapter1.FindPId(product);
                DataRow[] mydataRow = review.Select("[C_ID] = '" + c_id + "' AND [P_ID] = '" + p_id + "' AND [T_ID] = '" + t_id+"'");
                //DataRow dr = mydataRow[0];
                if(mydataRow.Length > 0)
                {
                    foreach (DataRow dr in mydataRow)
                    {
                        string pro_name = productTableAdapter1.FindName(dr["P_ID"].ToString());
                        //label4.Text = pro_name;
                        //label5.Text = m_num + " 매장";

                        textBox1.Text = dr[4].ToString();
                        richTextBox1.Text = dr[2].ToString();
                    }
                    button1.ForeColor = Color.White;
                    button1.Enabled = false;
                }
                
                
            }
        }

        string c_id, m_num, product,t_id;
        bool writed = false;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void SetData(string id, string mart,string p_name,string tr_id)
        {
            c_id = id;
            m_num = mart;
            product = p_name;
            t_id = tr_id;

            label4.Text = p_name;
            label5.Text = mart;
        }

        public void ShowReview(bool a)
        {
            writed = a;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string p_id = productTableAdapter1.FindPId(product);
            if(textBox1.Text == null)
            {
                MessageBox.Show("제목이 작성되지 않았습니다.");
            }
            else
            {
                if(richTextBox1.Text == null)
                {
                    MessageBox.Show("내용이 작성되지 않았습니다.");
                }
                else
                {
                    string title = textBox1.Text;
                    string text = richTextBox1.Text;
                    string r_num = reviewTableAdapter1.NextSeqValue().ToString();
                    reviewTableAdapter1.InsertQuery(p_id, c_id, text, m_num,title,t_id,r_num);

                    int numOfRows = reviewTableAdapter1.Update(dataSet11.REVIEW);
                    if (numOfRows < 1)
                    {
                        MessageBox.Show("리뷰 작성 완료");
                        reviewTableAdapter1.Fill(dataSet11.REVIEW);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("리뷰 작성 실패");
                    }
                }
            }
            
        }

        
    }
}
