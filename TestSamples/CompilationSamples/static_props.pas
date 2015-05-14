type TClass = class
class a : integer := 4;

class function getA (i : integer) : integer;
begin
end;

class procedure setA(i: integer; a : integer);
begin
end;

class property Prop1[i : integer] : integer read getA write setA;
end;

var t : TClass;
begin
TClass.Prop1[2] := 3;
end.