using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExam2
{
    class Program
    {
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

        static void Main(string[] args)
        {
            //TestSample();
            TestJoin();
        }
    }
}
