begin
  var l := new List<integer>;
  l.AddRange(SeqRandom());
  l.Println;
  l.Insert(5,777777);
  l.Println;
  l.RemoveAt(0);
  l.Println;
end.