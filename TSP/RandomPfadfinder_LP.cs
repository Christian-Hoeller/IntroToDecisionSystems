using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSPSolver.TSP
{
    public class RandomTSPSolver : ITSPSolver
    {
        Random r = new Random();

        public RandomTSPSolver(Random _r)
        {
            r = _r;
        }

        public List<int> Solve(IReadOnlyList<Location> cities)
        {
            double besteDistanz = double.MaxValue;
            List<int> besteRoute = new List<int>();

            for (int j = 0; j < 1; j++)
            {                
                List<int> staedteindexe = new List<int>();

                for (int i = 0; i < cities.Count; i++)
                {
                    staedteindexe.Add(i + 1);
                }

                List<int> routenergebnis = new List<int>();

                for (int i = 0; i < cities.Count; i++)
                {
                    int randomtemp = r.Next(0, staedteindexe.Count);
                    routenergebnis.Add(staedteindexe[randomtemp]);
                    staedteindexe.RemoveAt(randomtemp);
                }
                double aktuelleDistanz = Utils.GetDistance(routenergebnis, cities);
                if (aktuelleDistanz < besteDistanz)
                {
                    besteRoute = routenergebnis;
                    besteDistanz = aktuelleDistanz;
                }
            }
            return besteRoute;
        }
    }
}
