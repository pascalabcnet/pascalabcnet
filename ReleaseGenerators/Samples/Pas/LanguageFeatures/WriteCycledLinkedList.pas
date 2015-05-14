type 
  Node<T> = class
  public
    data: T;
    next: Node<T>;
    constructor (d: T; n: Node<T>);
    begin
      data := d;
      next := n;
    end;
  end;

begin
  var n1 := new Node<integer>(5,nil);
  var n2 := new Node<integer>(6,n1);
  writeln(n2);
  n1.next := n2;
  writeln(n2);
end.