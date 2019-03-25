type
  Base<T> = class
    XYZW: T;
    procedure p(d: T);
    begin
      XYZW := d;
    end;
  end;
  
  Derived<T1> = class(Base<T1>)
    //XYZW: T1;
  end;

begin
  var a := new Derived<integer>;
  a.p(2);
  a.XYZW := 2;
  Assert(a.XYZW=2);
end.