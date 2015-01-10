﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Econophysics;

namespace Interface
{
    public partial class Welcome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Login_Click(object sender, EventArgs e)
        {
            try
            {
            if (Experiment.AddAgent(Convert.ToInt32(HttpContext.Current.Request.UserHostAddress.Split('.').Last())))
            {
                Response.Redirect("Client.aspx");
            }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}