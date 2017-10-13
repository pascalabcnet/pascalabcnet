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
        public static void RestoreSymbolsInterfaceMember(SymbolInfoList si, string name)
        {
            if (si != null)
            {
                foreach (var si_unit in si.InfoUnitList)
                {
                    if (si_unit.sym_info != null)
                        if (si_unit.sym_info.semantic_node_type == PascalABCCompiler.TreeRealization.semantic_node_type.wrap_def)
                        {
                            PascalABCCompiler.TreeRealization.wrapped_definition_node wdn = (PascalABCCompiler.TreeRealization.wrapped_definition_node)si_unit.sym_info;
                            si_unit.sym_info = wdn.PCUReader.CreateInterfaceMember(wdn.offset, name);
                        }
                }
            }
        }


        public abstract definition_node CreateInterfaceInClassMember(int offset, string name);
        public abstract definition_node CreateImplementationMember(int offset, bool restore_code=true);
        public abstract definition_node CreateTypeSynonim(int offset, string name);
        
        public static void RestoreSymbols(SymbolInfoList si, string name, int index = 0)
        {
            if (si != null)
            {
                for (int i = index; i < si.InfoUnitList.Count; ++i)
                {
                    if (si.InfoUnitList[i].sym_info != null)
                        if (si.InfoUnitList[i].sym_info.semantic_node_type == PascalABCCompiler.TreeRealization.semantic_node_type.wrap_def)
                        {
                            PascalABCCompiler.TreeRealization.wrapped_definition_node wdn = (PascalABCCompiler.TreeRealization.wrapped_definition_node)si.InfoUnitList[i].sym_info;
                            RestoreSymbols(si.InfoUnitList[i], wdn, name);
                        }
                }
            }

        }

        static void RestoreSymbols(SymbolInfoUnit si, PascalABCCompiler.TreeRealization.wrapped_definition_node wdn, string name)
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
		//стартовый размер списка областей видимости
		public const int AreaList_StartSize=8;
		//во сколько раз расширять список областей видимости
		public const int AreaList_ResizeParam=2;

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
        public override bool Equals(object obj)
        {
            return ScopeNum == ((Scope)obj).ScopeNum;
        }
        public override int GetHashCode()
        {
            return ScopeNum;
        }
        private string ScopeName()
        {
            var s = this.GetType().Name;
            if (s == "UnitInterfaceScope")
                return "GLOBAL";
            return s;
        }
        public override string ToString() => ScopeNum + "->" + TopScopeNum + "," + ScopeName();

        public DSSymbolTable SymbolTable;
        public bool CaseSensitive;
		public int TopScopeNum;
        public bool AddStatementsToFront = false; // SSM - введено для необходимости добавлять statements не только в конец statement_list, но и в начало. Нужно для синтаксически сахарных конструкций: например, для создания объекта класса при замыканиях
		public Scope TopScope
		{
			get 
			{
				if(TopScopeNum>=0)
					return SymbolTable.ScopeTable[TopScopeNum];
				else
					return null;
			}
		}
		//public Scope()
		//{
		//}
		public int ScopeNum;
        public Scope(DSSymbolTable vSymbolTable, Scope TopScope)
		{
			SymbolTable=vSymbolTable;
			TopScopeNum=-1;
			if (TopScope!=null) 
				TopScopeNum=TopScope.ScopeNum;
            ScopeNum = SymbolTable.ScopeTable.Count;
			SymbolTable.ScopeTable.Add(this);
            this.CaseSensitive = SemanticRules.SymbolTableCaseSensitive;
		}
        public Scope(DSSymbolTable vSymbolTable, Scope TopScope, bool CaseSensetive)
        {
            SymbolTable = vSymbolTable;
            TopScopeNum = -1;
            if (TopScope != null)
                TopScopeNum = TopScope.ScopeNum;
            ScopeNum = SymbolTable.ScopeTable.Count;
            SymbolTable.ScopeTable.Add(this);
            this.CaseSensitive = CaseSensetive;
        }
        public virtual SymbolInfoList Find(string name)
        {
            return Find(name, null);
        }
        public virtual SymbolInfoList Find(string name, Scope CurrentScope)
        {
            SymbolInfoList si = SymbolTable.Find(this, name, CurrentScope);
            if (si == null) return si;
            PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(si, name);
            return si;
        }
        public virtual SymbolInfoList FindOnlyInScopeAndBlocks(string name)
		{
            return SymbolTable.FindOnlyInScope(this, name, true);
		}
        public virtual SymbolInfoList FindOnlyInScope(string name)
        {
            return SymbolTable.FindOnlyInScope(this, name, false);
        }
        public virtual SymbolInfoList FindOnlyInType(string name, Scope CurrentScope)
        {
            SymbolInfoList si = SymbolTable.FindOnlyInType(this, name, CurrentScope);
            if (si == null) return si;
            PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(si, name);
            return si;
        }
        public void AddSymbol(string Name,SymbolInfoUnit Inf)
		{
			SymbolTable.Add(this,Name,Inf);
		}
	}

    public class BlockScope : Scope
    {
        public BlockScope(DSSymbolTable vSymbolTable, Scope TopScope)
            : base(vSymbolTable, TopScope)
		{
        }
    }
	
	public class LambdaScope : Scope  //lroman//
    {
        public LambdaScope(DSSymbolTable vSymbolTable, Scope TopScope)
            : base(vSymbolTable, TopScope)
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
		public UnitPartScope(DSSymbolTable vSymbolTable,Scope TopScope,Scope[] vTopScopeArray):
			base(vSymbolTable,TopScope)
		{
			TopScopeArray=vTopScopeArray;
		}
	}
	public class UnitInterfaceScope:UnitPartScope
	{
        
		public UnitInterfaceScope(DSSymbolTable vSymbolTable,Scope TopScope,Scope[] vTopScopeArray):
			base(vSymbolTable,TopScope,vTopScopeArray)
		{
		}
	}
	public class UnitImplementationScope:UnitPartScope
	{
        public UnitImplementationScope(DSSymbolTable vSymbolTable, Scope TopScope, Scope[] vTopScopeArray)
            :
			base(vSymbolTable,TopScope,vTopScopeArray)
		{}
	}
    public class NamespaceScope: UnitInterfaceScope
    {
        public NamespaceScope(DSSymbolTable vSymbolTable, Scope TopScope, Scope[] vTopScopeArray):
			base(vSymbolTable,TopScope,vTopScopeArray)
		{
        }
    }
	public class ClassScope:Scope
	{
		public int BaseClassScopeNum;
        public ClassScope PartialScope;

		public Scope BaseClassScope
		{
			get 
			{
				if (BaseClassScopeNum>=0) 
					return SymbolTable.ScopeTable[BaseClassScopeNum];
				else
					return null;

			}
			//TODO: Kolya add this set accessor
			//Ask for Alexander
			set
			{
                if (value == null)
                {
                    BaseClassScopeNum = -2;
                    return;
                }
			    BaseClassScopeNum=value.ScopeNum;
            }
		}
		public ClassScope(DSSymbolTable vSymbolTable,Scope TopScope,Scope BaseClassScope):
			base(vSymbolTable,TopScope)
		{
			BaseClassScopeNum=-2;
			if (BaseClassScope!=null) 
				BaseClassScopeNum=BaseClassScope.ScopeNum;
		}
        public override SymbolInfoList Find(string name, Scope CurrentScope)
        {
            SymbolInfoList si_list = SymbolTable.Find(this, name, CurrentScope);
            if (PartialScope != null)
            {
                if (si_list == null)
                    si_list = SymbolTable.Find(PartialScope, name, CurrentScope);
                else
                {
                    si_list.Add(SymbolTable.Find(PartialScope, name, CurrentScope));
                }
            }
            if (si_list == null) return si_list;
            PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(si_list, name);
            return si_list;
        }

        public override SymbolInfoList FindOnlyInType(string name, Scope CurrentScope)
        {
            SymbolInfoList si = SymbolTable.FindOnlyInType(this, name, CurrentScope);
            if (PartialScope != null)
            {
                if (si == null)
                    si = SymbolTable.FindOnlyInType(PartialScope, name, CurrentScope);
                else
                {
                    si.Add(SymbolTable.FindOnlyInType(PartialScope, name, CurrentScope));
                }
            }
            if (si == null) return si;
            PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(si, name);
            return si;
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

        public override SymbolInfo Find(string name)
        {
            return _instance_type.ConvertSymbolInfo(_orig_scope.Find(name));
        }

        public override SymbolInfo Find(string name, Scope CurrentScope)
        {
            return _instance_type.ConvertSymbolInfo(_orig_scope.Find(name, CurrentScope));
        }

        public override SymbolInfo FindOnlyInScope(string name)
        {
            return _instance_type.ConvertSymbolInfo(_orig_scope.FindOnlyInScope(name));
        }

        public override SymbolInfo FindOnlyInScopeAndBlocks(string name)
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

        public InterfaceScope(DSSymbolTable vSymbolTable, Scope TopScope, Scope[] vTopInterfaceScopeArray)
            :
            base(vSymbolTable, TopScope, null)
        {
            _TopInterfaceScopeArray = vTopInterfaceScopeArray;
        }

        public InterfaceScope(DSSymbolTable vSymbolTable, Scope TopScope, Scope BaseClassScope, Scope[] vTopInterfaceScopeArray)
            :
            base(vSymbolTable, TopScope, BaseClassScope)
        {
            _TopInterfaceScopeArray = vTopInterfaceScopeArray;
        }
    }
    //\ssyy owns

	public class ClassMethodScope:Scope
	{
		public int MyClassNum;
		public Scope MyClass
		{
			get 
			{
				if (MyClassNum>=0) 
					return SymbolTable.ScopeTable[MyClassNum];
				else
					return null;
			}
		}
		public ClassMethodScope(DSSymbolTable vSymbolTable,Scope TopScope,Scope MyClass):
			base(vSymbolTable,TopScope)
		{
			MyClassNum=-2;
			if (MyClass!=null) 
				MyClassNum=MyClass.ScopeNum;
		}
	}
	#endregion
	
	#region AreaListNode элемент списка областей видимости
	public class AreaListNode
	{
        public override string ToString() => InfoList.JoinIntoString();
        public int Area;
		public List<SymbolInfoUnit> InfoList;//для перегрузки
		public AreaListNode()
		{
            InfoList = new List<SymbolInfoUnit>(SymbolTableConstants.InfoList_StartSize);
		}
		public AreaListNode(int ar,SymbolInfoUnit inf)
		{
            InfoList = new List<SymbolInfoUnit>(SymbolTableConstants.InfoList_StartSize);
			Area=ar;
			InfoList.Add(inf);
		}
    }
	#endregion
	
	#region HashTableNode элемент хеш-таблицы
	public class HashTableNode 
	{
		public string Name;
		public AreaNodesList NumAreaList;
		public HashTableNode(string name)
		{
			Name=name;
			NumAreaList=new AreaNodesList(SymbolTableConstants.AreaList_StartSize);
		}
        public override string ToString()
        {
            return NumAreaList.ToString();
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
		public DSHashTable HashTable;
		internal bool CaseSensitive;
		private Scope LastScope;
		//private SymbolInfo FirstInfo;
        private Scope CurrentScope;

        public override string ToString()
        {

            var sb = new System.Text.StringBuilder();
            var a = ScopeTable.SkipWhile(s => !(s.GetType() == typeof(UnitInterfaceScope))).Skip(1).SkipWhile(s => !(s.GetType() == typeof(UnitInterfaceScope)));
            var globscopenum = a.First().ScopeNum;
            var d = new Dictionary<Scope,List<Tuple<string,SymbolInfoUnit>>>();
            foreach (var x in a)
            {
                sb.Append(x.ToString()+"\n");
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
                                d[z.scope] = new List<Tuple<string, SymbolInfoUnit>>();
                            d[z.scope].Add(new Tuple<string, SymbolInfoUnit>(x.Key,z));
                        }
                    }
                }
            }
            sb.Append("\n");
            foreach (var x in d.OrderBy(x=>x.Key.ScopeNum))
            {
                sb.Append(x.Key+"\n");
                foreach (var y in x.Value)
                    sb.Append("  " + y.Item1 + ": " + y.Item2.sym_info.ToString()+ "\n");
            }
            return sb.ToString();
        }

        #region DSSymbolTable(int hash_size,bool case_sensitive)
        public DSSymbolTable(int hash_size,bool case_sensitive)
		{
			CaseSensitive=case_sensitive;
			HashTable=new DSHashTable(hash_size);
			Clear();
		}
		#endregion

		#region Clear() очистка таблицы
		public void Clear()
		{
			ScopeTable=new List<Scope>();
			HashTable.ClearTable();
		}
		#endregion
			
		#region CreateScope для различных Scope
		public LambdaScope CreateLambdaScope(Scope TopScope) //lroman//
        {
            return new LambdaScope(this, TopScope);
        }
		public Scope CreateScope(Scope TopScope)
		{
			return new Scope(this,TopScope);
		}
		public ClassScope CreateClassScope(Scope TopScope,Scope BaseClass)
		{
            return new ClassScope(this, TopScope, BaseClass);	
		}
        //ssyy
        public InterfaceScope CreateInterfaceScope(Scope TopScope, Scope[] TopInterfaces)
        {
            return new InterfaceScope(this, TopScope, TopInterfaces);
        }
        public InterfaceScope CreateInterfaceScope(Scope TopScope, Scope BaseClass, Scope[] TopInterfaces)
        {
            return new InterfaceScope(this, TopScope, BaseClass, TopInterfaces);
        }
        public ClassScope CreateInterfaceOrClassScope(bool is_interface)
        {
            if (is_interface)
            {
                return CreateInterfaceScope(null, null);
            }
            else
            {
                return CreateClassScope(null, null);
            }
        }
        //\ssyy
        public UnitInterfaceScope CreateUnitInterfaceScope(Scope[] UsedUnits)
		{
			return new UnitInterfaceScope(this,null,UsedUnits);
		}
        public NamespaceScope CreateNamespaceScope(Scope[] UsedUnits, Scope TopScope)
        {
            return new NamespaceScope(this, TopScope, UsedUnits);
        }
        public UnitImplementationScope CreateUnitImplementationScope(Scope InterfaceScope,Scope[] UsedUnits)
		{
			return new UnitImplementationScope(this,InterfaceScope,UsedUnits);
		}
		public ClassMethodScope CreateClassMethodScope(Scope TopScope,Scope MyClass)
		{
			return new ClassMethodScope(this,TopScope,MyClass);
		}
		#endregion

		//возвращает номер верхней области видимости относительно
		//области Scope
		public Scope GetTopScope(Scope scope)
		{
			//DEBUG
#if (DEBUG)
			if ((scope.TopScopeNum>=ScopeTable.Count)|(scope.TopScopeNum<0)) throw new Exception("Ошибка при взятии верхней области видимости: область с номером "+scope.ScopeNum+" не существует");
#endif
            return ScopeTable[scope.TopScopeNum];
		}
		public int GetTopScopeNum(int scope)
		{
			//DEBUG
#if (DEBUG)
			if ((scope>=ScopeTable.Count)|(scope<0)) throw new Exception("Ошибка при взятии верхней области видимости: область с номером "+scope+" не существует");
#endif
            return ScopeTable[scope].TopScopeNum;
		}

		//Возвращает количество уровней на которые надо поднятся начиная с Down чтобы очутиться в Up
		//Работает только для процедур. Модуль считает за одно Scope
		public int GetRelativeScopeDepth(Scope Up,Scope Down)
		{
			if (Up==Down) return 0;
			int depth=0;
			while(Down.TopScopeNum>=0)
			{
				if (Up==Down) return depth;
				if(!(Down is UnitImplementationScope))
					depth++;
				Down=Down.TopScope;
			}
			//throw new Exception("Can not execute st depth");
			return -1;
		}

		//Добавление символа
		//если такой символ в пр-ве имен уже существует то symbol_info добавляется к AreaListNode[].InfoList[]
        public void Add(Scope InScope, string Name, SymbolInfoUnit Inf)
        {
            //int.try
            //{
            Inf.scope = InScope;
            if (!InScope.CaseSensitive) Name = Name.ToLower();
            var hn = HashTable.Add(Name);//ЗДЕСь ВОЗНИКАЕТ НЕДЕТЕРМЕНИРОВАНЯ ОШИБКА - SSM 07.10.17 - странный комментарий. Вроде всё нормально.
            // SSM 07.10.17 - переделал внутреннее представление HashTable на основе Dictionary

            hn.NumAreaList.Add(new AreaListNode(InScope.ScopeNum, Inf));
            //}
            // catch (Exception e)
            //{
            //  throw e;
            // }
        }




        //Этот метод ищет ТОЛЬКО В УКАЗАННОЙ ОВ, и не смотрит есть ли имя выше.
        //Если это ОВ типа UnitImplementationScope то имя ищется также и
        //в верней ОВ, которая типа UnitInterfaceScope
        public SymbolInfoList FindOnlyInScope(Scope scope, string Name, bool FindInUpperBlocks)
        {
            if (!scope.CaseSensitive) Name = Name.ToLower();
            CurrentScope = null;
            LastScope = null;

            SymbolInfoList FirstInfo = new SymbolInfoList();

            int Area = scope.ScopeNum;
            var tn = HashTable.Find(Name);		//найдем имя в хеше
            if (tn == null || scope is DotNETScope)//если нет такого ищем в областях .NET
            {
                Scope an;
                an = ScopeTable[Area];
                if (an is DotNETScope)
                {
                    AddToSymbolInfo(FirstInfo, (DotNETScope)an, Name);
                }
                return FirstInfo.InfoUnitList.Count > 0 ? FirstInfo : null;
            }

            AreaNodesList AreaList = tn.NumAreaList;
            int CurrentArea = Area, ai, bs;
            do
            {
                if (ScopeTable[CurrentArea] is UnitPartScope) //мы очутились в модуле
                {

                    //мы в ImplementationPart?
                    if (ScopeTable[CurrentArea] is UnitImplementationScope)
                    {
                        ai = AreaList.IndexOf(CurrentArea);
                        if (ai >= 0) //что-то нашли!
                        {
                            AddToSymbolInfo(AreaList[ai].InfoList, ScopeTable[CurrentArea], FirstInfo);
                        }
                        CurrentArea = GetTopScopeNum(CurrentArea);
                    }
                    //сейча мы в InterfacePart
                    ai = AreaList.IndexOf(CurrentArea);
                    if (ai >= 0) //что-то нашли!
                    {
                        AddToSymbolInfo(AreaList[ai].InfoList, ScopeTable[CurrentArea], FirstInfo);
                    }
                    if (FirstInfo.InfoUnitList.Count > 0)
                        return FirstInfo;
                }
                if (ScopeTable[CurrentArea] is WithScope)//мы очутились в Width
                {
                    ai = AreaList.IndexOf(CurrentArea);
                    if (ai >= 0) //что-то нашли!
                    {
                        AddToSymbolInfo(AreaList[ai].InfoList, ScopeTable[CurrentArea], FirstInfo);
                    }
                    if (FirstInfo.InfoUnitList.Count > 0) //если что-то нашли то заканчиваем
                        return FirstInfo;

                    FindAllInAreaList(Name, (ScopeTable[CurrentArea] as WithScope).WithScopes, AreaList, true, FirstInfo);
                    if (FirstInfo.InfoUnitList.Count > 0) //если что-то нашли то заканчиваем
                        return FirstInfo;
                }
                else
                {
                    ai = AreaList.IndexOf(CurrentArea);
                    if (ai >= 0) //что-то нашли!
                    {
                        AddToSymbolInfo(AreaList[ai].InfoList, ScopeTable[CurrentArea], FirstInfo);
                        return FirstInfo.InfoUnitList.Count > 0 ? FirstInfo : null;
                    }
                }
                bs = CurrentArea;
                CurrentArea = GetTopScopeNum(CurrentArea);
            } while (CurrentArea >= 0 && (FindInUpperBlocks && ScopeTable[bs] is BlockScope));
            return null;
        }
        private void FindAllInClass(string name, int ClassArea, AreaNodesList AreaList, bool OnlyInThisClass, SymbolInfoList FirstInfo)
        {
            int ai;
            Scope ar = ScopeTable[ClassArea];

            if ((ai = AreaList.IndexOf(ClassArea)) >= 0)
                AddToSymbolInfo(AreaList[ai].InfoList, ar, FirstInfo);

            if (ar is DotNETScope)
            {
                PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(FirstInfo, name);
                AddToSymbolInfo(FirstInfo, (DotNETScope)ar, name);
                return;
            }

            ClassScope cl = (ClassScope)ScopeTable[ClassArea];

            if (!OnlyInThisClass)
                while (cl.BaseClassScopeNum >= 0)
                {
                    ai = AreaList.IndexOf(cl.BaseClassScopeNum);
                    if (ai >= 0)
                        AddToSymbolInfo(AreaList[ai].InfoList, cl.BaseClassScope, FirstInfo);
                    //cl=(ClassScope)ScopeTable[cl.BaseClassScopeNum];

                    ar = ScopeTable[cl.BaseClassScopeNum];
                    if (ar is DotNETScope)
                    {
                        AddToSymbolInfo(FirstInfo, (DotNETScope)ar, name);
                        return;
                    }
                    cl = (ClassScope)ScopeTable[cl.BaseClassScopeNum];
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
            return (Scope1 != null) && (Scope2 != null) && (Scope1.ScopeNum == Scope2.ScopeNum);
        }
        
        private bool IsInOneOrDerivedClass(Scope IdentScope, Scope FromScope)
        {
            IdentScope = FindClassScope(IdentScope);
            FromScope = FindClassScope(FromScope);
            while (FromScope != null)
            {
                if (IdentScope.ScopeNum == FromScope.ScopeNum)
                    return true;
                if (FromScope is ClassScope)
                    FromScope = ((ClassScope)FromScope).BaseClassScope;
                else
                    FromScope = ((PascalABCCompiler.NetHelper.NetTypeScope)FromScope).TopScope;
            }
            return false;
            
        }
        
        private bool IsVisible(SymbolInfoUnit ident, Scope fromScope)
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

        private bool IsNormal(SymbolInfoUnit to, SymbolInfoUnit add)
        {
            return //true;
                (to == null) || (to.scope == null) || ((to.scope != null) && //to.scope == null не нужно?
                (((to.symbol_kind == symbol_kind.sk_none) && (add.symbol_kind == symbol_kind.sk_none)) && (to.scope == add.scope))
                ||
                ((to.symbol_kind == symbol_kind.sk_overload_function) && (add.symbol_kind == symbol_kind.sk_overload_function))
                ||
                ((to.symbol_kind == symbol_kind.sk_overload_procedure) && (add.symbol_kind == symbol_kind.sk_overload_procedure))
                );
        }

        private bool AlreadyAdded(SymbolInfoUnit si, SymbolInfoList FirstInfo)
        {
            if (FirstInfo.First() == null || si == null)
                return false;
            for (int i = 1; i < FirstInfo.InfoUnitList.Count; ++i)
                if (FirstInfo.InfoUnitList[i] == si)
                    return true;

            return false;
        }

        private SymbolInfoList AddToSymbolInfo(List<SymbolInfoUnit> from, Scope scope, SymbolInfoList FirstInfo)
        {
            bool CheckVisible = CurrentScope != null, NeedAdd = false;
            SymbolInfoUnit to = FirstInfo.Last();

            foreach (SymbolInfoUnit si_unit in from)
            {
                if (CheckVisible)
                    NeedAdd = IsVisible(si_unit, CurrentScope) && IsNormal(to, si_unit);
                else
                    NeedAdd = IsNormal(to, si_unit);
                if (NeedAdd && !AlreadyAdded(si_unit, FirstInfo))
                {
                    FirstInfo.Add(si_unit);
                    to = si_unit;
                }
            }
            LastScope=scope;
            return to==null?null:new SymbolInfoList(to);
        }
        //Не используется ==> Не понятно работает или нет.
        private SymbolInfoList AddToSymbolInfo(SymbolInfoList to, SymbolInfoList si, Scope scope)
        {
            if(si != null)
            {
                if(IsNormal(to.First(), si.First()))
                {
                    SymbolInfoUnit temp = si.First();
                    to.Add(temp); si.InfoUnitList.RemoveRange(1, si.InfoUnitList.Count - 1);
                    LastScope = scope;

                    return si;
                }
            }
            return to;
        }

        private SymbolInfoList AddToSymbolInfo(SymbolInfoList sito, DotNETScope ar, string name)
        {
            SymbolInfoList si = ar.Find(name);
            if (si != null)
                if (IsNormal(sito.Last(), si.First()))
                {
                    sito.Add(si);
                    return si;
                }
            return sito;
        }

        private void FindAllInAreaList(string name, Scope[] arr, AreaNodesList AreaNodes,SymbolInfoList FirstInfo)
        {
            FindAllInAreaList(name, arr, AreaNodes, false, FirstInfo);
        }
        private void FindAllInAreaList(string name, Scope[] arr, AreaNodesList AreaNodes, bool StopIfFind, SymbolInfoList FirstInfo)
        {
            if (arr == null) return;
            int p;
            int add = FirstInfo.InfoUnitList.Count;
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
                    AddToSymbolInfo(FirstInfo, (DotNETScope)sc, name);
                    if (FirstInfo.InfoUnitList.Count > add && StopIfFind)
                        return;
                }
                else
                if (AreaNodes != null && sc != null)
                {
                    p = AreaNodes.IndexOf(sc.ScopeNum);
                    if (p >= 0)
                    {
                        AddToSymbolInfo(AreaNodes[p].InfoList, sc, FirstInfo);
                        if (FirstInfo.InfoUnitList.Count > add && StopIfFind)
                            return;
                    }
                }
            }
        }
        //поиск всех имен в ООВ.
        //  ищет наборы имен в ООВ, если находит то возвращает их список.
        //  иначе ищет в обьемлющем ООВ.
        //SymbolInfo возвращаются в поряде в котором они встретились при проходе областей
        public SymbolInfoList Find(Scope scope, string Name)
        {
            return FindAll(scope, Name, false, false, null);
        }

        public SymbolInfoList Find(Scope scope, string Name, Scope FromScope)
        {
            return FindAll(scope, Name, false, false, FromScope);
        }
        public SymbolInfoList FindOnlyInType(Scope scope, string Name)
        {
            //TODO: Почему ищет везде??? Только в типе и надтипах. А в юните он найдет?
            return FindAll(scope, Name, true, false, null);
        }
        public SymbolInfoList FindOnlyInType(Scope scope, string Name, Scope FromScope)
        {
            return FindAll(scope, Name, true, false, FromScope);
        }

        public SymbolInfoList FindOnlyInThisClass(ClassScope scope, string Name)
        {
            return FindAll(scope, Name, true, true, null);
        }
        private SymbolInfoList FindAll(Scope scope, string Name, bool OnlyInType, bool OnlyInThisClass, Scope FromScope)
        {
            if (OnlyInType && !(scope is ClassScope) && !(scope is SymbolTable.DotNETScope)) return null;
            //if (!CaseSensitive) Name=Name.ToLower();
            if (!scope.CaseSensitive)
                Name = Name.ToLower();
            CurrentScope = FromScope; //глобальные переменные могут привести к ошибкам при поиске и поторном вызове!
            LastScope = null;         //глобальные переменные могут привести к ошибкам при поиске и поторном вызове!

            SymbolInfoList FirstInfo = new SymbolInfoList(new SymbolInfoUnit());

            int Area = scope.ScopeNum;
            Scope[] used_units = null;
            var tn = HashTable.Find(Name);		//найдем имя в хеше

            // SSM 21.01.16
            if (Name.StartsWith("?"))     // это значит, надо искать в областях .NET
                Name = Name.Substring(1); // съели ? и ищем т.к. tn<0
            // end SSM 

            if (tn == null || scope is DotNETScope)//если нет такого ищем в областях .NET
            {
                //ssyy
                int NextUnitArea = -2;
                //\ssyy
                Scope an;
                while (Area >= 0)
                {
                    an = ScopeTable[Area];
                    if (an is DotNETScope)
                    {
                        if (tn == null)
                        {
                            AddToSymbolInfo(FirstInfo, (DotNETScope)an, Name);
                        }
                        else
                        {
                            FindAllInClass(Name, Area, tn.NumAreaList, false, FirstInfo);
                        }
                    }
                    if (FirstInfo.InfoUnitList.Count > 1)
                    {
                        FirstInfo.InfoUnitList.RemoveAt(0);
                        return FirstInfo;
                    }
                    if (an is UnitPartScope)
                    {
                        if (an is UnitImplementationScope)
                        {
                            FindAllInAreaList(Name, (an as UnitImplementationScope).TopScopeArray, null, FirstInfo);
                            an = ScopeTable[an.TopScopeNum];
                        }

                        FindAllInAreaList(Name, (an as UnitInterfaceScope).TopScopeArray, null, FirstInfo);

                        if (FirstInfo.InfoUnitList.Count > 1)
                        {
                            FirstInfo.InfoUnitList.RemoveAt(0);
                            return FirstInfo;
                        }
                    }
                    if (an is WithScope)//мы очутились в Width
                    {
                        FindAllInAreaList(Name, (an as WithScope).WithScopes, null, true, FirstInfo);

                        if (FirstInfo.InfoUnitList.Count > 1) //если что-то нашли то заканчиваем
                        {
                            FirstInfo.InfoUnitList.RemoveAt(0);
                            return FirstInfo;
                        }
                    }
                    if (an is ClassScope)
                    {
                        int unit_area = an.TopScopeNum;
                        InterfaceScope IntScope = an as InterfaceScope;
                        while (((ClassScope)an).BaseClassScopeNum >= 0)
                        {
                            an = ScopeTable[((ClassScope)an).BaseClassScopeNum];
                            if (an is DotNETScope)
                            {
                                AddToSymbolInfo(FirstInfo, (DotNETScope)an, Name);
                                if (FirstInfo.InfoUnitList.Count > 1) // || OnlyInType) 
                                {
                                    FirstInfo.InfoUnitList.RemoveAt(0);
                                    return FirstInfo;
                                }
                                break;
                            }
                        }
                        //В предках ничего не нашли, ищем по интерфейсам...
                        if (IntScope != null)
                        {
                            FindAllInAreaList(Name, IntScope.TopInterfaceScopeArray, null, FirstInfo);
                            if (FirstInfo.InfoUnitList.Count > 1) //если что-то нашли то заканчиваем
                            {
                                FirstInfo.InfoUnitList.RemoveAt(0);
                                return FirstInfo;
                            }
                        }
                        if (OnlyInType)
                        {
                            FirstInfo.InfoUnitList.RemoveAt(0);
                            return FirstInfo.InfoUnitList.Count > 0 ? FirstInfo : null;
                        }
                        //ssyy
                        if (NextUnitArea > -1)
                        {
                            Area = NextUnitArea;
                            //NextUnitArea = -2;
                            continue;
                        }
                        else
                            //\ssyy
                            an = ScopeTable[unit_area];
                    }
                    if (FirstInfo.InfoUnitList.Count > 1)
                    {
                        FirstInfo.InfoUnitList.RemoveAt(0);
                        return FirstInfo;
                    }

                    if (an is ClassMethodScope)
                    {
                        //ssyy
                        NextUnitArea = an.TopScopeNum;
                        //\ssyy
                        Area = (an as ClassMethodScope).MyClassNum;
                    }
                    else
                    {
                        Area = GetTopScopeNum(Area);
                    }

                    //Area=GetTopScopeNum(Area);
                }
                return null;                //если такого нет то поиск окончен
            }

            AreaNodesList AreaList = tn.NumAreaList;
            int CurrentArea = Area, ai;
            while (CurrentArea >= 0)
            {
                if (ScopeTable[CurrentArea] is UnitPartScope) //мы очутились в модуле
                {

                    //мы в ImplementationPart?
                    if (ScopeTable[CurrentArea] is UnitImplementationScope)
                    {
                        used_units = (ScopeTable[CurrentArea] as UnitImplementationScope).TopScopeArray;
                        ai = AreaList.IndexOf(CurrentArea);

                        if (ai >= 0) //что-то нашли!
                        {
                            AddToSymbolInfo(AreaList[ai].InfoList, ScopeTable[CurrentArea], FirstInfo);
                        }
                        CurrentArea = GetTopScopeNum(CurrentArea);
                    }
                    //сейча мы в InterfacePart
                    ai = AreaList.IndexOf(CurrentArea);
                    if (ai >= 0) //что-то нашли!
                    {
                        AddToSymbolInfo(AreaList[ai].InfoList, ScopeTable[CurrentArea], FirstInfo);
                    }
                    //смотрим в модулях
                    FindAllInAreaList(Name, used_units, AreaList, FirstInfo);
                    FindAllInAreaList(Name, (ScopeTable[CurrentArea] as UnitInterfaceScope).TopScopeArray, AreaList, FirstInfo);
                    FirstInfo.InfoUnitList.RemoveAt(0);
                    return FirstInfo.InfoUnitList.Count > 0 ? FirstInfo : null;
                }
                else
                if (ScopeTable[CurrentArea] is IInterfaceScope)
                {
                    FindAllInClass(Name, CurrentArea, AreaList, OnlyInThisClass, FirstInfo);

                    if (FirstInfo.InfoUnitList.Count > 1) //если что-то нашли то заканчиваем
                    {
                        FirstInfo.InfoUnitList.RemoveAt(0);
                        return FirstInfo;
                    }
                    //Зачем искать в интерфейсах?
                    //(ssyy) Не понимаю вопрос. Спросившему подумать, зачем в компиляторе нужен поиск.
                    FindAllInAreaList(Name, (ScopeTable[CurrentArea] as IInterfaceScope).TopInterfaceScopeArray, AreaList, FirstInfo);

                    if (FirstInfo.InfoUnitList.Count > 1 || OnlyInType) //если что-то нашли то заканчиваем
                    {
                        FirstInfo.InfoUnitList.RemoveAt(0);
                        return FirstInfo.InfoUnitList.Count > 0 ? FirstInfo : null;
                    }
                }
                else
                if (ScopeTable[CurrentArea] is ClassScope)//мы очутились в классе
                {
                    FindAllInClass(Name, CurrentArea, AreaList, OnlyInThisClass, FirstInfo);//надо сделать поиск по его предкам

                    if (FirstInfo.InfoUnitList.Count > 1 || OnlyInType) //если что-то нашли то заканчиваем
                    {
                        FirstInfo.InfoUnitList.RemoveAt(0);
                        return FirstInfo.InfoUnitList.Count > 0 ? FirstInfo : null;
                    }
                    //иначе ищем дальше
                }
                else
                if (ScopeTable[CurrentArea] is WithScope)//мы очутились в With
                {
                    ai = AreaList.IndexOf(CurrentArea);
                    if (ai >= 0) //что-то нашли!
                    {
                        AddToSymbolInfo(AreaList[ai].InfoList, ScopeTable[CurrentArea], FirstInfo);
                    }
                    if (FirstInfo.InfoUnitList.Count > 1) //если что-то нашли то заканчиваем
                    {
                        FirstInfo.InfoUnitList.RemoveAt(0);
                        return FirstInfo;
                    }
                    Scope[] wscopes = (ScopeTable[CurrentArea] as WithScope).WithScopes;
                    if (wscopes != null)
                        foreach (Scope wsc in wscopes)
                        {
                            FindAllInClass(Name, wsc.ScopeNum, AreaList, OnlyInThisClass, FirstInfo);//надо сделать поиск по его предкам                    

                            if (FirstInfo.InfoUnitList.Count > 1) //если что-то нашли то заканчиваем
                            {
                                FirstInfo.InfoUnitList.RemoveAt(0);
                                return FirstInfo;
                            }
                        }
                    //info = FindAllInAreaList(info, Name, (ScopeTable[CurrentArea] as WithScope).WithScopes, AreaList, true);
                }
                else
                {
                    ai = AreaList.IndexOf(CurrentArea);
                    if (ai >= 0) //что-то нашли!
                    {
                        AddToSymbolInfo(AreaList[ai].InfoList, ScopeTable[CurrentArea], FirstInfo);
                        FirstInfo.InfoUnitList.RemoveAt(0);
                        return FirstInfo.InfoUnitList.Count > 0 ? FirstInfo : null;
                    }
                    if (ScopeTable[CurrentArea] is ClassMethodScope)//мы очутились в методе класса
                    {
                        FindAllInClass(Name, (ScopeTable[CurrentArea] as ClassMethodScope).MyClassNum, AreaList, OnlyInThisClass, FirstInfo);//надо сделать поиск по его классу

                        if (FirstInfo.InfoUnitList.Count > 1) //если что-то нашли то заканчиваем
                        {
                            FirstInfo.InfoUnitList.RemoveAt(0);
                            return FirstInfo;
                        }
                    }
                }
                CurrentArea = GetTopScopeNum(CurrentArea);//Пошли вверх

            }
            return null;
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
