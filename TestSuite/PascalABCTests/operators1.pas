var i: integer;

type TRec<T> = record
  a, b: integer;
  class function operator=(a, b: TRec<T>): boolean;
  begin
    Result := false;
    i := 2;
  end;
  
end;

TRec2 = record
  a, b: integer;
  class function operator=(a, b: TRec2): boolean;
  begin
    Result := false;
    i := 3;
  end;
  
end;
begin
  var r1, r2: TRec<integer>;
  assert(not (r1 = r2));
  assert(i = 2);
  var r3, r4: TRec2;
  assert(not (r3 = r4));
  assert(i = 3);
  assert(not (r3 <> r4));
end.