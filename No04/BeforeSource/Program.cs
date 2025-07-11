
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SampleApp
{
    class Item
    {
        public string Name {get; set;}
        public string Category {get; set;}
        public int Quantity {get; set;}
        public string Location {get; set;}
        public string Type {get; set;}

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
                // 読み込み時のエラー(ファイルの存在チェック)
                var lines = File.ReadAllLines("data.csv");
                var output = string.Empty;
                string[] condition = {"A","B","Z"};

                // 行データからアイテムリストを作成
                foreach (var line in lines)
                {
                    var s = line.Split(',');
                    int quantity;
                    // parse時のエラー 解決済
                    if(int.TryParse(s[2], out quantity))
                    {
                        // CSVデータは要素数チェック(足りない場合は処理を中断)
                        var item = new Item(s[0], s[1], quantity, s[3], s[4]);
                        list.Add(item);
                    }
                    else
                    {
                        //　処理スキップするか中断するか続行するか
                        Console.WriteLine($"{s[0]}の数量が不正");
                    }
                }

                // LINQで条件抽出
                extractItemList = list.Where(item => condition.Contains(item.Type)).Where(item => !item.Name.Contains("テスト")).Where(item => item.Quantity > 0).ToList();
                
                // 文字列結合
                StringBuilder sb = new StringBuilder();
                foreach(var item in extractItemList){
                    sb.Append(item.Name);
                    sb.Append(string.Join(",", item.Category));
                    sb.AppendLine(string.Join(",", item.Quantity.ToString()));
                }

                // 出力
                var dt = DateTime.Now.ToString("yyyyMMddHHmmss");
                var filename = "output_" + dt + ".csv";
                // 書き込み時のエラー(書き込み権限チェック)
                File.WriteAllText(filename, output);

                Console.WriteLine("Done.");
            }
            catch(Exception ex){

            }
        }
    }
}
