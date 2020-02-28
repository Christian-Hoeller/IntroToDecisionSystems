using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSPSolver.TSP
{
    public class KruskalsAlgorithm : ITSPSolver
    {
        public List<int> Solve(IReadOnlyList<Location> cities)
        {
            List<List<int>> possiblePaths = new List<List<int>>();
            List<int> pathSigns = new List<int>();
            List<int> finalPath = new List<int>();

            possiblePaths = GetNearestThree(cities);
            Console.WriteLine("Got the possible paths: "+possiblePaths.Count);
            pathSigns = GoShortestPath(possiblePaths, cities);
            Console.WriteLine("Got the path, unordered: " + pathSigns.Count);
            finalPath = ConstructFinalPath(pathSigns);
            Console.WriteLine("Got the final path: " + finalPath.Count);

            return finalPath;
        }

        public List<List<int>> GetNearestThree(IReadOnlyList<Location> cities)
        {

            List<List<int>> nearestNeighbours = new List<List<int>>();
            List<int> neighbours;
            double distance;

            for (int i = 0; i < cities.Count; i++)
            {
                neighbours = new List<int>(3);
                for (int x = 0; x < cities.Count; x++)
                {
                    if (i != x)
                    {
                        distance = Utils.CityDistance(cities[i], cities[x]);
                        if (neighbours.Count == neighbours.Capacity)
                        {
                            if (distance < Utils.CityDistance(cities[i], cities[neighbours[0]])) { neighbours[2] = neighbours[1]; neighbours[1] = neighbours[0]; neighbours[0] = x; }
                            else if (distance < Utils.CityDistance(cities[i], cities[neighbours[1]])) { neighbours[2] = neighbours[1]; neighbours[1] = x; }
                            else if (distance < Utils.CityDistance(cities[i], cities[neighbours[2]])) { neighbours[2] = x; }
                        }
                        else
                        {
                            neighbours.Add(x);
                            if (neighbours.Count == 3)
                            {
                                int temp;
                                for (int o = 0; o < neighbours.Count; o++)
                                {
                                    if (neighbours[0] > neighbours[1]) { temp = neighbours[0]; neighbours[0] = neighbours[1]; neighbours[1] = temp; }
                                    if (neighbours[2] < neighbours[1]) { temp = neighbours[1]; neighbours[1] = neighbours[2]; neighbours[2] = temp; }
                                }
                            }
                        }
                    }
                }
                nearestNeighbours.Add(neighbours);
                Console.WriteLine("Neighbours added " + neighbours[0] + ", " + neighbours[1] + "," + neighbours[2]);
            }
            return nearestNeighbours;

        } // gives me a list of the three shortest paths for each city
        //works

        public List<int> GoShortestPath(List<List<int>> possiblePaths, IReadOnlyList<Location> cities) 
        {
            Console.WriteLine("Entered second method");
            bool[,] usedPaths = new bool[possiblePaths.Count, 3];
            int[] usedCities = new int[possiblePaths.Count];
            List<int> pathSigns = new List<int>();
            int pathLimit = (possiblePaths.Count) * 2;
            int pathValue = int.MaxValue;
            int pathOrderLists = 0;
            int pathOrderPaths = 0;
            bool reset = true;
            
            Console.WriteLine("added all the values");
            Console.WriteLine("possiblePaths.Count: " + possiblePaths.Count);

            for (int i = 0; i < possiblePaths.Count; i++)
            {
                usedCities[i] = 0;
            }
            Console.WriteLine("set cities 0");

            for (int i = 0; i < possiblePaths.Count; i++)
            {
                for (int x = 0; x < 3; x++)
                {
                    usedPaths[i, x] = false;
                }
            }
            Console.WriteLine("possible paths amount");

            do
            {
                for (int i = 0; i < possiblePaths.Count; i++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (Utils.CityDistance(cities[i], cities[possiblePaths[i][x]]) < pathValue && !usedPaths[i, x])
                        {
                            Console.WriteLine("Check Distance");
                            if (usedCities[i] != 2 && usedCities[x] != 2)
                            {
                                pathValue = possiblePaths[i][x];
                                pathOrderLists = i;
                                pathOrderPaths = x;
                                Console.WriteLine("Selected Path");
                            }
                            else
                            {
                                usedPaths[i, x] = true;
                                Console.WriteLine("Denied Path");
                            }
                        }
                    }
                }
                usedPaths[pathOrderLists, pathOrderPaths] = true;
                pathSigns.Add(pathOrderLists);
                pathSigns.Add(pathOrderPaths);
                if (usedCities[pathOrderLists] == 0) { usedCities[pathOrderLists] = 1; } else { usedCities[pathOrderLists] = 2; }
                if (usedCities[pathOrderPaths] == 0) { usedCities[pathOrderPaths] = 1; } else { usedCities[pathOrderPaths] = 2; }
                Console.WriteLine("Added path to list");
                if (pathSigns.Count == pathLimit) { reset = false; }
            } while (reset == true);

            return pathSigns;
        } // orders the paths in the order that they are supposed to be drawn in until every city is connected to two paths
        // works

        public List<int> ConstructFinalPath(List<int> pathSigns)
        {
            List<int> constructedPath = new List<int>((pathSigns.Count / 2) + 1);
            int currentValue = pathSigns[0];
            int numberOfPathsLeft = pathSigns.Count;

            constructedPath.Add(pathSigns[0]);
            do
            {
                for (int i = 0; i <= pathSigns.Count / 2; i++)
                {
                    if (pathSigns[i] == currentValue)
                    {
                        Console.WriteLine("current value: " + currentValue);
                        if ((i + 1) % 2 == 0)
                        {
                            constructedPath.Add(pathSigns[i - 1]);
                            currentValue = pathSigns[i - 1];
                            Console.WriteLine("new current value: " + currentValue);
                            pathSigns[i] = 0;
                            pathSigns[i - 1] = 0;
                            numberOfPathsLeft = numberOfPathsLeft - 2;
                            Console.WriteLine("add left: "+numberOfPathsLeft);
                        }
                        else
                        {
                            constructedPath.Add(pathSigns[i + 1]);
                            currentValue = pathSigns[i + 1];
                            Console.WriteLine("new current value: " + currentValue);
                            pathSigns[i] = 0;
                            pathSigns[i + 1] = 0;
                            numberOfPathsLeft = numberOfPathsLeft - 2;
                            Console.WriteLine("add right: "+numberOfPathsLeft);
                        }
                    }
                    if (numberOfPathsLeft == 0) { break; }
                }
            } while (numberOfPathsLeft != 0);

            return constructedPath;
        } // orders the various paths into a single path that can be processed by the draw method
        // does not work. had a few endless loops that kept occuring while I tried to fix this method, but I ran out of time eventually
    }
}
