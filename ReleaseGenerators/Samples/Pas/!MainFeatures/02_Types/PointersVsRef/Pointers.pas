// Выделение динамической памяти
// Использование указателей для создания односвязного списка
type 
  PNode = ^TNode;
  TNode = record
    data: integer;
    next: PNode;
  end;

function NewNode(d: integer; n: PNode): PNode;
begin
  New(Result);
  Result^.data := d;
  Result^.next := n;
end;

var first: PNode;
  
begin
  first := nil;
  // Добавляем в начало односвязного списка
  first := NewNode(3,first);
  first := NewNode(7,first);
  first := NewNode(5,first);
  
  // Вывод односвязного списка
  writeln('Содержимое односвязного списка: ');
  var p := first;
  while p<>nil do
  begin
    write(p^.data,' ');
    p := p^.next;
  end;
  
  // Разрушение односвязного списка
  p := first;
  while p<>nil do
  begin
    var p1 := p;
    p := p^.next;
    Dispose(p1); // Память обязательно возвращать
  end;
end.  