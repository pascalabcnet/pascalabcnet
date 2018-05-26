type
  Expr = class
  end;
  Cons = auto class(Expr)
    r: real;
  end;
  Add = auto class(Expr)
    left,right: Expr;
    procedure Deconstruct(var l,r: Expr);
    begin
      l := left; r := right;
    end;
  end;
  Mult = auto class(Expr)
    left,right: Expr;
    procedure Deconstruct(var l,r: Expr);
    begin
      l := left; r := right;
    end;
  end;
  Neg = auto class(Expr)
    ex: Expr;
  end;
  
function Eval(e: Expr): real;
begin
  match e with
    Cons(c): Result := c.r;
    Neg(n): Result := -Eval(n.Ex);
    Add(l,r): Result := Eval(l) + Eval(r);
    Mult(l,r): Result := Eval(l) * Eval(r);
  end;
end;  
  
begin
  var r := new Add(new Neg(new Cons(2)),new Mult(new Cons(3),new Cons(4)));
  Eval(r).Print;
end.  