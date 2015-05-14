uses visible_test_unit;

type
  c2=class(c1)
      c2internal_var:integer;
      procedure c2internal_proc;
      begin
      end;
    public
      c2public_var:integer;
      procedure c2public_proc;
      begin
      end;
    protected
      c2protected_var:integer;
      procedure c2protected_proc;
      begin
      end;
    private
      c2private_var:integer;
      procedure c2private_proc;
      begin
        c1protected_var:=0;
        //c1private_var:=0;
      end;
    end;

var c:c2;

begin
  c := new c2;
  writeln(c.c2internal_var);  
  writeln(c.c2public_var);  
  writeln(c.c2protected_var); // !!!!
  writeln(c.c2private_var);  
  
  writeln(c.c1internal_var);
  writeln(c.c1public_var);  
  //c.c1protected_proc;
  //writeln(c.c1protected_var);
  //writeln(c.c1private_var);  {}
  //readln;
end.