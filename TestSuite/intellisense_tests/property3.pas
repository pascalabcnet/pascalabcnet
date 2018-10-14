type
  T2 = class
    i: integer;
  end;
  
  T = class
    function Get(i: byte) := default(byte);
    
    property X[i: T2]: T2{@class T2@} read Get;
    property X2: T2{@class T2@} read Get;
  end;
  
begin
  var o:= new T;
end.