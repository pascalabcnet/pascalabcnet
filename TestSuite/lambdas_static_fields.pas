type
  t1<T>=class
    
    static p0: Action<T>;
    
    static procedure p1(p: Action<T>);
    begin
      t1&<T>.p0 := o1->p(o1);
    end;
    
  end;

var i: integer;

begin 
  t1&<integer>.p1(x->begin i := x; end);
  t1&<integer>.p0(1);
  assert(i = 1);
end.