type Node<T> = auto class
  data: T;
  left,right: Node<T>;
end;

function CNode<T>(x: T; l: Node<T> := nil; 
  r: Node<T> := nil): Node<T> := new Node<T>(x,l,r);

function Infix<T>(root: Node<T>): sequence of T;
begin
  if root = nil then exit;
  yield sequence Infix(root.left);
  yield root.data;
  yield sequence Infix(root.right);
end;

begin
  var root := CNode(1,CNode(2,CNode(3),CNode(4)),CNode(5));
  Infix(root).Print;
end.