using System;
using System.Collections.Generic;
using System.Linq;
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
                Id.Text = HttpContext.Current.Request.UserHostAddress.Split('.').Last();
                if (Experiment.Now.State == ExperimentState.Unbuilded || !Experiment.Market.Agents.ContainsKey(Convert.ToInt32(Id.Text)))
                {
                    Response.Redirect("Welcome.aspx");
                }


                Experiment.Market.Agents[Convert.ToInt32(Id.Text)].Login();
                TradeFee.Text = Experiment.Parameters.Agent.TradeFee.ToString();
                MaxStocks.Text = Experiment.Parameters.Agent.MaxStock.ToString();
                //TradeStocks.Text = "";
                ExpInfo.Text = getExpInfo();
                refresh();
            }
        }

        protected void Sell_Click(object sender, EventArgs e)
        {
            try
            {
                int tmp = Convert.ToInt32(TradeStocks.SelectedValue);//(TradeStocks.Text);
                if (tmp < 0)
                {
                    throw ErrorList.TradeStocksOut;
                }
                Experiment.Trade(Experiment.Market.Agents[Convert.ToInt32(Id.Text)].Index, -tmp);
            }
            catch (Exception err)
            {
                ExpInfo.Text = err.Message;
            }
            refresh();
        }



        protected void Buy_Click(object sender, EventArgs e)
        {
            try
            {
                int tmp = Convert.ToInt32(TradeStocks.SelectedValue);//(TradeStocks.Text);
                if (tmp < 0)
                {
                    throw ErrorList.TradeStocksOut;
                }
                Experiment.Trade(Experiment.Market.Agents[Convert.ToInt32(Id.Text)].Index, tmp);
            }
            catch (Exception err)
            {
                ExpInfo.Text = err.Message;
            }
            refresh();
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if (Experiment.Now.Turn != Convert.ToInt32(Turn.Text))//轮次改变更新
            {
                PriceImage.InnerHtml = getImage();
                ExpInfo.Text = getExpInfo();
                if (Experiment.Now.State == ExperimentState.End)
                {
                    Response.Redirect("Reward.aspx");
                }
            }
            refresh();
        }
        private void refresh()
        {

            Turn.Text = Experiment.Now.Turn.ToString();
            TimeTick.Text = (Experiment.TimeTick == -1) ? "计时停止" : (Experiment.TimeTick - 1).ToString();
            Cash.Text = Experiment.Market.Agents[Convert.ToInt32(Id.Text)].Now.Cash.ToString();
            Stocks.Text = Experiment.Market.Agents[Convert.ToInt32(Id.Text)].Now.Stocks.ToString();
            Endowment.Text = Experiment.Market.Agents[Convert.ToInt32(Id.Text)].Now.Endowment.ToString();
            Dividend.Text = Experiment.Market.Agents[Convert.ToInt32(Id.Text)].Now.Dividend.ToString();
            DividendTime.Text = (Experiment.Parameters.Agent.PeriodOfUpdateDividend - (Experiment.Now.Turn - 1) % Experiment.Parameters.Agent.PeriodOfUpdateDividend).ToString();
            DividendIncome.Text = (Convert.ToInt32(Stocks.Text) * Convert.ToDouble(Dividend.Text)).ToString();
            Buy.Enabled = !Experiment.Market.Agents[Convert.ToInt32(Id.Text)].IsTrade;
            Sell.Enabled = Buy.Enabled;
        }
        private string getImage()
        {
            return string.Format("<object data=\"PriceImage.svg?turn={0}\" width=\"900\" height=\"500\" type=\"image/svg+xml\"/>", Experiment.Now.Turn);
        }
        private string getExpInfo()
        {
            switch (Experiment.Now.State)
            {
                case ExperimentState.Unbuilded:
                    return "";
                case ExperimentState.Builded:
                    return "实验还未开始，等待实验开始！";
                case ExperimentState.Running:
                    return (Experiment.Market.Agents[Convert.ToInt32(Id.Text)].Now.Order == 0) ? "实验运行中。。。" : ("您上一轮的排名是：第 " + Experiment.Market.Agents[Convert.ToInt32(Id.Text)].Now.Order.ToString() + " 名");
                case ExperimentState.Suspend:
                    return "等待系统处理上一轮结果。。";
                case ExperimentState.Pause:
                    return "实验暂停中，等待实验继续！";
                case ExperimentState.End:
                    return "实验结束，您将会看到您能得到的报酬";
                default:
                    return "";
            }
        }
    }
}