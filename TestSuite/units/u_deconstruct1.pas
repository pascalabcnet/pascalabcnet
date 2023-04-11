unit u_deconstruct1;

type
  t1 = class
    procedure Deconstruct(var a,b: byte) := exit;
  end;
end.