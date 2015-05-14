uses compiled_static_property_as_constant_unit;
 
const c=system.drawing.color.red;
      c1=c;
var v:=system.drawing.color.green;

type cl=class
  pv:=system.drawing.color.black;
end;

procedure p;
const pc=system.drawing.color.blue;
var pv:=system.drawing.color.yellow;
begin
  writeln('>>local');
  writeln(pc);
  writeln(pv);
end;

begin
  writeln('>>global');
  writeln(c);
  writeln(v);
  p;
  writeln('>>class field');
  writeln((new cl).pv);
  writeln('>>in unit');
  writeln(compiled_static_property_as_constant_unit.c);
  readln;
end.