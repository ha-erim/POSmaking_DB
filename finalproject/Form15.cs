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
    public partial class Form15 : MetroForm
    {
        public Form15()
        {
            InitializeComponent();
        }

        DateTime start, end, b_date;
        DataTable buy;
        string m_num;
        int total = 0;

        private void Form15_Load(object sender, EventArgs e)
        {
            //comboBox1.SelectedIndex = 0;
            buY_VIEWTableAdapter1.Fill(dataSet11.BUY_VIEW);
            buy = dataSet11.Tables["BUY_VIEW"];
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            start = dateTimePicker1.Value;
            end = dateTimePicker2.Value;
            int same_day = DateTime.Compare(start, end);
            if (same_day > 0)
            {
                MessageBox.Show("종료일을 다시 선택해주세요");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("매장을 선택해주세요");
            }
            else
            {
                m_num = comboBox1.SelectedItem.ToString();
                start = dateTimePicker1.Value;
                end = dateTimePicker2.Value;

                if (m_num == "전체 매장")
                {
                    label8.Text = "전체";
                    buY_VIEWTableAdapter1.Fill(dataSet11.BUY_VIEW);
                    buy = dataSet11.Tables["BUY_VIEW"];

                }
                else
                {
                    label8.Text = m_num;
                    buY_VIEWTableAdapter1.FillByMart(dataSet11.BUY_VIEW, m_num);
                    buy = dataSet11.Tables["BUY_VIEW"];
                }

                if (metroRadioButton1.Checked == true)
                {
                    DateTime monday = DateTime.Parse("2022-11-07");
                    DayOfWeek(monday);
                }
                else if (metroRadioButton2.Checked == true)
                {
                    DateTime tuesday = DateTime.Parse("2022-11-01");
                    DayOfWeek(tuesday);
                }
                else if (metroRadioButton3.Checked == true)
                {
                    DateTime wednesday = DateTime.Parse("2022-11-02");
                    DayOfWeek(wednesday);
                }
                else if (metroRadioButton4.Checked == true)
                {
                    DateTime thursday = DateTime.Parse("2022-11-03");
                    DayOfWeek(thursday);
                }
                else if (metroRadioButton5.Checked == true)
                {
                    DateTime friday = DateTime.Parse("2022-11-04");
                    DayOfWeek(friday);
                }
                else if (metroRadioButton6.Checked == true)
                {
                    DateTime saturday = DateTime.Parse("2022-11-05");
                    DayOfWeek(saturday);
                }
                else if (metroRadioButton7.Checked == true)
                {
                    DateTime sunday = DateTime.Parse("2022-11-06");
                    DayOfWeek(sunday);
                }
                else
                {
                    Calculate();
                }

            }
        }

        private void DayOfWeek(DateTime day)
        {
            string str = "";
            DateTime fst_week, snd_week, trd_week, fth_week;
            if(day == DateTime.Parse("2022-11-07"))
            {
                snd_week = day;
                str += "2주차 : " + day_calculate(snd_week) + "  ";
                trd_week = snd_week.AddDays(7);
                str += "3주차 : " + day_calculate(trd_week) + "  ";
                fth_week = trd_week.AddDays(7);
                str += "4주차 : " + day_calculate(fth_week) + "  ";
            }
            else
            {
                fst_week = day;
                str += "1주차 : " + day_calculate(fst_week) + "  ";
                snd_week = day.AddDays(7);
                str += "2주차 : " + day_calculate(snd_week) + "  ";
                trd_week = snd_week.AddDays(7);
                str += "3주차 : " + day_calculate(trd_week) + "  ";
                fth_week = trd_week.AddDays(7);
                str += "4주차 : " + day_calculate(fth_week) + "  ";
            }
            label9.Text = str;
        }

        private String day_calculate(DateTime week_day)
        {
            int day_total = 0;
            foreach (DataRow dr in buy.Rows)
            {
                if (dr["B_DATE"].ToString() != "")
                {
                    b_date = DateTime.Parse(dr["B_DATE"].ToString());
                    int same_day = DateTime.Compare(week_day, b_date);
                    if (same_day == 0) //시작일과 종료일이 같은날
                    {
                        int result = DateTime.Compare(week_day, b_date);
                        if (result == 0 && dr["STATE"].ToString() == "1")
                        {
                            day_total += Int32.Parse(dr["total"].ToString());
                        }

                    }
                }
            }
            return day_total.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //요일 검색 해제
            metroRadioButton1.Checked = false;
            metroRadioButton2.Checked = false;
            metroRadioButton3.Checked = false;
            metroRadioButton4.Checked = false;
            metroRadioButton5.Checked = false;
            metroRadioButton6.Checked = false;
            metroRadioButton7.Checked = false;
        }

        private void Calculate()
        {
            total = 0;
            int same_day = DateTime.Compare(start, end);
            foreach (DataRow dr in buy.Rows)
            {
                if(dr["B_DATE"].ToString()!= "")
                {
                    b_date = DateTime.Parse(dr["B_DATE"].ToString());
                    if (same_day == 0) //시작일과 종료일이 같은날
                    {
                        int result = DateTime.Compare(start, b_date);
                        if (result == 0 && dr["STATE"].ToString() == "1")
                        {
                            total += Int32.Parse(dr["total"].ToString());
                        }

                    }

                    else if (same_day < 0) //다른 날
                    {
                        int after_start = DateTime.Compare(start, b_date);
                        int before_end = DateTime.Compare(b_date, end);

                        if (after_start <= 0 && before_end <= 0 && dr["STATE"].ToString() == "1")
                        {
                            total += Int32.Parse(dr["total"].ToString());
                        }
                    }
                }
                
            }
            label9.Text = total + " 원";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            


        }
    }
}
