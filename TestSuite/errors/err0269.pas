type
  TClass = class 
    property X1: byte read write; abstract;
  end;

begin
  var o := new TClass;
end.