using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;

namespace TSPSolver.TSP
{
    public class SparseTSP : ITSPSolver
    {
        List<List<double>> matrix = new List<List<double>>();

        public static IJSRuntime JSRuntime;

        public List<int> Solve(IReadOnlyList<Location> cities)
        {
		//Dreiecksmatrix erstellen
            for (int i = 0; i < cities.Count; i++)
            {
                List<double> distances = cities.Take(i).Select(city => Utils.CityDistance(cities[i], city)).ToList();
                matrix.Add(distances);

            }
		//Dreiecksmatrix ausgeben
            JSRuntime.InvokeVoidAsync("console.log", matrix);

            // In connections wird der gewählte Punkt gespeichert
            // zu welchem der Punkt eine Verbindung hat.
            // die Position in der Liste (+1) entspricht der City
            List <int> connections = new List<int>();

            // In tempList wird die Liste mit den Distanzen der akt. City gespeichert                                                                        
            List<double> tempList = new List<double>();
            // Die Liste die dann ausgegeben wird
            List<int> output = new List<int>();

            //Hier werden alle Cities durchgegangen und eine City gewählt
            //anschließend wird es in connections gespeichert
            for (int i = 0; i < matrix.Count(); i++)
            {
                                
                tempList = GetListFromMatrix(matrix, i);
 
                double minValue = double.MaxValue;
                int minValuePos = 0;

                for (int x = 1; x < tempList.Count(); x++)
                {
                    if (!(connections.Contains(x)))
                    {
                        if (tempList[x] < minValue)
                        {
                            minValue = tempList[x];
                            minValuePos = x;
                        }
                    }
                }

                connections.Add(minValuePos);

                JSRuntime.InvokeVoidAsync("console.log", connections);
            }
            JSRuntime.InvokeVoidAsync("console.log", connections);

            //Hier werden die Cities der Reihe nach sortiert und in output gespeichert
            int nextIndex = 0;

            for (int i = 0; i < connections.Count; i++)
            {
                output.Add(connections[nextIndex]);
                nextIndex = connections[nextIndex];

                JSRuntime.InvokeVoidAsync("console.log", nextIndex);
            }
            //Abschließend muss die City-Nummer um 1 erhöht werden
            output.Select(number => number++);
            return output;
        }

//Geht die Dreiecksmatrix durch und gibt die Liste einer bestimmten //Position aus
        private List<double> GetListFromMatrix(List<List<double>> matrix, int pos)
        {
            List<double> tempList = new List<double>();

            tempList.AddRange(matrix[pos]);

            tempList.Add(double.MaxValue);

            for (int j = pos + 1; j < matrix.Count(); j++)
            {
                tempList.Add(matrix[j][pos]);
            }
            return tempList;
        }
    }
}
