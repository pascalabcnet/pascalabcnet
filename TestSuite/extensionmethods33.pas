type
  T = record
    public X: integer;
  end;

procedure p(var self: T); extensionmethod;
begin
  self.X += 2;
end;

begin
  var a: T;
  a.p;
  Assert(a.X = 2);
end.