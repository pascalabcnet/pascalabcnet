namespace PascalABCNet.LanguageServices;

public readonly record struct TextPosition(int Line, int Character);

public static class TextCoordinates
{
    public static TextPosition GetPosition(string text, int offset)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (offset < 0 || offset > text.Length)
            throw new ArgumentOutOfRangeException(nameof(offset));

        var line = 0;
        var character = 0;

        for (var index = 0; index < offset; index++)
        {
            if (text[index] == '\n')
            {
                line++;
                character = 0;
            }
            else
            {
                character++;
            }
        }

        return new TextPosition(line, character);
    }

    public static int GetOffset(string text, TextPosition position)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (position.Line < 0 || position.Character < 0)
            throw new ArgumentOutOfRangeException(nameof(position));

        var line = 0;
        var lineStart = 0;

        while (line < position.Line)
        {
            var lineEnd = text.IndexOf('\n', lineStart);
            if (lineEnd < 0)
                throw new ArgumentOutOfRangeException(nameof(position));

            lineStart = lineEnd + 1;
            line++;
        }

        var contentEnd = text.IndexOf('\n', lineStart);
        if (contentEnd < 0)
            contentEnd = text.Length;
        if (contentEnd > lineStart && text[contentEnd - 1] == '\r')
            contentEnd--;

        var offset = lineStart + position.Character;
        if (offset > contentEnd)
            throw new ArgumentOutOfRangeException(nameof(position));

        return offset;
    }
}
