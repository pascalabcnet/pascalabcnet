type
  Recurrent<T> = class 
    first: T;
    next: Func<T,T>;
    count: integer;
    constructor (f: T; n: Func<T,T>; c: integer);
    begin
      first := f;
      next := n;
      count := c;
    end;
    
    function f(): IEnumerable<T>;
    begin 
      result := Range(1, count).Select(x -> begin result := first; first := next(first); end)
    end;
  end;
 
begin
  var a := new Recurrent<integer>(1, x -> x * 2, 10);
  writeln(a.f());
end.