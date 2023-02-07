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
    public partial class Form6 : MetroForm
    {
        public Form6()
        {
            InitializeComponent();
        }

        string c_id, c_name, m_num;
        DataTable buy_table;

        public void SetData(string id, string name, string mart)
        {
            c_id = id;
            c_name = name;
            m_num = mart;
            label2.Text = c_name + " 님";
            label1.Text = m_num + " 매장";
        }
        private void Form6_Load(object sender, EventArgs e)
        {
            cUS_BUY_ID_MARTTableAdapter.FillByIdMart(dataSet1.CUS_BUY_ID_MART, c_id, m_num);
            buyTableAdapter1.Fill(dataSet11.BUY);
            productTableAdapter1.Fill(dataSet11.PRODUCT);
            refundTableAdapter1.Fill(dataSet11.REFUND);

            buy_table = dataSet11.Tables["BUY"];
            metroGrid1.Sort(metroGrid1.Columns[2], System.ComponentModel.ListSortDirection.Descending);
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();

            checkBoxColumn.HeaderText = "환불 상태";
            checkBoxColumn.Name = "check";
            checkBoxColumn.FalseValue = "0";
            checkBoxColumn.TrueValue = "1";

            metroGrid1.Columns.Add(checkBoxColumn);
            metroGrid1.Columns[5].Width = 80;

            for (int i = 0; i < metroGrid1.RowCount; i++)
            {
                if (refundTableAdapter1.ReturnState(metroGrid1.Rows[i].Cells[0].Value.ToString()) == "1")
                {
                    metroGrid1.Rows[i].Cells[5].Value = "1";
                }
                else
                {
                    metroGrid1.Rows[i].Cells[5].Value = "0";
                }
            }
            metroGrid1.ClearSelection();
        }
        private void metroGrid1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            metroGrid2.Rows.Clear();
            //dataGridView1.CurrentRow.Cells[0].Value.ToString()


            if (metroGrid1.CurrentRow.Cells[3].Value.ToString() == "0")
            {
                MessageBox.Show("환불된 구매입니다.");
            }
            else
            {
                string t_id = metroGrid1.CurrentRow.Cells[0].Value.ToString();
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
                    metroGrid2.Rows.Add("0", pro_name, pro_cnt, price.ToString());
                }
            }
            metroGrid2.ClearSelection();
        }
        


        private void button1_Click(object sender, EventArgs e)
        {
            //리뷰작성
            Form7 form7 = new Form7();

            string p_name = metroGrid2.CurrentRow.Cells[1].Value.ToString();
            string p_id = productTableAdapter1.FindPId(p_name);
            string t_id = metroGrid1.CurrentRow.Cells[0].Value.ToString();

            form7.SetData(c_id, m_num, p_name, t_id);
            if (metroGrid1.CurrentRow.Cells[4].Value.ToString() != "0")
            {
                if (reviewTableAdapter1.FindReview(c_id, p_id, t_id) != null)
                {
                    form7.ShowReview(true);
                }

                form7.ShowDialog();
            }
            else
            {
                MessageBox.Show("아직 구매 요청이 수락되지 않았습니다");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //환불 목록 이동
            if (metroGrid1.CurrentRow.Cells[4].Value.ToString() == "0") //구매요청승인이 되지 않은 구매는 환불불가
            {
                MessageBox.Show("아직 구매 요청이 승인되지 않았습니다.");
            }
            else if (metroGrid1.CurrentRow.Cells[4].Value.ToString() == "1")
            {
                if (metroGrid1.CurrentRow.Cells[5].Value.ToString() == "1") //이미 환불한 영수증은 다시 환불 불가
                {
                    MessageBox.Show("이미 환불된 영수증 입니다.");
                }
                else
                {
                    string remove_list = "";

                    for (int i = 0; i < metroGrid2.RowCount; i++)
                    {
                        if (metroGrid2.Rows[i].Cells[0].Value.ToString() == "1") //체크된 상품 찾기
                        {
                            string pro_name = metroGrid2.Rows[i].Cells[1].Value.ToString();
                            int price = Int32.Parse(metroGrid2.Rows[i].Cells[3].Value.ToString());
                            price = price / Int32.Parse(metroGrid2.Rows[i].Cells[2].Value.ToString());

                            if (metroGrid3.RowCount == 0)
                            {
                                metroGrid3.Rows.Add("0", pro_name, "1", price.ToString());
                            }
                            else
                            {
                                int rcnt = metroGrid3.RowCount;
                                bool result = false;
                                for (int j = 0; j < rcnt; j++) //체크된 상품 추가
                                {
                                    if (metroGrid3.Rows[j].Cells[1].Value.ToString() == pro_name) //이미 환불 목록에 있으면
                                    {
                                        result = true;
                                        int pro_count = Int32.Parse(metroGrid3.Rows[j].Cells[2].Value.ToString());
                                        pro_count += 1;
                                        metroGrid3.Rows[j].Cells[2].Value = pro_count.ToString();
                                        metroGrid3.Rows[j].Cells[3].Value = (pro_count * price).ToString();
                                    }
                                }
                                if (result == false) //상품 목록에서 없으면
                                {
                                    metroGrid3.Rows.Add("0", pro_name, "1", price);
                                }

                            }
                            //지워야할 상품 목록 적기
                            remove_list = remove_list + pro_name + ",";
                            int cnt = Int32.Parse(metroGrid2.Rows[i].Cells[2].Value.ToString());
                            cnt -= 1;
                            metroGrid2.Rows[i].Cells[2].Value = cnt.ToString();
                            metroGrid2.Rows[i].Cells[3].Value = (cnt * price).ToString();
                        }
                    }

                    //구매 상품 목록에서 상품 삭제
                    string[] delete_list = remove_list.Split(','); //상품 목록 분리
                    for (int i = 0; i < delete_list.Length - 1; i++)
                    {
                        string product = delete_list[i];
                        for (int k = 0; k < metroGrid2.RowCount; k++)
                        {
                            if (metroGrid2.Rows[k].Cells[1].Value.ToString() == product)
                            {
                                if (metroGrid2.Rows[k].Cells[2].Value.ToString() == "0")
                                {
                                    metroGrid2.Rows.Remove(metroGrid2.Rows[k]);
                                }
                            }
                        }
                    }
                }
            }
            metroGrid3.ClearSelection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //삭제
            string remove_list = "";

            for (int i = 0; i < metroGrid3.RowCount; i++)
            {
                if (metroGrid3.Rows[i].Cells[0].Value.ToString() == "1") //체크된 상품 찾기
                {
                    string pro_name = metroGrid3.Rows[i].Cells[1].Value.ToString();
                    int price = Int32.Parse(metroGrid3.Rows[i].Cells[3].Value.ToString());
                    price = price / Int32.Parse(metroGrid3.Rows[i].Cells[2].Value.ToString());

                    if (metroGrid2.RowCount == 0)
                    {
                        metroGrid2.Rows.Add("0", pro_name, "1");
                    }
                    else
                    {
                        int rcnt = metroGrid2.RowCount;
                        bool result = false;
                        for (int j = 0; j < rcnt; j++) //체크된 상품 추가
                        {
                            if (metroGrid2.Rows[j].Cells[1].Value.ToString() == pro_name) //이미 구매 목록에 있으면
                            {
                                result = true;
                                int pro_count = Int32.Parse(metroGrid2.Rows[j].Cells[2].Value.ToString());
                                pro_count += 1;
                                metroGrid2.Rows[j].Cells[2].Value = pro_count.ToString();
                                metroGrid2.Rows[j].Cells[3].Value = (pro_count * price).ToString();
                            }
                        }
                        if (result == false) //상품 목록에서 없으면
                        {
                            metroGrid2.Rows.Add("0", pro_name, "1", price);
                        }

                    }
                    //지워야할 상품 목록 적기
                    remove_list = remove_list + pro_name + ",";
                    int cnt = Int32.Parse(metroGrid3.Rows[i].Cells[2].Value.ToString());
                    cnt -= 1;
                    metroGrid3.Rows[i].Cells[2].Value = cnt.ToString();
                    metroGrid3.Rows[i].Cells[3].Value = (cnt * price).ToString();
                }
            }

            //구매 상품 목록에서 상품 삭제
            string[] delete_list = remove_list.Split(','); //상품 목록 분리
            for (int i = 0; i < delete_list.Length - 1; i++)
            {
                string product = delete_list[i];
                for (int k = 0; k < metroGrid3.RowCount; k++)
                {
                    if (metroGrid3.Rows[k].Cells[1].Value.ToString() == product)
                    {
                        if (metroGrid3.Rows[k].Cells[2].Value.ToString() == "0")
                        {
                            metroGrid3.Rows.Remove(metroGrid3.Rows[k]);
                        }
                        else
                        {
                            int cnt = Int32.Parse(metroGrid3.Rows[k].Cells[2].Value.ToString());
                            cnt -= 1;
                            metroGrid3.Rows[k].Cells[2].Value = cnt.ToString();
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string t_id = metroGrid1.CurrentRow.Cells[0].Value.ToString();
            string b_list = "";
            string new_b_list = "";
            string state = buyTableAdapter1.FindState(t_id);

            if (state == "0") // 구매 요청이 수락되지 않은 구매는 환불 불가
            {
                MessageBox.Show("아직 구매 요청이 수락되지 않았습니다.");
            }

            else
            {
                if (metroGrid3.RowCount < 0)
                {
                    MessageBox.Show("선택된 상품이 없습니다.");
                }
                else
                {
                    for (int i = 0; i < metroGrid3.RowCount; i++)
                    {
                        string product_name = metroGrid3.Rows[i].Cells[1].Value.ToString();
                        string p_id = productTableAdapter1.FindPId(product_name);
                        string cnt = metroGrid3.Rows[i].Cells[2].Value.ToString();
                        b_list = b_list + p_id + "," + cnt + "/";
                    }
                    refundTableAdapter1.InsertQuery(t_id, b_list, c_id, "0"); //환불 요청

                    for (int i = 0; i < metroGrid2.RowCount; i++)
                    {
                        string product_name = metroGrid2.Rows[i].Cells[1].Value.ToString();
                        string p_id = productTableAdapter1.FindPId(product_name);
                        string cnt = metroGrid2.Rows[i].Cells[2].Value.ToString();
                        int price = Int32.Parse(metroGrid2.Rows[i].Cells[3].Value.ToString());
                        price = price / Int32.Parse(cnt);

                        new_b_list = new_b_list + p_id + "," + cnt + "." + price.ToString() + "/";
                    }
                    buyTableAdapter1.RefundUpdate(new_b_list, 0, t_id, m_num); //구매목록 업데이트

                    int numOfRows = refundTableAdapter1.Update(dataSet11.REFUND);
                    int numOfRows2 = buyTableAdapter1.Update(dataSet11.BUY);
                    if (numOfRows < 1 && numOfRows2 < 1)
                    {
                        // + 구매에서 구매완료 됐는지 확인하고, blist비우고, 상태 확인
                        refundTableAdapter1.Update(dataSet11.REFUND);
                        buyTableAdapter1.Update(dataSet11.BUY);
                        cUS_BUY_ID_MARTTableAdapter.FillByIdMart(dataSet1.CUS_BUY_ID_MART, c_id, m_num);
                        metroGrid3.Rows.Clear();
                        MessageBox.Show("환불 요청 완료");
                    }
                    else
                    {
                        MessageBox.Show("환불 요청 실패");
                    }
                }

            }
        }

    }
}
