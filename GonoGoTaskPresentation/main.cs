using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GonoGoTaskPresentation
{
    public partial class main : Form
    {
        public int gotrialnum, nogotrialnum;

        public main()
        {
            InitializeComponent();
        }

        private void Button_start_Click(object sender, EventArgs e)
        {

            gotrialnum = Int32.Parse(textBox_goTrialNum.Text);
            nogotrialnum = Int32.Parse(textBox_nogoTrialNum.Text);


            presentation taskpresent = new presentation();
            taskpresent.Show(this);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
        }
    }
}
