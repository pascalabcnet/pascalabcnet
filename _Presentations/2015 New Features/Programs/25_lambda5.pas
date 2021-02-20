begin
  SeqGen(1,x->x+2,10).Println;
  SeqGen(1,x->x*2,10).Println;
  SeqGen(1,1,(x,y)->x+y,10).Println;
end.