
var s:string;
    i,j:integer;
begin
//  i:=i div i;
  write('Введите строку: ');  
  readln(s); 
  writeln('Вы ввели: ',s);  
  
  write('Введите число: ');
  readln(i);
  writeln('Вы ввели: ',i);  
  
  write('Введите два числа: ');
  readln(i,j); 
  writeln('Вы ввели: ',i,' ',j);  
  
  writeln('Sleep(1000)');  
  sleep(1000);
  writeln('Конец');  
end.