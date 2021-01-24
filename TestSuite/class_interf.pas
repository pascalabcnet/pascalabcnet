type
  i1 = interface 
    procedure p;
  end;
  
  c1 = abstract class(i1) 
  public
    procedure p;
    begin
      Assert(111=111);
    end;
  end;
  c2 = sealed class(c1) end;
  
begin
  var a := new c2 as i1;
  a.P
end.