using Vortex.Chains.Model;

namespace Vortex.Chains
{
    public interface IChainsController
    {
        /// <summary>
        /// Получить цепочку по ее ID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public IChain GetChain(string guid);
    }
}