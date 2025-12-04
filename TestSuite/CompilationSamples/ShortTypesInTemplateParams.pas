begin
  var q := new Queue<(integer,integer)>;
  q.Enqueue((1,2));
  q.Enqueue((2,5));
  q.Println;
  Println(q);
  
  var l := new List<integer->integer>;
  l.Add(x->x);
  l.Add(x->x*x);
  l.ForEach(f->Print(f(5)));
  Println;
  
  var l1 := new List<(integer,integer)->integer>;
  l1.Add((x,y)->x+y);
  l1.Add((x,y)->x-y);
  l1.Add((x,y)->x*y);
  l1.ForEach(f->Print(f(2,3)));
end.