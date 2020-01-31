using System;
using System.Collections.Generic;
using System.Linq;

namespace TSPSolver.TSP
{
    public class MyAwesomeTSPSolver : ITSPSolver
    {
        public List<int> Solve(IReadOnlyList<Location> cities)
        {
            //Eigenen Solver schreiben
            //algorithums finden um die beste Route herauszufinden
            //bis 07.02.2019

            List<int> visitedCities = new List<int>();
            visitedCities.Add(0);

            for (int i = 0; i < cities.Count; i++)
            {
                int nearestIndex = NearestNeighbour(cities, visitedCities);
                visitedCities.Add(nearestIndex);    
            }

            for (int i = 0; i < visitedCities.Count; i++)
            {
                Console.WriteLine("göd: " + visitedCities[i]);
            }


            return visitedCities.Select(i => i + 1).ToList();
        }

        private int NearestNeighbour(IReadOnlyList<Location> cities, List<int> visitedCities)
        {
            //visitedCities[visitedCities.Count - 1]
            Location currentCity = cities[visitedCities[visitedCities.Count - 1]];

            List<double> allDistances = new List<double>();
            int lowestIndex = 0;

            for (int i = 0; i < cities.Count; i++)
            {
                if(visitedCities.Contains(i) == false)
                {
                    var distance = CityDistance(currentCity, cities[i]);


                    if (allDistances.Count != 0)
                    {
                        if (distance < allDistances.Min())
                        {
                            lowestIndex = i;
                        }
                    }
                    else
                        lowestIndex = i;

                    allDistances.Add(distance);
                }
            }

            return lowestIndex;
        }

        public static double CityDistance(Location l1, Location l2)
        {
            var dx = l2.X - l1.X;
            var dy = l2.Y - l1.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
