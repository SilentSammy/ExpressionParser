using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ExpressionParser
{
    public class ExpressionParser
    {
        static readonly DataTable computer = new DataTable();
        static readonly Dictionary<string, string> operators = new Dictionary<string, string>()
        {
            ["=="] = "=",
            ["!="] = "<>",
            ["&&"] = "AND",
            ["||"] = "OR",
        };
        public virtual IDictionary<string, object> Variables { get; }
        public virtual object GetVariable(string name) =>
            Variables[name];
        protected virtual IEnumerable<string> FindVariables(string expression) =>
            Variables.Keys.Where(k => expression.Contains(k));
        public ExpressionParser(IDictionary<string, object> variables = null) =>
            Variables = variables ?? new Dictionary<string, object>();
        public object Compute(string expression)
        {
            StringBuilder sb = new StringBuilder(expression);
            foreach (var key in FindVariables(expression))
                sb.Replace(key, GetVariable(key).ToString());
            foreach (var o in operators)
                sb.Replace(o.Key, o.Value);
            return computer.Compute(sb.ToString(), null);
        }
    }
}
