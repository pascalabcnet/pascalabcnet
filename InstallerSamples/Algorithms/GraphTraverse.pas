// Обход ориентированного графа в глубину
procedure TraverseDepth(gr: array of array of integer; p: array of boolean; x: integer);
begin
  Print(x);
  p[x] := True;
  foreach var y in gr[x] do
    if not p[y] then
      TraverseDepth(gr,p,y);
end;

begin
  var gr := [[1, 2], [3], [1, 4], [5], [6], [4, 7], [], []];
  var p := [False] * gr.Count;
  TraverseDepth(gr,p,0);  
end.

