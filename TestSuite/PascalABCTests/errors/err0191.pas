uses System;

type 
[AttributeUsage('', AllowMultiple=true)]
MyClassAttr = class(Attribute)
a : integer;
public fA : string;
public property Prop : integer read a write a;
end;


begin
  
end.