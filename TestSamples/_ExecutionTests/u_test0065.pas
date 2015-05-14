unit u_test0065;

uses u2_test0065;

type TRec4 = record
  d : array[1..3] of TRec5;
end;

type TRec2 = record
  c : TRec4;
end;

end.