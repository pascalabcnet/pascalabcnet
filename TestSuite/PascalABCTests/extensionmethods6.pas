uses System, System.Collections.Generic;

var r: Random;
var ir: IReadOnlyCollection<integer>;
procedure p(x: integer);
begin
  assert(x=3);
end;

procedure IEnumerable<T>.For_Each1(p: System.Action<T>);
begin
  foreach x: T in Self do
    p(x);
end;

function IEnumerable<T>.Print1: IEnumerable<T>;
begin
  foreach x: T in Self do;
    Result := Self;  
end;

begin
  var a: array of integer := new integer[3](3,3,3);
  var pp: System.Action<integer> := p;
  a.For_Each1(p);
  a.For_Each1(pp);
  a.Print1;
  
end.