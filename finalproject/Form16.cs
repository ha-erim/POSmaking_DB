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
    public partial class Form16 : MetroForm
    {
        public Form16()
        {
            InitializeComponent();
        }

        private void Form16_Load(object sender, EventArgs e)
        {
            sellTableAdapter1.Fill(dataSet11.SELL);
        }
        string m_num;
        DataTable sell_pro_table;

        private void show_product()
        {
            producT_SELL_VIEWTableAdapter1.FillByMart(this.dataSet11.PRODUCT_SELL_VIEW, m_num);
            sell_pro_table = dataSet11.Tables["PRODUCT_SELL_VIEW"];

            comboBox3.Items.Clear();

            string kind = comboBox2.SelectedItem.ToString();
            DataRow[] mydataRow = sell_pro_table.Select("[P_KIND] = '" + kind + "'");
            if (mydataRow.Length > 0)
            {
                foreach (DataRow mydataRows in mydataRow)
                {
                    comboBox3.Items.Add(mydataRows["P_NAME"].ToString());
                }
            }
            else
            {
                MessageBox.Show("선택된 상품 종류가 없습니다.");
            }

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //매장 선택
        {

            m_num = comboBox1.SelectedItem.ToString();
            producT_SELL_VIEWTableAdapter1.FillByMart(dataSet11.PRODUCT_SELL_VIEW, m_num);
            producT_SELL_VIEWTableAdapter1.InsertKind(dataSet11.PRODUCT_SELL_VIEW, m_num);
            sell_pro_table = dataSet11.Tables["PRODUCT_SELL_VIEW"];
            foreach (DataRow dr in sell_pro_table.Rows)
            {
                comboBox2.Items.Add(dr["KND"].ToString());
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) //t상품 종류 선택
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("선택된 매장이 없습니다.");
            }
            else
            {
                show_product();
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e) //변경
        {
            DateTime change_day = dateTimePicker1.Value;
            string p_id = productTableAdapter1.FindPId(comboBox3.SelectedItem.ToString());
            if (textBox1.Text != "변경 가격")
            {

                int new_price = Int32.Parse(textBox1.Text);
                int old_price = Int32.Parse(sellTableAdapter1.FindPrice(m_num, p_id).ToString());
                changE_PRICETableAdapter1.InsertQuery(change_day, m_num, p_id, old_price);
                sellTableAdapter1.UpdatePrice(new_price, p_id, m_num);



                int numOfRows = changE_PRICETableAdapter1.Update(dataSet11.CHANGE_PRICE);
                int numOfRows_2 = sellTableAdapter1.Update(dataSet11.SELL);
                if (numOfRows < 1 && numOfRows_2 < 1)
                {
                    MessageBox.Show("가격 변경 완료");
                }
                else MessageBox.Show("가격 변경 실패");

            }
            else
            {
                MessageBox.Show("변경할 가격을 작성하세요");
            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string product_name = comboBox3.SelectedItem.ToString();
            string p_id = productTableAdapter1.FindPId(product_name);
            string price = sellTableAdapter1.FindPrice(comboBox1.SelectedItem.ToString(), p_id).ToString();
            textBox1.Text = price;
        }
    }
}
