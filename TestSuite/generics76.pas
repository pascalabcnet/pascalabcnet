var i := 0;
type
  i1 = interface
    property prop1: integer read;
  end;
  c1<T> = class
  // Обязательно любое ограничение where
  where T: System.ICloneable;
    
    constructor(o: T);
    begin
      i := i1(o).prop1; // Выводит мусор
    end;
    
  end;
  r1 = record(i1, System.ICloneable)
    s: string;
    constructor(s: string) := self.s := s;
    property prop1: integer read s.Length; // Обязательно использовать какие-то данные из self
    function Clone: object := self;
  end;
  
begin
  new c1<r1>(new r1('abc'));
  assert(i = 3);
end.