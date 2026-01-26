// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.TreeRealization;
using System.Collections;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.Errors;

namespace PascalABCCompiler
{
    public class WarningStringResources
    {
        public static string Get(string key)
        {
            return PascalABCCompiler.StringResources.Get("WARNING_" + key);
        }

        public static string Get(string key, params object[] values)
        {
            return (string.Format(Get(key), values));
        }
    }

    public class HintStringResources
    {
        public static string Get(string key)
        {
            return PascalABCCompiler.StringResources.Get("HINT_" + key);
        }

        public static string Get(string key, params object[] values)
        {
            return (string.Format(Get(key), values));
        }
    }

    public class CompilerWarningWithLocation : CompilerWarning
    {
        protected location loc;

        public override SourceLocation SourceLocation
        {
            get
            {
                try
                {
                    return new SourceLocation(loc.file_name, loc.begin_line_num, loc.begin_column_num, loc.end_line_num, loc.end_column_num);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
    }

    public class GenericWarning : CompilerWarningWithLocation
    {
        protected string message;

        public GenericWarning(string message, location loc)
        {
            this.message = message;
            this.loc = loc;
        }

        public override string ToString()
        {
            //return ("Possible two type convertions\n"+_en.location.ToString());
            return WarningStringResources.Get(message);
        }
    }

    public class GenericHint : GenericWarning
    {
        public GenericHint(string message, location loc):base(message, loc)
        {
            
        }

        public override string ToString()
        {
            //return ("Possible two type convertions\n"+_en.location.ToString());
            return WarningStringResources.Get(message);
        }
    }

    public class UnreachableCodeDetected : CompilerWarningWithLocation
    {
        public UnreachableCodeDetected(location loc)
        {
            this.loc = loc;
        }

        public override string ToString()
        {
            //return ("Possible two type convertions\n"+_en.location.ToString());
            return (WarningStringResources.Get("UNREACHABLE_CODE_DETECTED"));
        }
    }

    public class InfiniteRecursion : CompilerWarningWithLocation
    {
        public InfiniteRecursion(location loc)
        {
            this.loc = loc;
        }

        public override string ToString()
        {
            //return ("Possible two type convertions\n"+_en.location.ToString());
            return (WarningStringResources.Get("INFINITE_RECURSION"));
        }
    }

    public class UndefinedReturnValue : CompilerWarningWithLocation
    {
        string name;
        public UndefinedReturnValue(string name, location loc)
        {
            this.name = name;
            this.loc = loc;
        }

        public override string ToString()
        {
            //return ("Possible two type convertions\n"+_en.location.ToString());
            return (string.Format(WarningStringResources.Get("UNDEFINED_RETURN_VALUE_{0}"),
                name));
        }
    }

    public class UseWithoutAssign : CompilerWarningWithLocation
    {
        string name;

        public UseWithoutAssign(string name, location loc)
        {
            this.name = name;
            this.loc = loc;
        }

        public override string ToString()
        {
            //return ("Possible two type convertions\n"+_en.location.ToString());
            return (string.Format(WarningStringResources.Get("USE_VAR_{0}_WITHOUT_ASSIGN"),
                name));
        }

        
    }

    public class UnusedVariable : CompilerWarningWithLocation
    {
        string name;

        public UnusedVariable(string name, location loc)
        {
            this.name = name;
            this.loc = loc;
        }

        public override string ToString()
        {
            //return ("Possible two type convertions\n"+_en.location.ToString());
            return (string.Format(WarningStringResources.Get("UNUSED_VAR_{0}"),
                name));
        }
    }

    public class UnusedParameter : CompilerWarningWithLocation
    {
        string name;

        public UnusedParameter(string name, location loc)
        {
            this.name = name;
            this.loc = loc;
        }

        public override string ToString()
        {
            //return ("Possible two type convertions\n"+_en.location.ToString());
            return (string.Format(WarningStringResources.Get("UNUSED_PARAM_{0}"),
                name));
        }
    }
    public class UnusedField : CompilerWarningWithLocation
    {
        string name;

        public UnusedField(string name, location loc)
        {
            this.name = name;
            this.loc = loc;
        }

        public override string ToString()
        {
            //return ("Possible two type convertions\n"+_en.location.ToString());
            return (string.Format(WarningStringResources.Get("UNUSED_FIELD_{0}"),
                name));
        }
    }

    public class AssignWithoutUsing : CompilerWarningWithLocation
    {
        string name;

        public AssignWithoutUsing(string name, location loc)
        {
            this.name = name;
            this.loc = loc;
        }

        public override string ToString()
        {
            //return ("Possible two type convertions\n"+_en.location.ToString());
            return (string.Format(WarningStringResources.Get("ASS_WITHOUT_USE_{0}"),
                name));
        }
    }
}