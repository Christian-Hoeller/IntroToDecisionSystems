using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TSPSolver.TSP
{
    public static class Utils
    {
        public static double GetDistance(IReadOnlyList<int> solution, IReadOnlyList<Location> cities)
        {
            double distance = 0;
            for (int i = 0; i < solution.Count; i++)
            {
                distance += GetDistanceBetweenTwoPoints(cities[solution[i] -1], cities[solution[i]]);
            }
            return distance;
        }


        private static double GetDistanceBetweenTwoPoints(Location point1, Location point2)
        {
            
            var lengthX = Math.Abs(point1.X - point2.X);
            var lengthY = Math.Abs(point1.Y - point2.Y);

            double distance = (Math.Pow(lengthX, 2) + Math.Pow(lengthY, 2));

            return distance;

        }
    }
}
