type
  TBaseClass = abstract class
    property p1: byte read write; abstract;
  end;
  
  TClass = class(TBaseClass)
    b: byte;
    property p1: byte read b write b; override;
  end;
  
  TClass2 = class(TBaseClass)
    b: byte;
    function getb := b;
    procedure setb(value: byte) := b := value;
    property p1: byte read getb write setb; override;
  end;
  
begin 
  var o: TBaseClass := new TClass;
  o.p1 := 2;
  assert(o.p1 = 2);
  o := new TClass2;
  o.p1 := 2;
  assert(o.p1 = 2);
end.