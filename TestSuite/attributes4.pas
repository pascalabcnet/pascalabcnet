uses System;

 type 
  AttributeParameter = class end;

  attrAttribute =  class(Attribute)
    constructor (params p1: array of AttributeParameter);
    begin end;
  end;
  
  c1 = class
    [attr]
    f1: integer;
  end;

begin
  var v1:= new c1;
  // Регрессия: тип параметра конструктора атрибута не должен влиять на тип f1.
  v1.f1 += 1;
  assert(v1.f1 = 1);
end.
