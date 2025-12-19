using System.Collections.Generic;
using Articy.Unity;

namespace AppScripts.Narrative.Model
{
    public class NrFlowPoint
    {
        public NrFlowPoint(IFlowObject point, IList<Branch> branches)
        {
            Point = point;
            Branches = branches;
        }

        public IFlowObject Point { get; }
        public IList<Branch> Branches { get; }
    }
}