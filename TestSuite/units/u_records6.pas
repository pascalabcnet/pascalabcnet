unit u_records6;

type
  // Обязательно запись - с классом не воспроизводится
  t1 = record
    
    // Обязательно свойство
    property p1: byte read 0;
    
  end;
  
  t2 = class
    
    // Обязательно свойство, использующее t1
    property p2: word write
    begin
      var o: t1;
    end;
    
  end;
  
end.