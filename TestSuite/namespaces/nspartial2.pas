namespace nspartial;

type
  T = partial class
    public i: integer;
    public procedure Test2;
    begin
      Inc(i);
    end;
  end;

end.