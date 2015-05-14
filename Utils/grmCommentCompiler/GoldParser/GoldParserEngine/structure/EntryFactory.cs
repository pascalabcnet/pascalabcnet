using System;
using com.calitha.commons;
using com.calitha.goldparser.structure;

namespace com.calitha.goldparser.structure
{
	/// <summary>
	/// The EntryFactory can create the correct Entry object by looking at the
	/// entry type byte.
	/// </summary>
	public sealed class EntryFactory
	{
		private EntryFactory()
		{
		}

		static public Entry CreateEntry(CalithaBinReader reader)
		{
			Entry entry = null;
			byte entryType = reader.ReadByte();
			switch (entryType)
			{
				case 69: // 'E'
					entry =  new EmptyEntry();
					break;
				case 98: // 'b'
					entry = new ByteEntry(reader);
					break;
				case 66: // 'B'
					entry = new BooleanEntry(reader);
					break;
				case 73: // 'I'
					entry =  new IntegerEntry(reader);
					break;
				case 83: // 'S'
					entry = new StringEntry(reader);
					break;
				default: //Unknown
					throw new CGTStructureException("Unknown entry type");
			}
			return entry;
		}

	}
}
