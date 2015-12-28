// Использование ссылок вместо указателей для создания односвязного списка
type 
  Node = class
    data: integer;
    next: Node;
    constructor (d: integer; n: Node);
    begin
      data := d;
      next := n;
    end;
  end;

// Переменная типа "класс" представляет собой ссылку на объект, выделяемый конструктором
var first: Node;
  
begin
  first := nil;
  // Добавляем в начало односвязного списка
  first := new Node(3,first);
  first := new Node(7,first);
  first := new Node(5,first);
  
  // Вывод односвязного списка. ^ отсутствуют
  writeln('Содержимое односвязного списка (использование ссылок вместо указателей): ');
  var p := first;
  while p<>nil do
  begin
    write(p.data,' ');
    p := p.next;
  end;
  
  // Разрушение односвязного списка
  first := nil; // Сборщик мусора соберет память, на которую никто больше не указывает
end.  