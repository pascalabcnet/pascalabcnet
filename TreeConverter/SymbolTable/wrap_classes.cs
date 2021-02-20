
namespace TreeConverter
{
	public class NetIntScope : scope
	{
		private NetScope _ns;

		public NetIntScope(NetScope ns)
		{
			_ns=ns;
		}

		public override SymbolInfo find(string name)
		{
			return _ns.Find(name);
		}

		public override SymbolInfo find_only_in_namespace(string name)
		{
			return _ns.Find(name);
		}

		public override base_scope top_scope
		{
			get
			{
				return null;
			}
		}

	}

	public class NetIntTypeScope : type_scope
	{
		private NetTypeScope _nts;

		public NetIntTypeScope(NetTypeScope nts) : base(null,null)
			//base(((type_scope)nts.internal_scope).base_type_scope,((type_scope)nts.internal_scope).comprehensive_scope)
		{
			_nts=nts;
		}

		public override SymbolInfo find(string name)
		{
			return _nts.Find(name);
		}

		public override SymbolInfo find_only_in_namespace(string name)
		{
			return _nts.Find(name);
		}

		public override base_scope top_scope
		{
			get
			{
				return null;
			}
		}


	}

}