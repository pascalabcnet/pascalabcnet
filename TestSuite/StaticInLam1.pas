//#2199 - частично
type
  t0<T2> = class 
  end;
  
  t2 = class(t0<integer>)
    static function p1 := 33;
    procedure ppp;
    begin
      Assert(p1=33); 
      Assert(t2.p1=33); 
      //t0&<integer>.p1;
      {var p: ()->() := ()->
      begin
      end;}
    end;
  end;
  
begin 
  t2.Create.ppp;
end.