type
  MyType<T> = class
    Variable: T;
  end;

begin
  var V{@var V: MyType<Nullable<integer>>;@} := new MyType&<integer?>();
  var l{@var l: List<Nullable<integer>>;@} := new List<integer?>();
end.