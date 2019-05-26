type
  CardInfo = auto class
  public 
    cardNumber: string;
    cv: integer;
  end;
  
  Person = class
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
  var a := new Person('Вася', 11, new CardInfo('12345678', 324));
  
  // Расширенный is
  if a is Person(_, var age, CardInfo(_, var cv)) then assert((age = 11) and (cv = 324));
  
  // match .. with
  match a with
    Person('Петя', 12, CardInfo('12345678', var cv)): assert(false); 
    Person('Вася', _, CardInfo(cardNum, 324)): assert(cardNum.Equals('12345678'));
    Person(_, _, CardInfo(_, x)): assert(false);
  end;
end.