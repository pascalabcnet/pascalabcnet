unit u1;

interface
  procedure pr1(param1: integer:= 0);
  
implementation
  procedure pr1(param1: integer):= exit;

begin
  pr1{@procedure u1.pr1(param1: integer:=0);@};
end.