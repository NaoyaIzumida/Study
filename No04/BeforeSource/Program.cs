
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<Item>();
            var lines = File.ReadAllLines("data.csv");
            foreach (var line in lines)
            {
                var s = line.Split(',');
                var item = new Item();
                item.Name = s[0];
                item.Category = s[1];
                item.Quantity = int.Parse(s[2]);
                item.Location = s[3];
                item.Type = s[4];
                list.Add(item);
            }

            var result = new List<Item>();
            foreach (var item in list)
            {
                if (item.Type == "A" || item.Type == "B" || item.Type == "Z")
                {
                    if (!item.Name.Contains("テスト") && item.Quantity > 0)
                    {
                        result.Add(item);
                    }
                }
            }

            var output = "";
            foreach (var item in result)
            {
                output += item.Name + "," + item.Category + "," + item.Quantity + Environment.NewLine;
            }

            var dt = DateTime.Now.ToString("yyyyMMddHHmmss");
            var filename = "output_" + dt + ".csv";
            File.WriteAllText(filename, output);

            Console.WriteLine("Done.");
        }

        class Item
        {
            public string Name;
            public string Category;
            public int Quantity;
            public string Location;
            public string Type;
        }
    }
}
