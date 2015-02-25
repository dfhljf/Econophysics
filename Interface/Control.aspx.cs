using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Econophysics;
using Econophysics.Type;
using System.Collections;

namespace Interface
{
    /// <summary>
    /// 控制台界面
    /// </summary>
    public partial class Control : System.Web.UI.Page
    {
        private static Dictionary<int, Parameters> _eht;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            refresh(Experiment.State);
        }
        protected void BuildExp_Click(object sender, EventArgs e)
        {
            Experiment.Build(Convert.ToInt32(ExpId.SelectedValue), getParameters());
            refresh(Experiment.State);
        }
        protected void StartExp_Click(object sender, EventArgs e)
        {
            Experiment.Start();
            refresh(Experiment.State);
        }
        protected void ContinueExp_Click(object sender, EventArgs e)
        {
            Experiment.Continue();
            updatePauseList();
            refresh(Experiment.State);
        }
        protected void ResetExp_Click(object sender, EventArgs e)
        {
            Experiment.Reset();
            refresh(Experiment.State);
        }
        protected void ExpId_SelectedIndexChanged(object sender, EventArgs e)
        {
            int expId = Convert.ToInt32(ExpId.SelectedValue);
            if (expId != 0)
            {
                loadParameters(_eht[expId]);
                Turn.Text = _eht[expId].Experiment.StartTurn.ToString();
            }
            else if (expId == 0)
            {
                Turn.Text = "0";
            }
        }
        protected void RemovePause_Click(object sender, EventArgs e)
        {
            int[] selectList = CurrentPauseList.GetSelectedIndices();
            foreach (int s in selectList)
            {
                Experiment.RemovePause(Convert.ToInt32(CurrentPauseList.Items[s].Value));
            }
            updatePauseList();
        }
        protected void AddPause_Click(object sender, EventArgs e)
        {
            Experiment.AddPause(Convert.ToInt32(PauseTurn.Text));
            updatePauseList();
        }
        protected void ResetParameters_Click(object sender, EventArgs e)
        {
            resetParameters();
        }



        private void Initialize()
        {
            BuildExp.Attributes.Add("onclick ", "return confirm('确定建立实验？');");
            RecoveryExp.Attributes.Add("onclick ", "return confirm('确定恢复实验？');");
            StartExp.Attributes.Add("onclick ", "return confirm('确定开始实验？');");
            ContinueExp.Attributes.Add("onclick ", "return confirm('确定继续实验？');");
            ResetExp.Attributes.Add("onclick ", "return confirm('确定重置实验？');");
            ResetParameters.Attributes.Add("onclick ", "return confirm('将重置参数为默认参数，请确认！');");
            RemovePause.Attributes.Add("onclick ", "return confirm('将删除你选中的暂停点，请确认！');");
            AddPause.Attributes.Add("onclick ", "return alert('暂停点将设置在你输入的轮次的开始时，即这轮还没开始交易！');");
            ExpState.Text = Experiment.State.ToString();
            refresh(Experiment.State);
            updateExpList();
            updatePauseList();
            resetParameters();
        }
        private void resetParameters()
        {
            InitCash.Text = "10000";
            InitStocks.Text = "100";
            InitDividend.Text = "1.0";
            MaxStock.Text = "10";
            PeriodOfUpdateDividend.Text = "5";
            TradeFee.Text = "1.0";
            InitPrice.Text = "100.0";
            InitMarketState.SelectedValue = "Active";
            Count.Text = "50";
            LeverageEffect.Text = "Null";
            Lambda.Text = "0.01";
            P01.Text = "0.25";
            P10.Text = "0.1";
            PDividend.Text = "0.8";
            P.Text = "0.7";
            TransP.Text = "0.5";
            TimeWindow.Text = "15";
            PeriodOfTurn.Text = "10";
            MaxTurn.Text = "1200";
        }
        private Parameters getParameters()
        {
            Parameters para;
            para.Agent.Init = new AgentInfo();
            para.Agent.Init.Cash = Convert.ToInt32(InitCash.Text);
            para.Agent.Init.Dividend = 0;
            para.Agent.Init.Stocks = Convert.ToInt32(InitStocks.Text);
            para.Agent.Init.TradeStocks = 0;
            para.Agent.Init.Order = 0;
            para.Agent.Dividend = Convert.ToDouble(InitDividend.Text);
            para.Agent.MaxStock = Convert.ToInt32(MaxStock.Text);
            para.Agent.PeriodOfUpdateDividend = Convert.ToInt32(PeriodOfUpdateDividend.Text);
            para.Agent.TradeFee = Convert.ToDouble(TradeFee.Text);

            para.Market.Init = new MarketInfo();
            para.Market.Init.NumberOfPeople = 0;
            para.Market.Init.Price = Convert.ToDouble(InitPrice.Text);
            para.Market.Init.Returns = 0;
            para.Market.Init.State = (MarketState)Enum.Parse(typeof(MarketState), InitMarketState.SelectedValue);
            para.Market.Init.AverageEndowment = 0;
            para.Market.Init.Volume = 0;
            para.Market.Count = Convert.ToInt32(Count.Text);
            para.Market.Lambda = Convert.ToDouble(Lambda.Text);
            para.Market.Leverage = (LeverageEffect)Enum.Parse(typeof(LeverageEffect), LeverageEffect.SelectedValue);
            para.Market.P = Convert.ToDouble(P.Text);
            para.Market.P01 = Convert.ToDouble(P01.Text);
            para.Market.P10 = Convert.ToDouble(P10.Text);
            para.Market.PDividend = Convert.ToDouble(PDividend.Text);
            para.Market.TimeWindow = Convert.ToInt32(TimeWindow.Text);
            para.Market.TransP = Convert.ToDouble(TransP.Text);

            para.Graphic.Init = new GraphicInfo();
            para.Graphic.Init.Count = Convert.ToInt32(Count.Text);
            para.Graphic.Init.Height = 500;
            para.Graphic.Init.Width = 900;
            para.Graphic.Init.Url = Server.MapPath("PriceImage.svg");
            para.Experiment.MaxTurn = Convert.ToInt32(MaxTurn.Text);
            para.Experiment.PeriodOfTurn = Convert.ToInt32(PeriodOfTurn.Text);
            para.Experiment.StartTurn = Convert.ToInt32(Turn.Text);
            para.Experiment.Comments = Comments.Text;
            para.Experiment.StartTime = DateTime.Now;
            para.Agent.Init.Endowment = para.Agent.Init.Cash + para.Agent.Init.Stocks * para.Market.Init.Price;
            return para;
        }
        private void loadParameters(Parameters para)
        {
            InitCash.Text = para.Agent.Init.Cash.ToString();
            InitStocks.Text = para.Agent.Init.Stocks.ToString();
            InitDividend.Text = para.Agent.Init.Dividend.ToString();
            MaxStock.Text = para.Agent.MaxStock.ToString();
            PeriodOfUpdateDividend.Text = para.Agent.PeriodOfUpdateDividend.ToString();
            TradeFee.Text = para.Agent.TradeFee.ToString();
            InitPrice.Text = para.Market.Init.Price.ToString();
            InitMarketState.SelectedValue = para.Market.Init.State.ToString();
            Count.Text = para.Market.Count.ToString();
            LeverageEffect.Text = para.Market.Leverage.ToString();
            Lambda.Text = para.Market.Lambda.ToString();
            P01.Text = para.Market.P01.ToString();
            P10.Text = para.Market.P10.ToString();
            PDividend.Text = para.Market.PDividend.ToString();
            P.Text = para.Market.P.ToString();
            TransP.Text = para.Market.TransP.ToString();
            TimeWindow.Text = para.Market.TimeWindow.ToString();
            PeriodOfTurn.Text = para.Experiment.PeriodOfTurn.ToString();
            MaxTurn.Text = para.Experiment.MaxTurn.ToString();
            Comments.Text = para.Experiment.Comments;
        }
        private void updatePauseList()
        {
            CurrentPauseList.Items.Clear();
            foreach (int turn in Experiment.PauseList.Keys)
            {
                CurrentPauseList.Items.Add(new ListItem("第 " + turn.ToString() + " 轮", turn.ToString()));
            }
        }
        private void updateExpList()
        {
            _eht = Experiment.Histories;
            ExpId.Items.Clear();
            ExpId.Items.Add(new ListItem("新建实验", "0"));
            foreach (int expId in _eht.Keys)
            {
                ExpId.Items.Add(new ListItem("实验：" + expId.ToString(), expId.ToString()));
            }
        }
        private void refresh(ExperimentState state)
        {
            switch (state)
            {
                case ExperimentState.Unbuilded:
                    Turn.Text = "0";
                    NumberOfPeople.Text = "0";
                    TimeTick.Text = "还未开始计时";
                    ExpId.Enabled = true;
                    ExpState.Text = "实验未建立";
                    ExpInfo.Disabled = false;
                    Parameters.Disabled = false;
                    PauseList.Disabled = true;
                    BuildExp.Enabled = true;
                    RecoveryExp.Enabled = true;
                    StartExp.Enabled = false;
                    ContinueExp.Enabled = false;
                    ResetExp.Enabled = false;
                    Timer1.Enabled = false;
                    break;
                case ExperimentState.Builded:
                    Turn.Text = Experiment.Turn.ToString();
                    NumberOfPeople.Text = Experiment.Market.Now.NumberOfPeople.ToString();
                    TimeTick.Text = "还未开始计时";
                    ExpId.Enabled = false;
                    ExpState.Text = "实验已建立，还未开始";
                    ExpInfo.Disabled = true;
                    Parameters.Disabled = true;
                    PauseList.Disabled = false;
                    BuildExp.Enabled = false;
                    RecoveryExp.Enabled = false;
                    StartExp.Enabled = true;
                    ContinueExp.Enabled = false;
                    ResetExp.Enabled = false;
                    Timer1.Enabled = true;
                    break;
                case ExperimentState.Running:
                    Turn.Text = Experiment.Turn.ToString();
                    NumberOfPeople.Text = Experiment.Market.Now.NumberOfPeople.ToString();
                    TimeTick.Text = (Experiment.TimeTick-1).ToString();
                    PriceImage.InnerHtml = getImage();
                    ExpId.Enabled = false;
                    ExpState.Text = "实验正在运行...";
                    ExpInfo.Disabled = true;
                    Parameters.Disabled = true;
                    PauseList.Disabled = false;
                    BuildExp.Enabled = false;
                    RecoveryExp.Enabled = false;
                    StartExp.Enabled = false;
                    ContinueExp.Enabled = false;
                    ResetExp.Enabled = false;
                    Timer1.Enabled = true;
                    break;
                case ExperimentState.Suspend:
                    Turn.Text = Experiment.Turn.ToString();
                    NumberOfPeople.Text = Experiment.Market.Now.NumberOfPeople.ToString();
                    TimeTick.Text = (Experiment.TimeTick - 1).ToString();
                    ExpId.Enabled = false;
                    ExpState.Text = "实验挂起，准备进入下一轮";
                    ExpInfo.Disabled = true;
                    Parameters.Disabled = true;
                    PauseList.Disabled = false;
                    BuildExp.Enabled = false;
                    RecoveryExp.Enabled = false;
                    StartExp.Enabled = false;
                    ContinueExp.Enabled = false;
                    ResetExp.Enabled = false;
                    Timer1.Enabled = true;
                    break;
                case ExperimentState.Pause:
                    Turn.Text = Experiment.Turn.ToString();
                    NumberOfPeople.Text = Experiment.Market.Now.NumberOfPeople.ToString();
                    TimeTick.Text = (Experiment.TimeTick - 1).ToString();
                    ExpId.Enabled = false;
                    ExpState.Text = "实验暂停中...";
                    ExpInfo.Disabled = true;
                    Parameters.Disabled = true;
                    PauseList.Disabled = false;
                    BuildExp.Enabled = false;
                    RecoveryExp.Enabled = false;
                    StartExp.Enabled = false;
                    ContinueExp.Enabled = true;
                    ResetExp.Enabled = true;
                    Timer1.Enabled = true;
                    break;
                case ExperimentState.End:
                    Turn.Text = Experiment.Turn.ToString();
                    NumberOfPeople.Text = Experiment.Market.Now.NumberOfPeople.ToString();
                    TimeTick.Text = "计时结束";
                    ExpId.Enabled = false;
                    ExpState.Text = "实验已结束";
                    ExpInfo.Disabled = true;
                    Parameters.Disabled = true;
                    PauseList.Disabled = false;
                    BuildExp.Enabled = false;
                    RecoveryExp.Enabled = false;
                    StartExp.Enabled = false;
                    ContinueExp.Enabled = false;
                    ResetExp.Enabled = true;
                    Timer1.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        private string getImage()
        {
            return string.Format("<object data=\"PriceImage.svg?turn={0}\" width=\"900\" height=\"500\" type=\"image/svg+xml\"/>",Experiment.Turn);
        }

    }
}