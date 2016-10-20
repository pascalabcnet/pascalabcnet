///- Модуль Исполнители
/// Описывает ряд Исполнителей, которые могут выполнять команды
unit Исполнители;

type
  ///!#
  целое = integer;
  вещественное = real;
  логическое = boolean;
  строковое = string;

///- Процедура Вывести(значения)
/// Выводит список значений
procedure Вывести(params args: array of object) := Println(args);
/// Выводит пустую строку
procedure ВывестиПустуюСтроку := Println;

///- Метод Целое.Вывести
/// Выводит целое
procedure Вывести(Self: целое); extensionmethod := Println(Self);
///- Метод Вещественное.Вывести
/// Выводит вещественное
procedure Вывести(Self: real); extensionmethod := Println(Self);
///- Метод Строка.Вывести
/// Выводит строку
procedure Вывести(Self: string); extensionmethod := Println(Self);

type
  ///!#
  Seq<T> = interface(IEnumerable<T>)
  
  end;
  
  ///!#
  ВыводКласс = class
  public
    ///- Метод Вывод.Данные(параметры)
    /// Выводит данные, разделяя их пробелами
    procedure Данные(params args: array of object) := Println(args);
    ///- Метод Вывод.ПустаяСтрока
    /// Выводит пустую строку
    procedure ПустаяСтрока := Println;
  end;
  
  ПоследовательностьКласс = class
    
  end;

// Сервисные методы

function DeleteEnd(Self: string; s: string): string; extensionmethod;
begin
  if Self.EndsWith(s) then
  begin
    var i := Self.LastIndexOf(s);
    if (i>=0) and (i<Self.Length) then
      Result := Self.Remove(i)
    else Result := Self;  
  end
  else Result := Self;
end;
 
Procedure PrintAllMethods(o: Object);
begin
  WritelnFormat('Методы исполнителя {0}:',o.GetType.Name.DeleteEnd('Класс'));
  o.GetType.GetMethods(System.Reflection.BindingFlags.Public or
            System.Reflection.BindingFlags.Instance or 
            System.Reflection.BindingFlags.DeclaredOnly)
    .Select(s->s.ToString.Replace('Void ','')
    .Replace('Int32','целое')
    .Replace('Boolean','логическое')
    .Replace('System.String','строковое')
    .Replace('Double','вещественное'))
    .Select(s->'  '+s.DeleteEnd('()'))
    .Where(s->not s.ToString.Contains('$Init$'))
    .Println(NewLine);
end;

// Реализация
type
  ///!#
  МножествоКласс = class
  private
    s := new SortedSet<integer>;
  public
    constructor;
    begin end;
    ///- Метод Множество.Добавить(элемент: целое)
    /// Добавляет элемент к множеству. Если такой элемент есть, то ничего не происходит
    procedure Добавить(params a: array of integer);
    begin
      a.ForEach(x->begin s.Add(x) end);
      //s.Add(элемент);
    end;   
    ///- Метод Множество.Удалить(элемент: целое)
    /// Удаляет элемент из множества. Если такого элемента нет, то ничего не происходит
    procedure Удалить(элемент: целое);
    begin
      s.Remove(элемент);
    end;
    ///- Метод Множество.Вывести
    /// Выводит элементы множества
    procedure Вывести;
    begin
      s.Println;
    end;
    ///- Метод Множество.Содержит(элемент: целое): логический
    /// Проверяет, есть ли элемент во множестве
    function Содержит(элемент: целое): логическое;
    begin
      Result := s.Contains(элемент)
    end;
    ///- Метод Множество.ВывестиВсеМетоды
    /// Выводит все методы Исполнителя Множество
    procedure ВывестиВсеМетоды;
    begin
      if Random(2)=1 then
        PrintAllMethods(Self)
      else 
      begin
        WritelnFormat('Методы исполнителя {0}:',Self.GetType.Name.DeleteEnd('Класс'));
        Writeln('  Добавить(элемент: целое)');
        Writeln('  Удалить(элемент: целое)');
        Writeln('  Вывести');
        Writeln('  Содержит(элемент: целое): логическое');
        Writeln('  ВывестиВсеМетоды');
      end;
    end;
    ///- Метод Множество.Очистить
    /// Делает множество пустым
    procedure Очистить;
    begin
      s.Clear
    end;
    ///- Метод Множество.Новое
    /// Создает новое множество
    function Новое(params a: array of integer): МножествоКласс;
    begin
      Result := new МножествоКласс();
      Result.Добавить(a)
    end;
    ///- Метод Множество.Пересечение(Множество1)
    /// Возвращает пересечение множеств   
    function Пересечение(s1: МножествоКласс): МножествоКласс;
    begin
      Result := new МножествоКласс();
      var ss := SSet(s.AsEnumerable&<integer>);
      ss.IntersectWith(s1.s);
      Result.s := ss;
    end;
  end;
  
type  
  ///!#
  МножествоСтрокКласс = class
  private
    s := new SortedSet<string>;
  public
    constructor;
    begin end;
    ///- Метод Множество.Добавить(элемент: строковое)
    /// Добавляет элемент к множеству. Если такой элемент есть, то ничего не происходит
    procedure Добавить(элемент: string);
    begin
      s.Add(элемент);
    end;
    ///- Метод Множество.Удалить(элемент: строковое)
    /// Удаляет элемент из множества. Если такого элемента нет, то ничего не происходит
    procedure Удалить(элемент: string);
    begin
      s.Remove(элемент);
    end;
    ///- Метод Множество.Вывести
    /// Выводит элементы множества
    procedure Вывести;
    begin
      s.Println;
    end;
    ///- Метод Множество.Содержит(элемент: строковое): логический
    /// Проверяет, есть ли элемент во множестве
    function Содержит(элемент: string): логическое;
    begin
      Result := s.Contains(элемент)
    end;
    ///- Метод Множество.ВывестиВсеМетоды
    /// Выводит все методы Исполнителя Множество
    procedure ВывестиВсеМетоды;
    begin
      if Random(2)=1 then
        PrintAllMethods(Self)
      else 
      begin
        WritelnFormat('Методы исполнителя {0}:',Self.GetType.Name.DeleteEnd('Класс'));
        Writeln('  Добавить(элемент: целое)');
        Writeln('  Удалить(элемент: целое)');
        Writeln('  Вывести');
        Writeln('  Содержит(элемент: целое): логическое');
        Writeln('  ВывестиВсеМетоды');
      end;
    end;
    ///- Метод Множество.Очистить
    /// Делает множество пустым
    procedure Очистить;
    begin
      s.Clear
    end;
    ///- Метод Множество.Новое
    /// Создает новое множество
    function Новое := new МножествоСтрокКласс;
    ///- Метод Множество.СоздатьПары(множество1)
    /// Создает пары элементов из двух множеств
    function СоздатьПары(мн1: МножествоСтрокКласс): МножествоСтрокКласс;
    begin
      var ss: SortedSet<string>;
      ss := (мн1 as МножествоСтрокКласс).s;
      
      var m := new МножествоСтрокКласс;
      m.s := s.ZipTuple(ss).Select(x -> x.ToString()).ToSortedSet;
      Result := m
    end;
  end;
  
type
///!#
  ВычислительКласс = class
  public
  ///- Метод Вычислитель.КвадратноеУравнение(a,b,c: вещественное)
  /// Выводит все решения квадратного уравнения
    procedure РешитьКвадратноеУравнение(a,b,c: real);
    begin
      writelnFormat('Квадратное уравнение: {0}*x*x+{1}*x+{2}=0',a,b,c);
      var D := b*b-4*a*c;
      if D<0 then
        writeln('Решений нет')
      else
      begin
        var x1 := (-b-sqrt(D))/2/a;
        var x2 := (-b+sqrt(D))/2/a;
        writelnFormat('Решения: x1={0} x2={1}',x1,x2)
      end;
    end;
  ///- Метод Вычислитель.АрифметическаяПрогрессия(a0,d: целое)
  /// Выводит арифметическую прогрессию
    procedure ВывестиАрифметическуюПрогрессию(a0,d: integer);
    begin
      writelnFormat('Арифметическая прогрессия: a0={0} d={1}',a0,d);
      SeqGen(10,a0,x->x+d).Println; // ! Ошибка если процедуры описываются перед
    end;
    procedure ВывестиВсеМетоды;
    begin
      PrintAllMethods(Self);
    end;
  end;

  FileState = (Closed,OpenedForRead,OpenedForWrite);
  ///!#
  ФайлКласс = class
  private
    f: Text;
    State := FileState.Closed;
  public
    constructor ;
    begin
    end;
  ///- Метод Файл.ОткрытьНаЧтение(имя)
  /// Открывает файл на чтение
    procedure ОткрытьНаЧтение(имя: строковое);
    begin
      if State<>FileState.Closed then
        f.Close;
      f := OpenRead(имя);
      State := FileState.OpenedForRead
    end;
  ///- Метод Файл.ОткрытьНаЗапись(имя)
  /// Открывает файл на запись
    procedure ОткрытьНаЗапись(имя: строковое);
    begin
      if State<>FileState.Closed then
        f.Close;
      f := OpenWrite(имя);
      State := FileState.OpenedForWrite
    end;
  ///- Метод Файл.Закрыть
  /// Закрывает файл
    procedure Закрыть;
    begin
      if State=FileState.Closed then
        Println('Ошибка: Файл уже закрыт')
      else f.Close;
      State := FileState.Closed;
    end;
  ///- Метод Файл.Записать(строка)
  /// Записывает строку в файл
    procedure Записать(строка: строковое);
    begin
      if State=FileState.Closed then
        Println('Ошибка: Перед записью файл следует открыть')
      else f.Writeln(строка)
    end;
  ///- Метод Файл.ПрочитатьСтроку
  /// Возвращает строку, считанную из файла
    function ПрочитатьСтроку: строковое;
    begin
      if State=FileState.Closed then
      begin
        Println('Ошибка: Перед чтением файл следует открыть');
        Result := '';
      end
      else 
      begin
        Result := f.ReadlnString;
        Println(Result);
      end;  
    end;
  ///- Метод Файл.ПрочитатьЦелое
  /// Возвращает целое, считанное из файла
    function ПрочитатьЦелое: целое;
    begin
      if State=FileState.Closed then
      begin
        Println('Ошибка: Перед чтением файл надо открыть');
        Result := 0;
      end
      else 
      begin
        Result := f.ReadInteger;
        Print(Result);
      end;  
    end;
  ///- Метод Файл.ПрочитатьВещественное
  /// Возвращает вещественное, считанное из файла
    function ПрочитатьВещественное: вещественное;
    begin
      if State=FileState.Closed then
      begin
        Println('Ошибка: Перед чтением файл надо открыть');
        Result := 0;
      end
      else 
      begin
        Result := f.ReadReal;
        Print(Result);
      end;  
    end;
  ///- Метод Файл.КонецФайла
  /// Возвращает, достигнут ли конец файла
    function КонецФайла: логическое;
    begin
      Result := f.Eof;
    end;
  ///- Метод Файл.ИмяФайла
  /// Возвращает имя файла
    function ИмяФайла: строковое;
    begin
      if State=FileState.Closed then
      begin
        Println('Ошибка: Файл закрыт, поэтому он не имеет имени');
        Result := '';
      end
      else 
      begin
        Println('Имя файла: ',f.Name);
        Result := f.Name;
      end
    end;
  ///- Метод Файл.ВывестиСодержимое
  /// Выводит содержимое файла
    procedure ВывестиСодержимое(имяфайла: строковое);
    begin
      if (State<>FileState.Closed) and (f.Name.ToLower=имяфайла.ToLower) then
        Println('Ошибка: Содержимое можно вывести только у закрытого файла')
      else 
      begin
        WritelnFormat('Содержимое файла {0}:',имяфайла);
        try
          ReadLines(имяфайла).Println(NewLine);
        except
          WritelnFormat('Файл {0}: отсутствует на диске',имяфайла);
        end;
      end;  
        
    end;
  ///- Метод Файл.ВывестиВсеМетоды
  /// Выводит все методы исполнителя Файл
    procedure ВывестиВсеМетоды;
    begin
      PrintAllMethods(Self);
    end;
  ///- Метод Файл.Новый
  /// Создает и возвращает нового исполнителя Файл
    function Новый := new ФайлКласс;
  ///- Метод Файл.ВсеСтроки
  /// Возвращает все строки файла в виде последовательности
    function ВсеСтроки(имяфайла: строковое): sequence of строковое;
    begin
      if (State<>FileState.Closed) and (f.Name.ToLower=имяфайла.ToLower) then
      begin
        Println('Ошибка: Строки можно получить только у закрытого файла');
        Result := nil;
        exit;
      end;
      Result := ReadLines(имяфайла).ToArray;
    end;
  end;

const dbname = 'countries.db';

var coun: array of string := nil;

function СтраныСтроки: sequence of string;
begin
  if coun = nil then
    coun := ReadLines(dbname).ToArray();
  Result := coun;  
end;

///- Метод Последовательность.Вывести
/// Выводит все элементы последовательности, разделяя их пробелами
function Вывести<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  Self.Println;
  Result := Self;
end;

///- Метод Последовательность.ВывестиПострочно
/// Выводит все элементы последовательности - каждый элемент с новой строки
function ВывестиПострочно<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  Self.Println(NewLine);
  Result := Self;
end;

///- Метод Последовательность.Выбрать(условие)
/// Выбирает все элементы последовательности, удовлетворяющие указанному условию
function Выбрать<T>(Self: sequence of T; cond: T -> boolean): sequence of T; extensionmethod;
begin
  Result := Self.Where(cond);  
end;

///- Метод Последовательность.Взять(n)
/// Возвращает первые n элементов последовательности
function Взять<T>(Self: sequence of T; n: integer): sequence of T; extensionmethod;
begin
  Result := Self.Take(n);  
end;

///- Метод Последовательность.Количество(условие)
/// Возвращает количество элементов последовательности, удовлетворяющих указанному условию
function Количество<T>(Self: sequence of T; cond: T -> boolean := nil): целое; extensionmethod;
begin
  if cond = nil then
    Result := Self.Count()
  else Result := Self.Count(cond)
end;

///- Метод Последовательность.Сумма
/// Возвращает сумму элементов последовательности
function Сумма(Self: sequence of integer): integer; extensionmethod;
begin
  Result := Self.Sum();  
end;  

///- Метод Последовательность.Среднее
/// Возвращает среднее элементов последовательности
function Среднее(Self: sequence of integer): real; extensionmethod;
begin
  Result := Self.Average;  
end;  

///- Метод Последовательность.Минимум
/// Возвращает минимальный элемент последовательности
function Минимум(Self: sequence of integer): integer; extensionmethod;
begin
  Result := Self.Min;  
end;  

///- Метод Последовательность.Максимум
/// Возвращает максимальный элемент последовательности
function Максимум(Self: sequence of integer): integer; extensionmethod;
begin
  Result := Self.Max;
end;  

///- Метод Последовательность.Преобразовать(функция преобразования)
/// Преобразует элементы последовательности с помощью функции и возвращает новую последовательность
function Преобразовать<T,Key>(Self: sequence of T; conv: T -> Key): sequence of Key; extensionmethod;
begin
  Result := Self.Select(conv);  
end;

///- Метод Последовательность.ОтсортироватьПо(проекция элемента на ключ)
/// Сортирует элементы последовательности по возрастанию ключа
function ОтсортироватьПо<T,Key>(Self: sequence of T; cond: T -> Key): sequence of T; extensionmethod;
begin
  Result := Self.OrderBy(cond);  
end;

///- Метод Последовательность.ОтсортироватьПо(проекция элемента на ключ)
/// Сортирует элементы последовательности по ключу
function Отсортировать<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  Result := Self.OrderBy(x->x);  
end;

///- Метод Последовательность.ОтсортироватьПоУбыванию(проекция элемента на ключ)
/// Сортирует элементы последовательности по убыванию ключа
function ОтсортироватьПоУбыванию<T,Key>(Self: sequence of T; cond: T -> Key): sequence of T; extensionmethod;
begin
  Result := Self.OrderByDescending(cond);  
end;

///- Метод Последовательность.ДляВсех(действие)
/// Выполняет указанное действие для каждого элемента последовательности
procedure ДляВсех<T>(Self: sequence of T; act: T -> ()); extensionmethod;
begin
  Self.Foreach(act);  
end;

function НаБукву(c: char): string -> boolean;
begin
  Result := страна -> страна[1] = c;
end;

function НаБукву(s: string): string -> boolean;
begin
  Result := страна -> страна[1] = s[1];
end;

function НачинаетсяНа(Self: string; s: string): boolean; extensionmethod;
begin
  Result := Self.StartsWith(s);  
end;

///- Четное(значение)
/// Возвращает, является ли значение четным
function Четное(x: integer): boolean;
begin
  Result := x mod 2 = 0;
end;

///- Нечетное(значение)
/// Возвращает, является ли значение нечетным
function Нечетное(x: integer): boolean;
begin
  Result := x mod 2 <> 0;
end;

type 
///!#
  ВсеИсполнителиКласс = class
public
  ///- Исполнитель Множество 
  /// Предоставляет методы для множества целых
  Множество: МножествоКласс;
  ///- Исполнитель МножествоСтрок
  /// Предоставляет методы для множества строк
  МножествоСтрок: МножествоСтрокКласс;
  ///- Исполнитель Вычислитель
  /// Предоставляет ряд методов из области математики
  Вычислитель: ВычислительКласс;
  ///- Исполнитель Файл
  /// Предоставляет ряд методов для работы с файлами на диске
  Файл: ФайлКласс;
  ///- Исполнитель Вывод
  /// Предоставляет методы для вывода данных
  Вывод: ВыводКласс;
  ///- Метод Вывести
  /// Выводит всех исполнителей
  procedure Вывести;
  begin
    Println('Множество');
    Println('МножествоСтрок');
    Println('Вычислитель');
    Println('Файл');
    Println('Вывод');
  end;
end;

type 
  ///!#
  Country = auto class
    nm,cap: string;
    inh: integer;
    cont: string;
  public  
    property Название: string read nm;
    property Столица: string read cap;
    property Население: integer read inh;
    property Континент: string read cont;
  end;
  
var страны: sequence of Country;  

procedure InitCountries();
begin
  страны := ReadLines('Страны.csv')
    .Select(s->s.ToWords(';'))
    .Select(w->new Country(w[0],w[1],w[2].ToInteger,w[3])).ToArray;
end;

// Последовательности

///- АрифметическаяПрогрессия(первый,шаг,количество)
/// Возвращает арифметическую прогрессию с указанным первым элементом, шагом и количеством
function АрифметическаяПрогрессия(a,d: integer; n: integer := 20): sequence of integer;
begin
  Result := SeqGen(n,a,a->a+d)
end;

///- АрифметическаяПрогрессия(первый,шаг,количество)
/// Возвращает арифметическую прогрессию с указанным первым элементом, шагом и количеством
function АрифметическаяПрогрессия(a,d: real; n: integer := 20): sequence of real;
begin
  Result := SeqGen(n,a,a->a+d)
end;

///- ГеометическаяПрогрессия(первый,шаг,количество)
/// Возвращает геометическую прогрессию с указанным первым элементом, шагом и количеством
function ГеометическаяПрогрессия(a,d: integer; n: integer := 10): sequence of integer;
begin
  Result := SeqGen(n,a,a->a*d)
end;

///- ГеометическаяПрогрессия(первый,шаг,количество)
/// Возвращает геометическую прогрессию с указанным первым элементом, шагом и количеством
function ГеометическаяПрогрессия(a,d: real; n: integer := 10): sequence of real;
begin
  Result := SeqGen(n,a,a->a*d)
end;

///- СлучайнаяПоследовательность(количество,от,до)
/// Возвращает случайную последовательность элементов в диапазоне [от, до] 
function СлучайнаяПоследовательность(n: integer := 10; a: integer := 0; b: integer := 10): sequence of integer;
begin
  Result := ArrRandom(n,a,b)
end;

function НовыйФайл := new ФайлКласс;

var 
  ///- Исполнитель Множество. Предоставляет методы для множества целых
  Множество := new МножествоКласс;
  ///- Исполнитель МножествоСтрок. Предоставляет методы для множества строк
  МножествоСтрок := new МножествоСтрокКласс;
  ///- Исполнитель Вычислитель. Предоставляет ряд методов из области математики
  Вычислитель := new ВычислительКласс;
  ///- Исполнитель Файл. Предоставляет ряд методов для работы с файлами на диске
  Файл := new ФайлКласс;
  ///- Исполнитель Вывод. Предоставляет методы для вывода данных
  Вывод := new ВыводКласс;
  ///- Исполнитель МирИсполнителей. Содержит всех исполнителей 
  МирИсполнителей := new ВсеИсполнителиКласс;
begin  
  МирИсполнителей.Множество := Множество;
  МирИсполнителей.МножествоСтрок := МножествоСтрок;
  МирИсполнителей.Вычислитель := Вычислитель;
  МирИсполнителей.Файл := Файл;
  МирИсполнителей.Вывод := Вывод;
  InitCountries;
end.  
  