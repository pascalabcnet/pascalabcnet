begin
var locker := new object();
var total := 0.0;
System.Threading.Tasks.Parallel.For(1, 1000000, procedure(i) -> begin total += sqrt(i); end);
//assert(abs(total)-456302086.334068<0.00001);
end.