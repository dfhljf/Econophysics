using Econophysics.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Econophysics
{
    public class Android:Agent
    {
        internal Android(int id, AgentInfo init) : base(id, init) { }
        internal int AI(Market market, Parameters parameters)
        {
            return (Random.GetDouble() > 0.5 ? -1 : 1) * Random.GetInt(parameters.Agent.MaxStock);
        }
    }
}
