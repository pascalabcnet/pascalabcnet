unit u_arrays5;
type TClass = class
a : array[1..5] of integer;
b : array of real;
procedure Test;
procedure Test2(var a : array of integer);
end;

procedure TClass.Test;
var arr : array of integer;
procedure Nested;
begin
 SetLength(arr,5);
 arr[2] := 100;
 assert(arr[2] = 100);
end;

begin
 Nested;
end;

procedure TClass.Test2(var a : array of integer);
begin
 SetLength(a,10);
 a[2] := 45;
 assert(a[2] = 45);
end;


var t : TClass;
    a : array of integer;
    
begin
 t := new TClass();
 t.Test;
 t.Test2(a);
 assert(a[2] = 45);
end.