// Методы Print
///- Выводит последовательность на экран, используя delim в качестве разделителя
function IEnumerable<T>.Print(delim: string := ' '): IEnumerable<T>;

/// Выводит последовательность на экран, используя delim в качестве разделителя, и переходит на новую строку
function IEnumerable<T>.Println(delim: string := ' '): IEnumerable<T>;

// Метод фильтрации Where
/// Выполняет фильтрацию последовательности значений на основе заданного предиката. Возвращает подпоследовательность значений исходной последовательности, удовлетворяющих предикату.
function IEnumerable<T>.Where(predicate: T->boolean): IEnumerable<T>;

/// Выполняет фильтрацию последовательности значений на основе заданного предиката с учётом индекса элемента. Возвращает подпоследовательность значений исходной последовательности, удовлетворяющих предикату.
function IEnumerable<T>.Where(predicate: (T,integer)->boolean): IEnumerable<T>;

// Метод проецирования Select
/// Проецирует каждый элемент последовательности на другой элемент с помощью функции selector. Возвращает последовательность элементов, полученных в результате проецирования.
function IEnumerable<T>.Select<Res>(selector: T->Res): IEnumerable<Res>;

/// Проецирует каждый элемент последовательности на другой элемент с помощью функции selector, учитывающую индекс элемента. Возвращает последовательность элементов, полученных в результате проецирования.
function IEnumerable<T>.Select<Res>(selector: (T,integer)->Res): IEnumerable<Res>;

// Метод проецирования SelectMany
/// Проецирует каждый элемент последовательности в новую последовательность и объединяет результирующие последовательности в одну последовательность. Возвращает объединённую последовательность.
function IEnumerable<T>.SelectMany<Res>(selector: T->IEnumerable<Res>): IEnumerable<Res>;

/// Проецирует каждый элемент последовательности в новую последовательность с учетом индекса элемента и объединяет результирующие последовательности в одну последовательность. Возвращает объединённую последовательность.
function IEnumerable<T>.SelectMany<Res>(selector: (T,integer)->IEnumerable<Res>): IEnumerable<Res>;

/// Проецирует каждый элемент последовательности в новую последовательность, объединяет результирующие последовательности в одну и вызывает функцию селектора результата для каждого элемента этой последовательности. Индекс каждого элемента исходной последовательности используется в промежуточной проецированной форме этого элемента. Возвращает объединённую последовательность.
function IEnumerable<T>.SelectMany<Coll,Res>(collSelector: (T,integer)->IEnumerable<Coll>; resultSelector: (T,Coll)->Res): IEnumerable<Res>;

/// Проецирует каждый элемент последовательности в новую последовательность, объединяет результирующие последовательности в одну и вызывает функцию селектора результата для каждого элемента этой последовательности. Возвращает объединённую последовательность.
function IEnumerable<T>.SelectMany<Coll,Res>(collSelector: T->IEnumerable<Coll>; resultSelector: (T,Coll)->Res): IEnumerable<Res>;

// Методы Take, TakeWhile, Skip, SkipWhile
/// Возвращает последовательность из count элементов с начала последовательности.
function IEnumerable<T>.Take(count: integer): IEnumerable<T>;

/// Возвращает цепочку элементов последовательности, удовлетворяющих указанному условию, до первого не удовлетворяющего
function IEnumerable<T>.TakeWhile(predicate: T->boolean): IEnumerable<T>;

/// Возвращает цепочку элементов последовательности, удовлетворяющих указанному условию, до первого не удовлетворяющего (учитывается индекс элемента)
function IEnumerable<T>.TakeWhile(predicate: (T,integer)->boolean): IEnumerable<T>;

/// Пропускает count элементов в последовательности и возвращает остальные элементы.
function IEnumerable<T>.Skip(count: integer): IEnumerable<T>;

/// Пропускает элементы в последовательности, пока они удовлетворяют заданному условию, и затем возвращает оставшиеся элементы.
function IEnumerable<T>.SkipWhile(predicate: T->boolean): IEnumerable<T>;

/// Пропускает элементы в последовательности, пока они удовлетворяют заданному условию, и затем возвращает оставшиеся элементы (учитывается индекс элемента)
function IEnumerable<T>.SkipWhile(predicate: (T,integer)->boolean): IEnumerable<T>;

// Метод Sorted
/// Возвращает отсортированную по возрастанию последовательность
function IEnumerable<T>.Sorted(): IEnumerable<T>;

// Методы OrderBy, OrderByDescending
/// Сортирует элементы последовательности в порядке возрастания ключа и возвращает отсортированнную последовательность. keySelector - функция, проектирующая элемент на ключ.
function IEnumerable<T>.OrderBy<Key>(keySelector: T->Key): System.Linq.IOrderedEnumerable<T>;

/// Сортирует элементы последовательности в порядке возрастания с использованием компаратора comparer и возвращает отсортированнную последовательность. keySelector - функция, проектирующая элемент на ключ.
function IEnumerable<T>.OrderBy<Key>(keySelector: T->Key; comparer: IComparer<Key>): System.Linq.IOrderedEnumerable<T>;

/// Сортирует элементы последовательности в порядке убывания ключа и возвращает отсортированнную последовательность. keySelector - функция, проектирующая элемент на ключ.
function IEnumerable<T>.OrderByDescending<Key>(keySelector: T->Key): System.Linq.IOrderedEnumerable<T>;

/// Сортирует элементы последовательности в порядке убывания с использованием компаратора comparer и возвращает отсортированнную последовательность. keySelector - функция, проектирующая элемент на ключ.
function IEnumerable<T>.OrderByDescending<Key>(keySelector: T->Key; comparer: IComparer<Key>): System.Linq.IOrderedEnumerable<T>;

// Методы ThenBy,ThenByDescending
///- Выполняет дополнительное упорядочение элементов последовательности в порядке возрастания ключа и возвращает отсортированнную последовательность. keySelector - функция, проектирующая элемент на ключ.
function IEnumerable<T>.ThenBy<Key>(keySelector: T->Key): System.Linq.IOrderedEnumerable<T>;

/// Выполняет дополнительное упорядочение элементов последовательности в порядке возрастания с использованием компаратора comparer и возвращает отсортированнную последовательность. keySelector - функция, проектирующая элемент на ключ.
function IEnumerable<T>.ThenBy<Key>(keySelector: T->Key; comparer: IComparer<Key>): System.Linq.IOrderedEnumerable<T>;

/// Выполняет дополнительное упорядочение элементов последовательности в порядке убывания ключа и возвращает отсортированнную последовательность. keySelector - функция, проектирующая элемент на ключ. 
function IEnumerable<T>.ThenByDescending<Key>(keySelector: T->Key): System.Linq.IOrderedEnumerable<T>;

/// Выполняет дополнительное упорядочение элементов последовательности в порядке убывания с использованием компаратора comparer и возвращает отсортированнную последовательность. keySelector - функция, проектирующая элемент на ключ. 
function IEnumerable<T>.ThenByDescending<Key>(keySelector: T->Key; comparer: IComparer<Key>): System.Linq.IOrderedEnumerable<T>;

// Метод Concat
/// Соединяет две последовательности, дописывая вторую в конец первой и возвращая результирующую последовательность
function IEnumerable<T>.Concat(second: IEnumerable<T>): IEnumerable<T>;

// Метод Zip
/// Объединяет две последовательности, используя указанную функцию, принимающую по одному элементу каждой последовательности и возвращающую элемент результирующей последовательности.
function IEnumerable<T>.Zip<TSecond,Res>(second: IEnumerable<TSecond>; resultSelector: (T,TSecond)->Res): IEnumerable<Res>;

// Метод Distinct
/// Возвращает различающиеся элементы последовательности.
function IEnumerable<T>.Distinct(): IEnumerable<T>;

/// Возвращает различающиеся элементы последовательности, используя для сравнения значений компаратор comparer.
function IEnumerable<T>.Distinct(comparer: IEqualityComparer<T>): IEnumerable<T>;

// Методы Union,Intersect,Except
/// Находит объединение множеств, представленных двумя последовательностями.
function IEnumerable<T>.Union(second: IEnumerable<T>): IEnumerable<T>;

/// Находит объединение множеств, представленных двумя последовательностями, используя указанный компаратор.
function IEnumerable<T>.Union(second: IEnumerable<T>; comparer: IEqualityComparer<T>): IEnumerable<T>;

/// Находит пересечение множеств, представленных двумя последовательностями.
function IEnumerable<T>.Intersect(second: IEnumerable<T>): IEnumerable<T>;

/// Находит пересечение множеств, представленных двумя последовательностями, используя для сравнения значений указанный компаратор.
function IEnumerable<T>.Intersect(second: IEnumerable<T>; comparer: IEqualityComparer<T>): IEnumerable<T>;

/// Находит разность множеств, представленных двумя последовательностями.
function IEnumerable<T>.Except(second: IEnumerable<T>): IEnumerable<T>;

/// Находит разность множеств, представленных двумя последовательностями, используя для сравнения значений указанный компаратор.
function IEnumerable<T>.Except(second: IEnumerable<T>; comparer: IEqualityComparer<T>): IEnumerable<T>;

// Метод Reverse
///- Возвращает инвертированную последовательность
function IEnumerable<T>.Reverse(): IEnumerable<T>;

// Метод SequenceEqual
///- Определяет, совпадают ли две последовательности.
function IEnumerable<T>.SequenceEqual(second: IEnumerable<T>): boolean;

/// Определяет, совпадают ли две последовательности, используя для сравнения элементов указанный компаратор.
function IEnumerable<T>.SequenceEqual(second: IEnumerable<T>; comparer: IEqualityComparer<T>): boolean;

// Методы First, FirstOrDefault
///- Возвращает первый элемент последовательности.
function IEnumerable<T>.First(): T;

/// Возвращает первый элемент последовательности, удовлетворяющий указанному условию.
function IEnumerable<T>.First(predicate: T->boolean): T;

/// Возвращает первый элемент последовательности или значение по умолчанию, если последовательность не содержит элементов.
function IEnumerable<T>.FirstOrDefault(): T;

/// Возвращает первый удовлетворяющий условию элемент последовательности или значение по умолчанию, если ни одного такого элемента не найдено.
function IEnumerable<T>.FirstOrDefault(predicate: T->boolean): T;

// Методы Last, LastOrDefault
///- Возвращает последний элемент последовательности.
function IEnumerable<T>.Last(): T;

/// Возвращает последний элемент последовательности, удовлетворяющий указанному условию.
function IEnumerable<T>.Last(predicate: T->boolean): T;

/// Возвращает последний элемент последовательности или значение по умолчанию, если последовательность не содержит элементов.
function IEnumerable<T>.LastOrDefault(): T;

/// Возвращает последний элемент последовательности, удовлетворяющий указанному условию, или значение по умолчанию, если ни одного такого элемента не найдено.
function IEnumerable<T>.LastOrDefault(predicate: T->boolean): T;

// Методы Single, SingleOrDefault
///- Возвращает единственный элемент последовательности и генерирует исключение, если число элементов последовательности отлично от 1.
function IEnumerable<T>.Single(): T;

/// Возвращает единственный элемент последовательности, удовлетворяющий заданному условию, и генерирует исключение, если таких элементов больше одного.
function IEnumerable<T>.Single(predicate: T->boolean): T;

/// Возвращает единственный элемент последовательности или значение по умолчанию, если последовательность пуста; если в последовательности более одного элемента, генерируется исключение.
function IEnumerable<T>.SingleOrDefault(): T;

/// Возвращает единственный элемент последовательности, удовлетворяющий заданному условию, или значение по умолчанию, если такого элемента не существует; если условию удовлетворяет более одного элемента, генерируется исключение.
function IEnumerable<T>.SingleOrDefault(predicate: T->boolean): T;

// Метод DefaultIfEmpty
///- Возвращает элементы указанной последовательности или одноэлементную коллекцию, содержащую значение параметра типа по умолчанию, если последовательность пуста.
function IEnumerable<T>.DefaultIfEmpty(): IEnumerable<T>;

/// Возвращает элементы указанной последовательности или одноэлементную коллекцию, содержащую указанное значение, если последовательность пуста.
function IEnumerable<T>.DefaultIfEmpty(defaultValue: T): IEnumerable<T>;

// Методы ElementAt, ElementAtOrDefault
///- Возвращает элемент по указанному индексу в последовательности.
function IEnumerable<T>.ElementAt(index: integer): T;

/// Возвращает элемент по указанному индексу в последовательности или значение по умолчанию, если индекс вне допустимого диапазона.
function IEnumerable<T>.ElementAtOrDefault(index: integer): T;

// Методы Any, All
///- Проверяет, содержит ли последовательность какие-либо элементы.
function IEnumerable<T>.Any(): boolean;

/// Проверяет, удовлетворяет ли какой-либо элемент последовательности заданному условию.
function IEnumerable<T>.Any(predicate: T->boolean): boolean;

/// Проверяет, все ли элементы последовательности удовлетворяют условию.
function IEnumerable<T>.All(predicate: T->boolean): boolean;

// Методы Count
/// Возвращает количество элементов в последовательности.
function IEnumerable<T>.Count(): integer;

///- Возвращает число, представляющее количество элементов последовательности, удовлетворяющих заданному условию.
function IEnumerable<T>.Count(predicate: T->boolean): integer;

/// Возвращает значение типа Int64, представляющее общее число элементов в последовательности.
function IEnumerable<T>.LongCount(): int64;

/// Возвращает значение типа Int64, представляющее число элементов последовательности, удовлетворяющих заданному условию.
function IEnumerable<T>.LongCount(predicate: T->boolean): int64;

// Метод Contains
///- Определяет, содержится ли указанный элемент в последовательности, используя компаратор проверки на равенство по умолчанию.
function IEnumerable<T>.Contains(value: T): boolean;

/// Определяет, содержит ли последовательность заданный элемент, используя указанный компаратор.
function IEnumerable<T>.Contains(value: T; comparer: IEqualityComparer<T>): boolean;

// Метод Aggregate
///- Применяет к последовательности агрегатную функцию. Возвращает конечное агрегатное значение.
function IEnumerable<T>.Aggregate(func: (T,T)->T): T;

/// Применяет к последовательности агрегатную функцию. Указанное начальное значение используется в качестве исходного значения агрегатной операции. Возвращает конечное агрегатное значение.
function IEnumerable<T>.Aggregate<Accum>(seed: T; func: (Accum,T)->Accum): T;

/// Применяет к последовательности агрегатную функцию.Указанное начальное значение служит исходным значением для агрегатной операции, а указанная функция используется для выбора результирующего значения. Возвращает конечное агрегатное значение.
function IEnumerable<T>.Aggregate<Accum,Res>(seed: T; func: (Accum,T)->Accum; resultSelector: Accum->Res): T;

// Методы Sum, Average
/// Вычисляет сумму последовательности значений числового типа  
function IEnumerable<число>.Sum(): число;

/// Вычисляет сумму последовательности значений числового типа, получаемой в результате применения функции преобразования к каждому элементу входной последовательности.
function IEnumerable<T>.Sum(selector: T->число): число;

/// Вычисляет среднее для последовательности значений числового типа  
function IEnumerable<число>.Average(): real;

/// Вычисляет среднее для последовательности значений числового типа, получаемой в результате применения функции преобразования к каждому элементу входной последовательности.
function IEnumerable<T>.Average(selector: T->число): real;

// Методы Min, Max
/// Вычисляет минимальный элемент последовательности значений числового типа  
function IEnumerable<число>.Min(): число;

/// Вызывает функцию преобразования для каждого элемента последовательности и возвращает минимальное значение числового типа.
function IEnumerable<T>.Min(selector: T->число): число;

/// Вычисляет максимальный элемент последовательности значений числового типа  
function IEnumerable<число>.Max(): число;

/// Вызывает функцию преобразования для каждого элемента последовательности и возвращает максимальное значение числового типа.
function IEnumerable<T>.Max(selector: T->число): число;

// Метод Join
/// Объединяет две последовательности на основе сопоставления ключей в третью последовательность. Функция resultSelector задаёт проекцию элементов двух последовательностей с одинаковыми значениями ключа в элемент третьей последовательности. 
function IEnumerable<T>.Join<TInner,Key,Res>(inner: IEnumerable<TInner>; outerKeySelector: T->Key; innerKeySelector: TInner->TKey; resultSelector: (T,TInner)->Res): IEnumerable<Res>;

/// Объединяет две последовательности на основе сопоставления ключей в третью последовательность. Функция resultSelector задаёт проекцию элементов двух последовательностей с одинаковыми значениями ключа в элемент третьей последовательности. Для сравнения ключей используется компаратор comparer
function IEnumerable<T>.Join<TInner,Key,Res>(inner: IEnumerable<TInner>; outerKeySelector: T->Key; innerKeySelector: TInner->TKey; resultSelector: (T,TInner)->Res; comparer: System.Collections.Generic.IEqualityComparer<Key>): IEnumerable<Res>;

// Метод GroupJoin
/// Объединяет две последовательности на основе равенства ключей и группирует результаты. Затем функция resultSelector проектирует ключ и последовательность соответствующих ему значений на элемент результирующей последовательности. 
function IEnumerable<T>.GroupJoin<TInner,Key,Res>(inner: IEnumerable<TInner>; outerKeySelector: T->Key; innerKeySelector: TInner->TKey; resultSelector: (T,IEnumerable<TInner>)->Res): IEnumerable<Res>;

/// Объединяет две последовательности на основе равенства ключей и группирует результаты. Для сравнения ключей используется указанный компаратор. Затем функция resultSelector проектирует ключ и последовательность соответствующих ему значений на элемент результирующей последовательности.
function IEnumerable<T>.GroupJoin<TInner,Key,Res>(inner: IEnumerable<TInner>; outerKeySelector: T->Key; innerKeySelector: TInner->TKey; resultSelector: (T,IEnumerable<TInner>)->Res; comparer: IEqualityComparer<Key>): IEnumerable<Res>;

// Метод GroupBy
/// Группирует элементы последовательности в соответствии с заданной функцией селектора ключа и возвращает последовательность групп; каждая группа соответствует одному значению ключа.
function IEnumerable<T>.GroupBy<Key>(keySelector: T->Key): IEnumerable<IGrouping<Key,T>>;

/// Группирует элементы последовательности в соответствии с заданной функцией селектора ключа, сравнивает ключи с помощью указанного компаратора и возвращает последовательность групп; каждая группа соответствует одному значению ключа.
function IEnumerable<T>.GroupBy<Key>(keySelector: T->Key; comparer: System.Collections.Generic.IEqualityComparer<Key>): IEnumerable<IGrouping<Key,T>>;

/// Группирует элементы последовательности в соответствии с заданной функцией селектора ключа и проецирует элементы каждой группы с помощью указанной функции. Возвращает последовательность групп; каждая группа соответствует одному значению ключа.
function IEnumerable<T>.GroupBy<Key,Element>(keySelector: T->Key; elementSelector: T->Element): IEnumerable<IGrouping<Key,T>>;

/// Группирует элементы последовательности в соответствии с функцией селектора ключа.Ключи сравниваются с помощью компаратора, элементы каждой группы проецируются с помощью указанной функции.
function IEnumerable<T>.GroupBy<Key,Element>(keySelector: T->Key; elementSelector: T->Element; comparer: IEqualityComparer<Key>): IEnumerable<IGrouping<Key,Element>>;

/// Группирует элементы последовательности в соответствии с заданной функцией селектора ключа и создает результирующее значение для каждой группы и ее ключа.
function IEnumerable<T>.GroupBy<Key,Res>(keySelector: T->Key; resultSelector: (Key,IEnumerable<T>)->Res): IEnumerable<Res>;

/// Группирует элементы последовательности в соответствии с заданной функцией селектора ключа и создает результирующее значение для каждой группы и ее ключа.Элементы каждой группы проецируются с помощью указанной функции.
function IEnumerable<T>.GroupBy<Key,Element,Res>(keySelector: T->Key; elementSelector: T->Element; resultSelector: (Key,IEnumerable<Element>)->Res): IEnumerable<Res>;

/// Группирует элементы последовательности в соответствии с заданной функцией селектора ключа и создает результирующее значение для каждой группы и ее ключа.Ключи сравниваются с использованием заданного компаратора.
function IEnumerable<T>.GroupBy<Key,Res>(keySelector: T->Key; resultSelector: (Key,IEnumerable<T>)->Res; comparer: IEqualityComparer<Key>): IEnumerable<Res>;

/// Группирует элементы последовательности в соответствии с заданной функцией селектора ключа и создает результирующее значение для каждой группы и ее ключа.Значения ключей сравниваются с помощью указанного компаратора, элементы каждой группы проецируются с помощью указанной функции.
function IEnumerable<T>.GroupBy<Key,Element,Res>(keySelector: T->Key; elementSelector: System.T->Element; resultSelector: (Key,IEnumerable<Element>)->Res; comparer: IEqualityComparer<Key>): IEnumerable<Res>;

// Метод AsEnumerable
/// Возвращает входные данные, приведенные к типу IEnumerable<T>.
function IEnumerable<T>.AsEnumerable(): IEnumerable<T>;

// Методы ToArray, ToList
/// Создает массив из последовательности.
function IEnumerable<T>.ToArray(): array of T;

/// Создает список List из последовательности.
function IEnumerable<T>.ToList(): List<T>;

// Метод ToDictionary
/// Создает словарь Dictionary из последовательности соответствии с заданной функцией селектора ключа.
function IEnumerable<T>.ToDictionary<Key>(keySelector: T->Key): Dictionary<Key,T>;

/// Создает словарь Dictionary из последовательности в соответствии с заданной функцией селектора ключа и компаратором ключей.
function IEnumerable<T>.ToDictionary<Key>(keySelector: T->Key; comparer: IEqualityComparer<Key>): Dictionary<Key,T>;

/// Создает словарь Dictionary из последовательности в соответствии с заданными функциями селектора ключа и селектора элемента.
function IEnumerable<T>.ToDictionary<Key,Element>(keySelector: T->Key; elementSelector: T->Element): Dictionary<Key,Element>;

/// Создает словарь Dictionary из последовательности в соответствии с заданным компаратором и функциями селектора ключа и селектора элемента.
function IEnumerable<T>.ToDictionary<Key,Element>(keySelector: T->Key; elementSelector: T->Element; comparer: IEqualityComparer<Key>): Dictionary<Key,Element>;

// Метод ToLookup
/// Создает объект System.Linq.Lookup из последовательности в соответствии с заданной функцией селектора ключа.
function IEnumerable<T>.ToLookup<Key>(keySelector: T->Key): System.Linq.ILookup<Key,T>;

/// Создает объект System.Linq.Lookup из последовательности в соответствии с заданной функцией селектора ключа и компаратором ключей.
function IEnumerable<T>.ToLookup<Key>(keySelector: T->Key; comparer: IEqualityComparer<Key>): System.Linq.ILookup<Key,T>;

/// Создает объект System.Linq.Lookup из последовательности в соответствии с заданными функциями селектора ключа и селектора элемента.
function IEnumerable<T>.ToLookup<Key,Element>(keySelector: T->Key; elementSelector: T->Element): System.Linq.ILookup<Key,Element>;

/// Создает объект System.Linq.Lookup из последовательности в соответствии с заданным компаратором и функциями селектора ключа и селектора элемента.
function IEnumerable<T>.ToLookup<Key,Element>(keySelector: T->Key; elementSelector: T->Element; comparer: IEqualityComparer<Key>): System.Linq.ILookup<Key,Element>;

// Метод OfType
/// Выполняет фильтрацию элементов объекта System.Collections.IEnumerable по заданному типу. Возвращает подпоследовательность данной последовательности. в которой все элементы принадлежат заданному типу.
function IEnumerable<T>.OfType<Res>(): IEnumerable<Res>;

// Метод Cast
/// Преобразовывает элементы объекта System.Collections.IEnumerable в заданный тип.
function IEnumerable<T>.Cast<Res>(): IEnumerable<Res>;

// Метод JoinIntoString
/// Преобразует элементы последовательности в строковое представление, после чего объединяет их в строку, используя delim в качестве разделителя
function IEnumerable<T>.JoinIntoString(delim: string := ' '): string;

// Метод ForEach
/// Применяет действие к каждому элементу последовательности
procedure IEnumerable<T>.ForEach(action: T->());

