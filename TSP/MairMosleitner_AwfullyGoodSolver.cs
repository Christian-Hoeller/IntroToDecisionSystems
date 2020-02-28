using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TSPSolver.TSP
{

    public class MySOEKsolver : ITSPSolver
    {

        public List<int> Solve(IReadOnlyList<Location> cities)
        {

            Stopwatch watch = new Stopwatch();
            watch.Start();
            //find 3 nearest cities
            //Locations List direkt zu int liste umbauen danach immer von "cities nehmen"
            List<KeyValuePair<Location, int>> locations = new List<KeyValuePair<Location, int>>();
            for (int i = 0; i < cities.Count; i++)
            {


                locations.Add(new KeyValuePair<Location, int>(cities[i], i));
            }
            locations = new List<KeyValuePair<Location, int>>(GetSortedList(cities[0], new List<KeyValuePair<Location, int>>(locations)));

            List<int> sortedIntList = new List<int>();

            for (int i = 0; i < locations.Count; i++)
            {
                sortedIntList.Add(locations[i].Value);
            }


            watch.Stop();
            Console.WriteLine("Time spent: " + watch.Elapsed);
            Console.WriteLine("SADLIJASDJ");
            return sortedIntList.Select(i=>i+1).ToList();
        }

        private List<KeyValuePair<Location, int>> GetSortedList(Location beginLocation, List<KeyValuePair<Location, int>> locations)
        {
            //double currentldistance;
            int lenght = locations.Count;


            //Liste mit Allen Startpunkten erstellen
            List<KeyValuePair<Location, int>> sortedList = SortListNearestPointsList(beginLocation, new List<KeyValuePair<Location, int>>(locations));

            List<List<KeyValuePair<Location, int>>> listsToBuild = new List<List<KeyValuePair<Location, int>>>();

            List<KeyValuePair<Location, int>> temp = new List<KeyValuePair<Location, int>>();


            for (int i = 0; i < sortedList.Count; i++)
            {
                temp.Add(sortedList[i]);
                // pfusch = temp.To
                List<KeyValuePair<Location, int>> pfusch = new List<KeyValuePair<Location, int>>(temp);
                listsToBuild.Add(pfusch);
                temp.Clear();
            }
            //Liste mit allen Startpunkten erstellt


            //Von jedem Startpunkt die nächst drei gelegenen Städten hinzufügen, danach von diesen Städten wieder die nächsten drei.....
            //Sehr Zeitintensiv
            //Fehler die Strecke ist zu lang, obwohl von der Logik her es einfach nearest neigbhour ist

            int sortedlistlenght = sortedList.Count;
            for (int x = 0; x < sortedlistlenght - 1; x++)
            {
                Console.WriteLine(listsToBuild.Count);
                int length = listsToBuild.Count;
                for (int i = 0; i < length; i++)
                {
                    List<KeyValuePair<Location, int>> temploc = new List<KeyValuePair<Location, int>>(SortListNearestPointsListWithoutTakenPoints(listsToBuild[i][listsToBuild[i].Count-1], new List<KeyValuePair<Location, int>>(sortedList), new List<KeyValuePair<Location, int>>(listsToBuild[i])));
                    List<KeyValuePair<Location, int>> temp1 = new List<KeyValuePair<Location, int>>(listsToBuild[i]);
                    List<KeyValuePair<Location, int>> temp2 = new List<KeyValuePair<Location, int>>(listsToBuild[i]);
                    List<KeyValuePair<Location, int>> temp3 = new List<KeyValuePair<Location, int>>(listsToBuild[i]);

                    temp2.Add(temploc[1]);
                    listsToBuild.Add(temp2);

                    temp3.Add(temploc[2]);
                    listsToBuild.Add(temp3);

                    temp1.Add(temploc[0]);
                    listsToBuild.Add(temp1);

                    


                    listsToBuild[i].Add(temploc[0]);

                    //}
                }
            }
            Console.WriteLine("Builded List Sucessfullyy");
            Console.WriteLine(listsToBuild.Count);
            Console.WriteLine("Calculate shortest distance:");

            double distance = 0;
            int listindex = 0;
            //GET SHORTEST LIST
            for (int i = 0; i < listsToBuild.Count; i++)
            {

                double tempdistance = 0;
                for (int x = 0; x < listsToBuild[i].Count - 1; x++)
                {
                    tempdistance += GetDistancePoints(listsToBuild[i][x].Key, listsToBuild[i][x + 1].Key);
                }
                if (i == 0)
                {
                    distance = tempdistance;
                    listindex = i;
                }
                if (tempdistance < distance)
                {
                    Console.WriteLine("Tempdistance > distance");
                    distance = tempdistance;
                    listindex = i;
                    Console.WriteLine(distance);
                }
                else
                {
                    Console.WriteLine("Tempdistance < distance");
                }
            }


            List<KeyValuePair<Location, int>> shortestList = new List<KeyValuePair<Location, int>>(listsToBuild[listindex]);


            return shortestList;

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



        private List<KeyValuePair<Location, int>> SortListNearestPointsListWithoutTakenPoints(KeyValuePair<Location, int> first, List<KeyValuePair<Location, int>> locations, List<KeyValuePair<Location, int>> takenPoints)
        {

            //entfernen aller schon benützen Punkte
            for (int i = 0; i < takenPoints.Count; i++)
            {
                for (int x = 0; x < locations.Count; x++)
                {
                    if (0 == GetnearestPoint(first.Key, takenPoints[i].Key, locations[x].Key))
                    {
                        locations.RemoveAt(x);
                    }
                }
            }



            //Suchen der nähesten Punkte zum Ausgangspunkt (first)
            List<KeyValuePair<Location, int>> sortedList = new List<KeyValuePair<Location, int>>();
            KeyValuePair<Location, int> nearestPoint = locations[0];

            int turns = locations.Count;
            int indexToDel = 0;
            for (int y = 0; y < turns; y++)
            {
                for (int x = 0; x < locations.Count; x++)
                {
                    if (x == 0)
                    {
                        nearestPoint = locations[x];
                        indexToDel = x;
                    }
                    int temp = GetnearestPoint(first.Key, locations[x].Key, nearestPoint.Key);
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


        private double GetDistancePoints(Location a, Location b)
        {
            double distA = Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
            return distA;
        }
        private int GetnearestPoint(Location first, Location a, Location b)
        {
            double distA = Math.Sqrt(Math.Pow(first.X - a.X, 2) + Math.Pow(first.Y - a.Y, 2));

            double distB = Math.Sqrt(Math.Pow(first.X - b.X, 2) + Math.Pow(first.Y - b.Y, 2));
            if (distA > distB) return 1;
            if (distA == distB) return 0;
            return -1;
        }

    
    }
}
