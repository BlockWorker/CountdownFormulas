using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoundownFormulas {
    public class FormulaNode {
        public List<FormulaNode> Roots = new List<FormulaNode>();
        public HashSet<int> Values = new HashSet<int>();
        public Dictionary<int, FormulaOperation> Operations = new Dictionary<int, FormulaOperation>();

        public FormulaNode(int number) {
            Roots.Add(this);
            Values.Add(number);
            Operations.Add(number, new FormulaOperation(null, null, FormulaOperator.Constant, number));
        }

        public FormulaNode(FormulaNode p1, FormulaNode p2) {
            if (p1 & p2) throw new ArgumentException("Parents cannot share roots");

            Roots.AddRange(p1.Roots);
            Roots.AddRange(p2.Roots);

            int v;
            foreach (int a in p1.Values) {
                foreach (int b in p2.Values) {
                    var opA = p1.Operations[a];
                    var opB = p2.Operations[b];
                    v = a + b;
                    if (Values.Add(v)) Operations.Add(v, new FormulaOperation(opA, opB, FormulaOperator.Add, v));
                    v = a - b;
                    if (v >= 0) {
                        if (Values.Add(v)) Operations.Add(v, new FormulaOperation(opA, opB, FormulaOperator.Subtract, v));
                    }
                    v = b - a;
                    if (v >= 0) {
                        if (Values.Add(v)) Operations.Add(v, new FormulaOperation(opB, opA, FormulaOperator.Subtract, v));
                    }
                    v = a * b;
                    if (Values.Add(v)) Operations.Add(v, new FormulaOperation(opA, opB, FormulaOperator.Mutiply, v));
                    if (b > 1 && a % b == 0) {
                        v = a / b;
                        if (Values.Add(v)) Operations.Add(v, new FormulaOperation(opA, opB, FormulaOperator.Divide, v));
                    }
                    if (a > 1 && b % a == 0) {
                        v = b / a;
                        if (Values.Add(v)) Operations.Add(v, new FormulaOperation(opB, opA, FormulaOperator.Divide, v));
                    }
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

    public enum FormulaOperator {
        Add,
        Subtract,
        Mutiply,
        Divide,
        Constant
    }

    public class FormulaOperation {
        public FormulaOperation a;
        public FormulaOperation b;
        public FormulaOperator op;
        public int value;

        public FormulaOperation(FormulaOperation a, FormulaOperation b, FormulaOperator op, int value) {
            this.a = a;
            this.b = b;
            this.op = op;
        }
    }
}
