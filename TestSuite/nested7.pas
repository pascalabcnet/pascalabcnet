type
  NewSet1<T> = class
  end;

procedure Test2;
  var b1 := new NewSet1<integer>;

  procedure Nested;
  begin
    assert(b1 <> nil);
  end;
    
begin
  Nested;
end;

begin
  Test2;
end.