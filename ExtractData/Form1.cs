using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Econophysics.Type;
using Econophysics.DataIO.Mysql;

namespace ExtractData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Hashtable eht = new Hashtable();
            ExperimentIO ei = new ExperimentIO();
            eht = ei.Read("select * from parameters");
            
        }
    }
}
