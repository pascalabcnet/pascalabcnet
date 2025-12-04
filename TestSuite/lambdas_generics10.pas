type
  
  t0<T> = class
  where T: t0<T>;
    b: boolean;
    
    static procedure p1(a: T);
    begin
      var f: Func<T, boolean> := o -> o.b;
      assert(f(a));
    end;
    
    static procedure p2(a: T);
    begin
      assert(a.b);
    end;
  end;
  
  t1 = class(t0<t1>) end;
  
begin
  var a := new t1;
  a.b := true;
  t1.p1(a);
  t1.p2(a);
end.