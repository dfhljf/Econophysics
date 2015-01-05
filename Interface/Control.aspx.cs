using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Econophysics;
using CommonType;
using System.Collections;

namespace Interface
{
    public partial class Control : System.Web.UI.Page
    {
        private Hashtable _eht;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            Experiment.StateChanged += Experiment_StateChanged;
            Experiment.GraphicReady += Experiment_GraphicReady;
            Experiment.NextTurnReady += Experiment_NextTurnReady;
            ExpId.Items.Add(new ListItem("新建实验","0"));
            _eht = Experiment.List();
            foreach (MarketKey mk in _eht.Keys)
            {
                ExpId.Items.Add(new ListItem("实验：" + mk.ExperimentId.ToString()+" 上次结束轮次："+mk.Turn.ToString(), mk.ExperimentId.ToString()));
            }
        }

        void Experiment_NextTurnReady(MarketInfo marketInfo)
        {
            throw new NotImplementedException();
        }

        void Experiment_GraphicReady(GraphicInfo graphicInfo)
        {
            throw new NotImplementedException();
        }

        private void Experiment_StateChanged(ExperimentState state)
        {
            ExpState.Text = state.ToString();
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Experiment.SetTimeTick();
            TimeTick.Text = Experiment.TimeTick.ToString();
        }

        protected void BuildExp_Click(object sender, EventArgs e)
        {
            Experiment.Build(getParameters());
        }
        private Parameters getParameters()
        {
            Parameters para;
            para.AgentPart.Init.Cash = Convert.ToInt32(InitCash.Text);
            para.AgentPart.Init.Dividend = Convert.ToDouble(InitDividend.Text);
            para.AgentPart.Init.Stocks = Convert.ToInt32(InitStocks.Text);
            para.AgentPart.Init.TradeStocks = 0;
            para.AgentPart.MaxStock = Convert.ToInt32(MaxStock.Text);
            para.AgentPart.PeriodOfUpdateDividend = Convert.ToInt32(PeriodOfUpdateDividend.Text);
            para.AgentPart.TradeFee = Convert.ToDouble(TradeFee.Text);
            para.MarketPart.Init.NumberOfPeople = 0;
            para.MarketPart.Init.Price = Convert.ToDouble(InitPrice.Text);
            para.MarketPart.Init.Returns = 0;
            para.MarketPart.Init.State = (MarketState)Enum.Parse(typeof(MarketState), InitMarketState.SelectedValue);
            para.MarketPart.Count = Convert.ToInt32(Count.Text);
            para.MarketPart.Lambda = Convert.ToDouble(Lambda.Text);
            para.MarketPart.Leverage = (LeverageEffect)Enum.Parse(typeof(LeverageEffect), LeverageEffect.SelectedValue);
            para.MarketPart.P = Convert.ToDouble(P.Text);
            para.MarketPart.P01 = Convert.ToDouble(P01.Text);
            para.MarketPart.P10 = Convert.ToDouble(P10.Text);
            para.MarketPart.PDividend = Convert.ToDouble(PDividend.Text);
            para.MarketPart.TimeWindow = Convert.ToInt32(TimeWindow.Text);
            para.MarketPart.TransP = Convert.ToDouble(TransP.Text);
            para.GraphicPart.Init.Count = Convert.ToInt32(Count.Text);
            para.GraphicPart.Init.Height = 500;
            para.GraphicPart.Init.Width = 900;
            para.GraphicPart.Init.Url = Server.MapPath("PriceImage.svg");
            para.ExperimentPart.MaxTurn = Convert.ToInt32(MaxTurn.Text);
            para.ExperimentPart.PeriodOfTurn = Convert.ToInt32(PeriodOfTurn.Text);
            para.AgentPart.Init.Endowment = para.AgentPart.Init.Cash + para.AgentPart.Init.Stocks * para.MarketPart.Init.Price;
            return para;
        }
        private void loadParameters(Parameters para)
        {

        }

        protected void ExpId_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}