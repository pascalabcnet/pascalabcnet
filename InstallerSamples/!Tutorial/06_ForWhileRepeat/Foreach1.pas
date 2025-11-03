// Цикл foreach

begin
  // foreach по диапазону
  foreach var x in 1..10 do
    Print(x);
  Println;
  
  var a := [1,3,5,7,9];
  // foreach по массиву
  foreach var x in a do
    Print(x);
  Println;
  
  // foreach с индексом 
  foreach var x in a index i do
    Print((i,x));
  Println; 
  
  // foreach по словарю
  var d := Dict('cat' to 'кошка', 'dog' to 'собака');
  foreach var kv in d do
    Print(kv);
  Println; 

  // foreach по словарю с распаковкой
  foreach var (key,value) in d do
    Println($'{key} → {value}');
end.
