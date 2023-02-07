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
    public partial class Form9 : MetroForm
    {
        public Form9()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.IsBalloon = true;
            this.toolTip1.SetToolTip(this.pictureBox1, "로그아웃");
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            
        }

       
        private void 상품관리ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form14 form14 = new Form14();
            form14.MdiParent = this;
            form14.Show();
        }


        private void 상품판매액현황ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            Form15 form15 = new Form15();
            form15.MdiParent = this;
            form15.Show();
        }

        private void 구매내역관리ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form12 form12 = new Form12();
            form12.MdiParent = this;
            form12.Show();
        }

        private void 고객통계ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form13 form13 = new Form13();
            form13.MdiParent = this;
            form13.Show();
        }

        private void 상품가격변경ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form16 form16 = new Form16();
            form16.MdiParent = this;
            form16.Show();
        }

        private void 신규상품입고ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form10 form10 = new Form10();
            form10.MdiParent = this;
            form10.Show();
        }

        private void 재고현황ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form11 form11 = new Form11();
            form11.MdiParent = this;
            form11.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            metroContextMenu3.Show(button1, 0, button1.Height);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            metroContextMenu2.Show(button2, 0, button2.Height);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            metroContextMenu1.Show(button3, 0, button3.Height);
        }
    }
}
