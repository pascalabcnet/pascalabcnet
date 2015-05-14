// Первые 3 минимума
begin
  SeqRandom().Println().OrderBy(x->x).Distinct().Take(3).Println;
end.