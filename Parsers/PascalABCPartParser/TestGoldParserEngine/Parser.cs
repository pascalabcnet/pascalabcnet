#region Copyright

//----------------------------------------------------------------------
// Gold Parser engine.
// See more details on http://www.devincook.com/goldparser/
// 
// Original code is written in VB by Devin Cook (GOLDParser@DevinCook.com)
//
// This translation is done by Vladimir Morozov (vmoroz@hotmail.com)
// 
// The translation is based on the other engine translations:
// Delphi engine by Alexandre Rai (riccio@gmx.at)
// C# engine by Marcus Klimstra (klimstra@home.nl)
//----------------------------------------------------------------------

#endregion

#region Using directives

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.PascalABCParser;

#endregion


namespace PascalABCCompiler.TestGoldParserEngine
{
    /// <summary>
    /// Pull parser which uses Grammar table to parser input stream.
    /// </summary>
    public sealed class Parser
    {
        #region Fields

        public Dictionary<string, char> SepecialSymbolPrefixDirection = new Dictionary<string, char>();
        //public char InsertSepecialSymbol = '\0';

        private Grammar m_grammar;               // Grammar of parsed language.
        private bool m_trimReductions = true; // Allowes to minimize reduction tree.

        private TextReader m_textReader;       // Data to parse.
        private char[] m_buffer;           // Buffer to keep current characters.
        private int m_bufferSize;       // Size of the buffer.
        private int m_bufferStartIndex; // Absolute position of buffered first character. 
        private int m_charIndex;        // Index of character in the buffer.
        private int m_preserveChars;    // Number of characters to preserve when buffer is refilled.
        private int m_lineStart;        // Relative position of line start to the buffer beginning.
        private int m_lineLength;       // Length of current source line.
        private int m_lineNumber = 1;   // Current line number.
        private int m_commentLevel;     // Keeps stack level for embedded comments
        private StringBuilder m_commentText;   // Current comment text.

        private SourceLineReadCallback m_sourceLineReadCallback; // Called when line reading finished. 

        private Token m_token;            // Current token
        private Token[] m_inputTokens;      // Stack of input tokens.
        private int m_inputTokenCount;  // How many tokens in the input.

        private LRStackItem[] m_lrStack;        // Stack of LR states used for LR parsing.
        private int m_lrStackIndex;   // Index of current LR state in the LR parsing stack. 
        private LRState m_lrState;        // Current LR state.
        private int m_reductionCount; // Number of items in reduction. It is Undefined if no reducton available. 
        private Symbol[] m_expectedTokens; // What tokens are expected in case of error?  
        private Symbol[] m_expectedNonTerminalTokens; // What tokens are expected in case of error?  

        private const int MinimumBufferSize = 4096;   // Minimum size of char buffer.
        private const char EndOfString = (char)0;     // Designates last string terminator.
        private const int MinimumInputTokenCount = 2; // Minimum input token stack size.
        private const int MinimumLRStackSize = 256;   // Minimum size of reduction stack.
        private const int Undefined = -1;             // Used for undefined int values. 

        #endregion
        // nige vse polya moi

        public bool PartModeON;
        public ParserConfig PartConfig;

        public bool eof_was_flag = false;
        public bool begin_flag = false;
        public int stack_count = 0;
        public int point_flag = 0;
        public bool pr_block_flag = false;
        public int sem_flag = 0;
        public int last_token = -1;
        public bool class_flag = false;
        public bool record_flag = false;
        public bool RO_flag = false;
        public bool RO_restore_was = false;
        public bool begin_restore = false;
        public bool RO_insert = false;
        public bool var_rule = false;
        public bool point_list_flag = false;
        public bool begin_must = false;

        public int count_end = 0;
        public bool restore_was = false;
        
        public Stack<int> StackUnclosedTokens = new Stack<int>();

        public Symbol error_token = null;

        #region PS: Parser state saving

        public struct ParserState
        {
            public int saved_lrStackIndex;
            public int saved_reductionCount;
            public int[] saved_StackUnclosedTokens;
            public int saved_stack_count;

            public bool saved_begin_flag;
            public int saved_point_flag;
            public bool saved_pr_block_flag;
            public int saved_sem_flag;
        }

        public Stack<ParserState> ps = new Stack<ParserState>();
        public bool stack_saved = false;
        public bool restore_flag = false;

        
        public void SaveParserState()
        {
            ParserState p = new ParserState();

            p = new ParserState();
            p.saved_lrStackIndex = m_lrStackIndex;
            p.saved_reductionCount = m_reductionCount;

            p.saved_stack_count = stack_count;
            p.saved_StackUnclosedTokens=new int[StackUnclosedTokens.Count];
            StackUnclosedTokens.CopyTo(p.saved_StackUnclosedTokens, 0);

            p.saved_point_flag = point_flag;
            p.saved_pr_block_flag=pr_block_flag;
            p.saved_sem_flag=sem_flag;

            ps.Push(p);

            count_end = 0;
            stack_saved = true;
        }

        public void RestoreParserState()
        {
            ParserState p=new ParserState();
            if (ps.Count==0)
                return;
            p = ps.Peek();
            m_lrStackIndex = p.saved_lrStackIndex;
            m_reductionCount = p.saved_reductionCount;
            m_lrState = m_lrStack[p.saved_lrStackIndex].m_state;
            m_token = new Token();
            m_token.m_symbol = new Symbol((int)GPBParser_PascalABC.SymbolConstants.SYMBOL_WHITESPACE, "Whitespace", SymbolType.WhiteSpace);
            stack_count = p.saved_stack_count;
            StackUnclosedTokens.Clear();
            for (int i = p.saved_StackUnclosedTokens.Length-1; i >=0; i--)
                StackUnclosedTokens.Push(p.saved_StackUnclosedTokens[i]);

            point_flag = p.saved_point_flag;
            pr_block_flag = p.saved_pr_block_flag;
            sem_flag = p.saved_sem_flag;

            stack_saved = false;
            restore_flag = true;
            restore_was = true;
            RO_restore_was = true;
            begin_restore = true;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes new instance of Parser class.
        /// </summary>
        /// <param name="textReader">TextReader instance to read data from.</param>
        /// <param name="grammar">Grammar with parsing tables to parser input stream.</param>
        public Parser(TextReader textReader, Grammar grammar)
        {
            if (textReader == null)
            {
                throw new ArgumentNullException("textReader");
            }
            if (grammar == null)
            {
                throw new ArgumentNullException("grammar");
            }

            m_textReader = textReader;
            m_bufferSize = MinimumBufferSize;
            m_buffer = new char[m_bufferSize + 1];
            m_lineLength = Undefined;
            ReadBuffer();

            m_inputTokens = new Token[MinimumInputTokenCount];
            m_lrStack = new LRStackItem[MinimumLRStackSize];

            m_grammar = grammar;

            // Put grammar start symbol into LR parsing stack.
            m_lrState = m_grammar.InitialLRState;
            LRStackItem start = new LRStackItem();
            start.m_token.m_symbol = m_grammar.StartSymbol;
            start.m_state = m_lrState;
            m_lrStack[m_lrStackIndex] = start;

            m_reductionCount = Undefined; // there are no reductions yet.
        }

        #endregion

        #region Parser general properties

        /// <summary>
        /// Gets the parser's grammar.
        /// </summary>
        public Grammar Grammar
        {
            get { return m_grammar; }
        }

        /// <summary>
        /// Gets or sets flag to trim reductions.
        /// </summary>
        public bool TrimReductions
        {
            get { return m_trimReductions; }
            set { m_trimReductions = value; }
        }

        #endregion

        #region Data Source properties and methods

        /// <summary>
        /// Gets source of parsed data.
        /// </summary>
        public TextReader TextReader
        {
            get { return m_textReader; }
        }

        /// <summary>
        /// Gets current char position.
        /// </summary>
        public int CharPosition
        {
            get { return m_charIndex + m_bufferStartIndex; }
        }

        /// <summary>
        /// Gets current line number. It is 1-based.
        /// </summary>
        public int LineNumber
        {
            get { return m_lineNumber; }
        }

        /// <summary>
        /// Gets current char position in the current source line. It is 1-based.
        /// </summary>
        public int LinePosition
        {
            get { return CharPosition - m_lineStart + 1; }
        }

        /// <summary>
        /// Gets current source line text. It can be truncated if line is longer than 2048 characters.
        /// </summary>
        public string LineText
        {
            get
            {
                int lineStart = Math.Max(m_lineStart, 0);
                int lineLength;
                if (m_lineLength == Undefined)
                {
                    // Line was requested outside of SourceLineReadCallback call
                    lineLength = m_charIndex - lineStart;
                }
                else
                {
                    lineLength = m_lineLength - (lineStart - m_lineStart);
                }
                if (lineLength > 0)
                {
                    return new String(m_buffer, lineStart, lineLength);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets callback function to track source line text.
        /// </summary>
        public SourceLineReadCallback SourceLineReadCallback
        {
            get { return m_sourceLineReadCallback; }
            set { m_sourceLineReadCallback = value; }
        }

        /// <summary>
        /// Reads next characters to the buffer.
        /// </summary>
        /// <returns>Number of characters read.</returns>
        private int ReadBuffer()
        {
            // Find out how many bytes to preserve.
            // We truncate long lines.
            int lineStart = (m_lineStart < 0) ? 0 : m_lineStart;
            int lineCharCount = m_charIndex - lineStart;
            if (lineCharCount > m_bufferSize / 2)
            {
                lineCharCount = m_bufferSize / 2;
            }
            int moveIndex = m_charIndex - lineCharCount;
            int moveCount = lineCharCount + m_preserveChars;
            if (moveCount > 0)
            {
                // We need to keep current token characters.
                if (m_bufferSize - moveCount < 20)
                {
                    // Grow the buffer
                    m_bufferSize = m_bufferSize * 2;
                    char[] newBuffer = new char[m_bufferSize + 1];
                    Array.Copy(m_buffer, moveIndex, newBuffer, 0, moveCount);
                    m_buffer = newBuffer;
                }
                else
                {
                    Array.Copy(m_buffer, moveIndex, m_buffer, 0, moveCount);
                }
            }

            // Read as many characters as possible.
            int count = m_bufferSize - moveCount;
            int result = m_textReader.Read(m_buffer, moveCount, count);
            // Mark character after last read one as End-Of-String
            m_buffer[moveCount + result] = EndOfString;
            // Adjust buffer variables.
            m_bufferStartIndex += moveIndex;
            m_charIndex -= moveIndex;
            m_lineStart -= moveIndex;
            return result;
        }

        /// <summary>
        /// Increments current char index by delta character positions.
        /// </summary>
        /// <param name="delta">Number to increment char index.</param>
        private void MoveBy(int delta)
        {
            for (int i = delta; --i >= 0; )
            {
                if (m_buffer[m_charIndex++] == '\n')
                {
                    if (m_sourceLineReadCallback != null)
                    {
                        m_lineLength = m_charIndex - m_lineStart - 1; // Exclude '\n'
                        int lastIndex = m_lineStart + m_lineLength - 1;
                        if (lastIndex >= 0 && m_buffer[lastIndex] == '\r')
                        {
                            m_lineLength--;
                        }
                        if (m_lineLength < 0)
                        {
                            m_lineLength = 0;
                        }
                        m_sourceLineReadCallback(this, m_lineStart + m_bufferStartIndex, m_lineLength);
                    }
                    m_lineNumber++;
                    m_lineStart = m_charIndex;
                    m_lineLength = Undefined;
                }
                if (m_buffer[m_charIndex] == '\0')
                {
                    if (m_sourceLineReadCallback != null)
                    {
                        m_lineLength = m_charIndex - m_lineStart;
                        if (m_lineLength > 0)
                        {
                            m_sourceLineReadCallback(this, m_lineStart + m_bufferStartIndex, m_lineLength);
                        }
                        m_lineLength = Undefined;
                    }
                }
            }
        }

        /// <summary>
        /// Moves current char pointer to the end of source line.
        /// </summary>
        private void MoveToLineEnd()
        {
            while (true)
            {
                char ch = m_buffer[m_charIndex];
                switch (ch)
                {
                    case '\r':
                    case '\n':
                        return;

                    case EndOfString:
                        if (ReadBuffer() == 0)
                        {
                            return;
                        }
                        break;

                    default:
                        if (m_commentText != null)
                        {
                            m_commentText.Append(ch);
                        }
                        break;
                }
                m_charIndex++;
            }
        }

        #endregion

        #region Tokenizer properties and methods

        /// <summary>
        /// Gets or sets current token symbol.
        /// </summary>
        public Symbol TokenSymbol
        {
            get { return m_token.m_symbol; }
            set { m_token.m_symbol = value; }
        }

        /// <summary>
        /// Gets or sets current token text.
        /// </summary>
        public string TokenText
        {
            get
            {
                if (m_token.m_text == null)
                {
                    if (m_token.m_length > 0)
                    {
                        m_token.m_text = new String(m_buffer, m_token.m_start - m_bufferStartIndex, m_token.m_length);
                    }
                    else
                    {
                        m_token.m_text = string.Empty;
                    }
                }
                return m_token.m_text;
            }
            set { m_token.m_text = value; }
        }

        /// <summary>
        /// Gets or sets current token position relative to input stream beginning.
        /// </summary>
        public int TokenCharPosition
        {
            get { return m_token.m_start; }
            set { m_token.m_start = value; }
        }

        /// <summary>
        /// Gets or sets current token text length.
        /// </summary>
        public int TokenLength
        {
            get { return m_token.m_length; }
            set { m_token.m_length = value; }
        }

        /// <summary>
        /// Gets or sets current token line number. It is 1-based.
        /// </summary>
        public int TokenLineNumber
        {
            get { return m_token.m_lineNumber; }
            set { m_token.m_lineNumber = value; }
        }

        /// <summary>
        /// Gets or sets current token position in current source line. It is 1-based.
        /// </summary>
        public int TokenLinePosition
        {
            get { return m_token.m_linePosition; }
            set { m_token.m_linePosition = value; }
        }

        /// <summary>
        /// Gets or sets token syntax object associated with the current token or reduction.
        /// </summary>
        public object TokenSyntaxNode
        {
            get
            {
                if (m_reductionCount == Undefined)
                {
                    return m_token.m_syntaxNode;
                }
                else
                {
                    return m_lrStack[m_lrStackIndex].m_token.m_syntaxNode;
                }
            }
            set
            {
                if (m_reductionCount == Undefined)
                {
                    m_token.m_syntaxNode = value;
                }
                else
                {
                    m_lrStack[m_lrStackIndex].m_token.m_syntaxNode = value;
                }
            }
        }

        /// <summary>
        /// Returns string representation of the token.
        /// </summary>
        /// <returns>String representation of the token.</returns>
        public string TokenString
        {
            get
            {
                if (m_token.m_symbol.m_symbolType != SymbolType.Terminal)
                {
                    return m_token.m_symbol.ToString();
                }
                StringBuilder sb = new StringBuilder(m_token.m_length);
                for (int i = 0; i < m_token.m_length; i++)
                {
                    char ch = m_buffer[m_token.m_start - m_bufferStartIndex + i];
                    if (ch < ' ')
                    {
                        switch (ch)
                        {
                            case '\n':
                                sb.Append("{LF}");
                                break;
                            case '\r':
                                sb.Append("{CR}");
                                break;
                            case '\t':
                                sb.Append("{HT}");
                                break;
                        }
                    }
                    else
                    {
                        sb.Append(ch);
                    }
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Pushes a token to the input token stack.
        /// </summary>
        /// <param name="symbol">Token symbol.</param>
        /// <param name="text">Token text.</param>
        /// <param name="syntaxNode">Syntax node associated with the token.</param>
        public void PushInputToken(Symbol symbol, string text, object syntaxNode)
        {
            if (m_token.m_symbol != null)
            {
                if (m_inputTokenCount == m_inputTokens.Length)
                {
                    Token[] newTokenArray = new Token[m_inputTokenCount * 2];
                    Array.Copy(m_inputTokens, newTokenArray, m_inputTokenCount);
                    m_inputTokens = newTokenArray;
                }
                m_inputTokens[m_inputTokenCount++] = m_token;
            }
            m_token = new Token();
            m_token.m_symbol = symbol;
            m_token.m_text = text;
            m_token.m_length = (text != null) ? text.Length : 0;
            m_token.m_syntaxNode = syntaxNode;
        }

        /// <summary>
        /// Pops token from the input token stack.
        /// </summary>
        /// <returns>Token symbol from the top of input token stack.</returns>
        public Symbol PopInputToken()
        {
            Symbol result = m_token.m_symbol;
            if (m_inputTokenCount > 0)
            {
                m_token = m_inputTokens[--m_inputTokenCount];
            }
            else
            {
                m_token.m_symbol = null;
                m_token.m_text = null;
            }
            return result;
        }

        char SpecialSymbol = '\0';
        bool NotExecuteSpecialSymbol = false;

        /// <summary>
        /// Reads next token from the input stream.
        /// </summary>
        /// <returns>Token symbol which was read.</returns>
        public Symbol ReadToken()
        {
            m_token.m_text = null;
            m_token.m_start = m_charIndex + m_bufferStartIndex;
            m_token.m_lineNumber = m_lineNumber;
            m_token.m_linePosition = m_charIndex /*+ m_bufferStartIndex*/ - m_lineStart + 1;
            int lookahead = m_charIndex;  // Next look ahead char in the input
            int tokenLength = 0;
            Symbol tokenSymbol = null;
            DfaState[] dfaStateTable = m_grammar.m_dfaStateTable;

            char ch = m_buffer[lookahead];
            if (ch == EndOfString)
            {
                if (ReadBuffer() == 0)
                {
                    m_token.m_symbol = m_grammar.m_endSymbol;
                    m_token.m_length = 0;
                    return m_token.m_symbol;
                }
                lookahead = m_charIndex;
                ch = m_buffer[lookahead];
            }
            if (SpecialSymbol != '\0')
            {
                ch = SpecialSymbol;
                SpecialSymbol = '\0';
                lookahead--;
                NotExecuteSpecialSymbol = true;
            }
            DfaState dfaState = m_grammar.m_dfaInitialState;
            while (true)
            {
                dfaState = dfaState.m_transitionVector[ch] as DfaState;

                // This block-if statement checks whether an edge was found from the current state.
                // If so, the state and current position advance. Otherwise it is time to exit the main loop
                // and report the token found (if there was it fact one). If the LastAcceptState is -1,
                // then we never found a match and the Error Token is created. Otherwise, a new token
                // is created using the Symbol in the Accept State and all the characters that
                // comprise it.
                if (dfaState != null)
                {
                    // This code checks whether the target state accepts a token. If so, it sets the
                    // appropiate variables so when the algorithm in done, it can return the proper
                    // token and number of characters.
                    lookahead++;
                    if (dfaState.m_acceptSymbol != null)
                    {
                        tokenSymbol = dfaState.m_acceptSymbol;
                        tokenLength = lookahead - m_charIndex;
                    }
                    ch = m_buffer[lookahead];
                    if (ch == EndOfString)
                    {
                        m_preserveChars = lookahead - m_charIndex;
                        if (ReadBuffer() == 0)
                        {
                            // Found end of of stream
                            lookahead = m_charIndex + m_preserveChars;
                        }
                        else
                        {
                            lookahead = m_charIndex + m_preserveChars;
                            ch = m_buffer[lookahead];
                        }
                        m_preserveChars = 0;
                    }
                }
                else
                {
                    if (tokenSymbol != null)
                    {
                        m_token.m_symbol = tokenSymbol;
                        m_token.m_length = tokenLength;
                        if ((SepecialSymbolPrefixDirection.Count > 0 && SepecialSymbolPrefixDirection.ContainsKey(TokenText)) /*|| InsertSepecialSymbol!='\0'*/)
                        {
                            if (!NotExecuteSpecialSymbol)
                            {
                                NotExecuteSpecialSymbol = false;
                                /*if (InsertSepecialSymbol != '\0')
                                    SpecialSymbol = InsertSepecialSymbol;
                                else*/
                                SpecialSymbol = SepecialSymbolPrefixDirection[TokenText];
                                return ReadToken();
                            }
                            NotExecuteSpecialSymbol = false;
                        }
                        MoveBy(tokenLength);
                    }
                    else
                    {
                        //Tokenizer cannot recognize symbol
                        m_token.m_symbol = m_grammar.m_errorSymbol;
                        m_token.m_length = 1;
                        MoveBy(1);
                    }
                    break;
                }
            }
            return m_token.m_symbol;
        }

        /// <summary>
        /// Removes current token and pops next token from the input stack.
        /// </summary>
        private void DiscardInputToken()
        {
            if (m_inputTokenCount > 0)
            {
                m_token = m_inputTokens[--m_inputTokenCount];
            }
            else
            {
                m_token.m_symbol = null;
                m_token.m_text = null;
            }
        }

        #endregion

        #region LR parser properties and methods

        /// <summary>
        /// Gets current LR state.
        /// </summary>
        public LRState CurrentLRState
        {
            get { return m_lrState; }
        }

        /// <summary>
        /// Gets current reduction syntax rule.
        /// </summary>
        public Rule ReductionRule
        {
            get { return m_lrStack[m_lrStackIndex].m_rule; }
        }

        /// <summary>
        /// Gets number of items in the current reduction
        /// </summary>
        public int ReductionCount
        {
            get { return m_reductionCount; }
        }

        /// <summary>
        /// Gets reduction item syntax object by its index.
        /// </summary>
        /// <param name="index">Index of reduction item.</param>
        /// <returns>Syntax object attached to reduction item.</returns>
        public object GetReductionSyntaxNode(int index)
        {
            //DarkStar Changed for Speed!
            /*if (index < 0 || index >= m_reductionCount)
            {
                throw new IndexOutOfRangeException();
            }*/
            return m_lrStack[m_lrStackIndex - m_reductionCount + index].m_token.m_syntaxNode;
        }

        /// <summary>
        /// Gets array of expected token symbols.
        /// </summary>
        public Symbol[] GetExpectedTokens()
        {
            return m_expectedTokens;
        }

        public Symbol[] GetExpectedNonTerminalTokens()
        {
            return m_expectedNonTerminalTokens;
        }

        public string _SeeToken()
        {
            int lookahead = m_charIndex;  // Next look ahead char in the input
            char ch = m_buffer[lookahead];

            if (ch == '.')
                return "tkPoint";
            if (ch == EndOfString)
                return "EOF";
            if (ch == ';')
                return "tkSemiColon";
            return "other";
        }

        /// <summary>
        /// Executes next step of parser and returns parser state.
        /// </summary>
        /// <returns>Parser current state.</returns>
        public ParseMessage Parse()
        {
            if (m_token.m_symbol != null)
            {
                switch (m_token.m_symbol.m_symbolType)//for comments
                {
                    case SymbolType.CommentLine:
                        DiscardInputToken(); //Remove it 
                        MoveToLineEnd();
                        break;
                    case SymbolType.CommentStart:
                        ProcessBlockComment();
                        break;
                }
            }
            while (true)
            {
                if (m_token.m_symbol == null)
                {
                    //We must read a token

                    if (var_rule)
                    {
                        SaveParserState();
                        var_rule = false;
                    }
                    if (begin_flag == true && !eof_was_flag)
                    {
                        SaveParserState();
                        begin_flag = false;
                    }
                    if (class_flag == true && !eof_was_flag)
                    {
                        SaveParserState();
                        class_flag = false;
                    }
                    if (record_flag == true && !eof_was_flag)
                    {
                        SaveParserState();
                        record_flag = false;
                    }
                    if (RO_flag == true && !eof_was_flag)
                    {
                        SaveParserState();
                        RO_flag = false;
                    }
                    
                    Symbol readTokenSymbol = null;

                    if (error_token != null && error_token.m_name == "tkSemiColon")
                    {
                        sem_flag = 1;
                        error_token = null;
                    }
                    
                    if (sem_flag == 1)
                    {
                        m_token.m_symbol=new Symbol(PartConfig.pas_tokens[14],"tkSemiColon",SymbolType.Terminal);
                        m_token.m_syntaxNode = new object();
                        sem_flag = 0;
                    }
                    else
                        if (point_flag == 1 && stack_count == 0)
                        {
                            m_token.m_symbol=new Symbol(PartConfig.pas_tokens[12],"tkPoint",SymbolType.Terminal);
                            m_token.m_syntaxNode = new object();
                            point_flag = 0;
                        }
                        else
                            if (restore_was && count_end > 0 && StackUnclosedTokens.Count>0 
                                && StackUnclosedTokens.Peek()==PartConfig.pas_tokens[1])
                            {
                                m_token.m_symbol = new Symbol(PartConfig.pas_tokens[2],
                                    "tkEnd", SymbolType.Terminal);
                                m_token.m_syntaxNode = new object();
                                count_end--;
                            }    
                            else
                                if (RO_restore_was && StackUnclosedTokens.Count>0 &&
                                    StackUnclosedTokens.Peek() == PartConfig.pas_tokens[3])
                                {
                                    m_token.m_symbol = new Symbol(PartConfig.pas_tokens[4],
                                    "tkRoundClose", SymbolType.Terminal);
                                    m_token.m_syntaxNode = new object();
                                    if (last_token == PartConfig.pas_tokens[1])
                                    {
                                        sem_flag = 1;
                                        RO_insert = true;
                                    }
                                }
                                else
                                    if (begin_restore && RO_insert && sem_flag==0 && !RO_restore_was && (last_token==PartConfig.pas_tokens[1] /*|| count_begin==1*/))
                                    {
                                        m_token.m_symbol = new Symbol(PartConfig.pas_tokens[1],
                                    "tkBegin", SymbolType.Terminal);
                                        m_token.m_syntaxNode = new object();
                                        begin_restore = false;
                                        RO_insert = false;
                                    }
                                    else
                                        if (begin_must)
                                        {
                                            m_token.m_symbol = new Symbol(PartConfig.pas_tokens[1],
                                        "tkBegin", SymbolType.Terminal);
                                            m_token.m_syntaxNode = new object();
                                            begin_must = false;
                                        }
                                        /*else
                                            if (error_token!=null && error_token.m_name=="tkSemicolon")
                                            {
                                                m_token.m_symbol = new Symbol(PartConfig.pas_tokens[14], "tkSemiColon", SymbolType.Terminal);
                                                m_token.m_syntaxNode = new object();
                                                error_token = null;
                                            }*/
                                        else
                                        {
                                            if (point_list_flag && last_token == PartConfig.pas_tokens[14])
                                                point_list_flag = false;
                                            
                                            readTokenSymbol = ReadToken();

                                            if (eof_was_flag)
                                            {
                                                m_token.m_symbol.m_name = "EOF";
                                                m_token.m_symbol.m_symbolType = SymbolType.End;
                                                m_token.m_symbol.m_index = PartConfig.pas_tokens[0];
                                                m_token.m_syntaxNode = new object();
                                            }

                                            if (point_list_flag && m_token.m_symbol.m_index != PartConfig.pas_tokens[1] && m_token.m_symbol.m_symbolType != SymbolType.WhiteSpace)
                                                point_list_flag = false;
                                            else
                                                if (m_token.m_symbol.m_index == PartConfig.pas_tokens[1] && point_list_flag)
                                                {
                                                    begin_must = true;
                                                    point_list_flag = false;
                                                    sem_flag = 1;
                                                }

                                            if (m_token.m_symbol.m_index == PartConfig.pas_tokens[1])
                                                begin_flag = true;
                                            if (m_token.m_symbol.m_index == PartConfig.pas_tokens[18])
                                                class_flag = true;
                                            if (m_token.m_symbol.m_index == PartConfig.pas_tokens[19])
                                                record_flag = true;
                                            if (m_token.m_symbol.m_index == PartConfig.pas_tokens[3])
                                                RO_flag = true;

                                            last_token = m_token.m_symbol.m_index;
                                        }
                    if (count_end == 0)
                        restore_was = false;
                    
                    switch (m_token.m_symbol.m_index)
                    {
                        case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKROUNDOPEN:
                            {
                                StackUnclosedTokens.Push(PartConfig.pas_tokens[3]);
                                break;
                            }//(
                        case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKPROCEDURE:
                            {
                                StackUnclosedTokens.Push(PartConfig.pas_tokens[17]);
                                break;
                            }
                        case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKFUNCTION:
                            {
                                StackUnclosedTokens.Push(PartConfig.pas_tokens[20]);
                                break;
                            }
                        case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKBEGIN: 
                        { 
                            StackUnclosedTokens.Push(PartConfig.pas_tokens[1]);  
                            break; 
                        }// Begin
                        case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKEND: 
                        {
                            while (StackUnclosedTokens.Count > 0 &&
                                (StackUnclosedTokens.Peek() == PartConfig.pas_tokens[17] || StackUnclosedTokens.Peek() == PartConfig.pas_tokens[20]))
                                StackUnclosedTokens.Pop();//10.05.08

                            if (StackUnclosedTokens.Count > 0 &&
                                (StackUnclosedTokens.Peek() == PartConfig.pas_tokens[1] || StackUnclosedTokens.Peek() == PartConfig.pas_tokens[16] ||
                                 StackUnclosedTokens.Peek() == PartConfig.pas_tokens[18] || StackUnclosedTokens.Peek() == PartConfig.pas_tokens[19]))
                                StackUnclosedTokens.Pop();

                            if (StackUnclosedTokens.Count > 0 &&
                                (StackUnclosedTokens.Peek() == PartConfig.pas_tokens[17] || StackUnclosedTokens.Peek() == PartConfig.pas_tokens[20]))
                                StackUnclosedTokens.Pop();
                            if (!eof_was_flag && !restore_was)//dlya vstavki end
                                count_end++;
                            break; 
                        }// End
                        case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKROUNDCLOSE:
                        {
                            if (StackUnclosedTokens.Count > 0 && StackUnclosedTokens.Peek() == PartConfig.pas_tokens[3])
                                StackUnclosedTokens.Pop();
                            break;
                        }
                        case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKCASE:
                        {
                            StackUnclosedTokens.Push(PartConfig.pas_tokens[16]);
                            break;
                        }// Case
                        case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKCLASS:
                        {
                            StackUnclosedTokens.Push(PartConfig.pas_tokens[18]);
                            break;
                        }
                        case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKRECORD:
                        {
                            StackUnclosedTokens.Push(PartConfig.pas_tokens[19]);
                            break;
                        }
                        case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_EOF: 
                        {
                            stack_count = StackUnclosedTokens.Count;
                            if (!pr_block_flag && StackUnclosedTokens.Count==0 && restore_flag)
                            {
                                m_token.m_symbol.m_name = "tkBegin";
                                m_token.m_symbol.m_symbolType = SymbolType.Terminal;
                                m_token.m_symbol.m_index = PartConfig.pas_tokens[1];
                                m_token.m_syntaxNode = new object();
                                StackUnclosedTokens.Push(PartConfig.pas_tokens[1]);
                                sem_flag = 1;
                            }
                            eof_was_flag = true;
                            break; 
                        }//EOF
                    }


                    if (stack_count > 0 && restore_flag && sem_flag == 0)
                    {
                        int num = StackUnclosedTokens.Peek();
                        switch (num)
                        {
                            case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKBEGIN:
                                {
                                    m_token.m_symbol.m_name = "tkEnd";
                                    m_token.m_symbol.m_symbolType = SymbolType.Terminal;
                                    m_token.m_symbol.m_index = PartConfig.pas_tokens[2];
                                    m_token.m_syntaxNode = new object();

                                    if (StackUnclosedTokens.Count == 1)
                                        point_flag = 1;
                                    else
                                        sem_flag = 1;

                                    if (StackUnclosedTokens.Count > 0)
                                    {
                                        StackUnclosedTokens.Pop();
                                        stack_count--;
                                    }
                                    if (StackUnclosedTokens.Count > 0 && 
                                        (StackUnclosedTokens.Peek() == PartConfig.pas_tokens[17] || StackUnclosedTokens.Peek() == PartConfig.pas_tokens[20]))
                                    {
                                        StackUnclosedTokens.Pop();
                                        stack_count--;
                                        //sem_flag = 1;
                                    }
                                    break;
                                }
                            case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKROUNDOPEN:
                                {
                                    m_token.m_symbol.m_name = "tkRoundClose";
                                    m_token.m_symbol.m_symbolType = SymbolType.Terminal;
                                    m_token.m_symbol.m_index = PartConfig.pas_tokens[4];
                                    m_token.m_syntaxNode = new object();
                                    if (StackUnclosedTokens.Count > 0)
                                    {
                                        StackUnclosedTokens.Pop();
                                        stack_count--;
                                    }
                                    sem_flag = 1;
                                    break;
                                }

                            case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKRECORD:
                                {
                                    m_token.m_symbol.m_name = "tkEnd";
                                    m_token.m_symbol.m_symbolType = SymbolType.Terminal;
                                    m_token.m_symbol.m_index = PartConfig.pas_tokens[2];
                                    m_token.m_syntaxNode = new object();
                                    sem_flag = 1;

                                    if (StackUnclosedTokens.Count > 0)
                                    {
                                        StackUnclosedTokens.Pop();
                                        stack_count--;
                                    }
                                    break;
                                }
                            case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKCLASS:
                                {
                                    m_token.m_symbol.m_name = "tkEnd";
                                    m_token.m_symbol.m_symbolType = SymbolType.Terminal;
                                    m_token.m_symbol.m_index = PartConfig.pas_tokens[2];
                                    m_token.m_syntaxNode = new object();
                                    sem_flag = 1;

                                    if (StackUnclosedTokens.Count > 0)
                                    {
                                        StackUnclosedTokens.Pop();
                                        stack_count--;
                                    }
                                    break;
                                }
                            case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKPROCEDURE://10.05.08
                                {
                                    while (StackUnclosedTokens.Count > 0 &&
                                (StackUnclosedTokens.Peek() == PartConfig.pas_tokens[17] || StackUnclosedTokens.Peek() == PartConfig.pas_tokens[20]))
                                    {
                                        StackUnclosedTokens.Pop();
                                        stack_count--;
                                    }
                                    if (StackUnclosedTokens.Count > 0 &&
                                        (StackUnclosedTokens.Peek() == PartConfig.pas_tokens[18] || StackUnclosedTokens.Peek() == PartConfig.pas_tokens[19]))
                                    {
                                        m_token.m_symbol.m_name = "tkEnd";
                                        m_token.m_symbol.m_symbolType = SymbolType.Terminal;
                                        m_token.m_symbol.m_index = PartConfig.pas_tokens[2];
                                        m_token.m_syntaxNode = new object();
                                        sem_flag = 1;
                                        StackUnclosedTokens.Pop();
                                        stack_count--;
                                    }
                                    break;
                                }
                            case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKFUNCTION:
                                {
                                    while (StackUnclosedTokens.Count > 0 &&
                                (StackUnclosedTokens.Peek() == PartConfig.pas_tokens[17] || StackUnclosedTokens.Peek() == PartConfig.pas_tokens[20]))
                                    {
                                        StackUnclosedTokens.Pop();
                                        stack_count--;
                                    }
                                    if (StackUnclosedTokens.Count > 0 &&
                                        (StackUnclosedTokens.Peek() == PartConfig.pas_tokens[18] || StackUnclosedTokens.Peek() == PartConfig.pas_tokens[19]))
                                    {
                                        m_token.m_symbol.m_name = "tkEnd";
                                        m_token.m_symbol.m_symbolType = SymbolType.Terminal;
                                        m_token.m_symbol.m_index = PartConfig.pas_tokens[2];
                                        m_token.m_syntaxNode = new object();
                                        sem_flag = 1;
                                        StackUnclosedTokens.Pop();
                                        stack_count--;
                                    }
                                    break;
                                }
                        }
                    }
                    if (StackUnclosedTokens.Count > 0 && StackUnclosedTokens.Peek() != PartConfig.pas_tokens[3])
                        RO_restore_was = false;
                    ///////////////////////////
                    SymbolType symbolType = m_token.m_symbol.m_symbolType;
                    if (m_commentLevel == 0
                        && symbolType != SymbolType.CommentLine
                        && symbolType != SymbolType.CommentStart
                        && symbolType != SymbolType.WhiteSpace)
                    {
                        return ParseMessage.TokenRead;//если не коментарии, то сообщение 
                    }
                }
                else
                {
                    //==== Normal parse mode - we have a token and we are not in comment mode
                    switch (m_token.m_symbol.m_symbolType)
                    {
                        case SymbolType.WhiteSpace:
                            DiscardInputToken();  // Discard Whitespace
                            break;

                        case SymbolType.CommentStart:
                            m_commentLevel = 1; // Switch to block comment mode.
                            return ParseMessage.CommentBlockRead;

                        case SymbolType.CommentLine:
                            return ParseMessage.CommentLineRead;

                        case SymbolType.Error:
                            return ParseMessage.LexicalError;

                        default:
                            //Finally, we can parse the token
                            TokenParseResult parseResult = ParseToken();
                            switch (parseResult)
                            {
                                case TokenParseResult.Accept:
                                    return ParseMessage.Accept;

                                case TokenParseResult.InternalError:
                                    return ParseMessage.InternalError;

                                case TokenParseResult.ReduceNormal:
                                    if (!m_lrStack[m_lrStackIndex].m_rule.m_hasEmpty)
                                        return ParseMessage.Reduction;
                                    break;

                                case TokenParseResult.Shift:
                                    //A simple shift, we must continue
                                    DiscardInputToken(); // Okay, remove the top token, it is on the stack
                                    break;

                                case TokenParseResult.SyntaxError:
                                    {
                                        ParserState p=new ParserState();
                                        if ((!stack_saved) && ps.Count>1)
                                            p=ps.Pop();
                                        return ParseMessage.SyntaxError;
                                    }

                                default:
                                    //Do nothing
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        private void ProcessBlockComment()
        {
            if (m_commentLevel > 0)
            {
                if (m_commentText != null)
                {
                    m_commentText.Append(TokenText);
                }
                DiscardInputToken();
                while (true)
                {
                    SymbolType symbolType = ReadToken().SymbolType;
                    if (m_commentText != null)
                    {
                        m_commentText.Append(TokenText);
                    }
                    DiscardInputToken();
                    switch (symbolType)
                    {
                        case SymbolType.CommentStart:
                            m_commentLevel++;
                            break;

                        case SymbolType.CommentEnd:
                            m_commentLevel--;
                            if (m_commentLevel == 0)
                            {
                                // Done with comment.
                                return;
                            }
                            break;

                        case SymbolType.End:
                            //TODO: replace with special exception.
                            throw new Exception("CommentError");

                        default:
                            //Do nothing, ignore
                            //The 'comment line' symbol is ignored as well
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets current comment text.
        /// </summary>
        public string CommentText
        {
            get
            {
                if (m_token.m_symbol != null)
                {
                    switch (m_token.m_symbol.m_symbolType)
                    {
                        case SymbolType.CommentLine:
                            m_commentText = new StringBuilder();
                            m_commentText.Append(TokenText);
                            DiscardInputToken(); //Remove token 
                            MoveToLineEnd();
                            string lineComment = m_commentText.ToString();
                            m_commentText = null;
                            return lineComment;

                        case SymbolType.CommentStart:
                            m_commentText = new StringBuilder();
                            ProcessBlockComment();
                            string blockComment = m_commentText.ToString();
                            m_commentText = null;
                            return blockComment;
                    }
                }
                return String.Empty;
            }
        }

        private void RsesizeLRStackIfNeed(int index)
        {
            if (index > m_lrStack.Length - 1)
            {
                LRStackItem[] tmp = new LRStackItem[m_lrStack.Length * 2];
                m_lrStack.CopyTo(tmp, 0);
                m_lrStack = tmp;
            }
        }

        private TokenParseResult ParseToken()
        {
            LRStateAction stateAction = m_lrState.m_transitionVector[m_token.m_symbol.m_index];
            if (stateAction != null)
            {
                //Work - shift or reduce
                if (m_reductionCount > 0)
                {
                    int newIndex = m_lrStackIndex - m_reductionCount;
                    m_lrStack[newIndex] = m_lrStack[m_lrStackIndex];
                    m_lrStackIndex = newIndex;
                }
                m_reductionCount = Undefined;

                //
                if (/*last_token!=(int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKEND &&*/ !eof_was_flag && ReductionRule != null && ReductionRule.Index == (int)GPBParser_PascalABC.RuleConstants.RULE_STMT_LIST_TKSEMICOLON)
                    SaveParserState();
                if (/*last_token!=(int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKEND &&*/ !eof_was_flag && ReductionRule != null && ReductionRule.Index == (int)GPBParser_PascalABC.RuleConstants.RULE_STMT_LIST)
                    SaveParserState();
                if (ReductionRule != null && ReductionRule.NonTerminal.m_name == "fp_list")
                    SaveParserState();
                if (ReductionRule != null && ReductionRule.NonTerminal.m_name == "var_decl_sect")
                    SaveParserState();
                if (ReductionRule != null && ReductionRule.NonTerminal.m_name == "const_decl_sect")
                    SaveParserState();
                if (ReductionRule != null && ReductionRule.NonTerminal.m_name == "type_decl_sect")
                    SaveParserState();
                if (ReductionRule != null && ReductionRule.NonTerminal.m_name == "label_decl_sect")
                    SaveParserState();
                if (ReductionRule != null && ReductionRule.NonTerminal.m_name == "var_decl_part_normal")
                    var_rule = true;
                if (ReductionRule != null && ReductionRule.NonTerminal.m_name == "main_used_units_list")
                    SaveParserState();
                if (ReductionRule != null && ReductionRule.NonTerminal.m_name == "ident_or_keyword_pointseparator_list")
                    point_list_flag = true;
                /*FileInfo f = new FileInfo("d:\\Check.txt");
                StreamWriter sw = f.CreateText();
                if (ReductionRule!=null)
                    sw.WriteLine(ReductionRule.m_nonTerminal.m_name);
                sw.Close();*/
                
                //
                switch (stateAction.Action)
                {
                    case LRAction.Accept:
                        m_reductionCount = 0;
                        return TokenParseResult.Accept;

                    case LRAction.Shift:
                        m_lrState = m_grammar.m_lrStateTable[stateAction.m_value];
                        LRStackItem nextToken = new LRStackItem();
                        nextToken.m_token = m_token;
                        nextToken.m_state = m_lrState;
                        RsesizeLRStackIfNeed(++m_lrStackIndex);
                        m_lrStack[m_lrStackIndex] = nextToken;
                        return TokenParseResult.Shift;

                    case LRAction.Reduce:
                        //Produce a reduction - remove as many tokens as members in the rule & push a nonterminal token
                        Rule currentRule = m_grammar.m_ruleTable[stateAction.m_value];

                        //
                        if (currentRule.m_nonTerminal.Name == "program_block")
                            pr_block_flag = true;
                        
                        //

                        LRStackItem head;
                        LRState nextState;
                        if (m_trimReductions && currentRule.m_hasOneNonTerminal)
                        {
                            head = m_lrStack[m_lrStackIndex];
                            head.m_token.m_symbol = currentRule.m_nonTerminal;
                            head.m_token.m_text = null;
                            nextState = m_lrStack[m_lrStackIndex - 1].m_state;
                            if (nextState.m_transitionVector[currentRule.m_nonTerminal.m_index] != null)
                                head.m_state = m_lrState = m_grammar.m_lrStateTable[nextState.m_transitionVector[currentRule.m_nonTerminal.m_index].m_value];
                            else
                            {
                                m_expectedTokens = new Symbol[m_lrState.ActionCount];
                                m_expectedNonTerminalTokens = new Symbol[m_lrState.ActionCount];
                                int lengtht1 = 0, lengthnt1 = 0;
                                for (int i = 0; i < m_lrState.ActionCount; i++)
                                {
                                    switch (m_lrState.GetAction(i).Symbol.SymbolType)
                                    {
                                        case SymbolType.NonTerminal:
                                            m_expectedNonTerminalTokens[lengthnt1++] = m_lrState.GetAction(i).Symbol;
                                            m_expectedTokens[lengtht1++] = m_lrState.GetAction(i).Symbol;
                                            break;
                                        case SymbolType.Terminal:
                                        case SymbolType.End:
                                            m_expectedTokens[lengtht1++] = m_lrState.GetAction(i).Symbol;
                                            break;
                                    }
                                }
                                if (lengtht1 < m_expectedTokens.Length)
                                {
                                    Symbol[] newArray = new Symbol[lengtht1];
                                    Array.Copy(m_expectedTokens, newArray, lengtht1);
                                    m_expectedTokens = newArray;
                                }
                                if (lengthnt1 < m_expectedNonTerminalTokens.Length)
                                {
                                    Symbol[] newArray = new Symbol[lengthnt1];
                                    Array.Copy(m_expectedNonTerminalTokens, newArray, lengthnt1);
                                    m_expectedNonTerminalTokens = newArray;
                                }
                                return TokenParseResult.SyntaxError;
                            }
                            m_lrStack[m_lrStackIndex] = head;
                            return TokenParseResult.ReduceEliminated;
                        }
                        else
                        {
                            head = new LRStackItem();
                            head.m_rule = currentRule;
                            head.m_token.m_symbol = currentRule.m_nonTerminal;
                            head.m_token.m_text = null;
                            m_reductionCount = currentRule.m_symbols.Length;
                            nextState = m_lrStack[m_lrStackIndex - m_reductionCount].m_state;
                            if (nextState.m_transitionVector[currentRule.m_nonTerminal.m_index] != null)
                                m_lrState = m_grammar.m_lrStateTable[nextState.m_transitionVector[currentRule.m_nonTerminal.m_index].m_value];
                            else
                            {
                                m_expectedTokens = new Symbol[m_lrState.ActionCount];
                                m_expectedNonTerminalTokens = new Symbol[m_lrState.ActionCount];
                                int lengtht1 = 0, lengthnt1 = 0;
                                for (int i = 0; i < m_lrState.ActionCount; i++)
                                {
                                    switch (m_lrState.GetAction(i).Symbol.SymbolType)
                                    {
                                        case SymbolType.NonTerminal:
                                            m_expectedNonTerminalTokens[lengthnt1++] = m_lrState.GetAction(i).Symbol;
                                            m_expectedTokens[lengtht1++] = m_lrState.GetAction(i).Symbol;
                                            break;
                                        case SymbolType.Terminal:
                                        case SymbolType.End:
                                            m_expectedTokens[lengtht1++] = m_lrState.GetAction(i).Symbol;
                                            break;
                                    }
                                }
                                if (lengtht1 < m_expectedTokens.Length)
                                {
                                    Symbol[] newArray = new Symbol[lengtht1];
                                    Array.Copy(m_expectedTokens, newArray, lengtht1);
                                    m_expectedTokens = newArray;
                                }
                                if (lengthnt1 < m_expectedNonTerminalTokens.Length)
                                {
                                    Symbol[] newArray = new Symbol[lengthnt1];
                                    Array.Copy(m_expectedNonTerminalTokens, newArray, lengthnt1);
                                    m_expectedNonTerminalTokens = newArray;
                                }
                                //err_flag = true;
                                return TokenParseResult.SyntaxError;
                            }
                            head.m_state = m_lrState;
                            RsesizeLRStackIfNeed(++m_lrStackIndex);
                            m_lrStack[m_lrStackIndex] = head;
                            return TokenParseResult.ReduceNormal;
                        }
                }
            }

            //=== Syntax Error! Fill Expected Tokens
            m_expectedTokens = new Symbol[m_lrState.ActionCount];
            m_expectedNonTerminalTokens = new Symbol[m_lrState.ActionCount];
            int lengtht = 0, lengthnt = 0;
            for (int i = 0; i < m_lrState.ActionCount; i++)
            {
                switch (m_lrState.GetAction(i).Symbol.SymbolType)
                {
                    case SymbolType.NonTerminal:
                        m_expectedNonTerminalTokens[lengthnt++] = m_lrState.GetAction(i).Symbol;
                        m_expectedTokens[lengtht++] = m_lrState.GetAction(i).Symbol;
                        break;
                    case SymbolType.Terminal:
                    case SymbolType.End:
                        m_expectedTokens[lengtht++] = m_lrState.GetAction(i).Symbol;
                        break;
                }
            }
            if (lengtht < m_expectedTokens.Length)
            {
                Symbol[] newArray = new Symbol[lengtht];
                Array.Copy(m_expectedTokens, newArray, lengtht);
                m_expectedTokens = newArray;
            }
            if (lengthnt < m_expectedNonTerminalTokens.Length)
            {
                Symbol[] newArray = new Symbol[lengthnt];
                Array.Copy(m_expectedNonTerminalTokens, newArray, lengthnt);
                m_expectedNonTerminalTokens = newArray;
            }
            return TokenParseResult.SyntaxError;
        }

        #endregion

        #region TokenParseResult enumeration

        /// <summary>
        /// Result of parsing token.
        /// </summary>
        private enum TokenParseResult
        {
            Empty = 0,
            Accept = 1,
            Shift = 2,
            ReduceNormal = 3,
            ReduceEliminated = 4,
            SyntaxError = 5,
            InternalError = 6
        }

        #endregion

        #region Token struct

        /// <summary>
        /// Represents data about current token.
        /// </summary>
        private struct Token
        {
            internal Symbol m_symbol;     // Token symbol.
            internal string m_text;       // Token text.
            internal int m_start;         // Token start stream start.
            internal int m_length;        // Token length.
            internal int m_lineNumber;    // Token source line number. (1-based).
            internal int m_linePosition;  // Token position in source line (1-based).
            internal object m_syntaxNode; // Syntax node which can be attached to the token.
        }

        #endregion

        #region LRStackItem struct

        /// <summary>
        /// Represents item in the LR parsing stack.
        /// </summary>
        private struct LRStackItem
        {
            internal Token m_token;   // Token in the LR stack item.
            internal LRState m_state; // LR state associated with the item.
            internal Rule m_rule;     // Reference to a grammar rule if the item contains non-terminal.
        }

        #endregion
    }
}
