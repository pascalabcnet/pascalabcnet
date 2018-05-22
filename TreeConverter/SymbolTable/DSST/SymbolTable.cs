// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Linq;
using PascalABCCompiler.TreeConverter;
using System.Collections.Generic;
using SymbolTable;
using System.Reflection;

namespace PascalABCCompiler.TreeRealization
{
    public abstract class BasePCUReader
    {
        public abstract definition_node CreateInterfaceMember(int offset, string name);
        public static void RestoreSymbolsInterfaceMember(List<SymbolInfo> sil, string name)
        {
            if (sil != null)
            {
                foreach (var si in sil)
                {
                    if (si.sym_info != null)
                        if (si.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                        {
                            wrapped_definition_node wdn = (wrapped_definition_node)si.sym_info;
                            si.sym_info = wdn.PCUReader.CreateInterfaceMember(wdn.offset, name);
                        }
                }
            }
        }


        public abstract definition_node CreateInterfaceInClassMember(int offset, string name);
        public abstract definition_node CreateImplementationMember(int offset, bool restore_code=true);
        public abstract definition_node CreateTypeSynonim(int offset, string name);


        public static void RestoreSymbol(SymbolInfo si, string name, int index = 0)
        {
            if (si != null)
            {
                if (si.sym_info != null)
                {
                    if (si.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                        {
                            wrapped_definition_node wdn = (wrapped_definition_node)si.sym_info;
                            RestoreSymbols(si, wdn, name);
                        }
                }
            }
        }

        public static void RestoreSymbols(List<SymbolInfo> sil, string name, int index = 0)
        {
            if (sil != null)
            {
                for (int i = index; i < sil.Count; ++i)
                {
                    if (sil[i].sym_info != null)
                        if (sil[i].sym_info.semantic_node_type == semantic_node_type.wrap_def)
                        {
                            wrapped_definition_node wdn = (wrapped_definition_node)sil[i].sym_info;
                            RestoreSymbols(sil[i], wdn, name);
                        }
                }
            }
        }

        static void RestoreSymbols(SymbolInfo si, wrapped_definition_node wdn, string name)
        {
        	if (wdn.is_synonim)
        		si.sym_info = wdn.PCUReader.CreateTypeSynonim(wdn.offset, name);
        	else
        	if (si.scope is ClassScope)
                si.sym_info = wdn.PCUReader.CreateInterfaceInClassMember(wdn.offset, name);
            else
                si.sym_info = wdn.PCUReader.CreateImplementationMember(wdn.offset, false);
        }

    }

    /*public class PCUReturner
    {
        private static Hashtable ht = new Hashtable();

        public static void Clear()
        {
            ht.Clear();
        }

        public static BasePCUReader GetPCUReader(PascalABCCompiler.TreeRealization.wrapped_definition_node wdn)
        {
            return (BasePCUReader)ht[wdn];
        }

        public static void AddPCUReader(PascalABCCompiler.TreeRealization.wrapped_definition_node wdn, BasePCUReader pr)
        {
            ht[wdn] = pr;
        }

    }*/

    public class wrapped_definition_node : definition_node
    {
        public int offset;
		public bool is_synonim;
        public BasePCUReader PCUReader;

        public wrapped_definition_node(int offset, BasePCUReader PCUReader)
        {
            this.offset = offset;
            this.PCUReader = PCUReader;
        }

        public override general_node_type general_node_type
        {
            get
            {
                return general_node_type.unit_node;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.wrap_def;
            }
        }
    }
}

namespace SymbolTable
{

	#region SymbolTableConstants набор констант определяющих поведение таблицы символов
	public class SymbolTableConstants
	{
		//стартовый размер списка информаций о символах
		public const int InfoList_StartSize=1;

		//рекомендуемый стартовый размер хештаблицы
		public const int HashTable_StartSize=8192;

		//параметры поведения хеш таблицы
		public const int HashTable_StartResise=85; //Расширить хеш на ProcResize процентов,
		public const int HashTable_ProcResize =100;//если он заполнен на StartResize процентов

		public const int SymbolNotFound=-1;
	}
	#endregion
	
	#region Scope,DotNETScope,UnitPartScope,UnitInterfaceScope,UnitImplementationScope,ClassScope элемены таблицы областей видимости
	//элемент таблицы областей видимости
	//при создании добавляет себя в vSymbolTable
	public class Scope:BaseScope
	{
        public string Name;
        public override string ToString() => Name == ""? GetType().Name : Name;

        public SymbolsDictionary Symbols;
        public List<Scope> InternalScopes;

        public DSSymbolTable SymbolTable;
        public bool CaseSensitive;
        public bool AddStatementsToFront = false; // SSM - введено для необходимости добавлять statements не только в конец statement_list, но и в начало. Нужно для синтаксически сахарных конструкций: например, для создания объекта класса при замыканиях
        public Scope TopScope;

		public int ScopeNum;
        public Scope(DSSymbolTable vSymbolTable, Scope TopScope, string Name)
		{
			SymbolTable=vSymbolTable;
            this.TopScope = null;
            if (TopScope != null) {
                this.TopScope = TopScope;
                TopScope.InternalScopes.Add(this);
            }

            this.Name = Name;

            ScopeNum = SymbolTable.GetNewScopeNum();
            SymbolTable.ScopeTable.Add(this);

            CaseSensitive = SemanticRules.SymbolTableCaseSensitive;

            Symbols = new SymbolsDictionary();
            InternalScopes = new List<Scope>();              
		}
        public Scope(DSSymbolTable vSymbolTable, Scope TopScope, bool CaseSensitive)
        {
            SymbolTable = vSymbolTable;
            this.TopScope = null;
            if (TopScope != null)
            {
                this.TopScope = TopScope;
                TopScope.InternalScopes.Add(this);
            }

            ScopeNum = SymbolTable.GetNewScopeNum();
            SymbolTable.ScopeTable.Add(this);

            this.CaseSensitive = CaseSensitive;

            Symbols = new SymbolsDictionary();
            InternalScopes = new List<Scope>();
        }

        public void ClearScope()
        {
            foreach (var sc in InternalScopes)
                sc.ClearScope();

            Symbols.ClearTable();
        }

        public virtual List<SymbolInfo> Find(string name)
        {
            return Find(name, null);
        }
        public virtual List<SymbolInfo> Find(string name, Scope CurrentScope)
        {
            List<SymbolInfo> sil = SymbolTable.Find(this, name, CurrentScope);
            if (sil == null) return sil;
            PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(sil, name);
            return sil;
        }
        public virtual List<SymbolInfo> FindOnlyInScopeAndBlocks(string name)
		{
            return SymbolTable.FindOnlyInScope(this, name, true);
		}
        public virtual List<SymbolInfo> FindOnlyInScope(string name)
        {
            return SymbolTable.FindOnlyInScope(this, name, false);
        }
        public virtual List<SymbolInfo> FindOnlyInType(string name, Scope CurrentScope)
        {
            List<SymbolInfo> sil = SymbolTable.FindOnlyInType(this, name, CurrentScope);
            if (sil == null) return sil;
            PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(sil, name);
            return sil;
        }
        public void AddSymbol(string Name, SymbolInfo Inf)
		{
			SymbolTable.Add(this, Name, Inf);
		}
	}

    public class BlockScope : Scope
    {
        public BlockScope(DSSymbolTable vSymbolTable, Scope TopScope)
            : base(vSymbolTable, TopScope, "")
		{
        }
    }
	
	public class LambdaScope : Scope  //lroman//
    {
        public LambdaScope(DSSymbolTable vSymbolTable, Scope TopScope)
            : base(vSymbolTable, TopScope, "")
        {
        }
    }
	
    public class WithScope : BlockScope
    {
        public Scope[] WithScopes;
        public WithScope(DSSymbolTable vSymbolTable, Scope TopScope, Scope[] WithScopes)
            : base(vSymbolTable, TopScope)
        {
            this.WithScopes = WithScopes;
        }
    }

	public class DotNETScope:Scope
	{
        public NamespaceScope AdditionalNamespaceScope;
        public DotNETScope(DSSymbolTable vSymbolTable):base(vSymbolTable,null,false)
		{

		}
	}
	
	public class UnitPartScope:Scope
	{
		public Scope[] TopScopeArray;
		public UnitPartScope(DSSymbolTable vSymbolTable,Scope TopScope,Scope[] vTopScopeArray, string Name):
			base(vSymbolTable,TopScope, Name)
		{
			TopScopeArray=vTopScopeArray;
		}
	}
	public class UnitInterfaceScope:UnitPartScope
	{
        
		public UnitInterfaceScope(DSSymbolTable vSymbolTable,Scope TopScope,Scope[] vTopScopeArray, string Name):
			base(vSymbolTable, TopScope, vTopScopeArray, Name)
		{
		}
	}
	public class UnitImplementationScope:UnitPartScope
	{
        public UnitImplementationScope(DSSymbolTable vSymbolTable, Scope TopScope, Scope[] vTopScopeArray, string Name)
            :
			base(vSymbolTable, TopScope, vTopScopeArray, Name)
		{}
	}
    public class NamespaceScope: UnitInterfaceScope
    {
        public NamespaceScope(DSSymbolTable vSymbolTable, Scope TopScope, Scope[] vTopScopeArray, string Name):
			base(vSymbolTable, TopScope, vTopScopeArray, Name)
		{
        }
    }
	public class ClassScope:Scope
	{
        public ClassScope PartialScope;

        public Scope BaseClassScope;

        public ClassScope(DSSymbolTable vSymbolTable,Scope TopScope,Scope BaseClassScope, string Name):
			base(vSymbolTable,TopScope, Name)
		{
            this.BaseClassScope = null;
			if (BaseClassScope != null) 
				this.BaseClassScope = BaseClassScope;
		}
        public override List<SymbolInfo> Find(string name, Scope CurrentScope)
        {
            List<SymbolInfo> si_list = SymbolTable.Find(this, name, CurrentScope);
            if (PartialScope != null)
            {
                if (si_list == null)
                    si_list = SymbolTable.Find(PartialScope, name, CurrentScope);
                else
                {
                    var sil = SymbolTable.Find(PartialScope, name, CurrentScope);
                    if(sil != null)
                        si_list.AddRange(sil);
                }
            }
            if (si_list == null) return si_list;
            PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(si_list, name);
            return si_list;
        }

        public override List<SymbolInfo> FindOnlyInType(string name, Scope CurrentScope)
        {
            List<SymbolInfo> sil = SymbolTable.FindOnlyInType(this, name, CurrentScope);
            if (PartialScope != null)
            {
                if (sil == null)
                    sil = SymbolTable.FindOnlyInType(PartialScope, name, CurrentScope);
                else
                {
                    var temp_sil = SymbolTable.FindOnlyInType(PartialScope, name, CurrentScope);
                    if(temp_sil != null)
                       sil.AddRange(temp_sil);
                }
            }
            if (sil == null) return sil;
            PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(sil, name);
            return sil;
        }
    }

    /*public class GenericTypeInstanceScope : ClassScope
    {
        private PascalABCCompiler.TreeRealization.generic_instance_type_node _instance_type;
        private Scope _orig_scope;

        public GenericTypeInstanceScope(PascalABCCompiler.TreeRealization.generic_instance_type_node instance_type,
            Scope orig_scope, Scope BaseClassScope)
            : base(orig_scope.SymbolTable, orig_scope.TopScope, BaseClassScope)
        {
            _orig_scope = orig_scope;
            _instance_type = instance_type;
        }

        public override SymbolInfoList Find(string name)
        {
            return _instance_type.ConvertSymbolInfo(_orig_scope.Find(name));
        }

        public override SymbolInfoList Find(string name, Scope CurrentScope)
        {
            return _instance_type.ConvertSymbolInfo(_orig_scope.Find(name, CurrentScope));
        }

        public override SymbolInfoList FindOnlyInScope(string name)
        {
            return _instance_type.ConvertSymbolInfo(_orig_scope.FindOnlyInScope(name));
        }

        public override SymbolInfoList FindOnlyInScopeAndBlocks(string name)
        {
            return _instance_type.ConvertSymbolInfo(_orig_scope.FindOnlyInScopeAndBlocks(name));
        }
    }*/

    //(ssyy) Интерфейс для интерфейсов
    public interface IInterfaceScope
    {
        Scope[] TopInterfaceScopeArray
        {
            get;
            set;
        }
    }

    //ssyy owns
    //Область видимости для интерфейсов
    //Наследование от ClassScope сделано для простоты, так как в кодах
    //местами есть проверки (scope is ClassScope)
    public class InterfaceScope : ClassScope, IInterfaceScope
    {
        private Scope[] _TopInterfaceScopeArray;
        public virtual Scope[] TopInterfaceScopeArray
        {
            get
            {
                return _TopInterfaceScopeArray;
            }
            set
            {
                _TopInterfaceScopeArray = value;
            }
        }

        public InterfaceScope(DSSymbolTable vSymbolTable, Scope TopScope, Scope[] vTopInterfaceScopeArray, string Name)
            :
            base(vSymbolTable, TopScope, null, Name)
        {
            _TopInterfaceScopeArray = vTopInterfaceScopeArray;
        }

        public InterfaceScope(DSSymbolTable vSymbolTable, Scope TopScope, Scope BaseClassScope, Scope[] vTopInterfaceScopeArray, string Name)
            :
            base(vSymbolTable, TopScope, BaseClassScope, Name)
        {
            _TopInterfaceScopeArray = vTopInterfaceScopeArray;
        }
    }
    //\ssyy owns

	public class ClassMethodScope:Scope
	{
        public Scope MyClass;

		public ClassMethodScope(DSSymbolTable vSymbolTable,Scope TopScope,Scope MyClass, string Name):
			base(vSymbolTable,TopScope, Name)
		{
            this.Name = Name;
            this.MyClass = null;
			if (MyClass != null) 
				this.MyClass = MyClass;
		}
	}
	#endregion

    #region HashTableNode элемент хеш-таблицы
    public class HashTableNode
    {
        public string Name;
        public List<SymbolInfo> InfoList;
        public HashTableNode(string name)
        {
            Name = name;
            InfoList = new List<SymbolInfo>(SymbolTableConstants.InfoList_StartSize);
        }
        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            foreach (var sym in InfoList)
                str.Append(sym.ToString() + ";");
            return str.ToString();
        }
    }
    #endregion

    #region Для отображения таблицы символов в отладке
    public class LightSymbolnfo
    {
        public PrimaryScope inner_scope
        {
            get
            {
                if (information.sym_info is PascalABCCompiler.TreeRealization.common_function_node)
                {
                    var temp = information.sym_info as PascalABCCompiler.TreeRealization.common_function_node;
                    if (temp.scope != null)
                        return new PrimaryScope(temp.scope);
                }
                else if (information.sym_info is PascalABCCompiler.TreeRealization.type_node)
                {
                    var temp = information.sym_info as PascalABCCompiler.TreeRealization.type_node;
                    if (temp.Scope != null)
                        return new PrimaryScope(temp.Scope);
                }
                return null;
            }
        }
        public SymbolInfo information;

        public override string ToString() => information.ToString();

        public LightSymbolnfo(SymbolInfo si)
        {
            information = si;
        }
    }

    public class LightScopeNode
    {
        public List<LightSymbolnfo> Overloads = new List<LightSymbolnfo>();

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            foreach (var sym in Overloads)
                str.Append(sym.ToString() + ";");
            return str.ToString();
        }
    }

    public class PrimaryScope
    {
        public List<PrimaryScope> SubScopes
        {
            get
            {
                var subScopes = new List<PrimaryScope>();
                foreach (var sub_sc in real_scope.InternalScopes)
                    subScopes.Add(new PrimaryScope(sub_sc));
                return subScopes;
            }
        }
        public List<LightScopeNode> Symbols
        {
            get
            {
                var symbols = new List<LightScopeNode>();
                foreach (var sy in real_scope.Symbols.dict)
                {
                    LightScopeNode res = new LightScopeNode();
                    foreach (var sy_overload in sy.Value.InfoList)
                        res.Overloads.Add(new LightSymbolnfo(sy_overload));
                    symbols.Add(res);
                }
                return symbols;
            }
        }

        public string ScopeName;
        public override string ToString() => ScopeName == "" ? GetType().Name : ScopeName;
        private Scope real_scope;
        public PrimaryScope(Scope sc)
        {
            ScopeName = sc.Name;
            real_scope = sc;
        }
    }
    #endregion

    // Определения
    // ОВ - область видимости
    // ООВ - особая область видимости в стиле delphi.это:
    //  - ОВ поцедуры
    //  - OВ класса + ОВ предков
    //  - ОВ модуля + ОВ интерфейсных частей всех модулей подклюценных к нему
    public class DSSymbolTable
	{
		public List<Scope> ScopeTable;

        internal bool CaseSensitive;

        private Scope CurrentScope;
        private int ScopeIndex = -1;

        /*public override string ToString()
        {

            var sb = new System.Text.StringBuilder();
            var a = ScopeTable.SkipWhile(s => !(s.GetType() == typeof(UnitInterfaceScope))).Skip(1).SkipWhile(s => !(s.GetType() == typeof(UnitInterfaceScope)));
            var globscopenum = a.First().ScopeNum;
            var d = new Dictionary<Scope, List<Tuple<string, SymbolInfo>>>();
            foreach (var x in a)
            {
                sb.Append(x.ToString() + "\n");
                //d[x.ScopeNum] = x;
            }
            foreach (var x in HashTable.dict)
            {
                foreach (var y in x.Value.NumAreaList.data.Take(x.Value.NumAreaList.Count))
                {
                    foreach (var z in y.InfoList)
                    {
                        if (z.scope.ScopeNum >= globscopenum)
                        {
                            if (!d.ContainsKey(z.scope))
                                d[z.scope] = new List<Tuple<string, SymbolInfo>>();
                            d[z.scope].Add(new Tuple<string, SymbolInfo>(x.Key, z));
                        }
                    }
                }
            }
            sb.Append("\n");

            foreach (var x in d.OrderBy(x => x.Key.ScopeNum))
            {
                sb.Append(x.Key.ScopeNum + "—>" + x.Key.TopScopeNum + "\n");
                foreach (var y in x.Value)
                {
                    sb.Append("  " + y.Item1 + ": " + y.Item2.sym_info.ToString() + "\n");
                }
            }
            return sb.ToString();
        }*/

        #region DSSymbolTable(int hash_size,bool case_sensitive)
        public DSSymbolTable(int hash_size,bool case_sensitive)
		{
            CaseSensitive = case_sensitive;
            Clear();
        }
		#endregion

		#region Clear() очистка таблицы
		public void Clear()
		{
            ScopeTable = new List<Scope>();
            ScopeIndex = -1;
        }
		#endregion
			
		#region CreateScope для различных Scope
		public LambdaScope CreateLambdaScope(Scope TopScope) //lroman//
        {
            return new LambdaScope(this, TopScope);
        }
		public Scope CreateScope(Scope TopScope, string Name = "")
		{
			return new Scope(this, TopScope, Name);
		}
		public ClassScope CreateClassScope(Scope TopScope,Scope BaseClass, string Name = "")
		{
            return new ClassScope(this, TopScope, BaseClass, Name);	
		}
        //ssyy
        public InterfaceScope CreateInterfaceScope(Scope TopScope, Scope[] TopInterfaces, string Name = "")
        {
            return new InterfaceScope(this, TopScope, TopInterfaces, Name);
        }
        public InterfaceScope CreateInterfaceScope(Scope TopScope, Scope BaseClass, Scope[] TopInterfaces, string Name = "")
        {
            return new InterfaceScope(this, TopScope, BaseClass, TopInterfaces, Name);
        }
        //\ssyy
        public UnitInterfaceScope CreateUnitInterfaceScope(Scope[] UsedUnits, string Name = "")
		{
			return new UnitInterfaceScope(this, null, UsedUnits, Name);
		}
        public NamespaceScope CreateNamespaceScope(Scope[] UsedUnits, Scope TopScope, string Name = "")
        {
            return new NamespaceScope(this, TopScope, UsedUnits, Name);
        }
        public UnitImplementationScope CreateUnitImplementationScope(Scope InterfaceScope,Scope[] UsedUnits, string Name = "")
		{
			return new UnitImplementationScope(this, InterfaceScope, UsedUnits, Name);
		}
		public ClassMethodScope CreateClassMethodScope(Scope TopScope,Scope MyClass, string Name = "")
		{
			return new ClassMethodScope(this, TopScope, MyClass, Name);
		}
		#endregion

		//Возвращает количество уровней на которые надо поднятся начиная с Down чтобы очутиться в Up
		//Работает только для процедур. Модуль считает за одно Scope
		public int GetRelativeScopeDepth(Scope Up,Scope Down)
		{
			if (Up == Down) return 0;
			int depth = 0;
			while(Down.TopScope != null)
			{
				if (Up == Down) return depth;
				if(!(Down is UnitImplementationScope))
					depth++;
				Down = Down.TopScope;
			}
			//throw new Exception("Can not execute st depth");
			return -1;
		}

		//Добавление символа
		//если такой символ в пр-ве имен уже существует то symbol_info добавляется к Symbols[].InfoList[]
        public void Add(Scope InScope, string Name, SymbolInfo Inf)
        {
            Inf.scope = InScope;
            if (!InScope.CaseSensitive) Name = Name.ToLower();
            var hn = InScope.Symbols.Add(Name);//ЗДЕСЬ ВОЗНИКАЕТ НЕДЕТЕРМЕНИРОВАННАЯ ОШИБКА - SSM 07.10.17 - странный комментарий. Вроде всё нормально.
            // SSM 07.10.17 - переделал внутреннее представление HashTable на основе Dictionary
            if (hn == null)
                throw new Exception("Попытка добавить уже добавленное имя " + Name + " в HashTable. Обратитесь к разработчикам");

            hn.Name = Name;
            hn.InfoList.Add(Inf);
        }
        public void RemoveScope(Scope scope)
        {
#if (DEBUG)
            if (scope == null) throw new Exception("Ошибка при взятии верхней области видимости: область с номером " + scope + " не существует");
#endif
            scope.ClearScope();

            foreach (var in_scope in scope.InternalScopes)
                ScopeTable.Remove(in_scope);
            ScopeTable.Remove(scope);

            if (scope.TopScope != null)
                scope.TopScope.InternalScopes.Remove(scope);
        }

        //Этот метод ищет ТОЛЬКО В УКАЗАННОЙ ОВ, и не смотрит есть ли имя выше.
        //Если это ОВ типа UnitImplementationScope то имя ищется также и
        //в верней ОВ, которая типа UnitInterfaceScope
        public List<SymbolInfo> FindOnlyInScope(Scope scope, string Name, bool FindInUpperBlocks)
        {
            if (!scope.CaseSensitive) Name = Name.ToLower();
            CurrentScope = null;

            List<SymbolInfo> Result = new List<SymbolInfo>();

            if (scope is DotNETScope)//если нет такого ищем в областях .NET
            {
                AddToSymbolInfo(Result, (DotNETScope)scope, Name);
                return Result.Count() > 0 ? Result : null;
            }

            Scope CurrentArea = scope, bs;
            HashTableNode tn;
            do
            {
                if (CurrentArea is UnitPartScope) //мы очутились в модуле
                {
                    //мы в ImplementationPart?
                    if (CurrentArea is UnitImplementationScope)
                    {
                        tn = CurrentArea.Symbols.Find(Name);
                        if (tn != null) //что-то нашли!
                            AddToSymbolInfo(tn.InfoList, Result);
                        CurrentArea = CurrentArea.TopScope;
                    }
                    //сейча мы в InterfacePart
                    tn = CurrentArea.Symbols.Find(Name);
                    if (tn != null) //что-то нашли!
                        AddToSymbolInfo(tn.InfoList, Result);

                    if (Result.Count() > 0)
                        return Result;
                }
                if (CurrentArea is WithScope)//мы очутились в Width
                {
                    tn = CurrentArea.Symbols.Find(Name);
                    if (tn != null) //что-то нашли!
                        AddToSymbolInfo(tn.InfoList, Result);

                    if (Result.Count() > 0) //если что-то нашли то заканчиваем
                        return Result;

                    FindAllInAreaList(Name, (CurrentArea as WithScope).WithScopes, true, true, Result);
                    if (Result.Count() > 0) //если что-то нашли то заканчиваем
                        return Result;
                }
                else
                {
                    tn = CurrentArea.Symbols.Find(Name);
                    if (tn != null) //что-то нашли!
                    {
                        AddToSymbolInfo(tn.InfoList, Result);
                        return Result.Count() > 0 ? Result : null;
                    }
                }
                bs = CurrentArea;
                CurrentArea = CurrentArea.TopScope;
            } while (CurrentArea != null && (FindInUpperBlocks && bs is BlockScope));

            return null;
        }
        private void FindAllInClass(string name, Scope ClassArea, bool OnlyInThisClass, List<SymbolInfo> Result)
        {
            HashTableNode tn;
            Scope ar = ClassArea;

            if ((tn = ar.Symbols.Find(name)) != null)
                AddToSymbolInfo(tn.InfoList, Result);

            if (ar is DotNETScope)
            {
                PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(Result, name);
                AddToSymbolInfo(Result, (DotNETScope)ar, name);
                return;
            }

            ClassScope cl = (ClassScope)ClassArea;

            if (!OnlyInThisClass)
                while (cl.BaseClassScope != null)
                {
                    tn = cl.BaseClassScope.Symbols.Find(name);
                    if (tn != null)
                        AddToSymbolInfo(tn.InfoList, Result);

                    ar = cl.BaseClassScope;
                    if (ar is DotNETScope)
                    {
                        AddToSymbolInfo(Result, (DotNETScope)ar, name);
                        return;
                    }
                    cl = (ClassScope)cl.BaseClassScope;
                }
        }

        private Scope FindUnitInterfaceScope(Scope scope)
        {
            while (scope!=null && !(scope is UnitInterfaceScope))
                scope = scope.TopScope;
            return scope;
        }
        private Scope FindClassScope(Scope scope)
        {
            while (scope != null && !(scope is ClassScope))
                if(scope is ClassMethodScope)
                    scope = ((ClassMethodScope)scope).MyClass;
                else
                    scope = scope.TopScope;
            return scope;
        }
        
        private bool IsInOneModule(Scope Scope1, Scope Scope2)
        {
            Scope1 = FindUnitInterfaceScope(Scope1);
            Scope2 = FindUnitInterfaceScope(Scope2);
            return (Scope1 != null) && (Scope2 != null) && (Scope1 == Scope2);
        }
        
        private bool IsInOneOrDerivedClass(Scope IdentScope, Scope FromScope)
        {
            IdentScope = FindClassScope(IdentScope);
            FromScope = FindClassScope(FromScope);
            while (FromScope != null)
            {
                if (IdentScope == FromScope)
                    return true;
                if (FromScope is ClassScope)
                    FromScope = ((ClassScope)FromScope).BaseClassScope;
                else
                    FromScope = ((PascalABCCompiler.NetHelper.NetTypeScope)FromScope).TopScope;
            }
            return false;
            
        }
        
        private bool IsVisible(SymbolInfo ident, Scope fromScope)
        {
            if (fromScope == null)
                return true;
            if (FindClassScope(ident.scope) == null)
                return true;
            switch (ident.access_level)
            {
                case access_level.al_public:
                case access_level.al_internal:
                    return true;
                case access_level.al_protected:
                    return IsInOneModule(ident.scope, fromScope) || IsInOneOrDerivedClass(ident.scope, fromScope);
                case access_level.al_private:
                    return IsInOneModule(ident.scope, fromScope);
            }
            return true;
        }

        private bool IsNormal(SymbolInfo to, SymbolInfo add)
        {
            return //true;
                (to == null) || (to.scope == null) || ((to.scope != null) && //to.scope == null не нужно?
                (((to.symbol_kind == symbol_kind.sk_none) && (add.symbol_kind == symbol_kind.sk_none)) && (to.scope == add.scope))
                ||
                ((to.symbol_kind == symbol_kind.sk_overload_function) && (add.symbol_kind == symbol_kind.sk_overload_function))
                ||
                ((to.symbol_kind == symbol_kind.sk_overload_procedure) && (add.symbol_kind == symbol_kind.sk_overload_procedure))
                || to.sym_info != add.sym_info && to.sym_info is PascalABCCompiler.TreeRealization.function_node && add.sym_info is PascalABCCompiler.TreeRealization.function_node && (to.sym_info as PascalABCCompiler.TreeRealization.function_node).is_extension_method && (add.sym_info as PascalABCCompiler.TreeRealization.function_node).is_extension_method
                );
        }

        private void AddToSymbolInfo(List<SymbolInfo> from, List<SymbolInfo> to)
        {
            bool CheckVisible = CurrentScope != null, NeedAdd = false;
            SymbolInfo last_sym = to.LastOrDefault();

            foreach (SymbolInfo si in from)
            {
                if (CheckVisible)
                    NeedAdd = IsVisible(si, CurrentScope) && IsNormal(last_sym, si);
                else
                    NeedAdd = IsNormal(last_sym, si);
                if (NeedAdd && to.IndexOf(si) == -1)
                {
                    to.Add(si);
                    last_sym = si;
                }
            }
        }
        private void AddToSymbolInfo(List<SymbolInfo> to, DotNETScope ar, string name)
        {
            List<SymbolInfo> sil = ar.Find(name);
            if (sil != null)
                if (IsNormal(to.LastOrDefault(), sil.FirstOrDefault()))
                    to.AddRange(sil);
        }

        private void FindAllInAreaList(string name, Scope[] arr, bool need, List<SymbolInfo> Result)
        {
            FindAllInAreaList(name, arr, false, need, Result);
        }
        public void FindAllInAreaList(string name, Scope[] arr, bool StopIfFind, bool NotOnlyInNetScopes, List<SymbolInfo> Result)
        {
            if (arr == null) return;

            int add = Result.Count;
            HashSet<Assembly> assm_cache = new HashSet<Assembly>();
            foreach (Scope sc in arr)
            {
                if (sc is DotNETScope)
                {
                    if (sc is PascalABCCompiler.NetHelper.NetScope)
                    {
                        PascalABCCompiler.NetHelper.NetScope netScope = sc as PascalABCCompiler.NetHelper.NetScope;
                        if (PascalABCCompiler.NetHelper.NetHelper.PABCSystemType == null || netScope.Assembly != PascalABCCompiler.NetHelper.NetHelper.PABCSystemType.Assembly)
                        {
                            if (!assm_cache.Contains(netScope.Assembly))
                                assm_cache.Add(netScope.Assembly);
                            else if (netScope.used_namespaces.Count == 0)
                                continue;
                        }
                    }
                    AddToSymbolInfo(Result, (DotNETScope)sc, name);
                    if (Result.Count > add && StopIfFind)
                        return;
                }
                else
                if (NotOnlyInNetScopes && sc != null)
                {
                    var tn = sc.Symbols.Find(name);
                    if (tn != null)
                    {
                        AddToSymbolInfo(tn.InfoList, Result);
                        if (Result.Count > add && StopIfFind)
                            return;
                    }
                }
            }
        }
        //поиск всех имен в ООВ.
        //  ищет наборы имен в ООВ, если находит то возвращает их список.
        //  иначе ищет в обьемлющем ООВ.
        //SymbolInfo возвращаются в поряде в котором они встретились при проходе областей
        public List<SymbolInfo> Find(Scope scope, string Name)
        {
           return FindAll(scope, Name, false, false, null);
        }

        public List<SymbolInfo> Find(Scope scope, string Name, Scope FromScope)
        {
            return FindAll(scope, Name, false, false, FromScope);
        }
        public List<SymbolInfo> FindOnlyInType(Scope scope, string Name)
        {
            return FindAll(scope, Name, true, false, null);
        }
        public List<SymbolInfo> FindOnlyInType(Scope scope, string Name, Scope FromScope)
        {
            return FindAll(scope, Name, true, false, FromScope);
        }

        public List<SymbolInfo> FindOnlyInThisClass(ClassScope scope, string Name)
        {
            return FindAll(scope, Name, true, true, null);
        }
        private List<SymbolInfo> FindAll(Scope scope, string Name, bool OnlyInType, bool OnlyInThisClass, Scope FromScope)
        {
            if (OnlyInType && !(scope is ClassScope) && !(scope is SymbolTable.DotNETScope)) return null;
            //if (!CaseSensitive) Name=Name.ToLower();

            if (!scope.CaseSensitive)
                Name = Name.ToLower();
            CurrentScope = FromScope; //глобальные переменные могут привести к ошибкам при поиске и поторном вызове!

            List<SymbolInfo> Result = new List<SymbolInfo>();

            Scope Area = scope;
            Scope[] used_units = null;

            HashTableNode tn = null;
            if (!(scope is DotNETScope) && !Name.StartsWith("?"))
            {
                Scope CurrentArea = Area;
                while (CurrentArea != null)
                {
                    if (CurrentArea is UnitPartScope) //мы очутились в модуле
                    {
                        //мы в ImplementationPart?
                        if (CurrentArea is UnitImplementationScope)
                        {
                            used_units = (CurrentArea as UnitImplementationScope).TopScopeArray;
                            tn = CurrentArea.Symbols.Find(Name);
                            if (tn != null) //что-то нашли!
                                AddToSymbolInfo(tn.InfoList, Result);
                            CurrentArea = CurrentArea.TopScope;
                        }
                        //сейча мы в InterfacePart
                        tn = CurrentArea.Symbols.Find(Name);
                        if (tn != null) //что-то нашли!
                            AddToSymbolInfo(tn.InfoList, Result);
                        //смотрим в модулях
                        FindAllInAreaList(Name, used_units, true, Result);
                        FindAllInAreaList(Name, (CurrentArea as UnitInterfaceScope).TopScopeArray, true, Result);

                        return Result.Count > 0 ? Result : null;
                    }
                    else
                    if (CurrentArea is IInterfaceScope)
                    {
                        FindAllInClass(Name, CurrentArea, OnlyInThisClass, Result);

                        if (Result.Count > 0) //если что-то нашли то заканчиваем
                            return Result;

                        //Зачем искать в интерфейсах?
                        //(ssyy) Не понимаю вопрос. Спросившему подумать, зачем в компиляторе нужен поиск.
                        FindAllInAreaList(Name, (CurrentArea as IInterfaceScope).TopInterfaceScopeArray, true, Result);

                        if (Result.Count > 0 || OnlyInType) //если что-то нашли то заканчиваем
                            return Result.Count > 0 ? Result : null;
                    }
                    else
                    if (CurrentArea is ClassScope)//мы очутились в классе
                    {
                        FindAllInClass(Name, CurrentArea, OnlyInThisClass, Result);//надо сделать поиск по его предкам

                        if (Result.Count > 0 || OnlyInType) //если что-то нашли то заканчиваем
                            return Result.Count > 0 ? Result : null;
                        //иначе ищем дальше
                    }
                    else
                    if (CurrentArea is WithScope)//мы очутились в With
                    {
                        tn = CurrentArea.Symbols.Find(Name);
                        if (tn != null) //что-то нашли!
                            AddToSymbolInfo(tn.InfoList, Result);
                        if (Result.Count > 0) //если что-то нашли то заканчиваем
                            return Result;
                        Scope[] wscopes = (CurrentArea as WithScope).WithScopes;
                        if (wscopes != null)
                            foreach (Scope wsc in wscopes)
                            {
                                FindAllInClass(Name, wsc, OnlyInThisClass, Result);//надо сделать поиск по его предкам                    

                                if (Result.Count > 0) //если что-то нашли то заканчиваем
                                    return Result;
                            }
                    }
                    else
                    {
                        tn = CurrentArea.Symbols.Find(Name);
                        if (tn != null) //что-то нашли!
                        {
                            AddToSymbolInfo(tn.InfoList, Result);
                            return Result.Count > 0 ? Result : null;
                        }
                        if (CurrentArea is ClassMethodScope)//мы очутились в методе класса
                        {
                            FindAllInClass(Name, (CurrentArea as ClassMethodScope).MyClass, OnlyInThisClass, Result);//надо сделать поиск по его классу

                            if (Result.Count > 0) //если что-то нашли то заканчиваем
                                return Result;
                        }
                    }
                    CurrentArea = CurrentArea.TopScope;//Пошли вверх
                }
            }

            //если нет такого ищем в областях .NET

            // SSM 21.01.16
            if (Name.StartsWith("?"))     // это значит, надо искать в областях .NET
                Name = Name.Substring(1); // съели ? и ищем т.к. tn<0
            // end SSM 

            //ssyy
            Scope NextUnitArea = null;
            //\ssyy
            Scope an;
            tn = Area.Symbols.Find(Name);
            while (Area != null)
            {
                an = Area;
                if (an is DotNETScope)
                {
                    if (tn == null)
                        AddToSymbolInfo(Result, (DotNETScope)an, Name);
                    else
                        FindAllInClass(Name, Area, false, Result);
                }
                if (Result.Count > 0)
                    return Result;
                if (an is UnitPartScope)
                {
                    if (an is UnitImplementationScope)
                    {
                        FindAllInAreaList(Name, (an as UnitImplementationScope).TopScopeArray, true, Result);
                        an = an.TopScope;
                    }

                    FindAllInAreaList(Name, (an as UnitInterfaceScope).TopScopeArray, false, Result);

                    if (Result.Count > 0)
                        return Result;
                }
                if (an is WithScope)//мы очутились в Width
                {
                    FindAllInAreaList(Name, (an as WithScope).WithScopes, true, false, Result);

                    if (Result.Count > 0) //если что-то нашли то заканчиваем
                        return Result;
                }
                if (an is ClassScope)
                {
                    Scope unit_area = an.TopScope;
                    InterfaceScope IntScope = an as InterfaceScope;
                    while (((ClassScope)an).BaseClassScope != null)
                    {
                        an = ((ClassScope)an).BaseClassScope;
                        if (an is DotNETScope)
                        {
                            AddToSymbolInfo(Result, (DotNETScope)an, Name);
                            if (Result.Count > 0) // || OnlyInType) 
                                return Result;
                            break;
                        }
                    }
                    //В предках ничего не нашли, ищем по интерфейсам...
                    if (IntScope != null)
                    {
                        FindAllInAreaList(Name, IntScope.TopInterfaceScopeArray, false, Result);
                        if (Result.Count > 0) //если что-то нашли то заканчиваем
                            return Result;

                    }
                    if (OnlyInType)
                        return Result.Count > 0 ? Result : null;

                    //ssyy
                    if (NextUnitArea != null)
                    {
                        Area = NextUnitArea;
                        //NextUnitArea = null;
                        continue;
                    }
                    else
                        //\ssyy
                        an = unit_area;
                }
                if (Result.Count > 0)
                    return Result;


                if (an is ClassMethodScope)
                {
                    //ssyy
                    NextUnitArea = an.TopScope;
                    //\ssyy
                    Area = (an as ClassMethodScope).MyClass;
                }
                else
                    Area = Area.TopScope;
                //Area = Area.TopScope;
            }
            return null;                //если такого нет то поиск окончен
        }

        public int GetNewScopeNum()
        {
            return ++ScopeIndex;
        }
    }
    public class TreeConverterSymbolTable:DSSymbolTable
	{
		public TreeConverterSymbolTable(bool case_sensitive):base(SymbolTableConstants.HashTable_StartSize,case_sensitive){}
		public TreeConverterSymbolTable():base(SymbolTableConstants.HashTable_StartSize,false){}
	}
    public class SymbolTableController
    {
        public static TreeConverterSymbolTable CurrentSymbolTable = new TreeConverterSymbolTable();
    }

}
