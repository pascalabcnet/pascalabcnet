// Нерекурсивное решение задачи о расстановке 8 ферзей

## (1..8).Permutations
.Where(v->v.Numerate.Combinations(2)
       .All(\(\(a,b),\(c,d)) -> abs(a-c)<>abs(b-d)))
.PrintLines