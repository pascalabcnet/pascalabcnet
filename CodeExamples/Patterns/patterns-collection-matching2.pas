type
  CardInfo = auto class
  public 
    cardNumber: string;
    cv: integer;
  end;
  
  Person = class
  
  public 
    name: string;
    age: integer;
    card: CardInfo;
    
    constructor(name: string; age: integer; card: CardInfo);
    begin
      self.name := name;
      self.age := age;
      self.card := card;
    end;
    
    procedure Deconstruct(var name: string; var age: integer; var card: CardInfo);
    begin
      name := self.name;
      age := self.age;
      card := self.card;
    end;
  end;

begin
  var p1 := new Person('Вася', 11, new CardInfo('12345671', 321));
  var p2 := new Person('Петя', 12, new CardInfo('12345672', 322));
  var p3 := new Person('Маша', 13, new CardInfo('12345673', 323));
  var personArr := Arr(p1, p2, p3);
  
  // Расширенный is
  if personArr is [Person(name1, 11, CardInfo(_, cv)), _, Person(name2, 13, _)] then 
    println(name1, cv, name2);
  
  // match .. with
  match personArr with
    [_, _, Person('Вася', age, _)]: print(age);
    [var p, .., Person('Маша', _, _)]: print((p as Person).name);
    [..]: print(1);
  end;
end.