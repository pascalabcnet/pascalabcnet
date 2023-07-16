unit u_abstract2;
type
  t1<TSelf> = abstract class
    where TSelf: t1<TSelf>;
  end;
    
end.