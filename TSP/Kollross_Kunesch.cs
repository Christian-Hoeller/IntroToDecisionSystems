using System;
using System.Collections.Generic;
using System.Linq;

namespace TSPSolver.TSP
{
    public class KUNDKTSPSolver : ITSPSolver
    {
        public List<int> Solve(IReadOnlyList<Location> cities)
 //Liste aus Locations mit X und Y Koordinaten
        {                      
            List<int> visitedIndexes = new List<int>();
            int nextStopIndex = ChooseStartLocation(cities.Count);
            Location currentLocation = cities[nextStopIndex];
            
            for (int i = 0; i < cities.Count; i++)
            {               
                visitedIndexes.Add(nextStopIndex);
                nextStopIndex = SelectNextStop(cities, currentLocation, visitedIndexes);            
                currentLocation = cities[nextStopIndex];               
            }          
            return visitedIndexes.Select(i=>i+1).ToList();
        }

        private int SelectNextStop(IReadOnlyList<Location> cities, 
 Location currentLocation, List<int>visitedIndexes)
        {
            List<double> distances = new List<double>();
            int index = 0;
            for (int i=0; i<cities.Count;i++)
            {
                if(!visitedIndexes.Contains(i))
//Ausschließen, dass mit Cities gerechnet wird, die schon besucht wurden.
                {
double currentDistance = GetDistanceBetweenTwoPoints(currentLocation,cities[i]);
distances.Add(currentDistance);
//Jetzt schauen: Ist currentDistance derzeit die Kürzeste?,
		//wenn ja --> Index von cities speichern (i).
                    if (CheckIfBestOption(currentDistance, distances))
                    {
                        index = i;
                    }                   
                }              
            }
            return index;                     
        }
 


        private bool CheckIfBestOption(double currentDistance, List<double> distances)
        {                    
            if (currentDistance == distances.Min())
            {
                return true;
            }                                                                              
            return false;
        }

        private int ChooseStartLocation(int length)
        {
            Random r = new Random();
            int startValue = r.Next(0, length);
            return startValue;
        }

        public static double GetDistanceBetweenTwoPoints(Location location1,
 	 Location location2)
        {
return Math.Sqrt(Math.Pow((location2.X - location1.X), 2) + Math.Pow((location2.Y - location1.Y), 2));
        }
    }
}
