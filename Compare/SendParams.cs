using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compare
{
    public partial class SendParams : Form
    {
        public SendParams(int x)
        {
            InitializeComponent();
            label1.Text = Convert.ToString(x);
        }
    }
}
