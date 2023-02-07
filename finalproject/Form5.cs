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

    public partial class Form5 : MetroForm
    {
        DataTable buy_table;
        DataTable buy_view_table;
        DataTable sell_table;

        string s_id, m_num;
        public Form5()
        {
            InitializeComponent();
        }
        public void SetData(string id, string mart)
        {
            s_id = id;
            m_num = mart;
            label1.Text = mart + " 매장";
            string s_name = sellerTableAdapter1.FindName(s_id);
            label2.Text = s_name + " 님";
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'dataSet11.BUY_REFUND_VIEW' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.

            buyTableAdapter1.Fill(dataSet11.BUY);
            sellerTableAdapter1.Fill(dataSet11.SELLER);
            m_SELLERTableAdapter.FillByMart(dataSet11.M_SELLER, m_num);
            bUY_VIEWTableAdapter.FillByMart(dataSet11.BUY_VIEW, m_num);
            bUY_REFUND_VIEWTableAdapter.FillByMart(dataSet11.BUY_REFUND_VIEW, m_num);


            buy_table = dataSet11.Tables["BUY"];
            buy_view_table = dataSet11.Tables["BUY_VIEW"];
            sell_table = dataSet11.Tables["SELL"];

            buy_request_cnt();
            refund_request_cnt();
            
            metroGrid1.Sort(metroGrid1.Columns[3], System.ComponentModel.ListSortDirection.Descending);
            metroGrid2.Sort(metroGrid2.Columns[1], System.ComponentModel.ListSortDirection.Descending);
            metroGrid1.ClearSelection();
            metroGrid2.ClearSelection();
            pictureBox1.BringToFront();
        }

        private void buy_request_cnt()
        {
            if (metroGrid1.RowCount > 0)
            {
                //직원, 고객,구매번호, 구매일자, 총 금액, 구매완료
                int count = 0;
                for (int i = 0; i < metroGrid1.RowCount; i++)
                {
                    int state_cnt = Int32.Parse(metroGrid1.Rows[i].Cells[5].Value.ToString());
                    if (state_cnt == 0)
                    {
                        count++;
                    }
                }
                metroTabPage1.Text = "구매 요청 처리 ( " + count.ToString() + " )";
            }
            else
            {
                metroTabPage1.Text = "구매 요청 처리 ( " + 0 + " )";
            }
        }

        private void refund_request_cnt()
        {
            if (metroGrid2.RowCount > 0)
            {
                //직원, 고객,구매번호, 구매일자, 총 금액, 구매완료
                int count = 0;
                for (int i = 0; i < metroGrid2.RowCount; i++)
                {
                    int state_cnt = Int32.Parse(metroGrid2.Rows[i].Cells[3].Value.ToString());
                    if (state_cnt == 0)
                    {
                        count++;
                    }
                }
                metroTabPage2.Text = "환불 요청 처리 ( " + count.ToString() + " )";
            }
            else
            {
                metroTabPage2.Text = "환불 요청 처리 ( " + 0 + " )";
            }
        }
        private void button1_Click(object sender, EventArgs e) // 구매 요청 수락
        {
            if (metroGrid1.RowCount > 0)
            {
                //직원, 고객,구매번호, 구매일자, 총 금액, 구매완료
                for (int i = 0; i < metroGrid1.RowCount; i++)
                {
                    if (metroGrid1.Rows[i].Cells[5].Value.ToString() == "1")
                    {
                        string buy_accept_seller = metroGrid1.Rows[i].Cells[0].Value.ToString();
                        DateTime datetime = Convert.ToDateTime(metroGrid1.Rows[i].Cells[3].Value.ToString());
                        string c_id = metroGrid1.Rows[i].Cells[1].Value.ToString();
                        string t_id = metroGrid1.Rows[i].Cells[2].Value.ToString();
                        string state = metroGrid1.Rows[i].Cells[5].Value.ToString();
                        buyTableAdapter1.BuyUpdate(buy_accept_seller, datetime, state, c_id, t_id);
                    }
                }
                int numOfRows = refundTableAdapter1.Update(dataSet11.REFUND);
                int numOfRows2 = buyTableAdapter1.Update(dataSet11.BUY);
                int numofRows3 = sellTableAdapter1.Update(dataSet11.SELL);

                if (numOfRows < 1 && numOfRows2 < 1 && numofRows3 < 1)
                {
                    MessageBox.Show("업데이트 완료");
                    refundTableAdapter1.Fill(dataSet11.REFUND);
                    buyTableAdapter1.Fill(dataSet11.BUY);
                    sellTableAdapter1.Fill(dataSet11.SELL);
                    bUY_REFUND_VIEWTableAdapter.FillByMart(dataSet11.BUY_REFUND_VIEW, m_num);
                    buy_request_cnt();
                    refund_request_cnt();

                }
                else
                {
                    MessageBox.Show("업데이트 실패");
                }
            }
            else
            {
                MessageBox.Show("구매 요청이 없습니다.");
            }

        }

        private void button2_Click(object sender, EventArgs e) //환불 요청 업데이트
        {
            if (metroGrid2.RowCount > 0)
            {
                //직원, 고객,구매번호, 구매일자, 총 금액, 구매완료
                for (int i = 0; i < metroGrid2.RowCount; i++)
                {
                    string state = metroGrid2.Rows[i].Cells[3].Value.ToString();
                    string t_id = metroGrid2.Rows[i].Cells[2].Value.ToString();
                    if (refundTableAdapter1.ReturnState(t_id).ToString() == "0" && state == "1") //환불처리가 되지 않았고, 체크가 됐을 때
                    {
                        //refund table에서 환불 상태 확인
                        string r_list = refundTableAdapter1.FindRList(t_id);
                        string[] r_pro_count_list = r_list.Split('/'); //상품아이디, 갯수. 가격
                        for (int k = 0; k < r_pro_count_list.Length - 1; k++)
                        {
                            string[] divide = r_pro_count_list[k].Split('.');

                            string[] divide2 = divide[0].Split(',');
                            string pro_id = divide2[0];
                            string pro_cnt = divide2[1];
                            //string p_price = divide[1];

                            int cnt = Int32.Parse(sellTableAdapter1.FindRefund(pro_id, m_num).ToString());
                            cnt += Int32.Parse(pro_cnt);

                            sellTableAdapter1.RefundUpdate(cnt.ToString(), pro_id, m_num);


                        }
                        //buyTableAdapter1.BuyUpdate(buy_accept_seller, datetime, state, c_id, t_id);
                        refundTableAdapter1.StateUpdate(state, t_id);


                        if (buyTableAdapter1.FindList(t_id) != null)
                        {
                            DateTime date = DateTime.Parse(metroGrid2.Rows[i].Cells[1].Value.ToString());
                            string b_list = buyTableAdapter1.FindList(t_id).ToString();
                            string c_id = metroGrid2.Rows[i].Cells[0].Value.ToString();
                            string new_t_id = buyTableAdapter1.NextSeqValue().ToString();
                            //총합 계산
                            int bank = 0;
                            string[] pro_count_list = b_list.Split('/'); //상품별로 상품번호, 갯수 만 나오게 슬라이싱
                            for (int j = 0; j < pro_count_list.Length - 1; j++)
                            {
                                string[] divide = pro_count_list[j].Split('.');

                                string[] divide2 = divide[0].Split(',');
                                string pro_id = divide2[0];
                                string pro_cnt = divide2[1];
                                int p_price = Int32.Parse(divide[1]);

                                //DataRow[] selected_product_price = sell_table.Select("[P_ID] = '" + pro_id + "'");
                                //DataRow real_price = selected_product_price[0];

                                //int price = Int32.Parse(sellTableAdapter1.FindPrice(m_num, pro_id).ToString());
                                p_price = Int32.Parse(pro_cnt) * p_price;
                                bank += p_price;
                            }
                            buyTableAdapter1.InsertQuery(c_id, new_t_id, b_list,date, bank, m_num, "0");
                        }
                    }

                }

                int numOfRows = refundTableAdapter1.Update(dataSet11.REFUND);
                int numOfRows2 = buyTableAdapter1.Update(dataSet11.BUY);
                int numofRows3 = sellTableAdapter1.Update(dataSet11.SELL);

                if (numOfRows < 1 && numOfRows2 < 1 && numofRows3 < 1)
                {
                    MessageBox.Show("업데이트 완료");
                    refundTableAdapter1.Fill(dataSet11.REFUND);
                    buyTableAdapter1.Fill(dataSet11.BUY);
                    sellTableAdapter1.Fill(dataSet11.SELL);
                    bUY_REFUND_VIEWTableAdapter.FillByMart(dataSet11.BUY_REFUND_VIEW, m_num);
                    bUY_VIEWTableAdapter.FillByMart(dataSet11.BUY_VIEW, m_num);
                    buy_request_cnt();
                    refund_request_cnt();
                }
                else
                {
                    MessageBox.Show("업데이트 실패");
                }
            }
            else
            {
                MessageBox.Show("환불 요청이 없습니다.");
            }
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.IsBalloon = true;
            this.toolTip1.SetToolTip(this.pictureBox1, "로그아웃");
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) //dgv에 달력 뜨게
        {
            if (e.RowIndex >= 0) //헤더 제외
            {
                if (e.ColumnIndex == 3) //구매 일자 일때만 타임피커 뜨도록
                {
                    DateTimePicker dtp = new DateTimePicker();
                    dtp.Format = DateTimePickerFormat.Short;
                    dtp.Visible = true;

                    if (metroGrid1.CurrentRow.Cells[3].Value.ToString() != "")
                    {
                        dtp.Value = DateTime.Parse(metroGrid1.CurrentRow.Cells[3].Value.ToString());
                    }
                    else
                    {
                        dtp.Value = DateTime.Now;
                    }
                    var rect = metroGrid1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    dtp.Size = new Size(rect.Width, rect.Height);
                    dtp.Location = new Point(rect.X, rect.Y);
                    metroGrid1.Controls.Add(dtp);

                    dtp.CloseUp += new EventHandler(dtp_CloseUp);
                    dtp.TextChanged += new EventHandler(dtp_OnTextChange);
                }
            }
        }

        private void dtp_OnTextChange(Object sender, EventArgs e)
        {
            metroGrid1.CurrentRow.Cells[3].Value = ((DateTimePicker)sender).Text.ToString();
        }
        private void dtp_CloseUp(object sender, EventArgs e)
        {
            ((DateTimePicker)sender).Visible = false;
            metroGrid1.Controls.Remove((DateTimePicker)sender);
        }

        private void metroTabPage1_Click(object sender, EventArgs e)
        {

        }

        private void metroGrid1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) //헤더 제외
            {
                if (e.ColumnIndex == 3) //구매 일자 일때만 타임피커 뜨도록
                {
                    DateTimePicker dtp = new DateTimePicker();
                    dtp.Format = DateTimePickerFormat.Short;
                    dtp.Visible = true;

                    if (metroGrid1.CurrentRow.Cells[3].Value.ToString() != "")
                    {
                        dtp.Value = DateTime.Parse(metroGrid1.CurrentRow.Cells[3].Value.ToString());
                    }
                    else
                    {
                        dtp.Value = DateTime.Now;
                    }
                    var rect = metroGrid1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    dtp.Size = new Size(rect.Width, rect.Height);
                    dtp.Location = new Point(rect.X, rect.Y);
                    metroGrid1.Controls.Add(dtp);

                    dtp.CloseUp += new EventHandler(dtp_CloseUp);
                    dtp.TextChanged += new EventHandler(dtp_OnTextChange);
                }
            }
        }

        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var datagridview = sender as DataGridView;
            if(e.ColumnIndex == 0)
            {
                datagridview.BeginEdit(true);
                ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            List<Form> formsToHide = new List<Form>();

            foreach (Form form in Application.OpenForms)
            {
                formsToHide.Add(form);
            }

            formsToHide.ForEach(f => f.Hide());

            Form1 form1 = new Form1();
            form1.ShowDialog();
            this.Close();
        }
    }
}
