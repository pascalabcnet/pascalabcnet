function EachCountMy<T>(Self: T): integer; extensionmethod;
begin
  Result := 3
end;

function EachCountMy<T,T1>(Self: sequence of System.Linq.IGrouping<T,T1>): integer; extensionmethod;
begin
  Result := 2
end;

function EachCountMy<T>(Self: sequence of T): integer; extensionmethod;
begin
  Result := 1
end;

function EachCountMy<T>(Self: sequence of List<T>): integer; extensionmethod;
begin
  Result := 4
end;

function EachCountMy<T,TXT1>(Self: Dictionary<T,TXT1>): integer; extensionmethod;
begin
  Result := 7
end;

function EachCountMy<T,TXT1>(Self: Dictionary<T,List<TXT1>>): integer; extensionmethod;
begin
  Result := 8
end;


begin
  var s := 'aa bbb aa c f f ddd e e f ';
  Assert(Lst(Lst(1),Lst(2)).EachCountMy = 4);      // 4
  Assert(s.ToWords.GroupBy(x->x).EachCountMy = 2); // 2
  Assert(s.ToWords.EachCountMy = 1);               // 1 
  Assert(Dict(KV(1,2)).EachCountMy = 7);           // 7
  Assert(Dict(KV(1,Lst(1,2))).EachCountMy = 8);    // 8
  Assert(1.EachCountMy = 3);                       // 3
  Assert(byte(1).EachCountMy = 3);                 // 3
end.