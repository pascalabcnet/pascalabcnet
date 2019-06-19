type
  Expr = interface
  end;
  V = auto class(Expr)
    name: string;
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
  
function NegC(ex: Expr) := new Neg(ex);  
function ConsC(r: real) := new Cons(r);  
function AddC(ex1,ex2: Expr) := new Add(ex1,ex2);  
function MultC(ex1,ex2: Expr) := new Mult(ex1,ex2);  
function VC(name: string) := new V(name);  
  
function Simplify(e: Expr): Expr; forward;

function Simplify1(e: Expr): Expr;
begin
  match e with
    Mult(Cons(c),Cons(c1)): Result := ConsC(c*c1);
    Mult(Cons(1.0),ex): Result := Simplify(ex);
    Mult(Cons(0.0),ex): Result := ConsC(0);
    Add(Cons(0.0),ex): Result := Simplify(ex);
    Add(ex,Cons(0.0)): Result := Simplify(ex);
    Add(Cons(c),Cons(c1)): Result := ConsC(c+c1); 
    Add(Cons(c),ex): Result := AddC(ex,ConsC(c)); // константы - в хвосте!
    Mult(Cons(c),ex): Result := MultC(ex,ConsC(c));
    Add(Add(ex,Cons(c)),Cons(c1)): Result := AddC(ex,ConsC(c+c1)); // ассоциативность
    Mult(Mult(ex,Cons(c)),Cons(c1)): Result := MultC(ex,ConsC(c*c1)); // ассоциативность
    Neg(Cons(c)): Result := ConsC(-c); 
    else Result := e;
  end;
end;

function Simplify(e: Expr): Expr;
begin
  match e with
    Mult(e1,e2): Result := Simplify1(MultC(Simplify(e1),Simplify(e2)));
    Add(e1,e2): Result := Simplify1(AddC(Simplify(e1),Simplify(e2)));
    Neg(e1): Result := Simplify1(NegC(Simplify(e1)));
    else Result := e;
  end;
end;

function Str(e: Expr): string;
begin
  match e with
    Mult(l,r): Result := Str(l) + ' * ' + Str(r);
    Add(l,r): Result := Str(l) + ' + ' + Str(r);
    Neg(n): Result := '-' + Str(n);
    Cons(c): Result := c.ToString;
    V(x): Result := x;
  end;
end;

  
begin
  // 0 + 1*x + 1 + 0*(2 + a) + 2
  var r: Expr := AddC(ConsC(1),AddC(MultC(ConsC(1),VC('x')),ConsC(1)));
  r := AddC(r,MultC(ConsC(0),AddC(ConsC(2),VC('a'))));
  r := AddC(r,ConsC(2));
  Str(r).Println;
  r := Simplify(r);
  Str(r).Println;
end.  