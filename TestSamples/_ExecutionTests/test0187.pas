
type TRec = record
  a: integer;
  constructor(a: integer);
  begin
    self.a := a;
  end;
end;
  
begin
  assert((new TRec(5)).a = 5);
end.