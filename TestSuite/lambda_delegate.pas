procedure p0(l: System.Delegate) := l.DynamicInvoke();

begin
  var i := 0;
  p0(()->
  begin
    i := 2;  
    Result := true;
    
  end);
  assert(i = 2);
end.