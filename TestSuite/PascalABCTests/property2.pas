type TClass<T> = class
a : array of T;
constructor Create(params arr : array of T);
begin
a := arr;
end;
function getItem(i : integer):T;
begin
Result := a[i];
end;
procedure setItem(i : integer; item: T);
begin
a[i] := item;
end;
property PropA[i : integer]: T read getItem write setItem; default;
property PropB[i : integer]: T read getItem write setItem;
end;

var t : TClass<real>;
    
begin
t := new TClass<real>(1.3,2,2.5);
assert(t[0] = 1.3);
t[0] := 4;
assert(t[0] = 4);
end.
