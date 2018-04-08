begin
  var i := 0;
  case $F0000000 of
    $F0000000: i := 1;
    else
      i := 2;
  end;
  assert(i = 1);
  i := 0;
  var ui64 := uint64.MaxValue;
  case ui64 of
    uint64.MaxValue: i := 1;
    -2: i := 3;
    else
      i := 2;
  end;
  assert(i = 1);
  i := 0;
  ui64 := 2;
  case ui64 of
    2: i := 1;
    else
      i := 2;
  end;
  assert(i = 1);
  i := 0;
  var i64 := int64.MaxValue;
  case i64 of
    int64.MaxValue: i := 1;
    uint64.MaxValue: i := 2;
  end;
  assert(i = 1);
  i := 0;
  i64 := 2;
  case i64 of
    2: i := 1;
    else
      i := 2;
  end;
  assert(i = 1);
  i64 := 100;
  i := 0;
  case i64 of
    5..int64.MaxValue: i := 1;
    1..4: i := 2;
  end;
  assert(i = 1);
  ui64 := 100;
  i := 0;
  case ui64 of
    5..uint64.MaxValue: i := 1;
    1..4: i := 2;
  end;
  assert(i = 1);
end.