//!semantic
type
  TClass = class
    fX: byte;
    
    property X: byte read 0;
    
    function F() := 0;
  end;

begin
  var instance := new TClass();
  var x1{@var x1: Nullable<byte>;@} := instance?.fX;
  var x3{@var x3: Nullable<integer>;@} := instance?.F;
  writeln(instance?.F{@function TClass.F(): integer;@});
end.