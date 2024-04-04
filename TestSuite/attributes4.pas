uses System;

 type 
  attrAttribute =  class(Attribute)
    constructor (params p1: array of System.Activator);
    begin end;
  end;
  
  c1 = class
    [attr]
    f1: integer;
  end;

begin
  var v1:= new c1;
  // ---> Операция '+=' не применима к типам array of Activator и integer <---
  v1.f1 += 1;
  assert(v1.f1 = 1);
end.