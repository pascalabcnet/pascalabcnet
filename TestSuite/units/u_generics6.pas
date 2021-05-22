unit u_generics6;

var i: integer;
type
  r1 = record(System.ICloneable)
    public function Clone: object;
    begin
      i := 1;
    end;
  end;
  
procedure p1<T>(a: T); where T: System.ICloneable;
begin
  System.ICloneable(a).Clone;
end;

end.