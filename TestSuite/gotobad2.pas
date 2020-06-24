// #2271
type
  A = class
    procedure p;
    label 1;
    begin
      Arr(3).Select(b -> b);
      if True then
        goto 1;
      1: Print(1);
    end;
  end;

begin
end.
