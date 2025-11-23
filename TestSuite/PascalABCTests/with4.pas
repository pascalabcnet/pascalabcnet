 type
  TRec = record
    b: integer;
  end;
  
begin
  var a: array [0..1,0..1] of TRec;
  with a[0,0] do
  begin  
    b := 2;
  end;  

  assert(a[0,0].b = 2);
end.