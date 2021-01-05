//#2199
type
  t0<T2> = class 
    static function p1 := 33;
  end;
  
  t2 = class(t0<integer>)
    procedure ppp;
    begin
      Assert(t0&<integer>.p1=33);
      Assert(p1=33); 
      Assert(t2.p1=33); 
      var p: ()->() := ()->
      begin
        Assert(t0&<integer>.p1=33);
        Assert(p1=33); 
        Assert(t2.p1=33); 
      end;
      p();
    end;
  end;
  
begin 
  t2.Create.ppp;
end.