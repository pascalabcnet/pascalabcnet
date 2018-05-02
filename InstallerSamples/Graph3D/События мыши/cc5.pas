uses Graph3D;

begin 
  var d := new Dictionary<Object3D,Line3D>;
  
  var pp := Arr(p3D(2,3,0),p3D(-3,0,0),p3D(2,-2,0));
  var p := p3D(0,0,5);
  foreach var x in pp do
  begin
    var lin := Line(x,p);
    Segment3D(x,p);
    var s := Sphere(x,0.2);
    d[s] := lin; // каждому объекту соответствует линия, по которой он может двигаться мышью
  end;
  
  var tr := Polygon3D(Arr(pp));
  
  var obj: Object3D;
  OnMouseDown += procedure (x,y,mb) -> begin
    obj := FindNearestObject(x,y);
    if obj=nil then exit;
    if not d.ContainsKey(obj) then
      obj := nil;
  end;

  OnMouseUp += procedure (x,y,mb) -> begin
    obj := nil
  end;

  OnMouseMove += procedure (x,y,mb) -> begin
    if mb<>1 then exit;
    if obj=nil then exit;
    
    obj.Position := d[obj].NearestPointOnLine(x,y);
    
    // Немного сложно - для изменения координат вершин треугольника
    var pp := d.Keys.Select(o->o.Position).ToArray;
    tr.Points := Arr(pp[0],pp[1],pp[1],pp[2],pp[2],pp[0]);
  end;

end.