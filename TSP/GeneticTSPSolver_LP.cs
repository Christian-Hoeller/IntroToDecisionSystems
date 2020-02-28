using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSPSolver.TSP;

namespace TSPsolver.TSP
{
    public class GeneticTSPSolver : ITSPSolver
    {
        Random r;

        public GeneticTSPSolver(Random _r)
        {
            r = _r;
        }

        public List<int> Solve(IReadOnlyList<Location> cities)
        {
            DateTime timeLimit = DateTime.Now.AddSeconds(60);  //zeit in sekunden welche zur ferfügung steht
            Random rand = new Random(0);

            Console.WriteLine("Init wurde durchgeführt " + timeLimit.ToLongDateString() + " " + timeLimit.ToLongTimeString());

            //Pfade
            List<List<int>> paths = new List<List<int>>();

            //Manuelle Eröffnungsalgorithmen
            paths.Add(new NearestNeighbourTSPSolver().Solve(cities));
            //Paths.Add(MichisCoolerTSPSolver);

            //Restliche algorithmen werden zufallsgeneriert erstellt
            while (paths.Count < 100)
            {
                paths.Add(new RandomTSPSolver(r).Solve(cities));
            }

            Console.WriteLine("Die AnfangsPfade wurden generiert.");


            do
            {
                Console.WriteLine("Schleife wurde Betreten");
                //Pfader werden Bewertet
                paths.Sort(new Comparer(cities));
                Console.WriteLine("Pfade wurden Sortiert/Bewertet");
                //Nur die besten Pfade werden behalten
                int anzahlZuBehaltenderPfade = 10;
                paths.RemoveRange(anzahlZuBehaltenderPfade, paths.Count - anzahlZuBehaltenderPfade /*- 1*/);
                Console.WriteLine("Schlechte Pfade Wurden Gelöscht");
                //ein Neuer Random Pfad wird hinzugefügt
                paths.Add(new RandomTSPSolver(r).Solve(cities));
                Console.WriteLine("Ein Zufälliger Weg wurde Hinzugefügt");
                //fortpflanzung entweder duch alle kategorien random oder immer 2 nebeneinander existierende Algorithmen.
                int pos = paths.Count - 2;
                while (pos >= 0)
                {
                    paths.Add(Fortpflanzung_Perfektion(paths[pos], paths[pos + 1]));
                    paths.Add(Fortpflanzung_v2(paths[pos], paths[r.Next(paths.Count)]));
                    paths.Add(Mutation(paths[pos]));
                    pos--;
                }
                Console.WriteLine("Fortpflanzung Druchgeführt");
            } while (timeLimit > DateTime.Now);
            Console.WriteLine("Die Schleife wurde Verlassen");
            paths.Sort(new Comparer(cities));
            return paths[0];
        }

        public List<int> Fortpflanzung_v1(List<int> mutter, List<int> vater)
        {
            List<int> kind = new List<int>();
            if (mutter.Count != vater.Count)
            {
                throw new ApplicationException("Programmierfehler");
            }
            do
            {
                kind.Clear();
                for (int i = 0; i < mutter.Count; i++)
                {
                    if (r.Next(2) == 0)
                    {
                        if (kind.Contains(mutter[i]))    //wenn die Location der Mutter beim Kind schon Vorhanden Ist, muss die Location des Vaters Verwendet werden.
                        {
                            if (kind.Contains(vater[i]))
                            {
                                break;
                            }
                            else
                            {
                                kind.Add(vater[i]);
                            }

                        }
                        else
                        {
                            kind.Add(mutter[i]);
                        }
                    }
                    else
                    {
                        if (kind.Contains(vater[i]))    //und anders herum
                        {
                            if (kind.Contains(mutter[i]))
                            {
                                break;
                            }
                            else
                            {
                                kind.Add(mutter[i]);
                            }
                        }
                        else
                        {
                            kind.Add(vater[i]);
                        }
                    }
                }
            } while (kind.Count < mutter.Count);

            return kind;
        }

        public List<int> Fortpflanzung_v2(List<int> mutter, List<int> vater)
        {
            List<int> kind = new List<int>();
            kind.AddRange(mutter.ToArray());
            int zufallsPosition = r.Next(mutter.Count);
            int zufallsStadtMutter = mutter[zufallsPosition];
            int zufallsStadtVater = vater[zufallsPosition];
            int aequivalenzStadtMutterPos = -1;

            for (int i = 0; i < mutter.Count; i++)
            {
                if (mutter[i] == zufallsStadtVater)
                {
                    aequivalenzStadtMutterPos = i;
                    break;
                }
            }

            kind[zufallsPosition] = zufallsStadtVater;
            kind[aequivalenzStadtMutterPos] = zufallsStadtMutter;
            return kind;
        }

        public List<int> Fortpflanzung_Perfektion(List<int> mutter, List<int> vater)
        {
            int synchronende = 0;
            for (int i = 0; i < mutter.Count; i++)
            {
                if (mutter[i] != vater[i])
                {
                    synchronende = i;
                    break;
                }
            }

            List<int> kind = new List<int>();
            kind.AddRange(mutter.ToArray());
            int zufallsPosition = r.Next(synchronende, mutter.Count);
            int zufallsStadtMutter = mutter[zufallsPosition];
            int zufallsStadtVater = vater[zufallsPosition];
            int aequivalenzStadtMutterPos = -1;

            for (int i = 0; i < mutter.Count; i++)
            {
                if (mutter[i] == zufallsStadtVater)
                {
                    aequivalenzStadtMutterPos = i;
                    break;
                }
            }

            kind[zufallsPosition] = zufallsStadtVater;
            kind[aequivalenzStadtMutterPos] = zufallsStadtMutter;
            return kind;
        }

        public List<int> Mutation(List<int> vorgaenger)
        {
            int wechselstelle = r.Next(vorgaenger.Count - 1);

            List<int> nachfolger = new List<int>();
            nachfolger.AddRange(vorgaenger.ToArray());

            int temp = nachfolger[wechselstelle];
            nachfolger[wechselstelle] = nachfolger[wechselstelle + 1];
            nachfolger[wechselstelle + 1] = temp;

            return nachfolger;
        }

        public List<int> BereichsMutation(List<int> vorgaenger)
        {
            throw new NotImplementedException();
        }
    }

    public class Comparer : IComparer<List<int>>
    {
        IReadOnlyList<Location> cities;

        public Comparer(IReadOnlyList<Location> _cities)
        {
            cities = _cities;
        }

        public int Compare(List<int> x, List<int> y)     //möglicherweise ist der vergleicher verkehrt!
        {
            double xLength = Utils.GetDistance(x, cities);
            double yLength = Utils.GetDistance(y, cities);
            if (xLength == yLength)
            {
                return 0;
            }
            if (xLength < yLength)
            {
                return -1;
            }
            else
            {
                return +1;
            }
        }
    }
}
