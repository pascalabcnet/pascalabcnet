unit unit1;

interface

type
  t2 = class
    private class i: integer;
  end;
  
procedure p1;

implementation

type
  t1 = class
    private class i: integer;
  end;

procedure p1;
begin
  t1.i{@var t1.i: integer;@} := 1;
  var j{@var j: integer;@} := t1.i;
  var k{@var k: integer;@} := t2.i;
end;

end.