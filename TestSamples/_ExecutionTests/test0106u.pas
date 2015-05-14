unit test0106u;

type TProc = procedure(sender: object);

var i : integer;

type TClass = class
event OnClick : TProc;
procedure MyProc(sender : object);
begin
i := 9;
end;
constructor Create;
begin
OnClick += MyProc;
OnClick(self);
assert(i=9);
OnClick -= MyProc;
end;
procedure RaiseMethod;
begin
OnClick(self);
end;
end;

procedure MyProc(sender : object);
begin
i := 8;
end;

end.