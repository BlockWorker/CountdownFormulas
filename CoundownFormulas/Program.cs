using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoundownFormulas {
    public class Program {
        public static void Main(string[] args) {
            int[] numbers;
            while (true) {
                Console.WriteLine("Available numbers, separated by one space: ");
                var strings = Console.ReadLine().Split(' ');
                try {
                    numbers = strings.Select(s => int.Parse(s)).ToArray();
                    break;
                } catch {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
            var tree = GetTree(numbers);
            while (true) {
                Console.Write("Number: ");
                var input = Console.ReadLine();
                if (input == "quit") break;
                else if (int.TryParse(input, out int num)) Console.WriteLine(GetShortestFormula(tree, num));
                else Console.WriteLine("Please input a valid number.");
            }
        }

        public static List<List<FormulaNode>> GetTree(int[] numbers) {
            var tree = new List<List<FormulaNode>>();
            var level = new List<FormulaNode>();

            //Level 0: Just the numbers
            foreach (int n in numbers) level.Add(new FormulaNode(n));
            tree.Add(level);

            for (int nLevel = 1; nLevel < numbers.Length; nLevel++) {
                level = new List<FormulaNode>();
                int iterations = nLevel % 2 == 0 ? nLevel / 2 : nLevel / 2 + 1;

                for (int i = 1; i <= iterations; i++) {
                    if (i - 1 == nLevel / 2) {
                        var parentLevel = tree[i - 1];
                        for (int j = 0; j < parentLevel.Count - 1; j++) {
                            var n1 = parentLevel[j];
                            for (int k = 1; k < parentLevel.Count; k++) {
                                var n2 = parentLevel[k];
                                if (n1 & n2) continue;
                                level.Add(new FormulaNode(n1, n2));
                            }
                        }
                    } else {
                        var parentLevel1 = tree[nLevel - i];
                        var parentLevel2 = tree[i - 1];
                        for (int j = 0; j < parentLevel1.Count; j++) {
                            var n1 = parentLevel1[j];
                            for (int k = 0; k < parentLevel2.Count; k++) {
                                var n2 = parentLevel2[k];
                                if (n1 & n2) continue;
                                level.Add(new FormulaNode(n1, n2));
                            }
                        }
                    }
                }

                tree.Add(level);
            }

            return tree;
        }

        public static string GetShortestFormula(List<List<FormulaNode>> tree, int number) {
            foreach (var level in tree) {
                var node = level.FirstOrDefault(n => n.Values.Contains(number));
                if (node == null) continue;
                return number.ToString() + " = " + node.Operations[number].ToString();
            }
            return "No exact solution for " + number;
        }
    }
}
