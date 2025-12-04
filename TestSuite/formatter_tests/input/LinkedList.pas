// Использование LinkedList - двусвязного списка стандартной библиотеки - и его итератора
uses System.Collections,System.Collections.Generic;

procedure print(l: ICollection);
begin
  foreach x: integer in l do
    write(x,' ');
  writeln;  
end;

var l: LinkedList<integer> := new LinkedList<integer>;

begin
  l.AddLast(3);
  l.AddLast(5);
  l.AddLast(7);
  l.AddFirst(2);
  print(l);
  
  var a := new integer[10];
  l.CopyTo(a,0);
  print(a);
  
  var lit: LinkedListNode<integer> := l.Find(5);  
  l.AddBefore(lit,777);
  print(l);
end.