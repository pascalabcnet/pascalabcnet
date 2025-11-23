// Свойства (properties)
type 
  /// Класс персоны
  Person = class
  private
    nm: string;
    ag: integer;
    procedure SetAge(a: integer); // Процедура доступа к свойству располагается обычно в приватной секции
    begin
      if a<0 then // Перед установкой значения свойства мы можем сделать дополнительные проверки 
        a := 0;   // и скорректировать значение свойства, либо сгенерировать исключение
      ag := a;  
    end;
  public
    constructor (n: string; a: integer);
    begin
      nm := n; ag := a;
    end;
    /// Имя персоны
    property Name: string read nm; // Свойство Name доступно только на чтение и возвращает значение поля nm
    /// Возраст персоны
    property Age: integer read ag write SetAge;
    procedure Print;
    begin
      writeln('Имя: ',nm,'  Возраст: ',ag);
    end;
  end;

var p: Person := new Person('Иванов', 20);

begin
  writeln('Имя: ',p.Name); // Менять Name нельзя, доступ - только на чтение
  p.Age := -1; // Попытка изменения возраста на отрицательное значение приводит к корректировке: возраст становится = 0
  writeln('Возраст: ',p.Age);
end.