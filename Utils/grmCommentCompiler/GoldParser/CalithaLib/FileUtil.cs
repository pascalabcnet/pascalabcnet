using System;
using System.IO;

namespace com.calitha.commons
{
	/// <summary>
	/// The FilUtil class contains a selection of convenience methods
	/// for dealing with files.
	/// </summary>
	public sealed class FileUtil
	{
		private FileUtil()
		{
		}

		/// <summary>
		/// Determines if a stream contains a BOM for UTF-16 Little Endian.
		/// </summary>
		/// <param name="stream">stream</param>
		/// <returns>true if it contains the BOM, otherwise false</returns>
		public static bool IsUTF16LE(Stream stream)
		{
			byte[] startBytes = new byte[2];
			int count = stream.Read(startBytes, 0, startBytes.Length);
			return (count == 2) && (startBytes[0] == 0xFF) && (startBytes[1] == 0xFE);
		}

		/// <summary>
		/// Determines if a file contains a BOM for UTF-16 Little Endian.
		/// </summary>
		/// <param name="filename">stream</param>
		/// <returns>true if it contains the BOM, otherwise false.</returns>
		public static bool IsUTF16LE(string filename)
		{
			FileStream fs = new FileStream(filename, FileMode.Open);
			bool result = IsUTF16LE(fs);
			fs.Close();
			return result;
		}

	}
}
