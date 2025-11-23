var l: List<integer>;

procedure p1<T>(f: ()->T);
begin
  f();
end;

begin
  p1(()->
  begin
    Result := new class(l := new List<integer>);
    
    loop 1 do // обязательно цикл loop for или foreach
    begin
      
      Result.l := new List<integer>;
      Result.l.Add(0);
      var o := Result.l;
      l := Result.l;
    end;
    
  end);
  assert(l[0] = 0);
end.