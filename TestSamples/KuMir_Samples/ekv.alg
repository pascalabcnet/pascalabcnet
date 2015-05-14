вкл GraphABC

вещ mw
mw:=2.9

алг главна€_прогр
нач
  SetWindowCaption("–екурси€: е-квадраты на  ућире");
  SetWindowSize(750,530);
  SetPenColor(clWhite);
  SetBrushColor(clRandom);
  е_квадрат(4,125,18,490)
кон

алг е_квадрат(цел n,x,y,w);
нач
  цел w1,h
  w1:=int(w/mw)
  h:=div((w-2*w1),3)
  FillRectangle(x,y,x+w,y+w)
  если n>0 то
    Sleep(10)
    SetBrushColor(clRandom)
    е_квадрат(n-1,x+h,y+h,w1)
    е_квадрат(n-1,x+w-h-w1,y+h,w1)
    е_квадрат(n-1,x+h,y+w-h-w1,w1)
    е_квадрат(n-1,x+w-h-w1,y+w-h-w1,w1)
  все
кон