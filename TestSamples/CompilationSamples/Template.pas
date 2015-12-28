type
  Pair<T,Q> = template class
  public
    First: T;
    Second: Q;
    constructor(First: T; Second: Q);
    begin
      Self.First := First;
      Self.Second := Second;
    end;
    class function operator+(Left,Right: Pair<T,Q>): Pair<T,Q>;
    begin
      Result := new Pair<T,Q>(Left.First + Right.First, 
      						  Left.Second + Right.Second);
    end;
    function ToString: string; override;
    begin
      Result := string.Format('[{0}; {1}]', First, Second);
    end;
  end;
  
var
  a,b: Pair<integer, string>;
  
begin
  a := new Pair<integer,string>(1, 'один ');
  b := new Pair<integer,string>(2, 'два');
  Writeln(a + b);
  readln;
end.