using System.Text;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string csvpath = "data.csv";
                //csvファイル存在確認
                if (!File.Exists(csvpath)) throw new Exception("csvファイルが見つかりません") ;
                var list = new List<Item>();
                var TypeList = new HashSet<string> { "A", "B", "Z" };
                string exclusionString = "テスト";

                // CSVデータ一括読込
                var lines = File.ReadAllLines(csvpath);

                // 読込データを行単位で処理
                list = lines.Select(x =>
                {
                    var s = x.Split(',');
                    int temp_Quantity;
                    //要素数が異なる場合、エラー
                    if (s.Length != System.Enum.GetValues(typeof(Item.ItemName)).Length) throw new Exception("csvフォーマットエラー");

                    //Quantityが不正の場合、エラー
                    if (!int.TryParse(s[(int)Item.ItemName.Quantity], out temp_Quantity)) throw new Exception("Quantityフォーマットエラー");
                    return new Item
                    {
                        // 各要素に設定
                        Name = s[(int)Item.ItemName.Name],
                        Category = s[(int)Item.ItemName.Category],
                        Quantity = temp_Quantity,
                        Location = s[(int)Item.ItemName.Location],
                        Type = s[(int)Item.ItemName.Type]
                    };
                }).ToList();

                // Typeが[A,B,Z]、かつ、Nameが[テスト]以外、かつQuantity > 0を抽出
                list = list.Where(x =>
                            TypeList.Contains(x.Type) && !x.Name.Contains(exclusionString) && x.Quantity > 0).ToList();

                // 抽出した行のName、Category、Quantityを出力対象文字列に変換
                var output = new StringBuilder(
                    string.Join(Environment.NewLine,
                        list.Select(item => $"{item.Name},{item.Category},{item.Quantity}")
                    )
                );
                // 書き込み
                File.WriteAllText(
                    $"output_{DateTime.Now:yyyyMMddHHmmss}.csv",
                    output.ToString()
                );

                Console.WriteLine("Done.");
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        /// <summary>
        /// csvファイルの項目を表すクラス
        /// </summary>
        class Item
        {
            public enum ItemName
            {
                Name, Category, Quantity, Location, Type
            }
            public required string Name { get; set; }
            public required string Category { get; set; }
            public required int Quantity { get; set; }
            public required string Location { get; set; }
            public required string Type { get; set; }

            //コンストラクタ
            public Item()
            {
                Name = string.Empty;
                Category = string.Empty;
                Quantity = 0;
                Location = string.Empty;
                Type = string.Empty;
            }
        }
    }
}
