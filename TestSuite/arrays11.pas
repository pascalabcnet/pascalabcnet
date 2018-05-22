type TRec = record
  k: array[0..1, 0..1] of integer;
end;
var
  k: array[0..1, 0..1] of integer;
  a: array[0..10] of integer;
  a2: array[1..2] of TRec;
  
begin
  a[5] := 1; 
  k[0, 1] := 100;
  k[a[5] mod 2][1] += 1;
  assert(k[a[5], 1] = 1);
  k[a[5] mod 2, 1] += 1;
  assert(k[a[5], 1] = 2);
  k[0, a[a[1] + 4] mod 2] += 1;
  assert(k[0, a[4] mod 2] = 1);
  a2[a[5]].k[a[5] mod 2,1] += 1;
  assert(a2[a[5]].k[a[5] mod 2,1] = 1);
end.