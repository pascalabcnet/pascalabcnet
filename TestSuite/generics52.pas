var j: integer;
    k: integer;
type
  IFoo<T> = interface
    procedure Test1(action: T -> ());
  end;
  
  TClass<T> = class(IFoo<T>)
    public procedure Test1(action: T -> ());
    begin
      action(default(T));
    end;
  end;
  
procedure Test2<T>(Self: IFoo<T>; action: T -> ()); extensionmethod;
begin
  assert(typeof(T) = typeof(integer));
  j := 1;
  action(default(T));
end;

procedure PrintIt(i:Integer);
begin
  k := 1;
end;

procedure PrintItInt(i:Integer);
begin
  
end;

procedure PrintIt(i:string);
begin

end;

var
  foo: IFoo<Integer>;
begin
  
  foo := new TClass<integer>;
  foo.Test2(PrintIt); 
  assert(j = 1);
  
  foo.Test1(PrintIt); 
  foo.Test2(PrintItInt);
  assert(k = 1);
end.