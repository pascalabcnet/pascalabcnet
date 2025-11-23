type
  t0<T> = class end;
  t1<T> = class(t0<T>) 
    public procedure p1();
    begin
      var b: T;
      var p: procedure := ()->
      begin
        var b1 := b; 
        var b2 := b; 
      end;
    end;
  end;

begin 
end.