unit u_attributes1;
uses System;

type

[AttributeUsage(AttributeTargets.All, AllowMultiple=false)]
MyAllAttr = class(Attribute)
end;
 
[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
MyClassAttr = class(Attribute)
a : integer;
public fA : string;
public property Prop : integer read a write a;
end;

[AttributeUsage(AttributeTargets.Delegate, AllowMultiple=true)]
MyDelegateAttr = class(Attribute)
a : integer;
public fA : string;
public property Prop : integer read a write a;
end;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple=true)]
MyStructAttr = class(Attribute)
a : integer;
public fA : string;
public property Prop : integer read a write a;
end;

[AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
MyProcAttr = class(Attribute)
private a, b : integer;
constructor (a, b : integer);
begin
  self.a := a;
  self.b := b;
end;
end;

[AttributeUsage(AttributeTargets.Field, AllowMultiple=true)]
MyFieldAttr = class(Attribute)
a : integer;
public fA : string;
public property Prop : integer read a write a;
end;

[AttributeUsage(AttributeTargets.Constructor, AllowMultiple=true)]
MyConstructorAttr = class(Attribute)
a : integer;
public fA : string;
public property Prop : integer read a write a;
end;

[AttributeUsage(AttributeTargets.Enum, AllowMultiple=true)]
MyEnumAttr = class(Attribute)
a : integer;
public fA : string;
public property Prop : integer read a write a;
end;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple=true)]
MyParamAttr = class(Attribute)
a : integer;
public fA : string;
public property Prop : integer read a write a;
end;

[AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
MyPropertyAttr = class(Attribute)
a : integer;
constructor Create(x : integer);
begin
  
end;
public fA : string;
public property Prop : integer read a write a;
end;

[MyAllAttr]
[MyStructAttr, MyStructAttr]
TRec = record
end;

[MyEnumAttr]
TColor = (red, green, blue);

[MyAllAttr, MyDelegateAttr]
TFunc = function(x : real):real;

[MyClassAttr, MyAllAttr]
TestClass = class
[MyFieldAttr(fA='abcd',Prop=34)]
fld : integer;
ch : char;

[MyPropertyAttr(2,Prop=32,fA='aa')]
property MyProp : char read ch;

[MyConstructorAttr(fA='abcd',Prop=34)]
constructor Create;
begin
  
end;
[MyProcAttr(23,45)]
procedure Proc([MyParamAttr(fA='xyz')]a : char);
begin
  
end;
end;

begin

end.