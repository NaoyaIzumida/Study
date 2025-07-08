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
                var lines = File.ReadAllLines(csvpath);

                list = lines.Select(x =>
                {
                    var s = x.Split(',');
                    int temp_Quantity;
                    if (s.Length != System.Enum.GetValues(typeof(Item.ItemName)).Length) throw new Exception("csvフォーマットエラー");
                    if (!int.TryParse(s[(int)Item.ItemName.Quantity], out temp_Quantity)) throw new Exception("Quantityフォーマットエラー");
                    return new Item
                    {
                        Name = s[(int)Item.ItemName.Name],
                        Category = s[(int)Item.ItemName.Category],
                        Quantity = temp_Quantity,
                        Location = s[(int)Item.ItemName.Location],
                        Type = s[(int)Item.ItemName.Type]
                    };
                }).ToList();

                list = list.Where(x =>
                            TypeList.Contains(x.Type) && !x.Name.Contains("テスト") && x.Quantity > 0).ToList();

                var output = new StringBuilder();
                foreach (var item in list)
                {
                    output.AppendLine(item.Name + "," + item.Category + "," + item.Quantity);
                }

                var filename = new StringBuilder();
                filename.AppendFormat("output_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
                File.WriteAllText(filename.ToString(), output.ToString());

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
