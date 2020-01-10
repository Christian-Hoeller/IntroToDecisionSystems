using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TSPSolver.TSP
{
    public static class Utils
    {
        public static double CityDistance(Location l1, Location l2)
        {
            var dx = l2.X - l1.X;
            var dy = l2.Y - l1.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double GetDistance(IReadOnlyList<int> solution, IReadOnlyList<Location> cities)
        {
            return solution
                .Concat(solution.Take(1))
                .Select(index => cities[index - 1])
                .Pairwise(CityDistance)
                .Sum();
         
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<double> Pairwise(this IEnumerable<Location> source, Func<Location, Location, double> fn)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (fn is null)
            {
                throw new ArgumentNullException(nameof(fn));
            }

            return source.Zip(source.Skip(1), fn);
        }


    }
}
