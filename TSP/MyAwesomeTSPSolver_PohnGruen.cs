using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSPSolver.TSP {
    public class MyAwesomeTSPSolverPG : ITSPSolver {
        public List<int> Solve(IReadOnlyList<Location> cities) {

            var random = new Random();
            List<Location> sorted = new List<Location>();
               
            var start =  cities[random.Next(cities.Count()-1)];
            sorted.Add(start);

            do {
                sorted.Add(NearestNeigbours(cities.Where(x => !sorted.Contains(x)).ToList(),sorted.Last()));
            } while (sorted.Count != cities.Count);

            sorted.Add(start);

            var item = sorted.Select(x => GetIndexOfItem(cities, x)).ToList();
            var listSolution = item.Select(i => i + 1).ToList();
       
            return listSolution;
        }



        private int GetIndexOfItem(IReadOnlyList<Location> cities, Location loc) {
            int a = cities.ToList().IndexOf(loc);
            Console.WriteLine(a.ToString());
            return a;
        }


        private Location NearestNeigbours(IReadOnlyList<Location> locations, Location l) {
            Location nearest = l;

            int cnt = 0;
            double distance = double.MaxValue;
            Parallel.ForEach(locations, x => {
                var calc_distance = Utils.CityDistance(x, l);
                if (calc_distance <= distance) {
                    nearest = x;
                    distance = calc_distance;
                }
            });
            return nearest;
        }
    }
}
