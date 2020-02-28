using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TSPSolver.TSP
{
    public class GeneticTSPSolver : ITSPSolver
    {
        public List<int> Solve(IReadOnlyList<Location> cities)
        {
            List<int> order = Enumerable.Range(1, cities.Count).ToList();
            Population[] populations = new Population[5];
            double recordDistance = 0;
            Population bestPopulation = new Population(order);

            int cntFails = 0;
            int cntMaxFails = cities.Count * 200;
            Console.WriteLine("Maximale Fails: " + cntMaxFails);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Random rnd = new Random();

            for (int i = 0; i < populations.Length; i++)
            {
                populations[i] = new Population(order.OrderBy(x => rnd.Next()).ToList());
            }

            int cntGeneration = 0;
            while (cntFails < cntMaxFails && sw.Elapsed.Minutes < 3)
            {
                cntGeneration++;
                for (int i = 0; i < populations.Length; i++)
                {
                    double d = Utils.GetDistance(populations[i].order, cities);
                    populations[i].Fitness = 1 / (d + 1);

                    if (i == 0)
                    {
                        recordDistance = d;
                        bestPopulation = populations[0];
                    }

                    if (d < recordDistance)
                    {
                        recordDistance = d;
                        bestPopulation = populations[i].Clone();
                        cntFails = 0;
                    }
                    else
                    {
                        cntFails++;
                    }
                }

                NormalizeFitness(populations);

                populations = NextGeneration(populations);
                populations[0] = bestPopulation;
            }

            sw.Stop();

            Console.WriteLine($"Dauer: {sw.Elapsed.Minutes} Minuten und {sw.Elapsed.Seconds} Sekunden");
            Console.WriteLine($"Anzahl Fails: {cntFails}");

            Console.WriteLine("Anzahl Generationen: " + cntGeneration);

            GC.Collect();

            return bestPopulation.order;
        }

        private void NormalizeFitness(Population[] populations)
        {
            double sum = 0;

            foreach (Population p in populations) sum += p.Fitness;
            foreach (Population p in populations) p.Fitness = p.Fitness / sum;
        }

        private Population[] NextGeneration(Population[] populations)
        {
            Population[] newGeneration = new Population[populations.Length];
            Population order;

            for (int i = 0; i < populations.Length; i++)
            {
                order = PickOne(populations).Clone();
                order = Mutate(order, 1);
                newGeneration[i] = order.Clone();
            }

            return newGeneration;
        }

        private Population PickOne(Population[] populations)
        {
            int index = 0;
            Random genR = new Random();
            int x = genR.Next(99) + 1;
            double r = (double)x / 100;

            while(r > 0)
            {
                r -= populations[index].Fitness;
                index++;
            }
            index--;

            return populations[index].Clone();
        }

        private Population Mutate(Population population, int mutationRate)
        {
            Population p = population.Clone();
            Random r = new Random();

            for(int i = 0; i < mutationRate; i++)
            {
                int indexA = r.Next(population.order.Count);
                int indexB = r.Next(population.order.Count);

                int x = p.order[indexA];
                p.order[indexA] = p.order[indexB];
                p.order[indexB] = x;
            }
            return p.Clone();
        }
    }

    public class Population
    {
        public List<int> order;

        public double Fitness
        {
            get;
            set;
        }

        public Population(List<int> order)
        {
            this.order = order;
        }

        public Population Clone()
        {
            Population p = new Population(order.ToList());
            return p;
        }
    }
}
