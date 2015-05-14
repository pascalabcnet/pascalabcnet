
using System;

namespace TreeConverter
{
	public class BaseScope
	{
		public virtual SymbolInfo Find(string name)
		{
			return null;
		}
	}
}

namespace SymbolTable
{

	public class Scope:TreeConverter.BaseScope
	{
		protected TreeConverter.scope _sc;
		protected Scope _top_scope;

		public TreeConverter.scope internal_scope
		{
			get
			{
				return _sc;
			}
		}

		public Scope()
		{
		}

		public Scope TopScope
		{
			get
			{
				return _top_scope;
			}
		}

		public Scope(TreeConverter.base_scope us,Scope top_scope)
		{
			_sc=new TreeConverter.function_scope(us);
			_top_scope=top_scope;
		}

		public override TreeConverter.SymbolInfo Find(string name)
		{
			return _sc.find(name);
		}
		public TreeConverter.SymbolInfo FindOnlyInScope(string name)
		{
			return _sc.find_only_in_namespace(name);
		}
		public TreeConverter.SymbolInfo FindOnlyInType(string name)
		{
			TreeConverter.type_scope ts=_sc as TreeConverter.type_scope;
			if (ts!=null)
			{
				return ts.find_in_type(name);
			}
			return null;
		}
		public void AddSymbol(string Name,TreeConverter.SymbolInfo Inf)
		{
			_sc.add_name(Name,Inf);
		}
	}

	public class UnitInterfaceScope : Scope
	{
		public UnitInterfaceScope(TreeConverter.base_scope[] bsa)
		{
			_sc=new TreeConverter.interface_scope(bsa);
		}

	}

	public class UnitImplementationScope : Scope
	{
		public UnitImplementationScope(TreeConverter.interface_scope isc,TreeConverter.base_scope[] bsa)
		{
			_sc=new TreeConverter.implementation_scope(isc,bsa);
		}
	}

	public class ClassScope : Scope
	{
		TreeConverter.type_scope _ts;
		Scope _bcs;
		public ClassScope(TreeConverter.base_scope bts, TreeConverter.base_scope cs, Scope bcs)
		{
			_bcs=bcs;
			_ts=new TreeConverter.type_scope(bts,cs);
			_sc=_ts;
		}
		public Scope BaseClassScope
		{
			get 
			{
				return _bcs;
			}
			set
			{
				_bcs=value;
				_ts.base_type_scope=(TreeConverter.type_scope)_bcs.internal_scope;
			}
		}
	}

	public class ClassMethodScope : Scope
	{
		public ClassMethodScope(TreeConverter.base_scope ts,TreeConverter.base_scope cns)
		{
			_sc=new TreeConverter.method_scope(ts,cns);
		}
	}

	public class TreeConverterSymbolTable
	{
		public Scope CreateScope(Scope TopScope)
		{
			TreeConverter.base_scope bs2=null;
			if (TopScope!=null)
			{
				bs2=TopScope.internal_scope;
			}
			return new Scope(bs2,TopScope);
		}
		public ClassScope CreateClassScope(Scope TopScope,Scope BaseClass)
		{
			TreeConverter.base_scope bs1=null;
			if (BaseClass!=null)
			{
				bs1=BaseClass.internal_scope;
			}
			TreeConverter.base_scope bs2=null;
			if (TopScope!=null)
			{
				bs2=TopScope.internal_scope;
			}
			return new ClassScope(bs1,bs2,BaseClass);	
		}
		public UnitInterfaceScope CreateUnitInterfaceScope(Scope[] UsedUnits)
		{
			TreeConverter.base_scope[] arr=new TreeConverter.base_scope[UsedUnits.Length];
			for(int i=0;i<arr.Length;i++)
			{
				if (UsedUnits[i]!=null)
				{
					arr[i]=UsedUnits[i].internal_scope;
				}
				else
				{
					arr[i]=null;
				}
			}
			return new UnitInterfaceScope(arr);
		}
		public UnitImplementationScope CreateUnitImplementationScope(Scope InterfaceScope,Scope[] UsedUnits)
		{
			TreeConverter.base_scope[] arr=new TreeConverter.base_scope[UsedUnits.Length];
			for(int i=0;i<arr.Length;i++)
			{
				arr[i]=UsedUnits[i].internal_scope;
			}
			TreeConverter.interface_scope isc1=null;
			if (InterfaceScope!=null)
			{
				isc1=(TreeConverter.interface_scope)InterfaceScope.internal_scope;
			}
			return new UnitImplementationScope(isc1,arr);
		}
		public ClassMethodScope CreateClassMethodScope(Scope TopScope,Scope MyClass)
		{
			TreeConverter.base_scope bs2=null;
			if (TopScope!=null)
			{
				bs2=TopScope.internal_scope;
			}
			TreeConverter.base_scope bs1=null;
			if (MyClass!=null)
			{
				bs1=MyClass.internal_scope;
			}
			return new ClassMethodScope(bs1,bs2);
		}

		public int GetRelativeScopeDepth(Scope Down,Scope Up)
		{
			int depth=0;
			/*TreeConverter.base_scope bs=Down.internal_scope;
			while ((bs!=null)&&(Up.internal_scope!=bs))
			{
				bs=bs.top_scope;
				depth++;
			}*/
			while ((Up!=null)&&(Down!=Up))
			{
				Up=Up.TopScope;
				depth++;
			}
			if (Up==null)
			{
				throw new TreeConverter.CompilerInternalError("Can not execute static depth");
			}
			return depth;
		}
	}
}