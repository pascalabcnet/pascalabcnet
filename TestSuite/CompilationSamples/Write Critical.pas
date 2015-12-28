// Вывод в параллельной секции без использования критических секций
// и с их использованием. В первом случае из-за параллельного доступа
// к разделяемому ресурсу возможно, что строки будут выводиться
// на одной строке, и в произвольно порядке. Во втором случае такого не будет. 
begin
  {$omp parallel sections}
  begin
    begin
      WriteLn('Thread 1 started');      
    end;
    begin
      WriteLn('Thread 2 started');
    end;
  end;
  
  {$omp parallel sections}
  begin
    begin
      {$omp critical a}
      WriteLn('Thread 1 started');
    end;
    begin
      {$omp critical a}
      WriteLn('Thread 2 started');
    end;
  end;  
end.