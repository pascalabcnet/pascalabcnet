procedure Test(b1: byte; var b2: byte);
begin
  assert(b1 < b2);
end;
procedure Test(b1: shortint; var b2: shortint);
begin
  assert(b1 < b2);
end;
procedure Test(b1: word; var b2: word);
begin
  assert(b1 < b2);
end;
procedure Test(b1: longword; var b2: longword);
begin
  assert(b1 < b2);
end;
procedure Test(b1: uint64; var b2: uint64);
begin
  assert(b1 < b2);
end;
begin
  var b1, b2: byte;
  b1 := 254;
  b2 := 255;
  Test(b1, b2);
  var w1, w2: word;
  w1 := word.MaxValue - 1;
  w2 := word.MaxValue;
  Test(w1, w2);
  var s1, s2: word;
  s1 := shortint.MaxValue - 1;
  s2 := shortint.MaxValue;
  Test(s1, s2);
  var lw1, lw2: longword;
  lw1 := longword.MaxValue - 1;
  lw2 := longword.MaxValue;
  Test(lw1, lw2);
  var ui1, ui2: uint64;
  ui1 := uint64.MaxValue - 1;
  ui2 := uint64.MaxValue;
  Test(ui1, ui2);
  var arr: array of byte := (b1, b2);
  Test(arr[0], arr[1]);
  var arr2: array of word := (w1, w2);
  Test(arr2[0], arr2[1]);
  var arr3: array of longword := (lw1, lw2);
  Test(arr3[0], arr3[1]);
  var arr4: array of uint64 := (ui1, ui2);
  Test(arr4[0], arr4[1]);
  var arr5: array of shortint := (shortint.MaxValue-1, shortint.MaxValue);
  Test(arr5[0], arr5[1]);
end.