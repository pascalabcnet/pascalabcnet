unit visible_test_unit;

type 
    c1=class
      c1internal_var:integer;
    internal      
      procedure c1internal_proc;
      begin
      end;
    public
      c1public_var:integer;
      procedure c1public_proc;
      begin
      end;
    protected
      c1protected_var:integer;
      procedure c1protected_proc;
      begin
      end;
    private
      c1private_var:integer;
      procedure c1private_proc;
      begin
      end;
    end;

var c:c1;

begin
  c:= new c1;
  c.c1protected_proc;
  writeln(c.c1protected_var);
  writeln(c.c1private_var);  
end.