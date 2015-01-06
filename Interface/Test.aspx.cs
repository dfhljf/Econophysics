using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Econophysics;
using CommonType;

namespace Interface
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Experiment.StateChanged += Experiment_StateChanged;
        }   

        void Experiment_StateChanged(CommonType.ExperimentState state)
        {
            ExpState.Text = state.ToString();
        }
        protected void BuildExp_Click(object sender, EventArgs e)
        {
            //Experiment.Build(getParameters());
            //Experiment.stateChanged(ExperimentState.Builded);
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Experiment.SetTimeTick();
            TimeTick.Text = Experiment.TimeTick.ToString();
        }
    }
}