type
  TClass = class
    static auto property X: byte;
  end;

begin
  TClass.X := 100;
  assert(TClass.X = 100);
end.