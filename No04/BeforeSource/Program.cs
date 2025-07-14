
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
            var lines = File.ReadAllLines("data.csv"); // ①ファイルが存在してるかどうかのチェックが必要？
            // ②linqを読み込み時に使う

            foreach (var line in lines)
            {
                var s = line.Split(',');
                var item = new Item(); // 長さ指定していないのは大丈夫？
                item.Name = s[0];
                item.Category = s[1];
                item.Quantity = int.Parse(s[2]); // ③int.TryParse　←　型チェックが必要
                item.Location = s[3];
                item.Type = s[4]; // ④.csvファイルのデータが5つで区切れるとは限らない　←　行の長さチェックが必要
                list.Add(item); // リストだから長さ指定は必要ない
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
                // ⑤stringbuilderを使う
                output += item.Name + "," + item.Category + "," + item.Quantity + Environment.NewLine;
            }

            var dt = DateTime.Now.ToString("yyyyMMddHHmmss");
            var filename = "output_" + dt + ".csv"; // アウトプットが空の場合はどうするのか？
            File.WriteAllText(filename, output); //　⑦トライキャッチが必要

            Console.WriteLine("Done.");
        }

        class Item // ⑥クラスの中にクラスを作らない
        {
            public string Name; // 初期化 もしくは get set の方に変更とか？
            public string Category;
            public int Quantity;
            public string Location;
            public string Type;
        }
    }
}
