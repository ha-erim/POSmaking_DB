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
    public partial class Form10 : MetroForm //신규 상품 입고창
    {

        public Form10()
        {
            InitializeComponent();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            productTableAdapter1.Fill(dataSet11.PRODUCT);
            sellTableAdapter1.Fill(dataSet11.SELL);

        }

        private void button1_Click(object sender, EventArgs e) //입고
        {
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox3.Checked && !checkBox4.Checked && !checkBox5.Checked)
            {
                MessageBox.Show("매장을 선택하세요");
            }
            else
            {
                //상품 정보
                string p_kind = textBox1.Text;
                string p_name = textBox2.Text;
                int price = Int32.Parse(textBox4.Text);
                int cnt = Int32.Parse(textBox3.Text);
                string p_id = productTableAdapter1.GetNextId().ToString();

                if(productTableAdapter1.FindPId(p_name) != null) //상품은 이미 존재할 때
                {
                    p_id = productTableAdapter1.FindPId(p_name);
                }
                else //완전히 새로운 상품일 때
                {
                    //product table채우기
                    productTableAdapter1.InsertQuery(p_id, p_name, p_kind);
                    //DB 동기화
                    int numOfRows = productTableAdapter1.Update(dataSet11.PRODUCT);
                    if (numOfRows > 1) MessageBox.Show("product table update fail");
                    //else MessageBox.Show("product table fail");
                }

                //입고할 매장 찾기
                string mart = "";
                if (checkBox1.Checked) mart = mart + "서울,";
                if (checkBox2.Checked) mart = mart + "인천,";
                if (checkBox3.Checked) mart = mart + "대구,";
                if (checkBox4.Checked) mart = mart + "경주,";
                if (checkBox5.Checked) mart = mart + "부산,";

                //sell에 추가하기
                string[] slice_mart = mart.Split(',');
                for (int i = 0; i < slice_mart.Length - 1; i++)
                {
                    string m_num = slice_mart[i];
                    if (producT_SELL_VIEWTableAdapter1.FindSameProduct(p_name, m_num) != 0)
                    {
                        MessageBox.Show(m_num + "매장에 입고되어 있는 상품입니다.");
                    }
                    else
                    {
                        sellTableAdapter1.InsertQuery(m_num, cnt, price, p_id, 0);
                    }
                }

                //DB 동기화
                int numOfRows2 = sellTableAdapter1.Update(dataSet11.SELL);
                if (numOfRows2 < 1)
                {
                    MessageBox.Show("상품 입고 되었습니다.");
                    //textBox1.Clear();
                    textBox2.Clear();
                    //textBox3.Clear();
                    //textBox4.Clear();
                    //checkBox1.Checked = false;
                    //checkBox2.Checked = false;
                    //checkBox3.Checked = false;
                    //checkBox4.Checked = false;
                    //checkBox5.Checked = false;
                }
                else MessageBox.Show("상품 입고 실패하였습니다.");
            }
        }

    }
}
