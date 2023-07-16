uses System;

type 
  [AttributeUsage(AttributeTargets.Field or AttributeTargets.Property) ]
  myattr = class (Attribute) end;
  
  TClass = class
    [myattr]
    a: integer;
    
    [myattr]
    property prop: integer read a;
    
  end;
  
begin
end.