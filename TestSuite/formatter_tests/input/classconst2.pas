type
  t1 = class
    const a = sin(1);
    class function test: real;
    begin
      Result := a;
    end;
  end;
 
begin
  assert(abs(t1.test-sin(1)) < 0.000001);
  assert(abs(t1.a-sin(1)) < 0.000001);
end.