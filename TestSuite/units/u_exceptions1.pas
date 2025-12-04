unit u_exceptions1;
type MyException1 = class(exception)
end;

type MyClass = class
procedure Test;
begin
  
end;
end;

begin
  var i := 0;
  try
    raise new MyException1;
  except
    i := 3;
  end;
  assert(i = 3);
  
  try
    raise new MyException1;
  except on MyException1 do
    i := 5;
  on Exception do
    i := 7;
  end;
  assert(i = 5);
  
  try
    raise new exception;
  except on MyException1 do
    i := 5;
  on Exception do
    i := 7;
  end;
  assert(i = 7);
  
  var obj: object := nil;
  try
    obj.ToString();
  except on System.NullReferenceException do
    i := 3;
  end;
  assert(i=3);
  
  var ex1: MyClass;
  try
    ex1.Test;
  except on System.NullReferenceException do
    i := 4;
  end;
  assert(i = 4);
  
  try
  try
    raise new MyException1;
  finally
    i := 7;
  end;
  except on ex: MyException1 do
    begin
      assert(i = 7);
      i := 6;
    end;
  end;
  
  assert(i=6);
  
  var arr : array[1..3] of integer;
  try
    arr[0] := 5;
  except on System.IndexOutOfRangeException do
    i := 4;
  end;
  assert(i=4);
  
  var arr2 : array of integer;
  SetLength(arr2, 3);
  try
    arr[-1] := 5;
  except on System.IndexOutOfRangeException do
    i := 3;
  end;
  assert(i=3);
  for var j := 0 to 5 do
    try
      if j = 2 then
        i := 4;
    except
      break;
    end;
 
  assert(i = 4);
end.