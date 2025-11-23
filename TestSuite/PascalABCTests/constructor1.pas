type
  MyException=class(Exception)
    constructor :=
    create('Some text');
  end;

begin 
var ex := new MyException;
assert(ex.Message = 'Some text');
end.