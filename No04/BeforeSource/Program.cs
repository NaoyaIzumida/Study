
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
        public const string CSV_FILE_PATH = @"data.csv";
        public const string OUTPUT_FILE_NAME = "output_{0}.csv";
        public const string DATETIME_FORMAT = "yyyyMMddHHmmss";
        public const int ELEMENT_LENGTH = 5;
        public const string EXCLUDE_NAME = "テスト";
        public static readonly string[] TYPE_CONDITION = {"A","B","Z" };

        static void Main(string[] args)
        {
            List<Item> list = new List<Item>();
            List<Item> extractItemList = new List<Item>();
            string output = string.Empty;

            // ファイル読み込み処理
            string[]? lines = ReadCsvFile(CSV_FILE_PATH);
            if (lines == null)
            {
                return;
            }

            // アイテムリストを作成
            foreach (var line in lines.Select((value, index) => new { value, index }))
            {
                string[] element = line.value.Split(',');
                int quantity;

                if (element.Length == ELEMENT_LENGTH)
                {
                    if (int.TryParse(element[2], out quantity))
                    {
                        Item item = new Item(element[0], element[1], quantity, element[3], element[4]);
                        list.Add(item);
                    }
                    else
                    {
                        //　数量が異常値の場合はスキップ
                        Console.WriteLine($"{line.index + 1}行目の数量が不正のためスキップしました。");
                    }
                }
                else
                {
                    // csv要素数が異常値の場合はスキップ
                    Console.WriteLine($"{line.index + 1}行目の要素数が不正のためスキップしました。");
                }
            }

            // 条件に合うItemの抽出
            extractItemList = list.Where(item => TYPE_CONDITION.Contains(item.Type)).Where(item => !item.Name.Contains(EXCLUDE_NAME)).Where(item => item.Quantity > 0).ToList();
                
            StringBuilder sb = new StringBuilder();
            foreach(var item in extractItemList){
                sb.AppendLine(string.Join(",", item.Name, item.Category, item.Quantity, item.Location, item.Type));
            }
            output = sb.ToString();

            //　出力処理
            string dt = DateTime.Now.ToString(DATETIME_FORMAT);
            string filename = string.Format(OUTPUT_FILE_NAME, dt);
            if (WriteCsvFile(filename, output))
            {
                Console.WriteLine("Success.");
            }
            else
            {
                Console.WriteLine("Failure.");
            }
        }

        /// <summary>
        /// ファイル読み込み処理
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string[]? ReadCsvFile(string filePath)
        {
            try
            {
                return File.ReadAllLines(filePath);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"例外発生：{ex.Message}");
                Console.WriteLine("ファイルが見つからなかったため処理を中止します。");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"例外発生：{ex.Message}");
                Console.WriteLine("ファイルの読み込みに失敗したため処理を中止します。");
                return null;
            }
        }

        /// <summary>
        /// ファイル書き込み処理
        /// </summary>
        /// <param name="fileName"></param>
        public static bool WriteCsvFile(string fileName, string output)
        {
            // 書き込み時のエラー(書き込み権限チェック)
            try
            {
                File.WriteAllText(fileName, output);
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"例外発生：{ex.Message}");
                Console.WriteLine("ファイルアクセスに失敗しました。");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"例外発生：{ex.Message}");
                Console.WriteLine("ファイルの書き込みに失敗しました。");
                return false;
            }
        }
    }
}
