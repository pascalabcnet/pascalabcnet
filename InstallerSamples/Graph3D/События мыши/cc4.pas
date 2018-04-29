uses Graph3D;

begin 
  View3D.Title := 'Перетягивайте мышью сферу';
  
  var p1 := p3D(2,3,0);
  var p := p3D(0,0,5);
  var lin := Line(p,p1);
  Segment3D(p,p1);
  var s := Sphere(p1,0.2);

  var obj: Object3D;
  OnMouseDown += procedure (x,y,mb) -> begin
    obj := FindNearestObject(x,y);
    if obj=s then View3D.Title := '';
  end;

  OnMouseUp += procedure (x,y,mb) -> begin
    obj := nil;
  end;

  OnMouseMove += procedure (x,y,mb) -> begin
    if mb<>1 then exit;
    if obj=nil then exit;
    
    obj.Position := lin.NearestPointOnLine(x,y);
  end;
end.