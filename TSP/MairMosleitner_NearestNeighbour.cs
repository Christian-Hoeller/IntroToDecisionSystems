using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TSPSolver.TSP;

namespace TSPSolver
{
    public class NearestNeighbourMosi : ITSPSolver
    {
        public List<int> Solve(IReadOnlyList<Location> cities)
        {

            Stopwatch watch = new Stopwatch();
            watch.Start();
            //Eine KeyValue Liste mit den Städten und den dazugehörigen Indexen erstellen

            List<KeyValuePair<Location, int>> locations = new List<KeyValuePair<Location, int>>();

            ///Die liste mit den dazugehörigen Daten füllen
            ///
            for (int i = 0; i < cities.Count; i++)
            {
                locations.Add(new KeyValuePair<Location, int>(cities[i], i));
            }

            //Nearest neighbour 
            locations = new List<KeyValuePair<Location, int>>(SortListNearestPointsList(cities[0], new List<KeyValuePair<Location, int>>(locations)));

            List<int> sortedIntList = new List<int>();


            for (int i = 0; i < locations.Count; i++)
            {
                sortedIntList.Add(locations[i].Value);
            }
            watch.Stop();
            Console.WriteLine(watch.Elapsed);


            return sortedIntList.Select(i => i + 1).ToList();
        }


        private List<KeyValuePair<Location, int>> SortListNearestPointsList(Location first, List<KeyValuePair<Location, int>> locations)
        {

            List<KeyValuePair<Location, int>> sortedList = new List<KeyValuePair<Location, int>>();

            sortedList.Add(locations[0]);
            locations.RemoveAt(0);

            KeyValuePair<Location, int> nearestPoint = locations[0];


            int turns = locations.Count;
            int indexToDel = 0;

            //turns-> die anzahl der locations in der gesamten Liste (da eine neue mit der gleichen Anzahl gefüllt werden muss)

            for (int y = 0; y < turns; y++)
            {

                //Die am NÄHSTEN location am vorherigen Punkt wird aus der vorherigen liste entfernt und in eine neue eingefügt 
                //deshalb hier locations.count und bei der schleife außerhalb die Länge(lenght)(die gleich bleibt)
                for (int x = 0; x < locations.Count; x++)
                {
                    if (x == 0)
                    {
                        nearestPoint = locations[x];
                        indexToDel = x;
                    }

                    //nähesten Punkt finden
                    int temp = GetnearestPoint(sortedList[y].Key, locations[x].Key, nearestPoint.Key);
                    if (temp == 1)
                    {
                        nearestPoint = locations[x];
                        indexToDel = x;
                    }
                    if (x == locations.Count - 1)
                    {
                        locations.RemoveAt(indexToDel);
                        sortedList.Add(nearestPoint);
                    }
                }
            }
            return sortedList;
        }

        private int GetnearestPoint(Location first, Location a, Location b)
        {
            double distA = Math.Sqrt(Math.Pow(first.X - a.X, 2) + Math.Pow(first.Y - a.Y, 2));

            double distB = Math.Sqrt(Math.Pow(first.X - b.X, 2) + Math.Pow(first.Y - b.Y, 2));
            if (distA < distB) return 1;
            if (distA == distB) return 0;
            return -1;
        }

    }
}
