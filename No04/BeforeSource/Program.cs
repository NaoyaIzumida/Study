
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
            try{
                var list = new List<Item>();
                var extractItemList = new List<Item>();
                var lines = File.ReadAllLines("data.csv");
                var output = string.Empty;
                string[] condition = {"A","B","Z"};

                foreach (var line in lines)
                {
                    var s = line.Split(',');
                    int quantity;
                    if(int.TryParse(s[2], out quantity))
                    {
                        var item = new Item(s[0], s[1], quantity, s[3], s[4]);
                        list.Add(item);
                    }
                    else
                    {
                        Console.WriteLine("数量の値が不正");
                    }
                }

                // LINQで条件抽出
                extractItemList = list.Where(x => condition.Contains(item.type)).Where(y => !item.Name.Contains("テスト")).Where(z => z.Quantity > 0).ToList();
                StringBuilder sb = new StringBuilder();

                foreach(var item in extractItemList){
                    sb.Append(item.Name);
                    sb.Append(string.Join(",", item.Category));
                    sb.Append(string.Join(",", item.Quantity.ToString()))
                }

                var dt = DateTime.Now.ToString("yyyyMMddHHmmss");
                var filename = "output_" + dt + ".csv";
                File.WriteAllText(filename, output);

                Console.WriteLine("Done.");
            }
            catch(Exception ex){

            }
            
        }

        class Item
        {
            public string Name {get; set;};
            public string Category {get; set;};
            public int Quantity {get; set;};
            public string Location {get; set;};
            public string Type {get; set;};

            public Item(string name, string category, int quantity, string location, string type)
            {
                this.Name = name;
                this.Category = category;
                this.Quantity = quantity;
                this.Location = location;
                this.Type = type;
            }
        }
    }
}
