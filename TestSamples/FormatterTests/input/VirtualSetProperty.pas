type
  A=class
    i:integer;
    procedure Seti(i: integer);virtual;
    begin
      Self.i:=i;
      write('A')
    end;
    property p:integer read i write Seti;
  end;
  B=class(A)
    procedure Seti(i: integer);override;
    begin
      Self.i:=i;
      write('B')
    end;
  end;
  
var a1: A;

begin
  a1:=B.Create;
  a1.p:=1;
end.
