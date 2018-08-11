type
  t1 = class
    
    _p1 := 0;
    
    property P1:integer read _p1 write _p1; virtual;
    
    procedure pr1;
    begin
      var p:procedure := ()->begin
        P1 := P1 + 1;
      end;
      p;
    end;
    
  end;

begin 
  var o := new t1;
  o.pr1;
  assert(o.P1 = 1);
end.