function QuickSort(a: IEnumerable<integer>): IEnumerable<integer> := a.Count = 0 ? a : QuickSort(a.Skip(1).Where(x->x<=a.First())) + a.First() + QuickSort(a.Skip(1).Where(x->x>a.First()));

begin
  var a := SeqRandom(20);
  a.Println;
  a.QuickSort;
  a.Println;
end.