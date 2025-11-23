// Иллюстрация конструкторов и методов в записях
// Если переопределен метод ToString, то он вызывается при выводе объекта этого типа процедурой writeln 
type
  SexType = (Male, Female);
  Person = record
    Name: string;
    Age, Weight: integer;
    Sex: SexType;
    constructor (Name: string; Age, Weight: integer; Sex: SexType);
    begin
      Self.Name := Name;
      Self.Age := Age;
      Self.Sex := Sex;
      Self.Weight := Weight;
    end;
    function ToString: string; override;
    begin
      Result := Format('Имя: {0}   Пол: {1}   Возраст: {2}   Вес: {3}', Name, Sex, Age, Weight);
    end;
  end;
  
var p: Person := new Person('Иванов',20,70,SexType.Male);

begin
  writeln(p);
end.  
