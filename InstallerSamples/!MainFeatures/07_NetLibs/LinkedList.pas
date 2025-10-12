// Использование LinkedList - двусвязного списка стандартной библиотеки - и его итератора

begin
  var L: LinkedList<integer> := new LinkedList<integer>;

  L.AddLast(3);
  L.AddLast(5);
  L.AddLast(7);
  L.AddFirst(2);
  Println(L);
  
  var a := new integer[10];
  L.CopyTo(a,0);
  Println(a);
  
  var lit: LinkedListNode<integer> := L.Find(5);
  L.AddBefore(lit,777);
  Println(L);
end.