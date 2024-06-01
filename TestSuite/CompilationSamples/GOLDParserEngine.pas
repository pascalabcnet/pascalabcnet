unit GOLDParserEngine;

{$zerobasedstrings on}

//----------------------------------------------------------------------
// Gold Parser engine for PascalABC.NET, v0.1 
// Project site http://pascalabc.net/
// See more details on http://www.devincook.com/goldparser/
// 
// Original code is written in VB by Devin Cook (GOLDParser@DevinCook.com)
//
// This translation is done by Tkachuk Aleksander (darkstarta@mail.ru)
// 
// The translation is based on the other engine translations:
// C# engine by Vladimir Morozov (vmoroz@hotmail.com)
// Delphi engine by Alexandre Rai (riccio@gmx.at)
// C# engine by Marcus Klimstra (klimstra@home.nl)
//----------------------------------------------------------------------

interface

uses System,
     System.Text,
     System.IO;

const
  //SymbolType enum
  SymbolTypeNonTerminal = 0;
  SymbolTypeTerminal 	= 1;  
  SymbolTypeWhiteSpace 	= 2;
  SymbolTypeEnd 		= 3;
  SymbolTypeCommentStart= 4;
  SymbolTypeCommentEnd 	= 5;
  SymbolTypeCommentLine = 6;
  SymbolTypeError 		= 7;
  //LRAction enum
  LRActionNone 		= 0;
  LRActionShift 	= 1;   
  LRActionReduce 	= 2;      
  LRActionGoto 		= 3;        
  LRActionAccept	= 4;      
  LRActionError 	= 5;       
  //const in ObjectMap
  MAXINDEX 			= 255;
  GROWTH 			= 32;
  MINSIZE 			= 32;
  MAXARRAYCOUNT 	= 12;
  INVALIDKEY 		= MaxInt;
  //ParseMessage enum
  ParseMessageEmpty 			= 0;  
  ParseMessageTokenRead 		= 1;
  ParseMessageReduction 		= 2;
  ParseMessageAccept 			= 3;
  ParseMessageNotLoadedError 	= 4;
  ParseMessageLexicalError 		= 5;
  ParseMessageSyntaxError 		= 6; 
  ParseMessageCommentError 		= 7;
  ParseMessageInternalError 	= 8;
  ParseMessageCommentBlockRead 	= 9;
  ParseMessageCommentLineRead 	= 10;
  //enum RecordType
  RecordTypeParameters  = 80; //'P'
  RecordTypeTableCounts = 84; //'T'
  RecordTypeInitial     = 73; //'I'
  RecordTypeSymbols     = 83; //'S'
  RecordTypeCharSets    = 67; //'C'
  RecordTypeRules       = 82; //'R'
  RecordTypeDfaStates   = 68; //'D'
  RecordTypeLRStates    = 76; //'L'
  RecordTypeComment     = 33; //'!'
  //enum EntryType
  EntryTypeEmpty		= 69; //'E'
  EntryTypeInteger		= 73; //'I'
  EntryTypeString		= 83; //'S'
  EntryTypeBoolean		= 66; //'B'
  EntryTypeByte			= 98; //'b'
  //enum TokenParseResult
  TokenParseResultEmpty            = 0;
  TokenParseResultAccept           = 1;
  TokenParseResultShift            = 2;
  TokenParseResultReduceNormal     = 3;
  TokenParseResultReduceEliminated = 4;
  TokenParseResultSyntaxError      = 5;
  TokenParseResultInternalError    = 6;
  
  charEnter 	= #13;
  charReset 	= #10;
  charTab 		= #9;
  charEndOfFile = #0;



type
  ParseMessage = integer;
  TokenParseResult = integer;
  
  Symbol = class
    Index: integer;      		// symbol index in symbol table
    Name: string;       		// name of the symbol
    SymbolType: integer; 		// type of the symbol
    m_text: string;       		// printable representation of symbol
    m_quotedChars: string;
  public
    constructor(index:integer; name:string; symType:integer);
	function IsQuotedChar(value:char): boolean;
	function FormatTerminalSymbol(source:string):string;
	function ToString:string; override;
  end;
  
  Entry = record //internal in ObjectMap
    Key: integer;
    Value: object;
  end;
  
  MapProvider = class
    m_count:integer;
    constructor;begin end;
    function GetIndex(key:integer):object;virtual;
    property Index[i:integer]:object read GetIndex; default;
    function GetEntry(key:integer):Entry;virtual;
    procedure Add(key:integer;value:object);virtual;
    procedure Remove(key:integer);virtual;
  end;
  
  SortedMapProvider = class(MapProvider)
    m_entries: array of Entry;
    m_lastKey:integer;      
	constructor(capacity:integer);
	function GetIndex(key:integer):object; override;//possible error!
    function GetEntry(key:integer):Entry;override;
    procedure Add(key:integer;value:object);override;
    procedure Remove(key:integer);virtual;
    function FindIndex(key:integer):integer; //possible error!
    function FindInsertIndex(key:integer;var found:boolean):integer;//possible error!
  end;

  IndexMapProvider = class(MapProvider)
    m_array: array of object;
	constructor;
	function GetIndex(key:integer):object; override;
	function GetEntry(key:integer):Entry;override;
    procedure Add(key:integer;value:object);override;
  end;
  
  ArrayMapProvider = class(MapProvider)
    m_entries: array of Entry;
    constructor(capacity:integer);
	function GetIndex(key:integer):object; override;
    function GetEntry(key:integer):Entry;override;
    procedure Add(key:integer;value:object);override;
  end;
  
  Unassigned = class
    constructor;begin end;
  end;

  ObjectMap = class
    m_readonly: boolean;
	m_mapProvider: MapProvider;
	constructor;
	function getCount:integer;
    property Count:integer read GetCount;
	procedure SetReadOnly(value: boolean);
    function GetIndex(key:integer):object;
    procedure SetIndex(key:integer; value:object);
	property ReadOnly:boolean read m_readonly write SetReadOnly;
  public
	property Index[i:integer]:object read GetIndex write SetIndex; default;
    function GetKey(index:integer):integer;
	procedure Remove(key:integer);
	procedure Add(key:integer;value:object);
	procedure SetMapProvider(readOnly:boolean);
  end;

  DfaState = class
    Index: integer;
    AcceptSymbol: Symbol;
    m_transitionVector: ObjectMap;
	constructor(Index:integer;acceptSymbol:Symbol;transitionVector:ObjectMap);
  end;
  
  LRStateAction = class
    Index: integer;
    StateSymbol: Symbol;
	Action: integer;
	Value: integer;
	constructor(Index:integer;StateSymbol:Symbol;Action:integer;Value:integer);
  end;

  LRState = class
	Index: integer;
    m_actions: array of LRStateAction;
    m_transitionVector: array of LRStateAction;
	constructor(Index:integer; actions: array of LRStateAction; transitionVector: array of LRStateAction);
    function ActionCount: integer;
	function GetAction(index:integer): LRStateAction;
	function GetActionBySymbolIndex(symbolIndex:integer): LRStateAction;
  end;
  
  Rule = class
    Index: integer;
    NonTerminal: Symbol;
    m_symbols: array of Symbol;
	ContainsOneNonTerminal: boolean;
    m_hasEmpty: boolean;
  public
	constructor(index:integer; nonTerminal:Symbol; symbols: array of Symbol);
	function GetIndex(key:integer):Symbol;
	property this[i:integer]:Symbol read GetIndex; default;
	function Name:string;
    function Count:integer;
	function Definition:string;
	function ToString:string;override;
  end;

  DfaEdge = record
	CharSetIndex: integer;
	TargetIndex: integer;
  end;

  Grammar = class
    Name: string;                		// Name of the grammar
	Version: string;             		// Version of the grammar
	Author: string;              		// Author of the grammar
	About: string;               		// Grammar description
	m_startSymbolIndex: integer;    	// Start symbol index
	CaseSensitive: boolean;       		// Grammar is case sensitive or not
	// Tables read from the binary grammar file
	m_symbolTable: array of Symbol;    	// Symbol table
	m_charSetTable: array of string;   	// Charset table
	m_ruleTable: array of Rule;      	// Rule table
	m_dfaStateTable: array of DfaState; // DFA state table
	m_lrStateTable: array of LRState;   // LR state table
	// Initial states
	m_dfaInitialStateIndex: integer;   	// DFA initial state index
	DfaInitialState: DfaState;   		// DFA initial state
	m_lrInitialState: integer;         	// LR initial state
	// Internal state of grammar parser
	m_reader: BinaryReader;             // Source of the grammar    
	m_entryCount: integer;              // Number of entries left
	m_errorSymbol: Symbol;
	EndSymbol: Symbol;
	constructor(reader:BinaryReader);
	function FileHeader:string;
    function StartSymbol:Symbol;
	function InitialLRState:LRState;
  private
	procedure Load;
	function ReadNextRecord:integer;
	procedure ReadHeader;
    procedure ReadTableCounts;
    procedure ReadInitialStates;
    procedure ReadSymbols;
	procedure ReadCharSets;
    procedure ReadRules;
    procedure ReadDfaStates;
    procedure ReadLRStates;
    function CreateDfaTransitionVector(edges: array of DfaEdge):ObjectMap;
	procedure ReadEmptyEntry;
	function ReadStringEntry:string;
	function ReadInt16Entry:integer;
	function ReadByteEntry:byte;
	function ReadBoolEntry:boolean;
	function ReadEntryType:integer;
	function ReadString:string;
	function ReadInt16:integer;
	function ReadByte:byte;
	function ReadBool:boolean;
	procedure OptimizeDfaTransitionVectors;
  end;

  xParser = object;//class!
  SourceLineReadCallback = procedure(pars: xParser; lineStart,lineLength:integer);
  
  //internal in Parser
  Token = record
	m_symbol: Symbol;     	// Token symbol.
	m_text: string;       	// Token text.
	m_start: integer;       // Token start stream start.
	m_length: integer;      // Token length.
	m_lineNumber: integer;  // Token source line number. (1-based).
	m_linePosition: integer;// Token position in source line (1-based).
	m_syntaxNode: object; 	// Syntax node which can be attached to the token.
  end;
  //internal in Parser
  LRStackItem = record
    m_token: Token;   		// Token in the LR stack item.
    m_state: LRState; 		// LR state associated with the item.
    m_rule: Rule;     		// Reference to a grammar rule if the item contains non-terminal.
  end;
  
  LALRParser = class
    m_grammar: Grammar;             // Grammar of parsed language.
    TrimReductions: boolean; 		// Allowes to minimize reduction tree.
    m_textReader: System.IO.TextReader;// Data to parse.
    m_buffer: array of char;     	// Buffer to keep current characters.
    m_bufferSize,       			// Size of the buffer.
	m_bufferStartIndex, 			// Absolute position of buffered first character. 
	m_charIndex,        			// Index of character in the buffer.
	m_preserveChars,    			// Number of characters to preserve when buffer is refilled.
	m_lineStart,        			// Relative position of line start to the buffer beginning.
	m_lineLength,       			// Length of current source line.
	m_lineNumber,   				// Current line number.
	m_commentLevel: integer;     	// Keeps stack level for embedded comments
	m_commentText: StringBuilder;   // Current comment text.
	SourceLineReadWenLineRead: SourceLineReadCallback;// Called when line reading finished. 
	m_token: Token;            		// Current token
	m_inputTokens: array of Token;  // Stack of input tokens.
	m_inputTokenCount: integer;  	// How many tokens in the input.
	m_lrStack: array of LRStackItem;// Stack of LR states used for LR parsing.
	m_lrStackIndex: integer;   		// Index of current LR state in the LR parsing stack. 
	m_lrState: LRState;        		// Current LR state.
	m_reductionCount: integer; 		// Number of items in reduction. It is Undefined if no reducton available. 
	m_expectedTokens: array of Symbol;// What tokens are expected in case of error?
	const MinimumBufferSize = 4096;	// Minimum size of char buffer.
    const EndOfString:char = char(0);// Designates last string terminator.
    const MinimumInputTokenCount = 2;// Minimum input token stack size.
    const MinimumLRStackSize = 256;	// Minimum size of reduction stack.
	const Undefined = -1;   		// Used for undefined int values. 


	constructor(tr:TextReader;grm:Grammar);
	function LanguageGrammar: Grammar;
	function TextReader: System.IO.TextReader;
    function CharPosition: integer;
    function LineNumber: integer;	// Gets current line number. It is 1-based.
    function LinePosition: integer;	// Gets current char position in the current source line. It is 1-based.
    function LineText: string; 		// Gets current source line text. It can be truncated if line is longer than 2048 characters.
    function ReadBuffer: integer;	// Reads next characters to the buffer.
	procedure MoveBy(delta:integer);// Increments current char index by delta character positions.
	procedure MoveToLineEnd;		// Moves current char pointer to the end of source line.
    procedure SetTokenSymbol(value:Symbol);
    function GetTokenSymbol: Symbol;
	property TokenSymbol:Symbol read GetTokenSymbol write SetTokenSymbol;
	function GetTokenText: string;
	procedure SetTokenText(text:string);
	property TokenText:string read GetTokenText write SetTokenText;// Gets or sets current token text.		
	function GetTokenSyntaxNode: object;
	procedure SetTokenSyntaxNode(text:object);
	property TokenSyntaxNode:object read GetTokenSyntaxNode write SetTokenSyntaxNode;// Gets or sets token syntax object associated with the current token or reduction.
    function TokenString: string;// Returns string representation of the token.
	procedure PushInputToken(sym:Symbol; text:string; syntaxNode:object);// Pushes a token to the input token stack.
	function PopInputToken: Symbol; // Pops token from the input token stack.
	function ReadToken: Symbol; // Reads next token from the input stream.
	procedure DiscardInputToken; // Removes current token and pops next token from the input stack.
	function CurrentLRState: LRState;
	function ReductionRule: Rule;
	function ReductionCount: integer;
	function GetReductionSyntaxNode(index:integer):object;
	function GetExpectedTokens:array of Symbol;
	function Parse: ParseMessage;// Executes next step of parser and returns parser state.
	procedure ProcessBlockComment;
	function CommentText: string;// Gets current comment text.
	function ParseToken: TokenParseResult;

		/// <summary>
		/// Gets or sets current token position relative to input stream beginning.
		/// </summary>
//		public int TokenCharPosition 
//		{
//			get { return m_token.m_start; }
//			set { m_token.m_start = value; }
//		}

		/// <summary>
		/// Gets or sets current token text length.
		/// </summary>
//		public int TokenLength 
//		{
//			get { return m_token.m_length; }
//			set { m_token.m_length = value; }
//		}

		/// <summary>
		/// Gets or sets current token line number. It is 1-based.
		/// </summary>
//		public int TokenLineNumber 
//		{
//			get { return m_token.m_lineNumber; }
//			set { m_token.m_lineNumber = value; }
//		}

		/// <summary>
		/// Gets or sets current token position in current source line. It is 1-based.
		/// </summary>
//		public int TokenLinePosition
//		{
//			get { return m_token.m_linePosition; }
//			set { m_token.m_linePosition = value; }
//		}

  end;


var UnassignedValue: Unassigned;


implementation
	


//------------------------------------------------------------------------------
//Symbol members
//------------------------------------------------------------------------------
constructor Symbol.Create(index:integer; name:string; symType:integer);
begin
  m_quotedChars := '|-+*?()[]{}<>!';
  self.Index := index;
  self.Name := name;
  self.SymbolType := symType;
end;

function Symbol.IsQuotedChar(value:char): boolean;
begin
  result := m_quotedChars.IndexOf(value) >= 0;
end;

function Symbol.FormatTerminalSymbol(source:string):string;
var res: StringBuilder;
    i: integer;
    ch: char;
begin
  res := new StringBuilder;
  for i:=0 to source.Length-1 do begin//string[0]
	ch := source[i]; 
	if ch = '''' then
	  res.Append('''')
	else 
	if IsQuotedChar(ch) or (ch = '"') then
	  res.Append('''' + ch + '''')
    else
	  res.Append(ch);
  end;	  
  result := res.ToString;
end;

function Symbol.ToString:string; 
begin
  if m_text=nil then
	case SymbolType of
	  SymbolTypeNonTerminal:  
		m_text := '<' + Name + '>';
	  SymbolTypeTerminal: 
		m_text := FormatTerminalSymbol(Name);
	  else
		m_text := '(' + Name + ')';
	end;	
  result := m_text;
end;


//------------------------------------------------------------------------------
//MapProvider members
//------------------------------------------------------------------------------
function MapProvider.GetIndex(key:integer):object;
begin
  result := -1;
end;

function MapProvider.GetEntry(key:integer):Entry;
begin
end;

procedure MapProvider.Add(key:integer;value:object);
begin
end;

procedure MapProvider.Remove(key:integer);
begin
  raise new System.InvalidOperationException();
end;

//------------------------------------------------------------------------------
//SortedMapProvider members
//------------------------------------------------------------------------------

constructor SortedMapProvider.Create(capacity:integer);
begin
  SetLength(m_entries, capacity);
end;

function SortedMapProvider.GetIndex(key:integer):object; //override;//possible error!
var minIndex,maxIndex,midIndex:integer;
begin
  minIndex := 0;
  maxIndex := m_count - 1;
  result := nil;
  if (maxIndex >= 0) and (key <= m_lastKey) then begin
    while minIndex < maxIndex do begin
	  midIndex := (maxIndex + minIndex) div 2;
	  if key <= m_entries[midIndex].Key then
	    maxIndex := midIndex
	  else
		minIndex := midIndex + 1;
	end;
	if key = m_entries[minIndex].Key then
	  result := m_entries[minIndex].Value;
  end;
end;

function SortedMapProvider.GetEntry(key:integer):Entry;//override;
begin
  result := m_entries[key];
end;

procedure SortedMapProvider.Add(key:integer;value:object);//override;
var found: boolean;
    index: integer;
    entries: array of Entry;
begin
  index := FindInsertIndex(key, found);
  if found then begin
    m_entries[index].Value := value;
    exit;
  end;
  if m_count >= m_entries.Length then begin
	SetLength(entries,m_entries.Length + GROWTH);
	System.Array.Copy(m_entries, 0, entries, 0, m_entries.Length);
	m_entries := entries;
  end;
  if index < m_count then
    System.Array.Copy(m_entries, index, m_entries, index + 1, m_count - index)
  else
    m_lastKey := key;
  m_entries[index].Key := key;
  m_entries[index].Value := value;
  m_count := m_count + 1;
end;

procedure SortedMapProvider.Remove(key:integer);//virtual;
var index,tailSize: integer;
begin
  index := FindIndex(key);
  if index >= 0 then begin
    tailSize := (m_count - 1) - index;
	if tailSize > 0 then begin
	  System.Array.Copy(m_entries, index + 1, m_entries, index, tailSize);
	end else 
	if m_count > 1 then begin
  	  m_lastKey := m_entries[m_count - 2].Key;
	end else begin
  	  m_lastKey := INVALIDKEY;
	end;
	m_count := m_count - 1;
	m_entries[m_count].Key := INVALIDKEY;
	m_entries[m_count].Value := nil;
  end;
end;

function SortedMapProvider.FindIndex(key:integer):integer; //possible error!
var minIndex,maxIndex,midIndex: integer;
begin
  minIndex := 0;
  result := -1;
  if (m_count > 0) and (key <= m_lastKey) then begin
    maxIndex := m_count - 1;
    while minIndex < maxIndex do begin
	  midIndex := (maxIndex + minIndex) div 2;
	  if key <= m_entries[midIndex].Key then
		maxIndex := midIndex
	  else
		minIndex := midIndex + 1;
    end;
	if key = m_entries[minIndex].Key then
	  result := minIndex;
  end;				
end;

function SortedMapProvider.FindInsertIndex(key:integer;var found:boolean):integer;//possible error!
var minIndex,maxIndex,midIndex: integer;
begin
  minIndex := 0;
  found := false;
  result := m_count;
  if (m_count > 0) and (key <= m_lastKey) then begin
    maxIndex := m_count - 1;
    while minIndex < maxIndex do begin
      midIndex := (maxIndex + minIndex) div 2;
  	  if key <= m_entries[midIndex].Key then
  	    maxIndex := midIndex
	  else
		minIndex := midIndex + 1;
	end;
	found := key = m_entries[minIndex].Key;
	result := minIndex;
  end;
end;

//------------------------------------------------------------------------------
//IndexMapProvider members
//------------------------------------------------------------------------------
constructor IndexMapProvider.Create;
var i: integer;
begin
  SetLength(m_array,MAXINDEX + 1);
  for i:=0 to m_array.Length-1 do
    m_array[i] := UnassignedValue;
end;
	
function IndexMapProvider.GetIndex(key:integer):object; //override;
begin 
  if (key >= m_array.Length) or (key < 0) then
    result := nil
  else
    result := m_array[key]; 
end;

function IndexMapProvider.GetEntry(key:integer):Entry;//override;
var idx,i: integer;
    value: object;
    entr: Entry;
begin
  idx := -1;
  result := entr;
  for i := 0 to m_array.Length-1 do begin
	value := m_array[i];
	if value <> UnassignedValue then
	  idx := idx +1;
	if idx = key then begin
	  entr.key := i;
	  entr.value := value;
	  exit;
	end;
  end;
end;

procedure IndexMapProvider.Add(key:integer;value:object);//override;
begin
  m_array[key] := value;
  m_count := m_count + 1;
end;


//------------------------------------------------------------------------------
//ArrayMapProvider members
//------------------------------------------------------------------------------
constructor ArrayMapProvider.Create(capacity:integer);
begin
  SetLength(m_entries,capacity);
end;

function ArrayMapProvider.GetIndex(key:integer):object; //override;
var i,entryKey:integer;
    entr: Entry;
begin
  i := m_count;
  result := nil;
  while i>0 do begin
    i := i - 1;
	entr := m_entries[i];
	entryKey := entr.Key;
	if entryKey < key then begin
	  continue;
	end else 
	if entryKey = key then begin
	  result := entr.Value;
	  break;
	end else 
	if entryKey > key then begin
      result := nil;
      break;
	end;
  end;
end;

function ArrayMapProvider.GetEntry(key:integer):Entry;//override;
begin
  result := m_entries[key];
end;

procedure ArrayMapProvider.Add(key:integer;value:object);//override;
begin
  m_entries[m_count].Key := key;
  m_entries[m_count].Value := value;
  m_count := m_count +1;
end;


//------------------------------------------------------------------------------
//ObjectMap members
//------------------------------------------------------------------------------
constructor ObjectMap.Create;
begin
  m_mapProvider := new SortedMapProvider(MINSIZE);
end;

function ObjectMap.getCount:integer;
begin
  result := m_mapProvider.m_count;
end;
  
procedure ObjectMap.SetReadOnly(value: boolean);
begin
  if m_readonly<>value then
    SetMapProvider(value);
  m_readonly := value; 
end;

function ObjectMap.GetIndex(key:integer):object;
begin
  result := m_mapProvider[key];
end;

procedure ObjectMap.SetIndex(key:integer; value:object);
begin
  m_mapProvider.Add(key, value);
end;

function ObjectMap.GetKey(index:integer):integer;
begin
  result := m_mapProvider.GetEntry(index).Key;
end;

procedure ObjectMap.Remove(key:integer);
begin
  m_mapProvider.Remove(key);
end;

procedure ObjectMap.Add(key:integer;value:object);
begin
  m_mapProvider.Add(key, Entry(value));
end;

procedure ObjectMap.SetMapProvider(readOnly:boolean);
var i,count: integer;
    provider: MapProvider;
    pr: SortedMapProvider;
    entr:Entry;
begin
  count := m_mapProvider.m_count;
  provider := m_mapProvider;
  if readOnly then begin
	pr := m_mapProvider as SortedMapProvider;
	if pr.m_lastKey <= MAXINDEX then begin
	  provider := new IndexMapProvider;
	end else 
	if (count <= MAXARRAYCOUNT) then begin
	  provider := new ArrayMapProvider(m_mapProvider.m_count);
	end;
  end else begin
	if not (provider is SortedMapProvider) then
	  provider := new SortedMapProvider(m_mapProvider.m_count);
  end;
  if provider <> m_mapProvider then begin
    for i:=0 to count-1 do begin
	  entr := m_mapProvider.GetEntry(i);
	  provider.Add(entr.Key, entr.Value);
	end;
    m_mapProvider := provider;
  end;
end;


//------------------------------------------------------------------------------
//DFAState members
//------------------------------------------------------------------------------
constructor DFAState.Create(Index:integer;acceptSymbol:Symbol;transitionVector:ObjectMap);
begin
  self.Index := Index;
  self.AcceptSymbol := AcceptSymbol;
  m_transitionVector := transitionVector;
end;


//------------------------------------------------------------------------------
//LRStateAction members
//------------------------------------------------------------------------------
constructor LRStateAction.Create(Index:integer;StateSymbol:Symbol;Action:integer;Value:integer);
begin
  self.Index := Index;
  self.StateSymbol := StateSymbol;
  self.Action := Action;
  self.Value := Value;
end;


//------------------------------------------------------------------------------
//LRState members
//------------------------------------------------------------------------------
constructor LRState.Create(Index:integer; actions: array of LRStateAction; transitionVector: array of LRStateAction);
begin
  self.Index := index;
  m_actions := actions;
  m_transitionVector := transitionVector;
end;

function LRState.ActionCount: integer;
begin
  result := m_actions.Length;
end;

function LRState.GetAction(index:integer): LRStateAction;
begin
  result := m_actions[index];
end;

function LRState.GetActionBySymbolIndex(symbolIndex:integer): LRStateAction;
begin
  result := m_transitionVector[symbolIndex];
end;


//------------------------------------------------------------------------------
//Rule members
//------------------------------------------------------------------------------
constructor Rule.Create(index:integer; nonTerminal:Symbol; symbols: array of Symbol);
begin
  self.Index := Index;
  self.NonTerminal := nonTerminal;
  m_symbols := symbols;
  ContainsOneNonTerminal := (symbols.Length = 1) and (symbols[0].SymbolType = SymbolTypeNonTerminal);
  m_hasEmpty := symbols.Length = 0;// && m_nonTerminal.Name == "empty";
end;

function Rule.Name:string;
begin
  result := '<' + NonTerminal.Name + '>';
end;

function Rule.Count:integer;
begin
  result := m_symbols.Length;
end;

function Rule.GetIndex(key:integer):Symbol;
begin
  result := m_symbols[index];
end;

function Rule.Definition:string;
var res:StringBuilder;
    i:integer;
begin
  res := new StringBuilder;
  for i:=0 to m_symbols.Length-1 do begin
	res.Append(m_symbols[i].ToString);
	if i < m_symbols.Length - 1 then
	  res.Append(' ');
  end;
  result := res.ToString;
end;

function Rule.ToString:string;
begin
  result := Name + ' ::= ' + Definition;
end;


//------------------------------------------------------------------------------
//Grammar members
//------------------------------------------------------------------------------
constructor Grammar.Create(reader:BinaryReader);
begin
  if reader = nil then
    raise new System.ArgumentNullException('reader');
  m_reader := reader;
  Load;
end;

function Grammar.FileHeader:string;
begin
  result := 'GOLD Parser Tables/v1.0';
end;
function Grammar.StartSymbol:Symbol;
begin
  result := m_symbolTable[m_startSymbolIndex];
end;
function Grammar.InitialLRState:LRState;
begin
  result := m_lrStateTable[m_lrInitialState]; 
end;

	
procedure Grammar.Load;
var recordType: integer;
begin
  if FileHeader<>ReadString then
    raise new FileLoadException('GPBLALRParser.GrammarWrongFileHeader');
  while m_reader.PeekChar <> -1 do begin
	recordType := ReadNextRecord;
	case recordType of
	  RecordTypeParameters: 	ReadHeader;
	  RecordTypeTableCounts: 	ReadTableCounts;
	  RecordTypeInitial: 		ReadInitialStates;
	  RecordTypeSymbols:	 	ReadSymbols;
	  RecordTypeCharSets:       ReadCharSets;
	  RecordTypeRules: 			ReadRules;
	  RecordTypeDfaStates:		ReadDfaStates;
	  RecordTypeLRStates: 		ReadLRStates;
	  else
	    raise new FileLoadException('GPBLALRParser.GrammarInvalidRecordType');
	end;
  end;
  DfaInitialState := m_dfaStateTable[m_dfaInitialStateIndex];
  OptimizeDfaTransitionVectors;
end;

function Grammar.ReadNextRecord:integer;
var recordType:char;
begin
  recordType := char(m_reader.ReadByte);
  case recordType of
    'M': begin
       m_entryCount := ReadInt16;
	   result := ReadByteEntry;
	 end;
	 else
	   raise new FileLoadException('GPBLALRParser.Grammar_InvalidRecordHeader');
  end;
end;
	
procedure Grammar.ReadHeader;
begin
  Name := ReadStringEntry;
  Version := ReadStringEntry;
  Author := ReadStringEntry;
  About := ReadStringEntry; 
  CaseSensitive := ReadBoolEntry; 
  m_startSymbolIndex := ReadInt16Entry; 
end;

procedure Grammar.ReadTableCounts;
begin
  SetLength(m_symbolTable    ,ReadInt16Entry);
  SetLength(m_charSetTable   ,ReadInt16Entry);
  SetLength(m_ruleTable      ,ReadInt16Entry);
  SetLength(m_dfaStateTable  ,ReadInt16Entry);
  SetLength(m_lrStateTable   ,ReadInt16Entry);
end;

procedure Grammar.ReadInitialStates;
begin
  m_dfaInitialStateIndex := ReadInt16Entry;
  m_lrInitialState       := ReadInt16Entry;
end;

procedure Grammar.ReadSymbols;
var index: integer;
    name: string;
    symbolType: integer;
    sym: Symbol;
begin
  index := ReadInt16Entry;
  name := ReadStringEntry;
  symbolType := ReadInt16Entry;			
  sym := new Symbol(index, name, symbolType);
  case symbolType of
	SymbolTypeError: m_errorSymbol := sym;
    SymbolTypeEnd:   EndSymbol := sym;
  end;
  m_symbolTable[index] := sym;
end;

procedure Grammar.ReadCharSets;
begin
  m_charSetTable[ReadInt16Entry] := ReadStringEntry;
end;

procedure Grammar.ReadRules;
var i,index: integer;
    nonTerminal: Symbol;
    symbols: array of Symbol;
begin
  index := ReadInt16Entry;
  nonTerminal := m_symbolTable[ReadInt16Entry];
  ReadEmptyEntry;
  SetLength(symbols,m_entryCount);
  for i:=0 to symbols.Length-1 do
    symbols[i] := m_symbolTable[ReadInt16Entry];
  m_ruleTable[index] := new Rule(index, nonTerminal, symbols);
end;

procedure Grammar.ReadDfaStates;
var i,index: integer;
    acceptSymbol: Symbol;
    acceptState: boolean;
    edges: array of DfaEdge;
    transitionVector: ObjectMap;
begin
  index := ReadInt16Entry;
  acceptSymbol := nil;
  acceptState := ReadBoolEntry;
  if acceptState then
    acceptSymbol := m_symbolTable[ReadInt16Entry]
  else
	ReadInt16Entry;
  ReadEmptyEntry;
  // Read DFA edges
  SetLength(edges,m_entryCount div 3);
  for i:=0 to edges.Length-1 do begin
    edges[i].CharSetIndex := ReadInt16Entry;
	edges[i].TargetIndex  := ReadInt16Entry;
	ReadEmptyEntry;
  end;
  // Create DFA state and store it in DFA state table
  transitionVector := CreateDfaTransitionVector(edges); 
  m_dfaStateTable[index] := new DfaState(index, acceptSymbol, transitionVector);
end;

procedure Grammar.ReadLRStates;
var targetIndex,i,index: integer;
	stateTable: array of LRStateAction;
	transitionVector: array of LRStateAction;
	sym: Symbol;
	action: integer;
begin
  index := ReadInt16Entry;
  ReadEmptyEntry;
  SetLength(stateTable, m_entryCount div 4); 
  for i:=0 to stateTable.Length-1 do begin
	sym := m_symbolTable[ReadInt16Entry];
	action := ReadInt16Entry;
	targetIndex := ReadInt16Entry;
	ReadEmptyEntry();
	stateTable[i] := new LRStateAction(i, sym, action, targetIndex);
  end;
  // Create the transition vector
  SetLength(transitionVector,m_symbolTable.Length); 
  for i:=0 to transitionVector.Length-1 do
    transitionVector[i] := nil;
  for i:=0 to stateTable.Length-1 do
	transitionVector[stateTable[i].StateSymbol.Index] := stateTable[i];
  m_lrStateTable[index] := new LRState(index, stateTable, transitionVector);
end;

function Grammar.CreateDfaTransitionVector(edges: array of DfaEdge):ObjectMap;
var i,j: integer;
    charSet: string;
begin
  result := new ObjectMap; 
  i := edges.Length;
  while i > 0 do begin
    i := i - 1;
    charSet := m_charSetTable[edges[i].CharSetIndex];
	for j:=0 to charSet.Length-1 do
  	  result[integer(charSet[j])] := edges[i].TargetIndex;
  end;
end;

procedure Grammar.ReadEmptyEntry;
begin
  if ReadEntryType<>EntryTypeEmpty then
    raise new FileLoadException('GPBLALRParser.GrammarEmptyEntryExpected');
  m_entryCount := m_entryCount - 1;
end;

function Grammar.ReadStringEntry:string;
begin
  if ReadEntryType<>EntryTypeString then
    raise new FileLoadException('GPBLALRParser.GrammarStringEntryExpected');
  m_entryCount := m_entryCount - 1;
  result := ReadString;
end;

function Grammar.ReadInt16Entry:integer;
begin
  if ReadEntryType<>EntryTypeInteger then
    raise new FileLoadException('GPBLALRParser.GrammarIntegerEntryExpected');
  m_entryCount := m_entryCount - 1;
  result := ReadInt16;
end;

function Grammar.ReadByteEntry:byte;
begin
  if ReadEntryType<>EntryTypeByte then
    raise new FileLoadException('GPBLALRParser.GrammarByteEntryExpected');
  m_entryCount := m_entryCount - 1;
  result := ReadByte;
end;

function Grammar.ReadBoolEntry:boolean;
begin
  if ReadEntryType<>EntryTypeBoolean then
    raise new FileLoadException('GPBParser.GrammarBooleanEntryExpected');
  m_entryCount := m_entryCount - 1;
  result := ReadBool;
end;

function Grammar.ReadEntryType:integer;
begin
  if m_entryCount = 0 then
    raise new FileLoadException('GPBLALRParser.GrammarNoEntry');
  result := ReadByte;
end;

function Grammar.ReadString:string;
var res: StringBuilder;
    unicodeChar: char;
begin  
  res := new StringBuilder; 
  unicodeChar := char(integer(ReadInt16));
  while unicodeChar <> char(0) do begin
    res.Append(unicodeChar);
	unicodeChar := char(integer(ReadInt16));
  end;
  result := res.ToString;
end;

function Grammar.ReadInt16:integer;
begin
  result := m_reader.ReadUInt16;
end;

function Grammar.ReadByte:byte;
begin
  result := m_reader.ReadByte;
end;

function Grammar.ReadBool:boolean;
begin
  result := ReadByte = 1;
end;
	
procedure Grammar.OptimizeDfaTransitionVectors;
var dfaStates: array of DfaState;
    state: DfaState;
    transitions: ObjectMap;
    transition: object;
    key,i,j,transitionIndex: integer;
begin
  dfaStates := m_dfaStateTable;
  for j:=0 to dfaStates.Length-1 do begin
    state := dfaStates[j];
	transitions := state.m_transitionVector;
	for i:=transitions.Count-1 downto 0 do begin
	  key := transitions.GetKey(i);
	  transition := transitions[key];
	  if transition <> nil then begin
	    transitionIndex := integer(transition);
		if transitionIndex >= 0 then
		  transitions[key] := dfaStates[transitionIndex]
		else
		  transitions[key] := nil;
  	  end;
	end;
    transitions.ReadOnly := true;
  end;
end;
	
	
	
//------------------------------------------------------------------------------
//Parser members
//------------------------------------------------------------------------------	
constructor LALRParser.Create(tr:System.IO.TextReader; grm:Grammar);
var start: LRStackItem;
begin
  m_lineNumber := 1;
  TrimReductions := true;
  if tr = nil then
    raise new ArgumentNullException('textReader');
  if grm = nil then
	raise new ArgumentNullException('grammar');
  m_textReader := tr;
  m_bufferSize := MinimumBufferSize;
  SetLength(m_buffer,m_bufferSize + 1);
  m_lineLength := Undefined;
  ReadBuffer;
  SetLength(m_inputTokens,MinimumInputTokenCount);
  SetLength(m_lrStack,MinimumLRStackSize);
  m_grammar := grm;
  // Put grammar start symbol into LR parsing stack.
  m_lrState := m_grammar.InitialLRState;
  start.m_token.m_symbol := m_grammar.StartSymbol;
  start.m_state := m_lrState;
  m_lrStack[m_lrStackIndex] := start;
  m_reductionCount := Undefined; // there are no reductions yet.
end;

function LALRParser.LanguageGrammar: Grammar;
begin
  result := m_grammar;
end;

function LALRParser.TextReader: System.IO.TextReader;
begin
  result := m_textReader;
end;

function LALRParser.CharPosition: integer;
begin
  result := m_charIndex + m_bufferStartIndex;
end;

function LALRParser.LineNumber: integer;// Gets current line number. It is 1-based.
begin
  result := m_lineNumber;
end;

function LALRParser.LinePosition:integer;// Gets current char position in the current source line. It is 1-based.
begin
  result := CharPosition - m_lineStart + 1;
end;

function LALRParser.LineText: string; // Gets current source line text. It can be truncated if line is longer than 2048 characters.
var lineStart,lineLength: integer;
begin
  lineStart := Math.Max(m_lineStart, 0);
  if m_lineLength = Undefined then
	// Line was requested outside of SourceLineReadCallback call
	lineLength := m_charIndex - lineStart
  else
	lineLength := m_lineLength - (lineStart - m_lineStart);
  if lineLength > 0 then
	result := new String(m_buffer, lineStart, lineLength)
  else
  	result := string.Empty;
end;

function LALRParser.ReadBuffer: integer;	// Reads next characters to the buffer.
var lineStart,lineCharCount,moveIndex,moveCount,count:integer;
    newBuffer:array of char;
begin
  // Find out how many bytes to preserve.
  // We truncate long lines.
  lineStart := m_lineStart;
  if m_lineStart < 0 then 
    lineStart := 0;
  lineCharCount := m_charIndex - lineStart;
  if lineCharCount > m_bufferSize div 2 then
	lineCharCount := m_bufferSize div 2;
  moveIndex := m_charIndex - lineCharCount;
  moveCount := lineCharCount + m_preserveChars;
  if moveCount > 0 then
    // We need to keep current token characters.
	if m_bufferSize - moveCount < 20 then begin
  	  // Grow the buffer
	  m_bufferSize := m_bufferSize * 2;
	  SetLength(newBuffer,m_bufferSize + 1);
	  System.Array.Copy(m_buffer, moveIndex, newBuffer, 0, moveCount);
	  m_buffer := newBuffer;				
	end else begin
	  System.Array.Copy(m_buffer, moveIndex, m_buffer, 0, moveCount); 
	end;
  // Read as many characters as possible.
  count := m_bufferSize - moveCount;
  result := m_textReader.Read(m_buffer, moveCount, count);
  // Mark character after last read one as End-Of-String
  m_buffer[moveCount + result] := EndOfString; 
  // Adjust buffer variables.
  m_bufferStartIndex := m_bufferStartIndex + moveIndex;
  m_charIndex := m_charIndex - moveIndex;
  m_lineStart := m_lineStart - moveIndex;
end;		

procedure LALRParser.MoveBy(delta:integer);// Increments current char index by delta character positions.
var i,lastIndex:integer;
begin
  for i:=delta-1 downto 0 do begin
    m_charIndex := m_charIndex + 1;
	if m_buffer[m_charIndex-1] = charEnter then begin// '\n'
	  if SourceLineReadWenLineRead <> nil then begin
		m_lineLength := m_charIndex - m_lineStart - 1; // Exclude '\n'
		lastIndex := m_lineStart + m_lineLength - 1;
		if (lastIndex >= 0) and (m_buffer[lastIndex] = charReset) then //'\r'
		  m_lineLength := m_lineLength - 1;
		if m_lineLength < 0 then
		  m_lineLength := 0;
		SourceLineReadWenLineRead(self, m_lineStart + m_bufferStartIndex, m_lineLength);
	  end;
	  m_lineNumber := m_lineNumber + 1;
	  m_lineStart := m_charIndex;
	  m_lineLength := Undefined;
	end;
	if m_buffer[m_charIndex] = charEndOfFile then
	  if SourceLineReadWenLineRead <> nil then begin
	    m_lineLength := m_charIndex - m_lineStart;
		if m_lineLength > 0 then
		  SourceLineReadWenLineRead(self, m_lineStart + m_bufferStartIndex, m_lineLength);
		m_lineLength := Undefined;
	  end;
  end;
end;

procedure LALRParser.MoveToLineEnd;		// Moves current char pointer to the end of source line.
var ch: char;
begin
  while true do begin
    ch := m_buffer[m_charIndex];
	case ch of
	  charEnter,charReset: 
	    break;
	  charEndOfFile:
	    if ReadBuffer = 0 then
		  break;
	  else
	    if m_commentText <> nil then
		  m_commentText.Append(ch);
	end;
	m_charIndex := m_charIndex + 1;
  end;
end;
    
procedure LALRParser.SetTokenSymbol(value:Symbol);
begin
  m_token.m_symbol := value;
end;
function LALRParser.GetTokenSymbol: Symbol;
begin
  result := m_token.m_symbol;
end;

function LALRParser.GetTokenText: string;
begin
  if m_token.m_text = nil then begin
    if m_token.m_length > 0 then
      m_token.m_text := new string(m_buffer, m_token.m_start - m_bufferStartIndex, m_token.m_length)
	else
	  m_token.m_text := string.Empty;
  end;
  result := m_token.m_text; 
end;

procedure LALRParser.SetTokenText(text:string);
begin
  m_token.m_text := text;
end;

function LALRParser.GetTokenSyntaxNode: object;
begin
  if m_reductionCount = Undefined then
	result := m_token.m_syntaxNode
  else
    result := m_lrStack[m_lrStackIndex].m_token.m_syntaxNode;
end;

procedure LALRParser.SetTokenSyntaxNode(text:object);
begin
  if m_reductionCount = Undefined then
    m_token.m_syntaxNode := text
  else
    m_lrStack[m_lrStackIndex].m_token.m_syntaxNode := text;
end;

function LALRParser.TokenString: string;// Returns string representation of the token.
var sb: StringBuilder;
    i: integer;
    ch: char;
begin
  if m_token.m_symbol.SymbolType <> SymbolTypeTerminal then begin
	result := m_token.m_symbol.ToString;
	exit;
  end;
  sb := new StringBuilder(m_token.m_length);
  for i := 0 to m_token.m_length-1 do begin
    ch := m_buffer[m_token.m_start - m_bufferStartIndex + i];
	if ch < ' ' then
	  case ch of
		charEnter: sb.Append('{LF}');
		charReset: sb.Append('{CR}');
		charTab: sb.Append('{HT}');
  	  end
	else
	  sb.Append(ch);
  end;
  result := sb.ToString;{}
end;

procedure LALRParser.PushInputToken(sym:Symbol; text:string; syntaxNode:object);// Pushes a token to the input token stack.
var newTokenArray: array of Token;
begin
  if m_token.m_symbol <> nil then begin
	if m_inputTokenCount = m_inputTokens.Length then begin
	  SetLength(newTokenArray, m_inputTokenCount*2);
	  System.Array.Copy(m_inputTokens, newTokenArray, m_inputTokenCount);
	  m_inputTokens := newTokenArray;
	end;
	m_inputTokens[m_inputTokenCount] := m_token;
	m_inputTokenCount := m_inputTokenCount + 1;
  end;
//  m_token := new Token;
  m_token.m_symbol := sym;
  m_token.m_text := text;
  m_token.m_length := 0;
  if text <> nil then 
    m_token.m_length := text.Length;
  m_token.m_syntaxNode := syntaxNode;{}
end;

function LALRParser.PopInputToken: Symbol; // Pops token from the input token stack.
begin
  result := m_token.m_symbol;
  if m_inputTokenCount > 0 then begin
    m_inputTokenCount := m_inputTokenCount - 1;
    m_token := m_inputTokens[m_inputTokenCount];
  end else begin
	m_token.m_symbol := nil;
	m_token.m_text := nil;
  end;{}
end;



function LALRParser.ReadToken: Symbol; // Reads next token from the input stream.
var tokenSymbol: Symbol;
    dfaStateTable: array of DfaState;
    ch: char;
    curDFAState: DfaState;
    lookahead,tokenLength: integer;
begin
  m_token.m_text := nil;
  m_token.m_start := m_charIndex + m_bufferStartIndex;
  m_token.m_lineNumber := m_lineNumber;
  m_token.m_linePosition := m_charIndex - m_lineStart + 1;
  lookahead   := m_charIndex;  // Next look ahead char in the input
  tokenLength := 0;       
  tokenSymbol := nil;
  dfaStateTable := m_grammar.m_dfaStateTable;
  ch := m_buffer[lookahead];
  if ch = EndOfString then begin
	if ReadBuffer = 0 then begin
	  m_token.m_symbol := m_grammar.EndSymbol;
	  m_token.m_length := 0;
	  result := m_token.m_symbol;
	  exit;
	end;
	lookahead   := m_charIndex;
	ch := m_buffer[lookahead];
  end;
  curDFAState := m_grammar.DfaInitialState;
  while true do begin
	curDFAState := curDFAState.m_transitionVector[integer(ch)] as DfaState;
	// This block-if statement checks whether an edge was found from the current state.
	// If so, the state and current position advance. Otherwise it is time to exit the main loop
	// and report the token found (if there was it fact one). If the LastAcceptState is -1,
	// then we never found a match and the Error Token is created. Otherwise, a new token
	// is created using the Symbol in the Accept State and all the characters that
	// comprise it.
	if curDFAState <> nil then begin
	  // This code checks whether the target state accepts a token. If so, it sets the
	  // appropiate variables so when the algorithm in done, it can return the proper
	  // token and number of characters.
	  lookahead := lookahead + 1;
	  if curDFAState.AcceptSymbol <> nil then begin
	    tokenSymbol := curDFAState.AcceptSymbol;
		tokenLength := lookahead - m_charIndex;
	  end;
	  ch := m_buffer[lookahead];
	  if ch = EndOfString then begin
	    m_preserveChars := lookahead - m_charIndex;
		if ReadBuffer = 0 then
		  lookahead := m_charIndex + m_preserveChars
		else begin
		  lookahead := m_charIndex + m_preserveChars;
		  ch := m_buffer[lookahead];
		end;
		m_preserveChars := 0;
	  end;
	end else begin
	  if tokenSymbol <> nil then begin
	    m_token.m_symbol := tokenSymbol;
		m_token.m_length := tokenLength;
		MoveBy(tokenLength);
	  end else begin
		//Tokenizer cannot recognize symbol
		m_token.m_symbol := m_grammar.m_errorSymbol;
		m_token.m_length := 1;
		MoveBy(1);
	  end;     
	  break;
	end;
  end;
  result := m_token.m_symbol;{}
end;

procedure LALRParser.DiscardInputToken; // Removes current token and pops next token from the input stack.
begin
  if m_inputTokenCount > 0 then begin
    m_inputTokenCount := m_inputTokenCount - 1;
	m_token := m_inputTokens[m_inputTokenCount];
  end else begin
	m_token.m_symbol := nil;
    m_token.m_text := nil;
  end;{}
end;

function LALRParser.CurrentLRState: LRState;
begin
  result := m_lrState;
end;

function LALRParser.ReductionRule: Rule;
begin
  result := m_lrStack[m_lrStackIndex].m_rule;
end;

function LALRParser.ReductionCount: integer;
begin
  result := m_reductionCount;
end;

function LALRParser.GetReductionSyntaxNode(index:integer):object;
begin
  //DarkStar Changed for Speed!
  //if (index < 0 || index >= m_reductionCount)
  //	throw new IndexOutOfRangeException();
  result := m_lrStack[m_lrStackIndex - m_reductionCount + index].m_token.m_syntaxNode;{}
end;

function LALRParser.GetExpectedTokens:array of Symbol;
begin
  result := m_expectedTokens;  {}
end;

function LALRParser.Parse: ParseMessage;// Executes next step of parser and returns parser state.
var parseResult: TokenParseResult;
    readTokenSymbol: Symbol;
    symType: integer;
begin
  if m_token.m_symbol <> nil then
    case m_token.m_symbol.SymbolType of
	  SymbolTypeCommentLine:begin
	    DiscardInputToken; //Remove it 
	    MoveToLineEnd;
	  end;
	  SymbolTypeCommentStart:
	    ProcessBlockComment;
	end;
  while true do begin
    if m_token.m_symbol = nil then begin
	  //We must read a token
	  readTokenSymbol := ReadToken;
	  symType := readTokenSymbol.SymbolType;					
	  if (m_commentLevel = 0) and (symType <> SymbolTypeCommentLine) and (symType <> SymbolTypeCommentStart) and (symType <> SymbolTypeWhiteSpace) then begin
		result := ParseMessageTokenRead;
		exit;
	  end;
	end else begin
	  //==== Normal parse mode - we have a token and we are not in comment mode
	  case m_token.m_symbol.SymbolType of
		SymbolTypeWhiteSpace:
		  DiscardInputToken;  // Discard Whitespace
		SymbolTypeCommentStart:begin
		  m_commentLevel := 1; // Switch to block comment mode.
		  result := ParseMessageCommentBlockRead;
		  exit;
		end;
		SymbolTypeCommentLine:begin
		  result := ParseMessageCommentLineRead;
		  exit;
		end;
		SymbolTypeError:begin
		  result := ParseMessageLexicalError;
		  exit;
		end;
		else begin
		  //Finally, we can parse the token
		  parseResult := ParseToken;
		  case parseResult of
			TokenParseResultAccept:begin
			  result := ParseMessageAccept;
			  exit;
			end;
			TokenParseResultInternalError:begin
			  result := ParseMessageInternalError;
			  exit;
			end;
			TokenParseResultReduceNormal:begin
              if not m_lrStack[m_lrStackIndex].m_rule.m_hasEmpty then begin
                result := ParseMessageReduction;
                exit;
              end;  
            end;
		    TokenParseResultShift: 
			  //A simple shift, we must continue
			  DiscardInputToken; // Okay, remove the top token, it is on the stack
			TokenParseResultSyntaxError:begin
			  result := ParseMessageSyntaxError;
			  exit;
			end;
		  end;
		end;					
      end;
    end;
  end;
end;

procedure LALRParser.ProcessBlockComment;
var symType: integer;
begin
  if m_commentLevel > 0 then begin
    if m_commentText <> nil then
	  m_commentText.Append(TokenText);
	DiscardInputToken();
	while true do begin
	  symType := ReadToken.SymbolType;
	  if m_commentText <> nil then
	    m_commentText.Append(TokenText);
	  DiscardInputToken();
	  case symType of
		SymbolTypeCommentStart: 
		  m_commentLevel := m_commentLevel + 1;
		SymbolTypeCommentEnd:begin
		  m_commentLevel := m_commentLevel - 1;
		  if m_commentLevel = 0 then
		    exit;
		end;
		SymbolTypeEnd:
		  raise new Exception('CommentError');
	  end;
	end;
  end;{}
end;

function LALRParser.CommentText: string;// Gets current comment text.
var lineComment,blockComment:string;
begin
  result := string.Empty;
  if m_token.m_symbol <> nil then 
    case m_token.m_symbol.SymbolType of
      SymbolTypeCommentLine:begin
		m_commentText := new StringBuilder;
		m_commentText.Append(TokenText);
		DiscardInputToken; //Remove token 
		MoveToLineEnd;
		lineComment := m_commentText.ToString;
		m_commentText := nil;
		result := lineComment;
      end;
	  SymbolTypeCommentStart:begin
		m_commentText := new StringBuilder;
		ProcessBlockComment; 
		blockComment := m_commentText.ToString;
		m_commentText := nil;
	    result := blockComment;
      end;
    end;{}
end;


function LALRParser.ParseToken: TokenParseResult;
var stateAction: LRStateAction;
    length,i,newIndex: integer;
    newArray: array of Symbol;
    nextToken:LRStackItem;
    currentRule: Rule;
    head: LRStackItem;
    nextState: LRState;
begin
  stateAction := m_lrState.m_transitionVector[m_token.m_symbol.Index];
  if stateAction <> nil then begin
    //Work - shift or reduce
    if m_reductionCount > 0 then begin
  	  newIndex := m_lrStackIndex - m_reductionCount;
	  m_lrStack[newIndex] := m_lrStack[m_lrStackIndex];
	  m_lrStackIndex := newIndex;
	end;
	m_reductionCount := Undefined;
	case stateAction.Action of
	  LRActionAccept:begin
		m_reductionCount := 0;
		result := TokenParseResultAccept;
		exit;
	  end;
	  LRActionShift:begin
		m_lrState := m_grammar.m_lrStateTable[stateAction.Value];
		//nextToken := new LRStackItem;
		nextToken.m_token := m_token;
		nextToken.m_state := m_lrState;
		m_lrStackIndex := m_lrStackIndex + 1;
		m_lrStack[m_lrStackIndex] := nextToken;
		result := TokenParseResultShift;
		exit;
	  end;
      LRActionReduce:begin
	    //Produce a reduction - remove as many tokens as members in the rule & push a nonterminal token
		currentRule := m_grammar.m_ruleTable[stateAction.Value];
        if TrimReductions and currentRule.ContainsOneNonTerminal then begin
          head := m_lrStack[m_lrStackIndex];
          head.m_token.m_symbol := currentRule.NonTerminal;
          head.m_token.m_text := nil;
          nextState := m_lrStack[m_lrStackIndex - 1].m_state;
          m_lrState := m_grammar.m_lrStateTable[nextState.m_transitionVector[currentRule.NonTerminal.Index].Value];
          head.m_state := m_lrState;
          m_lrStack[m_lrStackIndex] := head;
          result := TokenParseResultReduceEliminated;
          exit;
        end else begin
          //head := new LRStackItem;
          head.m_rule := currentRule;
          head.m_token.m_symbol := currentRule.NonTerminal;
          head.m_token.m_text := nil;
          m_reductionCount := currentRule.m_symbols.Length;
          nextState := m_lrStack[m_lrStackIndex - m_reductionCount].m_state;
          m_lrState := m_grammar.m_lrStateTable[nextState.m_transitionVector[currentRule.NonTerminal.Index].Value];
          head.m_state := m_lrState;
          m_lrStackIndex := m_lrStackIndex + 1;
          m_lrStack[m_lrStackIndex] := head;
          result := TokenParseResultReduceNormal;
          exit;
        end;
      end;  
	end;
  end;
  //=== Syntax Error! Fill Expected Tokens
  SetLength(m_expectedTokens, m_lrState.ActionCount); 
  length := 0;
  for i := 0 to  m_lrState.ActionCount-1 do 
    case m_lrState.GetAction(i).StateSymbol.SymbolType of
	  SymbolTypeTerminal,SymbolTypeEnd:begin
		m_expectedTokens[length] := m_lrState.GetAction(i).StateSymbol;
		length := length + 1;
	  end;
	end;
  if length < m_expectedTokens.Length then begin
	SetLength(newArray, length);
	System.Array.Copy(m_expectedTokens, newArray, length);
	m_expectedTokens := newArray;
  end;
  result := TokenParseResultSyntaxError;{}
end;
{}
initialization
  UnassignedValue := new Unassigned;
finalization

end.