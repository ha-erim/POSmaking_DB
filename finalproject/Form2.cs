using System;
using MetroFramework.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace finalproject
{
    public partial class Form2 : MetroForm
    {
        DataTable product_table;
        DataTable basket_table;
        DataTable pro_basket_table;
        DataTable sell_table;
        DataTable sell_pro_table;
        DataTable buy_table;
        DataTable seller_table;
        DataTable notice;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label1.BringToFront();
            label5.BringToFront();
            // TODO: 이 코드는 데이터를 'dataSet11.PRO_BASKET' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.pRO_BASKETTableAdapter.FillByCusNMart(this.dataSet11.PRO_BASKET, c_id, m_num);
            // TODO: 이 코드는 데이터를 'dataSet1.BASKET' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.basketTableAdapter1.Fill(this.dataSet1.BASKET);
            // TODO: 이 코드는 데이터를 'dataSet1.PRODUCT' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.productTableAdapter1.Fill(this.dataSet1.PRODUCT);
            producT_SELL_VIEWTableAdapter1.FillByMart(this.dataSet11.PRODUCT_SELL_VIEW, m_num);
            sellTableAdapter1.Fill(dataSet11.SELL);
            productTableAdapter1.Fill(dataSet11.PRODUCT);
            sellerTableAdapter1.Fill(dataSet11.SELLER);
            buyTableAdapter1.Fill(dataSet11.BUY);


            product_table = dataSet11.Tables["PRODUCT"];
            basket_table = dataSet11.Tables["BASKET"];
            pro_basket_table = dataSet11.Tables["PRO_BASKET"];
            sell_table = dataSet11.Tables["SELL"];
            sell_pro_table = dataSet11.Tables["PRODUCT_SELL_VIEW"];
            seller_table = dataSet11.Tables["SELLER"];
            buy_table = dataSet11.Tables["BUY"];

            
            //총합
            if (pRO_BASKETTableAdapter.CalculateTotal(c_id, m_num) != null)
            {
                string total = pRO_BASKETTableAdapter.CalculateTotal(c_id, m_num).ToString();
                label7.Text = total + " 원";
            }

            //상품 종류 추가
            producT_SELL_VIEWTableAdapter1.InsertKind(dataSet11.PRODUCT_SELL_VIEW, m_num);
            foreach (DataRow dr in sell_pro_table.Rows)
            {
                comboBox1.Items.Add(dr["KND"].ToString());
            }
            producT_SELL_VIEWTableAdapter1.FillByMart(this.dataSet11.PRODUCT_SELL_VIEW, m_num); //테이블 다시 채우기


            

            if (pro_basket_table.Rows.Count > 0)
            {
                DataRow dr = pro_basket_table.Rows[0];
                string p_name = dr["P_NAME"].ToString();
                string p_id = productTableAdapter1.FindPId(p_name);
                int price = Int32.Parse(sellTableAdapter1.FindPrice(m_num, p_id).ToString());
                int cheapest = 0;
                string cheapest_product = "";

                string kind = productTableAdapter1.FIndKind(p_id) ;
                DataRow[] mydataRow = sell_pro_table.Select("[P_KIND] = '" + kind + "'");
                if (mydataRow.Length > 0)
                {
                    foreach (DataRow dr2 in mydataRow)
                    {
                        string product_name = dr2["p_name"].ToString();
                        string pp_id = productTableAdapter1.FindPId(product_name).ToString();
                        int pprice = Int32.Parse(sellTableAdapter1.FindPrice(m_num, pp_id).ToString());
                        if (cheapest == 0 || pprice < cheapest)
                        {
                            cheapest = pprice;
                            cheapest_product = product_name;
                        }
                    }
                }
                bool exist = false;
                foreach(DataRow data in pro_basket_table.Rows)
                {
                    string name = data["P_NAME"].ToString();
                    if(name == cheapest_product)
                    {
                        exist = true;
                    }
                }
                if(exist == false && cheapest < price)
                {
                    label11.Text = p_name + " 보다 " + (price - cheapest).ToString() + "원 더 저렴한 " + cheapest_product + " 제품은 어떠세요?";
                }

            }

           
            //공지 this.Width * 2
            label8.Top = 37;
            label8.Left = 984;
            label9.Top = 37;
            label9.Left = 984 * 2;
            label10.Top = 37;
            label10.Left = 984 * 3;
            timer1.Enabled = true;

            string str_notice = m_num +" 매장 가격 변경 공지";
            changE_PRICETableAdapter1.FillByMart(dataSet11.CHANGE_PRICE, m_num);
            notice = dataSet11.Tables["CHANGE_PRICE"];
            foreach (DataRow dr in notice.Rows)
            {
                DateTime change_day = DateTime.Parse(dr["C_DATE"].ToString());
                DateTime middle_day = date.AddDays(-1);
                DateTime end_day = date.AddDays(-2);
                int same_day = DateTime.Compare(date, change_day);
                int yesterday = DateTime.Compare(change_day, middle_day);
                int three_days_ago = DateTime.Compare(change_day, end_day);
                if (same_day == 0 || yesterday == 0 || three_days_ago == 0)
                {
                    string pro_name = productTableAdapter1.FindName(dr["P_ID"].ToString());
                    str_notice +="  (" + change_day.ToString("MM월 dd일")+ ") "+pro_name +" : "
                        + dr["N_PRICE"].ToString() + " 원 → "
                        + sellTableAdapter1.FindPrice(m_num, dr["P_ID"].ToString()) + " 원";
                }

            }
            label8.Text = str_notice;
            label9.Text = str_notice;
            label10.Text = str_notice;

            metroGrid1.ClearSelection();
        }


        DateTime date;
        string c_id, c_name, m_num;

        public void SetData(string id, string name, string mart,DateTime cal)
        {
            c_id = id;
            c_name = name;
            m_num = mart;
            date = cal;
            label1.Text = c_id + " ( " + c_name + " 님)";
            label5.Text = m_num + " 매장";
        }

        private void show_product()
        {
            //sellTableAdapter1.Fill(this.dataSet11.SELL); //sell table 다시 채우기
            //sell_table = dataSet11.Tables["SELL"];
            producT_SELL_VIEWTableAdapter1.FillByMart(this.dataSet11.PRODUCT_SELL_VIEW, m_num);
            sell_pro_table = dataSet11.Tables["PRODUCT_SELL_VIEW"];

            checkedListBox1.Items.Clear();

            if (comboBox1.SelectedItem != null)
            {
                string kind = comboBox1.SelectedItem.ToString();
                DataRow[] mydataRow = sell_pro_table.Select("[P_KIND] = '" + kind + "'");
                if (mydataRow.Length > 0)
                {
                    foreach (DataRow mydataRows in mydataRow)
                    {
                        if (mydataRows["S_COUNT"].ToString() != "0")
                        {
                            checkedListBox1.Items.Add(mydataRows["P_NAME"].ToString() + "\t( 수량 : " + mydataRows["S_COUNT"] + " )");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("선택된 상품이 없습니다.");
                }
            }
        }


        private void button2_Click(object sender, EventArgs e) //장바구니 담기
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("선택된 상품이 없습니다");
            }
            else
            {
                //basketBindingSource.AddNew();
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    string[] real_product_name = checkedListBox1.CheckedItems[i].ToString().Split('\t');
                    //real_product_name[0] = 상품 이름

                    DataRow[] item = pro_basket_table.Select("[P_NAME] = '" + real_product_name[0] + "'");
                    // pro_basket_table.AcceptChanges();

                    if (item.Length > 0) //찾은 값의 존재 여부 확인
                    {
                        //장바구니에 있던 상품 가격이랑 갯수 업데이트
                        pro_basket_table = dataSet11.Tables["PRO_BASKET"];
                        foreach (DataRow dr in pro_basket_table.Rows)
                        {
                            if (dr["P_NAME"].ToString() == real_product_name[0].ToString())
                            {
                                int cnt = Int32.Parse(dr["B_COUNT"].ToString());
                                cnt += 1;

                                string p_id = productTableAdapter1.FindPId(dr["P_NAME"].ToString());

                                // 날짜에 맞게 변경 가격 찾기
                                //changE_PRICETableAdapter1.ReturnOldPrice(dataSet11.CHANGE_PRICE); //최근 날짜 순으로 정렬
                                //DataTable dt = dataSet11.Tables["CHANGE_PRICE"];
                                //if(dt.Rows.Count > 0)
                                //{
                                //    for(int j = 0; j<dt.Rows.Count; j+=2)
                                //    {
                                      
                                //    }
                                //}
                                int price = Int32.Parse(sellTableAdapter1.FindPrice(m_num, p_id).ToString());
                                price = cnt * price;
                                basketTableAdapter1.UpdateQuery(cnt, price, p_id, m_num);
                                sellTableAdapter1.S_Count_Down(p_id, m_num); //sell table count 감소
                            }

                        }

                    }
                    else
                    {
                        //장바구니에 없던 상품 추가
                        DataRow[] selected_product = product_table.Select("[P_NAME] = '" + real_product_name[0] + "'");
                        DataRow product = selected_product[0];

                        basketTableAdapter1.InsertQuery(c_id, product[0].ToString(), 1, Int32.Parse(sellTableAdapter1.FindPrice(m_num, product[0].ToString()).ToString()), m_num, product[2].ToString());
                        sellTableAdapter1.S_Count_Down(product[0].ToString(), m_num); //sell table count 감소
                    }

                    //total
                    string total = pRO_BASKETTableAdapter.CalculateTotal(c_id, m_num).ToString();
                    label7.Text = total + " 원";
                }

                //DB 동기화
                int numOfRows = basketTableAdapter1.Update(dataSet11.BASKET);
                int numOfRows_2 = sellTableAdapter1.Update(dataSet11.SELL);
                if (numOfRows < 1 && numOfRows_2 < 1)
                {
                    MessageBox.Show("장바구니 담기 성공");
                    pRO_BASKETTableAdapter.FillByCusNMart(this.dataSet11.PRO_BASKET, c_id, m_num); //pro_basket_view 다시 채우기
                    show_product();

                    //전체 선택 체크박스 해제
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                    }
                    checkBox1.Checked = false;
                }
                else
                {
                    MessageBox.Show("장바구니 담기 실패");

                }
                
            }
            metroGrid1.ClearSelection();
        }



        private void button3_Click(object sender, EventArgs e) //선택삭제
        {

            string delete_product = metroGrid1.CurrentRow.Cells[0].Value.ToString();
            string p_id = productTableAdapter1.FindPId(delete_product);
            //product[0] = p_id;
            basketTableAdapter1.DeleteQuery(p_id);
            pROBASKETBindingSource.RemoveCurrent();
            sellTableAdapter1.S_Count_Up(p_id, m_num); //sell table count 증가

            int numOfRows = basketTableAdapter1.Update(dataSet11.BASKET);
            int numOfRows_2 = sellTableAdapter1.Update(dataSet11.SELL);

            //total
            string total = basketTableAdapter1.CalculateTotal(c_id, m_num).ToString();
            label7.Text = total + " 원";

            if (numOfRows < 1 && numOfRows_2 < 1)
            {
                MessageBox.Show("삭제 성공");
                pRO_BASKETTableAdapter.FillByCusNMart(this.dataSet11.PRO_BASKET, c_id, m_num);
                show_product();
            }
            else
            {
                MessageBox.Show("삭제 실패");

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) //상품목록 전체선택
        {
            if (checkBox1.Checked == true)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                }

            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                }
                checkBox1.Checked = false;

            }
        }

        private void button6_Click(object sender, EventArgs e) //구매하기
        {
            //c_id, t_id, s_id, b_list, total, m_num, state - 0 : 요청 대기 / 1 : 구매 완료
            if (pro_basket_table.Rows != null)
            {
                int cnt_kind = Int32.Parse(basketTableAdapter1.CalculateKind(dataSet11.BASKET, c_id, m_num).ToString());
                if (cnt_kind >= 1) //상품의 종류 갯수
                {
                    int total = 0;
                    string p_id = "";
                    if (pRO_BASKETTableAdapter.CalculateTotal(c_id, m_num) != null)
                    {
                        total = Int32.Parse(pRO_BASKETTableAdapter.CalculateTotal(c_id, m_num).ToString());
                    }

                    string t_id = buyTableAdapter1.NextSeqValue().ToString();
                    string b_list = "";
                    foreach (DataRow dr in pro_basket_table.Rows)
                    {
                        p_id = productTableAdapter1.FindPId(dr["P_NAME"].ToString());
                        string p_price = sellTableAdapter1.FindPrice(m_num, p_id).ToString();
                        b_list = b_list + p_id + "," + dr["B_COUNT"].ToString() + "." + p_price + "/";
                    }

                    buyTableAdapter1.InsertQuery(c_id, t_id, b_list, date, total, m_num, "0");
                    basketTableAdapter1.BuyAfterDelete(c_id, m_num);

                    label7.Text = "0 원";

                    int numOfRows = basketTableAdapter1.Update(dataSet11.BASKET);
                    int numOfRows_2 = buyTableAdapter1.Update(dataSet11.BUY);

                    if (numOfRows < 1 && numOfRows_2 < 1)
                    {
                        MessageBox.Show("구매 요청 완료");
                        pRO_BASKETTableAdapter.FillByCusNMart(this.dataSet11.PRO_BASKET, c_id, m_num);
                    }
                    else
                    {
                        MessageBox.Show("구매 요청 실패");
                    }

                }
                else
                {
                    MessageBox.Show("상품 종류가 " + basketTableAdapter1.CalculateKind(dataSet11.BASKET, c_id, m_num).ToString() + "개 입니다.");
                }

            }
            else
            {
                MessageBox.Show("장바구니가 비었습니다.");
            }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // 콤보박스 아이템 선택하면 자동으로 채우기
        {
            show_product();
        }

        private void pictureBox2_Click(object sender, EventArgs e) //주문 내역 확인 / 환불 요청
        {
            Form6 form6 = new Form6();
            form6.SetData(c_id, c_name, m_num);
            form6.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e) //로그아웃
        {
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

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.IsBalloon = true;
            this.toolTip1.SetToolTip(this.pictureBox3, "로그아웃");
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            this.toolTip2.IsBalloon = true;
            this.toolTip2.SetToolTip(this.pictureBox2, "구매내역 / 환불");
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            this.toolTip3.IsBalloon = true;
            this.toolTip3.SetToolTip(this.pictureBox1, "설정");
        }
      
        private void timer1_Tick(object sender, EventArgs e)
        {
            label8.Left = label8.Left - 1;

            label9.Left = label9.Left - 1;

            label10.Left = label10.Left - 1;



            if (label8.Left < -label10.Width)

                label8.Left = this.Width * 3 - label10.Width;

            if (label9.Left < -label10.Width)

                label9.Left = this.Width * 3 - label10.Width;

            if (label10.Left < -label10.Width)

                label10.Left = this.Width * 3 - label10.Width;
        }


        private void checkedListBox1_MouseDoubleClick(object sender, MouseEventArgs e) //리뷰보기
        {
            string name_count = checkedListBox1.SelectedItem.ToString();
            string[] cut_name = name_count.Split('\t');
            string pro_name = cut_name[0];

            Form8 form8 = new Form8();
            form8.ReceiveData(pro_name);
            form8.Show();


        
        }


        private void pictureBox1_Click(object sender, EventArgs e) //설정버튼
        {
            Form4 form4 = new Form4();
            form4.SetData(c_id, c_name);
            form4.ShowDialog();
        }
    }
}

