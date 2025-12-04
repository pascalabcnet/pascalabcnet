type
  TRec = record
    a: integer;
  end;
  
  cls = record
    class i: integer := 3;
    class obj: object := new object;
    class rec: TRec;
  end;
    
begin
  assert(cls.i = 3);
  assert(cls.obj <> nil);
  with cls do
  begin
    i := 2;
    assert(i = 2);
    rec := new TRec;
    rec.a := 2;
    assert(rec.a = 2);
  end;
end.