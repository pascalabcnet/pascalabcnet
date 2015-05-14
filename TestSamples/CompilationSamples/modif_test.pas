uses modif_test_unit;


type  
c=class
  private
  pp:integer;
  procedure p; begin end;
  
  end;{}
c1=class(c)
  public
  //procedure p;override; begin writeln('cc');  end;
  constructor create;begin end;
  end;
  
var cc:c1;
begin
  cc:=new c1;
  //cc.p;//error
  //writeln(cc.pp);
//  writeln(cc.p);//error
  readln;
end.