// Ray Tracer с нуля. Код на C++ - отсюда: https://habr.com/ru/post/436790/
// Запускать по Shift-F9 с отключенным Debug-режимом
uses GraphWPF;

type
  Vec3 = record
    x, y, z: real;
    class function operator+(v1, v2: Vec3): Vec3 := new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    class function operator-(v1, v2: Vec3): Vec3 := new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    class function operator-(v: Vec3): Vec3 := new Vec3(-v.x, -v.y, -v.z);
    class function operator*(v: Vec3; r: real): Vec3 := new Vec3(v.x * r, v.y * r, v.z * r);
    class function operator/(v: Vec3; r: real): Vec3 := new Vec3(v.x / r, v.y / r, v.z / r);
    class function operator*(r: real; v: Vec3): Vec3 := new Vec3(v.x * r, v.y * r, v.z * r);
    class function operator*(v1, v2: Vec3): real := v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
    function norm := Sqrt(x * x + y * y + z * z);
    function normalize(l: real := 1): Vec3 := Self/norm;
    constructor(xx,yy,zz: real); begin x := xx; y := yy; z := zz end;
  end;
  
  Vec4 = auto class 
    x0, x1, x2, x3: real;
  end;

function V3(x, y, z: real) := new Vec3(x, y, z); 
function V4(x, y, z, t: real) := new Vec4(x, y, z, t); 

type
  TLight = auto class 
    position: Vec3;
    intensity: real;
  end;
  
  TMaterial = auto class 
    refractive_index: real := 1;
    albedo := v4(1, 0, 0, 0);
    diffuse_color := new Vec3(0, 0, 0);
    specular_exponent: real;
  end;
  
  TSphere = auto class 
    center: vec3;
    radius: real;
    material: TMaterial;
    function RayIntersect(orig, dir: vec3; var t0: real): boolean;
    begin
      var L := center - orig;
      var tca := L * dir;
      var d2 := L * L - tca * tca;
      if d2 > radius * radius then
        begin Result := false; exit end;
      var thc := Sqrt(radius * radius - d2);
      t0 := tca - thc;
      if t0 < 1e-3 then
        t0 := tca + thc;  // offset the original point to avoid occlusion by the object itself
      if t0 < 1e-3 then
        Result := false
      else Result := true;
    end;
    
  end;

function Light(position: Vec3; intensity: real) := new TLight(position, intensity); 

function Material(ri: real; alb: vec4; dif: vec3; spec: real) := new TMaterial(ri, alb, dif, spec); 

function Sphere(center: vec3; radius: real; material: TMaterial) := new TSphere(center, radius, material);

function Reflect(I, N: vec3) := I - N * 2 * (I * N);

function Refract(I, N: vec3; eta_t: real; eta_i: real := 1): vec3;
begin // Snell's law
  var cosi := -max(-1.0, min(1.0, I * N));
  if cosi < 0 then
    begin Result := Refract(I, -N, eta_i, eta_t); exit end; // if the ray comes from the inside the object, swap the air and the media
  var eta := eta_i / eta_t;
  var k := 1 - eta * eta * (1 - cosi * cosi);
  Result := if k < 0 then v3(1, 0, 0) else I * eta + N * (eta * cosi - Sqrt(k)); // k<0 = total reflection, no ray to refract. I refract it anyways, this has no physical meaning
end;

function scene_intersect(orig, dir: vec3; spheres: array of TSphere; var hit, N: vec3; var material: TMaterial): boolean;
begin
  var spheres_dist := real.MaxValue;
  foreach var s in spheres do 
  begin
    var dist_i: real;
    if s.RayIntersect(orig, dir, dist_i) and (dist_i < spheres_dist) then
    begin
      spheres_dist := dist_i;
      hit := orig + dir * dist_i;
      N := (hit - s.center).normalize();
      material := s.material;
    end;
  end;
  
  var checkerboard_dist := real.MaxValue;
  if abs(dir.y) > 1e-3 then 
  begin
    var d := -(orig.y + 4) / dir.y; // the checkerboard plane has equation y = -4
    var pt := orig + dir * d;
    if (d > 1e-3) and (abs(pt.x) < 10) and (pt.z < -10) and (pt.z > -30) and (d < spheres_dist) then 
    begin
      checkerboard_dist := d;
      hit := pt;
      N := v3(0, 1, 0);
      material.diffuse_color := if (Trunc(0.5 * hit.x + 1000) + Trunc(0.5 * hit.z)) and 1 = 1 then v3(0.3, 0.3, 0.3) else v3(0.3, 0.2, 0.1);
    end;
  end;
  Result := min(spheres_dist, checkerboard_dist) < 1000;
end;

function cast_ray(orig, dir: vec3; spheres: array of TSphere; lights: array of TLight; depth: integer := 0): vec3;
begin
  var point,N: Vec3;
  var material := new TMaterial;
  
  if (depth > 4) or not scene_intersect(orig, dir, spheres, point, N, material) then
    begin Result := v3(0.2, 0.7, 0.8); exit end;
  
  var reflect_dir := Reflect(dir, N).normalize();
  var refract_dir := Refract(dir, N, material.refractive_index).normalize();
  var reflect_color := cast_ray(point, reflect_dir, spheres, lights, depth + 1);
  var refract_color := cast_ray(point, refract_dir, spheres, lights, depth + 1);
  
  var (diffuse_light_intensity, specular_light_intensity) := (0.0, 0.0);
  foreach var light in lights do
  begin
    var light_dir := (light.position - point).normalize;
    
    var shadow_pt, trashnrm: vec3;
    var trashmat: TMaterial;
    if scene_intersect(point, light_dir, spheres, shadow_pt, trashnrm, trashmat) and
                ((shadow_pt - point).norm < (light.position - point).norm) then // checking if the point lies in the shadow of the light
      continue;
    
    diffuse_light_intensity += light.intensity * max(0, light_dir * N);
    specular_light_intensity += max(0, -Reflect(-light_dir, N) * dir) ** material.specular_exponent * light.intensity;
  end;
  Result := material.diffuse_color * diffuse_light_intensity * material.albedo.x0 + 
      v3(1.0, 1.0, 1.0) * specular_light_intensity * material.albedo.x1 + reflect_color * material.albedo.x2 + refract_color * material.albedo.x3;
end;

var 
  (width,height) := (1024,768);

function render(width,height: integer; spheres: array of TSphere; lights: array of TLight): array [,] of Color;
begin
  Result := new Color[width,height];  
  {$omp parallel for}
  for var j := 0 to height - 1 do
  begin
    for var i := 0 to width - 1 do
    begin
      var dir_x := (i + 0.5) - width / 2;
      var dir_y := -(j + 0.5) + height / 2;    // this flips the image at the same time
      var dir_z := -height / (2 * tan(PI / 6));
      var c := cast_ray(v3(0, 0, 0), v3(dir_x, dir_y, dir_z).normalize(), spheres, lights);
      var max := max(c.x, c.y, c.z);
      if max > 1 then
        c := c / max;
      Result[i,j] := RGB(Trunc(255*c.x),Trunc(255*c.y),Trunc(255*c.z));
    end;
  end;
end;

var
  ivory := Material(1.0, v4(0.6, 0.3, 0.1, 0.0), v3(0.4, 0.4, 0.3), 50);
  glass := Material(1.5, v4(0.0, 0.5, 0.1, 0.8), v3(0.6, 0.7, 0.8), 125);
  red_rubber := Material(1.0, v4(0.9, 0.1, 0.0, 0.0), v3(0.3, 0.1, 0.1), 10);
  mirror := Material(1.0, v4(0.0, 10.0, 0.8, 0.0), v3(1.0, 1.0, 1.0), 1425);

begin
  Window.SetSize(width,height);
  Window.CenterOnScreen;
  Window.Title := 'Ray tracing';
  var spheres := Arr(
        Sphere(v3(-3, 0, -16), 2, ivory),
        Sphere(v3(-1.0, -1.5, -12), 2, glass),
        Sphere(v3(1.5, -0.5, -18), 3, red_rubber),
        Sphere(v3(7, 5, -18), 4, mirror));
  
  var lights := Arr(
        Light(v3(-20, 20, 20), 1.5),
        Light(v3(30, 50, -25), 1.8),
        Light(v3(30, 20, 30), 1.7));
  
  MillisecondsDelta;
  var pixels := Render(width,height,spheres, lights);
  var m := MillisecondsDelta;
  DrawPixels(0,0,pixels);
  TextOut(0,0,m);
end.
