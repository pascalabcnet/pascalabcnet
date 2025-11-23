unit u_foreach1;
type TClass = class(System.Collections.IEnumerable)
public function GetEnumerator : System.Collections.IEnumerator;
begin
 Result := nil;
end;
end;

var s : string;
    arr : array[1..3] of integer;
    arr2 : array of real;
    set1 : set of byte;
    lst : System.Collections.ArrayList;
    lst2 : System.Collections.Generic.List<integer>;
    obj : TClass;
    
begin
  s := 'ab';
  var i := 0;
  foreach c : char in s do
  begin
    if i = 0 then assert(c='a')
    else assert(c='b');
    Inc(i);
  end;
  i := 0;
  arr[1] := 1; arr[2] := 2; arr[3] := 3;
  foreach v : integer in arr do
  begin
   case i of
    0 : assert(v=1);
    1 : assert(v=2);
    2 : assert(v=3);
   end;
   Inc(i);
  end;
  i := 0;
  SetLength(arr2,2);
  arr2[0] := 3.14; arr2[1] := 2.71;
  foreach v : real in arr2 do
  begin
   case i of
    0 : assert(v=3.14);
    1 : assert(v=2.71);
   end;
   Inc(i);
  end;
  
end.