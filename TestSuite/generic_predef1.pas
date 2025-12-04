type TNode<T> = class;
TList<T> = class
node: TNode<T>;
end;

TNode<T> = class
a: T;
lst: TList<T>;
end;

begin
var n := new TNode<integer>;
n.a := 1;
var lst := new TList<integer>;
lst.node := n;
n.lst := lst;
assert(n.a = 1);
assert(lst.node.a = 1);
end.