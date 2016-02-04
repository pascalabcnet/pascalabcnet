{function Reverse<T>(Self: array of T): array of T; extensionmethod;
begin
  System.Array.Reverse(Self);
  Result := Self;
end;}

function BinarySearch<T>(self: array of T; item: T): integer; extensionmethod;
begin
  Result := System.Array.BinarySearch(self,item);
end;


begin
<<<<<<< HEAD
  //var a := Arr(1,2,3);
  //System.Linq.Enumerable.Select(a,x->x); - жаль - пока не работает. Закомментил чтобы тесты не падали
=======
  var a := Arr(1,2,3);
  var seq := System.Linq.Enumerable.Select(a,x->x*2);
>>>>>>> 3333c549a1247d102718878f176f32cbbd17473d
  //a.Reverse.Print;
end.