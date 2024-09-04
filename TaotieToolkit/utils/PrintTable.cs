using System;
using System.Collections.Generic;
using System.Linq;

namespace TaotieToolkit.utils
{
    public class PrintTable
    {
        public static void PrintFunc(string[] headers, List<Dictionary<string, string>> rows)
        {
            int[] columnWidths = GetColumnWidths(headers, rows);
            int totalWidth = columnWidths.Sum() + 3 * (headers.Length - 1); // 总宽度包括列间空格

            // 打印表头
            PrintRow(headers, columnWidths);
            // 打印分隔线
            PrintSeparator(totalWidth);

            // 打印数据行
            foreach (var row in rows)
            {
                PrintRow(headers, row, columnWidths);
            }
        }

        private static void PrintRow(string[] headers, Dictionary<string, string> row, int[] columnWidths)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                Console.Write($"| {row[headers[i]].PadRight(columnWidths[i])} ");
            }

            Console.WriteLine("|");
        }

        private static void PrintRow(string[] headers, int[] columnWidths)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                Console.Write($"| {headers[i].PadRight(columnWidths[i])} ");
            }

            Console.WriteLine("|");
        }

        private static void PrintSeparator(int totalWidth)
        {
            Console.WriteLine(new string('-', totalWidth));
        }

        private static int[] GetColumnWidths(string[] headers, List<Dictionary<string, string>> rows)
        {
            int[] widths = headers.Select(header => header.Length).ToArray();

            foreach (var row in rows)
            {
                for (int i = 0; i < headers.Length; i++)
                {
                    widths[i] = Math.Max(widths[i], row[headers[i]].Length);
                }
            }

            return widths;
        }
    }
}