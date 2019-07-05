begin

match Arr(1,2,3) with
  [1,2]: 
    match Arr(1,2,3,4) with
      [1,2,3]: assert(false);
      [.., 4]: 
        match Arr(1,2) with
          [1,2]: ;
        end;
    end;
  [1, .., 3]:
    begin
    if 1 is integer(var i1) then
      if 2 is integer(var i2) then
        if 3 is integer(var i3) then
          assert(i3 = 3)
        else
          assert(false)
      else
        assert(false);
    end;
  [..]:
  else
    assert(false);
end;

match Arr(1,2,3) with
  [1,2]: 
    match Arr(1,2,3,4) with
      [1,2,3]: assert(false);
      [.., 4]: 
        match Arr(1,2) with
          [1,2]: ;
        end;
    end;
  [1, .., 3]:
    begin
    if 1 is integer(var i1) then
      if 2 is integer(var i2) then
        if 3 is integer(var i3) then
          assert(i3 = 3)
        else
          assert(false)
      else
        assert(false);
    end;
  [..]:
  else
    assert(false);
end;
end.