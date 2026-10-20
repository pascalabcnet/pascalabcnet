using System;

namespace VisualPascalABCPlugins
{
    // Recognize only a directive on its own line, including an unfinished one.
    // This deliberately does not require a successful Pascal parse.
    internal sealed class Net10PackageDirectiveContext
    {
        public readonly string Prefix;
        public readonly string PackageId;
        public readonly bool IsVersion;
        public readonly bool HasClosingBrace;

        private Net10PackageDirectiveContext(string prefix, string packageId, bool isVersion, bool hasClosingBrace)
        {
            Prefix = prefix; PackageId = packageId; IsVersion = isVersion; HasClosingBrace = hasClosingBrace;
        }

        public static Net10PackageDirectiveContext Parse(string text, int offset)
        {
            if (text == null || offset < 0 || offset > text.Length) return null;
            int lineStart = offset == 0 ? 0 : text.LastIndexOf('\n', offset - 1) + 1;
            string before = text.Substring(lineStart, offset - lineStart);
            int start = before.IndexOf("{$package", StringComparison.OrdinalIgnoreCase);
            if (start < 0 || !string.IsNullOrWhiteSpace(before.Substring(0, start))) return null;
            string body = before.Substring(start + "{$package".Length);
            if (body.Length != 0 && !char.IsWhiteSpace(body[0]) || body.IndexOfAny(new[] { '{', '}' }) >= 0)
                return null;
            body = body.TrimStart();
            string tail = text.Substring(offset);
            int lineEnd = tail.IndexOfAny(new[] { '\r', '\n' });
            if (lineEnd >= 0) tail = tail.Substring(0, lineEnd);
            tail = tail.TrimStart();
            bool closing = tail.StartsWith("}", StringComparison.Ordinal);
            if (tail.Length != 0 && !closing || closing && !string.IsNullOrWhiteSpace(tail.Substring(1)))
                return null;

            int separator = body.IndexOfAny(new[] { ' ', '\t' });
            if (separator < 0)
            {
                if (!Valid(body, false)) return null;
                return new Net10PackageDirectiveContext(body, null, false, closing);
            }
            string id = body.Substring(0, separator);
            string version = body.Substring(separator).TrimStart();
            if (!Valid(id, false) || !Valid(version, true) || id.Length == 0 ||
                version.IndexOfAny(new[] { ' ', '\t' }) >= 0) return null;
            return new Net10PackageDirectiveContext(version, id, true, closing);
        }

        private static bool Valid(string value, bool version)
        {
            foreach (char ch in value)
                if (!char.IsLetterOrDigit(ch) && ch != '.' && ch != '-' &&
                    (version ? ch != '+' : ch != '_')) return false;
            return true;
        }
    }
}
