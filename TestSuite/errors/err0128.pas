uses GraphABC;
type TRec2 = record
x : integer;
end;

type TRec = record
a : integer;
b : TRec2;
end;

var 
s : TRec := (a:2;b:4);
begin
end.