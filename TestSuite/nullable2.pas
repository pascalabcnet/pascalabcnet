function f: System.Nullable<integer>;
begin
  Result := 2;
end;
type TRec = record
i: integer;
end;

begin
  var t := System.Nullable&<integer>(2);
  assert(t <> 3);
  assert(t = 2);
  assert(f = 2);
  assert(f <> 3);
  var r, r2: TRec;
  r.i := 2;
  r2.i := 3;
  //assert(System.Nullable&<TRec>(r) = System.Nullable&<TRec>(r));
  //assert(System.Nullable&<TRec>(r) <> System.Nullable&<TRec>(r2));
end.