// Наследование. Полиморфизм и виртуальные функции
type 
  Person = class
  private
    name: string;
    age: integer;
  public
    constructor (n: string; a: integer);
    begin
      name := n; age := a;
    end;
    procedure Print; virtual; // Виртуальная функция. Переопределяется в классах-потомках
    begin
      var s := GetType.ToString;
      write('Тип: ',Copy(s,pos('.',s)+1,Length(s)):7,'   Имя: ',name,'   Возраст: ',age);
    end;
    procedure Println;
    begin
      Print;
      writeln;
    end;
  end;
  
  Pupil = class(Person) // Pupil - наследник Person
  private
    clas: integer;
  public
    constructor (n: string; a,c: integer);
    begin
      inherited Create(n,a); // Вызов унаследованного конструктора
      clas := c;
    end;
    procedure Print; override;
    begin
      inherited Print;
      write('   Класс: ',clas);
    end;
  end;
  
  Teacher = class(Person)
  private
    predm: string;
  public
    constructor (n: string; a: integer; p: string);
    begin
      inherited Create(n,a); // Вызов унаследованного конструктора
      predm := p;
    end;
    procedure Print; override;
    begin
      inherited Print;
      write('   Предмет: ',predm);
    end;
  end;
  
var a: array of Person := new Person[4]; // Полиморфный контейнер - контейнер объектов базового класса. Может содержать объекты производных классов

begin
  a[0] := new Pupil('Вова',11,5);
  a[1] := new Teacher('Марья Ивановна',30,'Информатика');
  a[2] := new Person('Иванов',65);
  a[3] := new Pupil('Вася',12,6);
  
  for var i:=0 to a.Length-1 do
    a[i].Println;
end.