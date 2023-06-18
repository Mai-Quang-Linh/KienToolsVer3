using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        private string Filename;
        private string Fileext;
        private string Filedir;
        private string res="";
        public Form2(string name,string ext, string dir)
        {
            Filename = name;
            Fileext = ext;
            Filedir = dir+"\\";
            InitializeComponent();
            label3.Text = ext;
            textBox1.Text = name;
            label4.Text = dir + "\\";
        }
        public string getRes()
        {
            return (res);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int length = textBox1.Text.Length + Fileext.Length+ Filedir.Length;
            label2.Text = "Tên file (" + (length).ToString() + " kí tự có path, "+(textBox1.Text.Length).ToString() +" kí tự ko path):";
            if (length < 260 && textBox1.Text.Length<=200)
            {
                if (File.Exists(Filedir + textBox1.Text + Fileext)){
                    label1.Text = "Tên này có file khác dùng rồi!!!";
                    label1.ForeColor = Color.Red;
                    label2.ForeColor = Color.Red;
                    button1.Enabled = false;
                }
                else {
                    button1.Enabled = true;
                    label1.Text = "Tên này thì còn ok.";
                    label1.ForeColor = Color.Green;
                    label2.ForeColor = Color.Green;
                    button1.Enabled = true;
                }
            }
            else
            {
                label1.Text = "Tên dài quá ban ơi sửa lại đê!!!";
                label1.ForeColor = Color.Red;
                label2.ForeColor = Color.Red;
                button1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = Filename;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            res = textBox1.Text;
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
