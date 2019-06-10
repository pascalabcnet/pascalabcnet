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
    assert(name1.Equals('Вася') and (cv = 321) and name2.Equals('Маша'));
  
  // match .. with
  match personArr with
    [_, _, Person('Вася', var age, _)]: assert(false);
    [var p, .., Person('Маша', _, _)]: assert((p as Person).name.Equals('Вася'));
    [..]: assert(false);
  end;
end.