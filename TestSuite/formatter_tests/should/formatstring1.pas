type
  TRec = record
    a: integer;
  end;

begin
  var x: TRec;
  x.a := 2;
  var s := $'Hello {x.a}';
  assert(s = 'Hello 2');
end.