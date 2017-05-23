unit u_exceptions2;

type
  MyException1 = class(Exception);
  MyException2 = class(Exception);
  IncorrectInputException = class(Exception);

begin
  var s := '';
  try
    raise new MyException1('EXCEPTION2!!');
  except
    on e: IncorrectInputException do writeln(e.Message);
    on e: MyException1 do s := e.Message;
    on e: MyException2 do writeln(e.Message);
  end;
  assert(s = 'EXCEPTION2!!');
end.