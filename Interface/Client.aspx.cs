using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Econophysics;
using Econophysics.Type;

namespace Interface
{
    public partial class Client : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TradeFee.Text = Experiment.Parameters.Agent.TradeFee.ToString();
                MaxStocks.Text = Experiment.Parameters.Agent.MaxStock.ToString();

            }
        }

        protected void Sell_Click(object sender, EventArgs e)
        {

        }

        protected void Buy_Click(object sender, EventArgs e)
        {

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Cash.Text = Experiment.Market.Agents[1].Now.Cash.ToString();
            Stocks.Text = Experiment.Market.Agents[1].Now.Stocks.ToString();
            Endowment.Text = Experiment.Market.Agents[1].Now.Endowment.ToString();
        }
    }
}