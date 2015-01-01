using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Econophysics;

namespace Interface
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label2.Text = DateTime.Now.ToString();
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Label1.Text = Experiment.TimeTick.ToString();
        }
    }
}