// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
#define USEGZIP

using System;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Threading;
using PascalABCCompiler.Errors;
using System.IO.Compression;

namespace PascalABCCompiler.ParserTools
{

    public class CGTResourceExtractor
    {

        public static Stream Extract(ResourceManager rm,string rname)
        {
            MemoryStream ms=null;
            try
            {
                
                ms = new MemoryStream((byte[])rm.GetObject(rname, Thread.CurrentThread.CurrentCulture));
#if USEGZIP
                GZipStream zipStream = new GZipStream(ms, CompressionMode.Decompress);
                ms = new MemoryStream();
                byte[] data = new byte[4096];
                int size;
                while ((size = zipStream.Read(data, 0, data.Length)) > 0)
                    ms.Write(data, 0, size);
                zipStream.Close();
#endif
                ms.Position = 0;
            }
            catch (Exception e)
            {
                throw new CompilerInternalError("CGTResourceExtractor." + rname, e);
            }
            return ms;
        }
    }
}