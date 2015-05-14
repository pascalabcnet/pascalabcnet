type
  Base<T> = template class
  public
    constructor; 
    begin
      Writeln(self);
    end;
  end;
  Foo = class(Base<Foo>)
    constructor; 
    begin 
      inherited Create;
    end;
  end;

var f:Foo;

begin
  f := new Foo;
  readln;
end.