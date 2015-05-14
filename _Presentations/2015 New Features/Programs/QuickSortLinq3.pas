function QuickSort(a: IEnumerable<integer>): IEnumerable<integer> := a.Count = 0 ? a : QuickSort(a.Skip(1).Where(x->x<=a.First())) + a.First() + QuickSort(a.Skip(1).Where(x->x>a.First()));

begin
  QuickSort(SeqRandom(20).Println()).Println;
end.