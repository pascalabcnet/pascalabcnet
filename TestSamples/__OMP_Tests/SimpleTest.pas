var kk:=0;

procedure test1(k:integer);
begin
  write(' '+(k:2)+' ');    
end;


procedure parTest;
begin
  {$omp parallel for}
  for var i:=1 to 20 do begin
    Sleep(100);
    kk:=i;
    test1(kk);
  end;  
end;


begin

  var m0 := Milliseconds;
  
  for var i:=1 to 20 do begin
    Sleep(100);
    test1(i);
  end;
    
  writeln;
  writeln(Milliseconds-m0);
  
  
  
  m0 := Milliseconds;
  
  {$omp parallel for}
  for var i:=1 to 20 do begin
    Sleep(100);
    test1(i);
  end;
    
  writeln;
  writeln(Milliseconds-m0);

  m0 := Milliseconds;
  
  parTest;
  
  writeln;
  writeln(Milliseconds-m0);
end.