uses
  System,
  System.Linq;

var j: integer;
 
procedure MyPrint(i:Int64); overload;
begin
  j := 1;
end;

procedure MyPrint(i:Integer); overload;
begin
  j := 2;
end;

begin
  Enumerable.Range(1, 10).ForEach(MyPrint);
  assert(j = 2);
end.