begin
  var i, j: integer;
  var k: integer;
  i := 2;
  j := 3;
  if i = 2 then
    i := 2
  else
    i := 3;
  if j < i + 2 then
    if i = 2 then
      if j = 3 then
        k := 5
      else
        k := 6
    else
      k := 7
  else
    k := 8;
  assert(k = 5);
  
  i := 2;
  j := 3;
  if j < i + 2 then
    if i = 2 then
    begin
      if j = 3 then
        k := 5
      else
        k := 6;
      k := 10; 
    end
    else
      k := 7
  else
    k := 8;
  
  assert(k = 10);
  i := 2;
  j := 3;
  if j < i + 2 then
    if i = 2 then
      if j = 4 then
        k := 5
      else
        k := 6
    else
      k := 7
  else
    k := 8;
  assert(k = 6);
  
  i := 2;
  j := 3;
  if j < i + 2 then
    if i = 3 then
      if j = 4 then
        k := 5
      else
        k := 6
    else
      k := 7
  else
    k := 8;
  assert(k = 7);
  
  i := 2;
  j := 7;
  
  if j < i + 2 then
    if i = 3 then
      if j = 4 then
        k := 5
      else
        k := 6
    else
      k := 7
  else
    k := 8;
  assert(k = 8);
  
  i := 2;
  j := 3;
  if i = 2 then
    case j of
      2: k := 2;
      3: k := 3;
    else k := 4;
    end
  else
    k := 5;
  assert(k = 3);
  
  i := 2;
  j := 3;
  if i = 2 then
    case j of
      2: k := 2;
      4: k := 3;
    else k := 4;
    end
  else
    k := 5;
  assert(k = 4);
  
  i := 2;
  j := 3;
  if i = 3 then
    case j of
      2: k := 2;
      4: k := 3;
    else k := 4;
    end
  else
    k := 5;
  assert(k = 5);
  
  if i = 2 then
    case j of
      3:
        begin
          if i = 2 then 
            k := 2;
          k := 2;
        end;
      2: k := 3;
    else k := 4;
    end;
  
end.