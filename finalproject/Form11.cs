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
    public partial class Form11 : MetroForm
    {
        public Form11()
        {
            InitializeComponent();
        }
        DataTable sell_pro_table;
        string m_num,p_kind;

        private void Form11_Load(object sender, EventArgs e)
        {
            sellTableAdapter1.Fill(dataSet11.SELL);
            pRODUCT_SELL_VIEWTableAdapter.Fill(dataSet1.PRODUCT_SELL_VIEW);

            sell_pro_table = dataSet11.Tables["PRODUCT_SELL_VIEW"];
            metroGrid1.Sort(metroGrid1.Columns[3], System.ComponentModel.ListSortDirection.Ascending);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //매장선택
        {
            label4.Text = "";
            label5.Text = "";
            comboBox2.Items.Clear();
            comboBox2.Items.Add("전체보기");
            m_num = comboBox1.SelectedItem.ToString();

            pRODUCT_SELL_VIEWTableAdapter.InsertKind(dataSet11.PRODUCT_SELL_VIEW, m_num);
            foreach (DataRow dr in sell_pro_table.Rows)
            {
                comboBox2.Items.Add(dr["KND"].ToString());
            }
            pRODUCT_SELL_VIEWTableAdapter.FillByMart(dataSet1.PRODUCT_SELL_VIEW,m_num);
            metroGrid1.Sort(metroGrid1.Columns[3], System.ComponentModel.ListSortDirection.Ascending);
        }

        public void setFilter()
        {
            if (comboBox2.Text == "상품 종류" || comboBox2.SelectedItem.ToString() == "전체보기")
            {
                
                pRODUCTSELLVIEWBindingSource.RemoveFilter();
            }
            else
            {
                p_kind = comboBox2.SelectedItem.ToString();
                pRODUCTSELLVIEWBindingSource.Filter = "P_KIND = '" + p_kind + "'";
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) //종류 선택
        {
            label4.Text = "";
            label5.Text = "";
            setFilter();
        }

        private void metroGrid1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            label4.Text = metroGrid1.CurrentRow.Cells[0].Value.ToString();
            label5.Text = metroGrid1.CurrentRow.Cells[2].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e) //재고 수정 저장
        {
            string p_name = metroGrid1.CurrentRow.Cells[2].Value.ToString();
            int cnt = Int32.Parse(textBox1.Text);
            string p_id = productTableAdapter1.FindPId(p_name);

            sellTableAdapter1.UpdateCount(cnt, p_id, m_num);
            //DB 동기화
            int numOfRows2 = sellTableAdapter1.Update(dataSet11.SELL);
            if (numOfRows2 < 1)
            {
                MessageBox.Show("상품 재입고 되었습니다.");
                textBox1.Clear();
                pRODUCT_SELL_VIEWTableAdapter.FillByMart(dataSet1.PRODUCT_SELL_VIEW, m_num);
                metroGrid1.Sort(metroGrid1.Columns[3], System.ComponentModel.ListSortDirection.Ascending);
                setFilter();

            }
            else MessageBox.Show("상품 재입고 실패하였습니다.");
        }
    }
}
