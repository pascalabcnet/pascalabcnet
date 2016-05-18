///- Модуль Исполнители
/// Описывает ряд Исполнителей, которые могут выполнять команды
unit Исполнители;

///- Процедура Вывод(значения)
/// Выводит список значений
procedure Вывод(params args: array of object) := Println(args);
/// Выводит пустую строку
procedure ВыводПустойСтроки := Println;

type
  целое = integer;
  вещественное = real;
  логическое = boolean;
  строковое = string;
  
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
  end;
  
  ВычислительТип = interface
  ///- Метод Вычислитель.КвадратноеУравнение(a,b,c: вещественное)
  /// Выводит все решения квадратного уравнения
    procedure КвадратноеУравнение(a,b,c: вещественное);
  ///- Метод Вычислитель.АрифметическаяПрогрессия(a0,d: целое)
  /// Выводит арифметическую прогрессию
    procedure АрифметическаяПрогрессия(a0,d: integer);
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
  end;
  
type
  ВычислительКласс = class(ВычислительТип)
  public
    procedure КвадратноеУравнение(a,b,c: real);
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
    procedure АрифметическаяПрогрессия(a0,d: integer);
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
        Println('Файл уже закрыт')
      else f.Close;
      State := FileState.Closed;
    end;
    procedure Записать(строка: строковое);
    begin
      if State=FileState.Closed then
        Println('Перед записью файл следует открыть')
      else f.Writeln(строка)
    end;
    function ПрочитатьСтроку: строковое;
    begin
      if State=FileState.Closed then
      begin
        Println('Перед чтением файл следует открыть');
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
        Println('Перед чтением файл надо открыть');
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
        Println('Файл закрыт, поэтому он не имеет имени');
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
        Println('Содержимое можно вывести только у закрытого файла')
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
  end;

  
var 
  ///- Множество - это набор значений
  Множество: МножествоТип := new МножествоКласс;
  ///- Множество - это набор значений
  Множество1: МножествоТип := new МножествоКласс;
  ///- Вычислитель - исполнитель, производящий математические вычисления
  Вычислитель: ВычислительТип := new ВычислительКласс;
  ///- Файл - исполнитель, считывающий из и записывающий в файл на диске
  Файл: ФайлТип := new ФайлКласс;
end.  
  