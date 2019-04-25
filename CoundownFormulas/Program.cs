using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoundownFormulas {
    public class Program {
        public static void Main(string[] args) {
            var numbers = new int[] { 3, 12, 30, 56, 60, 90 };
            GetTree(numbers);
        }

        public static List<HashSet<int>> GetTree(int[] numbers) {
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

            var ret = new List<HashSet<int>>();
            foreach (var lv in tree) {
                var set = new HashSet<int>();
                foreach (var node in lv) {
                    foreach (int v in node.Values) set.Add(v);
                }
                ret.Add(set);
            }

            return ret;
        }
    }
}
