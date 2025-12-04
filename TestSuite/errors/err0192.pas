uses System;

type 
[AttributeUsage(AttributeTargets.Class, AllowMultiple2=true)]
MyClassAttr = class(Attribute)
a : integer;
public fA : string;
public property Prop : integer read a write a;
end;


begin
  
end.