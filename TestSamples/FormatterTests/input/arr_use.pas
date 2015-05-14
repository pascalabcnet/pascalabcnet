
unit arr_use;

interface

type arr2=array[1..10] of integer;

procedure pr(a:arr2);

implementation

procedure pr(a:arr2);
var
i:integer;
begin
for i:=1 to 10 do
begin
 writeln(a[i]);
end;
end;

end.