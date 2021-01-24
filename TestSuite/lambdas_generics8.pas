type
  t0 = abstract class 
  end;
  
  t2 = class(t0)
  end;
  
  t1<T> = class
  where T: t0, constructor;
    public obj: T;
    procedure p1;
    begin
      var p: Action0 := ()->
      begin
        obj := new T;
      end;
      p();
    end;
    
  end;
  
begin 
  var o := new t1<t2>;
  o.p1;
  assert((o.obj <> nil) and (o.obj.GetType = typeof(t2)));
end.