uses System;

type 
MyClass = class
end;

[MyClass]
MyClassAttr = class
a : integer;
public fA : string;
public property Prop : integer read a write a;
end;


begin
  
end.