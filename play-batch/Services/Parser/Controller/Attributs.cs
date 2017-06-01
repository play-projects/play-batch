using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using batch.Services.Parser.Model;

namespace batch.Services.Parser.Controller
{
    public class Attributs
    {
        public List<AttributModel> GetAttributs(string str)
        {
            var attributes = new List<AttributModel>();

            var pAttribut = "[a-zA-Z0-9-_]+=(\"[^\"]*\"+|'[^']*'+)";
            var mAttributes = new Regex(pAttribut).Matches(str);

            foreach (Match mAttribut in mAttributes)
            {
                var key = mAttribut.Value.Split('=')[0];
                var equal = mAttribut.Value.IndexOf('=');
                var value = mAttribut.Value.Substring(equal + 2, mAttribut.Value.Length - equal - 3);
                var values = new List<string>();
                if (!key.Equals("class"))
                    values.Add(value);
                else values = value.Split(' ').Select(v => v.Trim()).ToList();
                attributes.Add(new AttributModel(key, values));
            }
            return attributes;
        }
    }
}
