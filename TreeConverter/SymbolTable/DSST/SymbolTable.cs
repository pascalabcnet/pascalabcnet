// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using PascalABCCompiler.TreeConverter;
using System.Collections.Generic;
using SymbolTable;
using System.Reflection;

namespace PascalABCCompiler.TreeRealization
{
    public abstract class BasePCUReader
    {
        public abstract definition_node CreateInterfaceMember(int offset, string name);
        public static void RestoreSymbolsInterfaceMember(SymbolInfo si, string name)
        {
            while (si != null)
            {
                if (si.sym_info!=null)
                if (si.sym_info.semantic_node_type == PascalABCCompiler.TreeRealization.semantic_node_type.wrap_def)
                {
                    PascalABCCompiler.TreeRealization.wrapped_definition_node wdn = (PascalABCCompiler.TreeRealization.wrapped_definition_node)si.sym_info;
                    si.sym_info = wdn.PCUReader.CreateInterfaceMember(wdn.offset, name);
                }
                si = si.Next;
            }
        }


        public abstract definition_node CreateInterfaceInClassMember(int offset, string name);
        public abstract definition_node CreateImplementationMember(int offset, bool restore_code=true);
        public abstract definition_node CreateTypeSynonim(int offset, string name);
        public static void RestoreSymbols(SymbolInfo si, string name)
        {
            while (si != null)
            {
                if (si.sym_info != null)
                if (si.sym_info.semantic_node_type == PascalABCCompiler.TreeRealization.semantic_node_type.wrap_def)
                {
                    PascalABCCompiler.TreeRealization.wrapped_definition_node wdn = (PascalABCCompiler.TreeRealization.wrapped_definition_node)si.sym_info;
                    RestoreSymbols(si, wdn, name);
                }
                si = si.Next;
            }
        }

       
        static void RestoreSymbols(SymbolInfo si, PascalABCCompiler.TreeRealization.wrapped_definition_node wdn, string name)
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
        public virtual SymbolInfo Find(string name)
        {
            return Find(name, null);
        }
        public virtual SymbolInfo Find(string name, Scope CurrentScope)
		{
            SymbolInfo si = SymbolTable.Find(this, name, CurrentScope);
            if (si == null) return si;
            PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(si, name);
            return si;
		}
		public virtual SymbolInfo FindOnlyInScopeAndBlocks(string name)
		{
			return SymbolTable.FindOnlyInScope(this, name, true);
		}
        public virtual SymbolInfo FindOnlyInScope(string name)
        {
            return SymbolTable.FindOnlyInScope(this, name, false);
        }
        public virtual SymbolInfo FindOnlyInType(string name, Scope CurrentScope)
		{
            SymbolInfo si = SymbolTable.FindOnlyInType(this, name, CurrentScope);
            if (si == null) return si;
            PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(si, name);
            return si;
		}
		public void AddSymbol(string Name,SymbolInfo Inf)
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

        public override SymbolInfo Find(string name, Scope CurrentScope)
        {
            SymbolInfo si = SymbolTable.Find(this, name, CurrentScope);
            if (PartialScope != null)
            {
                if (si == null)
                    si = SymbolTable.Find(PartialScope, name, CurrentScope);
                else
                {
                    SymbolInfo tmp_si = si;
                    while (tmp_si.Next != null)
                        tmp_si = tmp_si.Next;
                    tmp_si.Next = SymbolTable.Find(PartialScope, name, CurrentScope);
                }
            }
            if (si == null) return si;
            PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(si, name);
            return si;
        }

        public override SymbolInfo FindOnlyInType(string name, Scope CurrentScope)
        {
            SymbolInfo si = SymbolTable.FindOnlyInType(this, name, CurrentScope);
            if (PartialScope != null)
            {
                if (si == null)
                    si = SymbolTable.FindOnlyInType(PartialScope, name, CurrentScope);
                else
                {
                    SymbolInfo tmp_si = si;
                    while (tmp_si.Next != null)
                        tmp_si = tmp_si.Next;
                    tmp_si.Next = SymbolTable.FindOnlyInType(PartialScope, name, CurrentScope);
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
		public int Area;
		public List<SymbolInfo> InfoList;//для перегузки
		public AreaListNode()
		{
            InfoList = new List<SymbolInfo>(SymbolTableConstants.InfoList_StartSize);
		}
		public AreaListNode(int ar,SymbolInfo inf)
		{
            InfoList = new List<SymbolInfo>(SymbolTableConstants.InfoList_StartSize);
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
		private DSHashTable HashTable;
		internal bool CaseSensitive;
		private Scope LastScope;
		//private SymbolInfo FirstInfo;
        private Scope CurrentScope;

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

		//Возвращает количество уровней на которые надо поднятся начиная с Down чтобы очюбтиться в Up
		//Работает только для процедур.Модуль считает за одно Scope
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
		public void Add(Scope InScope,string Name,SymbolInfo Inf)
		{
            //int.try
           //{
                Inf.scope = InScope;
                if (!InScope.CaseSensitive) Name = Name.ToLower();
                int hn = HashTable.Add(new HashTableNode(Name));//ЗДЕСь ВОЗНИКАЕТ НЕДЕТЕРМЕНИРОВАНЯ ОШИБКА
                HashTable.hash_arr[hn].NumAreaList.Add(new AreaListNode(InScope.ScopeNum, Inf));
            //}
           // catch (Exception e)
            //{
             //  throw e;
           // }
		}
		

	

		//Этот метод ищет ТОЛЬКО В УКАЗАННОЙ ОВ, и не смотрит есть ли имя выше.
		//Если это ОВ типа UnitImplementationScope то имя ищется также и
		//в верней ОВ, которая типа UnitInterfaceScope
		public SymbolInfo FindOnlyInScope(Scope scope,string Name, bool FindInUpperBlocks)
		{
            if (!scope.CaseSensitive) Name = Name.ToLower();
            CurrentScope = null;
			LastScope=null;
            SymbolInfo FirstInfo = new SymbolInfo();
			SymbolInfo info=FirstInfo;
			int Area=scope.ScopeNum;
			int tn=HashTable.Find(Name);		//найдем имя в хеше
            if (tn < 0 || scope is DotNETScope)//если нет такого ищем в областях .NET
            {
                Scope an;
                an = ScopeTable[Area];
                if (an is DotNETScope)
                    info = AddToSymbolInfo(info, (DotNETScope)an, Name);
                return FirstInfo.Next;
            }
            
            AreaNodesList AreaList=HashTable.hash_arr[tn].NumAreaList;
			int CurrentArea=Area,ai,bs;
            do
            {
                if (ScopeTable[CurrentArea] is UnitPartScope) //мы очутились в модуле
                {

                    //мы в ImplementationPart?
                    if (ScopeTable[CurrentArea] is UnitImplementationScope)
                    {
                        ai = AreaList.IndexOf(CurrentArea);
                        if (ai >= 0) //что-то нашли!
                            info = AddToSymbolInfo(info, AreaList[ai].InfoList, ScopeTable[CurrentArea],FirstInfo);
                        CurrentArea = GetTopScopeNum(CurrentArea);
                    }
                    //сейча мы в InterfacePart
                    ai = AreaList.IndexOf(CurrentArea);
                    if (ai >= 0) //что-то нашли!
                        info = AddToSymbolInfo(info, AreaList[ai].InfoList, ScopeTable[CurrentArea],FirstInfo);
                    if(FirstInfo.Next!=null)
                        return FirstInfo.Next;
                }
                if (ScopeTable[CurrentArea] is WithScope)//мы очутились в Width
                {
                    ai = AreaList.IndexOf(CurrentArea);
                    if (ai >= 0) //что-то нашли!
                        info = AddToSymbolInfo(info, AreaList[ai].InfoList, ScopeTable[CurrentArea],FirstInfo);
                    if (FirstInfo.Next != null) //если что-то нашли то заканчиваем
                        return FirstInfo.Next;
                    info = FindAllInAreaList(info, Name, (ScopeTable[CurrentArea] as WithScope).WithScopes, AreaList, true,FirstInfo);
                    if (FirstInfo.Next != null) //если что-то нашли то заканчиваем
                        return FirstInfo.Next;
                }
                else
                {
                    ai = AreaList.IndexOf(CurrentArea);
                    if (ai >= 0) //что-то нашли!
                    {
                        info = AddToSymbolInfo(info, AreaList[ai].InfoList, ScopeTable[CurrentArea],FirstInfo);
                        return FirstInfo.Next;
                    }
                }
                bs = CurrentArea;
                CurrentArea = GetTopScopeNum(CurrentArea);
            } while (CurrentArea >= 0 && (FindInUpperBlocks && ScopeTable[bs] is BlockScope));
//            } while (CurrentArea >= 0 && ScopeTable[bs] is BlockScope);
            /*if (!CaseSensitive) Name=Name.ToLower();
            int Area=scope.ScopeNum;
            int tn=HashTable.Find(Name);
            if (tn<0) return null;
            int ai=HashTable.hash_arr[tn].NumAreaList.IndexOf(Area);
            CurrentScope = null;

            if (ScopeTable[Area] is UnitImplementationScope) 
            {
                int ai2=HashTable.hash_arr[tn].NumAreaList.IndexOf(ScopeTable[Area].TopScopeNum);
                if ((ai < 0) && (ai2 >= 0))
                //Kolay modified if.
                //return HashTable.hash_arr[tn].NumAreaList[ai2].InfoList[0];
                {
                    SymbolInfo si_init = HashTable.hash_arr[tn].NumAreaList[ai2].InfoList[0];
                    SymbolInfo si_next = si_init;
                    for (int iter = 1; iter < HashTable.hash_arr[tn].NumAreaList[ai2].InfoList.Count; iter++)
                    {
                        si_next.Next = HashTable.hash_arr[tn].NumAreaList[ai2].InfoList[iter];
                        si_next = si_next.Next;
                    }
                    return si_init;
                }
                if ((ai>=0)&&(ai2>=0))
                {
                    SymbolInfo si_int=HashTable.hash_arr[tn].NumAreaList[ai2].InfoList[0];
                    SymbolInfo si_impl=HashTable.hash_arr[tn].NumAreaList[ai].InfoList[0];

                    //Kolay modified. All methods searched.

                    //SymbolInfo si_init_interface = HashTable.hash_arr[tn].NumAreaList[ai].InfoList[0];
                    SymbolInfo si_next = si_int;
                    for (int iter = 1; iter < HashTable.hash_arr[tn].NumAreaList[ai].InfoList.Count; iter++)
                    {
                        si_next.Next = HashTable.hash_arr[tn].NumAreaList[ai].InfoList[iter];
                        si_next = si_next.Next;
                    }

                    si_next.Next=si_impl;
                    si_next = si_next.Next;
                    for (int iter = 1; iter < HashTable.hash_arr[tn].NumAreaList[ai2].InfoList.Count; iter++)
                    {
                        si_next.Next = HashTable.hash_arr[tn].NumAreaList[ai2].InfoList[iter];
                        si_next = si_next.Next;
                    }

                    return si_int;

                    
                    //return it;
                }
            }

            if (ai >= 0)
            //Kolay modifeds this if.
            //return HashTable.hash_arr[tn].NumAreaList[ai].InfoList[0];
            {
                SymbolInfo si_init = HashTable.hash_arr[tn].NumAreaList[ai].InfoList[0];
                SymbolInfo si_next = si_init;
                for (int iter = 1; iter < HashTable.hash_arr[tn].NumAreaList[ai].InfoList.Count;iter++ )
                {
                    si_next.Next = HashTable.hash_arr[tn].NumAreaList[ai].InfoList[iter];
                    si_next = si_next.Next;
                }
                //Проверить не чиго ли это не портит!
                PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbolsImplementationMember(si_init, Name);
                return si_init;
            }
			
			
            * */
            return null;
             
		}



        private SymbolInfo FindAllInClass(SymbolInfo si, string name, int ClassArea, AreaNodesList AreaList, bool OnlyInThisClass, SymbolInfo FirstInfo)
		{
			int ai;
            
            Scope ar = ScopeTable[ClassArea];

            if ((ai = AreaList.IndexOf(ClassArea)) >= 0)
                si = AddToSymbolInfo(si, AreaList[ai].InfoList, ar, FirstInfo);

            if (ar is DotNETScope)
            {
                PascalABCCompiler.TreeRealization.BasePCUReader.RestoreSymbols(si, name);
                return AddToSymbolInfo(si, (DotNETScope)ar, name);
            }

            ClassScope cl = (ClassScope)ScopeTable[ClassArea];

            if(!OnlyInThisClass)
                while (cl.BaseClassScopeNum >= 0)
                {
                    ai = AreaList.IndexOf(cl.BaseClassScopeNum);
                    if (ai >= 0) si = AddToSymbolInfo(si, AreaList[ai].InfoList, cl.BaseClassScope, FirstInfo);

                    //cl=(ClassScope)ScopeTable[cl.BaseClassScopeNum];

                    ar = ScopeTable[cl.BaseClassScopeNum];
                    if (ar is DotNETScope)
                        return AddToSymbolInfo(si, (DotNETScope)ar, name);
                    cl = (ClassScope)ScopeTable[cl.BaseClassScopeNum];
                }
			return si;
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
        
		private bool IsNormal(SymbolInfo to,SymbolInfo add)
		{
			return //true;
                (to.scope == null) || ((to.scope != null) &&
                (((to.symbol_kind==symbol_kind.sk_none)&&(add.symbol_kind==symbol_kind.sk_none))&&(to.scope==add.scope))
                ||
				((to.symbol_kind==symbol_kind.sk_overload_function)&&(add.symbol_kind==symbol_kind.sk_overload_function))
				);
		}
		
		private bool AlreadyAdded(SymbolInfo si, SymbolInfo FirstInfo)
		{
			if (FirstInfo == null)
				return false;
			while (FirstInfo.Next != null)
			{
				if (FirstInfo.Next == si)
					return true;
				FirstInfo = FirstInfo.Next;
			}
			return false;
		}
		
		private SymbolInfo AddToSymbolInfo(SymbolInfo to, List<SymbolInfo> from, Scope scope, SymbolInfo FirstInfo)
		{
            bool CheckVisible = CurrentScope != null, NeedAdd = false;
            foreach (SymbolInfo si in from)
            {
                if (CheckVisible)
                    NeedAdd = IsVisible(si, CurrentScope) && IsNormal(to, si);
                else
                    NeedAdd = IsNormal(to, si);
                if (NeedAdd && !AlreadyAdded(si,FirstInfo))
                {
                    to.Next = si;
                    si.Next = null;
                    to = si;
                }
            }
			LastScope=scope;
			return to;
		}
		private SymbolInfo AddToSymbolInfo(SymbolInfo to,SymbolInfo si,Scope scope)
		{
			if(si!=null)
				if(IsNormal(to,si))	
				{
					to.Next=si;si.Next=null;
					LastScope=scope;
					return si;
				}
			LastScope=scope;
			return to;	
		}
		private SymbolInfo AddToSymbolInfo(SymbolInfo sito,DotNETScope ar,string name)
		{
            SymbolInfo si=ar.Find(name);
			if(si!=null)
                if (IsNormal(sito, si))
                {
                    sito.Next = si;
                    return si;
                }
       
			return sito;	
		}
        private SymbolInfo FindAllInAreaList(SymbolInfo si, string name, Scope[] arr, AreaNodesList AreaNodes,SymbolInfo FirstInfo)
        {
            return FindAllInAreaList(si, name, arr, AreaNodes, false, FirstInfo);
        }
		private SymbolInfo FindAllInAreaList(SymbolInfo si,string name,Scope[] arr,AreaNodesList AreaNodes, bool StopIfFind, SymbolInfo FirstInfo)
		{
			if (arr==null) return si;
			int p;
            SymbolInfo sib=si;
            HashSet<Assembly> assm_cache = new HashSet<Assembly>();
			foreach(Scope sc in arr)
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
                    si = AddToSymbolInfo(si, (DotNETScope)sc, name);
                    if (sib.Next != null && StopIfFind)
                        return si;
                }
                else
                if (AreaNodes != null && sc != null)
                {
                    p = AreaNodes.IndexOf(sc.ScopeNum);
                    if (p >= 0)
                    {
                        si = AddToSymbolInfo(si, AreaNodes[p].InfoList, sc, FirstInfo);
                        if (sib.Next != null && StopIfFind)
                            return si;
                    }
                }
			}
			return si;
		}
		//поиск всех имен в ООВ.
		//  ищет наборы имен в ООВ, если находит то возвращает их список.
		//  иначе ищет в обьемлющем ООВ.
		//SymbolInfo возвращаются в поряде в котором они встретились при проходе областей
		public SymbolInfo Find(Scope scope,string Name)
		{
            return FindAll(scope, Name, false, false,  null);
		}
        public SymbolInfo Find(Scope scope, string Name, Scope FromScope)
        {
            return FindAll(scope, Name, false, false, FromScope);
        }
        public SymbolInfo FindOnlyInType(Scope scope, string Name)
		{
            //TODO: Почему ищет везде??? Только в типе и надтипах. А в юните он найдет?
            return FindAll(scope, Name, true, false, null);
		}
        public SymbolInfo FindOnlyInType(Scope scope, string Name, Scope FromScope)
        {
            return FindAll(scope, Name, true, false,FromScope);
        }
        public SymbolInfo FindOnlyInThisClass(ClassScope scope, string Name)
        {
            return FindAll(scope, Name, true, true, null);
        }
        
		private SymbolInfo FindAll(Scope scope,string Name,bool OnlyInType,bool OnlyInThisClass, Scope FromScope)
		{
			if(OnlyInType && !(scope is ClassScope) && !(scope is SymbolTable.DotNETScope)) return null;
			//if (!CaseSensitive) Name=Name.ToLower();
            if(!scope.CaseSensitive)
                Name = Name.ToLower();
            CurrentScope = FromScope; //глобальные переменные могут привести к ошибкам при поиске и поторном вызове!
            LastScope = null;         //глобальные переменные могут привести к ошибкам при поиске и поторном вызове!
            SymbolInfo FirstInfo = new SymbolInfo();
			SymbolInfo info=FirstInfo;
			int Area=scope.ScopeNum;
			Scope[] used_units=null;
			int tn=HashTable.Find(Name);		//найдем имя в хеше

            // SSM 21.01.16
            if (Name.StartsWith("?"))     // это значит, надо искать в областях .NET
                Name = Name.Substring(1); // съели ? и ищем т.к. tn<0
            // end SSM 

            if (tn<0 || scope is DotNETScope)//если нет такого ищем в областях .NET
			{
                //ssyy
                int NextUnitArea = -2;
                //\ssyy
				Scope an;
				while(Area>=0)
				{
					an=ScopeTable[Area];
                    if (an is DotNETScope)
                    {
                        if (tn < 0)
                            info = AddToSymbolInfo(info, (DotNETScope)an, Name);
                        else
                            info = FindAllInClass(info, Name, Area, HashTable.hash_arr[tn].NumAreaList, false, FirstInfo);
                    }
                    if (FirstInfo.Next != null) 
                        return FirstInfo.Next;
					if (an is UnitPartScope)
					{
						if (an is UnitImplementationScope)
						{
							info=FindAllInAreaList(info,Name,(an as UnitImplementationScope).TopScopeArray,null,FirstInfo);
							an=ScopeTable[an.TopScopeNum];
						}
						info=FindAllInAreaList(info,Name,(an as UnitInterfaceScope).TopScopeArray,null,FirstInfo);
						if (FirstInfo.Next!=null) 
                            return FirstInfo.Next;
					}
                    if (an is WithScope)//мы очутились в Width
                    {
                        info = FindAllInAreaList(info, Name, (an as WithScope).WithScopes, null, true,FirstInfo);
                        if (FirstInfo.Next != null) //если что-то нашли то заканчиваем
                            return FirstInfo.Next;
                    }
					if (an is ClassScope)
					{
						int unit_area=an.TopScopeNum;
                        InterfaceScope IntScope = an as InterfaceScope;
                        while (((ClassScope)an).BaseClassScopeNum >= 0)
						{
							an=ScopeTable[((ClassScope)an).BaseClassScopeNum];
							if(an is DotNETScope)
							{
								info=AddToSymbolInfo(info,(DotNETScope)an,Name);
                                if (FirstInfo.Next != null) // || OnlyInType) 
                                    return FirstInfo.Next;
								break;
							}
						}
                        //В предках ничего не нашли, ищем по интерфейсам...
                        if (IntScope != null)
                        {
                            info = FindAllInAreaList(info, Name, IntScope.TopInterfaceScopeArray, null,FirstInfo);
                            if (FirstInfo.Next != null) //если что-то нашли то заканчиваем
                                return FirstInfo.Next;
                        }
                        if (OnlyInType)
                            return FirstInfo.Next;
                        //ssyy
                        if (NextUnitArea > -1)
                        {
                            Area = NextUnitArea;
                            //NextUnitArea = -2;
                            continue;
                        }
                        else
                        //\ssyy
						    an=ScopeTable[unit_area];
					}
					if (info.Next!=null) return FirstInfo.Next;
	
					if (an is ClassMethodScope)
					{
                        //ssyy
                        NextUnitArea = an.TopScopeNum;
                        //\ssyy
						Area=(an as ClassMethodScope).MyClassNum;
					}
					else
					{
						Area=GetTopScopeNum(Area);
					}

					//Area=GetTopScopeNum(Area);
				}
				return null;				//если такого нет то поиск окончен
			} 
						
			AreaNodesList AreaList=HashTable.hash_arr[tn].NumAreaList;
			int CurrentArea=Area,ai;
			while (CurrentArea>=0)
			{
				if (ScopeTable[CurrentArea] is UnitPartScope) //мы очутились в модуле
				{
					
					//мы в ImplementationPart?
					if (ScopeTable[CurrentArea] is UnitImplementationScope)
					{
						used_units=(ScopeTable[CurrentArea] as UnitImplementationScope).TopScopeArray;
						ai=AreaList.IndexOf(CurrentArea);
						if (ai>=0) //что-то нашли!
							info=AddToSymbolInfo(info,AreaList[ai].InfoList,ScopeTable[CurrentArea],FirstInfo);
						CurrentArea=GetTopScopeNum(CurrentArea);
					}
					//сейча мы в InterfacePart
					ai=AreaList.IndexOf(CurrentArea);
					if (ai>=0) //что-то нашли!
						info=AddToSymbolInfo(info,AreaList[ai].InfoList,ScopeTable[CurrentArea],FirstInfo);
					//смотрим в модулях
					info=FindAllInAreaList(info,Name,used_units,AreaList,FirstInfo);
					info=FindAllInAreaList(info,Name,(ScopeTable[CurrentArea] as UnitInterfaceScope).TopScopeArray,AreaList,FirstInfo);
					return FirstInfo.Next;
				}
				else
                if (ScopeTable[CurrentArea] is IInterfaceScope)
                {
                    info = FindAllInClass(info, Name, CurrentArea, AreaList,OnlyInThisClass,FirstInfo);
                    if (FirstInfo.Next != null) //если что-то нашли то заканчиваем
                       return FirstInfo.Next; 
                    //Зачем искать в интерфейсах?
                    //(ssyy) Не понимаю вопрос. Спросившему подумать, зачем в компиляторе нужен поиск.
                    info = FindAllInAreaList(info, Name, (ScopeTable[CurrentArea] as IInterfaceScope).TopInterfaceScopeArray, AreaList, FirstInfo);
                    if (FirstInfo.Next != null || OnlyInType) //если что-то нашли то заканчиваем
                        return FirstInfo.Next;                    
                }
                else
				if(ScopeTable[CurrentArea] is ClassScope)//мы очутились в классе
				{	
					info=FindAllInClass(info,Name,CurrentArea,AreaList,OnlyInThisClass,FirstInfo);//надо сделать поиск по его предкам
                    if (FirstInfo.Next != null || OnlyInType) //если что-то нашли то заканчиваем
						return FirstInfo.Next; 
					//иначе ищем дальше
				}
                else
                if (ScopeTable[CurrentArea] is WithScope)//мы очутились в With
                {
                    ai = AreaList.IndexOf(CurrentArea);
                    if (ai >= 0) //что-то нашли!
                        info = AddToSymbolInfo(info, AreaList[ai].InfoList, ScopeTable[CurrentArea],FirstInfo);
                    if (FirstInfo.Next != null) //если что-то нашли то заканчиваем
                        return FirstInfo.Next;
                    Scope[] wscopes = (ScopeTable[CurrentArea] as WithScope).WithScopes;
                    if(wscopes!=null)
                        foreach (Scope wsc in wscopes)
                        {
                            info = FindAllInClass(info, Name, wsc.ScopeNum, AreaList, OnlyInThisClass,FirstInfo);//надо сделать поиск по его предкам                    
                            if (FirstInfo.Next != null) //если что-то нашли то заканчиваем
                                return FirstInfo.Next;
                        }
                    //info = FindAllInAreaList(info, Name, (ScopeTable[CurrentArea] as WithScope).WithScopes, AreaList, true);
                }
                else
                {
                    ai = AreaList.IndexOf(CurrentArea);
                    if (ai >= 0) //что-то нашли!
                    {
                        info = AddToSymbolInfo(info, AreaList[ai].InfoList, ScopeTable[CurrentArea],FirstInfo);
                        return FirstInfo.Next;
                    }
                    if (ScopeTable[CurrentArea] is ClassMethodScope)//мы очутились в методе класса
                    {
                        info = FindAllInClass(info, Name, (ScopeTable[CurrentArea] as ClassMethodScope).MyClassNum, AreaList, OnlyInThisClass,FirstInfo);//надо сделать поиск по его классу
                        if (FirstInfo.Next != null) //если что-то нашли то заканчиваем
                            return FirstInfo.Next;
                    }
                }
				CurrentArea=GetTopScopeNum(CurrentArea);//Пошли вверх
				
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
