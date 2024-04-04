unit NamedQData;

uses OpenCLABC;

function NamedQ(name: string; delay: integer := 1000) :=
HPQ(()->
begin
  lock output do Writeln($'{Milliseconds} Очередь {name} начала выполнятся');
  Sleep(delay);
  lock output do Writeln($'{Milliseconds} Очередь {name} выполнилась');
end);

end.