type
  Expr = interface
  end;
  Cons = auto class(Expr)
    r: real;
  end;
  Add = auto class(Expr)
    left,right: Expr;
  end;
  Mult = auto class(Expr)
    left,right: Expr;
  end;
  Neg = auto class(Expr)
    ex: Expr;
  end;
  
// Создающие функции
function ConsC(r: real) := new Cons(r);
function AddC(l,r: Expr) := new Add(l,r);
function MultC(l,r: Expr) := new Mult(l,r);
function NegC(ex: Expr) := new Neg(ex);

// Вычисляющая функция
function Eval(e: Expr): real;
begin
  match e with
    Cons(c): Result := c;
    Neg(n): Result := -Eval(n);
    Add(l,r): Result := Eval(l) + Eval(r);
    Mult(l,r): Result := Eval(l) * Eval(r);
  end;
end;  
  
begin
  // -2 + 3 * 4
  var r := AddC(NegC(ConsC(2)),MultC(ConsC(3),ConsC(4)));
  Eval(r).Print;
end.  