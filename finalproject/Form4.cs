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
    public partial class Form4 : MetroForm
    {
        DataTable cus_table,review,buy,refund;
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            customerTableAdapter1.Fill(dataSet11.CUSTOMER);
            reviewTableAdapter1.Fill(dataSet11.REVIEW);
            buyTableAdapter1.Fill(dataSet11.BUY);
            refundTableAdapter1.Fill(dataSet11.REFUND);

            buy = dataSet11.Tables["BUY"];
            review = dataSet11.Tables["REVIEW"];
            refund = dataSet11.Tables["REFUND"];
            cus_table = dataSet11.Tables["CUSTOMER"];
        }

        public void SetData(string id, string name)
        {
            label2.Text = id + " ( " + name + " 님)";
            
        }

        private void label3_Click(object sender, EventArgs e) //회원탈퇴
        {
            DataRow mydataRow = cus_table.Rows.Find(textBox2.Text);
            if (mydataRow == null)
            {
                MessageBox.Show("아이디가 틀렸습니다.");
            }

            else
            {
                if (mydataRow["C_NAME"].ToString() != textBox1.Text)
                {
                    MessageBox.Show("이름이 틀렸습니다.");
                }
                else if (mydataRow["C_PW"].ToString() != textBox3.Text)
                {
                    MessageBox.Show("비밀번호가 틀렸습니다.");
                }
                else
                {
                    //cus_table.Rows.Remove(mydataRow);
                    foreach(DataRow row in refund.Rows) //환불 삭제
                    {
                        if (row["C_ID"].ToString() == textBox2.Text)
                        {
                            row.Delete();
                        }
                    }
                    int numOfRows1 = refundTableAdapter1.Update(dataSet11.REFUND);

                    foreach (DataRow row in review.Rows)
                    {
                        if (row["C_ID"].ToString() == textBox2.Text)
                        {
                            row.Delete();
                        }
                    }
                    int numOfRows2 = reviewTableAdapter1.Update(dataSet11.REVIEW);

                    foreach (DataRow row in buy.Rows)
                    {
                        if (row["C_ID"].ToString() == textBox2.Text)
                        {
                            row.Delete();
                        }
                    }
                    int numOfRows3 = buyTableAdapter1.Update(dataSet11.BUY);

                    foreach (DataRow row in cus_table.Rows)
                    {
                        if (row["C_ID"].ToString() == textBox2.Text)
                        {
                            row.Delete();
                        }
                    }
                    int numOfRows = customerTableAdapter1.Update(dataSet11.CUSTOMER);

                    if (numOfRows < 1 && numOfRows1 < 1 && numOfRows2 < 1 && numOfRows3 < 1)
                    {
                        MessageBox.Show("탈퇴 실패");
                    }
                    else
                    {
                        MessageBox.Show("탈퇴 성공");

                        List<Form> formsToHide = new List<Form>();

                        foreach (Form form in Application.OpenForms)
                        {
                            //if (form != this)  // 만약 자기 자신 Form도 닫을 경우 if문 생략
                            //{
                            //    formsToHide.Add(form);
                            //}
                            formsToHide.Add(form);
                        }

                        formsToHide.ForEach(f => f.Hide());


                        Form1 form1 = new Form1();
                        form1.ShowDialog();
                        this.Close();
                    }
                }
            }
        }
    }
}
