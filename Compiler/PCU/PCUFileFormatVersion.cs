// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

// при внесении изменений в структуру PCU файла версию увеличить на 1

namespace PascalABCCompiler.PCU
{
    public static class PCUFileFormatVersion
    {
#if PABCNET_MODERN
        public static System.Int16 Version = 125;
#else
        public static System.Int16 Version = 124;
#endif
    }
}
