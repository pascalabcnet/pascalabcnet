// ������ Print
///- ������� ������������������ �� �����, ��������� delim � �������� �����������
function IEnumerable<T>.Print(delim: string := ' '): IEnumerable<T>;

/// ������� ������������������ �� �����, ��������� delim � �������� �����������, � ��������� �� ����� ������
function IEnumerable<T>.Println(delim: string := ' '): IEnumerable<T>;

// ����� ���������� Where
/// ��������� ���������� ������������������ �������� �� ������ ��������� ���������. ���������� ��������������������� �������� �������� ������������������, ��������������� ���������.
function IEnumerable<T>.Where(predicate: T->boolean): IEnumerable<T>;

/// ��������� ���������� ������������������ �������� �� ������ ��������� ��������� � ������ ������� ��������. ���������� ��������������������� �������� �������� ������������������, ��������������� ���������.
function IEnumerable<T>.Where(predicate: (T,integer)->boolean): IEnumerable<T>;

// ����� ������������� Select
/// ���������� ������ ������� ������������������ �� ������ ������� � ������� ������� selector. ���������� ������������������ ���������, ���������� � ���������� �������������.
function IEnumerable<T>.Select<Res>(selector: T->Res): IEnumerable<Res>;

/// ���������� ������ ������� ������������������ �� ������ ������� � ������� ������� selector, ����������� ������ ��������. ���������� ������������������ ���������, ���������� � ���������� �������������.
function IEnumerable<T>.Select<Res>(selector: (T,integer)->Res): IEnumerable<Res>;

// ����� ������������� SelectMany
/// ���������� ������ ������� ������������������ � ����� ������������������ � ���������� �������������� ������������������ � ���� ������������������. ���������� ����������� ������������������.
function IEnumerable<T>.SelectMany<Res>(selector: T->IEnumerable<Res>): IEnumerable<Res>;

/// ���������� ������ ������� ������������������ � ����� ������������������ � ������ ������� �������� � ���������� �������������� ������������������ � ���� ������������������. ���������� ����������� ������������������.
function IEnumerable<T>.SelectMany<Res>(selector: (T,integer)->IEnumerable<Res>): IEnumerable<Res>;

/// ���������� ������ ������� ������������������ � ����� ������������������, ���������� �������������� ������������������ � ���� � �������� ������� ��������� ���������� ��� ������� �������� ���� ������������������. ������ ������� �������� �������� ������������������ ������������ � ������������� �������������� ����� ����� ��������. ���������� ����������� ������������������.
function IEnumerable<T>.SelectMany<Coll,Res>(collSelector: (T,integer)->IEnumerable<Coll>; resultSelector: (T,Coll)->Res): IEnumerable<Res>;

/// ���������� ������ ������� ������������������ � ����� ������������������, ���������� �������������� ������������������ � ���� � �������� ������� ��������� ���������� ��� ������� �������� ���� ������������������. ���������� ����������� ������������������.
function IEnumerable<T>.SelectMany<Coll,Res>(collSelector: T->IEnumerable<Coll>; resultSelector: (T,Coll)->Res): IEnumerable<Res>;

// ������ Take, TakeWhile, Skip, SkipWhile
/// ���������� ������������������ �� count ��������� � ������ ������������������.
function IEnumerable<T>.Take(count: integer): IEnumerable<T>;

/// ���������� ������� ��������� ������������������, ��������������� ���������� �������, �� ������� �� ����������������
function IEnumerable<T>.TakeWhile(predicate: T->boolean): IEnumerable<T>;

/// ���������� ������� ��������� ������������������, ��������������� ���������� �������, �� ������� �� ���������������� (����������� ������ ��������)
function IEnumerable<T>.TakeWhile(predicate: (T,integer)->boolean): IEnumerable<T>;

/// ���������� count ��������� � ������������������ � ���������� ��������� ��������.
function IEnumerable<T>.Skip(count: integer): IEnumerable<T>;

/// ���������� �������� � ������������������, ���� ��� ������������� ��������� �������, � ����� ���������� ���������� ��������.
function IEnumerable<T>.SkipWhile(predicate: T->boolean): IEnumerable<T>;

/// ���������� �������� � ������������������, ���� ��� ������������� ��������� �������, � ����� ���������� ���������� �������� (����������� ������ ��������)
function IEnumerable<T>.SkipWhile(predicate: (T,integer)->boolean): IEnumerable<T>;

// ����� Sorted
/// ���������� ��������������� �� ����������� ������������������
function IEnumerable<T>.Sorted(): IEnumerable<T>;

// ������ OrderBy, OrderByDescending
/// ��������� �������� ������������������ � ������� ����������� ����� � ���������� ���������������� ������������������. keySelector - �������, ������������� ������� �� ����.
function IEnumerable<T>.OrderBy<Key>(keySelector: T->Key): System.Linq.IOrderedEnumerable<T>;

/// ��������� �������� ������������������ � ������� ����������� � �������������� ����������� comparer � ���������� ���������������� ������������������. keySelector - �������, ������������� ������� �� ����.
function IEnumerable<T>.OrderBy<Key>(keySelector: T->Key; comparer: IComparer<Key>): System.Linq.IOrderedEnumerable<T>;

/// ��������� �������� ������������������ � ������� �������� ����� � ���������� ���������������� ������������������. keySelector - �������, ������������� ������� �� ����.
function IEnumerable<T>.OrderByDescending<Key>(keySelector: T->Key): System.Linq.IOrderedEnumerable<T>;

/// ��������� �������� ������������������ � ������� �������� � �������������� ����������� comparer � ���������� ���������������� ������������������. keySelector - �������, ������������� ������� �� ����.
function IEnumerable<T>.OrderByDescending<Key>(keySelector: T->Key; comparer: IComparer<Key>): System.Linq.IOrderedEnumerable<T>;

// ������ ThenBy,ThenByDescending
///- ��������� �������������� ������������ ��������� ������������������ � ������� ����������� ����� � ���������� ���������������� ������������������. keySelector - �������, ������������� ������� �� ����.
function IEnumerable<T>.ThenBy<Key>(keySelector: T->Key): System.Linq.IOrderedEnumerable<T>;

/// ��������� �������������� ������������ ��������� ������������������ � ������� ����������� � �������������� ����������� comparer � ���������� ���������������� ������������������. keySelector - �������, ������������� ������� �� ����.
function IEnumerable<T>.ThenBy<Key>(keySelector: T->Key; comparer: IComparer<Key>): System.Linq.IOrderedEnumerable<T>;

/// ��������� �������������� ������������ ��������� ������������������ � ������� �������� ����� � ���������� ���������������� ������������������. keySelector - �������, ������������� ������� �� ����. 
function IEnumerable<T>.ThenByDescending<Key>(keySelector: T->Key): System.Linq.IOrderedEnumerable<T>;

/// ��������� �������������� ������������ ��������� ������������������ � ������� �������� � �������������� ����������� comparer � ���������� ���������������� ������������������. keySelector - �������, ������������� ������� �� ����. 
function IEnumerable<T>.ThenByDescending<Key>(keySelector: T->Key; comparer: IComparer<Key>): System.Linq.IOrderedEnumerable<T>;

// ����� Concat
/// ��������� ��� ������������������, ��������� ������ � ����� ������ � ��������� �������������� ������������������
function IEnumerable<T>.Concat(second: IEnumerable<T>): IEnumerable<T>;

// ����� Zip
/// ���������� ��� ������������������, ��������� ��������� �������, ����������� �� ������ �������� ������ ������������������ � ������������ ������� �������������� ������������������.
function IEnumerable<T>.Zip<TSecond,Res>(second: IEnumerable<TSecond>; resultSelector: (T,TSecond)->Res): IEnumerable<Res>;

// ����� Distinct
/// ���������� ������������� �������� ������������������.
function IEnumerable<T>.Distinct(): IEnumerable<T>;

/// ���������� ������������� �������� ������������������, ��������� ��� ��������� �������� ���������� comparer.
function IEnumerable<T>.Distinct(comparer: IEqualityComparer<T>): IEnumerable<T>;

// ������ Union,Intersect,Except
/// ������� ����������� ��������, �������������� ����� ��������������������.
function IEnumerable<T>.Union(second: IEnumerable<T>): IEnumerable<T>;

/// ������� ����������� ��������, �������������� ����� ��������������������, ��������� ��������� ����������.
function IEnumerable<T>.Union(second: IEnumerable<T>; comparer: IEqualityComparer<T>): IEnumerable<T>;

/// ������� ����������� ��������, �������������� ����� ��������������������.
function IEnumerable<T>.Intersect(second: IEnumerable<T>): IEnumerable<T>;

/// ������� ����������� ��������, �������������� ����� ��������������������, ��������� ��� ��������� �������� ��������� ����������.
function IEnumerable<T>.Intersect(second: IEnumerable<T>; comparer: IEqualityComparer<T>): IEnumerable<T>;

/// ������� �������� ��������, �������������� ����� ��������������������.
function IEnumerable<T>.Except(second: IEnumerable<T>): IEnumerable<T>;

/// ������� �������� ��������, �������������� ����� ��������������������, ��������� ��� ��������� �������� ��������� ����������.
function IEnumerable<T>.Except(second: IEnumerable<T>; comparer: IEqualityComparer<T>): IEnumerable<T>;

// ����� Reverse
///- ���������� ��������������� ������������������
function IEnumerable<T>.Reverse(): IEnumerable<T>;

// ����� SequenceEqual
///- ����������, ��������� �� ��� ������������������.
function IEnumerable<T>.SequenceEqual(second: IEnumerable<T>): boolean;

/// ����������, ��������� �� ��� ������������������, ��������� ��� ��������� ��������� ��������� ����������.
function IEnumerable<T>.SequenceEqual(second: IEnumerable<T>; comparer: IEqualityComparer<T>): boolean;

// ������ First, FirstOrDefault
///- ���������� ������ ������� ������������������.
function IEnumerable<T>.First(): T;

/// ���������� ������ ������� ������������������, ��������������� ���������� �������.
function IEnumerable<T>.First(predicate: T->boolean): T;

/// ���������� ������ ������� ������������������ ��� �������� �� ���������, ���� ������������������ �� �������� ���������.
function IEnumerable<T>.FirstOrDefault(): T;

/// ���������� ������ ��������������� ������� ������� ������������������ ��� �������� �� ���������, ���� �� ������ ������ �������� �� �������.
function IEnumerable<T>.FirstOrDefault(predicate: T->boolean): T;

// ������ Last, LastOrDefault
///- ���������� ��������� ������� ������������������.
function IEnumerable<T>.Last(): T;

/// ���������� ��������� ������� ������������������, ��������������� ���������� �������.
function IEnumerable<T>.Last(predicate: T->boolean): T;

/// ���������� ��������� ������� ������������������ ��� �������� �� ���������, ���� ������������������ �� �������� ���������.
function IEnumerable<T>.LastOrDefault(): T;

/// ���������� ��������� ������� ������������������, ��������������� ���������� �������, ��� �������� �� ���������, ���� �� ������ ������ �������� �� �������.
function IEnumerable<T>.LastOrDefault(predicate: T->boolean): T;

// ������ Single, SingleOrDefault
///- ���������� ������������ ������� ������������������ � ���������� ����������, ���� ����� ��������� ������������������ ������� �� 1.
function IEnumerable<T>.Single(): T;

/// ���������� ������������ ������� ������������������, ��������������� ��������� �������, � ���������� ����������, ���� ����� ��������� ������ ������.
function IEnumerable<T>.Single(predicate: T->boolean): T;

/// ���������� ������������ ������� ������������������ ��� �������� �� ���������, ���� ������������������ �����; ���� � ������������������ ����� ������ ��������, ������������ ����������.
function IEnumerable<T>.SingleOrDefault(): T;

/// ���������� ������������ ������� ������������������, ��������������� ��������� �������, ��� �������� �� ���������, ���� ������ �������� �� ����������; ���� ������� ������������� ����� ������ ��������, ������������ ����������.
function IEnumerable<T>.SingleOrDefault(predicate: T->boolean): T;

// ����� DefaultIfEmpty
///- ���������� �������� ��������� ������������������ ��� �������������� ���������, ���������� �������� ��������� ���� �� ���������, ���� ������������������ �����.
function IEnumerable<T>.DefaultIfEmpty(): IEnumerable<T>;

/// ���������� �������� ��������� ������������������ ��� �������������� ���������, ���������� ��������� ��������, ���� ������������������ �����.
function IEnumerable<T>.DefaultIfEmpty(defaultValue: T): IEnumerable<T>;

// ������ ElementAt, ElementAtOrDefault
///- ���������� ������� �� ���������� ������� � ������������������.
function IEnumerable<T>.ElementAt(index: integer): T;

/// ���������� ������� �� ���������� ������� � ������������������ ��� �������� �� ���������, ���� ������ ��� ����������� ���������.
function IEnumerable<T>.ElementAtOrDefault(index: integer): T;

// ������ Any, All
///- ���������, �������� �� ������������������ �����-���� ��������.
function IEnumerable<T>.Any(): boolean;

/// ���������, ������������� �� �����-���� ������� ������������������ ��������� �������.
function IEnumerable<T>.Any(predicate: T->boolean): boolean;

/// ���������, ��� �� �������� ������������������ ������������� �������.
function IEnumerable<T>.All(predicate: T->boolean): boolean;

// ������ Count
/// ���������� ���������� ��������� � ������������������.
function IEnumerable<T>.Count(): integer;

///- ���������� �����, �������������� ���������� ��������� ������������������, ��������������� ��������� �������.
function IEnumerable<T>.Count(predicate: T->boolean): integer;

/// ���������� �������� ���� Int64, �������������� ����� ����� ��������� � ������������������.
function IEnumerable<T>.LongCount(): int64;

/// ���������� �������� ���� Int64, �������������� ����� ��������� ������������������, ��������������� ��������� �������.
function IEnumerable<T>.LongCount(predicate: T->boolean): int64;

// ����� Contains
///- ����������, ���������� �� ��������� ������� � ������������������, ��������� ���������� �������� �� ��������� �� ���������.
function IEnumerable<T>.Contains(value: T): boolean;

/// ����������, �������� �� ������������������ �������� �������, ��������� ��������� ����������.
function IEnumerable<T>.Contains(value: T; comparer: IEqualityComparer<T>): boolean;

// ����� Aggregate
///- ��������� � ������������������ ���������� �������. ���������� �������� ���������� ��������.
function IEnumerable<T>.Aggregate(func: (T,T)->T): T;

/// ��������� � ������������������ ���������� �������. ��������� ��������� �������� ������������ � �������� ��������� �������� ���������� ��������. ���������� �������� ���������� ��������.
function IEnumerable<T>.Aggregate<Accum>(seed: T; func: (Accum,T)->Accum): T;

/// ��������� � ������������������ ���������� �������.��������� ��������� �������� ������ �������� ��������� ��� ���������� ��������, � ��������� ������� ������������ ��� ������ ��������������� ��������. ���������� �������� ���������� ��������.
function IEnumerable<T>.Aggregate<Accum,Res>(seed: T; func: (Accum,T)->Accum; resultSelector: Accum->Res): T;

// ������ Sum, Average
/// ��������� ����� ������������������ �������� ��������� ����  
function IEnumerable<�����>.Sum(): �����;

/// ��������� ����� ������������������ �������� ��������� ����, ���������� � ���������� ���������� ������� �������������� � ������� �������� ������� ������������������.
function IEnumerable<T>.Sum(selector: T->�����): �����;

/// ��������� ������� ��� ������������������ �������� ��������� ����  
function IEnumerable<�����>.Average(): real;

/// ��������� ������� ��� ������������������ �������� ��������� ����, ���������� � ���������� ���������� ������� �������������� � ������� �������� ������� ������������������.
function IEnumerable<T>.Average(selector: T->�����): real;

// ������ Min, Max
/// ��������� ����������� ������� ������������������ �������� ��������� ����  
function IEnumerable<�����>.Min(): �����;

/// �������� ������� �������������� ��� ������� �������� ������������������ � ���������� ����������� �������� ��������� ����.
function IEnumerable<T>.Min(selector: T->�����): �����;

/// ��������� ������������ ������� ������������������ �������� ��������� ����  
function IEnumerable<�����>.Max(): �����;

/// �������� ������� �������������� ��� ������� �������� ������������������ � ���������� ������������ �������� ��������� ����.
function IEnumerable<T>.Max(selector: T->�����): �����;

// ����� Join
/// ���������� ��� ������������������ �� ������ ������������� ������ � ������ ������������������. ������� resultSelector ����� �������� ��������� ���� ������������������� � ����������� ���������� ����� � ������� ������� ������������������. 
function IEnumerable<T>.Join<TInner,Key,Res>(inner: IEnumerable<TInner>; outerKeySelector: T->Key; innerKeySelector: TInner->TKey; resultSelector: (T,TInner)->Res): IEnumerable<Res>;

/// ���������� ��� ������������������ �� ������ ������������� ������ � ������ ������������������. ������� resultSelector ����� �������� ��������� ���� ������������������� � ����������� ���������� ����� � ������� ������� ������������������. ��� ��������� ������ ������������ ���������� comparer
function IEnumerable<T>.Join<TInner,Key,Res>(inner: IEnumerable<TInner>; outerKeySelector: T->Key; innerKeySelector: TInner->TKey; resultSelector: (T,TInner)->Res; comparer: System.Collections.Generic.IEqualityComparer<Key>): IEnumerable<Res>;

// ����� GroupJoin
/// ���������� ��� ������������������ �� ������ ��������� ������ � ���������� ����������. ����� ������� resultSelector ����������� ���� � ������������������ ��������������� ��� �������� �� ������� �������������� ������������������. 
function IEnumerable<T>.GroupJoin<TInner,Key,Res>(inner: IEnumerable<TInner>; outerKeySelector: T->Key; innerKeySelector: TInner->TKey; resultSelector: (T,IEnumerable<TInner>)->Res): IEnumerable<Res>;

/// ���������� ��� ������������������ �� ������ ��������� ������ � ���������� ����������. ��� ��������� ������ ������������ ��������� ����������. ����� ������� resultSelector ����������� ���� � ������������������ ��������������� ��� �������� �� ������� �������������� ������������������.
function IEnumerable<T>.GroupJoin<TInner,Key,Res>(inner: IEnumerable<TInner>; outerKeySelector: T->Key; innerKeySelector: TInner->TKey; resultSelector: (T,IEnumerable<TInner>)->Res; comparer: IEqualityComparer<Key>): IEnumerable<Res>;

// ����� GroupBy
/// ���������� �������� ������������������ � ������������ � �������� �������� ��������� ����� � ���������� ������������������ �����; ������ ������ ������������� ������ �������� �����.
function IEnumerable<T>.GroupBy<Key>(keySelector: T->Key): IEnumerable<IGrouping<Key,T>>;

/// ���������� �������� ������������������ � ������������ � �������� �������� ��������� �����, ���������� ����� � ������� ���������� ����������� � ���������� ������������������ �����; ������ ������ ������������� ������ �������� �����.
function IEnumerable<T>.GroupBy<Key>(keySelector: T->Key; comparer: System.Collections.Generic.IEqualityComparer<Key>): IEnumerable<IGrouping<Key,T>>;

/// ���������� �������� ������������������ � ������������ � �������� �������� ��������� ����� � ���������� �������� ������ ������ � ������� ��������� �������. ���������� ������������������ �����; ������ ������ ������������� ������ �������� �����.
function IEnumerable<T>.GroupBy<Key,Element>(keySelector: T->Key; elementSelector: T->Element): IEnumerable<IGrouping<Key,T>>;

/// ���������� �������� ������������������ � ������������ � �������� ��������� �����.����� ������������ � ������� �����������, �������� ������ ������ ������������ � ������� ��������� �������.
function IEnumerable<T>.GroupBy<Key,Element>(keySelector: T->Key; elementSelector: T->Element; comparer: IEqualityComparer<Key>): IEnumerable<IGrouping<Key,Element>>;

/// ���������� �������� ������������������ � ������������ � �������� �������� ��������� ����� � ������� �������������� �������� ��� ������ ������ � �� �����.
function IEnumerable<T>.GroupBy<Key,Res>(keySelector: T->Key; resultSelector: (Key,IEnumerable<T>)->Res): IEnumerable<Res>;

/// ���������� �������� ������������������ � ������������ � �������� �������� ��������� ����� � ������� �������������� �������� ��� ������ ������ � �� �����.�������� ������ ������ ������������ � ������� ��������� �������.
function IEnumerable<T>.GroupBy<Key,Element,Res>(keySelector: T->Key; elementSelector: T->Element; resultSelector: (Key,IEnumerable<Element>)->Res): IEnumerable<Res>;

/// ���������� �������� ������������������ � ������������ � �������� �������� ��������� ����� � ������� �������������� �������� ��� ������ ������ � �� �����.����� ������������ � �������������� ��������� �����������.
function IEnumerable<T>.GroupBy<Key,Res>(keySelector: T->Key; resultSelector: (Key,IEnumerable<T>)->Res; comparer: IEqualityComparer<Key>): IEnumerable<Res>;

/// ���������� �������� ������������������ � ������������ � �������� �������� ��������� ����� � ������� �������������� �������� ��� ������ ������ � �� �����.�������� ������ ������������ � ������� ���������� �����������, �������� ������ ������ ������������ � ������� ��������� �������.
function IEnumerable<T>.GroupBy<Key,Element,Res>(keySelector: T->Key; elementSelector: System.T->Element; resultSelector: (Key,IEnumerable<Element>)->Res; comparer: IEqualityComparer<Key>): IEnumerable<Res>;

// ����� AsEnumerable
/// ���������� ������� ������, ����������� � ���� IEnumerable<T>.
function IEnumerable<T>.AsEnumerable(): IEnumerable<T>;

// ������ ToArray, ToList
/// ������� ������ �� ������������������.
function IEnumerable<T>.ToArray(): array of T;

/// ������� ������ List �� ������������������.
function IEnumerable<T>.ToList(): List<T>;

// ����� ToDictionary
/// ������� ������� Dictionary �� ������������������ ������������ � �������� �������� ��������� �����.
function IEnumerable<T>.ToDictionary<Key>(keySelector: T->Key): Dictionary<Key,T>;

/// ������� ������� Dictionary �� ������������������ � ������������ � �������� �������� ��������� ����� � ������������ ������.
function IEnumerable<T>.ToDictionary<Key>(keySelector: T->Key; comparer: IEqualityComparer<Key>): Dictionary<Key,T>;

/// ������� ������� Dictionary �� ������������������ � ������������ � ��������� ��������� ��������� ����� � ��������� ��������.
function IEnumerable<T>.ToDictionary<Key,Element>(keySelector: T->Key; elementSelector: T->Element): Dictionary<Key,Element>;

/// ������� ������� Dictionary �� ������������������ � ������������ � �������� ������������ � ��������� ��������� ����� � ��������� ��������.
function IEnumerable<T>.ToDictionary<Key,Element>(keySelector: T->Key; elementSelector: T->Element; comparer: IEqualityComparer<Key>): Dictionary<Key,Element>;

// ����� ToLookup
/// ������� ������ System.Linq.Lookup �� ������������������ � ������������ � �������� �������� ��������� �����.
function IEnumerable<T>.ToLookup<Key>(keySelector: T->Key): System.Linq.ILookup<Key,T>;

/// ������� ������ System.Linq.Lookup �� ������������������ � ������������ � �������� �������� ��������� ����� � ������������ ������.
function IEnumerable<T>.ToLookup<Key>(keySelector: T->Key; comparer: IEqualityComparer<Key>): System.Linq.ILookup<Key,T>;

/// ������� ������ System.Linq.Lookup �� ������������������ � ������������ � ��������� ��������� ��������� ����� � ��������� ��������.
function IEnumerable<T>.ToLookup<Key,Element>(keySelector: T->Key; elementSelector: T->Element): System.Linq.ILookup<Key,Element>;

/// ������� ������ System.Linq.Lookup �� ������������������ � ������������ � �������� ������������ � ��������� ��������� ����� � ��������� ��������.
function IEnumerable<T>.ToLookup<Key,Element>(keySelector: T->Key; elementSelector: T->Element; comparer: IEqualityComparer<Key>): System.Linq.ILookup<Key,Element>;

// ����� OfType
/// ��������� ���������� ��������� ������� System.Collections.IEnumerable �� ��������� ����. ���������� ��������������������� ������ ������������������. � ������� ��� �������� ����������� ��������� ����.
function IEnumerable<T>.OfType<Res>(): IEnumerable<Res>;

// ����� Cast
/// ��������������� �������� ������� System.Collections.IEnumerable � �������� ���.
function IEnumerable<T>.Cast<Res>(): IEnumerable<Res>;

// ����� JoinIntoString
/// ����������� �������� ������������������ � ��������� �������������, ����� ���� ���������� �� � ������, ��������� delim � �������� �����������
function IEnumerable<T>.JoinIntoString(delim: string := ' '): string;

// ����� ForEach
/// ��������� �������� � ������� �������� ������������������
procedure IEnumerable<T>.ForEach(action: T->());

