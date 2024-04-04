// Результат функции кешируется в словаре
[Cache]
function fib(n: integer): integer := 
  if n in 1..2 then 1 
  else fib(n-1) + fib(n-2);

function fib1(n: integer): integer := 
  if n in 1..2 then 1 
  else fib1(n-1) + fib1(n-2);
  
begin
  // Кеширование есть. Работает сотые доли секунды
  Println(fib(42),MillisecondsDelta/1000);
  
  // Кеширования нет - быстродействие катастрофически падает 
  // из-за многочисленных повторяющихся вычислений
  Println(fib1(42),MillisecondsDelta/1000);
end.
