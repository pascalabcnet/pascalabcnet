uses Graph3D;

begin 
  var s := Sphere(3,0,0,1);
  var b := Box(0,2,0,3,2,1);
  OnMouseDown += procedure (x,y,mb) -> begin
    if mb=1 then 
    begin
      var v := FindObject3D(x,y);
      if v<>nil then
      begin
        var v1 := v as ObjectWithMaterial3D;
        if v1<>nil then
          v1.Color := RandomColor;
      end;
    end;
  end;
end.