var i: integer;

procedure test;
begin
  try
    raise new Exception;
  finally
    i := 1;
  end;
end;

procedure test2;
begin
  try
    
  finally
    i := 3;
  end;
end;

procedure test3;
begin
  try
  while true do
  try
    raise new Exception;
  finally
    i := 4;
  end;
  except
  end;
end;

procedure test4;
begin
  try
    raise new Exception
  finally
    try
      raise new System.NotSupportedException;
    except
      i := 5;
    end;
  end;
  i := 6;
end;

begin
  try
    test;
  except
    Inc(i);
  end;
  assert(i = 2);
  test2;
  assert(i = 3);
  test3;
  assert(i = 4);
  try
    test4;
  except
  end;
  assert(i = 5);
end.