type
  T = static class
    public static function A(s: string) := 0;
    public static procedure A := exit;
  end;

begin
  T.A{@static procedure T.A();@};
end.