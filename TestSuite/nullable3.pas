begin
  var a := new System.Nullable<integer>[1,1];
  a[0,0] := 1;
  assert(a[0,0] = 1);
  a[0,0] := nil;
  assert(a[0,0] <> 0);
  assert(a[0,0] = nil);
  var a2 : array of array of System.Nullable<integer>;
  SetLength(a2,1);
  SetLength(a2[0],1);
  a2[0,0] := nil;
  assert(a2[0,0] <> 0);
  assert(a2[0,0] = nil);
end.