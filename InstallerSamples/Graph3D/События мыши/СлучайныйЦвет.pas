uses Graph3D;

begin 
  var s := Sphere(3,0,1,1);
  var b := Box(0,2,0.5,3,2,1);
  var t := Text3D(-5,0,1,'Graph3D',2);
  OnMouseDown += procedure (x,y,mb) -> begin
    if mb<>1 then exit;
    var v := FindNearestObject(x,y);
    if v<>nil then
      v.Color := RandomColor;
  end;
end.