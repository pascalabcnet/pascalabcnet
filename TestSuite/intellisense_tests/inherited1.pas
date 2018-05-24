type
  t1=class
    procedure p1; virtual;
    begin
    end;
  end;
  t2=class(t1)
    procedure p1; override;
    begin
      inherited p1{@procedure t1.p1(); virtual;@}();
    end;
  end;
  t3=class(t2)
    procedure p1; override;
    begin
      inherited p1{@procedure t2.p1(); override;@};
    end;
  end;

begin end.