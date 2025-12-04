type
  TBaseClass = abstract class
    property p1: byte read write; abstract;
    property p2: byte read; abstract;
  end;
  
  TClass = class(TBaseClass)
    b: byte;
    property p1: byte read b write b; override;
    property p2: byte read b; override;
  end;
  
  TClass2 = class(TBaseClass)
    b: byte;
    function getb := b;
    procedure setb(value: byte) := b := value;
    property p1: byte read getb write setb; override;
    property p2: byte read getb; override;
  end;
  
begin 
  var o: TBaseClass := new TClass;
  o.p1 := 2;
  assert(o.p1 = 2);
  assert(o.p2 = 2);
  o := new TClass2;
  o.p1 := 2;
  assert(o.p1 = 2);
  assert(o.p2 = 2);
end.