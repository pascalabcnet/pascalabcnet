// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Все типы семантических ошибок компилятора.
using System;
using System.Collections.Generic;

using PascalABCCompiler.SemanticTree;

using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.TreeConverter
{
    public class StringResources
    {
        public static string Get(string key)
        {
            return PascalABCCompiler.StringResources.Get("SEMANTICERROR_" + key);
        }

        public static string Get(string key, params object[] values)
        {
            return (string.Format(Get(key), values));
        }
    }

    /*public static class CompilationErrorMessages
    {
        public static string ERROR_MESSAGE = "ERROR_MESSAGE";
    }

    public class CompilationErrorMessageWithLocation : CompilationErrorWithLocation
    {
        string text;
        public CompilationErrorMessageWithLocation(string text, location loc)
            : base(loc)
        {
            this.text = text;
        }
        public override string ToString()
        {
            return string.Format(StringResources.Get(text));
        }
    }*/

    /*public class ConcreteCompilationError: CompilationErrorWithLocation
    {
        public enum msg { msg1, msg2, msg3 };
        msg _msg;
        public ConcreteCompilationError(msg _msg, location loc)
            : base(loc)
        {
            this._msg = _msg;
        }
        public override string ToString()
        {
            switch (_msg)
            {
                case msg.msg1: return string.Format(StringResources.Get("MSG1"));
                    ...
            }
            
        }
    }*/

    public class CompilationError : Errors.SemanticError
    {
        public CompilationError()
            : base("???", "Undefined file_name")
        {
        }

        //TODO: Сделать static.
        public string loc_to_string(ILocation loc)
        {
            if (loc == null) return "Undefined location(!)";
            string res = "File:  " + loc.document.file_name;
            res += "  line:  " + loc.begin_line_num.ToString();
            res += "  column:  " + loc.begin_column_num.ToString();
            return res;
        }

        //TODO: Сделать static. И вообще этот метод нужно вынести в location т.к. он определен в нескольких местах. И не плохо было бы сделать два вида интерфейса ILocated - один для получения ILocation а второй для получения location.
        public SemanticTree.ILocation get_location(semantic_node sn)
        {
            SemanticTree.ILocated iloc = sn as SemanticTree.ILocated;
            if (iloc == null)
            {
                return null;
            }
            return iloc.Location;
        }

        public override string Message
        {
            get
            {
                return (this.ToString());
            }
        }

    }

    public class CompilationErrorWithLocation : CompilationError
    {
        private location _loc;

        public CompilationErrorWithLocation(location loc)
        {
            this._loc = loc;
        }

        public location loc
        {
            get
            {
                return _loc;
            }
            set
            {
                _loc = value;
            }
        }
        public override ILocation Location
        {
            get
            {
                return loc;
            }
        }
    }

    // SSM - 01.2014 - ужасно плодить много классов ошибок - достаточно сделать один общий для простых ошибок
    public class SimpleSemanticError : CompilationErrorWithLocation
    {
        protected string ErrResourceString;
        protected object[] values;

        public SimpleSemanticError(location loc, string ErrResourceString, params object[] values): base(loc)
        {
            this.ErrResourceString = ErrResourceString;
            this.values = values;
        }

        public string ErrorResourceString
        {
            get
            {
                return ErrResourceString;
            }
        }

        public override string ToString()
        {
            if (values != null)
                return string.Format(StringResources.Get(ErrResourceString), values);
            else
                return string.Format(StringResources.Get(ErrResourceString), "");
        }
    }

    public class SourceFileError : CompilationError
    {
        private string _filename;

        public SourceFileError(string file_name)
        {
            _filename = file_name;
        }

        public override string ToString()
        {
            return string.Format(StringResources.Get("ERROR_LOADING_RESOURCE_FILE_{0}"), _filename);
        }
    }

    public class SaveAssemblyError : CompilationErrorWithLocation
    {
        private string _text;

        public SaveAssemblyError(string text, location loc): base(loc)
        {
            _text = text;
        }

        public override string ToString()
        {
            return string.Format(StringResources.Get("SAVE_ASSEMBLY_ERROR_{0}"), _text);
        }
    }

    public class TwoTypeConversionsPossible : CompilationError
    {

        private readonly expression_node _en;
        private readonly type_conversion _first;
        private readonly type_conversion _second;

        public TwoTypeConversionsPossible(expression_node expression_node, type_conversion first,
            type_conversion second)
        {
            _en = expression_node;
            _first = first;
            _second = second;
        }

        public expression_node expression_node
        {
            get
            {
                return _en;
            }
        }

        public type_conversion first
        {
            get
            {
                return _first;
            }
        }

        public type_conversion second
        {
            get
            {
                return _second;
            }
        }

        public override string ToString()
        {
            //return ("Possible two type convertions\n"+_en.location.ToString());
            return (string.Format(StringResources.Get("TWO_TYPE_CONVERTIONS_POSSIBLE_FIRST_TYPE_{0}_SECOND_TYE_{1}"),
                _first.to?.PrintableName??"null", _second.to?.PrintableName??"null"));
        }

        public override ILocation Location
        {
            get
            {
                if (_en!=null)
                    return _en.location;
                return null;
            }
        }

    }

    public class TwoTypeConversionsPossibleT : CompilationErrorWithLocation
    {

        private readonly type_conversion _first;
        private readonly type_conversion _second;

        public TwoTypeConversionsPossibleT(type_conversion first, type_conversion second, location loc): base(loc)
        {
            _first = first;
            _second = second;
        }

        public type_conversion first
        {
            get
            {
                return _first;
            }
        }

        public type_conversion second
        {
            get
            {
                return _second;
            }
        }

        public override string ToString()
        {
            return (string.Format(StringResources.Get("TWO_TYPE_CONVERTIONS_POSSIBLE_FIRST_TYPE_{0}_SECOND_TYE_{1}"),
                _first.to.PrintableName, _second.to.PrintableName));
        }

        public override ILocation Location
        {
            get
            {
                return loc;
            }
        }

    }

    public class GenericFunctionCannotBeAnArgument : CompilationErrorWithLocation
    {
        private readonly string _name;
        public GenericFunctionCannotBeAnArgument(string name, location loc)
            : base(loc)
        {
            _name = name;
        }
        public type_node name { get; }
        public override string ToString()
        {
            return string.Format(StringResources.Get("GENERIC_FUNCTION_{0}_CANNOT_BE_AN_ARGUMENT"), _name);
        }
    }

    public class CanNotConvertTypes : CompilationErrorWithLocation
    {

        private readonly expression_node _en;
        private readonly type_node _from;
        private readonly type_node _to;

        public CanNotConvertTypes(expression_node expression_node, type_node from, type_node to, location loc)
            :base(loc)
        {
            _en = expression_node;
            _from = from;
            _to = to;
        }

        public expression_node expression_node
        {
            get
            {
                return _en;
            }
        }

        public type_node from
        {
            get
            {
                return _from;
            }
        }

        public type_node to
        {
            get
            {
                return _to;
            }
        }

        public override string ToString()
        {
            /*
			string res="Can not convert types:";
			res+="\nFrom: "+_from.PrintableName;
			res+="\nTo:  "+_to.PrintableName;
			res+="\n"+_en.location.ToString();
			return res;
            */
           if (_from == null_type_node.get_type_node())
           	return StringResources.Get("NIL_WITH_VALUE_TYPES_NOT_ALLOWED");
            if (_from is delegated_methods && (_from as delegated_methods).empty_param_method != null && (_from as delegated_methods).empty_param_method.simple_function_node.return_value_type is undefined_type)
                return string.Format(StringResources.Get("RETURN_TYPE_UNDEFINED_{0}"), (_from as delegated_methods).empty_param_method.simple_function_node.name);
            return (string.Format(StringResources.Get("CAN_NOT_CONVERT_TYPES_FROM_{0}_TO_{1}"), _from.PrintableName, _to.PrintableName));
        }

        public override ILocation Location
        {
            get
            {
                if (_en!=null && _en.location != null)
                    return _en.location;
                return base.Location;
            }
        }
    }

    public class NoFunctionWithThisName : CompilationError
    {
        private readonly ILocation _loc;

        public NoFunctionWithThisName(ILocation loc)
        {
            _loc = loc;
        }

        public ILocation loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return ("No function with this name\n"+loc_to_string(_loc));
            return (StringResources.Get("NO_FUNCION_WITH_THIS_NAME"));
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class NoFunctionWithSameParametresNum : CompilationErrorWithLocation
    {
        private readonly bool _is_alone;
        private function_node first_function;
        internal bool is_constructor = false;

        public NoFunctionWithSameParametresNum(location loc, bool is_alone, function_node first_function)
            :base(loc)
        {
            _is_alone = is_alone;
            this.first_function = first_function;
            if (first_function is compiled_constructor_node || first_function is common_method_node && (first_function as common_method_node).is_constructor)
                is_constructor = true;
        }

        public bool is_alone_function_defined
        {
            get
            {
                return _is_alone;
            }
        }

        public override string ToString()
        {
            if (_is_alone)
            {
                if (is_constructor)
                    return StringResources.Get("INVALID_CONSTRUCTOR_PARAMETERS_NUM");
                if (first_function != null && first_function.return_value_type != null)
                    if(first_function.IsOperator)
                        return StringResources.Get("INVALID_OPERATOR_PARAMETERS_NUM");
                    else
                        return StringResources.Get("INVALID_FUNCTION_PARAMETERS_NUM");
                return StringResources.Get("INVALID_PROCEDURE_PARAMETERS_NUM");
            }
            if (is_constructor)
                return StringResources.Get("NO_CONSTRUCTOR_WITH_SAME_PARAMETERS_NUM");
            return StringResources.Get("NO_FUNCTION_WITH_SAME_PARAMETERS_NUM",((first_function == null) ? "" : first_function.name));
        }

    }

    public class NoFunctionWithSameArguments : CompilationError
    {
        private readonly ILocation _loc;
        private readonly bool _is_alone;
        private string _name;

        public NoFunctionWithSameArguments(string name, ILocation loc, bool is_alone)
        {
            _loc = loc;
            _is_alone = is_alone;
            _name = name;
        }

        public ILocation loc
        {
            get
            {
                return _loc;
            }
        }

        public bool is_alone_function_defined
        {
            get
            {
                return _is_alone;
            }
        }
        public string name
        {
            get
            {
                return _name;
            }
        }

        public override string ToString()
        {
            //return ("No function with same arguments\n"+loc_to_string(_loc)); - старое

            //if (_is_alone) // убрал 07.05.23. Общее сообщение об ошибке лучше
            //{
            //    return (StringResources.Get("INVALID_FUNCTION_ARGUMENTS_{0}",name));
            //}
            return (StringResources.Get("NO_OVERLOADED_FUNCTION_{0}_WITH_SAME_ARGUMENTS",name));
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class SeveralFunctionsCanBeCalled : CompilationError
    {
        private readonly ILocation _loc;
        private readonly List<function_node> _possible_functions;

        public SeveralFunctionsCanBeCalled(ILocation loc, List<function_node> set_of_possible_functions)
        {
            _loc = loc;
            _possible_functions = set_of_possible_functions;
        }

        public ILocation loc
        {
            get
            {
                return _loc;
            }
        }

        public List<function_node> set_of_possible_functions
        {
            get
            {
                return _possible_functions;
            }
        }

        public override string ToString()
        {
            var name = set_of_possible_functions[0].name;
            //return ("Several functions can be called\n"+loc_to_string(_loc));
            return (StringResources.Get("SEVERAL_FUNCTIONS_{0}_CAN_BE_CALLED",name));
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class FunctionNameIsUsedToDefineSomethigElse : CompilationError
    {

        private readonly ILocation _loc;
        private readonly definition_node _def;

        public FunctionNameIsUsedToDefineSomethigElse(ILocation loc, definition_node def)
        {
            _loc = loc;
            _def = def;
        }

        public ILocation loc
        {
            get
            {
                return _loc;
            }
        }

        public definition_node definition
        {
            get
            {
                return _def;
            }
        }

        public override string ToString()
        {
            //string res="Function name is used to define something else";
            //res+="\nFirst definition: "+loc_to_string(_loc);
            //res+="\nSecond definition: "+loc_to_string(_loc);
            //return res;
            return (StringResources.Get("FUNCTION_NAME_USED_TO_DEFINE_ANOTHER_KIND_OF_OBJECT"));
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class FunctionDuplicateDefinition : CompilationError
    {
        private readonly function_node _first;
        private readonly function_node _second;

        public FunctionDuplicateDefinition(function_node first, function_node second)
        {
            _first = first;
            _second = second;
        }

        public function_node first
        {
            get
            {
                return _first;
            }
        }

        public function_node second
        {
            get
            {
                return _second;
            }
        }

        public override string ToString()
        {
            //string res="Function duplicate definition";
            //ILocation loc_first=get_location(_first);
            //ILocation loc_second=get_location(_second);
            //res+="\nFirst definition: "+loc_to_string(loc_first);
            //res+="\nSecond definition: "+loc_to_string(loc_second);
            //return res;
            return (StringResources.Get("FUNCTION_DUPLICATE_DEFINITION"));
        }

        public override ILocation Location
        {
            get
            {
                return get_location(_second);
            }
        }
    }

    /*->*/
    public class AutoClassMustNotHaveParentsWithFields : CompilationErrorWithLocation
    {
        public AutoClassMustNotHaveParentsWithFields(location loc) : base(loc) { }
        public override string ToString()
        {
            return string.Format(StringResources.Get("AUTO_CLASS_MUST_NOT_HAVE_PARENTS_WITH_FIELDS"));
        }
    }
    /*->*/

    public class NameRedefinition : CompilationError
    {
        private readonly ILocation _first;
        private readonly ILocation _second;
        private string _name;

        public NameRedefinition(string name,ILocation first, ILocation second)
        {
            _first = first;
            _second = second;
            _name = name;
        }

        public ILocation first
        {
            get
            {
                return _first;
            }
        }

        public ILocation second
        {
            get
            {
                return _second;
            }
        }

        public override string ToString()
        {
            return string.Format(StringResources.Get("NAME_REDEFINITION_{0}"), _name);
        }

        public override ILocation Location
        {
            get
            {
                return (_second);
            }
        }
    }

    public class PossibleTwoTypeConversionsInFunctionCall : CompilationError
    {
        private readonly ILocation _loc;
        private readonly type_conversion _first;
        private readonly type_conversion _second;

        public PossibleTwoTypeConversionsInFunctionCall(ILocation loc, type_conversion first,
            type_conversion second)
        {
            _loc = loc;
            _first = first;
            _second = second;
        }

        public ILocation loc
        {
            get
            {
                return _loc;
            }
        }

        public type_conversion first
        {
            get
            {
                return _first;
            }
        }

        public type_conversion second
        {
            get
            {
                return _second;
            }
        }

        public override string ToString()
        {
            //return ("Possible two type conversions in function call:\n  "+loc_to_string(_loc));
            return (StringResources.Get("POSSIBLE_TWO_TYPE_CONVERTIONS"));
        }

        public override ILocation Location
        {
            get
            {
                return (_loc);
            }
        }

    }

    public class ThisIsNotFunctionName : CompilationError
    {
        //private readonly function_call _fc;
        private readonly ILocation _definition_location;

        public ThisIsNotFunctionName(ILocation definition_location)
        {
            _definition_location = definition_location;
        }

        public ILocation definition_location
        {
            get
            {
                return _definition_location;
            }
        }

        public override string ToString()
        {
            //return ("This is not function name:\n "+loc_to_string(_definition_location));
            return (StringResources.Get("THIS_IS_NOT_FUNCTION_NAME"));
        }

        public override ILocation Location
        {
            get
            {
                return (_definition_location);
            }
        }
    }

    public class UndefinedNameReference : CompilationErrorWithLocation
    {
        private readonly string _name;

        public UndefinedNameReference(string name, location loc)
            :base(loc)
        {
            _name = name;
        }

        public string name
        {
            get
            {
                return _name;
            }
        }

        public override string ToString()
        {
            if (_name.ToLower().StartsWith("$rv_"))
            {
                return string.Format(StringResources.Get("UNDEFINED_NAME_RESULT_IN_{0}"), _name.Remove(0,4));
            }
            return string.Format(StringResources.Get("UNDEFINED_NAME_REFERENCE_{0}"), _name);
        }        
    }

    public class NameCannotHaveGenericParameters : CompilationErrorWithLocation
    {
        private readonly string _name;

        public NameCannotHaveGenericParameters(string name, location loc)
            : base(loc)
        {
            _name = name;
        }

        public string name
        {
            get
            {
                return _name;
            }
        }

        public override string ToString()
        {
            return string.Format(StringResources.Get("NAME_CANNOT_HAVE_GENERIC_PARAMETERS_{0}"), _name);
        }
    }
    /*->*/
    public class EventNameExpected : CompilationErrorWithLocation
    {
        public EventNameExpected(location location)
            :base(location)
        {
        }

        public override string ToString()
        {
            return StringResources.Get("EVENT_NAME_EXPECTED");
        }
    }
    /*<-*/

    public class ExpectedAnotherKindOfObject : CompilationError
    {
        private readonly general_node_type[] _expected_node_types;
        private readonly general_node_type _met_node_type;

        private readonly ILocation _def_location;
        private readonly ILocation _use_location;

        public ExpectedAnotherKindOfObject(general_node_type[] expected_node_types, general_node_type met_node_type,
            ILocation def_location, ILocation use_location)
        {
            _expected_node_types = expected_node_types;
            _met_node_type = met_node_type;
            _def_location = def_location;
            _use_location = use_location;
        }

        public general_node_type[] expected_node_types
        {
            get
            {
                return _expected_node_types;
            }
        }

        public general_node_type met_node_type
        {
            get
            {
                return _met_node_type;
            }
        }

        public ILocation def_location
        {
            get
            {
                return _def_location;
            }
        }

        public ILocation use_location
        {
            get
            {
                return _use_location;
            }
        }

        public override string ToString()
        {
            //string res="Expected another kind of object:\n";
            //res+="Meet: "+_met_node_type.ToString();
            //res+="\nUse location: "+loc_to_string(_use_location);
            //res+="\nDefinition location: "+loc_to_string(_def_location);
            //return res;
            foreach(general_node_type gtn in _expected_node_types)
                if(gtn == general_node_type.variable_node)
                    return StringResources.Get("EXPECTED_VARIABLE");
            return StringResources.Get("EXPECTED_ANOTHER_KIND_OF_OBJECT");
        }

        public override ILocation Location
        {
            get
            {
                return (_use_location);
            }
        }

    }

    /*public class ThisNameIsUsedToDefineAnotherKindOfObject : CompilationError
    {
        private readonly name_information_type _nit;
        private readonly ILocation _first;
        private readonly ILocation _second;

        public ThisNameIsUsedToDefineAnotherKindOfObject(name_information_type nit,ILocation first,ILocation second)
        {
            _nit=nit;
            _first=first;
            _second=second;
        }

        public name_information_type name_information_type
        {
            get
            {
                return _nit;
            }
        }

        public ILocation first
        {
            get
            {
                return _first;
            }
        }

        public ILocation second
        {
            get
            {
                return _second;
            }
        }

    }*/

    /*public class ExpectedVariableOrFunctionName : CompilationError
    {
        private readonly location _loc;
        private readonly location _first_def;

        public ExpectedVariableOrFunctionName(location location,location first_definition)
        {
            _loc=location;
            _first_def=first_definition;
        }

        public location location
        {
            get
            {
                return _loc;
            }
        }

        public location first_definition
        {
            get
            {
                return _first_def;
            }
        }
    }

    public class ForLoopControlMustBeSimpleLocalVariable : CompilationError
    {
        private readonly location _loc;

        public ForLoopControlMustBeSimpleLocalVariable(location location)
        {
            _loc=location;
        }

        public location name_reference_location
        {
            get
            {
                return _loc;
            }
        }
    }*/

    /*public class ExpectedEnumerableType : CompilationError
    {
        private readonly variable_node _vn;

        public ExpectedEnumerableType(variable_node variable_node)
        {
            _vn=variable_node;
        }

        public variable_node variable_node
        {
            get
            {
                return _vn;
            }
        }
    }*/

    /*public class NotAddressedExpressionInLeftPartOfAssign : CompilationError
    {
        private readonly expression_node _left_expr;

        public NotAddressedExpressionInLeftPartOfAssign(expression_node left_expression)
        {
            _left_expr=left_expression;
        }

        public expression_node left_expression
        {
            get
            {
                return _left_expr;
            }
        }
    }

    public class FileIsNotAssembly : CompilationError
    {
        private readonly string _file_name;
        private readonly location _loc;

        public FileIsNotAssembly(string file_name,location loc)
        {
            _file_name=file_name;
            _loc=loc;
        }

        public string file_name
        {
            get
            {
                return _file_name;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }
    }

    public class UnitNotFound : CompilationError
    {
        private readonly string _file_name;
        private readonly location _loc;

        public UnitNotFound(string file_name,location loc)
        {
            _file_name=file_name;
            _loc=loc;
        }

        public string file_name
        {
            get
            {
                return _file_name;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }
    }

    public class MainProgramUnitNotFound : CompilationError
    {
        private readonly string _file_name;

        public MainProgramUnitNotFound(string file_name)
        {
            _file_name=file_name;
        }

        public string file_name
        {
            get
            {
                return _file_name;
            }
        }
    }*/

    public class OperatorCanNotBeAppliedToThisType : CompilationError
    {
        private readonly string _operator_name;
        private readonly expression_node _expr;

        public OperatorCanNotBeAppliedToThisType(string operator_name, expression_node expr)
        {
            _operator_name = operator_name;
            _expr = expr;
        }

        public string operator_name
        {
            get
            {
                return _operator_name;
            }
        }

        public expression_node expr
        {
            get
            {
                return _expr;
            }
        }

        public override string ToString()
        {
            //return (_operator_name + "   can not be applied to:\n" + _expr.location.ToString());
            return (string.Format(StringResources.Get("OPERATOR_{0}_CAN_NOT_APPLIED_TO_TYPE_{1}"), _operator_name, _expr.type.PrintableName));
        }

        public override ILocation Location
        {
            get
            {
                return _expr.location;
            }
        }

    }

    public class OperatorCanNotBeAppliedToThisTypes : CompilationErrorWithLocation
    {
        private readonly string _operator_name;
        private readonly expression_node _left;
        private readonly expression_node _right;

        public OperatorCanNotBeAppliedToThisTypes(string operator_name, expression_node left, expression_node right,location loc)
            :base(loc)
        {
            _operator_name = operator_name;
            _left = left;
            _right = right;
        }

        public string operator_name
        {
            get
            {
                return _operator_name;
            }
        }

        public expression_node left
        {
            get
            {
                return _left;
            }
        }

        public expression_node right
        {
            get
            {
                return _right;
            }
        }

        public override string ToString()
        {
            //string res=_operator_name+" can not be applied to types:  ";
            //res+=_left.type.PrintableName+" and "+_right.type.PrintableName;
            //res += "\nLeft: " + _left.location.ToString();
            //res += "\nRight: " + _right.location.ToString();
            //return res;
            if(left.type!=right.type)
                return string.Format(StringResources.Get("OPERATOR_{0}_CAN_NOT_BE_APPLIED_TO_TYPES_{1}_AND_{2}"),_operator_name, left.type.PrintableName, right.type.PrintableName);
            else
                return string.Format(StringResources.Get("OPERATOR_{0}_CAN_NOT_BE_APPLIED_TO_TYPE_{1}"), _operator_name, left.type.PrintableName);
        }


    }

    public class TwoOperatorsCanBeCalled : CompilationError
    {
        private readonly function_node _first_func;
        private readonly function_node _second_func;

        private readonly expression_node _left;
        private readonly expression_node _right;

        public TwoOperatorsCanBeCalled(function_node first_func, function_node second_func, expression_node left,
            expression_node right)
        {
            _first_func = first_func;
            _second_func = second_func;
            _left = left;
            _right = right;
        }

        public function_node first_funct
        {
            get
            {
                return _first_func;
            }
        }

        public function_node second_func
        {
            get
            {
                return _second_func;
            }
        }

        public expression_node left
        {
            get
            {
                return _left;
            }
        }

        public expression_node right
        {
            get
            {
                return _right;
            }
        }

        public override string ToString()
        {
            //return ("Two operators can be called  " + left.location.ToString());
            return (string.Format(StringResources.Get("TWO_OPERATORS_CAN_BE_CALLED_{0}_AND_{1}"),
                _first_func.name, _second_func.name));
        }

        public override ILocation Location
        {
            get
            {
                return _left.location;
            }
        }

    }

    public class ClassCanNotBeDefinedInTypeOrFunction : CompilationError
    {
        private string _type_name;
        private ILocation _definition_loc;

        public ClassCanNotBeDefinedInTypeOrFunction(string type_name, ILocation definition_loc)
        {
            _type_name = type_name;
            _definition_loc = definition_loc;
        }

        public string type_name
        {
            get
            {
                return _type_name;
            }
        }

        public ILocation definition_location
        {
            get
            {
                return _definition_loc;
            }
        }

        public override string ToString()
        {
            //return ("Type can not be defined in type: "+loc_to_string(_definition_loc));
            return (StringResources.Get("CLASS_CAN_NOT_BE_DEFINED_IN_TYPE_OR_FUNCTION"));
        }

        public override ILocation Location
        {
            get
            {
                return _definition_loc;
            }
        }
    }

    public class ParameterReferenceInDefaultParameterNotAllowed: CompilationErrorWithLocation
    {
        public ParameterReferenceInDefaultParameterNotAllowed(location loc):base(loc)
        {

        }

        public override string ToString()
        {
            //return ("Only one parameter name with default value allowed: "+loc_to_string(_params_location));
            return (StringResources.Get("PARAMETER_REFERENCE_IN_DEFAULT_PARAMETER_NOT_ALLOWED"));
        }
    }

    public class OnlyOneParameterNameWithDefaultValueAllowed : CompilationError
    {
        private ILocation _params_location;

        public OnlyOneParameterNameWithDefaultValueAllowed(ILocation params_location)
        {
            _params_location = params_location;
        }

        public ILocation params_location
        {
            get
            {
                return _params_location;
            }
        }

        public override string ToString()
        {
            //return ("Only one parameter name with default value allowed: "+loc_to_string(_params_location));
            return (StringResources.Get("ONLY_ONE_PARAMETER_NAME_WITH_DEFAULT_VALUE_ALLOWED"));
        }

        public override ILocation Location
        {
            get
            {
                return _params_location;
            }
        }
    }

    public class NeedDefaultValueForParameter : CompilationError
    {
        private ILocation _param_location;

        public NeedDefaultValueForParameter(ILocation param_location)
        {
            _param_location = param_location;
        }

        public ILocation param_location
        {
            get
            {
                return _param_location;
            }
        }

        public override string ToString()
        {
            //return ("Need default value for parameter:  "+loc_to_string(_param_location));
            return (StringResources.Get("NEED_DEFAULT_VALUE_FOR_PARAMETER"));
        }

        public override ILocation Location
        {
            get
            {
                return _param_location;
            }
        }
    }

    public class DuplicateAttributeDefinition : CompilationError
    {
        private ILocation _first_attribute_location;
        private ILocation _second_attribute_location;

        public DuplicateAttributeDefinition(ILocation first_attribute_location, ILocation second_attribute_location)
        {
            _first_attribute_location = first_attribute_location;
            _second_attribute_location = second_attribute_location;
        }

        public ILocation first_attribute_location
        {
            get
            {
                return _first_attribute_location;
            }
        }

        public ILocation second_attribute_location
        {
            get
            {
                return _second_attribute_location;
            }
        }

        public override string ToString()
        {
            //string res="Duplicate attribute definition:";
            //res+="\nFirst: "+loc_to_string(_first_attribute_location);
            //res+="\nSecond: "+loc_to_string(_second_attribute_location);
            //return res;
            return (StringResources.Get("DUPLICATE_ATTRIBUTE_APPLICATION"));
        }

        public override ILocation Location
        {
            get
            {
                return _second_attribute_location;
            }
        }
    }

    public class VarParametersCanNotHaveDefaultValue : CompilationError
    {
        private ILocation _param_location;

        public VarParametersCanNotHaveDefaultValue(ILocation param_location)
        {
            _param_location = param_location;
        }

        public ILocation param_location
        {
            get
            {
                return _param_location;
            }
        }

        public override string ToString()
        {
            //return ("Var parameters can not have default value: "+loc_to_string(_param_location));
            return (StringResources.Get("VAR_PARAMETER_CAN_NOT_HAVE_DEFAULT_VALUE"));
        }

        public override ILocation Location
        {
            get
            {
                return _param_location;
            }
        }

    }

    public class FunctionMustBeWithOverloadDirective : CompilationError
    {
        private common_function_node _first_function;
        private common_function_node _second_function;

        public FunctionMustBeWithOverloadDirective(common_function_node first_function, common_function_node second_function)
        {
            _first_function = first_function;
            _second_function = second_function;
        }

        private common_function_node first_function
        {
            get
            {
                return _first_function;
            }
        }

        private common_function_node second_function
        {
            get
            {
                return _second_function;
            }
        }

        public override string ToString()
        {
            //string res="Function must be with overload directive:";
            //res+="\nFirst definition: "+_first_function.loc.ToString();
            //res+="\nSecond definition: "+_second_function.loc.ToString();
            //return res;
            return (StringResources.Get("DUPLICATE_FUNCTION_DEFINITION")+" "+ first_function.name);
        }

        public override ILocation Location
        {
            get
            {
                return _second_function.Location;
            }
        }
    }

    /*public class ThisIsNotFunctionName : CompilationError
    {
        private ILocation _ref_loc;
        private definition_node _prev_def;

        public ThisIsNotFunctionName(ILocation ref_loc,definition_node prev_def)
        {
            _ref_loc=ref_loc;
            _prev_def=prev_def;
        }

        public ILocation ref_loc
        {
            get
            {
                return _ref_loc;
            }
        }

        public definition_node prev_def
        {
            get
            {
                return _prev_def;
            }
        }
    }*/

    /*public class CanNotAssignToThisExpression : CompilationError
    {
        private expression_node _expr;

        public CanNotAssignToThisExpression(expression_node expr)
        {
            _expr=expr;
        }

        public expression_node expr
        {
            get
            {
                return _expr;
            }
        }
    }*/

    public class ThisExpressionCanNotBePassedAsVarParameter : CompilationError
    {
        private expression_node _expr;

        public ThisExpressionCanNotBePassedAsVarParameter(expression_node expr)
        {
            _expr = expr;
        }

        public expression_node expr
        {
            get
            {
                return _expr;
            }
        }

        public override string ToString()
        {
            //return ("This expression can not be passed as var parameter:\n" + _expr.location.ToString());
            return (StringResources.Get("THIS_EXPRESSION_CAN_NOT_BE_PASSED_BY_ADDRESS"));
        }

        public override ILocation Location
        {
            get
            {
                return _expr.location;
            }
        }
    }
	/*
    public class ThisExpressionCanNotBePassedAsConstParameter : CompilationErrorWithLocation
    {
    	public ThisExpressionCanNotBePassedAsConstParameter(location loc):base(loc)
    	{
    		
    	}
    	
		public override string ToString()
		{
			return StringResources.Get("THIS_EXPRESSION_CAN_NOT_BE_PASSED_BY_CONST");
		}    	
    }
    */
    public class ParserError : CompilationError
    {
        private PascalABCCompiler.Errors.Error _error;
        private location _loc;

        public ParserError(PascalABCCompiler.Errors.Error error)
        {
            _error = error;
        }

        public ParserError(PascalABCCompiler.Errors.Error error, location loc)
        {
            _error = error;
            _loc = loc;
        }

        public PascalABCCompiler.Errors.Error error
        {
            get
            {
                return _error;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //string error_message="Parser error:     "+_error.ToString();
            //if (_loc!=null)
            //{
            //	error_message+="\n"+_loc.ToString();
            //}
            //return error_message;
            return _error.Message;
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class MemberIsNotDeclaredInType : CompilationError
    {
        private SyntaxTree.ident _id;
        private location _loc;
        private type_node _tn;

        public MemberIsNotDeclaredInType(SyntaxTree.ident id, location loc, type_node tn)
        {
            _id = id;
            _loc = loc;
            _tn = tn;
        }

        public SyntaxTree.ident ident
        {
            get
            {
                return _id;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public type_node type
        {
            get
            {
                return _tn;
            }
        }

        public override string ToString()
        {
            if (_id.Parent != null && _id.Parent is SyntaxTree.dot_node dnn && dnn.left is SyntaxTree.dot_node dn && 
                dn.right is SyntaxTree.ident iid && iid.name.StartsWith("<>") && iid.name.EndsWith("__self"))
                    return StringResources.Get("MEMBER_{0}_OF_TYPE_{1}_CANNOT_BE_FOUND_IN_THE_CONTEEXT_OF_FUNCTION_WITH_YIELD", _id.name, _tn.PrintableName);
            //return ("Member "+_id.PrintableName+" is not declared in type "+_tn.PrintableName+"\n"+loc_to_string(_loc));
            return (StringResources.Get("MEMBER_{0}_IS_NOT_DECLARED_IN_TYPE_{1}", _id.name, _tn.PrintableName));
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class MemberIsNotDeclaredInNamespace : CompilationError
    {
        private SyntaxTree.ident _id;
        private location _loc;
        private namespace_node _nn;

        public MemberIsNotDeclaredInNamespace(SyntaxTree.ident id, location loc, namespace_node nn)
        {
            _id = id;
            _loc = loc;
            _nn = nn;
        }

        public SyntaxTree.ident ident
        {
            get
            {
                return _id;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public namespace_node namespace_node
        {
            get
            {
                return _nn;
            }
        }

        public override string ToString()
        {
            //return ("Member "+_id.PrintableName+" is not declared in namespace "+_nn.PrintableNamespace_full_name+"\n"+loc_to_string(_loc));
            return StringResources.Get("MEMBER_{0}_IS_NOT_DECLARED_IN_NAMESPACE_{1}", _id.name, _nn.namespace_full_name);
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class CanNotCallStaticMethodWithExpression : CompilationError
    {
        private expression_node _en;
        private function_node _method;

        public CanNotCallStaticMethodWithExpression(expression_node en, function_node method)
        {
            _en = en;
            _method = method;
        }

        public expression_node expr
        {
            get
            {
                return _en;
            }
        }

        public function_node method
        {
            get
            {
                return _method;
            }
        }

        public override string ToString()
        {
            //return ("Can not call static method " + _method.PrintableName + " with expression\n" + _en.location.ToString());
            return (StringResources.Get("CAN_NOT_CALL_STATIC_MEMBER_{0}_WITH_EXPRESSION", _method.name));
        }

        public override ILocation Location
        {
            get
            {
                return _en.location;
            }
        }
    }

    public class CanNotCallNonStaticMethodWithClass : CompilationError
    {
        private type_node _tn;
        private location _loc;
        private function_node _method;

        public CanNotCallNonStaticMethodWithClass(type_node tn, location loc, function_node method)
        {
            _tn = tn;
            _loc = loc;
            _method = method;
        }

        public type_node type
        {
            get
            {
                return _tn;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public function_node method
        {
            get
            {
                return _method;
            }
        }

        public override string ToString()
        {
            //return ("Can not call not static method "+_method.PrintableName+" with class "+_tn.PrintableName);
            return (StringResources.Get("CAN_NOT_CALL_NON_STATIC_MEMBER_{0}_WITH_TYPE_{1}", _method.name,_tn.PrintableName));
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class CanNotReferanceToStaticFieldWithExpression : CompilationError
    {
        private var_definition_node _field;
        private location _loc;
        private expression_node _en;

        public CanNotReferanceToStaticFieldWithExpression(var_definition_node field, location loc, expression_node en)
        {
            _field = field;
            _loc = loc;
            _en = en;
        }

        public var_definition_node field
        {
            get
            {
                return _field;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public expression_node expr
        {
            get
            {
                return _en;
            }
        }

        public override string ToString()
        {
            //return ("Can not reference to static field "+_field.PrintableName+" with expression.\n"+_loc.ToString());
            return (StringResources.Get("CAN_NOT_REFERENCE_TO_STATIC_FIELD_{0}_WITH_EXPRESSION", _field.name));
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }
    
    public class CanNotReferenceToNonStaticMethodWithType : CompilationErrorWithLocation
    {
        string message = "CAN_NOT_REFERENCE_TO_NONSTATIC_METHOD_{0}_WITH_TYPE";
        string name;
        public CanNotReferenceToNonStaticMethodWithType(string meth_name, location loc)
            : base(loc)
        {
            if (meth_name.Contains(StringConstants.event_add_method_prefix) ||
                meth_name.Contains(StringConstants.event_remove_method_prefix))
            {
                meth_name = meth_name.Replace(StringConstants.event_add_method_prefix, "");
                meth_name = meth_name.Replace(StringConstants.event_remove_method_prefix, "");
                message = "CAN_NOT_REFERENCE_TO_NONSTATIC_EVENT_{0}_WITH_TYPE";
            }
            name=meth_name;
        }
        public override string ToString()
        {
            return string.Format(StringResources.Get(message,name));
        }
}

    public class CanNotReferenceToNonStaticFromStatic : CompilationErrorWithLocation
    {
        string message = "CAN_NOT_REFERENCE_TO_NONSTATIC_METHOD_{0}_FROM_STATIC_METHOD";
        string name;
        public CanNotReferenceToNonStaticFromStatic(definition_node dn, location loc)
            : base(loc)
        {
            string meth_name;
            switch (dn.general_node_type)
            {
                case general_node_type.function_node:
                    meth_name = (dn as function_node).name; 
                    if (meth_name.Contains(StringConstants.event_add_method_prefix) ||
                        meth_name.Contains(StringConstants.event_remove_method_prefix))
                    {
                        meth_name = meth_name.Replace(StringConstants.event_add_method_prefix, "");
                        meth_name = meth_name.Replace(StringConstants.event_remove_method_prefix, "");
                        message = "CAN_NOT_REFERENCE_TO_NONSTATIC_EVENT_{0}_FROM_STATIC_METHOD";
                    }
                    break;
                case general_node_type.variable_node:
                    meth_name = (dn as var_definition_node).name;
                    message = "CAN_NOT_REFERENCE_TO_NONSTATIC_FIELD_{0}_FROM_STATIC_METHOD";
                    break;
                default:
                    message = "CAN_NOT_REFERENCE_TO_NONSTATIC_{0}_FROM_STATIC";
                    meth_name = "";
                    break;
        }
            name = meth_name;
        }
        public override string ToString()
        {
            return string.Format(StringResources.Get(message, name));
        }
    }

    public class CanNotReferenceToNonStaticFromStaticInitializer : CompilationErrorWithLocation
    {
        string message = "CAN_NOT_REFERENCE_TO_NONSTATIC_METHOD_{0}_FROM_STATIC_INITIALIZER";
        string name;
        public CanNotReferenceToNonStaticFromStaticInitializer(definition_node dn, location loc)
            : base(loc)
        {
            string meth_name;
            switch (dn.general_node_type)
            {
                case general_node_type.function_node:
                    meth_name = (dn as function_node).name;
                    if (meth_name.Contains(StringConstants.event_add_method_prefix) ||
                        meth_name.Contains(StringConstants.event_remove_method_prefix))
                    {
                        meth_name = meth_name.Replace(StringConstants.event_add_method_prefix, "");
                        meth_name = meth_name.Replace(StringConstants.event_remove_method_prefix, "");
                        message = "CAN_NOT_REFERENCE_TO_NONSTATIC_EVENT_{0}_FROM_STATIC_INITIALIZER";
                    }
                    break;
                case general_node_type.variable_node:
                    meth_name = (dn as var_definition_node).name;
                    message = "CAN_NOT_REFERENCE_TO_NONSTATIC_FIELD_{0}_FROM_STATIC_INITIALIZER";
                    break;
                default:
                    message = "CAN_NOT_REFERENCE_TO_NONSTATIC_{0}_FROM_INITIALIZER";
                    meth_name = "";
                    break;
            }
            name = meth_name;
        }
        public override string ToString()
        {
            return string.Format(StringResources.Get(message, name));
        }
    }
    /*->*/
    public class CanNotReferenceToNonStaticFieldWithType : CompilationError
    {
        private var_definition_node _field;
        private location _loc;
        private type_node _tn;

        public CanNotReferenceToNonStaticFieldWithType(var_definition_node field, location loc, type_node tn)
        {
            _field = field;
            _loc = loc;
            _tn = tn;
        }

        public var_definition_node field
        {
            get
            {
                return _field;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public type_node type
        {
            get
            {
                return _tn;
            }
        }

        public override string ToString()
        {
            //return ("Can not reference to non static field "+_field.PrintableName+" with type\n"+_loc.ToString());
            return (StringResources.Get("CAN_NOT_REFERENCE_TO_NONSTATIC_FIELD_{0}_WITH_TYPE", _field.name));
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class FunctionExpectedProcedureMeet : CompilationError
    {
        private function_node _fn;
        private location _loc;

        public FunctionExpectedProcedureMeet(function_node fn, location loc)
        {
            _fn = fn;
            _loc = loc;
        }

        public function_node function
        {
            get
            {
                return _fn;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return ("Function expected procedure meet. Name: "+_fn.PrintableName+"\n"+loc_to_string(_loc));
            return (StringResources.Get("FUNCTION_EXPECTED_PROCEDURE_{0}_MEET", _fn.name));
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class OnlyProcedureNameAllowedInClassFunctionDefinition : CompilationError
    {
        private string _type_name;
        private location _loc;

        public OnlyProcedureNameAllowedInClassFunctionDefinition(string type_name, location loc)
        {
            _type_name = type_name;
            _loc = loc;
        }

        public string type_name
        {
            get
            {
                return _type_name;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return ("Only procedure name allowed in class function definition\n"+loc_to_string(_loc));
            return StringResources.Get("ONLY_PROCEDURE_NAME_ALLOWED_IN_CLASS_FUNCTION_DEFINITION");
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }

    }

    public class BreakStatementWithoutComprehensiveCycle : CompilationError
    {
        private ILocation _loc;

        public BreakStatementWithoutComprehensiveCycle(ILocation loc)
        {
            _loc = loc;
        }

        public ILocation loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return ("Break cycle without comprehensive cycle\n"+loc_to_string(_loc));
            return StringResources.Get("BREAK_STATEMENT_WITHOUT_COMPREHENSIVE_CYCLE");
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class ContinueStatementWithoutComprehensiveCycle : CompilationError
    {
        private ILocation _loc;

        public ContinueStatementWithoutComprehensiveCycle(ILocation loc)
        {
            _loc = loc;
        }

        public ILocation loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return ("Continue cycle without comprehensive cycle\n"+loc_to_string(_loc));
            return StringResources.Get("CONTINUE_STATEMENT_WITHOUT_COMPREHENSIVE_CYCLE");
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }


    public class ForLoopControlMustBeSimpleLocalVariable : CompilationError
    {
        private ILocation _loc;

        public ForLoopControlMustBeSimpleLocalVariable(ILocation loc)
        {
            _loc = loc;
        }

        public ILocation loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return ("For loop control must be simple local variable\n"+loc_to_string(_loc));
            return StringResources.Get("FOR_LOOP_CONTROL_MUST_BE_SIMPLE_LOCAL_VARIABLE");
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class OrdinalTypeExpected : CompilationError
    {
        private ILocation _loc;

        public OrdinalTypeExpected(ILocation loc)
        {
            _loc = loc;
        }

        public ILocation loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return ("Ordinal type expected\n"+loc_to_string(_loc));
            return StringResources.Get("ORDINAL_TYPE_EXPECTED");
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class OrdinalOrStringTypeExpected : CompilationError
    {
        private ILocation _loc;

        public OrdinalOrStringTypeExpected(ILocation loc)
        {
            _loc = loc;
        }

        public ILocation loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return ("Ordinal type expected\n"+loc_to_string(_loc));
            return StringResources.Get("ORDINAL_OR_STRING_TYPE_EXPECTED");
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class CanNotReferenceToNonStaticPropertyWithType : CompilationError
    {
        private string _name;
        private location _loc;
        private type_node _tn;
        private property_node _prop;

        public CanNotReferenceToNonStaticPropertyWithType(property_node prop, location loc, type_node tn)
        {
            _name = prop.name;
            _loc = loc;
            _tn = tn;
            _prop = prop;
        }

        public property_node property
        {
            get
            {
                return _prop;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public type_node type
        {
            get
            {
                return _tn;
            }
        }

        public override string ToString()
        {
            //return ("Can not reference to non static property "+_prop.PrintableName+" with type\n"+_loc.ToString());
            return StringResources.Get("CAN_NOT_REFERENCE_TO_NONSTATIC_PROPERTY_{0}_WITH_TYPE", _name);
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class CanNotReferenceToStaticPropertyWithExpression : CompilationError
    {
        private property_node _prop;
        private location _loc;
        private type_node _tn;

        public CanNotReferenceToStaticPropertyWithExpression(property_node prop, location loc, type_node tn)
        {
            _prop = prop;
            _loc = loc;
            _tn = tn;
        }

        public property_node property
        {
            get
            {
                return _prop;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public type_node type
        {
            get
            {
                return _tn;
            }
        }

        public override string ToString()
        {
            //return ("Can not reference to static property with expression "+_prop.PrintableName+" with type\n"+_loc.ToString());
            return StringResources.Get("CAN_NOT_REFERENCE_TO_STATIC_PROPERTY_{0}_WITH_EXPRESSION", _prop.name);
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class ThisPropertyCanNotBeReaded : CompilationError
    {
        private property_node _prop;
        private location _loc;

        public ThisPropertyCanNotBeReaded(property_node pn, location loc)
        {
            _prop = pn;
            _loc = loc;
        }

        public property_node property
        {
            get
            {
                return _prop;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return (_prop.PrintableName+"  property can not be readed\n"+_loc.ToString());
            return StringResources.Get("THIS_PROPERTY_{0}_CAN_NOT_BE_READED", _prop.name);
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class ThisPropertyCanNotBeWrited : CompilationError
    {
        private property_node _prop;
        private location _loc;

        public ThisPropertyCanNotBeWrited(property_node pn, location loc)
        {
            _prop = pn;
            _loc = loc;
        }

        public property_node property
        {
            get
            {
                return _prop;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return (_prop.PrintableName+" property can not be writed\n"+_loc.ToString());
            return StringResources.Get("THIS_PROPERTY_{0}_CAN_NOT_BE_WRITED", _prop.name);
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class PropertyAndReadAccessorParamsCountConvergence : CompilationError
    {
        private function_node _accessor;
        private common_property_node _prop;
        private location _loc;

        public PropertyAndReadAccessorParamsCountConvergence(function_node accessor, common_property_node prop, location loc)
        {
            _accessor = accessor;
            _prop = prop;
            _loc = loc;
        }

        public function_node accessor
        {
            get
            {
                return _accessor;
            }
        }

        public common_property_node prop
        {
            get
            {
                return _prop;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return ("Property and read accessor params count convergence\n"+_loc.ToString());
            return StringResources.Get("PROPERTY_{0}_AND_READ_ACCESSOR_{1}_PARAMETERS_COUNT_CONVERGENCE", _prop.name, _accessor.name);
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class PropertyAndWriteAccessorParamsCountConvergence : CompilationError
    {
        private function_node _accessor;
        private common_property_node _prop;
        private location _loc;

        public PropertyAndWriteAccessorParamsCountConvergence(function_node accessor, common_property_node prop, location loc)
        {
            _accessor = accessor;
            _prop = prop;
            _loc = loc;
        }

        public function_node accessor
        {
            get
            {
                return _accessor;
            }
        }

        public common_property_node prop
        {
            get
            {
                return _prop;
            }
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return ("Property and write accessor params count convergence\n"+_loc.ToString());
            return StringResources.Get("PROPERTY_{0}_AND_WRITE_ACCESSOR_{1}_PARAMETERS_COUNT_CONVERGENCE", _prop.name, _accessor.name);
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }
    /*<-*/
    // <=

    public class OverloadOperatorCanNotBeProcedure : CompilationError
    {
        private location _loc;

        public OverloadOperatorCanNotBeProcedure(location loc)
        {
            _loc = loc;
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            //return ("Overload operator can not be procedure in: "+_loc.ToString());
            return StringResources.Get("OVERLOAD_OPERATOR_CAN_NOT_BE_PROCEDURE");
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }

        public override bool MustThrow
        {
            get
            {
                return false;
            }
        }
    }
    public class NotSupportedError : Errors.SemanticNonSupportedError
    {
        private location _loc;
        string message = "NOT_SUPPORTED_BY_THIS_VERSION_OF_COMPILER";

        public NotSupportedError(location loc)
            :base(loc.document.file_name)
        {
            _loc = loc;
        }
        public NotSupportedError(location loc, string msg)
            : base(loc.document.file_name)
        {
            _loc = loc;
            message = msg;
        }
        
        public NotSupportedError()
            : base(null)
        {
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            return StringResources.Get(message);
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }
    }

    public class NamespaceNotFound : CompilationError
    {
        private location _loc;
        public string Namespace;

        public NamespaceNotFound(string Namespace, location loc)
        {
            _loc = loc;
            this.Namespace = Namespace;
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }

        public override ILocation Location
        {
            get
            {
                return _loc;
            }
        }

        public override string ToString()
        {
            return string.Format(StringResources.Get("NAMESPACE_{0}_NOTFOUND"),Namespace);
        }
    }
	
	// #ErrorRefactoringProblem
	// Попытка убрать этот класс приводит к зацикливанию приложения bin\TestRunner.exe
    public class CircuralityInPointer : CompilationErrorWithLocation
    {
    	public CircuralityInPointer(location loc):base(loc)
    	{
    		
    	}
    	public override string ToString()
        {
            return string.Format(StringResources.Get("CIRCULARITY_IN_POINTER"));
        }
    }
	
    public class ExpectedExprHaveTypeTypedFile : CompilationErrorWithLocation
    {
        private type_node _file_type;
        private type_node _element_type;
        private bool _expected_variable;
        public ExpectedExprHaveTypeTypedFile(type_node file_type, type_node element_type, bool expected_variable, location loc)
            : base(loc)
        {
            _element_type = element_type;
            _file_type = file_type;
            _expected_variable = expected_variable;
        }
        public override string ToString()
        {
            string msg;
            if (_expected_variable)
                msg = "EXPECTED_VARIABLE_HAVE_TYPE_{0}_TYPED_FILE";
            else
                msg = "EXPECTED_EXPRESSION_HAVE_TYPE_{0}_TYPED_FILE";
            return string.Format(string.Format(StringResources.Get(msg), _file_type.PrintableName));
        }
    }
    
    public class ConstMemberCannotBeAccessedWithAnInstanceReference : CompilationErrorWithLocation
    {
        class_constant_definition cdn;
        public ConstMemberCannotBeAccessedWithAnInstanceReference(class_constant_definition cdn, location loc)
            : base(loc)
        {
            this.cdn = cdn;
        }
        public override string ToString()
        {
            return string.Format(string.Format(StringResources.Get("CONST_MEMBER_{0}_CANNOT_BE_ACCESSED_WITH_AN_INSTANCE_REFERNCE"), cdn.comperehensive_type.PrintableName + "." + cdn.name));
        }
    }

    public class ExpectedType : CompilationErrorWithLocation
    {
        private string _name;
        public ExpectedType(location loc):base(loc)
        {
        	
        }
        public ExpectedType(string name, location loc)
            : base(loc)
        {
            _name = name;
        }
        public override string ToString()
        {
        	if (_name != null)
        	return string.Format(StringResources.Get("EXPECTED_TYPE_{0}"), _name);
        	return string.Format(StringResources.Get("EXPECTED_TYPE"));
        }
    }
    


    public class InterfaceFunctionWithBody : CompilationErrorWithLocation
    {
        public InterfaceFunctionWithBody(location loc)
            : base(loc)
        {
        }
        public override string ToString()
        {
            return string.Format(StringResources.Get("INTERFACE_FUNCTION_WITH_BODY"));
        }
        public override bool MustThrow
        {
            get
            {
                return false;
            }
        }
    }

    public class ConstructorInInterface : CompilationErrorWithLocation
    {
        public ConstructorInInterface(location loc)
            : base(loc)
        {
        }
        public override string ToString()
        {
            return string.Format(StringResources.Get("CONSTRUCTOR_IN_INTERFACE"));
        }
        public override bool MustThrow
        {
            get
            {
                return false;
            }
        }
    }

    public class InterfaceMemberNotImplemented : CompilationErrorWithLocation
    {
        private string _class_name;
        private string _interface_name;
        private string _member_name;
        private bool _is_value;
        
        public InterfaceMemberNotImplemented(string class_name, string interface_name, string member_name, bool is_value, location loc)
            : base(loc)
        {
            _class_name = class_name;
            _interface_name = interface_name;
            _member_name = member_name;
            _is_value = is_value;
        }
        public override string ToString()
        {
            if(_is_value)
                return string.Format(StringResources.Get("RECORD_{0}_DOES_NOT_IMPLEMENT_MEMBER_{2}_OF_INTERFACE_{1}"), _class_name, _interface_name, _member_name);
            else
                return string.Format(StringResources.Get("CLASS_{0}_DOES_NOT_IMPLEMENT_MEMBER_{2}_OF_INTERFACE_{1}"), _class_name, _interface_name, _member_name);
        }
    }
	
    public class AbstractMemberNotImplemented : CompilationErrorWithLocation
    {
    	private string _class_name;
        private string _abstr_class_name;
        private string _member_name;
        private bool _is_value;
        
        public AbstractMemberNotImplemented(string class_name, string abstr_class_name, string member_name, bool is_value, location loc)
            : base(loc)
        {
            _class_name = class_name;
            _abstr_class_name = abstr_class_name;
            _member_name = member_name;
            _is_value = is_value;
        }
        public override string ToString()
        {
            if(_is_value)
                return string.Format(StringResources.Get("RECORD_{0}_DOES_NOT_IMPLEMENT_MEMBER_{2}_OF_CLASS_{1}"), _class_name, _abstr_class_name, _member_name);
            else
                return string.Format(StringResources.Get("CLASS_{0}_DOES_NOT_IMPLEMENT_MEMBER_{2}_OF_CLASS_{1}"), _class_name, _abstr_class_name, _member_name);
        }
    }
    
    public class DerivedFromInterfaceMethodMustBePublicAndNonStatic : CompilationErrorWithLocation
    {
        private string _class_name;
        private string _interface_name;
        private string _member_name;
        private bool _is_value;
        public DerivedFromInterfaceMethodMustBePublicAndNonStatic(string class_name, string interface_name, string member_name, bool is_value, location loc)
            : base(loc)
        {
            _class_name = class_name;
            _interface_name = interface_name;
            _member_name = member_name;
            _is_value = is_value;
        }
        public override string ToString()
        {
            if (_member_name.StartsWith("get_"))
                return string.Format(StringResources.Get("PROPERTY_{2}_OF_CLASS_{0}_FROM_INTERFACE_{1}_MUST_BE_PUBLIC_AND_NON_STATIC"), _class_name, _interface_name, _member_name.Replace("get_",""));
            if (_is_value)
            {
                return string.Format(StringResources.Get("MEMBER_{2}_OF_RECORD_{0}_FROM_INTERFACE_{1}_MUST_BE_PUBLIC_AND_NON_STATIC"), _class_name, _interface_name, _member_name);
            }
            else
            {
                
                return string.Format(StringResources.Get("MEMBER_{2}_OF_CLASS_{0}_FROM_INTERFACE_{1}_MUST_BE_PUBLIC_AND_NON_STATIC"), _class_name, _interface_name, _member_name);
            }
        }
    }

    public class VirtualMethodInRecord : CompilationErrorWithLocation
    {
        public VirtualMethodInRecord(location loc)
            : base(loc)
        {
        }
        public override string ToString()
        {
            return string.Format(StringResources.Get("VIRTUAL_METHOD_IN_RECORD"));
        }

        public override bool MustThrow
        {
            get
            {
                return false;
            }
        }
    }

    public class NoMethodInClassWithThisParams : CompilationErrorWithLocation
    {
        private common_method_node _cmn;
        private common_type_node _ctn;
        public NoMethodInClassWithThisParams(common_method_node cmn, common_type_node ctn, location loc)
            : base(loc)
        {
            _cmn = cmn;
            _ctn = ctn;
        }
        public override string ToString()
        {
            return string.Format(StringResources.Get("NO_METHOD_{0}_IN_CLASS_{1}_WITH_THIS_PARAMS"), _cmn.name, _ctn.PrintableName);
        }
    }


    public class CanNotAssignToReadOnlyElement : CompilationErrorWithLocation
    {
    	private general_node_type _node_type;
    	
    	public  CanNotAssignToReadOnlyElement(location loc, general_node_type node_type):base(loc)
    	{
    		this._node_type = node_type;
    	}
    	
		public override string ToString()
		{
			if (_node_type == general_node_type.variable_node)
				return StringResources.Get("CANNOT_ASSIGN_TO_READONLY_FIELD");
			if (_node_type == general_node_type.property_node)
				return StringResources.Get("CANNOT_ASSIGN_TO_READONLY_PROPERTY");
			return StringResources.Get("CAN_NOT_ASSIGN_TO_CONSTANT_OBJECT");
		}
    }

    public class AbstractMethodWithBody : CompilationErrorWithLocation
    {
    	public AbstractMethodWithBody(location loc) : base(loc)
    	{
    		
    	}
    	
		public override string ToString()
		{
			return string.Format(StringResources.Get("ABSTRACT_METHOD_WITH_BODY"));
		}

        public override bool MustThrow
        {
            get
            {
                return false;
            }
        }
    }

	//lroman//

    public class NestedLambdasAreNotSupportedInThisVersionOfCompiler : CompilationErrorWithLocation
    {
        public NestedLambdasAreNotSupportedInThisVersionOfCompiler(location loc)
            : base(loc)
        {

        }

        public override string ToString()
        {
            return StringResources.Get("NESTED_LAMBDAS_ARE_NOT_SUPPORTED_IN_THIS_VERSION_OF_COMPILER");
        }
    }

    public class LambdasAreNotAllowedInNestedSubprogram : CompilationErrorWithLocation
    {
        public LambdasAreNotAllowedInNestedSubprogram(location loc)
            : base(loc)
        {

        }

        public override string ToString()
        {
            return StringResources.Get("LAMBDAS_ARE_NOT_ALLOWED_IN_NESTED_SUBPROGRAM");
        }
    }

    public class ThisTypeOfVariablesCannotBeCaptured : CompilationErrorWithLocation
    {
        public ThisTypeOfVariablesCannotBeCaptured(location loc)
            : base(loc)
        {
            
        }

        public override string ToString()
        {
            return StringResources.Get("THIS_TYPE_OF_VARIABLES_CANNOT_BE_CAPTURED");
        }
    }

    public class CannotCaptureNonValueParameters : CompilationErrorWithLocation
    {
        public CannotCaptureNonValueParameters(location loc)
            : base(loc)
        {

        }

        public override string ToString()
        {
            return StringResources.Get("CANNOT_CAPTURE_NON_VALUE_PARAMETERS");
        }
    }

    public class LambdasNotAllowedInForeachInWhatSatetement : CompilationErrorWithLocation
    {
        public LambdasNotAllowedInForeachInWhatSatetement(location loc)
            : base(loc)
        {

        }

        public override string ToString()
        {
            return StringResources.Get("LAMBDAS_NOT_ALLOWED_IN_FOREACH_IN_WHAT_STATEMENT");
        }
    }

    public class LambdasNotAllowedWhenNestedSubprogrammesAreUsed : CompilationErrorWithLocation
    {
        public LambdasNotAllowedWhenNestedSubprogrammesAreUsed(location loc)
            : base(loc)
        {

        }

        public override string ToString()
        {
            return StringResources.Get("LAMBDAS_NOT_ALLOWED_WHEN_NESTED_SUBPROGRAMMES_ARE_USED");
        }
    }

    public class LambdasNotAllowedInDeclarationsSection : CompilationErrorWithLocation
    {
        public LambdasNotAllowedInDeclarationsSection(location loc)
            : base(loc)
        {

        }

        public override string ToString()
        {
            return StringResources.Get("LAMBDAS_NOT_ALLOWED_IN_DECLARATIONS_SECTION");
        }
    }

    public class LambdasNotAllowedInInterfacePartOfModuleInInit : CompilationErrorWithLocation
    {
        public LambdasNotAllowedInInterfacePartOfModuleInInit(location loc)
            : base(loc)
        {

        }

        public override string ToString()
        {
            return StringResources.Get("LAMBDAS_NOT_ALLOWED_IN_INTERFACE_PART_OF_MODULE_IN_INIT");
        }
    }

    public class LambdasNotAllowedInImplementationOfModuleInInit : CompilationErrorWithLocation
    {
        public LambdasNotAllowedInImplementationOfModuleInInit(location loc)
            : base(loc)
        {

        }

        public override string ToString()
        {
            return StringResources.Get("LAMBDAS_NOT_ALLOWED_IN_IMPLEMENTATION_PART_OF_MODULE_IN_INIT");
        }
    }

    public class LambdasNotAllowedInInitializationPartOfModule : CompilationErrorWithLocation
    {
        public LambdasNotAllowedInInitializationPartOfModule(location loc)
            : base(loc)
        {

        }

        public override string ToString()
        {
            return StringResources.Get("LAMBDAS_NOT_ALLOWED_IN_INITIALIZATION_PART_OF_MODULE");
        }
    }

    public class LambdasNotAllowedInFinalizationPartOfModule : CompilationErrorWithLocation
    {
        public LambdasNotAllowedInFinalizationPartOfModule(location loc)
            : base(loc)
        {

        }

        public override string ToString()
        {
            return StringResources.Get("LAMBDAS_NOT_ALLOWED_IN_FINALIZATION_PART_OF_MODULE");
        }
    }

    public class LambdasNotAllowedInFieldsInitialization : CompilationErrorWithLocation
    {
        public LambdasNotAllowedInFieldsInitialization(location loc)
            : base(loc)
        {

        }

        public override string ToString()
        {
            return StringResources.Get("LAMBDAS_NOT_ALLOWED_IN_FIELDS_INITIALIZATION");
        }
    }

    public class UsingCapturedParameterIsNotAllowedInInitializers : CompilationErrorWithLocation
    {
        public UsingCapturedParameterIsNotAllowedInInitializers(location loc)
            : base(loc)
        {

        }

        public override string ToString()
        {
            return StringResources.Get("USING_CAPTURED_PARAMETERS_IS_NOT_ALLOWED_IN_INITIALIZERS");
        }
    }


    public class FunctionPredefinitionWithoutDefinition : CompilationErrorWithLocation
    {
        private common_function_node cfn;

        public FunctionPredefinitionWithoutDefinition(common_function_node cfn, location loc): base(loc)
        {
            this.cfn = cfn;
        }

        public override string ToString()
        {
            if (cfn is common_method_node && (cfn as common_method_node).is_constructor)
                return StringResources.Get("CONSTRUCTOR_PREDEFINITION_WITHOUT_DEFINITION");
            if (cfn.return_value_type == null)
                return StringResources.Get("PROCEDURE_PREDEFINITION_WITHOUT_DEFINITION");
            return StringResources.Get("FUNCTION_PREDEFINITION_WITHOUT_DEFINITION");
        }
    }


    public class FailedWhileTryingToCompileLambdaBodyWithGivenParametersException : Exception
    {
        public Exception ExceptionOnCompileBody { get; private set; }

        public FailedWhileTryingToCompileLambdaBodyWithGivenParametersException(Exception exceptionOnCompileBody)
        {
            ExceptionOnCompileBody = exceptionOnCompileBody;
        }
    }
    //\lroman//
}