using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finalproject
{
    public partial class Form1 : MetroForm
    {
        DataTable cus_table;
        DataTable sel_table;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            customerTableAdapter1.Fill(dataSet11.CUSTOMER);
            sellerTableAdapter1.Fill(dataSet11.SELLER);
            cus_table = dataSet11.Tables["CUSTOMER"];
            sel_table = dataSet11.Tables["SELLER"];
        }
        private void Login()
        {
            // 아이디 - 1 비번 - 2
            // 라디오버튼 1 - 회원, 2 - 직원, 3 - 관리자

            //회원 선택
            if (metroRadioButton1.Checked)
            {
                DataRow mydataRow = cus_table.Rows.Find(textBox1.Text);
                if (mydataRow == null)
                {
                    MessageBox.Show("아이디/비밀번호가 틀렸습니다.");
                }
                else if (mydataRow["C_PW"].ToString() == textBox2.Text)
                {
                    MessageBox.Show("회원 로그인 성공");
                    Form3 form3 = new Form3();
                    form3.SetData(textBox1.Text, mydataRow["C_NAME"].ToString());
                    this.Hide();

                    form3.ShowDialog();
                    this.Close();

                }
                else
                {
                    MessageBox.Show("로그인 실패");
                }
            }
            //직원 선택
            else if (metroRadioButton2.Checked)
            {
                // seller 테이블은 key가 두 개라서 Find()로 찾을 수 없음. -> select 사용
                DataRow[] mydataRow = sel_table.Select("[S_ID] = '" + textBox1.Text + "'");
                DataRow mydataRows = mydataRow[0];
                if (mydataRow == null)
                {
                    MessageBox.Show("아이디가 틀렸습니다.");
                }
                else if (mydataRows[1].ToString() == textBox2.Text)
                {
                    MessageBox.Show("직원 로그인 성공");

                    string s_id = mydataRows[0].ToString();
                    string m_num = mydataRows[3].ToString();

                    Form5 form5 = new Form5();
                    form5.SetData(s_id, m_num);
                    this.Hide();
                    form5.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("로그인 실패");
                }
            }
            //관리자 선택
            else if (metroRadioButton3.Checked)
            {
                //if (textBox1.Text == "admin" && textBox2.Text == "admin")
                //{
                //    MessageBox.Show("관리자 로그인 성공");
                //    Form9 form9 = new Form9();
                //    this.Hide();
                //    form9.ShowDialog();
                //    this.Close();
                //}
                //else
                //{
                //    MessageBox.Show("로그인 실패");
                //}
                DataRow[] mydataRow = sel_table.Select("[S_ID] = '" + textBox1.Text + "'");
                DataRow mydataRows = mydataRow[0];
                if (mydataRow == null)
                {
                    MessageBox.Show("아이디/비밀번호가 틀렸습니다.");
                }
                else if (mydataRows[1].ToString() == textBox2.Text)
                {
                    MessageBox.Show("관리자 로그인 성공");

                    string s_id = mydataRows[0].ToString();
                    string m_num = mydataRows[3].ToString();

                    Form9 form9 = new Form9();
                    this.Hide();
                    form9.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("로그인 실패");
                }
            }

        }
        private void button2_Click(object sender, EventArgs e) //로그인
        {
            Login();
        }

        private void Join()
        {
            //이름 - 4 아이디 - 3 비번 - 6 전화번호 - 5

            DataRow mydataRow = cus_table.Rows.Find(textBox1.Text);
            if (mydataRow != null)
            {
                MessageBox.Show("이미 사용중인 아이디입니다.");
            }
            else
            {
                //customerBindingSource.AddNew();
                //DataRow mynewDataRow = cus_table.NewRow();

                //mynewDataRow["C_NAME"] = textBox4.Text;
                //mynewDataRow["C_ID"] = textBox3.Text;
                //mynewDataRow["c_PHONE"] = textBox5.Text;
                //mynewDataRow["C_PW"] = textBox6.Text;

                //cus_table.Rows.Add(mynewDataRow);

                customerTableAdapter1.InsertQuery(textBox3.Text, textBox6.Text, textBox4.Text, textBox5.Text);

                textBox4.Text = "";
                textBox3.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";

                //DB와 동기화
                int numOfRows = customerTableAdapter1.Update(dataSet11.CUSTOMER);
                if (numOfRows < 1)
                {
                    MessageBox.Show(textBox5.Text);
                    MessageBox.Show("회원가입 완료");
                    customerTableAdapter1.Fill(dataSet11.CUSTOMER);
                }
                else
                {
                    MessageBox.Show("회원가입 실패");

                }
            }

        }
        private void button1_Click(object sender, EventArgs e) //회원가입
        {
            Join();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar)|| e.KeyChar == Convert.ToInt32(Keys.Back) || (e.KeyChar == '-'))
            {
            }
            else
            {
                e.Handled = true; // 처리되었다실행하지말아라.
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = CreateGraphics();
            Pen pen = new Pen(Color.Gray);
            graphics.DrawLine(pen, 400, 136, 400, 360);
            //graphics.Dispose();
            //Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));
            //e.Graphics.DrawLine(pen, 400, 136, 400, 360);
        }


        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Join();
            }
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            textBox2.Text = string.Empty;
        }

        private void textBox3_MouseClick(object sender, MouseEventArgs e)
        {
            textBox3.Text = string.Empty;
        }

        private void textBox6_MouseClick(object sender, MouseEventArgs e)
        {
            textBox6.Text = string.Empty;
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = string.Empty;
        }
    }
}
