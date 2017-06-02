// Пример использования классового (статического) конструктора
type 
  Person = class
  private
    class arr: array of Person; // Классовое поле. Связано не с переменной класса, а с классом. 
    name: string;
    age: integer;
  public
    class constructor; // Конструктор класса. Вызывается до создания первого объекта класса и до вызова любого классового метода
    begin
      writeln('  Вызван классовый конструктор');
      SetLength(arr,3);
      arr[0] := new Person('Иванов',20);
      arr[1] := new Person('Петрова',19);
      arr[2] := new Person('Попов',35);
    end;
    constructor (n: string; a: integer);
    begin
      name := n;
      age := a;
    end;
    function ToString: string; override;
    begin
      Result := Format('Имя: {0}   Возраст: {1}',name,age);      
    end;
    class function RandomPerson: Person; // Классовый метод. Может обращаться только к классовым полям
    begin
      Result := arr[Random(3)];
    end;
  end;
  
begin
  writeln('Случайные персоны');
  for var i:=1 to 5 do
    writeln(Person.RandomPerson); // Вызов классового метода
end.  