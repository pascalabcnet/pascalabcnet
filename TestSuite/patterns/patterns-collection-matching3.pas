type
  CardInfo = auto class
  public 
    cardNumber: string;
    cv: integer;
  end;
  
  Person = class
    name: string;
    age: integer;
    card: List<CardInfo> := new List<CardInfo>();
    
    constructor(name: string; age: integer; card: List<CardInfo>);
    begin
      self.name := name;
      self.age := age;
      self.card := card;
    end;
    
    procedure Deconstruct(var name: string; var age: integer; var card: List<CardInfo>);
    begin
      name := self.name;
      age := self.age;
      card := self.card;
    end;
  end;

begin
  var cards := new List<CardInfo>();
  cards.Add(new CardInfo('12345671', 321));
  cards.Add(new CardInfo('12345672', 322));
  cards.Add(new CardInfo('12345673', 323));
  cards.Add(new CardInfo('12345674', 324));
  
  var p := new Person('Вася', 21, cards);
  
  // match .. with
  match p with
    Person('Петя', _, _): assert(false);
    Person('Вася', _, [_, _, var x]): assert(false);
    Person(name, _, [CardInfo(_, 321), .., CardInfo(_, 324)]): assert(name.Equals('Вася'));
  end;
end.