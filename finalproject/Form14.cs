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
    public partial class Form14 : MetroForm
    {
        public Form14()
        {
            InitializeComponent();
        }

        DataTable table, refund;
        string m_num;
        private void Form14_Load(object sender, EventArgs e)
        {
            cuS_BUY_ID_MARTTableAdapter1.Fill(dataSet11.CUS_BUY_ID_MART);

            table = dataSet11.Tables["CUS_BUY_ID_MART"];
            productTableAdapter1.Fill(dataSet11.PRODUCT);
            sellTableAdapter1.Fill(dataSet11.SELL);

            refund = dataSet11.Tables["SELL"];
            comboBox1.SelectedIndex = 0;
            chart1.Visible = false;
        }

        public void ListFill()
        {
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();

            DataGridViewTextBoxColumn textboxcolumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn textboxcolumn2 = new DataGridViewTextBoxColumn();
            textboxcolumn.HeaderText = "상품명";
            textboxcolumn2.HeaderText = "판매갯수";
            metroGrid1.Columns.Add(textboxcolumn);
            metroGrid1.Columns.Add(textboxcolumn2);
            metroGrid1.Columns[0].Width = 380;
            metroGrid1.Columns[1].Width = 100;

            foreach (DataRow dr in table.Rows)
            {
                if (dr["total"].ToString() != "0")
                {
                    string[] str = dr["B_LIST"].ToString().Split('/'); //상품,갯수 로 분리
                    for (int i = 0; i < str.Length - 1; i++)
                    {
                        string[] divide = str[i].Split('.');
                        string[] divide2 = divide[0].Split(',');
                        string pro_id = divide2[0];
                        string cnt = divide2[1];
                        string p_price = divide[1];

                        string p_name = productTableAdapter1.FindName(pro_id);
                        bool exist = false;
                        for (int j = 0; j < metroGrid1.RowCount; j++)
                        {
                            if (metroGrid1.Rows[j].Cells[0].Value == p_name)
                            {
                                metroGrid1.Rows[j].Cells[1].Value = Int32.Parse(metroGrid1.Rows[j].Cells[1].Value.ToString()) + cnt;
                                exist = true;
                            }
                        }
                        if (exist == false)
                        {
                            metroGrid1.Rows.Add(p_name, cnt);
                        }
                    }
                }
            }
            metroGrid1.Sort(metroGrid1.Columns[1], System.ComponentModel.ListSortDirection.Descending); //큰 숫자부터 정렬 (내림차순. 오름차순은 Ascending)
        }

        public void RefundFill()
        {
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();

            DataGridViewTextBoxColumn textboxcolumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn textboxcolumn2 = new DataGridViewTextBoxColumn();
            textboxcolumn.HeaderText = "상품명";
            textboxcolumn2.HeaderText = "환불갯수";
            metroGrid1.Columns.Add(textboxcolumn);
            metroGrid1.Columns.Add(textboxcolumn2);
            metroGrid1.Columns[0].Width = 380;
            metroGrid1.Columns[1].Width = 100;

            foreach (DataRow dr in refund.Rows)
            {
                string p_name = productTableAdapter1.FindName(dr["p_id"].ToString());
                string refu = dr["REFUND"].ToString();
                bool exist = false;
                if (refu != "0")
                {
                    for (int i = 0; i < metroGrid1.RowCount; i++)
                    {
                        if (metroGrid1.Rows[i].Cells[0].Value != null)
                        {
                            if (metroGrid1.Rows[i].Cells[0].Value.ToString() == p_name)
                            {
                                metroGrid1.Rows[i].Cells[1].Value = Int32.Parse(metroGrid1.Rows[i].Cells[1].Value.ToString()) + refu;
                                exist = true;
                            }
                        }

                    }
                    if (exist == false)
                    {
                        metroGrid1.Rows.Add(p_name, refu);
                    }
                }

            }
            metroGrid1.Sort(metroGrid1.Columns[1], System.ComponentModel.ListSortDirection.Descending);
        }
        private void metroGrid1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //int a = int.Parse(e.CellValue1.ToString()), b = int.Parse(e.CellValue2.ToString());
            //e.SortResult = a.CompareTo(b);
            //e.Handled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) //판매순
        {
            m_num = comboBox1.SelectedItem.ToString();
            if (m_num == "전체")
            {
                cuS_BUY_ID_MARTTableAdapter1.Fill(dataSet11.CUS_BUY_ID_MART);
                table = dataSet11.Tables["CUS_BUY_ID_MART"];
                ListFill();
            }
            else
            {
                cuS_BUY_ID_MARTTableAdapter1.FillByMart(dataSet11.CUS_BUY_ID_MART, m_num);
                table = dataSet11.Tables["CUS_BUY_ID_MART"];
                ListFill();

            }
            chart_fill();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();
            radioButton1.Checked = false;
            radioButton3.Checked = false;
        }

        private void chart_fill()
        {
            chart1.Titles.Clear();
            if (radioButton1.Checked == true)
            {
                chart1.Titles.Add(m_num + " 매장의 상품별 판매량");
            }
            if (radioButton3.Checked == true)
            {
                chart1.Titles.Add(m_num + " 매장의 상품별 환불량");
            }
            //차트로 보기
            chart1.Series["상품"].Points.Clear();
            //if (radioButton1.Checked == false && radioButton3.Checked == false)
            //{
            //    MessageBox.Show("종류를 선택하세요");
            //}
            //else
            //{
            if (metroGrid1.RowCount > 7)
            {
                for (int i = 0; i < 8; i++)
                {
                    string p_name = metroGrid1.Rows[i].Cells[0].Value.ToString();
                    int count = Int32.Parse(metroGrid1.Rows[i].Cells[1].Value.ToString());
                    chart1.Series["상품"].Points.AddXY(p_name, count);
                }
            }
            else
            {
                for (int i = 0; i < metroGrid1.RowCount; i++)
                {
                    string p_name = metroGrid1.Rows[i].Cells[0].Value.ToString();
                    int count = Int32.Parse(metroGrid1.Rows[i].Cells[1].Value.ToString());
                    chart1.Series["상품"].Points.AddXY(p_name, count);
                }
            }
            // }
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (metroButton1.Text == "차트로 보기")
            {
                metroButton1.Text = "표로 보기";
                chart1.Visible = true;
            }
            else
            {
                metroButton1.Text = "차트로 보기";
                chart1.Visible = false;
            }


        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e) //환불 순
        {
            m_num = comboBox1.SelectedItem.ToString();

            if (m_num == "전체")
            {
                sellTableAdapter1.Fill(dataSet11.SELL);
                cuS_BUY_ID_MARTTableAdapter1.Fill(dataSet11.CUS_BUY_ID_MART);
                refund = dataSet11.Tables["SELL"];
                table = dataSet11.Tables["CUS_BUY_ID_MART"];
                RefundFill();
            }
            else
            {
                sellTableAdapter1.FillByMart(dataSet11.SELL, m_num);
                cuS_BUY_ID_MARTTableAdapter1.FillByMart(dataSet11.CUS_BUY_ID_MART, m_num);
                refund = dataSet11.Tables["SELL"];
                table = dataSet11.Tables["CUS_BUY_ID_MART"];

                RefundFill();
            }
            chart_fill();
        }
    }
}
