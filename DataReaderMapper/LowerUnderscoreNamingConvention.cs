using System.Text.RegularExpressions;

namespace DataReaderMapper
{
    public class LowerUnderscoreNamingConvention : INamingConvention
    {
        public Regex SplittingExpression { get; } = new Regex(@"[\p{Ll}\p{Lu}0-9]+(?=_?)");

        public string SeparatorCharacter => "_";

        public string ReplaceValue(Match match)
        {
            return match.Value.ToLower();
        }
    }
}