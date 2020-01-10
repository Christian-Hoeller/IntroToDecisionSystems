using System.Collections.Generic;

namespace TSPSolver.TSP
{
    public interface ITSPSolver
    {
        List<int> Solve(IReadOnlyList<Location> cities);
    }
}
