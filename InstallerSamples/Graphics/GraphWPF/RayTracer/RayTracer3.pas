// Улучшенный Ray Tracer с красивой сценой
// Добавлены новые материалы, объекты и улучшенное освещение
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
    function cross(v: Vec3): Vec3 := new Vec3(y * v.z - z * v.y, z * v.x - x * v.z, x * v.y - y * v.x);
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
    color: Vec3;
  end;
  
  TMaterial = auto class 
    refractive_index: real := 1;
    albedo := v4(1, 0, 0, 0);
    diffuse_color := new Vec3(0, 0, 0);
    specular_exponent: real;
    emission: Vec3 := new Vec3(0, 0, 0); // Эмиссия для светящихся объектов
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
        t0 := tca + thc;
      if t0 < 1e-3 then
        Result := false
      else Result := true;
    end;
  end;

function Light(position: Vec3; intensity: real; color: Vec3) := new TLight(position, intensity, color); 
function Material(ri: real; alb: vec4; dif: vec3; spec: real; em: vec3 := new Vec3(0,0,0)) := new TMaterial(ri, alb, dif, spec, em); 
function Sphere(center: vec3; radius: real; material: TMaterial) := new TSphere(center, radius, material);

function Reflect(I, N: vec3) := I - N * 2 * (I * N);

function Refract(I, N: vec3; eta_t: real; eta_i: real := 1): vec3;
begin
  var cosi := -max(-1.0, min(1.0, I * N));
  if cosi < 0 then
    begin Result := Refract(I, -N, eta_i, eta_t); exit end;
  var eta := eta_i / eta_t;
  var k := 1 - eta * eta * (1 - cosi * cosi);
  Result := if k < 0 then v3(1, 0, 0) else I * eta + N * (eta * cosi - Sqrt(k));
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
  
  // Улучшенная шахматная доска с волнистым узором
  var checkerboard_dist := real.MaxValue;
  if abs(dir.y) > 1e-3 then 
  begin
    var d := -(orig.y + 4) / dir.y;
    var pt := orig + dir * d;
    if (d > 1e-3) and (abs(pt.x) < 15) and (pt.z < -8) and (pt.z > -35) and (d < spheres_dist) then 
    begin
      checkerboard_dist := d;
      hit := pt;
      N := v3(0, 1, 0);
      
      // Создаем новый материал для пола
      material := new TMaterial;
      
      // Волнистый узор на полу
      var wave := 0.1 * sin(pt.x * 0.5) * sin(pt.z * 0.3);
      var checker := (Trunc(0.5 * hit.x + 1000) + Trunc(0.5 * hit.z)) and 1;
      
      if checker = 1 then
        material.diffuse_color := v3(0.4 + wave, 0.4 + wave, 0.4 + wave)
      else
        material.diffuse_color := v3(0.2 + wave, 0.15 + wave, 0.1 + wave);
      
      material.albedo := v4(0.8, 0.2, 0.0, 0.0);
      material.specular_exponent := 20;
      material.refractive_index := 1.0;
      material.emission := v3(0, 0, 0);
    end;
  end;
  
  Result := min(spheres_dist, checkerboard_dist) < 1000;
end;

function cast_ray(orig, dir: vec3; spheres: array of TSphere; lights: array of TLight; depth: integer := 0): vec3;
begin
  var point,N: Vec3;
  var material := new TMaterial;
  
  if (depth > 5) or not scene_intersect(orig, dir, spheres, point, N, material) then
  begin
    // Градиентное небо вместо однотонного
    var t := 0.5 * (dir.y + 1.0);
    Result := (1.0 - t) * v3(1.0, 1.0, 1.0) + t * v3(0.3, 0.7, 1.0);
    exit;
  end;
  
  // Добавляем эмиссию с учетом формы объекта
  var emission_intensity := 1.0;
  if material.emission.norm > 0 then
  begin
    // Создаем градиент эмиссии от центра к краям для сфер
    var found_sphere: TSphere := nil;
    foreach var s in spheres do
    begin
      var dist_to_center := (point - s.center).norm;
      if dist_to_center <= s.radius + 0.001 then // Небольшая погрешность
      begin
        found_sphere := s;
        break;
      end;
    end;
    
    if found_sphere <> nil then
    begin
      var dist_from_center := (point - found_sphere.center).norm;
      var radius_ratio := dist_from_center / found_sphere.radius;
      // Создаем falloff от центра к краям
      emission_intensity := 1.0 - radius_ratio * radius_ratio;
      emission_intensity := max(0.1, emission_intensity); // Минимальная яркость
    end;
  end;
  
  var result_color := material.emission * emission_intensity;
  
  var reflect_dir := Reflect(dir, N).normalize();
  var refract_dir := Refract(dir, N, material.refractive_index).normalize();
  var reflect_color := cast_ray(point, reflect_dir, spheres, lights, depth + 1);
  var refract_color := cast_ray(point, refract_dir, spheres, lights, depth + 1);
  
  var (diffuse_light_intensity, specular_light_intensity) := (0.0, 0.0);
  var color_influence := new Vec3(0, 0, 0);
  
  foreach var light in lights do
  begin
    var light_dir := (light.position - point).normalize;
    var light_distance := (light.position - point).norm;
    
    var shadow_pt, trashnrm: vec3;
    var trashmat: TMaterial;
    if scene_intersect(point, light_dir, spheres, shadow_pt, trashnrm, trashmat) and
                ((shadow_pt - point).norm < light_distance) then
      continue;
    
    // Учитываем затухание света с расстоянием
    var attenuation := 1.0 / (1.0 + 0.01 * light_distance);
    
    diffuse_light_intensity += light.intensity * max(0, light_dir * N) * attenuation;
    specular_light_intensity += max(0, -Reflect(-light_dir, N) * dir) ** material.specular_exponent * light.intensity * attenuation;
    
    // Добавляем цветное освещение
    var light_contrib := light.color * light.intensity * max(0, light_dir * N) * attenuation;
    color_influence := color_influence + light_contrib;
  end;
  
  // Компонуем итоговый цвет
  var diffuse_component := v3(material.diffuse_color.x * color_influence.x, 
                              material.diffuse_color.y * color_influence.y, 
                              material.diffuse_color.z * color_influence.z) * material.albedo.x0;
  
  result_color := result_color + diffuse_component + 
      v3(1.0, 1.0, 1.0) * specular_light_intensity * material.albedo.x1 + 
      reflect_color * material.albedo.x2 + 
      refract_color * material.albedo.x3;
  
  Result := result_color;
end;

var 
  (width,height) := (1024,768);

function render(width,height: integer; spheres: array of TSphere; lights: array of TLight): array [,] of Color;
begin
  Result := new Color[width,height];  
  var samples := 3; // Количество сэмплов на пиксель (2x2 = 4 сэмпла)
  var inv_samples := 1.0 / (samples * samples);
  
  {$omp parallel for}
  for var j := 0 to height - 1 do
  begin
    for var i := 0 to width - 1 do
    begin
      var final_color := v3(0, 0, 0);
      
      // Суперсэмплинг - берем несколько сэмплов на пиксель
      for var sy := 0 to samples - 1 do
        for var sx := 0 to samples - 1 do
        begin
          // Добавляем небольшое смещение для каждого сэмпла
          var offset_x := (sx + 0.5) / samples;
          var offset_y := (sy + 0.5) / samples;
          
          var dir_x := (i + offset_x) - width / 2;
          var dir_y := -(j + offset_y) + height / 2;
          var dir_z := -height / (2 * tan(PI / 6));
          
          var sample_color := cast_ray(v3(0, 0, 0), v3(dir_x, dir_y, dir_z).normalize(), spheres, lights);
          final_color := final_color + sample_color;
        end;
      
      // Усредняем цвет всех сэмплов
      var c := final_color * inv_samples;
      
      // Ограничиваем значения и применяем простое масштабирование
      var max_val := max(c.x, max(c.y, c.z));
      if max_val > 1 then
        c := c / max_val;
      
      // Убеждаемся, что значения в диапазоне [0,1]
      c.x := c.x.Clamp(0.0, 1.0);
      c.y := c.y.Clamp(0.0, 1.0);
      c.z := c.z.Clamp(0.0, 1.0);
      
      Result[i,j] := RGB(Trunc(255*c.x),Trunc(255*c.y),Trunc(255*c.z));
    end;
  end;
end;

var
  // Улучшенные материалы
  ivory := Material(1.0, v4(0.6, 0.3, 0.1, 0.0), v3(0.4, 0.4, 0.3), 50);
  crystal := Material(1.5, v4(0.0, 0.5, 0.1, 0.8), v3(0.6, 0.8, 0.9), 125);
  red_rubber := Material(1.0, v4(0.9, 0.1, 0.0, 0.0), v3(0.3, 0.1, 0.1), 10);
  gold_mirror := Material(1.0, v4(0.0, 10.0, 0.8, 0.0), v3(1.0, 0.8, 0.3), 1425);
  emerald := Material(1.5, v4(0.1, 0.6, 0.2, 0.1), v3(0.1, 0.8, 0.1), 100);
  glowing_sphere := Material(1.0, v4(0.4, 0.3, 0.1, 0.0), v3(1.0, 0.4, 0.1), 50, v3(0.8, 0.3, 0.1));

begin
  Window.SetSize(width,height);
  Window.CenterOnScreen;
  Window.Title := 'Enhanced Ray Tracer - Beautiful Scene';
  
  // Создаем более интересную сцену
  var spheres := Arr(
        // Центральная композиция
        Sphere(v3(-3, 0, -16), 2, ivory),
        Sphere(v3(-1.0, -1.5, -12), 2, crystal),
        Sphere(v3(1.5, -0.5, -18), 3, red_rubber),
        Sphere(v3(7, 5, -18), 4, gold_mirror),
        
        // Дополнительные объекты для красоты
        Sphere(v3(-8, 2, -20), 1.5, emerald),
        Sphere(v3(4, -2, -14), 1, glowing_sphere),
        Sphere(v3(-5, -3, -25), 2.5, crystal),
        Sphere(v3(10, -1, -25), 1.8, ivory),
        
        // Маленькие декоративные сферы
        Sphere(v3(-2, 3, -12), 0.5, gold_mirror),
        Sphere(v3(3, 1, -15), 0.8, emerald),
        Sphere(v3(-6, 0, -22), 1.2, red_rubber)
    );
  
  // Улучшенное освещение с цветными источниками
  var lights := Arr(
        Light(v3(-20, 20, 20), 1.5, v3(1.0, 0.9, 0.8)),   // Теплый свет
        Light(v3(30, 50, -25), 1.8, v3(0.8, 0.9, 1.0)),  // Холодный свет
        Light(v3(30, 20, 30), 1.7, v3(1.0, 0.8, 0.9)),   // Розоватый свет
        Light(v3(-10, 10, -10), 1.2, v3(0.9, 1.0, 0.8))  // Зеленоватый свет
    );
  
  WritelnFormat('Rendering {0}x{1} scene...', width, height);
  MillisecondsDelta;
  var pixels := Render(width, height, spheres, lights);
  var m := MillisecondsDelta;
  
  DrawPixels(0, 0, pixels);
  TextOut(5, 5, Format('Rendered in {0} ms', m), Colors.White);
  TextOut(5, 25, Format('Resolution: {0}x{1} (2x2 AA)', width, height), Colors.White);
  TextOut(5, 45, Format('Objects: {0} spheres, {1} lights', spheres.Length, lights.Length), Colors.White);
end.