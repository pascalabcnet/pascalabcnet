///- Модуль Исполнители
/// Описывает ряд Исполнителей, которые могут выполнять команды
unit Исполнители;

///- Процедура Вывод(значения)
/// Выводит список значений
procedure Вывести(params args: array of object) := Println(args);
/// Выводит пустую строку
procedure ВывестиПустуюСтроку := Println;

///- Метод Вывести
procedure integer.Вывести := Println(Self);
procedure real.Вывести := Println(Self);
procedure string.Вывести := Println(Self);


type
  целое = integer;
  вещественное = real;
  логическое = boolean;
  строковое = string;
  
  ВыводТип = interface
    procedure Данные(params args: array of object);
    procedure ПустаяСтрока;
  end;

  ВыводКласс = class(ВыводТип)
  public
    procedure Данные(params args: array of object) := Println(args);
    procedure ПустаяСтрока := Println;
  end;
  
  МножествоТип = interface
    ///- Метод Множество.Добавить(элемент: целое)
    /// Добавляет элемент к множеству. Если такой элемент есть, то ничего не происходит
    procedure Добавить(элемент: целое);
    ///- Метод Множество.Удалить(элемент: целое)
    /// Удаляет элемент из множества. Если такого элемента нет, то ничего не происходит
    procedure Удалить(элемент: целое);
    ///- Метод Множество.Вывести
    /// Выводит элементы множества
    procedure Вывести;
    ///- Метод Множество.Содержит(элемент: целое): логический
    /// Проверяет, есть ли элемент во множестве
    function Содержит(элемент: целое): логическое;
    ///- Метод Множество.ВывестиВсеМетоды
    /// Выводит все методы Исполнителя Множество
    procedure ВывестиВсеМетоды;
    ///- Метод Множество.Очистить
    /// Делает множество пустым
    procedure Очистить;
    ///- Метод Множество.Новое
    /// Создает новое множество
    function Новое: МножествоТип;
  end;
  
  МножествоСтрокТип = interface
    ///- Метод Множество.Добавить(элемент: строковое)
    /// Добавляет элемент к множеству. Если такой элемент есть, то ничего не происходит
    procedure Добавить(элемент: string);
    ///- Метод Множество.Удалить(элемент: строковое)
    /// Удаляет элемент из множества. Если такого элемента нет, то ничего не происходит
    procedure Удалить(элемент: string);
    ///- Метод Множество.Вывести
    /// Выводит элементы множества
    procedure Вывести;
    ///- Метод Множество.Содержит(элемент: строковое): логический
    /// Проверяет, есть ли элемент во множестве
    function Содержит(элемент: string): логическое;
    ///- Метод Множество.ВывестиВсеМетоды
    /// Выводит все методы Исполнителя Множество
    procedure ВывестиВсеМетоды;
    ///- Метод Множество.Очистить
    /// Делает множество пустым
    procedure Очистить;
    ///- Метод Множество.Новое
    /// Создает новое множество
    function Новое: МножествоСтрокТип;
    ///- Метод Множество.СоздатьПары(множество1)
    /// Создает пары элементов из двух множеств
    function СоздатьПары(мн1: МножествоСтрокТип): МножествоСтрокТип;
  end;

  ВычислительТип = interface
  ///- Метод Вычислитель.КвадратноеУравнение(a,b,c: вещественное)
  /// Выводит все решения квадратного уравнения
    procedure РешитьКвадратноеУравнение(a,b,c: вещественное);
  ///- Метод Вычислитель.АрифметическаяПрогрессия(a0,d: целое)
  /// Выводит арифметическую прогрессию
    procedure ВывестиАрифметическуюПрогрессию(a0,d: integer);
    procedure ВывестиВсеМетоды;
  end;
  
  ФайлТип = interface
    procedure ОткрытьНаЧтение(имя: строковое);
    procedure ОткрытьНаЗапись(имя: строковое);
    procedure Закрыть;
    procedure Записать(строка: строковое);
    function ПрочитатьСтроку: строковое;
    function ПрочитатьЦелое: целое;
    function КонецФайла: логическое;
    function ИмяФайла: строковое;
    procedure ВывестиСодержимое(имяфайла: строковое);
    procedure ВывестиВсеМетоды;
    function Новый: ФайлТип;
    function ВсеСтроки(имяфайла: строковое): sequence of строковое;
  end;
  
  ПоследовательностьТип = interface
    
  end;


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
  МножествоКласс = class(МножествоТип)
    s := new SortedSet<integer>;
  public
    constructor;
    begin end;
    procedure Добавить(элемент: целое);
    begin
      s.Add(элемент);
    end;
    procedure Удалить(элемент: целое);
    begin
      s.Remove(элемент);
    end;
    procedure Вывести;
    begin
      s.Println;
    end;
    function Содержит(элемент: целое): логическое;
    begin
      Result := s.Contains(элемент)
    end;
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
    procedure Очистить;
    begin
      s.Clear
    end;
    function Новое: МножествоТип;
    begin
      Result := new МножествоКласс();
    end;
  end;
  
type
  МножествоСтрокКласс = class(МножествоСтрокТип)
    s := new SortedSet<string>;
  public
    constructor;
    begin end;
    procedure Добавить(элемент: string);
    begin
      s.Add(элемент);
    end;
    procedure Удалить(элемент: string);
    begin
      s.Remove(элемент);
    end;
    procedure Вывести;
    begin
      s.Println;
    end;
    function Содержит(элемент: string): логическое;
    begin
      Result := s.Contains(элемент)
    end;
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
    procedure Очистить;
    begin
      s.Clear
    end;
    function Новое: МножествоСтрокТип;
    begin
      Result := new МножествоСтрокКласс
    end;
    function СоздатьПары(мн1: МножествоСтрокТип): МножествоСтрокТип;
    begin
      var ss: SortedSet<string>;
      ss := (мн1 as МножествоСтрокКласс).s;
      
      var m := new МножествоСтрокКласс;
      m.s := s.ZipTuple(ss).Select(x -> x.ToString()).ToSortedSet;
      Result := m
    end;
  end;
  
type
  ВычислительКласс = class(ВычислительТип)
  public
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
  ФайлКласс = class(ФайлТип)
    f: Text;
    State := FileState.Closed;
  public
    constructor ;
    begin
    end;
    procedure ОткрытьНаЧтение(имя: строковое);
    begin
      if State<>FileState.Closed then
        f.Close;
      f := OpenRead(имя);
      State := FileState.OpenedForRead
    end;
    procedure ОткрытьНаЗапись(имя: строковое);
    begin
      if State<>FileState.Closed then
        f.Close;
      f := OpenWrite(имя);
      State := FileState.OpenedForWrite
    end;
    procedure Закрыть;
    begin
      if State=FileState.Closed then
        Println('Ошибка: Файл уже закрыт')
      else f.Close;
      State := FileState.Closed;
    end;
    procedure Записать(строка: строковое);
    begin
      if State=FileState.Closed then
        Println('Ошибка: Перед записью файл следует открыть')
      else f.Writeln(строка)
    end;
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
    function КонецФайла: логическое;
    begin
      Result := f.Eof;
    end;
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
    procedure ВывестиВсеМетоды;
    begin
      PrintAllMethods(Self);
    end;
    function Новый: ФайлТип;
    begin
      Result := new ФайлКласс
    end;
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

function Вывести<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  Self.Println
end;

function ВывестиПострочно<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  Self.Println(NewLine)
end;

function Выбрать<T>(Self: sequence of T; cond: T -> boolean): sequence of T; extensionmethod;
begin
  Result := Self.Where(cond);  
end;

function Взять<T>(Self: sequence of T; n: integer): sequence of T; extensionmethod;
begin
  Result := Self.Take(n);  
end;

function Количество<T>(Self: sequence of T; cond: T -> boolean): integer; extensionmethod;
begin
  Result := Self.Count(cond)
end;

function Сумма(Self: sequence of integer): integer; extensionmethod := Self.Sum();  

function Преобразовать<T,Key>(Self: sequence of T; conv: T -> Key): sequence of Key; extensionmethod;
begin
  Result := Self.Select(conv);  
end;

function ОтсортироватьПо<T,Key>(Self: sequence of T; cond: T -> Key): sequence of T; extensionmethod;
begin
  Result := Self.OrderBy(cond);  
end;

function ОтсортироватьПоУбыванию<T,Key>(Self: sequence of T; cond: T -> Key): sequence of T; extensionmethod;
begin
  Result := Self.OrderByDescending(cond);  
end;

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


type ВсеИсполнителиКласс = class
public
  Множество: МножествоТип;
  Вычислитель: ВычислительТип;
  Файл: ФайлТип;
  Вывод: ВыводТип;
  procedure Вывести;
  begin
    Println('Множество');
    Println('Вычислитель');
    Println('Файл');
    Println('Вывод');
  end;
end;

type 
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

function АрифметическаяПрогрессия(a,d: integer; n: integer := 20): sequence of integer;
begin
  Result := SeqGen(n,a,a->a+d)
end;

function АрифметическаяПрогрессия(a,d: real; n: integer := 20): sequence of real;
begin
  Result := SeqGen(n,a,a->a+d)
end;

function ГеометическаяПрогрессия(a,d: integer; n: integer := 10): sequence of integer;
begin
  Result := SeqGen(n,a,a->a*d)
end;

function ГеометическаяПрогрессия(a,d: real; n: integer := 10): sequence of real;
begin
  Result := SeqGen(n,a,a->a*d)
end;

function НовыйФайл: ФайлТип;
begin
  Result := new ФайлКласс;
end;

var 
  ///- Множество - это набор значений
  ВычислительМножество: МножествоТип := new МножествоКласс;
  ///- Множество - это набор значений
  Множество: МножествоТип := new МножествоКласс;
  ///- Множество - это набор значений
  Множество1: МножествоТип := new МножествоКласс;
  ///- Множество - это набор значений
  МножествоСтрок: МножествоСтрокТип := new МножествоСтрокКласс;
  ///- Вычислитель - исполнитель, производящий математические вычисления
  Вычислитель: ВычислительТип := new ВычислительКласс;
  ///- Файл - исполнитель, считывающий из и записывающий в файл на диске
  Файл: ФайлТип := new ФайлКласс;
  ///- Вывод - исполнитель, выводящий данные
  Вывод: ВыводТип := new ВыводКласс;
  ///- ВсеИсполнители
  ВсеИсполнители := new ВсеИсполнителиКласс;
begin  
  ВсеИсполнители.Множество := Множество;
  ВсеИсполнители.Вычислитель := Вычислитель;
  ВсеИсполнители.Файл := Файл;
  ВсеИсполнители.Вывод := Вывод;
  InitCountries;
end.  
  