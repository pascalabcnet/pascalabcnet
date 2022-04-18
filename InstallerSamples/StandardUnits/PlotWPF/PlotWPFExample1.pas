// пример вывода графиков разных видов 
uses PlotWPF;

begin
  var g := new GridWPF(2,2,10);

  var c := new LineGraphWPF(0,Pi,v -> v*Sin(v*10));
  c.PlotRect := Rect(0,0,10,10);
  c.Graph[0].ChangeData(0,Pi,x->x*x);
  c.Graph[0].Color := Colors.Green;
  c.Graph[0].Thickness := 3;
  c.AddLineGraph(0,Pi,v -> Sqrt(v));
  
  var c2 := new LineGraphWPF(0,Pi,v -> Sin(v*10)-Cos(v*7));
  
  var c4 := new MarkerGraphWPF(|1.0,2,3,4,5|,|5.0,15,7,12,2|);
  c4.AddLineGraph(|1.0,2,3,4,5|,|5.0+1,15+1,7+1,12+1,2+1|);
  c4.AddMarkerGraph(|1.0,2,3,4,5|,|5.0+1,15+1,7+1,12+1,2+1|,Colors.Bisque,MarkerType.Diamond,8);
  c4.Graph[0].Thickness := 0.7;
  c4.Graph[1].MarkerType := MarkerType.Box;
  c4.Graph[2].Thickness := 0.7;

  var gg := new GridWPF(2,2,3);
  new LineGraphWPF(0,2,x->Cos(10*x));
  new LineGraphWPF(0,2,x->Sqrt(x));
  new LineGraphWPF(0,2,x->Sin(10*x));
  new LineGraphWPF(0,2,x->exp(x));
end.
