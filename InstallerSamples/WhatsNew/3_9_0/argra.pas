uses GraphWPF;

begin
  // Представление взвешенного графа как словаря словарей
  //var EmptyDict := new Dictionary<char,integer>;
  var graph := Dict(
    ('A',Dict(('B',2),('E',5))),
    //('B',EmptyDict),
    ('C',Dict(('B',7),('D',9))),
    ('D',Dict(('B',34))),
    ('E',Dict(('C',12),('B',66)))
  );
  var dPoints := new Dictionary<char,Point>;
  var R := 200;
  var n := ('A'..'E').Count;
  var Phi := 0.0; 
  for var c := 'A' to 'E' do
  begin  
    dPoints[c] := Window.Center + R * Vect(Cos(Phi),Sin(Phi));
    Phi += 2 * Pi/n;
  end;
  
  Window.Title := 'Рисование ориентированного графа';
  var Radius := 10;
  foreach var kv in dPoints do
  begin
    var p := kv.Value;
    Circle(p,Radius);
    TextOut(p.x,p.y,kv.Key,Alignment.Center);
  end;
  foreach var k1 in graph.Keys do
  foreach var k2 in graph[k1].Keys do
  begin
    var weight := graph[k1][k2];
    var (p1,p2) := (dPoints[k1],dPoints[k2]);
    var v := p2 - p1;
    var pp1 := p1 + v.Norm * Radius;
    var pp2 := p2 - v.Norm * Radius;
    Arrow(pp1,pp2);
    var cc := p1 + v/2;
    var txt := weight.ToString;
    var sz := TextSize(txt);
    FillRectangle(cc.X - sz.Width/2, cc.Y - sz.Height/2, sz.Width, sz.Height);
    TextOut(cc.x,cc.y,weight,Alignment.Center);
  end;
end.