var s:string;
    i,j:integer;
begin
  write('Введите строку: ');  
  readln(s); 
  writeln('Вы ввели: ',s);  
  write('Введите число: ');
  readln(i);
  writeln('Вы ввели: ',i);  
  i:=i div i;
{
ss
1
2 1
}  
  write('Введите два числа: ');
  readln(i,j); 
  writeln('Вы ввели: ',i,' ',j);  
  
  writeln('Sleep(1000)');  
  sleep(1000);
  writeln('Конец');  
end.