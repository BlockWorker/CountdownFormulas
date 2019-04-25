using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoundownFormulas {
    public class FormulaNode {
        public List<FormulaNode> Roots = new List<FormulaNode>();
        public HashSet<int> Values = new HashSet<int>();

        public FormulaNode(int number) {
            Roots.Add(this);
            Values.Add(number);
        }

        public FormulaNode(FormulaNode p1, FormulaNode p2) {
            if (p1 & p2) throw new ArgumentException("Parents cannot share roots");

            Roots.AddRange(p1.Roots);
            Roots.AddRange(p2.Roots);

            int v;
            foreach (int a in p1.Values) {
                foreach (int b in p2.Values) {
                    v = a + b;
                    if (v <= 999) Values.Add(v);
                    v = a - b;
                    if (v >= 0) Values.Add(v);
                    v = b - a;
                    if (v >= 0) Values.Add(v);
                    v = a * b;
                    if (v <= 999) Values.Add(v);
                    if (b > 1 && a % b == 0) Values.Add(a / b);
                    if (a > 1 && b % a == 0) Values.Add(b / a);
                }
            }
        }

        public static bool operator &(FormulaNode a, FormulaNode b) {
            return a.Roots.Intersect(b.Roots).Count() > 0;
        }

        public override string ToString() {
            string s = "[ ";
            foreach (int v in Values) s += v + " ";
            return s + "]";
        }
    }
}
