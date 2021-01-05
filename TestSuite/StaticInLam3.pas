//#2199
type
  t0<TTTT1,TTTT2> = class 
    static function p1 := 33;
  end;
  
  TGen1 = class end;
  TGen2 = class end;
  t2 = class(t0<TGen1,TGen2>)
    constructor;
    begin
      Assert(t0&<integer,integer>.p1=33);
      Assert(p1=33); 
      Assert(t2.p1=33); 
      var p: ()->() := ()->
      begin
        Assert(t0&<integer,integer>.p1=33);
        Assert(p1=33); 
        Assert(t2.p1=33); 
      end;
      p();
    end;
  end;
  
begin 
  t2.Create;
end.