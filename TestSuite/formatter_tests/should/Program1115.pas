begin
  (new System.Threading.Thread(() -> begin
    //комментарий
    writeln;//
    writeln;
    //не_код
    (new System.Threading.Thread(() -> begin
      //комментарий
      writeln;//
      writeln;
      //не_код
    end)).Start;
  end)).Start;
end.