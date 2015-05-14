const max:byte = 4;

var 
  a: shortint;
  b: longword;
  c : uint64;
  d3 : 1..max;
begin
  writeln((a + b).Gettype);
  writeln((a + c).GetType);
end.