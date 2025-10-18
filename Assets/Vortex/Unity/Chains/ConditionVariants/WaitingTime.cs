using System.Threading.Tasks;
using UnityEngine;

namespace Vortex.Chains.Model
{
    public class WaitingTime : ChainCondition
    {
        [SerializeField, Range(0, 10000)] private int milliseconds = 1000;

        public override void Check() => Timer();

        public override void Cancel()
        {
            //Ignore
        }

        private async Task Timer()
        {
            await Task.Delay(milliseconds);
            Complete();
        }
    }
}