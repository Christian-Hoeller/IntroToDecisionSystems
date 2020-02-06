using System;
using System.Collections.Generic;
using System.Linq;

namespace TSPSolver.TSP
{
    public class MyAwesomeTSPSolver : ITSPSolver
    {
        public List<int> Solve(IReadOnlyList<Location> cities)
        {
            List<int> visitedCities = new List<int>();
            visitedCities.Add(0);   //add the first index of the cities as the startindex

            return RecursiveSolve(cities, visitedCities).Select(i => i + 1).ToList();   //returns the list of indexes and adds +1 to every index
        }

        public List<int> RecursiveSolve(IReadOnlyList<Location> cities, List<int> visitedCities)
        {
            if (visitedCities.Count == cities.Count)    //returns the visited Cities if all the cities are visited
            {
                return visitedCities;
            }
            else
            {
                Location currentCity = cities[visitedCities.Last()];    //gets the currentCity with the index of the last item in visitedCities
                List<double> allDistances = new List<double>(); //new list where we store all the distances
                List<int> CitiesNotVisited = GetCitiesNotVisited(cities, visitedCities);    //Gets all the cities that haven't been visited yet

                foreach (var city in CitiesNotVisited)
                {
                    allDistances.Add(CityDistance(currentCity, cities[city]));  //adds the distances between the currentCity and every other (notVisited)city to the List
                }

                int lowestIndex = CitiesNotVisited[allDistances.IndexOf(allDistances.Min())];   //gets the index of the lowest distance --> this is the index in citiesNotVisited
                visitedCities.Add(lowestIndex);     //adds the lowest index to the visitedCities
                return RecursiveSolve(cities, visitedCities);

            }
        }

        private List<int> GetCitiesNotVisited(IReadOnlyList<Location> cities, List<int> visitedCities)
        {
            List<int> allCityIndexes = Enumerable.Range(0, cities.Count).ToList();     //gets every index in the list
            return allCityIndexes.Except(visitedCities).ToList();   //excepts the visitedCites from all the city indexes
        }

        public static double CityDistance(Location l1, Location l2)
        {
            var dx = l2.X - l1.X;
            var dy = l2.Y - l1.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
