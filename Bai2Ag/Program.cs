using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai2Ag
{


    class Program
    {
        static void Main(string[] args)
        {
            Manage manage = new Manage("input.txt");
            manage.Cal();

            Console.ReadKey();
        }

        /*        public static IEnumerable<T[]> Combinations<T>(IEnumerable<T> source)
                {
                    if (null == source)
                        throw new ArgumentNullException(nameof(source));

                    T[] data = source.ToArray();

                    return Enumerable
                      .Range(0, 1 << (data.Length))
                      .Select(index => data
                         .Where((v, i) => (index & (1 << i)) != 0)
                         .ToArray());
                }*/



    }

    class Element
    {
        public Element(List<string> lt, string name)
        {
            this.lt = lt;
            this.name = name;
        }

        public List<string> lt { get; set; }
        public string name { get; set; }
    }
    class C
    {
        public C(List<string> lt)
        {
            this.lt = lt;
        }

        public List<string> lt { get; set; }
        public int count { get; set; }
    }
    class Manage
    {
        public List<Element> ltElement { get; set; }
        public List<string> strltElement { get; set; }
        public List<C> c { get; set; }
        public int Suport { get; set; }
        public float Suportpt { get; set; }
        public float Confident { get; set; }
        public Manage(string filepath)
        {
            ltElement = new List<Element>();
            strltElement = new List<string>();
            bool first = true;
            foreach (var s in File.ReadLines(filepath))
            {
                string[] str = s.Trim().Split(' ');

                if (str.Count() > 0)
                {
                    if (first) { first = false; Suportpt = float.Parse(str[0]); Confident = float.Parse(str[1]); continue; }
                    else
                    {
                        ltElement.Add(new Element(Array.FindAll(str, x => !x.Equals(str[0])).ToList(), str[0]));
                        strltElement.AddRange(Array.FindAll(str, x => !x.Equals(str[0])).ToList());
                    }
                }
            }

            strltElement = strltElement.Distinct().OrderBy(x => x).ToList() ;
            Suport = (int)(ltElement.Count * Suportpt);
        }

        public void setC(int level)
        {
            c = new List<C>();
            var lt = strltElement.Combinations(level);
            foreach (var item in lt)
                c.Add(new C(item.ToList()));
        }

        public void Cal()
        {
            for (int i = 1; i < 999; i++)
            {
                Console.Write($"\n\n[K={i}]:");

                setC(i);
                foreach (var item in c)
                {
                    foreach (var e in ltElement)
                    {
                        if (!item.lt.Except(e.lt).ToList().Any())
                            item.count++;
                    }

                }
                strltElement.Clear();
                foreach (var item in c)
                {
                    Console.Write($"\n[{string.Join(", ", item.lt)}] count: {item.count}");
                    if (item.count >= Suport)
                    {
                        strltElement.AddRange(item.lt);
                        Console.Write($" in L{i}");
                    }
                }
                if (strltElement.Count == 0) return;
                strltElement = strltElement.Distinct().OrderBy(x=>x).ToList();
            }

        }
    }
}
