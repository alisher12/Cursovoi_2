﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Engine;

namespace SuperAdventure
{

    public partial class TradingScreen : Form
    {
        public TradingScreen()
        {
            InitializeComponent();
        }
        public Player CurrentPlayer { get; set; }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
