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

    public class MyAwesomeTSPSolver : ITSPSolver
    {
        public List<int> Solve(IReadOnlyList<Location> cities)
        {
            //Eigenen Solver schreiben
            //algorithums finden um die beste Route herauszufinden
            //bis 07.02.2019
            throw new System.NotImplementedException("Homework");
        }
    }
}
