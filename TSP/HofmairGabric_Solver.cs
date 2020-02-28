using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSPSolver.TSP;

namespace TSPSolver.TSP
{
    public class NearestNeighbourHG : ITSPSolver
    {
        private DateTime startTime;
        public TimeSpan time;

        private void StartTimer()
        {
            startTime = DateTime.Now;
        }

        private void StopTimer()
        {
            time = DateTime.Now - startTime;
        }

        public List<int> Solve(IReadOnlyList<Location> cities)
        {
            StartTimer();
            Location currentCity = cities[0];
            Location nextNeighbour = cities[1];
            double nextNeighbourDistance = CityDistance(cities[0], cities[1]);

            List<Location> path = new List<Location>();
            path.Add(currentCity);

            bool setNextDist = true;

            for (int i = 0; i < cities.Count; i++)
            {
                foreach (Location otherCity in cities)
                {
                    if (currentCity != otherCity && !path.Contains(otherCity))
                    {
                        double distance = CityDistance(currentCity, otherCity);
                        if (nextNeighbourDistance > distance || setNextDist)
                        {
                            nextNeighbour = otherCity;
                            nextNeighbourDistance = distance;
                            setNextDist = false;
                        }
                    }
                }

                if (cities.Count - 1 == i)
                {
                    path.Add(cities[0]);
                    break;
                }

                setNextDist = true;
                currentCity = nextNeighbour;
                path.Add(currentCity);
            }
            StopTimer();
            return ConvertLocationListToIntList(path, cities);
        }

        private List<int> ConvertLocationListToIntList(List<Location> locList, IReadOnlyList<Location> intList)
        {
            List<int> convertedList = new List<int>();
            foreach (Location location in locList)
            {
                for (int i = 0; i < intList.Count; i++)
                {
                    if (intList[i] == location) convertedList.Add(i + 1);
                }
            }
            return convertedList;
        }

        private static double CityDistance(Location city1, Location city2)
        {
            return Math.Sqrt(Math.Pow(city1.X - city2.X, 2) + Math.Pow(city1.Y - city2.Y, 2));
        }

      
    }
}

