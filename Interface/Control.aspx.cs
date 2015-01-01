using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Econophysics;
using CommonType;

namespace Interface
{
    public partial class Control : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Experiment.SetTimeTick();
            TimeTick.Text = Experiment.TimeTick.ToString();
        }
    }
}