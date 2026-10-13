namespace PascalABCNet.LanguageServices;

internal static class SymbolDescriptionParser
{
    public static (string Label, string? Documentation) Split(string description)
    {
        var separator = description.IndexOf('\n');
        if (separator < 0)
            return (description.TrimEnd('\r'), null);

        var label = description[..separator].TrimEnd('\r');
        var documentation = description[(separator + 1)..].Trim();
        return (label, string.IsNullOrWhiteSpace(documentation) ? null : documentation);
    }

    public static SignatureInfo ParseSignature(string description)
    {
        var (label, documentation) = Split(description);
        return new SignatureInfo(label, documentation, ParseParameters(label));
    }

    private static IReadOnlyList<string> ParseParameters(string label)
    {
        var open = label.IndexOf('(');
        if (open < 0)
            return Array.Empty<string>();

        var close = FindClosingParenthesis(label, open);
        if (close <= open + 1)
            return Array.Empty<string>();

        var result = new List<string>();
        var start = open + 1;
        var roundDepth = 0;
        var squareDepth = 0;
        var angleDepth = 0;

        for (var index = start; index < close; index++)
        {
            switch (label[index])
            {
                case '(':
                    roundDepth++;
                    break;
                case ')':
                    roundDepth--;
                    break;
                case '[':
                    squareDepth++;
                    break;
                case ']':
                    squareDepth--;
                    break;
                case '<':
                    angleDepth++;
                    break;
                case '>':
                    angleDepth--;
                    break;
                case ',' or ';' when roundDepth == 0 && squareDepth == 0 && angleDepth == 0:
                    AddParameter(label[start..index], result);
                    start = index + 1;
                    break;
            }
        }

        AddParameter(label[start..close], result);
        return result;
    }

    private static int FindClosingParenthesis(string label, int open)
    {
        var depth = 0;
        for (var index = open; index < label.Length; index++)
        {
            if (label[index] == '(')
                depth++;
            else if (label[index] == ')' && --depth == 0)
                return index;
        }

        return -1;
    }

    private static void AddParameter(string parameter, ICollection<string> result)
    {
        var value = parameter.Trim();
        if (value.Length > 0)
            result.Add(value);
    }
}
