
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SampleApp
{
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

    class Program
    {
        static void Main(string[] args)
        {
            try{
                var list = new List<Item>();
                var extractItemList = new List<Item>();
                // エラー1
                var lines = File.ReadAllLines("data.csv");
                var output = string.Empty;
                string[] condition = {"A","B","Z"};

                // 行データからアイテムリストを作成
                foreach (var line in lines)
                {
                    var s = line.Split(',');
                    int quantity;
                    // エラー2 解決済
                    if(int.TryParse(s[2], out quantity))
                    {
                        // CSVデータは要素数足りてる？
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
                
                // 文字列結合
                StringBuilder sb = new StringBuilder();
                foreach(var item in extractItemList){
                    sb.Append(item.Name);
                    sb.Append(string.Join(",", item.Category));
                    sb.Append(string.Join(",", item.Quantity.ToString()))
                }

                // 出力
                var dt = DateTime.Now.ToString("yyyyMMddHHmmss");
                var filename = "output_" + dt + ".csv";
                // エラー4
                File.WriteAllText(filename, output);

                Console.WriteLine("Done.");
            }
            catch(Exception ex){

            }
        }
    }
}
