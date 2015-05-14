uses SyntaxTree;

type TClass<T> = class end;

var b : int32_const;
    vis : IVisitor;
    t1 : System.Collections.Generic.List<integer>;
    t2 : System.Collections.Generic.List<real>;
begin
b := new int32_const(45);
assert(b.val = 45);
end.