﻿using System;
using System.Collections.Generic;

namespace GrafFeladat_CSharp
{
    /// <summary>
    /// Irányítatlan, egyszeres gráf.
    /// </summary>
    class Graf
    {
        int csucsokSzama;
        /// <summary>
        /// A gráf élei.
        /// Ha a lista tartalmaz egy(A, B) élt, akkor tartalmaznia kell
        /// a(B, A) vissza irányú élt is.
        /// </summary>
        readonly List<El> elek = new List<El>();
        /// <summary>
        /// A gráf csúcsai.
        /// A gráf létrehozása után új csúcsot nem lehet felvenni.
        /// </summary>
        readonly List<Csucs> csucsok = new List<Csucs>();

        /// <summary>
        /// Létehoz egy úgy, N pontú gráfot, élek nélkül.
        /// </summary>
        /// <param name="csucsok">A gráf csúcsainak száma</param>
        public Graf(int csucsok)
        {
            this.csucsokSzama = csucsok;

            // Minden csúcsnak hozzunk létre egy új objektumot
            for (int i = 0; i < csucsok; i++)
            {
                this.csucsok.Add(new Csucs(i));
            }
        }

        /// <summary>
        /// Hozzáad egy új élt a gráfhoz.
        /// Mindkét csúcsnak érvényesnek kell lennie:
        /// 0 &lt;= cs &lt; csúcsok száma.
        /// </summary>
        /// <param name="cs1">Az él egyik pontja</param>
        /// <param name="cs2">Az él másik pontja</param>
        public void Hozzaad(int cs1, int cs2)
        {
            if (cs1 < 0 || cs1 >= csucsokSzama ||
                cs2 < 0 || cs2 >= csucsokSzama)
            {
                throw new ArgumentOutOfRangeException("Hibas csucs index");
            }

            // Ha már szerepel az él, akkor nem kell felvenni
            foreach (var el in elek)
            {
                if (el.Csucs1 == cs1 && el.Csucs2 == cs2)
                {
                    return;
                }
            }

            elek.Add(new El(cs1, cs2));
            elek.Add(new El(cs2, cs1));
        }
        public void SzelessegBejar(int kezdes)
        {
            var bejart = new HashSet<int>();
            var kovetkezo = new Queue<int>();

            kovetkezo.Enqueue(kezdes);

            bejart.Add(kezdes);

            while (kovetkezo.Count > 0)
            {
                var k = kovetkezo.Dequeue();

                Console.WriteLine(this.csucsok[k]);

                foreach (El el in this.elek)
                {
                    if ((el.Csucs1 == k) && (!bejart.Contains(el.Csucs2)))
                    {
                        bejart.Add(el.Csucs2);
                        kovetkezo.Enqueue(el.Csucs2);
                    }
                }
            }
        }
        public void MelysegBejar(int kezdes)
        {
            var bejart = new HashSet<int>();
            var kovetkezo = new Stack<int>();

            kovetkezo.Push(kezdes);

            bejart.Add(kezdes);

            while (kovetkezo.Count > 0)
            {
                var k = kovetkezo.Pop();

                Console.WriteLine(this.csucsok[k]);

                foreach (El el in this.elek)
                {
                    if ((el.Csucs1 == k) && (!bejart.Contains(el.Csucs2)))
                    {
                        bejart.Add(el.Csucs2);
                        kovetkezo.Push(el.Csucs2);
                    }
                }
            }
        }

        public bool Osszefuggo()
        {
            var bejart = new HashSet<int>();
            var kovetkezo = new Queue<int>();

            kovetkezo.Enqueue(0);

            bejart.Add(0);

            while (kovetkezo.Count > 0)
            {
                var k = kovetkezo.Dequeue();

                foreach (El el in this.elek)
                {
                    if ((el.Csucs1 == k) && (!bejart.Contains(el.Csucs2)))
                    {
                        bejart.Add(el.Csucs2);
                        kovetkezo.Enqueue(el.Csucs2);
                    }
                }
            }
            if (bejart.Count == this.csucsokSzama)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Graf Feszitofa()
        {
            var fa = new Graf(this.csucsokSzama);
            var bejart = new HashSet<int>();
            var kovetkezo = new Queue<int>();

            kovetkezo.Enqueue(0);

            bejart.Add(0);

            while (kovetkezo.Count > 0)
            {
                var k = kovetkezo.Dequeue();
                Console.WriteLine(this.csucsok[k]);

                foreach (El el in this.elek)
                {
                    if ((el.Csucs1 == k) && (!bejart.Contains(el.Csucs2)))
                    {
                        bejart.Add(el.Csucs2);
                        kovetkezo.Enqueue(el.Csucs2);
                        fa.Hozzaad(el.Csucs1, el.Csucs2);
                    }
                }
            }
            return fa;
        }

        public Dictionary<int, int> mohoAlgoritmus()
        {
            var szinezes = new Dictionary<int, int>();
            int maxSzin = this.csucsokSzama;

            for (int aktCsucs = 0; aktCsucs < this.csucsokSzama; aktCsucs++)
            {
                var szinek = new HashSet<int>();

                for (int i = 0; i < this.csucsokSzama; i++)
                {
                    szinek.Add(i);
                }

                foreach (El el in this.elek)
                {
                    if (el.Csucs1 == aktCsucs)
                    {
                        if (szinezes.ContainsKey(el.Csucs2))
                        {
                            var szin = szinezes[el.Csucs2];
                            szinek.Remove(szin);
                        }
                    }
                }

                var valasztottSzin = Min(szinek);
                szinezes.Add(aktCsucs, valasztottSzin);
            }

            return szinezes;
        }

        public int Min(HashSet<int> halmaz)
        {
            int min = int.MaxValue;

            foreach (int elem in halmaz)
            {
                if (elem < min)
                {
                    min = elem;
                }
            }

            return min;
        }




        public override string ToString()
        {
            string str = "Csucsok:\n";
            foreach (var cs in csucsok)
            {
                str += cs + "\n";
            }
            str += "Elek:\n";
            foreach (var el in elek)
            {
                str += el + "\n";
            }
            return str;
        }
    }
}