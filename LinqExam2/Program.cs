using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExam2
{
    class Program
    {
        #region TestTemplate
        private static void TestTemplate()
        {
            // LINQデータソース (CSVを読み取り、LINQ可能にする）
            // 列挙される「文字列配列」は、一行のカラム群を示す。
            IEnumerable<string[]> x_ken_all = new TextFieldContext("x_ken_all.csv");

            // 郵便番号辞書は、以下のようなCSVフォーマット:
            // 01101,"064  ","0640941","ホッカイドウ","サッポロシチュウオウク","アサヒガオカ","北海道","札幌市中央区","旭ケ丘",0,0,1,0,0,0

            // 以下は全ての郵便番号を抽出し、数値化し、重複を除去する:
            IEnumerable<int> zipCodes =
                x_ken_all.
                Select(columns => int.Parse(columns[2])).   // カラム2（郵便番号）
                Distinct();
        }
        #endregion

        #region Samples
        private static string[] ExtractByNumberByLinq(TextFieldContext data, int floor)
        {
            // 01101,"064  ","0640941","ホッカイドウ","サッポロシチュウオウク","アサヒガオカ","北海道","札幌市中央区","旭ケ丘",0,0,1,0,0,0
            return
                data.
                Where(columns => int.Parse(columns[2]) >= floor).  // 900-0000
                OrderBy(columns => columns[2]).
                Select(columns => $"{columns[2]}, {columns[6]}{columns[7]}{columns[8]}").
                ToArray();
        }

        private static string[] ExtractByNumberByLinqQuery(TextFieldContext data, int floor)
        {
            // 01101,"064  ","0640941","ホッカイドウ","サッポロシチュウオウク","アサヒガオカ","北海道","札幌市中央区","旭ケ丘",0,0,1,0,0,0
            return
               (from columns in data
                where int.Parse(columns[2]) >= floor
                orderby columns[2]
                select $"{columns[2]}, {columns[6]}{columns[7]}{columns[8]}").
                ToArray();
        }

        private static void TestSample()
        {
            var x_ken_all = new TextFieldContext("x_ken_all.csv");

            //var results = ExtractByNumberByLinq(x_ken_all, 9000000);
            var results = ExtractByNumberByLinqQuery(x_ken_all, 9000000);
            Console.WriteLine(string.Join("\r\n", results));
        }
        #endregion

        #region TestJoin
        private static void TestJoin()
        {
            var x_ken_all = new TextFieldContext("x_ken_all.csv");

            // 01101,"064  ","0640941","ホッカイドウ","サッポロシチュウオウク","アサヒガオカ","北海道","札幌市中央区","旭ケ丘",0,0,1,0,0,0
            var ids = new[] {8102, 50302, 62924, 72962};
            var results =
                from columns in x_ken_all
                join id in ids
                    on int.Parse(columns[1]) equals id
                select $"{columns[2]}, {columns[6]}{columns[7]}{columns[8]}";

            Console.WriteLine(string.Join("\r\n", results));
        }

        private static void TestJoinUseMethod()
        {
            var x_ken_all = new TextFieldContext("x_ken_all.csv");

            // 01101,"064  ","0640941","ホッカイドウ","サッポロシチュウオウク","アサヒガオカ","北海道","札幌市中央区","旭ケ丘",0,0,1,0,0,0
            var ids = new[] {8102, 50302, 62924, 72962};
            var results = x_ken_all.Join(
                ids,
                columns => int.Parse(columns[1]),
                id => id,
                (columns, id) => $"{columns[2]}, {columns[6]}{columns[7]}{columns[8]}");

            Console.WriteLine(string.Join("\r\n", results));
        }

        private static void TestJoinUseContainsBruteForce()
        {
            var x_ken_all = new TextFieldContext("x_ken_all.csv");

            // 01101,"064  ","0640941","ホッカイドウ","サッポロシチュウオウク","アサヒガオカ","北海道","札幌市中央区","旭ケ丘",0,0,1,0,0,0
            var ids = new[] { 8102, 50302, 62924, 72962 };
            var results =
                from columns in x_ken_all
                where ids.Contains(int.Parse(columns[1]))
                select $"{columns[2]}, {columns[6]}{columns[7]}{columns[8]}";

            Console.WriteLine(string.Join("\r\n", results));
        }
        #endregion

        #region TestGroupBy()
        private static void TestGroupBy()
        {
            var x_ken_all = new TextFieldContext("x_ken_all.csv");

            // 01101,"064  ","0640941","ホッカイドウ","サッポロシチュウオウク","アサヒガオカ","北海道","札幌市中央区","旭ケ丘",0,0,1,0,0,0
            var results =
                from columns in x_ken_all
                group columns by int.Parse(columns[1])
                into g
                select new
                {
                    ID = (int)g.Key,
                    Infos = (IEnumerable<string>)g.Select(
                        columns => $"{columns[2]}, {columns[6]}{columns[7]}{columns[8]}")
                };

            Console.WriteLine(string.Join(
                "\r\n",
                results.Select(entry =>
                    string.Format("ID={0},\r\n  {1}",
                        entry.ID,
                        string.Join("\r\n  ", entry.Infos)))));
        }

        private static void TestGroupByUseMethod()
        {
            var x_ken_all = new TextFieldContext("x_ken_all.csv");

            // 01101,"064  ","0640941","ホッカイドウ","サッポロシチュウオウク","アサヒガオカ","北海道","札幌市中央区","旭ケ丘",0,0,1,0,0,0
            var results = x_ken_all.GroupBy(
                columns => int.Parse(columns[1]),
                columns => columns).
                Select(g =>
                    new {
                        ID = (int)g.Key,
                        Infos = (IEnumerable<string>)g.Select(
                            columns => $"{columns[2]}, {columns[6]}{columns[7]}{columns[8]}")
                    });

            Console.WriteLine(string.Join(
                "\r\n",
                results.Select(entry =>
                    string.Format("ID={0},\r\n  {1}",
                        entry.ID,
                        string.Join("\r\n  ", entry.Infos)))));
        }

        private static void TestGroupByNoMap()
        {
            var x_ken_all = new TextFieldContext("x_ken_all.csv");

            // 01101,"064  ","0640941","ホッカイドウ","サッポロシチュウオウク","アサヒガオカ","北海道","札幌市中央区","旭ケ丘",0,0,1,0,0,0
            IEnumerable<IGrouping<int, string[]>> results =
                x_ken_all.GroupBy(
                    columns => int.Parse(columns[1]),
                    columns => columns);
        }
        #endregion

        static void Main(string[] args)
        {
            //TestSample();
            //TestJoinUseMethod();
            //TestJoinUseContainsBruteForce();
            //TestGroupBy();
            TestGroupByUseMethod();
        }
    }
}
