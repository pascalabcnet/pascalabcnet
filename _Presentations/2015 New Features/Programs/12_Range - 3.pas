begin
  Range(2,1000).Where(x->Range(2,Round(sqrt(x))).All(i->x mod i <> 0)).Print;
end.