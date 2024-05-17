using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.Parsers
{
    public interface IDocParser
    {

        documentation_comment_list BuildTree(string Text);
    }
}
