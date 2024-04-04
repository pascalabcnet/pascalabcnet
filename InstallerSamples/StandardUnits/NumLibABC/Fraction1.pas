uses NumLibABC;

// вычисление выражения с обыкновенными дробями
begin
   var r:=((Frc(5,5,9)-Frc(7,18))/35+(Frc(40,63)-Frc(8,21))/20+
      (Frc(83,90)-Frc(41,50))/2)*50;
   r.Print   
end.
