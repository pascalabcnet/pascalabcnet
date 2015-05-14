unit gen_unit;

interface

uses System.Collections.Generic;

type
  ca = class
    i: integer;
    constructor(w: integer);
    begin
      i := w;
    end;
  end;
    
procedure gen_exec;

implementation

procedure gen_exec;
var
  a, b: List<ca>;
  c: integer;
  x: ca;
  k: integer;
begin
  a := new List<ca>;
  a.Add(new ca(7));
  c := a.Count;
  writeln(a.Count);
  writeln(a[0]);
  writeln(a[0].i);
  b := new List<ca>;
  b.Add(new ca(8));
  b.Add(new ca(9));
  a.AddRange(b);
  for k := 0 to a.Count - 1 do
    writeln(a[k].i);
end;

end.