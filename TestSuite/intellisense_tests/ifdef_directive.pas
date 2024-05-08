procedure p1(b: byte);
begin
  {$ifdef def1}
  var x: integer;
  {$else}
  b{@parameter b: byte;@}.ToString;
  {$endif}
end;

begin 
end.