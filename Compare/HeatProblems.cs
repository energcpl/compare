﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compare
{
    public partial class HeatProblems : Form
    {
        public HeatProblems()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DataTable dt = DBStuff.GetHeatDataLast7Days();
            heatGridView.DataSource = dt;
        }
    }
}
