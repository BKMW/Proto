using System;
using System.IO;
using System.Linq;

namespace FindDeplucateLines
{
    class Program
    {
        static void Main(string[] args)
        {
           // var path = @"C:\Users\PC\Desktop\Gateway V3\BNAFILE20210621154401DETAIL.txt";
           // var newPath = @"C:\Users\PC\Desktop\Gateway V3\deplucated.txt";

            //var path = @"C:\Users\PC\Desktop\Gateway V3\BNAFILE20210621154401GROUPE.txt";
            //var newPath = @"C:\Users\PC\Desktop\Gateway V3\deplucatedG.txt";

            Console.WriteLine("Paste read path");
            var path = Console.ReadLine();
            Console.WriteLine("Paste write path");
            var newPath = Console.ReadLine();

            string[] lines = File.ReadAllLines(path);
            lines = lines.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
            File.WriteAllLines(newPath, lines);
            Console.WriteLine("KEY To Exit");
            Console.ReadKey();
        }
    }
}
