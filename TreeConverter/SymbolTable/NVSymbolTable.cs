
using System;
using System.Collections;

namespace TreeConverter
{
	public abstract class scope : base_scope
	{
		private Hashtable ht=new Hashtable();

		public string name_to_symtab(string name)
		{
			return name.ToLower();
		}

		public void add_name(string name,SymbolInfo si)
		{
			name=name_to_symtab(name);
			object o=ht[name];
			if (o==null)
			{
				SymbolInfoArrayList syal=new SymbolInfoArrayList();
				syal.Add(si);
				ht[name]=syal;
				return;
			}
			else
			{
				/*if (si.symbol_kind==symbol_kind.sk_none)
				{
					throw new CompilerInternalError("Duplicate name definition");
				}*/
				SymbolInfoArrayList syal1=(SymbolInfoArrayList)o;
				syal1.Add(si);
				return;
			}
		}

		public virtual SymbolInfo find_only_in_namespace(string name)
		{
			name=name_to_symtab(name);
			object o=ht[name];
			if (o==null)
			{
				return null;
			}
			SymbolInfoArrayList syal=(SymbolInfoArrayList)o;
			int i=1;
			while(i<syal.Count)
			{
				syal[i-1].Next=syal[i];
				i++;
			}
			syal[i-1].Next=null;
			return syal[0];
		}

		public static SymbolInfo split_lists(SymbolInfo si_left,SymbolInfo si_right)
		{
			if (si_left==null)
			{
				return si_right;
			}
			if (si_right==null)
			{
				return si_left;
			}
			SymbolInfo si=si_left;
			while(si.Next!=null)
			{
				si=si.Next;
			}
			si.Next=si_right;
			return si_left;
		}

		//ВНИМАНИЕ. В этом методе, поиск в another_scopes производится в том порядке, в котором они передаются.
		//И в выходном списке эти узлы будут в соответствующем порядке.
		//Преобразователь дерева простматривая этот список, и встретив два абсолютно одинаковых метода
		//выкинет из списка, встреченный последним.
		//Т.е. если первое из another_namespace - пространство имен класса,
		//а второе - пространство имен, в котором объявлен метод, то найденные в классе методы будут
		//расположены в списке раньше, чем найденные в пространстве имен.
		//Таким образом если и в классе и в пространстве имен, окружающем метод, есть два перегруженных
		//метода с одинаковым списком параметров, то метод в пространстве имен, окружающем метод,
		//будет проигнорирован.
		public SymbolInfo find_and_split(string name,bool brake_if_find,params base_scope[] another_scopes)
		{
			SymbolInfo si_first=this.find_only_in_namespace(name);
			if (si_first!=null)
			{
				if ((brake_if_find)||(si_first.symbol_kind==symbol_kind.sk_none))
				{
					return si_first;
				}
			}
			name=name_to_symtab(name);
			foreach(scope sc in another_scopes)
			{
				if (sc==null)
				{
					continue;
				}
				SymbolInfo si_second=sc.find(name);
				if (si_second==null)
				{
					continue;
				}
				if (si_first==null)
				{
					if ((brake_if_find)||(si_second.symbol_kind==symbol_kind.sk_none))
					{
						return si_second;
					}
					si_first=si_second;
					continue;
				}
				if (si_second.symbol_kind==symbol_kind.sk_none)
				{
					break;
				}
				si_first=split_lists(si_first,si_second);	
			}
			return si_first;
		}

		public SymbolInfo find_in_all_and_split(string name,params base_scope[] another_scopes)
		{
			return find_and_split(name,false,another_scopes);
		}

	}

	public class function_scope : scope
	{
		private base_scope _up_scope;
		
		public function_scope(base_scope up_scope)
		{
			_up_scope=up_scope;
		}
	
		public override SymbolInfo find(string name)
		{
			return find_in_all_and_split(name,_up_scope);
		}

		public override base_scope top_scope
		{
			get
			{
				return _up_scope;
			}
		}

	}

	public class type_scope : scope
	{
		private base_scope _base_type_scope;
		private base_scope _comprehensive_scope;

		public type_scope(base_scope base_type_scope,base_scope comprehensive_scope)
		{
			_base_type_scope=base_type_scope;
			_comprehensive_scope=comprehensive_scope;
		}

		public override SymbolInfo find(string name)
		{
			return find_in_all_and_split(name,_base_type_scope,_comprehensive_scope);	
		}

		public SymbolInfo find_in_type(string name)
		{
			return find_in_all_and_split(name,_base_type_scope);
		}

		public base_scope comprehensive_scope
		{
			get
			{
				return _comprehensive_scope;
			}
			set
			{
				_comprehensive_scope=value;
			}
		}

		public base_scope base_type_scope
		{
			get
			{
				return _base_type_scope;
			}
			set
			{
				_base_type_scope=value;
			}
		}

		public override base_scope top_scope
		{
			get
			{
				return _base_type_scope;
			}
		}

	}

	public class namespace_scope : scope
	{
		private base_scope _up_scope;

		public namespace_scope(base_scope up_scope)
		{
			_up_scope=up_scope;
		}

		public override SymbolInfo find(string name)
		{
			return find_in_all_and_split(name,_up_scope);
		}

		public override base_scope top_scope
		{
			get
			{
				return _up_scope;
			}
		}

	}

	public class interface_scope : scope
	{
		private base_scope[] _used_units_scopes;

		public interface_scope(base_scope[] used_units_scopes)
		{
			_used_units_scopes=new base_scope[used_units_scopes.Length];
			for(int i=0;i<used_units_scopes.Length;i++)
			{
				_used_units_scopes[used_units_scopes.Length-i-1]=used_units_scopes[i];
			}
		}

		public override SymbolInfo find(string name)
		{
			return find_in_all_and_split(name,_used_units_scopes);
		}

		public override base_scope top_scope
		{
			get
			{
				return null;
			}
		}
		
	}

	public class implementation_scope : scope
	{
		private base_scope[] _used_units_scopes;
		private interface_scope _iscope;

		public implementation_scope(interface_scope iscope,base_scope[] used_units_scopes)
		{
			_iscope=iscope;
			_used_units_scopes=new base_scope[used_units_scopes.Length+1];
			_used_units_scopes[0]=iscope;
			for(int i=0;i<used_units_scopes.Length;i++)
			{
				_used_units_scopes[used_units_scopes.Length-i]=used_units_scopes[i];
			}
		}

		public override SymbolInfo find(string name)
		{
			return find_in_all_and_split(name,_used_units_scopes);
		}

		public override SymbolInfo find_only_in_namespace(string name)
		{
			if (_used_units_scopes[0]==null)
			{
				return base.find_only_in_namespace(name);
			}
			return split_lists(base.find_only_in_namespace(name),_iscope.find_only_in_namespace(name));
		}

		public override base_scope top_scope
		{
			get
			{
				return _used_units_scopes[0];
			}
		}

	}

	public class method_scope : scope
	{
		private base_scope _type_scope;
		private base_scope _comprehensive_namespace_scope;

		public method_scope(base_scope _type_scope,base_scope comprehensive_namespace_scope)
		{
			this._type_scope=_type_scope;
			_comprehensive_namespace_scope=comprehensive_namespace_scope;
		}

		public override SymbolInfo find(string name)
		{
			return find_and_split(name,true,_type_scope,_comprehensive_namespace_scope);
		}

		public override base_scope top_scope
		{
			get
			{
				return _type_scope;
			}
		}

	}

	/*public class SymbolTable
	{
		public function_scope create_function_scope(base_scope up_scope)
		{
			return new function_scope(up_scope);
		}

		public type_scope create_type_scope(base_scope base_type_scope,base_scope comprehensive_namespace_scope)
		{
			return new type_scope(base_type_scope,comprehensive_namespace_scope);
		}

		public namespace_scope create_namespace_scope(base_scope up_scope)
		{
			return new namespace_scope(up_scope);
		}

		public interface_scope create_interface_scope(base_scope[] used_units_scopes)
		{
			return new interface_scope(used_units_scopes);
		}

		public implementation_scope create_implementation_scope(interface_scope _interface_scope,base_scope[] used_units_scopes)
		{
			return new implementation_scope(_interface_scope,used_units_scopes);
		}

		public method_scope create_method_scope(base_scope base_type_scope,base_scope comprehensive_namespace_scope)
		{
			return new method_scope(base_type_scope,comprehensive_namespace_scope);
		}

		public int get_relative_scope_depth(base_scope upper,base_scope lower)
		{
			int depth=0;
			base_scope bs=lower;
			while ((bs!=null)&&(upper!=bs))
			{
				bs=bs.top_scope;
				depth++;
			}
			if (lower==null)
			{
				throw new CompilerInternalError("Can not execute static depth");
			}
			return depth;
		}
	}*/
}