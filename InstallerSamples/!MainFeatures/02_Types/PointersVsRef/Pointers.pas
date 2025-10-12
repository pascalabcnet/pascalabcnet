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

begin
  var first: PNode := nil;
  
  // Добавляем в начало односвязного списка
  first := NewNode(3,first);
  first := NewNode(7,first);
  first := NewNode(5,first);
  
  // Вывод односвязного списка
  Println('Содержимое односвязного списка:');
  var p := first;
  while p<>nil do
  begin
    Print(p^.data);
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