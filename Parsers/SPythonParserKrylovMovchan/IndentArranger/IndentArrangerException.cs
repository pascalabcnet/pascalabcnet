using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndentArranger
{
    public class IndentArrangerException : Exception
    {
        public IndentArrangerException(string message) : base(message) { }
    }

    public class ManyIndentsAdditionException : IndentArrangerException
    {
        public ManyIndentsAdditionException() : 
            base("ManyIndentsAdditionException: в одной строке было добавлено более одного отступа.") { }
    }

    public class NotPossibleIndentException : IndentArrangerException
    {
        public NotPossibleIndentException() : 
            base("NotPossibleIndentException: количество пробелов в строке не соответствует отступу.") { }
    }
}
