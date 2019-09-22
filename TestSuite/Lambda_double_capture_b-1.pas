type
  t0<T> = class end;
  t1<T> = class(t0<T>) 
    //fld: integer;
    public procedure p1();
    begin
      var b: integer;
      var bw: T;
      var p: procedure := ()->
      begin
        var b1 := b; 
        var b2 := b; 
        //var f := fld;
      end;
    end;
  end;

begin 
end.