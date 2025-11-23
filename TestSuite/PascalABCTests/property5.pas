type
  IInt = interface
    property p1: byte read write;
  end;
  
  TClass = class(IInt)
    b: byte;
    public property p1: byte read b write b;
  end;
  
  TClass2 = class(IInt)
    b: byte;
    function getb := b;
    procedure setb(value: byte) := b := value;
    public property p1: byte read getb write setb;
  end;
  
begin 
  var o: IInt := new TClass;
  o.p1 := 2;
  assert(o.p1 = 2);
  o := new TClass2;
  o.p1 := 2;
  assert(o.p1 = 2);
end.