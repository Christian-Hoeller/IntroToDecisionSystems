using System.Collections.Generic;
using System.Linq;

namespace TSPSolver.TSP
{
    public class DummyTSPSolver : ITSPSolver
    {
        public List<int> Solve(IReadOnlyList<Location> cities)
        {
            return Enumerable.Range(1, cities.Count).ToList();
        }
    }
}
