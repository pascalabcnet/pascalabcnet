type
  Expr = class
  end;
  V = auto class(Expr)
    name: string;
    procedure Deconstruct(var name: string);
    begin
      name := Self.name
    end;
  end;
  Cons = auto class(Expr)
    r: real;
    procedure Deconstruct(var r: real);
    begin
      r := Self.r
    end;
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
    procedure Deconstruct(var ex: Expr);
    begin
      ex := Self.ex
    end;
  end;
  
function NegC(ex: Expr) := new Neg(ex);  
function ConsC(r: real) := new Cons(r);  
function AddC(ex1,ex2: Expr) := new Add(ex1,ex2);  
function MultC(ex1,ex2: Expr) := new Mult(ex1,ex2);  
function VC(name: string) := new V(name);  
  
function Simplify(e: Expr): Expr;
begin
  match e with
    Mult(Cons(c),Cons(c1)): Result := ConsC(c*c1);
    Mult(Cons(c),ex) when c=1: Result := Simplify(ex);
    Mult(Cons(c),ex) when c=0: Result := ConsC(0);
    Add(Cons(c),ex) when c=0: Result := Simplify(ex);
    Add(ex,Cons(c)) when c=0: Result := Simplify(ex);
    Add(Cons(c),Cons(c1)): Result := ConsC(c+c1); 
    Neg(Cons(c)): Result := ConsC(-c); 
    Mult(e1,e2): Result := MultC(Simplify(e1),Simplify(e2));
    Add(e1,e2): Result := AddC(Simplify(e1),Simplify(e2));
    Neg(e1): Result := NegC(Simplify(e1));
    else Result := e;
  end;
end;

procedure Print(e: Expr);
begin
  match e with
    Mult(l,r): begin Print(l); Print('*'); Print(r); end;
    Add(l,r): begin Print(l); Print('+'); Print(r); end;
    Neg(n): begin Print('-'); Print(n); end;
    Cons(c): Print(c);
    V(x): Print(x);
  end;
end;

  
begin
  // 0 + 1*x + 0 + 0*(2 + a)
  var r: Expr := AddC(ConsC(0),AddC(MultC(ConsC(1),VC('x')),ConsC(0)));
  r := AddC(r,MultC(ConsC(0),AddC(ConsC(2),VC('a'))));
  Print(r);
  Println;
  loop 2 do
  begin
    r := Simplify(r);
    Print(r);
    Println;
  end;
end.  