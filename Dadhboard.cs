using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Dadhboard : Form
    {
        public Dadhboard()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var add = new MatchFix();
            this.Hide();
            add.ShowDialog();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var add = new playersAdd();
            this.Hide();
            add.ShowDialog();
            this.Show();
        }
    }
}
