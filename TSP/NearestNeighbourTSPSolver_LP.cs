using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSPSolver.TSP
{
    public class NearestNeighbourTSPSolver : ITSPSolver
    {
        public List<int> Solve(IReadOnlyList<Location> cities)
        {
            //throw new NotImplementedException();
            List<int> route = new List<int>();

            List<int> staedteindexe = new List<int>();
            for (int i = 0; i < cities.Count; i++)
            {
                staedteindexe.Add(i + 1);
            }

            route.Add(staedteindexe[0]);
            staedteindexe.RemoveAt(0);

            while (staedteindexe.Count > 0)
            {
                int naehesteStadtIndexeIndex = 0;
                double naehesterAbstand = double.MaxValue;
                for (int i = 0; i < staedteindexe.Count; i++)
                {
                    double aktuellerAbstand = Math.Sqrt((Math.Pow(cities[route[route.Count - 1] - 1].X - cities[staedteindexe[i] - 1].X, 2)) + (Math.Pow(cities[route[route.Count - 1] - 1].Y - cities[staedteindexe[i] - 1].Y, 2)));
                    if (aktuellerAbstand < naehesterAbstand)
                    {
                        naehesterAbstand = aktuellerAbstand;
                        naehesteStadtIndexeIndex = i;
                    }
                }
                route.Add(staedteindexe[naehesteStadtIndexeIndex]);
                staedteindexe.RemoveAt(naehesteStadtIndexeIndex);
            }

            return route;

            //for (int i = 1; i < cities.Count; i++)
            //{
            //    List<double> distances = new List<double>();
            //    for (int j = 0; j < cities.Count; j++)
            //    {

            //    }
            //}

        }
    }
}
