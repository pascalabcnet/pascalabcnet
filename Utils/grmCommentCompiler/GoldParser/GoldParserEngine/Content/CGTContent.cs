using System;
using com.calitha.goldparser.structure;

namespace com.calitha.goldparser.content
{
	/// <summary>
	/// The CGTContent contains all the records that is needed to implement a parser.
	/// </summary>
	public class CGTContent
	{
		private Parameters parameters;
		private TableCounts tableCounts;
		private SymbolTable symbolTable;
		private CharacterSetTable characterSetTable;
		private RuleTable ruleTable;
		private InitialStatesRecord initialStates;
		private DFAStateTable dfaStateTable;
		private LALRStateTable lalrStateTable;

		public CGTContent(CGTStructure structure)
		{
			if (structure.Records.Count < 3)
				throw new CGTContentException("File does not have enough records");
			parameters = new Parameters(structure.Records[0]);
			tableCounts = new TableCounts(structure.Records[1]);
			
			int initialStatesStart = 2;
			int characterSetStart = initialStatesStart + 1;
			int symbolStart = characterSetStart + TableCounts.CharacterSetTable;
			int ruleStart = symbolStart + TableCounts.SymbolTable;
			int dfaStart = ruleStart + TableCounts.RuleTable;
			int lalrStart = dfaStart + TableCounts.DFATable;
			int specifiedRecordCount = lalrStart + TableCounts.LALRTable;
			if (structure.Records.Count != specifiedRecordCount)
				throw new CGTContentException("Invalid number of records");
			
			characterSetTable =	new CharacterSetTable(structure,
													  characterSetStart,
													  TableCounts.CharacterSetTable);
			symbolTable = new SymbolTable(structure,
				symbolStart,
				TableCounts.SymbolTable);

			ruleTable = new RuleTable(structure,
									  ruleStart,
									  TableCounts.RuleTable);
			initialStates = new InitialStatesRecord(structure.Records[initialStatesStart]);
			dfaStateTable = new DFAStateTable(structure,dfaStart,TableCounts.DFATable);
			lalrStateTable = new LALRStateTable(structure,lalrStart,TableCounts.LALRTable);
		}
		
		
		public Parameters Parameters{get{return parameters;}}
		public TableCounts TableCounts{get{return tableCounts;}}
		public SymbolTable SymbolTable{get{return symbolTable;}}
		public CharacterSetTable CharacterSetTable{get{return characterSetTable;}}
		public RuleTable RuleTable{get{return ruleTable;}}
		public InitialStatesRecord InitialStates{get{return initialStates;}}
		public DFAStateTable DFAStateTable{get{return dfaStateTable;}}
		public LALRStateTable LALRStateTable{get{return lalrStateTable;}}
	}
}
