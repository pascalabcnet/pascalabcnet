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
  var a := Arr(1, 9, 8, 7, 2, 3, 4, 5); 
  
  // Расширенный is
  if a is [1, .., var x, _, 5] then
    Println(x);
  
  // match .. with
  match a with
    [1, 9, 8, _, 2]: Print(1);
    [.., var y, var x]: Print(x + y);
    [_, .., _]: Print(3);
  end;
end.