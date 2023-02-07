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
    public partial class Form13 : MetroForm
    {
        public Form13()
        {
            InitializeComponent();
        }
        DataTable cus;
        private void Form13_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'dataSet1.CUS_BUY_REF' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.cUS_BUY_REFTableAdapter.FillByIdBcntTotal(this.dataSet11.CUS_BUY_REF);
            cus = dataSet11.Tables["CUS_BUY_REF"];

            int newby = 0, green = 0, white = 0, black = 0;
            foreach (DataRow dr in cus.Rows)
            {
                if (dr["t_id"].ToString() == "0") //신규
                {
                    newby++;
                }
                else
                {
                    
                    if (dr["t_id"].ToString() == dr["R_STATE"].ToString()) //블랙
                    {
                        black++;
                    }
                    else
                    {
                        if (Int32.Parse(dr["total"].ToString()) > 20000 && dr["t_id"].ToString() != dr["R_STATE"].ToString()) //화이트 등급
                        {
                            white++;
                        }
                        else //일반 회원
                        {
                            green++;
                        }
                    }
                }
            }


            //draw chart
            //chart1.Titles.Add("등급 별 인원");

            chart1.Series["Series1"].Points.AddXY("신규", newby); // 차트 한줄 출력해주는 코드
            chart1.Series["Series1"].Points.AddXY("화이트", white);
            chart1.Series["Series1"].Points.AddXY("블랙", black);
            chart1.Series["Series1"].Points.AddXY("그린", green);

            //chart1.Series[0].LegendText = "Male";
            chart1.Series[0].LegendText = "#VALX (#PERCENT{P1})";      // 범례에 백분율을 표기하고자 할 때

            //chart1.Series["Series1"].LegendText = "수학";   // 차트 이름을 "수학"으로 설정

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
