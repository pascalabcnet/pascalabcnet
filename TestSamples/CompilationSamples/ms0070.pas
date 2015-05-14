type
  A = class
    procedure p; virtual;
  end;
  B = class(A)
    procedure p; override;
  end;

{ A }
procedure A.p;
begin
  write(1);
end;

{ B }
procedure B.p;
begin
  write(2);
  inherited p;
end;

var b1: B;

begin
  b1:=B.Create;
  b1.p;
  readln;
end.
