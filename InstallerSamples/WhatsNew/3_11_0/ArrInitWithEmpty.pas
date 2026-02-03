// Уточнение инициализации элемента массива пустой коллекцией
begin
  var a: array of array of integer := [[],[],[1]];
  Println(a,TypeName(a));
  var b := [[],[1],[]];
  Println(b,TypeName(b));
  var aa: array of array of array of integer := [[[],[2]],[]];
  Print(aa,TypeName(aa));
end.