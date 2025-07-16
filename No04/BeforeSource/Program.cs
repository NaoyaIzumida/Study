using System.Text;

namespace SampleApp
{
    class Program
    {
        /// <summary>
        /// Mainメソッド
        /// </summary>
        static void Main()
        {
            string inputFile = "data.csv";

            // ① ファイルの存在チェックを追加
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("ファイルが見つかりません: " + inputFile);
                return;
            }

            var lines = File.ReadAllLines(inputFile);

            // ② LINQを使用した読み込み処理
            var result = lines
                .Select(line => line.Split(','))
                .Where(s => s.Length == 5) // ④ 行の長さチェック
                .Select(s => 
                {
                    if (!int.TryParse(s[2], out int quantity)) // ③ int.TryParseに変更
                        quantity = 0;

                    return new Item
                    {
                        Name = s[0],
                        Category = s[1],
                        Quantity = quantity,
                        Location = s[3],
                        Type = s[4]
                    };
                })
                .Where(item =>
                    (item.Type == "A" || item.Type == "B" || item.Type == "Z") && 
                    (!item.Name.Contains("テスト") && item.Quantity > 0))
                .ToList();

            var sb = new StringBuilder(); // ⑤ StringBuilderを使用
            foreach (var item in result)
            {
                sb.Append(item.Name);
                sb.Append(',');
                sb.Append(item.Category);
                sb.Append(',');
                sb.Append(item.Quantity);
                sb.Append(Environment.NewLine);
            }
            Console.WriteLine(sb.ToString());

            var dt = DateTime.Now.ToString("yyyyMMddHHmmss");
            var filename = "output_" + dt + ".csv";
            try // ⑦ トライキャッチを使用
            {
                File.WriteAllText(filename, sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("ファイルの書き込み中にエラーが発生しました: " + ex.Message);
                return;
            }

            Console.WriteLine("Done.");
        }
    }

    /// <summary>
    /// Itemクラス
    /// </summary>
    class Item // ⑥ mainクラス外に移動
    {
        public required string Name;
        public required string Category;
        public int Quantity;
        public required string Location;
        public required string Type;
    }
}
