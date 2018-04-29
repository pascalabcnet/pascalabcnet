uses Graph3D;

begin 
  View3D.Title := 'Перетягивайте мышью сферу';
  var s := Sphere(0,0,0,1);
  var obj: Object3D;
  OnMouseDown += procedure (x,y,mb) -> begin
    obj := FindNearestObject(x,y);
    if obj=s then View3D.Title := '';
  end;

  OnMouseUp += procedure (x,y,mb) -> begin
    obj := nil
  end;
  
  OnMouseMove += procedure (x,y,mb) -> begin
    if obj = nil then exit;
    var pp := PlaneXY.PointOnPlane(x,y);
    if pp<>BadPoint then
    begin
      if pp.X<-5 then pp.X := -5;
      if pp.X>5 then pp.X := 5;
      if pp.Y<-5 then pp.Y := -5;
      if pp.Y>5 then pp.Y := 5;
      obj.Position := pp;
      View3D.SubTitle := $'X={pp.x.ToString(2)} Y={pp.y.ToString(2)}';
    end;
  end;
end.