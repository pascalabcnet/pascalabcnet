var i: integer;
type
  t1<T>=class
    
    procedure p1(a:byte; b:word);
    begin
      i := 2;
    end;
    
  end;

procedure p1<T>(self:t1<T>;c:single); extensionmethod;
begin
  i := 3;
end;

begin 
  var o: t1<integer> := new t1<integer>;
  o.p1(2,3);
  assert(i = 2);
  o.p1(4);
  assert(i = 3);
end.