type
  Pair<T, Q> = template class
  public
    first: T;
    second: Q;
    constructor; overload;
    begin
    end;
    constructor(_t: T; _q: Q); overload;
    begin
      first := _t;
      second := _q;
    end;
    function ToString: string; override;
    begin
      result := first.ToString + ' ' + second.ToString;
    end;
  end;
  
var
  a: Pair<integer, string>;
  b: Pair<real, real>;
  
begin
  a := new Pair<integer, string>;
  b := new Pair<real, real>(2.71, 3.14);
  a.first := 2;
  a.first := a.first * a.first;
  a.second := 'Hi!';
  writeln(a.ToString);
  writeln(b.ToString);
  readln;
end.