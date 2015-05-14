var r: real;
begin
  if real.IsNaN(r) then
    writeln(1);
  r:=real.NaN;
  if r<>r then
    writeln('NaN');
end.