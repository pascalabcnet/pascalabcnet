// Демонстрация использования критических секций и возможных взаимоблокировок
begin
  {$omp parallel sections}
  begin
    begin
      WriteLn('Thread 1 started');
      {$omp critical a}
      begin
        Writeln('Lock a set by 1 thread');
        //ReadLn;
        {$omp critical b}
        begin
          Writeln('Lock b set by 1 thread');
        end;
      end;
      WriteLn('Thread 1 finished');
    end;
    begin
      WriteLn('Thread 2 started');
      {$omp critical b}
      begin
        Writeln('Lock b set by 2 thread');
        //ReadLn;
        {$omp critical a}
        begin
          Writeln('Lock a set by 2 thread');
        end;
      end;
      WriteLn('Thread 2 finished');
    end;
  end;
  Writeln('Program finished without mutual lock!');
end.