// Демонстрация создания класса стека на базе списка
type
  pnode=^node;
  node=record
    data: integer;
    next: pnode;
  end;
  
  stack=class
    t: pnode;
    constructor Create;
    begin
      t:=nil;
    end;
    function empty: boolean;
    begin
      empty:=t=nil;
    end;
    procedure push(i: integer);
    var v: pnode;
    begin
      New(v);
      v^.data:=i;
      v^.next:=t;
      t:=v;
    end;
    function top: integer;
    begin
      if empty then begin
        writeln('попытка взять элемент с пустого стека');
        exit;
      end;
      top:=t^.data;
    end;
    function pop: integer;
    var v: pnode;
    begin
      if empty then begin
        writeln('попытка взять элемент с пустого стека');
        exit;
      end;
      pop:=top;
      v:=t;
      t:=t^.next;
      dispose(v);
    end;
    procedure Destroy;
    var i: integer;
    begin
      while not empty do
        i:=pop;
    end;
  end;
  
var s: stack;

begin
  cls;
  s:=stack.Create;
  s.push(7);
  s.push(2);
  s.push(5);
  while not s.empty do
    write(s.pop,' ');
  s.Destroy;
end.
