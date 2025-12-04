// Описание методов вне интерфейса класса
// Удобство: интерфейс лучше виден
// Если класс описан в модуле, то реализация методов помещается в секцию реализации модуля
type 
  Person = class
  private
    name: string;
    age: integer;
  public
    constructor (n: string; a: integer);
    procedure Print;
  end;

//----------------- Person -------------------
constructor Person.Create(n: string; a: integer);
begin
  name := n; age := a;
end;

procedure Person.Print;
begin
  writeln('Имя: ',name,'  Возраст: ',age);
end;

var p: Person;

begin
  p := new Person('Иванов',20);
  p.Print;
end.