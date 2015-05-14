unit u_pointers4;
type PRec = ^TRec;
     TRec = record
      a : integer;
      next : PRec;
     end;

procedure Test2(var p : PRec);
begin
  assert(p^.a = 4);
  New(p);
  p^.a := 12;
end;

procedure Test(p : PRec);
begin
  assert(p^.a = 3);
  New(p^.next); p^.next^.a := 4;
  Test2(p^.next);
  assert(p^.next^.a = 12);
end;

procedure Test3(var p : PRec);
 procedure Nested;
 begin
  New(p); p^.a := 10;
  New(p^.next); p^.next^.a := 11;
 end;
 
begin
  Nested;
  assert(p^.a = 10);
  assert(p^.next^.a = 11);
end;

var p : PRec;
     
begin
  New(p);
  p^.a := 3;
  Test(p);
  Test3(p);
  Dispose(p);
end.