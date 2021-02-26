using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionParser
{
    public class JExpressionParser : ExpressionParser
    {
        const string self = ". ";
        public JToken JVariables { get; set; }
        public override object GetVariable(string name) => name == self ? JVariables : JVariables.SelectToken(name);
        static List<string> GetAllPaths(JToken token, bool isBaseToken = false)
        {
            var list = new List<string>();
            if (token != null)
            {
                foreach (var item in token.Children())
                {
                    if (item.HasValues) list.AddRange(GetAllPaths(item));
                    else list.Add(item.Path);
                }
                if (isBaseToken && token.Parent != null)
                    for (int i = 0; i < list.Count; i++)
                        list[i] = list[i].Substring(token.Path.Length + 1);
                if (isBaseToken) list.Add(self);
            }
            return list;
        }
        protected override IEnumerable<string> FindVariables(string expression) =>
            GetAllPaths(JVariables, true).Where(p => expression.Contains(p));
        public JExpressionParser(JToken jVariables = null)
        {
            JVariables = jVariables ?? new JObject();
        }
    }
}