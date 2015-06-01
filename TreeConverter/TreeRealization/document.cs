// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace PascalABCCompiler.TreeRealization
{
    /// <summary>
    /// Класс, описывающий файл кода.
    /// Используется для указания позиции частей кода.
    /// </summary>
	[Serializable]
	public class document : SemanticTree.IDocument
	{
        /// <summary>
        /// Имя файла.
        /// </summary>
		private readonly string _file_name;

        /// <summary>
        /// Конструктор документа.
        /// </summary>
        /// <param name="file_name">Путь к документу.</param>
		public document(string file_name)
		{
			_file_name=file_name;
		}

        /// <summary>
        /// Путь к документу.
        /// </summary>
		public string file_name
		{
			get
			{
				return _file_name;
			}
		}
        
		public override string ToString()
		{
			return (_file_name);
		}

	}

}