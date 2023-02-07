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
    public partial class Form12 : MetroForm
    {
        public Form12()
        {
            InitializeComponent();
        }

        DataTable customer,buy_view;
        string c_id,c_name;
        private void Form12_Load(object sender, EventArgs e)
        {
            customerTableAdapter1.Fill(dataSet11.CUSTOMER);

            customer = dataSet11.Tables["CUSTOMER"];


            //listbox 채우기
            foreach (DataRow dr in customer.Rows)
            {
                listBox1.Items.Add(dr["C_NAME"].ToString() + "\t" + dr["C_ID"].ToString());
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void metroGrid1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            listBox2.Items.Clear();

            //상품 구매 목록 채우기
            if (metroGrid1.CurrentRow.Cells[3].Value.ToString() == "0")
            {
                listBox2.Items.Add("환불된 영수증입니다.");
                listBox2.Items.Add("");
            }

            if (metroGrid1.CurrentRow.Cells[4].Value.ToString() == "0")
            {
                listBox2.Items.Add("구매 요청 수락 대기 중");
                listBox2.Items.Add("");
            }

            string t_id = metroGrid1.CurrentRow.Cells[1].Value.ToString();
            string b_list = buyTableAdapter1.FindList(t_id).ToString();
            string[] pro_count_list = b_list.Split('/'); //상품별로 상품번호, 갯수 만 나오게 슬라이싱
            for (int i = 0; i < pro_count_list.Length - 1; i++)
            {
                string[] divide = pro_count_list[i].Split('.');
                string[] divide2 = divide[0].Split(',');
                string pro_id = divide2[0];
                string pro_cnt = divide2[1];
                string p_price = divide[1];
                int price = Int32.Parse(pro_cnt) * Int32.Parse(p_price);
                string pro_name = productTableAdapter1.FindName(pro_id).ToString();

                listBox2.Items.Add(pro_name + "  ( 수량 : " + pro_cnt + " )\t" + price.ToString()+" 원");
            }
        }

        private void metroGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e) //회원 선택
        {
            listBox2.Items.Clear();
            string str = listBox1.SelectedItem.ToString();
            string[] str2 = str.Split('\t');
            c_name = str2[0];
            c_id = str2[1];

            label5.Text = c_name + "  님";
            

            //구매횟수, 반품횟수, 총 구매액 계산
            bUY_VIEWTableAdapter.FillByCid(dataSet11.BUY_VIEW, c_id);
            buy_view = dataSet11.Tables["BUY_VIEW"];

            int buy_cnt = 0, refund_cnt = 0, total = 0;

            foreach(DataRow dr in buy_view.Rows)
            {
                int price = Int32.Parse(dr["total"].ToString());
                if(price == 0)
                {
                    refund_cnt++; //환불 횟수
                }
                total += price; //총합 계산
            }
            buy_cnt = buy_view.Rows.Count;
            buy_cnt = buy_cnt - refund_cnt; //구매횟수 = 총 갯수 - 환불 횟수 (환불하고 다시 구매가 들어가니까. 환불 제외 구매 횟수)

            label9.Text = buy_cnt + " 번";
            label10.Text = refund_cnt + " 번";
            label11.Text = total + " 원";


        }

    }
}
