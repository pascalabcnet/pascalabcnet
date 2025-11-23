type
  i1 = interface
    
    function GetHashCode(x: byte): byte;
    
  end;
  
  t1 = class(i1)
    public function GetHashCode(x: byte) := x;
  end;
  
procedure p1<T>(o: T); where T: I1;
begin
  assert(o.GetHashCode(5) = 5);
  assert(o.GetHashCode() = o.GetHashCode());
end;

begin 
  p1&<I1>(new t1);
end.