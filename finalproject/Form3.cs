using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finalproject
{
    public partial class Form3 : MetroForm
    {
        string c_id, c_name, m_num;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        public void SetData(string id, string name)
        {
            c_id = id;
            c_name = name;
            label3.Text = name + " 님";
        }

        private void button1_Click(object sender, EventArgs e) //입장
        {
            Form2 form2 = new Form2();

            if (metroRadioButton1.Checked)
            {
                m_num = "서울";
                form2.SetData(c_id, c_name, m_num, dateTimePicker1.Value);
            }
            else if (metroRadioButton2.Checked)
            {
                m_num = "인천";
                form2.SetData(c_id, c_name, m_num,dateTimePicker1.Value);
            }
            else if (metroRadioButton3.Checked)
            {
                m_num = "대구";
                form2.SetData(c_id, c_name, m_num,dateTimePicker1.Value);
            }
            else if (metroRadioButton4.Checked)
            {
                m_num = "경주";
                form2.SetData(c_id, c_name, m_num,dateTimePicker1.Value);
            }
            else if (metroRadioButton5.Checked)
            {
                m_num = "부산";
                form2.SetData(c_id, c_name, m_num,dateTimePicker1.Value);
            }

            this.Hide();
            form2.ShowDialog();
            this.Close();
        }
    }
}
