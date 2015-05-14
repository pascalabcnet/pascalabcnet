using System;

namespace com.calitha.goldparser
{
	/// <summary>
	/// The Location defines positional information of the input that is being parsed.
	/// </summary>
	public class Location
	{
		private int position;
		private int lineNr;
		private int columnNr;

		/// <summary>
		/// Creates a new Location object.
		/// </summary>
		/// <param name="position">Zero based position.</param>
		/// <param name="lineNr">Zero based line number.</param>
		/// <param name="columnNr">Zero based column number.</param>
		public Location(int position, int lineNr, int columnNr)
		{
			Init(position,lineNr,columnNr);
		}

		/// <summary>
		/// Creates a new Location object.
		/// </summary>
		/// <param name="location">Positional information will be copied from this object.</param>
		public Location(Location location)
		{
			Init(location.position,location.lineNr,location.columnNr);
		}

		private void Init(int position, int lineNr, int columnNr)
		{
			this.position = position;
			this.lineNr = lineNr;
			this.columnNr = columnNr;
		}

		public Location Clone()
		{
			return new Location(this);
		}

		/// <summary>
		/// Converts the location to a string. Line number and column number will be
		/// incremented by one.
		/// </summary>
		/// <returns>The output string.</returns>
		public override string ToString()
		{
			return "(pos: "+(position+0)+", ln: "+(lineNr+1)+", col: "+(columnNr+1)+")"; 
		}

		/// <summary>
		/// Signals that the input has encountered an end-of-line.
		/// </summary>
		public void NextLine()
		{
			position++;
			lineNr++;
			columnNr = 0;
		}

		/// <summary>
		/// Signals that the input has advanced one character (which was not an end-of-line.
		/// </summary>
		public void NextColumn()
		{
			position++;
			columnNr++;
		}

		/// <summary>
		/// The zero-based position.
		/// </summary>
		public int Position
		{
			get{return position;}
		}

		/// <summary>
		/// The zero-based line number.
		/// </summary>
		public int LineNr
		{
			get{return lineNr;}
		}

		/// <summary>
		/// The zero-based column number.
		/// </summary>
		public int ColumnNr
		{
			get{return columnNr;}
		}
	}

}
