function operator implicit<T>(f: System.Func<T>): System.Action; extensionmethod;
begin
  Result := procedure()->f();
end;

function operator implicit<T, TResult>(f: System.Func<T, TResult>): System.Func<TResult>; extensionmethod;
begin
  Result := ()->f(default(T));
end;

function operator implicit<T>(f: System.Action): System.Func<T>; extensionmethod;
begin
  Result := ()->begin f(); Result := default(T) end;
end;

var i: integer;

function ff(): integer;
begin
  i := 2;
end;

function ff2(j: integer): integer;
begin
  i := 3;
end;

begin
  var fff: System.Func<integer> := ff;
  var a: System.Action := fff;
  a;
  assert(i=2);
  fff := a;
  assert(fff() = 0);
  var fff2: System.Func<integer, integer> := ff2;
  fff := fff2;
  fff();
  assert(i=3);
end.