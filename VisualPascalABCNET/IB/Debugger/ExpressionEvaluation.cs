// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using System.Text;
using Debugger;
using VisualPascalABCPlugins;
using System.Runtime.ExceptionServices;
using PascalABCCompiler.Parsers;
using Languages.Integration;

namespace VisualPascalABC
{
    public enum ValueKind
    {
        Integer,

    }

    public class ValueWrapper : IValue
    {
        private Value value;

        public ValueWrapper(Value value)
        {
            this.value = value;
        }
    }

    public struct RetValue : IRetValue
    {
        //private ValueKind kind;
        public object prim_val;
        public Value obj_val;
        public DebugType type;
        public Type managed_type;
        public bool syn_err;
        public string err_mes;
        public bool is_null;

        #region IRetValue Member

        IValue IRetValue.ObjectValue
        {
            get { return new ValueWrapper(obj_val); }
        }

        object IRetValue.PrimitiveValue
        {
            get { return prim_val; }
        }

        #endregion
    }

    public abstract class WatchEvaluationError : System.Exception
    {
    }

    public class CommonEvaluationError : WatchEvaluationError
    {
        private string mes;

        public CommonEvaluationError(System.Exception ex)
        {
            mes = ex.Message;
        }

        public override string ToString()
        {
            return mes;
        }
    }

    public class IncompatibleTypesInExpression : WatchEvaluationError
    {
        public override string ToString()
        {
            return PascalABCCompiler.StringResources.Get("EXPR_VALUE_INCOMPATIBLE_TYPES_IN_EXPRESSION");
        }
    }

    public class VariableExpectedTypeFound : WatchEvaluationError
    {
        private string name;

        public VariableExpectedTypeFound(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return string.Format(PascalABCCompiler.StringResources.Get("EXPR_VALUE_VARIABLE_EXPECTED_TYPE_{0}_FOUND"), name);
        }
    }

    public class NoSuitableMethod : WatchEvaluationError
    {
        public override string ToString()
        {
            return string.Format(PascalABCCompiler.StringResources.Get("EXPR_VALUE_NO_SUITABLE_METHOD"));
        }
    }

    public class NoOperatorForThisType : WatchEvaluationError
    {
        private string op;
        public NoOperatorForThisType(string op)
        {
            this.op = op;
        }

        public override string ToString()
        {
            return string.Format(PascalABCCompiler.StringResources.Get("EXPR_VALUE_OPERATOR_{0}_NOT_IN_TYPE"), op);
        }
    }

    public class WrongNumberArguments : WatchEvaluationError
    {
        private string name;

        public WrongNumberArguments(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return string.Format(PascalABCCompiler.StringResources.Get("EXPR_VALUE_WRONG_NUMBER_ARGUMENTS_IN_{0}"), name);
        }
    }

    public class WrongTypeOfArgument : WatchEvaluationError
    {
        private string name;

        public WrongTypeOfArgument(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return string.Format(PascalABCCompiler.StringResources.Get("EXPR_VALUE_WRONG_TYPE_OF_ARGUMENT_IN_{0}"), name);
        }
    }

    public class WrongTypeInIndexer : WatchEvaluationError
    {
        public override string ToString()
        {
            return PascalABCCompiler.StringResources.Get("EXPR_VALUE_WRONG_TYPE_IN_INDEXER");
        }
    }

    public class WrongIndexersNumber : WatchEvaluationError
    {
        public override string ToString()
        {
            return PascalABCCompiler.StringResources.Get("EXPR_VALUE_WRONG_INDEXERS_NUMBER");
        }
    }

    public class NoIndexerProperty : WatchEvaluationError
    {
        public override string ToString()
        {
            return PascalABCCompiler.StringResources.Get("EXPR_VALUE_NO_INDEXER_PROPERTY");
        }
    }

    public class NameIsNotStatic : WatchEvaluationError
    {
        private string name;

        public NameIsNotStatic(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return string.Format(PascalABCCompiler.StringResources.Get("EXPR_VALUE_NAME_{0}_IS_NOT_STATIC"), name);
        }
    }

    public class InvalidRangesInDiapason : WatchEvaluationError
    {
        public override string ToString()
        {
            return PascalABCCompiler.StringResources.Get("EXPR_VALUE_LOWER_BOUND_GREATER_THAN_UPPER_BOUND");
        }
    }

    public class InvalidExpressionTypeInDiapason : WatchEvaluationError
    {
        public override string ToString()
        {
            return PascalABCCompiler.StringResources.Get("EXPR_VALUE_INVALID_EXPRESSION_TYPE_IN_DIAPASON");
        }
    }

    public class UnknownName : WatchEvaluationError
    {
        private string name;

        public UnknownName(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return string.Format(PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNKNOWN_NAME_{0}"), name);
        }
    }

    /// <summary>
    /// Класс для вычисления выражений при отладке (WATCH)
    /// </summary>
    public class ExpressionEvaluator : PascalABCCompiler.SyntaxTree.AbstractVisitor
    {
        private Stack<RetValue> eval_stack = new Stack<RetValue>();
        private RetValue ret_val;
        private Debugger.Process debuggedProcess;
        private IVisualEnvironmentCompiler vec;
        private string FileName;
        private bool for_immediate = false;

        public ExpressionEvaluator(Debugger.Process debuggedProcess, IVisualEnvironmentCompiler vec, string FileName)
        {
            this.debuggedProcess = debuggedProcess;
            this.vec = vec;
            this.FileName = FileName;
        }

        /// <summary>
        /// Вычисляет значение выражения expr
        /// </summary>
        [HandleProcessCorruptedStateExceptionsAttribute]
        public RetValue Evaluate(string expr, bool for_immediate=false)
        {
            //VisualEnvironmentCompilerCompiler.ParsersController.GetExpression
            declaringType = null;
            names.Clear();
            ret_val = default(RetValue);
            this.for_immediate = for_immediate;
            string fileName = "test" + System.IO.Path.GetExtension(this.FileName);
            List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
            List<PascalABCCompiler.Errors.CompilerWarning> Warnings = new List<PascalABCCompiler.Errors.CompilerWarning>();
            syntax_tree_node e = null;

            BaseParser parser = Languages.Facade.LanguageProvider.Instance.SelectLanguageByExtension(fileName).Parser;

            if (for_immediate)
            {
                e = parser.GetExpression(fileName, expr, Errors, Warnings);
                if (e == null)
                {
                    Errors.Clear();
                    e = parser.GetStatement(fileName, expr, Errors, Warnings);
                }
            }
            else
                e = parser.GetExpression(fileName, expr, Errors, Warnings);
            RetValue res = new RetValue(); res.syn_err = false;
            try
            {
                if (e != null && Errors.Count == 0)
                {
                    e.visit(this);
                    if (eval_stack.Count > 0)
                    {
                        res = eval_stack.Pop();
                        if (res.prim_val == null && res.obj_val != null)
                            if (res.obj_val.IsObject && res.obj_val.Type.IsValueType)
                            {
                                Value tmp = res.obj_val;
                                res.obj_val = DebugUtils.unbox(res.obj_val);
                                if (res.obj_val != null && res.obj_val.IsPrimitive)
                                {
                                    res.prim_val = res.obj_val.PrimitiveValue;
                                    res.obj_val = null;
                                }
                                else
                                    res.obj_val = tmp;
                            }
                    }
                }
                else if (Errors.Count > 0)
                {
                    res.syn_err = true;
                }
            }
            catch (WatchEvaluationError er)
            {
                res.err_mes = er.ToString();
            }
            catch (System.Exception ex)
            {
                //throw new System.Exception(ex.Message);
            }
            finally
            {
                if (eval_stack.Count > 0) eval_stack.Clear();

            }
            return res;
        }

        private bool need_declaring_type;

        public DebugType declaringType;

        private List<string> names = new List<string>();
        [HandleProcessCorruptedStateExceptionsAttribute]
        public bool IsEqual(RetValue left, RetValue right)
        {
            try
            {
                if (left.obj_val == null && left.prim_val == null)
                    return false;
                if (right.obj_val == null && right.prim_val == null)
                    return false;
                eval_stack.Push(left);
                eval_stack.Push(right);
                EvalEqual();
                RetValue rv = eval_stack.Pop();
                if (rv.prim_val != null && rv.prim_val is bool)
                {
                    return (bool)rv.prim_val == true;
                }
                else if (rv.obj_val != null && rv.obj_val.IsPrimitive && rv.obj_val.PrimitiveValue is bool)
                {
                    return (bool)rv.obj_val.PrimitiveValue == true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }
        [HandleProcessCorruptedStateExceptionsAttribute]
        public RetValue GetValueForExpression(string expr, out string preformat)
        {
            declaringType = null;
            for_immediate = false;
            ret_val = default(RetValue);
            preformat = expr.Trim(' ', '\n', '\r', '\t');
            names.Clear();
            string fileName = "test" + System.IO.Path.GetExtension(this.FileName);
            List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
            expression e = Languages.Facade.LanguageProvider.Instance.SelectLanguageByExtension(fileName).Parser.GetExpression(fileName, expr, Errors, new List<PascalABCCompiler.Errors.CompilerWarning>());
            RetValue res = new RetValue(); res.syn_err = false;
            try
            {
                if (e != null && Errors.Count == 0)
                {
                    e.visit(this);
                    if (eval_stack.Count > 0)
                    {
                        res = eval_stack.Pop();
                        preformat = string.Join("", names.ToArray());
                        return res;
                    }
                }
            }
            catch (System.Exception ex)
            {
                //throw new System.Exception(ex.Message);
            }
            finally
            {
                if (eval_stack.Count > 0) eval_stack.Clear();
            }
            return new RetValue();
        }

        public override void visit(template_type_name node)
        {
            throw new NotImplementedException();
        }

        public override void visit(syntax_tree_node _syntax_tree_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(token_taginfo node)
        {
        }

        public override void visit(statement_list _statement_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(default_operator node)
        {
            throw new NotImplementedException();
        }

        public override void visit(expression _expression)
        {
            throw new NotImplementedException();
        }

        public override void visit(assign _assign)
        {
            _assign.from.visit(this);
            if (ret_val.prim_val == null && ret_val.obj_val == null && eval_stack.Count > 0)
            {
                ret_val = eval_stack.Peek();
            }
            if (_assign.to is ident)
            {
                Value nv = GetValue((_assign.to as ident).name);
                if (ret_val.prim_val != null)
                    nv.SetValue(DebugUtils.MakeValue(ret_val.prim_val));
                else if (ret_val.obj_val != null)
                    nv.SetValue(ret_val.obj_val);
            }
        }

        public void EvalPlus()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val + (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val + (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (int)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val + (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val + (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val + (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val + (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val + (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (int)left.prim_val + (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (int)left.prim_val + (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (double)left.prim_val + (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (double)left.prim_val + (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (double)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (double)left.prim_val + (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (double)left.prim_val + (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (double)left.prim_val + (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (double)left.prim_val + (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (double)left.prim_val + (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (double)left.prim_val + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (double)left.prim_val + (System.Single)right.prim_val; break;
                                //case TypeCode.Decimal : res.prim_val = (double)left.prim_val + (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val + (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val + (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (byte)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val + (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val + (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val + (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val + (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val + (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (byte)left.prim_val + (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (byte)left.prim_val + (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val + (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val + (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int16)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val + (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val + (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val + (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val + (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val + (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.Int16)left.prim_val + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int16)left.prim_val + (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (System.Int16)left.prim_val + (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val + (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val + (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int64)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val + (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val + (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val + (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val + (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val + (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int64)left.prim_val + (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (System.Int64)left.prim_val + (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.String:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                case TypeCode.Int32: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                case TypeCode.Double: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                case TypeCode.Int64: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                case TypeCode.String: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                case TypeCode.Int16: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                case TypeCode.SByte: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                case TypeCode.UInt16: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                case TypeCode.UInt32: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                case TypeCode.UInt64: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                case TypeCode.Single: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                case TypeCode.Char: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                case TypeCode.Decimal: res.prim_val = (string)left.prim_val + right.prim_val.ToString(); break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val + (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val + (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.SByte)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val + (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val + (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val + (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val + (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val + (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.SByte)left.prim_val + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.SByte)left.prim_val + (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (System.SByte)left.prim_val + (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val + (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val + (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt16)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val + (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val + (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val + (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val + (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val + (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt16)left.prim_val + (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (System.UInt16)left.prim_val + (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val + (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val + (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt32)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val + (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val + (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val + (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val + (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val + (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt32)left.prim_val + (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (System.UInt32)left.prim_val + (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val + (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val + (System.UInt64)((int)right.prim_val); break;
                                case TypeCode.Double: res.prim_val = (System.UInt64)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt64)left.prim_val + (System.UInt64)((long)right.prim_val); break;
                                case TypeCode.String: res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val + (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val + (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val + (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val + (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt64)left.prim_val + (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (System.UInt64)left.prim_val + (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (decimal)left.prim_val + (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (decimal)left.prim_val + (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (decimal)left.prim_val + (System.UInt64)((long)right.prim_val); break;
                                case TypeCode.String: res.prim_val = ((decimal)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (decimal)left.prim_val + (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (decimal)left.prim_val + (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (decimal)left.prim_val + (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (decimal)left.prim_val + (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (decimal)left.prim_val + (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val + (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (decimal)left.prim_val + (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Single:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Single)left.prim_val + (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Single)left.prim_val + (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Single)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Single)left.prim_val + (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Single)left.prim_val + (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Single)left.prim_val + (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Single)left.prim_val + (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Single)left.prim_val + (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Single)left.prim_val + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Single)left.prim_val + (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Char:
                        {
                            switch (rcode)
                            {
                                case TypeCode.String: res.prim_val = left.prim_val.ToString() + (string)right.prim_val; break;
                                case TypeCode.Char: res.prim_val = (string)left.prim_val + (string)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Object:
                        {
                            if (left.obj_val.Type.FullName == "PABCSystem.TypedSet" && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
                            {
                                res.obj_val = (left.obj_val.Type.GetMember("UnionSet", BindingFlags.All)[0] as MethodInfo).Invoke(left.obj_val, new Value[1] { right.obj_val });
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("+");
                }

            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                if (left.obj_val.Type.FullName == "PABCSystem.TypedSet" && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
                {
                    res.obj_val = (left.obj_val.Type.GetMember("UnionSet", BindingFlags.All)[0] as MethodInfo).Invoke(left.obj_val, new Value[1] { right.obj_val });
                }
                else
                {
                    res.obj_val = EvalCommonOperation(left.obj_val, right.obj_val, PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.plus_name), "+");
                }
            }
            eval_stack.Push(res);
        }

        private Value EvalCommonOperation(Value left, Value right, string op, string sn)
        {
            if (left.Type != right.Type)
            {
                Type left_type = AssemblyHelper.GetType(left.Type.FullName);
                Type right_type = AssemblyHelper.GetType(right.Type.FullName);
                System.Reflection.MethodInfo conv_meth = left_type.GetMethod("op_Implicit", new Type[1] { right_type });
                IList<MethodInfo> meths = null;
                if (conv_meth != null)
                {
                    meths = left.Type.GetMethods(BindingFlags.All);
                    foreach (MethodInfo mi in meths)
                        if (mi.MetadataToken == conv_meth.MetadataToken)
                        {
                            right = mi.Invoke(null, new Value[1] { right });
                            break;
                        }
                }
                else
                {
                    conv_meth = right_type.GetMethod("op_Implicit", new Type[1] { left_type });
                    if (conv_meth != null)
                    {
                        meths = right.Type.GetMethods(BindingFlags.All);
                        foreach (MethodInfo mi in meths)
                            if (mi.MetadataToken == conv_meth.MetadataToken)
                            {
                                left = mi.Invoke(null, new Value[1] { left });
                                break;
                            }
                    }
                }
            }
            if (left.Type == right.Type)
            {
                IList<MemberInfo> mems = left.Type.GetMember(op, Debugger.BindingFlags.All);
                if (mems != null && mems.Count == 1 && mems[0] is MethodInfo)
                {
                    MethodInfo mi = mems[0] as MethodInfo;
                    return mi.Invoke(null, new Value[2] { left, right });
                }
                else
                    throw new NoOperatorForThisType(sn);
            }
            else
                throw new NoOperatorForThisType(sn);
        }

        private void EvalMinus()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val - (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val - (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (int)left.prim_val - (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val - (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val - (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val - (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val - (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val - (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) - (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (int)left.prim_val - (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (double)left.prim_val - (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (double)left.prim_val - (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (double)left.prim_val - (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (double)left.prim_val - (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (double)left.prim_val - (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (double)left.prim_val - (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (double)left.prim_val - (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (double)left.prim_val - (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (double)left.prim_val - (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (double)left.prim_val - (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val - (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val - (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (byte)left.prim_val - (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val - (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val - (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val - (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val - (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val - (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val - (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (byte)left.prim_val - (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val - (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val - (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int16)left.prim_val - (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val - (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val - (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val - (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val - (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val - (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) - (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int16)left.prim_val - (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val - (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val - (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int64)left.prim_val - (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val - (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val - (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val - (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val - (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val - (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val - (System.Int64)((System.UInt64)right.prim_val); break;
                                case TypeCode.Single: res.prim_val = (System.Int64)left.prim_val - (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val - (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val - (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.SByte)left.prim_val - (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val - (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val - (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val - (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val - (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val - (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) - (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.SByte)left.prim_val - (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val - (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val - (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt16)left.prim_val - (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val - (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val - (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val - (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val - (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val - (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val - (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt16)left.prim_val - (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val - (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val - (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt32)left.prim_val - (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val - (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val - (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val - (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val - (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val - (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val - (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt32)left.prim_val - (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val - (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val - (System.UInt64)((int)right.prim_val); break;
                                case TypeCode.Double: res.prim_val = (System.UInt64)left.prim_val - (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) - (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val - (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val - (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val - (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val - (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val - (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt64)left.prim_val - (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (decimal)left.prim_val - (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (decimal)left.prim_val - (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (decimal)left.prim_val - (System.UInt64)((long)right.prim_val); break;
                                //case TypeCode.String : res.prim_val = ((decimal)left.prim_val).ToString() - (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (decimal)left.prim_val - (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (decimal)left.prim_val - (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (decimal)left.prim_val - (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (decimal)left.prim_val - (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (decimal)left.prim_val - (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val + (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (decimal)left.prim_val - (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Single:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Single)left.prim_val - (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Single)left.prim_val - (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Single)left.prim_val - (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Single)left.prim_val - (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Single)left.prim_val - (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Single)left.prim_val - (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Single)left.prim_val - (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Single)left.prim_val - (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Single)left.prim_val - (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Single)left.prim_val - (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Object:
                        {
                            if (left.obj_val.Type.FullName == "PABCSystem.TypedSet" && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
                            {
                                res.obj_val = (left.obj_val.Type.GetMember("SubtractSet", BindingFlags.All)[0] as MethodInfo).Invoke(left.obj_val, new Value[1] { right.obj_val });
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("-");
                }

            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                if (left.obj_val.Type.FullName == "PABCSystem.TypedSet" && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
                {
                    res.obj_val = (left.obj_val.Type.GetMember("SubtractSet", BindingFlags.All)[0] as MethodInfo).Invoke(left.obj_val, new Value[1] { right.obj_val });
                }
                else
                {
                    res.obj_val = EvalCommonOperation(left.obj_val, right.obj_val, PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.minus_name), "-");
                }
            }
            eval_stack.Push(res);
        }

        private void EvalMult()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (int)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (int)left.prim_val * (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (double)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (double)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (double)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (double)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (double)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (double)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (double)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (double)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (double)left.prim_val * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (double)left.prim_val * (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (byte)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (byte)left.prim_val * (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int16)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int16)left.prim_val * (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int64)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val * (System.Int64)((System.UInt64)right.prim_val); break;
                                case TypeCode.Single: res.prim_val = (System.Int64)left.prim_val * (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.SByte)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.SByte)left.prim_val * (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt16)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt16)left.prim_val * (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt32)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt32)left.prim_val * (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val * (System.UInt64)((int)right.prim_val); break;
                                case TypeCode.Double: res.prim_val = (System.UInt64)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val * (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val * (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt64)left.prim_val * (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (decimal)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (decimal)left.prim_val * (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (decimal)left.prim_val * (System.UInt64)((long)right.prim_val); break;
                                //case TypeCode.String : res.prim_val = ((decimal)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (decimal)left.prim_val * (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (decimal)left.prim_val * (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (decimal)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (decimal)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (decimal)left.prim_val * (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val + (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (decimal)left.prim_val * (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Single:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Single)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Single)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Single)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Single)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Single)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Single)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Single)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Single)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Single)left.prim_val * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Single)left.prim_val * (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Object:
                        {
                            if (left.obj_val.Type.FullName == "PABCSystem.TypedSet" && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
                            {
                                res.obj_val = (left.obj_val.Type.GetMember("IntersectSet", BindingFlags.All)[0] as MethodInfo).Invoke(left.obj_val, new Value[1] { right.obj_val });
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("*");
                }

            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                if (left.obj_val.Type.FullName == "PABCSystem.TypedSet" && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
                {
                    res.obj_val = (left.obj_val.Type.GetMember("IntersectSet", BindingFlags.All)[0] as MethodInfo).Invoke(left.obj_val, new Value[1] { right.obj_val });
                }
                else
                {
                    res.obj_val = EvalCommonOperation(left.obj_val, right.obj_val, PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.mul_name), "*");
                }
            }
            eval_stack.Push(res);
        }

        private void EvalDiv()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (double)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (double)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (double)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (double)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (double)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (double)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (double)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (double)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (double)left.prim_val / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (double)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val / (System.Int64)((System.UInt64)right.prim_val); break;
                                case TypeCode.Single: res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val / (System.UInt64)((int)right.prim_val); break;
                                case TypeCode.Double: res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val / (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val / (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (decimal)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (decimal)left.prim_val / (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val + (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (decimal)left.prim_val / (System.UInt64)((long)right.prim_val); break;
                                //case TypeCode.String : res.prim_val = ((decimal)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (decimal)left.prim_val / (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (decimal)left.prim_val / (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (decimal)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (decimal)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (decimal)left.prim_val / (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val + (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (decimal)left.prim_val / (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Single:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Single)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Single)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Single)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Single)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Single)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Single)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Single)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Single)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Single)left.prim_val / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Single)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("/");
                }


            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                res.obj_val = EvalCommonOperation(left.obj_val, right.obj_val, PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.div_name), "/");
            }
            eval_stack.Push(res);
        }

        private void EvalIDiv()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val / (System.Int64)((System.UInt64)right.prim_val); break;
                                case TypeCode.Single: res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val / (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val / (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val / (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val / (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val / (System.UInt64)((int)right.prim_val); break;
                                case TypeCode.Double: res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) / (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val / (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val / (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val / (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val / (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val / (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("div");
                }
            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                res.obj_val = EvalCommonOperation(left.obj_val, right.obj_val, PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.idiv_name), "div");
            }
            eval_stack.Push(res);
        }

        private void EvalAnd()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val & (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val & (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val & (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val & (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val & (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val & (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val & (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) & (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val & (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val & (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val & (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val & (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val & (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val & (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val & (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val & (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val & (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val & (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val & (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val & (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val & (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val & (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val & (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) & (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val & (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val & (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val & (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val & (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val & (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val & (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val & (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val & (System.Int64)((System.UInt64)right.prim_val); break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val & (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val & (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val & (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val & (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val & (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val & (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val & (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) & (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val & (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val & (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val & (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val & (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val & (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val & (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val & (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val & (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val & (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val & (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val & (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val & (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val & (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val & (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val & (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val & (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val & (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val & (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) & (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val & (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val & (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val & (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val & (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val & (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Boolean:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Boolean: res.prim_val = (bool)left.prim_val && (bool)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("and");
                }
                eval_stack.Push(res);
            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                res.obj_val = EvalCommonOperation(left.obj_val, right.obj_val, PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.and_name), "and");
            }
        }

        private void EvalOr()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val | (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val | (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((int)left.prim_val) | (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val | (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val | (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val | (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val | (int)((System.UInt32)right.prim_val); break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) | (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val | (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val | (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val | (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val | (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val | (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val | (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val | (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val | (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val | (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val | (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val | (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val | (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val | (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val | (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val | (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) | (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val | (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val | (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val | (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val | (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val | (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val | (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val | (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val | (System.Int64)((System.UInt64)right.prim_val); break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val | (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val | (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val | (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val | (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val | (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val | (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val | (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) | (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val | (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val | (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val | (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val | (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val | (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val | (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val | (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val | (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val | (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val | (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val | (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val | (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val | (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val | (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val | (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val | (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val | (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val | (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) | (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val | (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val | (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val | (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val | (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val | (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Boolean:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Boolean: res.prim_val = (bool)left.prim_val || (bool)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("or");
                }
                eval_stack.Push(res);
            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                res.obj_val = EvalCommonOperation(left.obj_val, right.obj_val, PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.plus_name), "-");
            }
        }

        private void EvalXor()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null && right.prim_val != null)
            {

                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val ^ (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val ^ (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val ^ (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val ^ (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val ^ (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val ^ (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val ^ (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) ^ (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val ^ (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val ^ (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val ^ (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val ^ (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val ^ (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val ^ (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val ^ (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val ^ (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val ^ (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val ^ (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val ^ (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val ^ (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val ^ (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val ^ (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val ^ (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) ^ (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val ^ (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val ^ (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val ^ (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val ^ (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val ^ (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val ^ (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val ^ (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val ^ (System.Int64)((System.UInt64)right.prim_val); break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val ^ (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val ^ (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val ^ (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val ^ (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val ^ (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val ^ (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val ^ (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) ^ (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val ^ (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val ^ (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val ^ (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val ^ (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val ^ (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val ^ (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val ^ (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val ^ (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val ^ (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val ^ (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val ^ (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val ^ (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val ^ (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val ^ (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val ^ (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val ^ (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val ^ (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val ^ (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) ^ (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val ^ (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val ^ (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val ^ (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val ^ (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val ^ (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("xor");
                }
                eval_stack.Push(res);
            }
            else throw new NoOperatorForThisType("xor");
        }

        private void EvalBitwiseLeft()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val << (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val << (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (long)((int)left.prim_val) << (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val << (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val << (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val << (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val << (int)((System.UInt32)right.prim_val); break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) << (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val << (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val << (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (long)((byte)left.prim_val) << (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val << (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val << (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val << (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (System.UInt32)((byte)left.prim_val) << (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.UInt64)((byte)left.prim_val) << (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val << (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val << (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (long)((System.Int16)left.prim_val) << (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val << (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val << (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val << (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (System.UInt32)((System.Int16)left.prim_val) << (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.Int16)left.prim_val) << (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val << (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val << (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (long)((System.Int64)left.prim_val) << (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val << (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val << (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val << (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val << (System.Int64)((System.UInt32)right.prim_val); break;
                                //case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val << (System.Int64)((System.UInt64)right.prim_val); break;
                                //case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val << (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val << (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val << (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val << (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val << (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val << (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (System.UInt32)((System.SByte)left.prim_val) << (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.SByte)left.prim_val) << (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val << (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val << (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val << (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val << (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val << (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val << (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val << (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val << (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val << (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val << (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val << (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val << (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val << (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val << (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val << (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val << (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val << (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val << (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (System.UInt64)left.prim_val << (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val << (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val << (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val << (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val << (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val << (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("shl");
                }

                eval_stack.Push(res);
            }
            else throw new NoOperatorForThisType("shl");
        }

        private void EvalBitwiseRight()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val >> (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val >> (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (int)left.prim_val >> (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val >> (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val >> (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val >> (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (int)left.prim_val >> (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (int)left.prim_val >> (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val >> (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val >> (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (byte)left.prim_val >> (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val >> (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val >> (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val >> (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val >> (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val >> (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val >> (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val >> (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val >> (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val >> (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val >> (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val >> (System.UInt16)right.prim_val; break;
                                //case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val >> (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.Int16)left.prim_val >> (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val >> (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val >> (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val >> (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val >> (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val >> (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val >> (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val >> (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val >> (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val >> (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val >> (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val >> (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val >> (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val >> (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val >> (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val >> (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.SByte)left.prim_val >> (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val >> (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val >> (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val >> (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val >> (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val >> (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val >> (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val >> (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val >> (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val >> (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val >> (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val >> (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val >> (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val >> (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val >> (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val >> (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val >> (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val >> (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val >> (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
                                //case TypeCode.Int64 : res.prim_val = (System.UInt64)left.prim_val >> (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val >> (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val >> (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val >> (System.UInt16)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val >> (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val >> (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("shr");
                }

                eval_stack.Push(res);
            }
            else throw new NoOperatorForThisType("shr");
        }

        private void EvalEqual()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
                else
                    left.prim_val = DebugUtils.GetEnumValue(left.obj_val);
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
                else
                    right.prim_val = DebugUtils.GetEnumValue(right.obj_val);
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val == (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val == (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (int)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val == (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((int)left.prim_val).ToString() == (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val == (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val == (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val == (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val == (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) == (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (int)left.prim_val == (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (double)left.prim_val == (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (double)left.prim_val == (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (double)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (double)left.prim_val == (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((double)left.prim_val).ToString() == (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (double)left.prim_val == (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (double)left.prim_val == (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (double)left.prim_val == (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (double)left.prim_val == (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (double)left.prim_val == (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (double)left.prim_val == (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val == (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val == (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (byte)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val == (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((byte)left.prim_val).ToString() == (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val == (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val == (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val == (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val == (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val == (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (byte)left.prim_val == (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val == (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val == (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int16)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val == (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.Int16)left.prim_val).ToString() == (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val == (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val == (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val == (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val == (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.Int16)left.prim_val + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int16)left.prim_val == (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val == (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val == (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int64)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val == (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.Int64)left.prim_val).ToString() == (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val == (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val == (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val == (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val == (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int64)left.prim_val == (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.String:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
                                case TypeCode.Int32: res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
                                case TypeCode.Double: res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
                                case TypeCode.Int64: res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
                                case TypeCode.String: res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
                                case TypeCode.Int16: res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
                                case TypeCode.SByte: res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
                                case TypeCode.UInt16: res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
                                case TypeCode.UInt32: res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
                                case TypeCode.UInt64: res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
                                case TypeCode.Single: res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
                                case TypeCode.Char: res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val == (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val == (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.SByte)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val == (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.SByte)left.prim_val).ToString() == (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val == (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val == (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val == (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val == (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.SByte)left.prim_val + (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.SByte)left.prim_val == (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val == (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val == (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt16)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val == (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.UInt16)left.prim_val).ToString() == (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val == (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val == (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val == (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val == (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val == (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt16)left.prim_val == (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val == (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val == (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt32)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val == (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.UInt32)left.prim_val).ToString() == (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val == (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val == (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val == (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val == (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val == (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt32)left.prim_val == (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val == (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val == (System.UInt64)((int)right.prim_val); break;
                                case TypeCode.Double: res.prim_val = (System.UInt64)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt64)left.prim_val == (System.UInt64)((long)right.prim_val); break;
                                case TypeCode.String: res.prim_val = ((System.UInt64)left.prim_val).ToString() == (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val == (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val == (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val == (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val == (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val == (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt64)left.prim_val == (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (decimal)left.prim_val == (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (decimal)left.prim_val == (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (decimal)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (decimal)left.prim_val == (System.UInt64)((long)right.prim_val); break;
                                //case TypeCode.String : res.prim_val = ((decimal)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (decimal)left.prim_val == (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (decimal)left.prim_val == (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (decimal)left.prim_val == (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (decimal)left.prim_val == (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (decimal)left.prim_val == (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (decimal)left.prim_val == (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (decimal)left.prim_val == (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Single:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Single)left.prim_val == (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Single)left.prim_val == (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Single)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Single)left.prim_val == (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.Single)left.prim_val).ToString() == (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Single)left.prim_val == (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Single)left.prim_val == (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Single)left.prim_val == (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Single)left.prim_val == (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Single)left.prim_val == (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Single)left.prim_val == (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Boolean:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Boolean: res.prim_val = (bool)left.prim_val == (bool)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Char:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Char: res.prim_val = (char)left.prim_val == (char)right.prim_val; break;
                                case TypeCode.String: res.prim_val = left.prim_val.ToString() == right.prim_val.ToString(); break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("=");
                }

            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                if (left.is_null)
                {
                    if (right.is_null)
                        res.prim_val = true;
                    else
                        res.prim_val = right.obj_val.IsNull;
                }
                else if (right.is_null)
                {
                    res.prim_val = left.obj_val.IsNull;
                }
                else
                    if (left.obj_val.Type.FullName == "PABCSystem.TypedSet" && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
                    {
                        res.obj_val = (left.obj_val.Type.GetMember("CompareEquals", BindingFlags.All)[0] as MethodInfo).Invoke(left.obj_val, new Value[1] { right.obj_val });
                    }
                    else
                    {
                        string op = PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.eq_name);
                        if (left.obj_val.Type != right.obj_val.Type)
                        {
                            Type left_type = AssemblyHelper.GetType(left.obj_val.Type.FullName);
                            Type right_type = AssemblyHelper.GetType(right.obj_val.Type.FullName);
                            System.Reflection.MethodInfo conv_meth = left_type.GetMethod("op_Implicit", new Type[1] { right_type });
                            IList<MethodInfo> meths = null;
                            if (conv_meth != null)
                            {
                                meths = left.obj_val.Type.GetMethods(BindingFlags.All);
                                foreach (MethodInfo mi in meths)
                                    if (mi.MetadataToken == conv_meth.MetadataToken)
                                    {
                                        right.obj_val = mi.Invoke(null, new Value[1] { right.obj_val });
                                        break;
                                    }
                            }
                            else
                            {
                                conv_meth = right_type.GetMethod("op_Implicit", new Type[1] { left_type });
                                if (conv_meth != null)
                                {
                                    meths = right.obj_val.Type.GetMethods(BindingFlags.All);
                                    foreach (MethodInfo mi in meths)
                                        if (mi.MetadataToken == conv_meth.MetadataToken)
                                        {
                                            left.obj_val = mi.Invoke(null, new Value[1] { left.obj_val });
                                            break;
                                        }
                                }
                            }
                        }
                        if (left.obj_val.Type == right.obj_val.Type)
                        {
                            IList<MemberInfo> mems = left.obj_val.Type.GetMember(op, Debugger.BindingFlags.All);
                            if (mems != null && mems.Count == 1 && mems[0] is MethodInfo)
                            {
                                MethodInfo mi = mems[0] as MethodInfo;
                                res.obj_val = mi.Invoke(null, new Value[2] { left.obj_val, right.obj_val });
                            }
                            else
                                res.prim_val = left.obj_val.IsReferenceEqual(right.obj_val);
                        }
                        else
                            res.prim_val = left.obj_val.IsReferenceEqual(right.obj_val);
                    }
            }
            eval_stack.Push(res);
        }

        private void EvalNotEqual()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
                else
                    left.prim_val = DebugUtils.GetEnumValue(left.obj_val);
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
                else
                    right.prim_val = DebugUtils.GetEnumValue(right.obj_val);
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val != (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val != (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (int)left.prim_val != (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val != (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((int)left.prim_val).ToString() != (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val != (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val != (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val != (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val != (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) != (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (int)left.prim_val != (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (double)left.prim_val != (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (double)left.prim_val != (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (double)left.prim_val != (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (double)left.prim_val != (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((double)left.prim_val).ToString() != (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (double)left.prim_val != (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (double)left.prim_val != (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (double)left.prim_val != (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (double)left.prim_val != (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (double)left.prim_val != (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (double)left.prim_val != (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val != (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val != (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (byte)left.prim_val != (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val != (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((byte)left.prim_val).ToString() != (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val != (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val != (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val != (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val != (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val != (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (byte)left.prim_val != (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val != (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val != (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int16)left.prim_val != (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val != (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.Int16)left.prim_val).ToString() != (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val != (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val != (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val != (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val != (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.Int16)left.prim_val != (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int16)left.prim_val != (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val != (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val != (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int64)left.prim_val != (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val != (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.Int64)left.prim_val).ToString() != (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val != (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val != (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val != (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val != (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val != (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int64)left.prim_val != (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.String:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
                                case TypeCode.Int32: res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
                                case TypeCode.Double: res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
                                case TypeCode.Int64: res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
                                case TypeCode.String: res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
                                case TypeCode.Int16: res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
                                case TypeCode.SByte: res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
                                case TypeCode.UInt16: res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
                                case TypeCode.UInt32: res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
                                case TypeCode.UInt64: res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
                                case TypeCode.Single: res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
                                case TypeCode.Char: res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val != (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val != (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.SByte)left.prim_val != (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val != (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.SByte)left.prim_val).ToString() != (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val != (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val != (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val != (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val != (System.UInt32)right.prim_val; break;
                                //case TypeCode.UInt64 : res.prim_val = (System.SByte)left.prim_val != (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.SByte)left.prim_val != (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val != (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val != (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt16)left.prim_val != (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val != (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.UInt16)left.prim_val).ToString() != (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val != (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val != (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val != (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val != (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val != (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt16)left.prim_val != (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val != (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val != (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt32)left.prim_val != (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val != (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.UInt32)left.prim_val).ToString() != (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val != (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val != (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val != (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val != (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val != (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt32)left.prim_val != (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val != (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val != (System.UInt64)((int)right.prim_val); break;
                                case TypeCode.Double: res.prim_val = (System.UInt64)left.prim_val != (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt64)left.prim_val != (System.UInt64)((long)right.prim_val); break;
                                case TypeCode.String: res.prim_val = ((System.UInt64)left.prim_val).ToString() != (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val != (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val != (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val != (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val != (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val != (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt64)left.prim_val != (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (decimal)left.prim_val != (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (decimal)left.prim_val != (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (decimal)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (decimal)left.prim_val != (System.UInt64)((long)right.prim_val); break;
                                //case TypeCode.String : res.prim_val = ((decimal)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (decimal)left.prim_val != (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (decimal)left.prim_val != (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (decimal)left.prim_val != (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (decimal)left.prim_val != (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (decimal)left.prim_val != (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (decimal)left.prim_val == (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (decimal)left.prim_val != (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Single:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Single)left.prim_val != (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Single)left.prim_val != (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Single)left.prim_val != (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Single)left.prim_val != (long)right.prim_val; break;
                                case TypeCode.String: res.prim_val = ((System.Single)left.prim_val).ToString() != (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Single)left.prim_val != (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Single)left.prim_val != (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Single)left.prim_val != (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Single)left.prim_val != (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Single)left.prim_val != (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Single)left.prim_val != (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Boolean:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Boolean: res.prim_val = (bool)left.prim_val != (bool)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Char:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Char: res.prim_val = (char)left.prim_val != (char)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("<>");

                }

            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                if (left.is_null)
                {
                    if (right.is_null)
                        res.prim_val = false;
                    else
                        res.prim_val = !right.obj_val.IsNull;
                }
                else if (right.is_null)
                {
                    res.prim_val = !left.obj_val.IsNull;
                }
                else
                    if (left.obj_val.Type.FullName == "PABCSystem.TypedSet" && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
                    {
                        res.obj_val = (left.obj_val.Type.GetMember("CompareInEquals", BindingFlags.All)[0] as MethodInfo).Invoke(left.obj_val, new Value[1] { right.obj_val });
                    }
                    else
                    {
                        string op = PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.noteq_name);
                        if (left.obj_val.Type != right.obj_val.Type)
                        {
                            Type left_type = AssemblyHelper.GetType(left.obj_val.Type.FullName);
                            Type right_type = AssemblyHelper.GetType(right.obj_val.Type.FullName);
                            System.Reflection.MethodInfo conv_meth = left_type.GetMethod("op_Implicit", new Type[1] { right_type });
                            IList<MethodInfo> meths = null;
                            if (conv_meth != null)
                            {
                                meths = left.obj_val.Type.GetMethods(BindingFlags.All);
                                foreach (MethodInfo mi in meths)
                                    if (mi.MetadataToken == conv_meth.MetadataToken)
                                    {
                                        right.obj_val = mi.Invoke(null, new Value[1] { right.obj_val });
                                        break;
                                    }
                            }
                            else
                            {
                                conv_meth = right_type.GetMethod("op_Implicit", new Type[1] { left_type });
                                if (conv_meth != null)
                                {
                                    meths = right.obj_val.Type.GetMethods(BindingFlags.All);
                                    foreach (MethodInfo mi in meths)
                                        if (mi.MetadataToken == conv_meth.MetadataToken)
                                        {
                                            left.obj_val = mi.Invoke(null, new Value[1] { left.obj_val });
                                            break;
                                        }
                                }
                            }
                        }
                        if (left.obj_val.Type == right.obj_val.Type)
                        {
                            IList<MemberInfo> mems = left.obj_val.Type.GetMember(op, Debugger.BindingFlags.All);
                            if (mems != null && mems.Count == 1 && mems[0] is MethodInfo)
                            {
                                MethodInfo mi = mems[0] as MethodInfo;
                                res.obj_val = mi.Invoke(null, new Value[2] { left.obj_val, right.obj_val });
                            }
                            else
                                res.prim_val = !left.obj_val.IsReferenceEqual(right.obj_val);
                        }
                        else
                            res.prim_val = !left.obj_val.IsReferenceEqual(right.obj_val);
                    }
            }
            eval_stack.Push(res);
        }

        private void EvalLess()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
                else
                    left.prim_val = DebugUtils.GetEnumValue(left.obj_val);
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
                else
                    right.prim_val = DebugUtils.GetEnumValue(right.obj_val);
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val < (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val < (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (int)left.prim_val < (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val < (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val < (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val < (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val < (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val < (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) < (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (int)left.prim_val < (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (double)left.prim_val < (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (double)left.prim_val < (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (double)left.prim_val < (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (double)left.prim_val < (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (double)left.prim_val < (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (double)left.prim_val < (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (double)left.prim_val < (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (double)left.prim_val < (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (double)left.prim_val < (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (double)left.prim_val < (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val < (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val < (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (byte)left.prim_val < (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val < (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val < (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val < (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val < (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val < (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val < (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (byte)left.prim_val < (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val < (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val < (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int16)left.prim_val < (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val < (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val < (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val < (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val < (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val < (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) < (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int16)left.prim_val < (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val < (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val < (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int64)left.prim_val < (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val < (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val < (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val < (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val < (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val < (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val < (System.Int64)((System.UInt64)right.prim_val); break;
                                case TypeCode.Single: res.prim_val = (System.Int64)left.prim_val < (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val < (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val < (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.SByte)left.prim_val < (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val < (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val < (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val < (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val < (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val < (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) < (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.SByte)left.prim_val < (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val < (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val < (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt16)left.prim_val < (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val < (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val < (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val < (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val < (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val < (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val < (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt16)left.prim_val < (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val < (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val < (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt32)left.prim_val < (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val < (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val < (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val < (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val < (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val < (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val < (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt32)left.prim_val < (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val < (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val < (System.UInt64)((int)right.prim_val); break;
                                case TypeCode.Double: res.prim_val = (System.UInt64)left.prim_val < (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) < (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val < (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val < (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val < (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val < (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val < (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt64)left.prim_val < (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (decimal)left.prim_val < (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (decimal)left.prim_val < (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (decimal)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (decimal)left.prim_val < (System.UInt64)((long)right.prim_val); break;
                                //case TypeCode.String : res.prim_val = ((decimal)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (decimal)left.prim_val < (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (decimal)left.prim_val < (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (decimal)left.prim_val < (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (decimal)left.prim_val < (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (decimal)left.prim_val < (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (decimal)left.prim_val == (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (decimal)left.prim_val < (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Single:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Single)left.prim_val < (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Single)left.prim_val < (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Single)left.prim_val < (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Single)left.prim_val < (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Single)left.prim_val < (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Single)left.prim_val < (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Single)left.prim_val < (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Single)left.prim_val < (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Single)left.prim_val < (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Single)left.prim_val < (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Boolean:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Boolean: res.prim_val = (int)left.prim_val < (int)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.String:
                        {
                            if (rcode == TypeCode.String)
                                res.prim_val = string.Compare((string)left.prim_val, (string)right.prim_val) < 0;
                            else throw new IncompatibleTypesInExpression();
                        }
                        break;
                    case TypeCode.Char:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Char: res.prim_val = (char)left.prim_val < (char)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("<");
                }

            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                if (left.obj_val.Type.FullName == "PABCSystem.TypedSet" && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
                {
                    res.obj_val = (left.obj_val.Type.GetMember("CompareLess", BindingFlags.All)[0] as MethodInfo).Invoke(left.obj_val, new Value[1] { right.obj_val });
                }
                else
                {
                    res.obj_val = EvalCommonOperation(left.obj_val, right.obj_val, PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.sm_name), "<");
                }
            }
            eval_stack.Push(res);
        }

        private void EvalLessEqual()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
                else
                    left.prim_val = DebugUtils.GetEnumValue(left.obj_val);
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
                else
                    right.prim_val = DebugUtils.GetEnumValue(right.obj_val);
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val <= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val <= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (int)left.prim_val <= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val <= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val <= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val <= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val <= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val <= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) <= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (int)left.prim_val <= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (double)left.prim_val <= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (double)left.prim_val <= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (double)left.prim_val <= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (double)left.prim_val <= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (double)left.prim_val <= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (double)left.prim_val <= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (double)left.prim_val <= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (double)left.prim_val <= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (double)left.prim_val <= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (double)left.prim_val <= (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val <= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val <= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (byte)left.prim_val <= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val <= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val <= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val <= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val <= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val <= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val <= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (byte)left.prim_val <= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val <= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val <= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int16)left.prim_val <= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val <= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val <= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val <= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val <= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val <= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) <= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int16)left.prim_val <= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val <= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val <= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int64)left.prim_val <= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val <= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val <= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val <= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val <= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val <= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val <= (System.Int64)((System.UInt64)right.prim_val); break;
                                case TypeCode.Single: res.prim_val = (System.Int64)left.prim_val <= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val <= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val <= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.SByte)left.prim_val <= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val <= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val <= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val <= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val <= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val <= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) <= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.SByte)left.prim_val <= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val <= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val <= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt16)left.prim_val <= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val <= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val <= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val <= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val <= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val <= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val <= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt16)left.prim_val <= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val <= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val <= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt32)left.prim_val <= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val <= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val <= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val <= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val <= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val <= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val <= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt32)left.prim_val <= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val <= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val <= (System.UInt64)((int)right.prim_val); break;
                                case TypeCode.Double: res.prim_val = (System.UInt64)left.prim_val <= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) <= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val <= (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val <= (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val <= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val <= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val <= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt64)left.prim_val <= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (decimal)left.prim_val <= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (decimal)left.prim_val <= (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (decimal)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (decimal)left.prim_val <= (System.UInt64)((long)right.prim_val); break;
                                //case TypeCode.String : res.prim_val = ((decimal)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (decimal)left.prim_val <= (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (decimal)left.prim_val <= (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (decimal)left.prim_val <= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (decimal)left.prim_val <= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (decimal)left.prim_val <= (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (decimal)left.prim_val == (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (decimal)left.prim_val <= (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Single:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Single)left.prim_val <= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Single)left.prim_val <= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Single)left.prim_val <= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Single)left.prim_val <= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Single)left.prim_val <= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Single)left.prim_val <= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Single)left.prim_val <= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Single)left.prim_val <= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Single)left.prim_val <= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Single)left.prim_val <= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Boolean:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Boolean: res.prim_val = (int)left.prim_val <= (int)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.String:
                        {
                            if (rcode == TypeCode.String)
                                res.prim_val = string.Compare((string)left.prim_val, (string)right.prim_val) <= 0;
                            else throw new IncompatibleTypesInExpression();
                        }
                        break;
                    case TypeCode.Char:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Char: res.prim_val = (char)left.prim_val <= (char)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("<=");
                }

            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                if (left.obj_val.Type.FullName == "PABCSystem.TypedSet" && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
                {
                    res.obj_val = (left.obj_val.Type.GetMember("CompareLessEqual", BindingFlags.All)[0] as MethodInfo).Invoke(left.obj_val, new Value[1] { right.obj_val });
                }
                else
                {
                    res.obj_val = EvalCommonOperation(left.obj_val, right.obj_val, PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.smeq_name), "<=");
                }
            }
            eval_stack.Push(res);
        }

        private void EvalGreater()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
                else
                    left.prim_val = DebugUtils.GetEnumValue(left.obj_val);
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
                else
                    right.prim_val = DebugUtils.GetEnumValue(right.obj_val);
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val > (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val > (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (int)left.prim_val > (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val > (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val > (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val > (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val > (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val > (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) > (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (int)left.prim_val > (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (double)left.prim_val > (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (double)left.prim_val > (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (double)left.prim_val > (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (double)left.prim_val > (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (double)left.prim_val > (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (double)left.prim_val > (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (double)left.prim_val > (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (double)left.prim_val > (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (double)left.prim_val > (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (double)left.prim_val > (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val > (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val > (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (byte)left.prim_val > (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val > (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val > (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val > (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val > (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val > (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val > (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (byte)left.prim_val > (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val > (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val > (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int16)left.prim_val > (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val > (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val > (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val > (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val > (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val > (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) > (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int16)left.prim_val > (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val > (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val > (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int64)left.prim_val > (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val > (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val > (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val > (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val > (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val > (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val > (System.Int64)((System.UInt64)right.prim_val); break;
                                case TypeCode.Single: res.prim_val = (System.Int64)left.prim_val > (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val > (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val > (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.SByte)left.prim_val > (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val > (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val > (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val > (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val > (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val > (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) > (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.SByte)left.prim_val > (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val > (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val > (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt16)left.prim_val > (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val > (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val > (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val > (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val > (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val > (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val > (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt16)left.prim_val > (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val > (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val > (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt32)left.prim_val > (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val > (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val > (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val > (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val > (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val > (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val > (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt32)left.prim_val > (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val > (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val > (System.UInt64)((int)right.prim_val); break;
                                case TypeCode.Double: res.prim_val = (System.UInt64)left.prim_val > (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) > (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val > (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val > (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val > (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val > (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val > (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt64)left.prim_val > (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (decimal)left.prim_val > (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (decimal)left.prim_val > (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (decimal)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (decimal)left.prim_val > (System.UInt64)((long)right.prim_val); break;
                                //case TypeCode.String : res.prim_val = ((decimal)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (decimal)left.prim_val > (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (decimal)left.prim_val > (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (decimal)left.prim_val > (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (decimal)left.prim_val > (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (decimal)left.prim_val > (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (decimal)left.prim_val == (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (decimal)left.prim_val > (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Single:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Single)left.prim_val > (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Single)left.prim_val > (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Single)left.prim_val > (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Single)left.prim_val > (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Single)left.prim_val > (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Single)left.prim_val > (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Single)left.prim_val > (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Single)left.prim_val > (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Single)left.prim_val > (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Single)left.prim_val > (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Boolean:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Boolean: res.prim_val = (int)left.prim_val > (int)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.String:
                        {
                            if (rcode == TypeCode.String)
                                res.prim_val = string.Compare((string)left.prim_val, (string)right.prim_val) > 0;
                            else throw new IncompatibleTypesInExpression();
                        }
                        break;
                    case TypeCode.Char:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Char: res.prim_val = (char)left.prim_val > (char)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType(">");
                }

            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                if (left.obj_val.Type.FullName == "PABCSystem.TypedSet" && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
                {
                    res.obj_val = (left.obj_val.Type.GetMember("CompareGreater", BindingFlags.All)[0] as MethodInfo).Invoke(left.obj_val, new Value[1] { right.obj_val });
                }
                else
                {
                    res.obj_val = EvalCommonOperation(left.obj_val, right.obj_val, PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.gr_name), ">");
                }
            }
            eval_stack.Push(res);
        }

        private void EvalGreaterEqual()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
                else
                    left.prim_val = DebugUtils.GetEnumValue(left.obj_val);
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
                else
                    right.prim_val = DebugUtils.GetEnumValue(right.obj_val);
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val >= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val >= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (int)left.prim_val >= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val >= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val >= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val >= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val >= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val >= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) >= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (int)left.prim_val >= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (double)left.prim_val >= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (double)left.prim_val >= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (double)left.prim_val >= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (double)left.prim_val >= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (double)left.prim_val >= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (double)left.prim_val >= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (double)left.prim_val >= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (double)left.prim_val >= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (double)left.prim_val >= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (double)left.prim_val >= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val >= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val >= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (byte)left.prim_val >= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val >= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val >= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val >= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val >= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val >= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val >= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (byte)left.prim_val >= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val >= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val >= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int16)left.prim_val >= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val >= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val >= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val >= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val >= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val >= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) >= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int16)left.prim_val >= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val >= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val >= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int64)left.prim_val >= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val >= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val >= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val >= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val >= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val >= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val >= (System.Int64)((System.UInt64)right.prim_val); break;
                                case TypeCode.Single: res.prim_val = (System.Int64)left.prim_val >= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val >= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val >= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.SByte)left.prim_val >= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val >= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val >= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val >= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val >= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val >= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) >= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.SByte)left.prim_val >= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val >= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val >= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt16)left.prim_val >= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val >= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val >= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val >= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val >= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val >= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val >= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt16)left.prim_val >= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val >= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val >= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt32)left.prim_val >= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val >= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val >= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val >= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val >= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val >= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val >= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt32)left.prim_val >= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val >= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val >= (System.UInt64)((int)right.prim_val); break;
                                case TypeCode.Double: res.prim_val = (System.UInt64)left.prim_val >= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) >= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val >= (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val >= (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val >= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val >= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val >= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt64)left.prim_val >= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (decimal)left.prim_val >= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (decimal)left.prim_val >= (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (decimal)left.prim_val == (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (decimal)left.prim_val >= (System.UInt64)((long)right.prim_val); break;
                                //case TypeCode.String : res.prim_val = ((decimal)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (decimal)left.prim_val >= (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (decimal)left.prim_val >= (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (decimal)left.prim_val >= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (decimal)left.prim_val >= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (decimal)left.prim_val >= (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (decimal)left.prim_val == (System.Single)right.prim_val; break;
                                case TypeCode.Decimal: res.prim_val = (decimal)left.prim_val >= (decimal)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Single:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Single)left.prim_val >= (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Single)left.prim_val >= (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Single)left.prim_val >= (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Single)left.prim_val >= (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Single)left.prim_val >= (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Single)left.prim_val >= (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Single)left.prim_val >= (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Single)left.prim_val >= (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Single)left.prim_val >= (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Single)left.prim_val >= (System.Single)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.Boolean:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Boolean: res.prim_val = (int)left.prim_val >= (int)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    case TypeCode.String:
                        {
                            if (rcode == TypeCode.String)
                                res.prim_val = string.Compare((string)left.prim_val, (string)right.prim_val) >= 0;
                            else throw new IncompatibleTypesInExpression();
                        }
                        break;
                    case TypeCode.Char:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Char: res.prim_val = (char)left.prim_val >= (char)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType(">=");
                }

            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                if (left.obj_val.Type.FullName == "PABCSystem.TypedSet" && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
                {
                    res.obj_val = (left.obj_val.Type.GetMember("CompareGreaterEqual", BindingFlags.All)[0] as MethodInfo).Invoke(left.obj_val, new Value[1] { right.obj_val });
                }
                else
                {
                    res.obj_val = EvalCommonOperation(left.obj_val, right.obj_val, PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.greq_name), ">=");
                }
            }
            eval_stack.Push(res);
        }

        private void EvalRem()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (right.prim_val == null && right.obj_val != null)
            {
                if (right.obj_val.IsPrimitive)
                    right.prim_val = right.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val % (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val % (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (int)left.prim_val % (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val % (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val % (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val % (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val % (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val % (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) % (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (int)left.prim_val % (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val % (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val % (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (byte)left.prim_val % (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val % (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val % (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val % (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val % (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val % (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val % (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (byte)left.prim_val % (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val % (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val % (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val % (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val % (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val % (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val % (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val % (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val % (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) % (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val % (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val % (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val % (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val % (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val % (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val % (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val % (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val % (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val % (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val % (System.Int64)((System.UInt64)right.prim_val); break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val % (System.Single)right.prim_val; break;
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val % (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val % (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val % (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val % (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val % (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val % (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val % (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val % (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) % (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val % (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val % (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val % (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val % (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val % (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val % (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val % (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val % (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val % (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val % (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val % (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val % (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val % (int)right.prim_val; break;
                                //case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val % (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val % (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val % (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val % (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val % (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val % (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val % (System.UInt64)right.prim_val; break;
                                //case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val % (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val % (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val % (System.UInt64)((int)right.prim_val); break;
                                //case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val % (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) % (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val % (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val % (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val % (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val % (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val % (System.UInt64)right.prim_val; break;
                                default: throw new IncompatibleTypesInExpression();
                                //case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val % (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    default: throw new NoOperatorForThisType("rem");
                }

                eval_stack.Push(res);
            }
            else
            {
                if (left.obj_val == null && left.prim_val != null)
                    left.obj_val = DebugUtils.MakeValue(left.prim_val);
                if (right.obj_val == null && right.prim_val != null)
                    right.obj_val = DebugUtils.MakeValue(right.prim_val);
                res.obj_val = EvalCommonOperation(left.obj_val, right.obj_val, PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.mod_name), "mod");
            }
        }

        private void EvalIs()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (right.type != null && (left.obj_val != null || left.prim_val != null))
            {
                if (left.obj_val != null)
                {
                    DebugType t = left.obj_val.Type;
                    while (t != null && t != right.type)
                    {
                        t = t.BaseType;
                    }
                    res.prim_val = t == right.type;
                }
                else if (right.managed_type != null)
                    res.prim_val = left.prim_val.GetType().IsSubclassOf(right.managed_type);
                else
                    res.prim_val = false;
            }
            else if (left.managed_type != null)
                throw new VariableExpectedTypeFound(left.managed_type.Name);

            eval_stack.Push(res);
        }

        private void EvalAs()
        {
            throw new NotImplementedException();
        }

        public Value MakeValue(RetValue val)
        {
            if (val.obj_val != null) return val.obj_val;
            return DebugUtils.MakeValue(val.prim_val);
        }

        private Value box(Value v)
        {
            if (v.Type.IsPrimitive || v.Type.IsValueType)
            {
                return v.GetPermanentReference();
            }
            return v;
        }

        private void EvalIn()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (right.obj_val != null && right.obj_val.Type.FullName == "PABCSystem.TypedSet")
            {
                MethodInfo mi = right.obj_val.Type.GetMember("Contains", BindingFlags.All)[0] as MethodInfo;
                Value tmp = box(MakeValue(left));
                Value v = mi.Invoke(right.obj_val, new Value[1] { tmp });
                res.prim_val = v.PrimitiveValue;
                //res.prim_val = MakeValue(left).PrimitiveValue;//v.PrimitiveValue;
                //Value v = mi.Invoke(right.obj_val,new Value[1]{right.obj_val});
                //res.prim_val = MakeValue(left).PrimitiveValue;
            }
            else throw new NoOperatorForThisType("in");
            eval_stack.Push(res);
        }

        public override void visit(bin_expr _bin_expr)
        {
            _bin_expr.left.visit(this);
            switch (_bin_expr.operation_type)
            {
                case Operators.Plus: names.Add("+"); break;
                case Operators.Minus: names.Add("-"); break;
                case Operators.Multiplication: names.Add("*"); break;
                case Operators.Division: names.Add("/"); break;
                case Operators.IntegerDivision: names.Add(" div "); break;
                case Operators.BitwiseAND: names.Add(" and "); break;
                case Operators.BitwiseOR: names.Add(" or "); break;
                case Operators.BitwiseXOR: names.Add(" xor "); break;
                case Operators.BitwiseLeftShift: names.Add(" shl "); break;
                case Operators.BitwiseRightShift: names.Add(" shr "); break;
                case Operators.Equal: names.Add("="); break;
                case Operators.NotEqual: names.Add("<>"); break;
                case Operators.Less: names.Add("<"); break;
                case Operators.LessEqual: names.Add("<="); break;
                case Operators.Greater: names.Add(">"); break;
                case Operators.GreaterEqual: names.Add(">="); break;
                case Operators.LogicalAND: names.Add(" and "); break;
                case Operators.LogicalOR: names.Add(" or "); break;
                case Operators.ModulusRemainder: names.Add(" mod "); break;
                case Operators.Is: names.Add(" is "); break;
                case Operators.As: names.Add(" as "); break;
                case Operators.In: names.Add(" in "); break;
            }
            _bin_expr.right.visit(this);
            switch (_bin_expr.operation_type)
            {
                case Operators.Plus: EvalPlus(); break;
                case Operators.Minus: EvalMinus(); break;
                case Operators.Multiplication: EvalMult(); break;
                case Operators.Division: EvalDiv(); break;
                case Operators.IntegerDivision: EvalIDiv(); break;
                case Operators.BitwiseAND: EvalAnd(); break;
                case Operators.BitwiseOR: EvalOr(); break;
                case Operators.BitwiseXOR: EvalXor(); break;
                case Operators.BitwiseLeftShift: EvalBitwiseLeft(); break;
                case Operators.BitwiseRightShift: EvalBitwiseRight(); break;
                case Operators.Equal: EvalEqual(); break;
                case Operators.NotEqual: EvalNotEqual(); break;
                case Operators.Less: EvalLess(); break;
                case Operators.LessEqual: EvalLessEqual(); break;
                case Operators.Greater: EvalGreater(); break;
                case Operators.GreaterEqual: EvalGreaterEqual(); break;
                case Operators.LogicalAND: EvalAnd(); break;
                case Operators.LogicalOR: EvalOr(); break;
                case Operators.ModulusRemainder: EvalRem(); break;
                case Operators.Is: EvalIs(); break;
                case Operators.As: EvalAs(); break;
                case Operators.In: EvalIn(); break;
                case Operators.Assignment: throw new System.Exception();

            }
        }

        private void EvalNot()
        {
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Boolean: res.prim_val = !(bool)left.prim_val; break;
                    case TypeCode.Byte: res.prim_val = ~(byte)left.prim_val; break;
                    case TypeCode.Int16: res.prim_val = ~(System.Int16)left.prim_val; break;
                    case TypeCode.Int32: res.prim_val = ~(System.Int32)left.prim_val; break;
                    case TypeCode.Int64: res.prim_val = ~(System.Int64)left.prim_val; break;
                    case TypeCode.SByte: res.prim_val = ~(sbyte)left.prim_val; break;
                    case TypeCode.UInt16: res.prim_val = ~(System.UInt16)left.prim_val; break;
                    case TypeCode.UInt32: res.prim_val = ~(System.UInt32)left.prim_val; break;
                    case TypeCode.UInt64: res.prim_val = ~(System.UInt64)left.prim_val; break;
                    default: throw new NoOperatorForThisType("not");
                }
                eval_stack.Push(res);
            }
            else
            {
                string op = PascalABCCompiler.StringConstants.GetNETOperName(PascalABCCompiler.StringConstants.not_name);
                {
                    IList<MemberInfo> mems = left.obj_val.Type.GetMember(op, Debugger.BindingFlags.All);
                    if (mems != null && mems.Count == 1 && mems[0] is MethodInfo)
                    {
                        MethodInfo mi = mems[0] as MethodInfo;
                        res.obj_val = mi.Invoke(null, new Value[1] { left.obj_val });
                    }
                    else
                        throw new NoOperatorForThisType("not");
                }
            }
        }

        private void EvalUnmin()
        {
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val == null && left.obj_val != null)
            {
                if (left.obj_val.IsPrimitive)
                    left.prim_val = left.obj_val.PrimitiveValue;
            }
            if (left.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Byte: res.prim_val = -(byte)left.prim_val; break;
                    case TypeCode.Int16: res.prim_val = -(System.Int16)left.prim_val; break;
                    case TypeCode.Int32: res.prim_val = -(System.Int32)left.prim_val; break;
                    case TypeCode.Int64: res.prim_val = -(System.Int64)left.prim_val; break;
                    case TypeCode.SByte: res.prim_val = -(sbyte)left.prim_val; break;
                    case TypeCode.UInt16: res.prim_val = -(System.UInt16)left.prim_val; break;
                    case TypeCode.UInt32: res.prim_val = -(System.UInt32)left.prim_val; break;
                    //case TypeCode.UInt64: res.prim_val = -(System.UInt64)left.prim_val; break;
                    case TypeCode.Double: res.prim_val = -(double)left.prim_val; break;
                    case TypeCode.Single: res.prim_val = -(System.Single)left.prim_val; break;
                    default: throw new NoOperatorForThisType("-");
                }
                eval_stack.Push(res);
            }
        }

        public override void visit(un_expr _un_expr)
        {
            _un_expr.subnode.visit(this);
            switch (_un_expr.operation_type)
            {
                case Operators.BitwiseNOT:
                    names.Add("not ");
                    EvalNot(); break;
                case Operators.LogicalNOT:
                    names.Add("not ");
                    EvalNot(); break;
                case Operators.Minus:
                    names.Add("-");
                    EvalUnmin(); break;
            }
        }

        public override void visit(const_node _const_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(bool_const _bool_const)
        {
            RetValue val = new RetValue();
            val.prim_val = _bool_const.val;
            names.Add(_bool_const.val.ToString());
            eval_stack.Push(val);
        }

        public override void visit(int32_const _int32_const)
        {
            RetValue val = new RetValue();
            val.prim_val = _int32_const.val;
            names.Add(_int32_const.val.ToString());
            eval_stack.Push(val);
        }

        public override void visit(double_const _double_const)
        {
            RetValue val = new RetValue();
            val.prim_val = _double_const.val;
            names.Add(_double_const.val.ToString());
            eval_stack.Push(val);
        }

        public override void visit(statement _statement)
        {
            throw new NotImplementedException();
        }

        public override void visit(subprogram_body _subprogram_body)
        {
            throw new NotImplementedException();
        }

        public Value GetValue(string var)
        {
            try
            {
                List<NamedValue> unit_vars = new List<NamedValue>();
                NamedValueCollection nvc = null;
                NamedValue ret_nv = null;
                NamedValue global_nv = null;
                NamedValue disp_nv = null;
                nvc = debuggedProcess.SelectedFunction.LocalVariables;
                List<NamedValue> val_list = new List<NamedValue>();
                foreach (NamedValue nv in nvc)//smotrim sredi lokalnyh peremennyh
                {
                    if (nv.Name.IndexOf("<>local_variables") == -1)
                        val_list.Add(nv);
                    else
                    {
                        foreach (NamedValue nv2 in nv.GetMembers())//smotrim sredi lokalnyh peremennyh
                        {
                            val_list.Add(nv2);
                        }
                    }
                }
                foreach (NamedValue nv in val_list)
                {
                    if (nv.Name.IndexOf(':') != -1)
                    {
                        int pos = nv.Name.IndexOf(':');
                        string name = nv.Name.Substring(0, pos);

                        if (string.Compare(name, var, true) == 0)
                        {
                            int num_line = debuggedProcess.SelectedFunction.NextStatement.StartLine;
                            int start_line = Convert.ToInt32(nv.Name.Substring(pos + 1, nv.Name.LastIndexOf(':') - pos - 1));
                            int end_line = Convert.ToInt32(nv.Name.Substring(nv.Name.LastIndexOf(':') + 1));
                            if (num_line >= start_line && num_line <= end_line)
                                return nv;
                        }
                    }
                    if (string.Compare(nv.Name, var, true) == 0) return nv;
                    else if (nv.Name.Contains("$class_var")) global_nv = nv;
                    else if (nv.Name.Contains("$unit_var")) unit_vars.Add(nv);
                    else if (nv.Name == "$disp$") disp_nv = nv;
                    else if (nv.Name.StartsWith("$rv_"))
                        ret_nv = nv;
                }
                nvc = debuggedProcess.SelectedFunction.Arguments;
                NamedValue self_nv = null;
                foreach (NamedValue nv in nvc)
                {
                    if (string.Compare(nv.Name, var, true) == 0) return nv;
                    if (nv.Name == "$obj$")
                        self_nv = nv;
                }
                if (var.ToLower() == "result" && ret_nv != null) return ret_nv;
                if (var.ToLower() == "self")
                {
                    try
                    {
                        return debuggedProcess.SelectedFunction.ThisValue;
                    }
                    catch
                    {
                        if (self_nv != null)
                            return self_nv;
                    }
                }
                if (disp_nv != null)
                {
                    NamedValue parent_val = null;
                    IList<FieldInfo> fields = disp_nv.Type.GetFields(BindingFlags.All);
                    foreach (FieldInfo fi in fields)
                        if (string.Compare(fi.Name, var, true) == 0) return fi.GetValue(disp_nv);
                        else if (fi.Name == "$parent$") parent_val = fi.GetValue(disp_nv);
                    NamedValue pv = parent_val;
                    while (!pv.IsNull)
                    {
                        fields = parent_val.Type.GetFields(BindingFlags.All);
                        foreach (FieldInfo fi in fields)
                            if (string.Compare(fi.Name, var, true) == 0) return fi.GetValue(parent_val);
                            else if (fi.Name == "$parent$") pv = fi.GetValue(parent_val);
                        if (!pv.IsNull) parent_val = pv;
                    }
                }
                nvc = debuggedProcess.SelectedFunction.ContaingClassVariables;
                foreach (NamedValue nv in nvc)
                {
                    if (string.Compare(nv.Name, var, true) == 0) return nv;
                }
                if (self_nv != null)
                {
                    IList<FieldInfo> fields = self_nv.Dereference.Type.GetFields(BindingFlags.All);
                    foreach (FieldInfo fi in fields)
                        if (string.Compare(fi.Name, var, true) == 0)
                            return fi.GetValue(self_nv.Dereference);
                }
                if (global_nv != null)
                {
                    IList<FieldInfo> fields = global_nv.Type.GetFields(BindingFlags.All);
                    foreach (FieldInfo fi in fields)
                        if (string.Compare(fi.Name, var, true) == 0) return fi.GetValue(global_nv);
                    Type t = AssemblyHelper.GetType(global_nv.Type.FullName);
                    if (t != null)
                    {
                        System.Reflection.FieldInfo fi = t.GetField(var, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.IgnoreCase);
                        if (fi != null && fi.IsLiteral)
                            return DebugUtils.MakeValue(fi.GetRawConstantValue());
                    }
                }

                List<DebugType> types = AssemblyHelper.GetUsesTypes(debuggedProcess, debuggedProcess.SelectedFunction.DeclaringType);
                foreach (DebugType dt in types)
                {
                    IList<FieldInfo> fields = dt.GetFields(BindingFlags.All);
                    foreach (FieldInfo fi in fields)
                        if (fi.IsStatic && string.Compare(fi.Name, var, true) == 0)
                            return fi.GetValue(null);
                    Type t = AssemblyHelper.GetType(dt.FullName);
                    if (t != null)
                    {
                        System.Reflection.FieldInfo fi = t.GetField(var, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.IgnoreCase);
                        if (fi != null && fi.IsLiteral)
                            return DebugUtils.MakeValue(fi.GetRawConstantValue());
                    }
                }
                /*foreach (NamedValue nv in unit_vars)
                {
                    IList<FieldInfo> fields = nv.Type.GetFields(BindingFlags.All);
                    foreach (FieldInfo fi in fields)
                        if (string.Compare(fi.Name, var, true) == 0) return fi.GetValue(nv);
                }*/

                IList<Function> funcs = debuggedProcess.SelectedThread.Callstack;
                for (int i = 0; i < funcs.Count; i++)
                {
                    nvc = funcs[i].ContaingClassVariables;
                    foreach (NamedValue nv in nvc)
                    {
                        if (string.Compare(nv.Name, var, true) == 0) return nv;
                    }
                }

            }
            catch (System.Exception e)
            {
                //System.Windows.Forms.MessageBox.Show(e.ToString());

            }
            return null;
            //throw new UnknownName(var);
        }

        private MethodInfo[] GetMethods(string name)
        {
            List<MethodInfo> meths = new List<MethodInfo>();
            IList<MemberInfo> mis = debuggedProcess.SelectedFunction.DeclaringType.GetMember(name, BindingFlags.All);
            foreach (MemberInfo mi in mis)
            {
                if (mi is MethodInfo)
                    meths.Add(mi as MethodInfo);
            }
            DebugType pabc_system_type = AssemblyHelper.GetPABCSystemType();
            if (pabc_system_type != null)
            {
                mis = pabc_system_type.GetMember(name, BindingFlags.All);
                foreach (MemberInfo mi in mis)
                {
                    if (mi is MethodInfo)
                        meths.Add(mi as MethodInfo);
                }
            }
            
            return meths.ToArray();
        }

        private MethodInfo[] get_method(addressed_value av, out Value obj_val)
        {
            MethodInfo[] meths = null;
            obj_val = null;
            if (av is ident)
            {
                ident e = av as ident;
                meths = GetMethods(e.name);
                obj_val = null;
            }
            else if (av is dot_node)
            {
                dot_node e = av as dot_node;
                string name = build_name(e.left);
                ident id = e.right as ident;
                if (name != null && id != null)
                {
                    Type t = AssemblyHelper.GetType(name);
                    if (t != null)
                    {
                        //DebugType dt = DebugType.Create(this.debuggedProcess.GetModule(name),(uint)t.MetadataToken);
                        DebugType dt = DebugUtils.GetDebugType(t);
                        IList<MemberInfo> mis = new List<MemberInfo>();
                        DebugType tmp = dt;
                        while (tmp != null && mis.Count == 0)
                        {
                            try
                            {
                                mis = tmp.GetMember(id.name, BindingFlags.All);
                                tmp = tmp.BaseType;
                            }
                            catch
                            {

                            }
                        }
                        List<MethodInfo> meth_list = new List<MethodInfo>();

                        foreach (MemberInfo mi in mis)
                        {
                            if (mi is MethodInfo)
                                meth_list.Add(mi as MethodInfo);
                        }
                        meths = meth_list.ToArray();
                    }
                }
            }
            return meths;
        }

        public override void visit(ident _ident)
        {
            RetValue val = new RetValue();
            if (string.Compare(_ident.name, "true", true) == 0)
            {
                val.prim_val = true;
            }
            else if (string.Compare(_ident.name, "false", true) == 0)
            {
                val.prim_val = false;
            }
            else
            {
                names.Add(_ident.name);
                val.obj_val = GetValue(_ident.name);
                if (val.obj_val == null)
                {
                    val.prim_val = EvalStandFuncNoParam(_ident.name);
                }
                if (val.obj_val == null && val.prim_val == null)
                {
                    Type t = AssemblyHelper.GetType(_ident.name);
                    if (t != null)
                    {
                        val.type = DebugUtils.GetDebugType(t);//DebugType.Create(this.debuggedProcess.GetModule(name),(uint)t.MetadataToken);
                        val.managed_type = t;
                    }
                }
                if (val.obj_val == null && val.prim_val == null && val.type == null && !by_dot)
                    throw new UnknownName(_ident.name);
            }
            eval_stack.Push(val);
        }

        private object EvalStandFuncNoParam(string name)
        {
            switch (name.ToLower())
            {
                case "pi": return Math.PI;
                case "e": return Math.E;
                case "random": return new Random().NextDouble();
                case "maxshortint": return sbyte.MaxValue;
                case "maxbyte": return byte.MaxValue;
                case "maxsmallint": return short.MaxValue;
                case "maxword": return ushort.MaxValue;
                case "maxlongword": return uint.MaxValue;
                case "maxint": return int.MaxValue;
                case "maxint64": return long.MaxValue;
                case "maxuint64": return ulong.MaxValue;
                case "maxdouble":
                case "maxreal": return double.MaxValue;
                case "minreal": return double.MinValue;
                case "maxsingle": return float.MaxValue;
                case "minsingle": return float.MinValue;
                case "getexefilename": return WorkbenchServiceFactory.DebuggerManager.ExeFileName;
                case "paramcount": return param_count();
                case "getdir":
                case "getcurrentdir": return current_directory();
                case "sign":
                case "sin":
                case "cos":
                case "tan":
                case "ln":
                case "log10":
                case "exp":
                case "arctan":
                case "arccos":
                case "arcsin":
                case "abs":
                case "sqrt":
                case "trunc":
                case "round":
                case "ceil":
                case "floor":
                case "int":
                case "frac":
                case "sqr":
                case "chr":
                case "ord":
                case "odd":
                case "sinh":
                case "cosh":
                case "tanh":
                case "strtoint":
                case "inttostr":
                case "succ":
                case "pred":
                case "logn":
                case "min":
                case "max":
                case "power":
                    throw new WrongNumberArguments(name);
                default:
                    return null;

            }
            return null;
        }

        private object param_count()
        {
            DebugType t = DebugUtils.GetDebugType(typeof(System.Environment));
            MethodInfo mi = DebugUtils.GetMethod(t, "GetCommandLineArgs");
            Value args = mi.Invoke(null, new Value[0]);
            return args.ArrayLenght - 2;
        }

        private object param_str(int ind)
        {
            DebugType t = DebugUtils.GetDebugType(typeof(System.Environment));
            MethodInfo mi = DebugUtils.GetMethod(t, "GetCommandLineArgs");
            Value args = mi.Invoke(null, new Value[0]);
            return args.GetArrayElement((uint)(ind + 1)).PrimitiveValue;
        }

        private object current_directory()
        {
            DebugType t = DebugUtils.GetDebugType(typeof(System.Environment));
            PropertyInfo pi = DebugUtils.GetProperty(t, "CurrentDirectory");
            return pi.GetValue(null).PrimitiveValue;
        }

        private object sign(object x)
        {
            return Math.Sign(Convert.ToDouble(x));
        }

        private double _int(double x)
        {
            return x >= 0 ? Math.Floor(x) : Math.Ceiling(x);
        }

        private string left_str(string s, int count)
        {
            if (count > s.Length)
                count = s.Length;
            return s.Substring(0, count);
        }

        private string right_str(string s, int count)
        {
            if (count > s.Length)
                count = s.Length;
            return s.Substring(s.Length - count, count);
        }

        private string reverse_str(string s)
        {
            char[] ca = s.ToCharArray();
            Array.Reverse(ca);
            return new string(ca);
        }

        private const string FILE_NOT_ASSIGNED = "Ошибка ввода/вывода: для файловой переменной не выполнена процедура Assign";
        private const string FILE_NOT_OPENED = "Ошибка ввода/вывода: файл не открыт";

        private int _low(Value v, string name)
        {
            if (v.IsArray)
            {
                MethodInfo mi = DebugUtils.GetMethod(v.Type, "GetLowerBound");
                return Convert.ToInt32(mi.Invoke(v, new Value[1] { DebugUtils.MakeValue(0) }).PrimitiveValue);
            }
            if (DebugUtils.GetNullBasedArray(v) != null)
            {
                System.Reflection.FieldInfo fi = AssemblyHelper.GetType(v.Type.FullName).GetField("LowerIndex");
                return Convert.ToInt32(fi.GetRawConstantValue());
            }
            throw new WrongTypeOfArgument(name);
        }

        private int _high(Value v, string name)
        {
            if (v.IsArray)
            {
                MethodInfo mi = DebugUtils.GetMethod(v.Type, "GetUpperBound");
                return Convert.ToInt32(mi.Invoke(v, new Value[1] { DebugUtils.MakeValue(0) }).PrimitiveValue);
            }
            if (DebugUtils.GetNullBasedArray(v) != null)
            {
                System.Reflection.FieldInfo fi = AssemblyHelper.GetType(v.Type.FullName).GetField("UpperIndex");
                return Convert.ToInt32(fi.GetRawConstantValue());
            }
            throw new WrongTypeOfArgument(name);
        }

        private bool _eof(Value v, string name)
        {
            if (v.Type.FullName == "PABCSystem.Text")
            {
                Value sr = v.GetMember("sr");
                Value sw = v.GetMember("sw");
                if (!sr.IsNull)
                    return Convert.ToBoolean(sr.GetMember("EndOfStream").PrimitiveValue);
                else
                    if (!sw.IsNull)
                        throw new CommonEvaluationError(new System.Exception("Функция Eof не может быть вызвана для текстового файла, открытого на запись"));
                    else
                        throw new CommonEvaluationError(new System.Exception(FILE_NOT_OPENED));
            }
            else if (v.Type.FullName == "PABCSystem.BinaryFile" || v.Type.FullName == "PABCSystem.TypedFile")
            {
                Value fi = v.GetMember("fi");
                Value fs = v.GetMember("fs");
                if (fi.IsNull)
                    throw new CommonEvaluationError(new System.Exception(FILE_NOT_ASSIGNED));
                if (fs.IsNull)
                    throw new CommonEvaluationError(new System.Exception(FILE_NOT_OPENED));
                return Convert.ToInt64(fs.GetMember("Position").PrimitiveValue) == Convert.ToInt64(fs.GetMember("Length").PrimitiveValue);
            }
            throw new WrongTypeOfArgument(name);
        }

        private long file_pos(Value v, string name)
        {
            if (v.Type.FullName == "PABCSystem.TypedFile")
            {
                Value fi = v.GetMember("fi");
                Value fs = v.GetMember("fs");
                if (fi.IsNull)
                    throw new CommonEvaluationError(new System.Exception(FILE_NOT_ASSIGNED));
                if (fs.IsNull)
                    throw new CommonEvaluationError(new System.Exception(FILE_NOT_OPENED));
                return Convert.ToInt64(fs.GetMember("Position").PrimitiveValue) / Convert.ToInt64(v.GetMember("ElementSize").PrimitiveValue);
            }
            else if (v.Type.FullName == "PABCSystem.BinaryFile")
            {
                Value fi = v.GetMember("fi");
                Value fs = v.GetMember("fs");
                if (fi.IsNull)
                    throw new CommonEvaluationError(new System.Exception(FILE_NOT_ASSIGNED));
                if (fs.IsNull)
                    throw new CommonEvaluationError(new System.Exception(FILE_NOT_OPENED));
                return (long)fs.GetMember("Position").PrimitiveValue;
            }
            throw new WrongTypeOfArgument(name);
        }

        private long file_size(Value v, string name)
        {
            if (v.Type.FullName == "PABCSystem.TypedFile")
            {
                Value fi = v.GetMember("fi");
                Value fs = v.GetMember("fs");
                if (fi.IsNull)
                    throw new CommonEvaluationError(new System.Exception(FILE_NOT_ASSIGNED));
                if (fs.IsNull)
                    throw new CommonEvaluationError(new System.Exception(FILE_NOT_OPENED));
                return Convert.ToInt64(fs.GetMember("Length").PrimitiveValue) / Convert.ToInt64(v.GetMember("ElementSize").PrimitiveValue);
            }
            else if (v.Type.FullName == "PABCSystem.BinaryFile")
            {
                Value fi = v.GetMember("fi");
                Value fs = v.GetMember("fs");
                if (fi.IsNull)
                    throw new CommonEvaluationError(new System.Exception(FILE_NOT_ASSIGNED));
                if (fs.IsNull)
                    throw new CommonEvaluationError(new System.Exception(FILE_NOT_OPENED));
                return Convert.ToInt64(fs.GetMember("Length").PrimitiveValue);
            }
            throw new WrongTypeOfArgument(name);
        }

        private bool _eoln(Value v, string name)
        {
            if (v.Type.FullName == "PABCSystem.Text")
            {
                Value sr = v.GetMember("sr");
                Value sw = v.GetMember("sw");
                if (!sr.IsNull)
                {
                    MethodInfo mi = DebugUtils.GetMethod(sr.Type, "Peek");
                    int val = (int)mi.Invoke(sr, new Value[0]).PrimitiveValue;
                    return val == 13;
                }
                else
                    if (!sw.IsNull)
                        throw new CommonEvaluationError(new System.Exception("Функция Eoln не может быть вызвана для текстового файла, открытого на запись"));
                    else
                        throw new CommonEvaluationError(new System.Exception(FILE_NOT_OPENED));
            }
            throw new WrongTypeOfArgument(name);
        }

        private string extract_file_path(string fname)
        {
            string fpath = (new System.IO.FileInfo(fname)).DirectoryName;
            if ((fpath.Length > 0) && (fpath[fpath.Length - 1] != '\\') && (fpath[fpath.Length - 1] != '/'))
                fpath += '\\';
            return fpath;
        }

        private string extract_file_drive(string fname)
        {
            string fpath = (new System.IO.FileInfo(fname)).DirectoryName;
            int ind = fpath.IndexOf(':');
            if (ind != -1)
                return fpath.Substring(0, ind + 1);
            return "";
        }

        private string _copy(string s, int index, int count)
        {
            try
            {
                if (index - 1 >= s.Length) return "";
                else
                    return s.Substring(index - 1, count);
            }
            catch (System.Exception)
            {
                return s.Substring(index - 1, s.Length - index + 1);
            }
        }

        private System.Text.Encoding enc = System.Text.Encoding.GetEncoding(1251);
        private System.Globalization.CultureInfo enCulture = new System.Globalization.CultureInfo("en-US");

        private object EvalStandFuncWithParam(string name, params object[] pars)
        {
            switch (name.ToLower())
            {
                case "sign":
                case "sin":
                case "cos":
                case "tan":
                case "ln":
                case "logn":
                case "log2":
                case "log10":
                case "exp":
                case "arctan":
                case "arccos":
                case "arcsin":
                case "abs":
                case "sqrt":
                case "trunc":
                case "round":
                case "ceil":
                case "floor":
                case "int":
                case "frac":
                case "sqr":
                case "degtorad":
                case "radtodeg":
                case "chr":
                case "chrunicode":
                case "ord":
                case "odd":
                case "sinh":
                case "cosh":
                case "tanh":
                case "reversestring":
                case "lowercase":
                case "uppercase":
                case "lowcase":
                case "upcase":
                case "strtoint":
                case "strtoint64":
                case "strtofloat":
                case "inttostr":
                case "trim":
                case "trimleft":
                case "trimright":
                case "floattostr":
                case "succ":
                case "low":
                case "high":
                case "eof":
                case "eoln":
                case "filepos":
                case "filesize":
                case "fileexists":
                case "extractfilename":
                case "extractfileext":
                case "extractfilepath":
                case "extractfiledir":
                case "extractfiledrive":
                case "expandfilename":
                case "pred":
                case "paramstr":
                    if (pars.Length != 1)
                        throw new WrongNumberArguments(name);
                    if (pars[0] == null)
                        throw new WrongTypeOfArgument(name);
                    break;
                case "min":
                case "pos":
                case "leftstr":
                case "rightstr":
                case "stringofchar":
                case "comparestr":
                case "max":
                case "power":
                    if (pars.Length != 2)
                        throw new WrongNumberArguments(name);
                    if (pars[0] == null || pars[1] == null)
                        throw new WrongTypeOfArgument(name);
                    break;
                case "pi":
                case "e":
                    throw new WrongNumberArguments(name);
                case "random":
                case "getexefilename":
                case "paramcount":
                case "getdir":
                case "getcurrentdir":
                    if (pars.Length == 0)
                        return EvalStandFuncNoParam(name);
                    else
                        throw new WrongNumberArguments(name);
                case "length":
                    if (pars.Length == 0 || pars.Length > 2)
                        throw new WrongNumberArguments(name);
                    break;
                case "posex":
                case "copy":
                    if (pars.Length != 3)
                        throw new WrongNumberArguments(name);
                    if (pars[0] == null || pars[1] == null || pars[2] == null)
                        throw new WrongTypeOfArgument(name);
                    break;
                case "format":
                    if (pars.Length == 0)
                        throw new WrongNumberArguments(name);
                    break;
                case "concat":
                    break;
                case "integer":
                case "byte":
                case "shortint":
                case "smallint":
                case "word":
                case "longword":
                case "int64":
                case "uint64":
                case "real":
                case "single":
                case "char":
                case "string":
                case "boolean":
                    if (pars.Length != 1)
                        throw new WrongNumberArguments(name);
                    break;

                default:
                    Type t = AssemblyHelper.GetType(name);
                    if (t != null)
                    {
                        if (pars.Length != 1)
                            throw new WrongNumberArguments(name);
                        if (pars[0] is Value)
                        {
                            Value v = pars[0] as Value;
                            DebugType dt = DebugUtils.GetDebugType(t);
                            if (dt == v.Type || v.Type.IsSubclassOf(dt))
                                return v;
                            else
                                throw new InvalidCastException();
                        }
                        else
                            throw new InvalidCastException();
                    }
                    throw new UnknownName(name);
            }
            try
            {
                switch (name.ToLower())
                {
                    case "sign": return sign(pars[0]);
                    case "sin": return Math.Sin(Convert.ToDouble(pars[0]));
                    case "cos": return Math.Cos(Convert.ToDouble(pars[0]));
                    case "tan": return Math.Tan(Convert.ToDouble(pars[0]));
                    case "ln": return Math.Log(Convert.ToDouble(pars[0]));
                    case "log10": return Math.Log10(Convert.ToDouble(pars[0]));
                    case "log2": return Math.Log(Convert.ToDouble(pars[0]), 2);
                    case "logn": return Math.Log(Convert.ToDouble(pars[1]), Convert.ToInt32(pars[0]));
                    case "exp": return Math.Exp(Convert.ToDouble(pars[0]));
                    case "arctan": return Math.Atan(Convert.ToDouble(pars[0]));
                    case "arccos": return Math.Acos(Convert.ToDouble(pars[0]));
                    case "arcsin": return Math.Asin(Convert.ToDouble(pars[0]));
                    case "min": return Math.Min(Convert.ToDouble(pars[0]), Convert.ToDouble(pars[1]));
                    case "max": return Math.Max(Convert.ToDouble(pars[0]), Convert.ToDouble(pars[1]));
                    case "abs": return Math.Abs(Convert.ToDouble(pars[0]));
                    case "sqrt": return Math.Sqrt(Convert.ToDouble(pars[0]));
                    case "trunc": return Math.Truncate(Convert.ToDouble(pars[0]));
                    case "round": return Math.Round(Convert.ToDouble(pars[0]));
                    case "power": return Math.Pow(Convert.ToDouble(pars[0]), Convert.ToDouble(pars[1]));
                    case "ceil": return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(pars[0])));
                    case "floor": return Math.Floor(Convert.ToDouble(pars[0]));
                    case "int": return _int(Convert.ToDouble(pars[0]));
                    case "frac": return Convert.ToDouble(pars[0]) - _int(Convert.ToDouble(pars[0]));
                    case "sqr": return Convert.ToDouble(pars[0]) * Convert.ToDouble(pars[0]);
                    case "degtorad": return Convert.ToDouble(pars[0]) * Math.PI / 180;
                    case "radtodeg": return Convert.ToDouble(pars[0]) * 180 / Math.PI;
                    case "chrunicode": return Convert.ToChar(pars[0]);
                    case "chr":
                        {
                            byte[] arr = new byte[1];
                            arr[0] = Convert.ToByte(pars[0]);
                            return enc.GetChars(arr)[0];
                        }
                    case "ord":
                        {
                            TypeCode tc = Type.GetTypeCode(pars[0].GetType());
                            switch (tc)
                            {
                                case TypeCode.Boolean: return Convert.ToInt32(Convert.ToBoolean(pars[0]));
                                case TypeCode.Byte: return Convert.ToInt32(Convert.ToByte(pars[0]));
                                case TypeCode.SByte: return Convert.ToInt32(Convert.ToSByte(pars[0]));
                                case TypeCode.Int16: return Convert.ToInt32(Convert.ToInt16(pars[0]));
                                case TypeCode.UInt16: return Convert.ToInt32(Convert.ToUInt16(pars[0]));
                                case TypeCode.Int32: return Convert.ToInt32(pars[0]);
                                case TypeCode.UInt32: return Convert.ToUInt32(pars[0]);
                                case TypeCode.Int64: return Convert.ToInt64(pars[0]);
                                case TypeCode.UInt64: return Convert.ToUInt64(pars[0]);
                                case TypeCode.Char:
                                    {
                                        char[] arr = new char[1];
                                        arr[0] = Convert.ToChar(pars[0]);
                                        return enc.GetBytes(arr)[0];
                                    }
                                default:
                                    throw new WrongTypeOfArgument(name);
                            }
                        }
                    case "ordunicode": return Convert.ToUInt16(Convert.ToChar(pars[0]));
                    case "odd": return Convert.ToInt32(pars[0]) % 2 != 0;
                    case "sinh": return Math.Sinh(Convert.ToDouble(pars[0]));
                    case "cosh": return Math.Cosh(Convert.ToDouble(pars[0]));
                    case "tanh": return Math.Tanh(Convert.ToDouble(pars[0]));
                    case "strtoint": return Convert.ToInt32(Convert.ToString(pars[0]));
                    case "inttostr": return Convert.ToString(Convert.ToInt32(pars[0]));
                    case "floattostr": return Convert.ToString(Convert.ToDouble(pars[0]));
                    case "strtofloat": return Convert.ToDouble(Convert.ToString(pars[0]));
                    case "strtoint64": return Convert.ToInt64(Convert.ToString(pars[0]));
                    case "comparestr": return string.Compare(Convert.ToString(pars[0]), Convert.ToString(pars[1]));
                    case "stringofchar": return new string(Convert.ToChar(pars[0]), Convert.ToInt32(pars[1]));
                    case "leftstr": return left_str(Convert.ToString(pars[0]), Convert.ToInt32(pars[1]));
                    case "rightstr": return right_str(Convert.ToString(pars[0]), Convert.ToInt32(pars[1]));
                    case "reversestring": return reverse_str(Convert.ToString(pars[0]));
                    case "pos": return Convert.ToString(pars[1]).IndexOf(Convert.ToString(pars[0])) + 1;
                    case "posex": return Convert.ToString(pars[1]).IndexOf(Convert.ToString(pars[0]), Convert.ToInt32(pars[2]) - 1) + 1;
                    case "trim": return Convert.ToString(pars[0]).Trim();
                    case "trimleft": return Convert.ToString(pars[0]).TrimStart();
                    case "trimright": return Convert.ToString(pars[0]).TrimEnd();
                    case "filepos":
                        if (pars[0] is Value)
                            return file_pos(pars[0] as Value, name);
                        else
                            throw new WrongTypeOfArgument(name);
                    case "filesize":
                        if (pars[0] is Value)
                            return file_size(pars[0] as Value, name);
                        else
                            throw new WrongTypeOfArgument(name);
                    case "fileexists":
                        return System.IO.File.Exists(Convert.ToString(pars[0]));
                    case "extractfilename":
                        return (new System.IO.FileInfo(Convert.ToString(pars[0]))).Name;
                    case "extractfileext":
                        return (new System.IO.FileInfo(Convert.ToString(pars[0]))).Extension;
                    case "extractfilepath":
                        return extract_file_path(Convert.ToString(pars[0]));
                    case "extractfiledir":
                        return (new System.IO.FileInfo(Convert.ToString(pars[0]))).DirectoryName;
                    case "extractfiledrive":
                        return extract_file_drive(Convert.ToString(pars[0]));
                    case "expandfilename":
                        return (new System.IO.FileInfo(Convert.ToString(pars[0]))).FullName;
                    case "copy":
                        return _copy(Convert.ToString(pars[0]), Convert.ToInt32(pars[1]), Convert.ToInt32(pars[2]));
                    case "format":
                        {
                            object[] prms = new object[pars.Length - 1];
                            Array.ConstrainedCopy(pars, 1, prms, 0, pars.Length - 1);
                            return string.Format(enCulture, Convert.ToString(pars[0]), prms);
                        }
                    case "lowercase":
                        if (pars[0] is char)
                            return char.ToLower(Convert.ToChar(pars[0]));
                        else
                            return Convert.ToString(pars[0]).ToLower();
                    case "uppercase":
                        if (pars[0] is char)
                            return char.ToUpper(Convert.ToChar(pars[0]));
                        else
                            return Convert.ToString(pars[0]).ToUpper();
                    case "lowcase":
                        return char.ToLower(Convert.ToChar(pars[0]));
                    case "upcase":
                        return char.ToUpper(Convert.ToChar(pars[0]));
                    case "concat":
                        return string.Concat(pars);
                    case "paramstr":
                        return param_str(Convert.ToInt32(pars[0]));
                    case "length":
                        {
                            if (pars[0] is string)
                                if (pars.Length == 1)
                                    return (pars[0] as string).Length;
                                else
                                    throw new WrongNumberArguments(name);
                            if (pars[0] is Value)
                            {
                                Value v = pars[0] as Value;
                                if (!v.IsArray)
                                {
                                    NamedValue nv = DebugUtils.GetNullBasedArray(v);
                                    if (nv == null || !nv.IsArray)
                                        throw new WrongTypeOfArgument(name);
                                    if (pars.Length == 2)
                                        throw new WrongNumberArguments(name);
                                    return nv.ArrayLenght;
                                }
                                if (pars.Length > 1)
                                {
                                    uint[] dims = v.ArrayDimensions;
                                    int dim = Convert.ToInt32(pars[1]);
                                    if (dim > dims.Length)
                                        throw new RankException();
                                    return dims[dim];
                                }
                                else
                                    return v.ArrayLenght;
                            }
                            else
                                throw new WrongTypeOfArgument(name);
                        }
                    case "eof":
                        if (pars[0] is Value)
                            return _eof(pars[0] as Value, name);
                        else
                            throw new WrongTypeOfArgument(name);
                    case "eoln":
                        if (pars[0] is Value)
                            return _eoln(pars[0] as Value, name);
                        else
                            throw new WrongTypeOfArgument(name);
                    case "succ":
                        switch (Type.GetTypeCode(pars[0].GetType()))
                        {
                            case TypeCode.Boolean: return !Convert.ToBoolean(pars[0]);
                            case TypeCode.Int32: return Convert.ToInt32(pars[0]) + 1;
                            case TypeCode.Int16: return Convert.ToInt16(pars[0]) + 1;
                            case TypeCode.Byte: return Convert.ToByte(pars[0]) + 1;
                            case TypeCode.SByte: return Convert.ToSByte(pars[0]) + 1;
                            case TypeCode.UInt16: return Convert.ToUInt16(pars[0]) + 1;
                            case TypeCode.UInt32: return Convert.ToUInt32(pars[0]) + 1;
                            case TypeCode.Int64: return Convert.ToInt64(pars[0]) + 1;
                            case TypeCode.UInt64: return Convert.ToUInt64(pars[0]) + 1;
                            case TypeCode.Char: return Convert.ToChar(Convert.ToInt32(Convert.ToChar(pars[0])) + 1);
                            default: throw new WrongTypeOfArgument(name);
                        }
                    case "pred":
                        switch (Type.GetTypeCode(pars[0].GetType()))
                        {
                            case TypeCode.Boolean: return !Convert.ToBoolean(pars[0]);
                            case TypeCode.Int32: return Convert.ToInt32(pars[0]) - 1;
                            case TypeCode.Int16: return Convert.ToInt16(pars[0]) - 1;
                            case TypeCode.Byte: return Convert.ToByte(pars[0]) - 1;
                            case TypeCode.SByte: return Convert.ToSByte(pars[0]) - 1;
                            case TypeCode.UInt16: return Convert.ToUInt16(pars[0]) - 1;
                            case TypeCode.UInt32: return Convert.ToUInt32(pars[0]) - 1;
                            case TypeCode.Int64: return Convert.ToInt64(pars[0]) - 1;
                            case TypeCode.UInt64: return Convert.ToUInt64(pars[0]) - 1;
                            case TypeCode.Char: return Convert.ToChar(Convert.ToInt32(Convert.ToChar(pars[0])) - 1);
                            default: throw new WrongTypeOfArgument(name);
                        }
                    case "low":
                        {
                            if (pars[0] is Value)
                                return _low(pars[0] as Value, name);
                            else
                                throw new WrongTypeOfArgument(name);
                        }
                    case "high":
                        {
                            if (pars[0] is Value)
                                return _high(pars[0] as Value, name);
                            else
                                throw new WrongTypeOfArgument(name);
                        }
                    case "integer":
                        {
                            return Convert.ToInt32(pars[0]);
                        }
                    case "byte":
                        {
                            return Convert.ToByte(pars[0]);
                        }
                    case "shortint":
                        {
                            return Convert.ToSByte(pars[0]);
                        }
                    case "smallint":
                        {
                            return Convert.ToInt16(pars[0]);
                        }
                    case "word":
                        {
                            return Convert.ToUInt16(pars[0]);
                        }
                    case "longword":
                        {
                            return Convert.ToUInt32(pars[0]);
                        }
                    case "int64":
                        {
                            return Convert.ToInt64(pars[0]);
                        }
                    case "uint64":
                        {
                            return Convert.ToUInt64(pars[0]);
                        }
                    case "real":
                        {
                            return Convert.ToDouble(pars[0]);
                        }
                    case "single":
                        {
                            return Convert.ToSingle(pars[0]);
                        }
                    case "char":
                        {
                            return Convert.ToChar(pars[0]);
                        }
                    case "string":
                        {
                            return Convert.ToString(pars[0]);
                        }
                    case "boolean":
                        {
                            return Convert.ToBoolean(pars[0]);
                        }
                        //case "uppercase" : return char.ToUpper(Convert.ToChar(pars[0]));
                        //case "lowercase" : return char.ToLower(Convert.ToChar(pars[0]));

                }
            }
            catch (System.FormatException)
            {
                throw new WrongTypeOfArgument(name);
            }
            /*catch (System.InvalidCastException)
            {
                throw new WrongTypeOfArgument(name);
            }*/
            return null;
        }

        public override void visit(addressed_value _addressed_value)
        {
            throw new NotImplementedException();
        }

        public override void visit(type_definition _type_definition)
        {
            throw new NotImplementedException();
        }

        private DebugType make_debug_type(Type t, List<DebugType> gen_args)
        {
            string name;
            if (t.Assembly == typeof(int).Assembly)
                name = "mscorlib.dll";
            else
                name = t.Assembly.ManifestModule.ScopeName;
            return DebugType.Create(this.debuggedProcess.GetModule(name), (uint)t.MetadataToken, gen_args.ToArray());
        }

        public override void visit(named_type_reference _named_type_reference)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _named_type_reference.names.Count; i++)
            {
                sb.Append(_named_type_reference.names[i].name);
                if (i < _named_type_reference.names.Count - 1)
                    sb.Append('.');
            }
            RetValue res = new RetValue();
            res.managed_type = AssemblyHelper.GetTypeForStatic(sb.ToString());
            if (res.managed_type != null)
                res.type = DebugUtils.GetDebugType(res.managed_type);
            else
                throw new UnknownName(sb.ToString());
            eval_stack.Push(res);
        }

        public override void visit(variable_definitions _variable_definitions)
        {
            throw new NotImplementedException();
        }

        public override void visit(ident_list _ident_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(var_def_statement _var_def_statement)
        {
            throw new NotImplementedException();
        }

        public override void visit(declaration _declaration)
        {
            throw new NotImplementedException();
        }

        public override void visit(declarations _declarations)
        {
            throw new NotImplementedException();
        }

        public override void visit(program_tree _program_tree)
        {
            throw new NotImplementedException();
        }

        public override void visit(program_name _program_name)
        {
            throw new NotImplementedException();
        }

        public override void visit(string_const _string_const)
        {
            RetValue val = new RetValue();
            val.prim_val = _string_const.Value;
            eval_stack.Push(val);
            names.Add("'" + _string_const.Value + "'");
        }

        public override void visit(expression_list _expression_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(dereference _dereference)
        {
            //throw new NotImplementedException();

        }

        public override void visit(roof_dereference _roof_dereference)
        {
            _roof_dereference.dereferencing_value.visit(this);
            RetValue rv = eval_stack.Pop();
            Value v = rv.obj_val.GetPermanentReference();
            rv = new RetValue();
            rv.obj_val = v;
            eval_stack.Push(rv);
        }

        private void check_for_out_of_range(Value v)
        {
            if (v != null)
                try
                {
                    bool b = v.IsObject;
                }
                catch
                {
                    throw new CommonEvaluationError(new IndexOutOfRangeException());
                }
        }

        private Value last_obj = null;

        public override void visit(indexer _indexer)
        {
            bool tmp = by_dot;
            by_dot = true;
            _indexer.dereferencing_value.visit(this);
            by_dot = tmp;
            RetValue rv = eval_stack.Pop();
            List<object> indices = new List<object>();
            names.Add("[");
            for (int i = 0; i < _indexer.indexes.expressions.Count; i++)
            {
                expression e = _indexer.indexes.expressions[i];
                by_dot = false;
                e.visit(this);
                if (i < _indexer.indexes.expressions.Count - 1)
                    names.Add(",");
                RetValue val = eval_stack.Pop();
                if (val.prim_val == null && val.obj_val != null)
                {
                    if (val.obj_val.IsPrimitive)
                        val.prim_val = val.obj_val.PrimitiveValue;
                }
                if (val.prim_val != null)
                {
                    TypeCode code = Type.GetTypeCode(val.prim_val.GetType());
                    switch (code)
                    {
                        //case TypeCode.Boolean : indices.Add((uint)((int)((bool)val.prim_val))); break;
                        case TypeCode.Byte: indices.Add((uint)((byte)val.prim_val)); break;
                        case TypeCode.Char: indices.Add((uint)((char)val.prim_val)); break;
                        case TypeCode.Int16: indices.Add((uint)((System.Int16)val.prim_val)); break;
                        case TypeCode.Int32: indices.Add((int)((System.Int32)val.prim_val)); break;
                        case TypeCode.Int64: indices.Add((uint)((System.Int64)val.prim_val)); break;
                        case TypeCode.SByte: indices.Add((uint)((sbyte)val.prim_val)); break;
                        case TypeCode.UInt16: indices.Add((uint)((System.UInt16)val.prim_val)); break;
                        case TypeCode.UInt32: indices.Add((uint)((System.UInt32)val.prim_val)); break;
                        case TypeCode.UInt64: indices.Add((uint)((System.UInt64)val.prim_val)); break;
                        default: indices.Add(val.prim_val); break;
                    }
                }
                else
                {
                    indices.Add(val.obj_val);
                }

            }
            by_dot = tmp;
            names.Add("]");
            if (rv.obj_val != null)
            {
                if (rv.obj_val is MemberValue && (rv.obj_val as MemberValue).MemberInfo is PropertyInfo)
                {
                    PropertyInfo pi = (rv.obj_val as MemberValue).MemberInfo as PropertyInfo;
                    Type t = AssemblyHelper.GetType(last_obj.Type.FullName);
                    System.Reflection.PropertyInfo rpi = t.GetProperty(pi.Name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
                    /*System.Reflection.MemberInfo[] props = t.GetMembers(System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.CreateInstance);
                    System.Reflection.PropertyInfo rpi = null;
                    foreach (System.Reflection.MemberInfo p in props)
                    if (p is System.Reflection.PropertyInfo && p.Name == pi.Name)
                    {
                        rpi = p as System.Reflection.PropertyInfo;
                        break;
                    }*/
                    RetValue res = new RetValue();
                    res.obj_val = ((rv.obj_val as MemberValue).MemberInfo as PropertyInfo).GetValue(last_obj,
                                                                                                    get_val_arr(indices, false, rpi.GetGetMethod(true)));
                    eval_stack.Push(res);
                    return;
                }
                if (rv.obj_val.IsArray)
                {
                    RetValue res = new RetValue();
                    res.obj_val = rv.obj_val.GetArrayElement(conv_to_uint_arr(indices));
                    check_for_out_of_range(res.obj_val);
                    eval_stack.Push(res);

                }
                else
                {
                    IList<MemberInfo> mis = rv.obj_val.Type.GetMember("get_val", BindingFlags.All);
                    if (mis.Count > 0)
                    {
                        MethodInfo cur_mi = null;
                        int low_bound = 0;
                        foreach (MemberInfo mi in mis)
                        {
                            if (mi is MethodInfo)
                            {
                                cur_mi = mi as MethodInfo;
                                //FieldInfo fi = rv.obj_val.Type.GetMember("LowerIndex",BindingFlags.All)[0] as FieldInfo;
                                System.Reflection.FieldInfo fi = AssemblyHelper.GetType(rv.obj_val.Type.FullName).GetField("LowerIndex");
                                low_bound = Convert.ToInt32(fi.GetRawConstantValue());
                            }
                        }
                        RetValue res = new RetValue();
                        uint[] tmp_indices = new uint[1];
                        int j = 0;
                        try
                        {
                            object obj = indices[j++];
                            int v = 0;
                            if (obj is Value && DebugUtils.IsEnum(obj as Value, out v))
                                tmp_indices[0] = (uint)(v - low_bound);
                            else
                                tmp_indices[0] = (uint)(Convert.ToInt32(obj) - low_bound);
                        }
                        catch (System.FormatException)
                        {
                            throw new WrongTypeInIndexer();
                        }
                        catch (System.InvalidCastException)
                        {
                            throw new WrongTypeInIndexer();
                        }
                        //res.obj_val = cur_mi.Invoke(rv.obj_val,indices.ToArray()) as NamedValue;
                        Value nv = rv.obj_val.GetMember("NullBasedArray");
                        res.obj_val = nv.GetArrayElement(tmp_indices);
                        check_for_out_of_range(res.obj_val);
                        nv = res.obj_val.GetMember("NullBasedArray");
                        while (nv != null && j < indices.Count)
                        {
                            System.Reflection.FieldInfo tmp_fi = AssemblyHelper.GetType(res.obj_val.Type.FullName).GetField("LowerIndex");
                            low_bound = Convert.ToInt32(tmp_fi.GetRawConstantValue());
                            try
                            {
                                object obj = indices[j++];
                                int v = 0;
                                if (obj is Value && DebugUtils.IsEnum(obj as Value, out v))
                                    tmp_indices[0] = (uint)(v - low_bound);
                                else
                                    tmp_indices[0] = (uint)(Convert.ToInt32(obj) - low_bound);
                            }
                            catch (System.FormatException)
                            {
                                throw new WrongTypeInIndexer();
                            }
                            catch (System.InvalidCastException)
                            {
                                throw new WrongTypeInIndexer();
                            }
                            res.obj_val = nv.GetArrayElement(tmp_indices);
                            check_for_out_of_range(res.obj_val);
                            nv = res.obj_val.GetMember("NullBasedArray");

                        }
                        if (j < indices.Count)
                            throw new WrongIndexersNumber();
                        eval_stack.Push(res);
                    }
                    else
                    {
                        string name = get_type_name(rv.obj_val.Type);
                        Type t = AssemblyHelper.GetType(name);
                        if (t == null && declaringType != null)
                            t = AssemblyHelper.GetType(declaringType.FullName + "+" + name);
                        DebugType[] gen_args = rv.obj_val.Type.GetGenericArguments();
                        if (gen_args.Length > 0)
                        {
                            List<Type> gens = new List<Type>();
                            for (int i = 0; i < gen_args.Length; i++)
                                gens.Add(AssemblyHelper.GetType(get_type_name(gen_args[i])));
                            t = t.MakeGenericType(gens.ToArray());
                        }
                        System.Reflection.MemberInfo[] def_members = t.GetDefaultMembers();
                        System.Reflection.PropertyInfo _default_property = null;
                        if (def_members != null && def_members.Length > 0)
                        {
                            foreach (System.Reflection.MemberInfo mi in def_members)
                            {
                                System.Reflection.PropertyInfo pi = mi as System.Reflection.PropertyInfo;
                                if (pi != null)
                                {
                                    _default_property = pi;
                                    break;
                                }
                            }
                        }
                        if (_default_property != null)
                        {
                            if (_default_property.GetGetMethod().GetParameters().Length != indices.Count)
                                throw new WrongIndexersNumber();
                            MethodInfo mi2 = rv.obj_val.Type.GetMember(_default_property.GetGetMethod().Name, Debugger.BindingFlags.All)[0] as MethodInfo;
                            RetValue res = new RetValue();
                            res.obj_val = mi2.Invoke(rv.obj_val, get_val_arr(indices, _default_property.DeclaringType == typeof(string), _default_property.GetGetMethod()));
                            check_for_out_of_range(res.obj_val);
                            eval_stack.Push(res);
                        }
                        else
                            throw new NoIndexerProperty();

                    }
                }
            }
        }

        public static string GetToString(Value v, out bool is_failed)
        {
            is_failed = false;
            try
            {
                //if (v is ArrayElement || true )
                //    return v.AsString != null?v.AsString.Replace("{", "").Replace("}", ""):"";
                IList<MemberInfo> mis = v.Type.GetMember("ToString", Debugger.BindingFlags.All);
                DebugType tmp = v.Type.BaseType;
                while (mis.Count == 0 && tmp != null)
                {
                    mis = tmp.GetMember("ToString", Debugger.BindingFlags.All);
                    tmp = tmp.BaseType;
                    if (tmp != null && tmp.BaseType.FullName == "System.Object")
                        break;
                }
                if (mis.Count == 0)
                    return v.AsString != null ? v.AsString.Replace("{", "").Replace("}", "") : "";
                MethodInfo mi = mis[0] as MethodInfo;
                v = mi.Invoke(v, new Value[0]);
                if (v != null) return v.PrimitiveValue as string;
            }
            catch (Debugger.TooLongEvaluation ex)
            {
                is_failed = true;
                return PascalABCCompiler.StringResources.Get("DEBUG_VIEW_TOO_LONG_EVALUATION");
            }
            catch (System.Exception)
            {

            }
            return null;
        }

        private uint[] conv_to_uint_arr(List<object> arr)
        {
            try
            {
                List<uint> l = new List<uint>();
                foreach (object i in arr)
                    l.Add(Convert.ToUInt32(i));
                return l.ToArray();
            }
            catch
            {
                throw new WrongTypeInIndexer();
            }
        }

        private Value[] get_val_arr(List<object> arr, bool in_str, System.Reflection.MethodInfo mi)
        {
            List<Value> l = new List<Value>();
            for (int i = 0; i < arr.Count; i++)
            {
                object o = arr[i];
                if (o is Value)
                {
                    if (mi.GetParameters()[i].ParameterType == typeof(object) && (o as Value).Type.IsValueType)
                        o = box(o as Value);
                    l.Add(o as Value);
                }
                else
                    if (!in_str)
                    {
                        if (mi.GetParameters()[i].ParameterType == typeof(object) && o.GetType().IsValueType)
                        {
                            if (o.GetType() == typeof(uint) || o.GetType() == typeof(int))
                                o = Convert.ToInt32(o);
                            l.Add(box(DebugUtils.MakeValue(o)));
                        }
                        else
                            l.Add(DebugUtils.MakeValue(o));
                    }
                    else
                        l.Add(DebugUtils.MakeValue((int)o - 1));
            }
            return l.ToArray();
        }

        private string get_type_name(DebugType t)
        {
            string name = t.FullName;
            int ind = name.IndexOf('<');
            if (ind != -1)
            {
                name = name.Substring(0, ind);
                name += "`" + t.GetGenericArguments().Length.ToString();
            }
            return name;
        }

        public override void visit(for_node _for_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(repeat_node _repeat_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(while_node _while_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(if_node _if_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(ref_type _ref_type)
        {
            throw new NotImplementedException();
        }

        public override void visit(diapason _diapason)
        {

        }

        public override void visit(indexers_types _indexers_types)
        {
            throw new NotImplementedException();
        }

        public override void visit(array_type _array_type)
        {
            throw new NotImplementedException();
        }

        public override void visit(label_definitions _label_definitions)
        {
            throw new NotImplementedException();
        }

        public override void visit(procedure_attribute _procedure_attribute)
        {
            throw new NotImplementedException();
        }

        public override void visit(typed_parameters _typed_parametres)
        {
            throw new NotImplementedException();
        }

        public override void visit(formal_parameters _formal_parametres)
        {
            throw new NotImplementedException();
        }

        public override void visit(procedure_attributes_list _procedure_attributes_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(procedure_header _procedure_header)
        {
            throw new NotImplementedException();
        }

        public override void visit(function_header _function_header)
        {
            throw new NotImplementedException();
        }

        public override void visit(procedure_definition _procedure_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(type_declaration _type_declaration)
        {
            throw new NotImplementedException();
        }

        public override void visit(type_declarations _type_declarations)
        {
            throw new NotImplementedException();
        }

        public override void visit(simple_const_definition _simple_const_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(typed_const_definition _typed_const_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(const_definition _const_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(consts_definitions_list _consts_definitions_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(unit_name _unit_name)
        {
            throw new NotImplementedException();
        }

        public override void visit(unit_or_namespace _unit_or_namespace)
        {
            throw new NotImplementedException();
        }

        public override void visit(uses_unit_in _uses_unit_in)
        {
            throw new NotImplementedException();
        }

        public override void visit(uses_list _uses_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(program_body _program_body)
        {
            throw new NotImplementedException();
        }

        public override void visit(compilation_unit _compilation_unit)
        {
            throw new NotImplementedException();
        }

        public override void visit(unit_module _unit_module)
        {
            throw new NotImplementedException();
        }

        public override void visit(program_module _program_module)
        {
            throw new NotImplementedException();
        }

        public override void visit(hex_constant _hex_constant)
        {
            RetValue val = new RetValue();
            val.prim_val = _hex_constant.val;
            eval_stack.Push(val);
        }

        public override void visit(get_address _get_address)
        {
            throw new NotImplementedException();
        }

        public override void visit(case_variant _case_variant)
        {
            throw new NotImplementedException();
        }

        public override void visit(case_node _case_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(method_name _method_name)
        {
            throw new NotImplementedException();
        }

        private bool by_dot;

        private void check_for_static(Value v, string name)
        {
            try
            {
                bool b = v.IsObject;
            }
            catch
            {
                throw new NameIsNotStatic(name);
            }
        }

        public override void visit(dot_node _dot_node)
        {
            bool tmp2 = by_dot;
            by_dot = true;
            _dot_node.left.visit(this);
            by_dot = tmp2;
            RetValue rv = eval_stack.Pop();
            RetValue res = new RetValue();
            if (rv.obj_val != null)
            {
                //if (rv.obj_val.Dereference != null)
                //    rv.obj_val = rv.obj_val.Dereference;
                if (_dot_node.right is ident)
                {
                    if (rv.obj_val.IsNull)
                        throw new CommonEvaluationError(new NullReferenceException());
                    ident id = _dot_node.right as ident;
                    names.Add("." + id.name);
                    //IList<Debugger.MemberInfo> members = rv.obj_val.Type.GetMember(id.name, BindingFlags.All);
                    if (rv.obj_val.IsArray)
                    {
                        if (string.Compare(id.name, "Length", true) == 0)
                            res.prim_val = rv.obj_val.ArrayLenght;
                        else
                            if (string.Compare(id.name, "Rank", true) == 0)
                                res.prim_val = rv.obj_val.ArrayRank;
                    }
                    else
                    {
                        res.obj_val = rv.obj_val.GetMember(id.name);
                        if (res.obj_val == null)
                            throw new UnknownName(id.name);
                        if (res.obj_val is MemberValue && (res.obj_val as MemberValue).MemberInfo is PropertyInfo)
                            last_obj = rv.obj_val;
                    }
                    declaringType = rv.obj_val.Type;
                }
            }
            else if (_dot_node.right is ident)
            {
                ident id = _dot_node.right as ident;
                names.Add("." + id.name);
                string name = build_name(_dot_node.left);
                if (name != null)
                {
                    Type t = AssemblyHelper.GetType(name);
                    if (t != null)
                    {
                        //DebugType dt = DebugType.Create(this.debuggedProcess.GetModule(name),(uint)t.MetadataToken);
                        DebugType dt = DebugUtils.GetDebugType(t);
                        IList<MemberInfo> mis = new List<MemberInfo>();//dt.GetMember(id.name,BindingFlags.All);
                        declaringType = dt;
                        DebugType tmp = dt;
                        while (tmp != null && mis.Count == 0)
                        {
                            try
                            {
                                mis = tmp.GetMember(id.name, BindingFlags.All);
                                tmp = tmp.BaseType;
                            }
                            catch
                            {

                            }
                        }
                        if (mis.Count > 0)
                        {
                            if (mis[0] is FieldInfo)
                            {
                                FieldInfo fi = mis[0] as FieldInfo;
                                res.obj_val = fi.GetValue(null);
                                if (res.obj_val == null)
                                    throw new UnknownName(id.name);
                                check_for_static(res.obj_val, id.name);
                            }
                            else if (mis[0] is PropertyInfo)
                            {
                                PropertyInfo pi = mis[0] as PropertyInfo;
                                res.obj_val = pi.GetValue(null);
                                if (res.obj_val == null)
                                    throw new UnknownName(id.name);
                                check_for_static(res.obj_val, id.name);
                            }
                        }
                        else
                        {
                            System.Reflection.FieldInfo fi = t.GetField(id.name);
                            if (fi != null)
                                res.prim_val = fi.GetRawConstantValue();
                            else
                                throw new UnknownName(id.name);
                        }
                    }
                    else
                    {
                        t = AssemblyHelper.GetType(name + "." + id.name);
                        if (t != null)
                        {
                            res.type = DebugUtils.GetDebugType(t);//DebugType.Create(this.debuggedProcess.GetModule(name),(uint)t.MetadataToken);
                            res.managed_type = t;
                        }
                        //						else 
                        //							throw new UnknownName(id.name);
                    }
                }
            }
            eval_stack.Push(res);
        }

        private string build_name(expression e)
        {
            if (e is dot_node)
            {
                string s = build_name((e as dot_node).left);
                if (s != null)
                    return s + "." + ((e as dot_node).right as ident).name;
                else return s;
            }
            else if (e is ident)
                return (e as ident).name;
            return null;
        }

        public override void visit(empty_statement _empty_statement)
        {
            throw new NotImplementedException();
        }

        public override void visit(goto_statement _goto_statement)
        {
            throw new NotImplementedException();
        }

        public override void visit(labeled_statement _labeled_statement)
        {
            throw new NotImplementedException();
        }

        public override void visit(with_statement _with_statement)
        {
            throw new NotImplementedException();
        }

        public object GetSimpleValue(RetValue rv)
        {
            if (rv.prim_val != null) return rv.prim_val;
            else if (rv.obj_val != null && rv.obj_val.IsPrimitive) return rv.obj_val.PrimitiveValue;
            else if (rv.obj_val != null) return rv.obj_val;
            return null;
        }

        public override void visit(method_call _method_call)
        {
            //throw new NotImplementedException();
            RetValue res = new RetValue();
            if (!for_immediate)
            {
                ident id = _method_call.dereferencing_value as ident;
                List<object> args = new List<object>();
                if (id != null)
                {
                    if (_method_call.parameters != null)
                        foreach (expression e in _method_call.parameters.expressions)
                        {
                            e.visit(this);
                            RetValue rv = eval_stack.Pop();
                            args.Add(GetSimpleValue(rv));
                        }
                    res.prim_val = EvalStandFuncWithParam(id.name, args.ToArray());
                    if (res.prim_val is Value)
                    {
                        res.obj_val = res.prim_val as Value;
                        res.prim_val = null;
                    }
                }
                eval_stack.Push(res);
            }
            else
            {
                Value obj_val = null;
                MethodInfo[] meths = get_method(_method_call.dereferencing_value, out obj_val);
                //ret_val = eval_stack.Pop();
                //if (ret_val.obj_val == null && ret_val.prim_val == null)
                //	return;
                List<Value> args = new List<Value>();
                if (_method_call.parameters != null)
                    foreach (expression e in _method_call.parameters.expressions)
                    {
                        e.visit(this);
                        RetValue rv = eval_stack.Pop();
                        if (rv.obj_val != null)
                            args.Add(rv.obj_val);
                        else if (rv.prim_val != null)
                            args.Add(DebugUtils.MakeValue(rv.prim_val));

                    }
                MethodInfo mi = select_method(meths, get_values_types(args));
                if (mi == null)
                    throw new NoSuitableMethod();
                res.obj_val = mi.Invoke(obj_val, args.ToArray());
                eval_stack.Push(res);
            }
        }

        private DebugType[] get_values_types(List<Value> values)
        {
            List<DebugType> types = new List<DebugType>();
            for (int i = 0; i < values.Count; i++)
                types.Add(values[i].Type);
            return types.ToArray();
        }

        private MethodInfo select_method(MethodInfo[] meths, DebugType[] args)
        {
            if (meths.Length > 0)
                return meths[0];
            return null;
        }

        public override void visit(pascal_set_constant _pascal_set_constant)
        {
            //throw new NotImplementedException();
            List<object> args = new List<object>();
            if (_pascal_set_constant.values != null)
            {
                foreach (expression e in _pascal_set_constant.values.expressions)
                {
                    e.visit(this);
                    RetValue rv = eval_stack.Pop();
                    if (rv.prim_val != null)
                        args.Add(rv.prim_val);
                    else if (rv.obj_val != null)
                    {
                        args.Add(rv.obj_val);
                    }
                }
            }
            List<Value> vargs = new List<Value>();
            foreach (object o in args)
                if (o is Value) vargs.Add(box(o as Value));
                else vargs.Add(box(DebugUtils.MakeValue(o)));
            RetValue res = new RetValue();
            Type t = AssemblyHelper.GetType("PABCSystem.TypedSet");
            string name = t.Assembly.ManifestModule.ScopeName;
            DebugType dt = DebugType.Create(this.debuggedProcess.GetModule(name), (uint)t.MetadataToken);
            res.obj_val = Eval.NewObject(this.debuggedProcess, dt.CorClass);
            MethodInfo mi = res.obj_val.Type.GetMember("CreateIfNeed", BindingFlags.All)[0] as MethodInfo;
            mi.Invoke(res.obj_val, new Value[0]);
            mi = res.obj_val.Type.GetMember("IncludeElement", BindingFlags.All)[0] as MethodInfo;
            foreach (Value v in vargs)
                mi.Invoke(res.obj_val, new Value[] { v });
            eval_stack.Push(res);
        }

        public override void visit(array_const _array_const)
        {
            throw new NotImplementedException();
        }

        public override void visit(write_accessor_name _write_accessor_name)
        {
            throw new NotImplementedException();
        }

        public override void visit(read_accessor_name _read_accessor_name)
        {
            throw new NotImplementedException();
        }

        public override void visit(property_accessors _property_accessors)
        {
            throw new NotImplementedException();
        }

        public override void visit(simple_property _simple_property)
        {
            throw new NotImplementedException();
        }

        public override void visit(index_property _index_property)
        {
            throw new NotImplementedException();
        }

        public override void visit(class_members _class_members)
        {
            throw new NotImplementedException();
        }

        public override void visit(access_modifer_node _access_modifer_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(class_body_list _class_body)
        {
            throw new NotImplementedException();
        }

        public override void visit(class_definition _class_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(default_indexer_property_node _default_indexer_property_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(known_type_definition _known_type_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(set_type_definition _set_type_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(record_const_definition _record_const_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(record_const _record_const)
        {
            throw new NotImplementedException();
        }

        public override void visit(record_type _record_type)
        {
            throw new NotImplementedException();
        }

        public override void visit(enum_type_definition _enum_type_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(char_const _char_const)
        {
            RetValue val = new RetValue();
            val.prim_val = _char_const.cconst;
            eval_stack.Push(val);
            names.Add("'" + _char_const.cconst.ToString() + "'");
        }

        public override void visit(raise_statement _raise_statement)
        {
            throw new NotImplementedException();
        }

        public override void visit(sharp_char_const _sharp_char_const)
        {
            RetValue val = new RetValue();
            val.prim_val = Convert.ToChar(_sharp_char_const.char_num);
            eval_stack.Push(val);
            names.Add("#" + _sharp_char_const.char_num.ToString());
        }

        public override void visit(literal_const_line _literal_const_line)
        {
            string s = "";
            string_const strc;
            sharp_char_const sharpcharconst;
            char_const charconst;
            syntax_tree_node tnf = _literal_const_line.literals[0], tnl = null;
            for (int i = 0; i < _literal_const_line.literals.Count; i++)
            {
                if ((strc = _literal_const_line.literals[i] as string_const) != null)
                    s = s + strc.Value;
                else
                    if ((sharpcharconst = _literal_const_line.literals[i] as sharp_char_const) != null)
                        s = s + Convert.ToChar(sharpcharconst.char_num);
                    else
                        if ((charconst = _literal_const_line.literals[i] as char_const) != null)
                            s = s + charconst.cconst;
                if (i == _literal_const_line.literals.Count - 1)
                    tnl = _literal_const_line.literals[i];
            }
            RetValue val = new RetValue();
            val.prim_val = s;
            eval_stack.Push(val);
        }

        public override void visit(string_num_definition _string_num_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(variant _variant)
        {
            throw new NotImplementedException();
        }

        public override void visit(variant_list _variant_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(variant_type _variant_type)
        {
            throw new NotImplementedException();
        }

        public override void visit(variant_types _variant_types)
        {
            throw new NotImplementedException();
        }

        public override void visit(variant_record_type _variant_record_type)
        {
            throw new NotImplementedException();
        }

        public override void visit(procedure_call _procedure_call)
        {
            throw new NotImplementedException();
        }

        public override void visit(class_predefinition _class_predefinition)
        {
            throw new NotImplementedException();
        }

        public override void visit(nil_const _nil_const)
        {
            RetValue res = new RetValue();
            res.is_null = true;
            eval_stack.Push(res);
        }

        public override void visit(file_type_definition _file_type_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(constructor _constructor)
        {
            throw new NotImplementedException();
        }

        public override void visit(destructor _destructor)
        {
            throw new NotImplementedException();
        }

        public override void visit(inherited_method_call _inherited_method_call)
        {
            throw new NotImplementedException();
        }

        public override void visit(typecast_node _typecast_node)
        {
            _typecast_node.expr.visit(this);
            _typecast_node.type_def.visit(this);
            if (_typecast_node.cast_op == op_typecast.is_op)
                EvalIs();
            else if (_typecast_node.cast_op == op_typecast.as_op)
            {
                eval_stack.Pop();
            }
        }

        public override void visit(interface_node _interface_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(implementation_node _implementation_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(diap_expr _diap_expr)
        {
            throw new NotImplementedException();
        }

        public override void visit(block _block)
        {
            throw new NotImplementedException();
        }

        public override void visit(proc_block _proc_block)
        {
            throw new NotImplementedException();
        }

        public override void visit(array_of_named_type_definition _array_of_named_type_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(array_of_const_type_definition _array_of_const_type_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(literal _literal)
        {
            throw new NotImplementedException();
        }

        public override void visit(case_variants _case_variants)
        {
            throw new NotImplementedException();
        }

        public override void visit(diapason_expr _diapason_expr)
        {
            RetValue res = new RetValue();
            _diapason_expr.left.visit(this);
            RetValue left = eval_stack.Pop();
            _diapason_expr.right.visit(this);
            RetValue right = eval_stack.Pop();
            Type t = AssemblyHelper.GetType("PABCSystem.PABCSystem");
            string name = t.Assembly.ManifestModule.ScopeName;
            DebugType dt = DebugType.Create(this.debuggedProcess.GetModule(name), (uint)AssemblyHelper.GetType("PABCSystem.PABCSystem").MetadataToken);
            Value lv = null;
            Value rv = null;
            if (left.prim_val != null && right.prim_val != null)
            {
                Type left_type = left.prim_val.GetType();
                Type right_type = right.prim_val.GetType();
                if (left.prim_val is IComparable && right.prim_val is IComparable)
                {
                    if ((left.prim_val as IComparable).CompareTo(right.prim_val) > 0)
                        throw new InvalidRangesInDiapason();
                    switch (Type.GetTypeCode(left_type))
                    {
                        case TypeCode.Boolean:
                        case TypeCode.Byte:
                        case TypeCode.Char:
                        case TypeCode.SByte:
                        case TypeCode.UInt16:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64: break;
                        default:
                            throw new InvalidExpressionTypeInDiapason();
                    }
                    switch (Type.GetTypeCode(right_type))
                    {
                        case TypeCode.Boolean:
                        case TypeCode.Byte:
                        case TypeCode.Char:
                        case TypeCode.SByte:
                        case TypeCode.UInt16:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64: break;
                        default:
                            throw new InvalidExpressionTypeInDiapason();
                    }
                }
                else
                    throw new InvalidExpressionTypeInDiapason();
            }
            if (left.prim_val != null)
                lv = (DebugUtils.MakeValue(left.prim_val));
            else
                lv = (left.obj_val);
            if (right.prim_val != null)
                rv = (DebugUtils.MakeValue(right.prim_val));
            else
                rv = (right.obj_val);
            MethodInfo mi = null;
            if (lv.Type.ManagedType != typeof(char))
            {
                mi = dt.GetMember("CreateDiapason", BindingFlags.All)[0] as MethodInfo;
            }
            else
            {
                mi = dt.GetMember("CreateObjDiapason", BindingFlags.All)[0] as MethodInfo;
                lv = box(lv);
                rv = box(rv);
            }
            res.obj_val = mi.Invoke(null, new Value[2] { lv, rv });
            eval_stack.Push(res);
        }

        public override void visit(var_def_list_for_record _var_def_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(record_type_parts _record_type_parts)
        {
            throw new NotImplementedException();
        }

        public override void visit(property_array_default _property_array_default)
        {
            throw new NotImplementedException();
        }

        public override void visit(property_interface _property_interface)
        {
            throw new NotImplementedException();
        }

        public override void visit(property_parameter _property_parameter)
        {
            throw new NotImplementedException();
        }

        public override void visit(property_parameter_list _property_parameter_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(inherited_ident _inherited_ident)
        {
            throw new NotImplementedException();
        }

        public override void visit(format_expr _format_expr)
        {
            if (_format_expr.format2 == null)
                throw new NotImplementedException();
            RetValue res = new RetValue();
            _format_expr.expr.visit(this);
            RetValue expr = eval_stack.Pop();
            _format_expr.format1.visit(this);
            RetValue format1 = eval_stack.Pop();
            RetValue format2 = new RetValue();
            if (_format_expr.format2 != null)
            {
                _format_expr.format2.visit(this);
                format2 = eval_stack.Pop();
            }
            Type t = AssemblyHelper.GetType("PABCSystem.PABCSystem");
            string name = t.Assembly.ManifestModule.ScopeName;
            DebugType dt = DebugType.Create(this.debuggedProcess.GetModule(name), (uint)AssemblyHelper.GetType("PABCSystem.PABCSystem").MetadataToken);
            IList<MemberInfo> meths = dt.GetMember("FormatValue", BindingFlags.All);
            MethodInfo mi = selectFormatMethod(meths, _format_expr.format2 == null ? 2 : 3);
            if (_format_expr.format2 == null)
                res.obj_val = mi.Invoke(null, new Value[2] { MakeValue(expr), MakeValue(format1) });
            else
                res.obj_val = mi.Invoke(null, new Value[3] { MakeValue(expr), MakeValue(format1), MakeValue(format2) });
            eval_stack.Push(res);
        }

        private MethodInfo selectFormatMethod(IList<MemberInfo> meths, int param_count)
        {
            foreach (MethodInfo mi in meths)
            {
                if (mi.ParameterCount == param_count)
                    return mi;
            }
            return null;
        }

        public override void visit(initfinal_part _initfinal_part)
        {
            throw new NotImplementedException();
        }

        public override void visit(token_info _token_info)
        {
            throw new NotImplementedException();
        }

        public override void visit(raise_stmt _raise_stmt)
        {
            throw new NotImplementedException();
        }

        public override void visit(op_type_node _op_type_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(file_type _file_type)
        {
            throw new NotImplementedException();
        }

        public override void visit(known_type_ident _known_type_ident)
        {
            throw new NotImplementedException();
        }

        public override void visit(exception_handler _exception_handler)
        {
            throw new NotImplementedException();
        }

        public override void visit(exception_ident _exception_ident)
        {
            throw new NotImplementedException();
        }

        public override void visit(exception_handler_list _exception_handler_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(exception_block _exception_block)
        {
            throw new NotImplementedException();
        }

        public override void visit(try_handler _try_handler)
        {
            throw new NotImplementedException();
        }

        public override void visit(try_handler_finally _try_handler_finally)
        {
            throw new NotImplementedException();
        }

        public override void visit(try_handler_except _try_handler_except)
        {
            throw new NotImplementedException();
        }

        public override void visit(try_stmt _try_stmt)
        {
            throw new NotImplementedException();
        }

        public override void visit(inherited_message _inherited_message)
        {
            throw new NotImplementedException();
        }

        public override void visit(external_directive _external_directive)
        {
            throw new NotImplementedException();
        }

        public override void visit(using_list _using_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(jump_stmt _jump_stmt)
        {
            throw new NotImplementedException();
        }

        public override void visit(loop_stmt _loop_stmt)
        {
            throw new NotImplementedException();
        }

        public override void visit(foreach_stmt _foreach_stmt)
        {
            throw new NotImplementedException();
        }

        public override void visit(addressed_value_funcname _addressed_RetValue_funcname)
        {
            throw new NotImplementedException();
        }

        public override void visit(named_type_reference_list _named_type_reference_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(template_param_list _template_param_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(template_type_reference _template_type_reference)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _template_type_reference.name.names.Count; i++)
            {
                sb.Append(_template_type_reference.name.names[i].name);
                if (i < _template_type_reference.name.names.Count - 1)
                    sb.Append('.');
            }
            List<DebugType> gen_args = new List<DebugType>();
            List<Type> gen_refl_args = new List<Type>();
            for (int i = 0; i < _template_type_reference.params_list.params_list.Count; i++)
            {
                _template_type_reference.params_list.params_list[i].visit(this);
                RetValue rv = eval_stack.Pop();
                gen_args.Add(rv.type);
                gen_refl_args.Add(rv.managed_type);
            }
            RetValue res = new RetValue();
            res.managed_type = AssemblyHelper.GetTypeForStatic(sb.ToString(), gen_refl_args);
            if (res.managed_type != null)
            {
                res.type = DebugUtils.GetDebugType(res.managed_type, gen_args);
                res.managed_type = res.managed_type.MakeGenericType(gen_refl_args.ToArray());
            }
            else
                throw new UnknownName(sb.ToString());
            eval_stack.Push(res);
        }

        public override void visit(int64_const _int64_const)
        {
            RetValue val = new RetValue();
            val.prim_val = _int64_const.val;
            names.Add(_int64_const.val.ToString());
        }

        public override void visit(uint64_const _uint64_const)
        {
            RetValue val = new RetValue();
            val.prim_val = _uint64_const.val;
            names.Add(_uint64_const.val.ToString());
        }

        public override void visit(new_expr _new_expr)
        {
            throw new NotImplementedException();
        }

        public override void visit(where_type_specificator_list _type_definition_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(where_definition _where_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(where_definition_list _where_definition_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(sizeof_operator _sizeof_operator)
        {
            RetValue rv = new RetValue();
            StringBuilder sb = new StringBuilder();

            if (_sizeof_operator.type_def != null && _sizeof_operator.type_def is named_type_reference)
            {
                named_type_reference ntr = _sizeof_operator.type_def as named_type_reference;
                for (int i = 0; i < ntr.names.Count; i++)
                {
                    sb.Append(ntr.names[i].name);
                    if (i < ntr.names.Count - 1)
                        sb.Append('.');
                }
            }
            Type t = AssemblyHelper.GetType(sb.ToString());
            if (t != null && t.FullName != null)
            {
                if (t == typeof(char))
                {
                    rv.prim_val = 2;
                }
                else if (t == typeof(bool))
                {
                    rv.prim_val = 1;
                }
                else
                {
                    Value v = DebugUtils.MakeValue(t.FullName);
                    //DebugType dt = DebugType.Create(this.debuggedProcess.GetModule(typeof(Type).Assembly.ManifestModule.ScopeName),(uint)typeof(Type).MetadataToken);
                    MethodInfo mi = MethodInfo.GetFromName(this.debuggedProcess, typeof(Type), "GetType", 1);
                    v = mi.Invoke(null, new Value[1] { v });
                    t = typeof(System.Runtime.InteropServices.Marshal);
                    DebugType dt = DebugUtils.GetDebugType(t);//DebugType.Create(this.debuggedProcess.GetModule(name),(uint)t.MetadataToken);
                    IList<MethodInfo> mis = dt.GetMethods();
                    System.Reflection.MethodInfo rmi = t.GetMethod("SizeOf", new Type[1] { typeof(Type) });
                    mi = null;
                    foreach (MethodInfo tmp_mi in mis)
                        if (tmp_mi.Name == "SizeOf" && rmi.MetadataToken == tmp_mi.MetadataToken)
                        {
                            mi = tmp_mi;
                            break;
                        }
                    if (mi != null)
                        rv.obj_val = mi.Invoke(null, new Value[1] { v });
                }
            }
            eval_stack.Push(rv);
        }

        public override void visit(typeof_operator _typeof_operator)
        {
            RetValue rv = new RetValue();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _typeof_operator.type_name.names.Count; i++)
            {
                sb.Append(_typeof_operator.type_name.names[i].name);
                if (i < _typeof_operator.type_name.names.Count - 1)
                    sb.Append('.');
            }
            Type t = AssemblyHelper.GetType(sb.ToString());
            if (t != null && t.FullName != null)
            {
                Value v = DebugUtils.MakeValue(t.FullName);
                //DebugType dt = DebugType.Create(this.debuggedProcess.GetModule(typeof(Type).Assembly.ManifestModule.ScopeName),(uint)typeof(Type).MetadataToken);
                MethodInfo mi = MethodInfo.GetFromName(this.debuggedProcess, typeof(Type), "GetType", 1);
                rv.obj_val = mi.Invoke(null, new Value[1] { v });
            }
            eval_stack.Push(rv);
        }

        public override void visit(compiler_directive _compiler_directive)
        {
            throw new NotImplementedException();
        }

        public override void visit(operator_name_ident _operator_name_ident)
        {
            throw new NotImplementedException();
        }

        public override void visit(var_statement _var_statement)
        {
            throw new NotImplementedException();
        }

        public override void visit(question_colon_expression _question_colon_expression)
        {
            _question_colon_expression.condition.visit(this);
            RetValue cond_expr = eval_stack.Pop();
            _question_colon_expression.ret_if_true.visit(this);
            RetValue true_expr = eval_stack.Pop();
            _question_colon_expression.ret_if_false.visit(this);
            RetValue false_expr = eval_stack.Pop();
            if (cond_expr.prim_val != null)
            {
                if ((bool)cond_expr.prim_val)
                    eval_stack.Push(true_expr);
                else
                    eval_stack.Push(false_expr);
            }
            else
                throw new NotImplementedException();
        }

        public override void visit(expression_as_statement _expression_as_statement)
        {
            _expression_as_statement.expr.visit(this);
        }

        public override void visit(c_scalar_type _c_scalar_type)
        {
            throw new NotImplementedException();
        }

        public override void visit(c_module _c_module)
        {
            throw new NotImplementedException();
        }

        public override void visit(declarations_as_statement _declarations_as_statement)
        {
            throw new NotImplementedException();
        }

        public override void visit(array_size _array_size)
        {
            throw new NotImplementedException();
        }

        public override void visit(enumerator _enumerator)
        {
            throw new NotImplementedException();
        }

        public override void visit(enumerator_list _enumerator_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(c_for_cycle _c_for_cycle)
        {
            throw new NotImplementedException();
        }

        public override void visit(switch_stmt _switch_stmt)
        {
            throw new NotImplementedException();
        }

        public override void visit(type_definition_attr_list _type_definition_attr_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(type_definition_attr _type_definition_attr)
        {
            throw new NotImplementedException();
        }

        public override void visit(lock_stmt _lock_stmt)
        {
            throw new NotImplementedException();
        }
        public override void visit(compiler_directive_if node)
        {
            throw new NotImplementedException();
        }
        public override void visit(compiler_directive_list node)
        {
            throw new NotImplementedException();
        }
        public override void visit(documentation_comment_list node)
        {
            throw new NotImplementedException();
        }
        public override void visit(documentation_comment_section node)
        {
            throw new NotImplementedException();
        }
        public override void visit(documentation_comment_tag node)
        {
            throw new NotImplementedException();
        }
        public override void visit(documentation_comment_tag_param node)
        {
            throw new NotImplementedException();
        }
        public override void visit(declaration_specificator node)
        {
            throw new NotImplementedException();

        }
        public override void visit(ident_with_templateparams node)
        {
            throw new NotImplementedException();
        }
        public override void visit(bracket_expr _bracket_expr)
        {
            _bracket_expr.expr.visit(this);
        }
        public override void visit(attribute _attribute)
        {

        }

        public override void visit(attribute_list _attribute_list)
        {

        }

        public override void visit(simple_attribute_list _simple_attribute_list)
        {

        }

        public override void visit(function_lambda_definition _function_lambda_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(function_lambda_call _function_lambda_call)
        {
            throw new NotImplementedException();
        }
        public override void visit(semantic_check _semantic_check)
        {
            throw new NotImplementedException();
        }
        public override void visit(lambda_inferred_type lit) //lroman//
        {

        }
        public override void visit(same_type_node stn) //SS 22/06/13//
        {
        }
        public override void visit(name_assign_expr _name_assign_expr) // SSM 27.06.13
        {
        }
        public override void visit(name_assign_expr_list _name_assign_expr_list) // SSM 27.06.13
        {
        }
        public override void visit(unnamed_type_object _unnamed_type_object) // SSM 27.06.13
        {
        }
        public override void visit(semantic_type_node stn) // SSM 
        {
        }
    }
}
