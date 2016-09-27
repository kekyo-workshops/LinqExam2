using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExam2
{
    class Program
    {
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

        static void Main(string[] args)
        {
            var x_ken_all = new TextFieldContext("x_ken_all.csv");

            //var results = ExtractByNumberByLinq(x_ken_all, 9000000);
            var results = ExtractByNumberByLinqQuery(x_ken_all, 9000000);
            Console.WriteLine(string.Join("\r\n", results));
        }
    }
}
