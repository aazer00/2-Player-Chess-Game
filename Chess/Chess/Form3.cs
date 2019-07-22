using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form3 : Form
    {
        public static string text = "";
        public Form3()
        {
            InitializeComponent();
            
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.Text = text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            Form2 obj = (Form2)Application.OpenForms["Form2"];
            obj.Close();
           form1.Show();
           
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            Form2 obj = (Form2)Application.OpenForms["Form2"];
            obj.Close();
            Form2.turn = 1;
            form2.Show();
            this.Close();
        }
    }
}
