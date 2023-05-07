// Copyright (©) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
/// Модуль трёхмерной графики
unit Graph3D;

{$reference System.Xml.dll}
{$reference '%GAC%\HelixToolkit.Wpf.dll'}

interface

uses GraphWPFBase;

uses System.Windows;
uses System.Windows.Controls;
uses System.Windows.Shapes;
uses System.Windows.Media;
uses System.Windows.Media.Animation;
uses System.Windows.Media.Media3D;

uses System.Windows.Markup; 
uses System.XML; 
uses System.IO; 
uses System.Threading;
uses System.Windows.Input;
uses System.Runtime.Serialization;

uses HelixToolkit.Wpf;
//uses Petzold.Media3D;

//{{{doc: Начало секции 1 }}} 
type
// -----------------------------------------------------
//>>     Типы модуля Graph3D # Graph3D types
// -----------------------------------------------------
  /// Тип клавиши
  Key = System.Windows.Input.Key;
  /// Цветовые константы
  Colors = System.Windows.Media.Colors;
  /// Тип цвета
  GColor = System.Windows.Media.Color;
  /// Тип материала
  GMaterial = System.Windows.Media.Media3D.Material;
  /// Тип диффузного материала
  GDiffuseMaterial = System.Windows.Media.Media3D.DiffuseMaterial;
  /// Тип зеркальногоы материала
  GSpecularMaterial = System.Windows.Media.Media3D.SpecularMaterial;
  /// Тип материала свечения
  GEmissiveMaterial = System.Windows.Media.Media3D.EmissiveMaterial;
  /// Тип камеры
  GCamera = System.Windows.Media.Media3D.ProjectionCamera;
  /// Тип прямоугольника
  GRect = System.Windows.Rect;
  /// Тип режима камеры
  CameraMode = HelixToolkit.Wpf.CameraMode;
  /// Тип предопределенных материалов
  GMaterials=Helixtoolkit.Wpf.Materials;
  /// Тип точки
  Point = System.Windows.Point;
  /// Тип 3D-точки
  Point3D = System.Windows.Media.Media3D.Point3D;
  /// Тип 3D-вектора
  Vector3D = System.Windows.Media.Media3D.Vector3D;
  /// Тип 3D-луча
  Ray3D = HelixToolkit.Wpf.Ray3D;
  /// Тип 3D-прямой
  Line3D = class(Ray3D) end;
  /// Тип плоскости
  Plane3D = HelixToolkit.Wpf.Plane3D;
  /// Тип 3D-матрицы
  Matrix3D = System.Windows.Media.Media3D.Matrix3D;

var
  hvp: HelixViewport3D;
  LightsGroup: Model3DGroup;
  gvl: GridLinesVisual3D;

// -----------------------------------------------------
//>>     Короткие функции модуля Graph3D # Graph3D short functions
// -----------------------------------------------------

/// Процедура синхронизации операций с потоком, создающим графические элементы
procedure Invoke(p: procedure);
/// Процедура ускорения вывода. Обновляет экран после всех изменений
procedure Redraw(p: procedure);
/// Возвращает цвет по красной, зеленой и синей составляющей (в диапазоне 0..255)
function RGB(r, g, b: byte): Color;
/// Возвращает цвет по красной, зеленой и синей составляющей и параметру прозрачности (в диапазоне 0..255)
function ARGB(a, r, g, b: byte): Color;
/// Возвращает серый цвет с интенсивностью b
function GrayColor(b: byte): Color;
/// Возвращает случайный цвет
function RandomColor: Color;
/// Возвращает полностью прозрачный цвет
function EmptyColor: Color;
/// Возвращает точку с координатами (x,y)
function Pnt(x, y: real): Point;
/// Возвращает прямоугольник с координатами угла (x,y), шириной w и высотой h
function Rect(x, y, w, h: real): GRect;
/// Возвращает 3D-точку с координатами (x,y,z)
function P3D(x, y, z: real): Point3D;
/// Возвращает 3D-вектор с координатами (x,y,z)
function V3D(x, y, z: real): Vector3D;
/// Возвращает 3D-размер с координатами (x,y,z)
function Sz3D(x, y, z: real): Size3D; 

// -----------------------------------------------------
//>>     Graph3D: функции для создания материалов Materials # Graph3D Materials functions
// ----------------------------------------------------- 
/// Диффузный материал
function DiffuseMaterial(c: Color): Material;
/// Зеркальный материал
function SpecularMaterial(specularBrightness: byte; specularpower: real := 100): Material;
/// Зеркальный материал
function SpecularMaterial(c: Color; specularpower: real := 100): Material;
/// Светящийся материал
function EmissiveMaterial(c: Color): Material;
/// Материал, формируемый на основе изображения. M,N - количество повторений изображения по ширине и высоте
function ImageMaterial(fname: string; M: real := 1; N: real := 1): Material;
/// Радужный материал
function RainbowMaterial: Material;
/// Возвращает материал по умолчанию
function DefaultMaterial: Material;


type
// -----------------------------------------------------
//>>     Graph3D: класс Materials # Graph3D Materials class
// ----------------------------------------------------- 
  ///!#
  /// Класс для создания материалов
  Materials = class
/// Диффузный материал
    class function Diffuse(c: Color) := DiffuseMaterial(c);
/// Зеркальный материал
    class function Specular(specularBrightness: byte := 255; specularpower: real := 100) := SpecularMaterial(specularBrightness, specularpower);
/// Зеркальный материал
    class function Specular(c: Color; specularpower: real := 100) := SpecularMaterial(c, specularpower);
/// Светящийся материал
    class function Emissive(c: Color) := EmissiveMaterial(c);
/// Материал, формируемый на основе изображения. M,N - количество повторений изображения по ширине и высоте
    class function Image(fname: string; M: real := 1; N: real := 1) := ImageMaterial(fname,M,N);
/// Радужный материал
    class function Rainbow := RainbowMaterial;
  end;

type
// -----------------------------------------------------
//>>     Graph3D: класс View3DType # Graph3D View3DType class
// ----------------------------------------------------- 
  ///!#
  /// Класс пространства отображения
  View3DType = class
  private
    procedure SetSGLP(v: boolean) := gvl.Visible := v;
    procedure SetSGL(v: boolean) := Invoke(SetSGLP, v);
    function GetSGL: boolean := InvokeBoolean(()->gvl.Visible);
    
    procedure SetSCIP(v: boolean) := hvp.ShowCameraInfo := v;
    procedure SetSCI(v: boolean) := Invoke(SetSCIP, v);
    function GetSCI: boolean := InvokeBoolean(()->hvp.ShowCameraInfo);
    
    procedure SetSVCP(v: boolean) := hvp.ShowViewCube := v;
    procedure SetSVC(v: boolean) := Invoke(SetSVCP, v);
    function GetSVC: boolean := InvokeBoolean(()->hvp.ShowViewCube);
    
    procedure SetTP(v: string) := hvp.Title := v;
    procedure SetT(v: string) := Invoke(SetTP, v);
    function GetT: string := InvokeString(()->hvp.Title);
    
    procedure SetSTP(v: string) := hvp.SubTitle := v;
    procedure SetST(v: string) := Invoke(SetSTP, v);
    function GetST: string := InvokeString(()->hvp.SubTitle);
    
    procedure SetCMP(v: HelixToolkit.Wpf.CameraMode) := hvp.CameraMode := v;
    procedure SetCM(v: HelixToolkit.Wpf.CameraMode) := Invoke(SetCMP, v);
    function GetCM: HelixToolkit.Wpf.CameraMode := Invoke&<HelixToolkit.Wpf.CameraMode>(()->hvp.CameraMode);
    
    procedure SetBCP(v: GColor) := hvp.Background := new SolidColorBrush(v);
    procedure SetBC(v: GColor) := Invoke(SetBCP, v);
    function GetBC: GColor := Invoke&<GColor>(()->(hvp.Background as SolidColorBrush).Color);
    procedure ExportP(fname: string) := hvp.Viewport.Export(fname, hvp.Background);
  public 
  /// Отображать ли координатную систему
    property ShowCoordinateSystem: boolean 
      read InvokeBoolean(()->hvp.ShowCoordinateSystem) 
      write Invoke(procedure(v: boolean)->hvp.ShowCoordinateSystem := v, value);
  /// Отображать ли координатную сетку
    property ShowGridLines: boolean read GetSGL write SetSGL;
  /// Отображать ли информацию о камере
    property ShowCameraInfo: boolean read GetSCI write SetSCI;
  /// Отображать ли ViewCube
    property ShowViewCube: boolean read GetSVC write SetSVC;
  /// Не отображать координатную систему, координатную сетку и ViewCube 
    procedure HideAll;
    begin
      ShowCoordinateSystem := False;
      ShowGridLines := False;
      ShowViewCube := False;
    end;
  /// Заголовок пространства отображения
    property Title: string read GetT write SetT;
  /// Подзаголовок пространства отображения
    property SubTitle: string read GetST write SetST;
  /// Режим камеры
    property CameraMode: HelixToolkit.Wpf.CameraMode read GetCM write SetCM;
  /// Цвет фона
    property BackgroundColor: GColor read GetBC write SetBC;
  /// Сохраняет содержимое 3d-окна в файл  
    procedure Save(fname: string) := Invoke(ExportP, fname);
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс CameraType # Graph3D CameraType class
// ----------------------------------------------------- 
  ///!#
  /// Класс камеры
  CameraType = class
  private 
    function Cam: GCamera := hvp.Camera;
    procedure SetPP(p: Point3D);
    begin
      Cam.Position := p;
    end;
    
    procedure SetP(p: Point3D) := Invoke(SetPP, p);
    function GetP: Point3D := Invoke&<Point3D>(()->Cam.Position);
    procedure SetLDP(v: Vector3D) := Cam.LookDirection := v;
    procedure SetLD(v: Vector3D) := Invoke(SetLDP, v);
    function GetLD: Vector3D := Invoke&<Vector3D>(()->Cam.LookDirection);
    procedure SetUDP(v: Vector3D) := Cam.UpDirection := v;
    procedure SetUD(v: Vector3D) := Invoke(SetUDP, v);
    function GetUD: Vector3D := Invoke&<Vector3D>(()->Cam.UpDirection);
    procedure SetDP(d: real);
    begin
      var dist := Cam.Position.DistanceTo(P3D(0, 0, 0));
      Cam.Position := Cam.Position.Multiply(d / dist);
    end;
    
    procedure SetD(d: real) := Invoke(SetDP, d);
    function GetD: real := InvokeReal(()->Cam.Position.DistanceTo(P3D(0, 0, 0)));
    procedure MoveByP(x,y,z: real);
    begin
      Cam.Position += V3D(x,y,z)//:= P3D(Cam.Position.
    end;
    procedure MoveByPV(v: Vector3D);
    begin
      Cam.Position += v;
    end;
    procedure AddMoveForceP(x,y,z: real);
    begin
      hvp.CameraController.ShowCameraTarget := True;
      hvp.CameraController.AddMoveForce(x,y,z);
    end;
    procedure AddRotateForceP(x,y: real);
    begin
      hvp.CameraController.AddRotateForce(x,y);
    end;
    procedure RotateP(axis: Vector3D; angle: real);
    begin
      var look := Cam.LookDirection;
      var q := new Quaternion(axis, angle);
      var qConjugate := q;
      qConjugate.Conjugate();
      
      var p := new Quaternion(look.X, look.Y, look.Z, 0);
      var qRotatedPoint := q * p * qConjugate;
      Cam.LookDirection := V3D(qRotatedPoint.X, qRotatedPoint.Y, qRotatedPoint.Z);
    end;
    
  public 
  /// Позиция камеры 
    property Position: Point3D read GetP write SetP;
  /// Направление взгляда камеры 
    property LookDirection: Vector3D read GetLD write SetLD;
  /// Направление "вверх" камеры 
    property UpDirection: Vector3D read GetUD write SetUD;
  /// Расстояние камеры до начала координат
    property Distanse: real read GetD write SetD;

  /// Перемещает камеру на вектор (dx,dy,dz)
    procedure MoveBy(dx,dy,dz: real) := Invoke(MoveByP,dx,dy,dz);
  /// Перемещает камеру на вектор v
    procedure MoveBy(v: Vector3D) := Invoke(MoveByPV,v);
  ///--
    procedure MoveOn(dx,dy,dz: real) := Invoke(MoveByP,dx,dy,dz);
  ///--
    procedure MoveOn(v: Vector3D) := Invoke(MoveByPV,v);
  /// Обеспечивает плавное движение камеры
    procedure AddMoveForce(ForwardForce,RightForce,UpForce: real) := Invoke(AddMoveForceP,RightForce,UpForce,ForwardForce);
  /// Обеспечивает плавное движение камеры вперед с некоторой силой
    procedure AddForwardForce(Force: real := 0.2) := AddMoveForce(Force,0,0);
  /// Обеспечивает плавное движение камеры назад с некоторой силой
    procedure AddBackwardForce(Force: real := 0.2) := AddMoveForce(-Force,0,0);
  /// Обеспечивает плавное движение камеры вправо
    procedure AddRightForce(Force: real := 0.2) := AddMoveForce(0,Force,0);
  /// Обеспечивает плавное движение камеры влево
    procedure AddLeftForce(Force: real := 0.2) := AddMoveForce(0,-Force,0);
  /// Обеспечивает плавное движение камеры вверх
    procedure AddUpForce(Force: real := 0.2) := AddMoveForce(0,0,Force);
  /// Обеспечивает плавное движение камеры вниз
    procedure AddDownForce(Force: real := 0.2) := AddMoveForce(0,0,-Force);
  /// Обеспечивает плавный поворот камеры 
    procedure AddRotateForce(RightForce,UpForce: real) := Invoke(AddRotateForceP,RightForce,UpForce);
  /// Поворачивает камеру на данный угол относительно данной оси
    procedure Rotate(axis: Vector3D; angle: real) := Invoke(RotateP,axis,angle);
  end;
  
  ///!#
  /// Класс источника света
  LightsType = class
  public 
  /// Количество источников света
    property Count: integer read Invoke&<integer>(()->LightsGroup.Children.Count);
  /// Добавляет направленный источник света
    procedure AddDirectionalLight(c: Color; v: Vector3D) := Invoke(()->LightsGroup.Children.Add(new DirectionalLight(c, v)));
  /// Добавляет конусообразный источник света
    procedure AddSpotLight(c: Color; p: Point3D; v: Vector3D; outerconeangle, innerconeangle: real) := Invoke(()->LightsGroup.Children.Add(new SpotLight(c, p, v, outerconeangle, innerconeangle)));
  /// Добавляет точечный источник света
    procedure AddPointLight(c: Color; p: Point3D) := Invoke(()->LightsGroup.Children.Add(new PointLight(c, p)));
  /// Удаляет источник света
    procedure RemoveLight(i: integer) := Invoke(()->LightsGroup.Children.RemoveAt(i));
    {procedure Proba();
    begin
      var p := new PointLight(Colors.Gray, P3D(2, 2, 2));
      p.Color := Colors.Gray;
      p.Position := P3D(3, 3, 3);
    end;}
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс GridLinesType # Graph3D GridLinesType class
// ----------------------------------------------------- 
  ///!#
  /// Класс координатной сетки
  GridLinesType = class
  private 
    procedure SetW(r: real) := Invoke(procedure(r: real)->gvl.Width := r, r);
    function GetW: real := InvokeReal(()->gvl.Width);
    procedure SetL(r: real) := Invoke(procedure(r: real)->gvl.Length := r, r);
    function GetL: real := InvokeReal(()->gvl.Length);
    procedure SetMj(r: real) := Invoke(procedure(r: real)->gvl.MajorDistance := r, r);
    function GetMj: real := InvokeReal(()->gvl.MajorDistance);
    procedure SetMn(r: real) := Invoke(procedure(r: real)->gvl.MinorDistance := r, r);
    function GetMn: real := InvokeReal(()->gvl.MinorDistance);
    procedure SetN(r: Vector3D) := Invoke(procedure(r: Vector3D)->gvl.Normal := r, r);
    function GetN: Vector3D := Inv(()->gvl.Normal);
  public 
  /// Ширина координаной сетки (размер по оси OY)
    property Width: real read GetW write SetW;
  /// Длина координаной сетки (размер по оси OX)
    property Length: real read GetL write SetL;
  /// Вектор нормали координаной сетки
    property Normal: Vector3D read GetN write SetN;
  /// Большое расстояние между линиями координаной сетки
    property MajorDistance: real read GetMj write SetMj;
  /// Маленькое расстояние между линиями координаной сетки
    property MinorDistance: real read GetMn write SetMn;
  end;

type
  AnimationBase = class;
  ObjectWithChildren3D = class;
  
// -----------------------------------------------------
//>>     Graph3D: класс Object3D # Graph3D Object3D class
// ----------------------------------------------------- 
  [Serializable]
  ///!#
  /// Базовый класс трехмерных объектов
  Object3D = class(ISerializable)
    //(DependencyObject) // для генерации документации
  public
    model: Visual3D;
  private 
    Parent: ObjectWithChildren3D;
    transfgroup := new Transform3DGroup; 

    rotatetransform := new MatrixTransform3D;
    scaletransform := new ScaleTransform3D;
    transltransform: TranslateTransform3D;
    rotatetransform_absolute := new MatrixTransform3D;

    procedure AddToObject3DList;
    procedure DeleteFromObject3DList;
  protected  
    procedure CreateBase0(m: Visual3D; x, y, z: real);
    begin
      model := m;
      transltransform := new TranslateTransform3D(x, y, z);
      //transfgroup.Children.Add(new MatrixTransform3D); // ответственен за поворот. Не храним в отдельной переменной т.к. при повороте меняется сам объект, а не поля объекта!!!
      
      transfgroup.Children.Add(rotatetransform);
      transfgroup.Children.Add(scaletransform); 
      transfgroup.Children.Add(transltransform);
      transfgroup.Children.Add(rotatetransform_absolute);
      
      model.Transform := transfgroup;
      hvp.Children.Add(model);
      AddToObject3DList;
    end;
  private  
    procedure SetX(xx: real) := Invoke(()->begin transltransform.OffsetX += xx - Self.X; end); 
    function GetX: real := InvokeReal(()->transfgroup.Value.OffsetX);
    procedure SetY(yy: real) := Invoke(()->begin transltransform.OffsetY += yy - Self.Y; end);
    function GetY: real := InvokeReal(()->transfgroup.Value.OffsetY);
    procedure SetZ(zz: real) := Invoke(()->begin transltransform.OffsetZ += zz - Self.Z; end);
    function GetZ: real := InvokeReal(()->transfgroup.Value.OffsetZ);
    function GetPos: Point3D := Invoke&<Point3D>(()->P3D(Self.X, Self.Y, Self.Z));
    
    function FindVisual(v: Visual3D): Object3D; virtual;
    begin
      if model = v then
        Result := Self
    end;
    
    function GetColor: GColor := EmptyColor;
    procedure SetColor(c: GColor);
    begin end;
  
  protected 
    function CreateObject: Object3D; virtual;// нужно для клонирования
    begin
      Result := nil;
    end;
    
    procedure CloneChildren(from: Object3D); virtual;
    begin
    end;
    
    function CloneT: Object3D; virtual;
    begin
      Result := CreateObject;
      Result.CloneChildren(Self);
      var ind := (model.Transform as Transform3DGroup).Children.IndexOf(rotatetransform);
      (Result.model.Transform as Transform3DGroup).Children[ind] := (model.Transform as Transform3DGroup).Children[ind].Clone;
      Result.rotatetransform := (Result.model.Transform as Transform3DGroup).Children[ind] as MatrixTransform3d;
      
      ind := transfgroup.Children.IndexOf(rotatetransform_absolute);
      (Result.model.Transform as Transform3DGroup).Children[ind] := (model.Transform as Transform3DGroup).Children[ind].Clone;
      Result.rotatetransform_absolute := (Result.model.Transform as Transform3DGroup).Children[ind] as MatrixTransform3d;
      
      ind := transfgroup.Children.IndexOf(transltransform);
      (Result.model.Transform as Transform3DGroup).Children[ind] := (model.Transform as Transform3DGroup).Children[ind].Clone;
      Result.transltransform := (Result.model.Transform as Transform3DGroup).Children[ind] as TranslateTransform3D;

      //(Result.model.Transform as Transform3DGroup).Children[1] := (model.Transform as Transform3DGroup).Children[1].Clone;
      //(Result.model.Transform as Transform3DGroup).Children[2] := (model.Transform as Transform3DGroup).Children[2].Clone;
      //(Result.model.Transform as Transform3DGroup).Children[3] := (model.Transform as Transform3DGroup).Children[3].Clone; //- почему-то это не нужно!!! с ним не работает!
    end;
  
  public
    function CreateModel: Visual3D; virtual;
    begin
      Result := nil;
    end;
  
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      //Invoke(procedure -> begin
        info.AddValue('rotatetransform', rotatetransform.Value, typeof(Matrix3D));
        info.AddValue('scalex', scaletransform.ScaleX, typeof(real));
        info.AddValue('scaley', scaletransform.ScaleY, typeof(real));
        info.AddValue('scalez', scaletransform.ScaleZ, typeof(real));
        info.AddValue('offsetx', transltransform.OffsetX, typeof(real));
        info.AddValue('offsety', transltransform.OffsetY, typeof(real));
        info.AddValue('offsetz', transltransform.OffsetZ, typeof(real));
        info.AddValue('rotatetransform_absolute', rotatetransform_absolute.Value, typeof(Matrix3D));
      //end);
    end;

    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      //Invoke(procedure -> begin
        model := CreateModel;
        rotatetransform := new MatrixTransform3D(Matrix3D(info.GetValue('rotatetransform', typeof(Matrix3D))));
        var scalex := real(info.GetValue('scalex', typeof(real)));
        var scaley := real(info.GetValue('scaley', typeof(real)));
        var scalez := real(info.GetValue('scalez', typeof(real)));
        scaletransform := new ScaleTransform3D(scalex,scaley,scalez);
        
        var offsetx := real(info.GetValue('offsetx', typeof(real)));
        var offsety := real(info.GetValue('offsety', typeof(real)));
        var offsetz := real(info.GetValue('offsetz', typeof(real)));
  
        transltransform := new TranslateTransform3D(offsetx,offsety,offsetz);
  
        rotatetransform_absolute := new MatrixTransform3D(Matrix3D(info.GetValue('rotatetransform_absolute', typeof(Matrix3D))));
        transfgroup := new Transform3DGroup; 
        transfgroup.Children.Add(rotatetransform);
        transfgroup.Children.Add(scaletransform); 
        transfgroup.Children.Add(transltransform);
        transfgroup.Children.Add(rotatetransform_absolute);
        
        AddToObject3DList;
        //if model<>nil then
        begin
          model.Transform := transfgroup;
          hvp.Children.Add(model);
        end;
      //end);
    end;
    
    procedure Serialize(fname: string);
    
    static function DeSerialize(fname: string): Object3D;
    
    constructor(model: Visual3D) := CreateBase0(model, 0, 0, 0);
    
  /// Координата X
    property X: real read GetX write SetX;
  /// Координата Y
    property Y: real read GetY write SetY;
  /// Координата Z
    property Z: real read GetZ write SetZ;
  /// Направление движения (используется методом MoveTime)
    auto property Direction: Vector3D;
  /// Скорость в направлении Direction
    auto property Velocity: real := 3;
  /// Перемещает 3D-объект к точке (xx,yy,zz)
    function MoveTo(xx, yy, zz: real): Object3D := 
    Invoke&<Object3D>(()->begin
      transltransform.OffsetX += xx - Self.X;
      transltransform.OffsetY += yy - Self.Y;
      transltransform.OffsetZ += zz - Self.Z;
      Result := Self;
    end);
  /// Перемещает 3D-объект к точке p
    function MoveTo(p: Point3D): Object3D := MoveTo(p.X, p.y, p.z);
  /// Перемещает 3D-объект на вектор (dx,dy,dz)
    function MoveBy(dx, dy, dz: real): Object3D := MoveTo(x + dx, y + dy, z + dz);
  /// Перемещает 3D-объект на вектор v
    function MoveBy(v: Vector3D): Object3D := MoveBy(v.X, v.Y, v.Z);
  /// Перемещает x-координату 3D-объекта на dx
    function MoveByX(dx: real): Object3D := MoveBy(dx, 0, 0);
  /// Перемещает y-координату 3D-объекта на dy
    function MoveByY(dy: real): Object3D := MoveBy(0, dy, 0);
  /// Перемещает z-координату 3D-объекта на dz
    function MoveByZ(dz: real): Object3D := MoveBy(0, 0, dz);
  ///--
    function MoveOn(dx, dy, dz: real): Object3D := MoveTo(x + dx, y + dy, z + dz);
  ///--
    function MoveOn(v: Vector3D): Object3D := MoveBy(v.X, v.Y, v.Z);
  ///--
    function MoveOnX(dx: real): Object3D := MoveBy(dx, 0, 0);
  ///--
    function MoveOnY(dy: real): Object3D := MoveBy(0, dy, 0);
  ///--
    function MoveOnZ(dz: real): Object3D := MoveBy(0, 0, dz);
  /// Перемещает 3D-объект вдоль вектора Direction со скоростью Velocity за время dt
    procedure MoveTime(dt: real); virtual;
    begin
      var dx := Direction.X;
      var dy := Direction.Y;
      var dz := Direction.Z;
      var len := Sqrt(dx*dx+dy*dy+dz*dz);
      if len = 0 then
        exit;
      var dvx := dx/len*Velocity;
      var dvy := dy/len*Velocity;
      var dvz := dz/len*Velocity;
      MoveBy(dvx*dt,dvy*dt,dvz*dt);
    end;
  /// Цвет 3D-объекта
    property Color: GColor read GetColor write SetColor; virtual;
    
  /// Локальная ось X в глобальных координатах
    property LocalAxisX: Vector3D 
      read Invoke&<Vector3D>(()->model.GetTransform.Transform(new Vector3D(1,0,0)));
  /// Локальная ось X в глобальных координатах
    property LocalAxisY: Vector3D 
      read Invoke&<Vector3D>(()->model.GetTransform.Transform(new Vector3D(0,1,0)));
  /// Локальная ось X в глобальных координатах
    property LocalAxisZ: Vector3D 
      read Invoke&<Vector3D>(()->model.GetTransform.Transform(new Vector3D(0,0,1)));
    
  /// Перемещает 3D-объект к точке (x,y,z) в локальных координатах
    procedure MoveToLocal(x,y,z: real);
    begin
      var p := Invoke&<Point3D>(()->model.GetTransform.Transform(new Point3D(x,y,z)));
      MoveTo(p);
    end;
  /// Перемещает 3D-объект к точке p в локальных координатах  
    procedure MoveToLocal(p: Point3D) := MoveToLocal(p.x,p.y,p.z);
  /// Перемещает 3D-объект на вектор (dx,dy,dz) в локальных координатах
    procedure MoveByLocal(dx,dy,dz: real);
    begin
      // x,y,z в локальных координатах всегда нули
     // MoveToLocal(dx, dy, dz);
      var v := LocalAxisX*dx + LocalAxisY*dy + LocalAxisZ*dz;
      MoveBy(v);
    end;
  /// Перемещает 3D-объект на вектор v в локальных координатах
    procedure MoveByLocal(v: Vector3D) := MoveByLocal(v.x,v.y,v.z);
/// Возвращает анимацию перемещения объекта к точке (x, y, z) за seconds секунд в локальных координатах
    function AnimMoveToLocal(x, y, z: real; seconds: real := 1): AnimationBase;
    begin
      var p := Invoke&<Point3D>(()->model.Transform.Transform(new Point3D(x,y,z)));
      Result := AnimMoveTo(p);
    end;
/// Возвращает анимацию перемещения объекта к точке p за seconds секунд в локальных координатах
    function AnimMoveToLocal(p: Point3D; seconds: real := 1): AnimationBase 
      := AnimMoveToLocal(p.x,p.y,p.z,seconds);
/// Возвращает анимацию перемещения объекта на вектор (dx, dy, dz) за seconds секунд в локальных координатах
    function AnimMoveByLocal(dx, dy, dz: real; seconds: real := 1): AnimationBase;
    begin
      var v := LocalAxisX*dx + LocalAxisY*dy + LocalAxisZ*dz;
      Result := AnimMoveBy(v);
      //Result := AnimMoveToLocal(dx, dy, dz, seconds);
    end;
/// Возвращает анимацию перемещения объекта на вектор v за seconds секунд в локальных координатах
    function AnimMoveByLocal(v: Vector3D; seconds: real := 1): AnimationBase
      := AnimMoveByLocal(v.x,v.y,v.z,seconds);
  private
    procedure MoveToProp(p: Point3D) := MoveTo(p);
  
  public 
  /// Позиция 3D-объекта
    property Position: Point3D read GetPos write MoveToProp;
    
  /// Будущая позиция 3D-объекта по прошествии времени dt
    function PositionAfterTime(dt: real): Point3D;
    begin
      var dx := Direction.X;
      var dy := Direction.Y;
      var dz := Direction.Z;
      var len := Sqrt(dx*dx+dy*dy+dz*dz);
      if len = 0 then
      begin  
        Result := Position;
        exit;
      end;
      var dvx := dx/len*Velocity;
      var dvy := dy/len*Velocity;
      var dvz := dz/len*Velocity;
      Result := P3D(X + dvx*dt, Y + dvy*dt, Z + dvz*dt);
    end;
    
  /// Масштабирует 3D-объект в f раз
    function Scale(f: real): Object3D := 
    Invoke&<Object3D>(()->begin
      scaletransform.ScaleX *= f;
      scaletransform.ScaleY *= f;
      scaletransform.ScaleZ *= f;
      Result := Self;
    end);
  /// Масштабирует 3D-объект в f раз по оси OX
    function ScaleX(f: real): Object3D := 
    Invoke&<Object3D>(()->begin
      scaletransform.ScaleX *= f;
      Result := Self;
    end);
  /// Масштабирует 3D-объект в f раз по оси OY
    function ScaleY(f: real): Object3D := 
    Invoke&<Object3D>(()->begin
      scaletransform.ScaleY *= f;
      Result := Self;
    end);
  /// Масштабирует 3D-объект в f раз по оси OZ
    function ScaleZ(f: real): Object3D := 
    Invoke&<Object3D>(()->begin
      scaletransform.ScaleZ *= f;
      Result := Self;
    end);
    /// Поворачивает объект на угол angle вокруг оси axis
    function Rotate(axis: Vector3D; angle: real): Object3D := 
    Invoke&<Object3D>(()->begin
      var m := Matrix3D.Identity;
      m.Rotate(new Quaternion(axis, angle));
      var ind := transfgroup.Children.IndexOf(rotatetransform);
      rotatetransform := new MatrixTransform3D(m * rotatetransform.Value);
      transfgroup.Children[ind] := rotatetransform;
      Result := Self;
    end);
    /// Поворачивает объект на угол angle вокруг оси axis относительно точки center (её координаты задаются относительно центра объекта)
    function RotateAt(axis: Vector3D; angle: real; center: Point3D): Object3D :=
    Invoke&<Object3D>(()->begin
      var m := Matrix3D.Identity;
      m.RotateAt(new Quaternion(axis, angle), center);
      var ind := transfgroup.Children.IndexOf(rotatetransform);
      rotatetransform := new MatrixTransform3D(m * rotatetransform.Value);
      transfgroup.Children[ind] := rotatetransform;
      Result := Self;
    end);
    /// Поворачивает объект на угол angle вокруг оси axis относительно точки center (в абсолютных координатах)
    function RotateAtAbsolute(axis: Vector3D; angle: real; center: Point3D): Object3D :=
    Invoke&<Object3D>(()->begin
      var m := Matrix3D.Identity;
      m.RotateAt(new Quaternion(axis, angle), center);
      var ind := transfgroup.Children.IndexOf(rotatetransform_absolute);
      rotatetransform_absolute := new MatrixTransform3D(rotatetransform_absolute.Value * m);
      transfgroup.Children[ind] := rotatetransform_absolute;
      Result := Self;
    end);
    /// Возвращает анимацию перемещения объекта к точке (x, y, z) за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimMoveTo(x, y, z: real; seconds: real; Completed: procedure): AnimationBase;
    /// Возвращает анимацию перемещения объекта к точке (x, y, z) за seconds секунд
    function AnimMoveTo(x, y, z: real; seconds: real := 1): AnimationBase := AnimMoveTo(x,y,z,seconds,nil);
    /// Возвращает анимацию перемещения объекта к точке p за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimMoveTo(p: Point3D; seconds: real; Completed: procedure) := AnimMoveTo(p.x, p.y, p.z, seconds, Completed);
    /// Возвращает анимацию перемещения объекта к точке p за seconds секунд
    function AnimMoveTo(p: Point3D; seconds: real := 1) := AnimMoveTo(p.x, p.y, p.z, seconds, nil);
    /// Возвращает анимацию перемещения объекта по траектории, заданной последовательностью точек trajectory за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimMoveTrajectory(trajectory: sequence of Point3D; seconds: real; Completed: procedure): AnimationBase;
    /// Возвращает анимацию перемещения объекта по траектории, заданной последовательностью точек trajectory за seconds секунд
    function AnimMoveTrajectory(trajectory: sequence of Point3D; seconds: real := 1): AnimationBase := AnimMoveTrajectory(trajectory,seconds,nil);

    /// Возвращает анимацию перемещения объекта на вектор (dx, dy, dz) за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimMoveBy(dx, dy, dz: real; seconds: real; Completed: procedure): AnimationBase;
    /// Возвращает анимацию перемещения объекта на вектор (dx, dy, dz) за seconds секунд
    function AnimMoveBy(dx, dy, dz: real; seconds: real := 1): AnimationBase := AnimMoveBy(dx,dy,dz,seconds,nil);

    /// Возвращает анимацию перемещения объекта на вектор v за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimMoveBy(v: Vector3D; seconds: real; Completed: procedure) := AnimMoveBy(v.x, v.y, v.z, seconds, Completed);
    /// Возвращает анимацию перемещения объекта на вектор v за seconds секунд
    function AnimMoveBy(v: Vector3D; seconds: real := 1) := AnimMoveBy(v.x, v.y, v.z, seconds, nil);

    /// Возвращает анимацию перемещения объекта по оси OX на величину dx за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimMoveByX(dx: real; seconds: real; Completed: procedure) := AnimMoveBy(dx, 0, 0, seconds, Completed);
    /// Возвращает анимацию перемещения объекта по оси OX на величину dx за seconds секунд
    function AnimMoveByX(dx: real; seconds: real) := AnimMoveByX(dx, seconds, nil);
    /// Возвращает анимацию перемещения объекта по оси OX на величину dx за 1 секунду
    function AnimMoveByX(dx: real) := AnimMoveByX(dx, 1, nil);

    /// Возвращает анимацию перемещения объекта по оси OY на величину dy за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimMoveByY(dy: real; seconds: real; Completed: procedure) := AnimMoveBy(0, dy, 0, seconds, Completed);
    /// Возвращает анимацию перемещения объекта по оси OY на величину dy за seconds секунд
    function AnimMoveByY(dy: real; seconds: real) := AnimMoveByY(dy, seconds, nil);
    /// Возвращает анимацию перемещения объекта по оси OZ на величину dz за 1 секунду
    function AnimMoveByY(dy: real) := AnimMoveByY(dy, 1, nil);

    /// Возвращает анимацию перемещения объекта по оси OZ на величину dz за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimMoveByZ(dz: real; seconds: real; Completed: procedure) := AnimMoveBy(0, 0, dz, seconds, Completed);
    /// Возвращает анимацию перемещения объекта по оси OZ на величину dz за seconds секунд
    function AnimMoveByZ(dz: real; seconds: real) := AnimMoveByZ(dz, seconds, nil);
    /// Возвращает анимацию перемещения объекта по оси OZ на величину dz за 1 секунду
    function AnimMoveByZ(dz: real) := AnimMoveByZ(dz, 1, nil);

    /// Возвращает анимацию масштабирования объекта на величину sc за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimScale(sc: real; seconds: real; Completed: procedure): AnimationBase;
    /// Возвращает анимацию масштабирования объекта по оси OX на величину sc за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimScaleX(sc: real; seconds: real; Completed: procedure): AnimationBase;
    /// Возвращает анимацию масштабирования объекта по оси OY на величину sc за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimScaleY(sc: real; seconds: real; Completed: procedure): AnimationBase;
    /// Возвращает анимацию масштабирования объекта по оси OZ на величину sc за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimScaleZ(sc: real; seconds: real; Completed: procedure): AnimationBase;

    /// Возвращает анимацию масштабирования объекта на величину sc за seconds секунд
    function AnimScale(sc: real; seconds: real := 1): AnimationBase := AnimScale(sc,seconds,nil);
    /// Возвращает анимацию масштабирования объекта по оси OX на величину sc за seconds секунд
    function AnimScaleX(sc: real; seconds: real := 1): AnimationBase := AnimScale(sc,seconds,nil);
    /// Возвращает анимацию масштабирования объекта по оси OY на величину sc за seconds секунд
    function AnimScaleY(sc: real; seconds: real := 1): AnimationBase := AnimScale(sc,seconds,nil);
    /// Возвращает анимацию масштабирования объекта по оси OZ на величину sc за seconds секунд
    function AnimScaleZ(sc: real; seconds: real := 1): AnimationBase := AnimScale(sc,seconds,nil);

    /// Возвращает анимацию поворота объекта вокруг вектора (vx,vy,vz) на величину angle за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimRotate(vx, vy, vz, angle: real; seconds: real; Completed: procedure): AnimationBase;
    /// Возвращает анимацию поворота объекта вокруг вектора (vx,vy,vz) на величину angle за seconds секунд
    function AnimRotate(vx, vy, vz, angle: real; seconds: real := 1): AnimationBase := AnimRotate(vx,vy,vz,angle,seconds,nil);

    /// Возвращает анимацию поворота объекта вокруг вектора v, направленного из центра объекта, на величину angle за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimRotate(v: Vector3D; angle: real; seconds: real; Completed: procedure) := AnimRotate(v.x, v.y, v.z, angle, seconds, Completed);
    /// Возвращает анимацию поворота объекта вокруг вектора v, направленного из центра объекта, на величину angle за seconds секунд
    function AnimRotate(v: Vector3D; angle: real; seconds: real := 1) := AnimRotate(v.x, v.y, v.z, angle, seconds, nil);
    
    /// Возвращает анимацию поворота объекта вокруг вектора axis, направленного из точки center (её координаты задаются относительно центра объекта), на величину angle за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimRotateAt(axis: Vector3D; angle: real; center: Point3D; seconds: real; Completed: procedure): AnimationBase;
    /// Возвращает анимацию поворота объекта вокруг вектора axis, направленного из точки center (её координаты задаются относительно центра объекта), на величину angle за seconds секунд
    function AnimRotateAt(axis: Vector3D; angle: real; center: Point3D; seconds: real := 1): AnimationBase := AnimRotateAt(axis,angle,center,seconds,nil);
    /// Возвращает анимацию поворота объекта вокруг вектора axis, направленного из точки center (в абсолютных координатах), на величину angle за seconds секунд. В конце анимации выполняется процедура Completed
    function AnimRotateAtAbsolute(axis: Vector3D; angle: real; center: Point3D; seconds: real; Completed: procedure): AnimationBase;
    /// Возвращает анимацию поворота объекта вокруг вектора axis, направленного из точки center (в абсолютных координатах), на величину angle за seconds секунд
    function AnimRotateAtAbsolute(axis: Vector3D; angle: real; center: Point3D; seconds: real := 1): AnimationBase := AnimRotateAtAbsolute(axis,angle,center,seconds,nil);

    /// Клонирует 3D-объект
    function Clone: Object3D := Invoke&<Object3D>(CloneT);
  private    
    procedure SaveP(fname: string);
    begin
      var f := new System.IO.StreamWriter(fname);
      XamlWriter.Save(Model, f);
      f.Close()
    end;
  public    
    /// Сохраняет 3D-объект в файл
    procedure Save(fname: string); virtual := Invoke(SaveP, fname); // надо её сделать виртуальной!
    class function Load(fname: string): Object3D := Invoke&<Object3D>(()->begin
      var m := XamlReader.Load(new System.IO.FileStream(fname, System.IO.FileMode.Open)) as Visual3D;
      Result := new Object3D(m);
    end);
    /// Удаляет 3D-объект
    procedure Destroy(); virtual := DeleteFromObject3DList;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс ObjectWithChildren3D # Graph3D ObjectWithChildren3D class
// ----------------------------------------------------- 
  [Serializable]
  /// 3D-объект с дочерними подобъектами
  ObjectWithChildren3D = class(Object3D,ISerializable) // model is ModelVisual3D
  private 
    l := new List<Object3D>;
    
    procedure DestroyT;
    begin
    end;
    
    procedure AddT(obj: Object3D);
    begin
      var p := Self;
      while p <> nil do
      begin
        if obj = p then
          raise new System.ArgumentException('Group.Add: Нельзя в дочерние элементы группы добавить себя или своего предка');
        p := p.Parent
      end;
      if obj.Parent = Self then
        exit;
      
      if obj.Parent = nil then
        hvp.Children.Remove(obj.model)
      else 
      begin
        var q := obj.Parent.model as ModelVisual3D;
        q.Children.Remove(obj.model);
        obj.Parent.l.Remove(obj);
      end;
      
      (model as ModelVisual3D).Children.Add(obj.model);
      l.Add(obj);  
      obj.Parent := Self;
    end;
    
    procedure RemoveT(obj: Object3D);
    begin
      var b := (model as ModelVisual3D).Children.Remove(obj.model);
      if not b then exit;
      l.Remove(obj);
      hvp.Children.Add(obj.model);
      obj.Parent := nil;
    end;
    
    function GetObj(i: integer): Object3D := l[i];
    function CountT: integer := (model as ModelVisual3D).Children.Count;
    function FindVisual(v: Visual3D): Object3D; override;
    begin
      Result := nil;
      if model = v then
        Result := Self
      else
        foreach var x in l do
        begin
          Result := x.FindVisual(v);
          if Result <> nil then
            exit;
        end;
    end;
  
  protected 
    procedure CloneChildren(from: Object3D); override;
    begin
      var ll := (from as ObjectWithChildren3D).l;
      if ll.Count = 0 then exit;
      foreach var xx in ll do
        AddChild(xx.Clone);
    end;

    function ColorToLongWord(c: GColor): longword;
    begin
      Result := ((c.A * 256 + c.R) * 256 + c.G) * 256 + c.B;
    end;
    
    function LongWordToColor(w: longword): GColor;
    begin
      var b := w mod 256;
      w := w div 256;
      var g := w mod 256;
      w := w div 256;
      var r := w mod 256;
      var a := w div 256;
      Result := ARGB(a,r,g,b);
    end;
  
  public  
  /// Добавить дочерний подобъект
    procedure AddChild(obj: Object3D) := Invoke(AddT, obj);
  /// Добавить дочерний подобъект
    procedure AddChilds(params arr: array of Object3D) := arr.ForEach(obj->AddChild(obj));
  /// Удалить дочерний подобъект
    procedure RemoveChild(obj: Object3D) := Invoke(RemoveT, obj);
  /// i-тый дочерний подобъект
    property Items[i: integer]: Object3D read GetObj; default;
    
  /// Количество дочерних подобъектов
    function Count: integer := Invoke&<integer>(CountT);
    
  private
    procedure DestroyP;
    begin
      if Parent = nil then
        hvp.Children.Remove(model)
      else 
      begin
        var q := Parent.model as ModelVisual3D;
        q.Children.Remove(model);
        Parent.l.Remove(Self);
      end;
      model := nil;
    end;
    
  public
  /// Удалить дочерний подобъект
    procedure Destroy; override;
    begin
      inherited Destroy;
      Invoke(DestroyP);
    end;
    
    function CreateModel: Visual3D; override;
    begin
      Result := nil;
    end;
  
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('listchildrencount',l.Count,typeof(integer));
      for var i:=0 to l.Count -1 do
        info.AddValue('children'+i, l[i], typeof(Object3D));
    end;

    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      l := new List<Object3D>;
      var count := integer(info.GetValue('listchildrencount',typeof(integer)));
      for var i:=0 to count-1 do
      begin
        var xx := info.GetValue('children'+i, typeof(Object3D)) as Object3D;
        AddChild(xx);
      end;
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс ObjectWithMaterial3D # Graph3D ObjectWithMaterial3D class
// ----------------------------------------------------- 
  [Serializable]
  /// 3D-объект с материалом
  ObjectWithMaterial3D = class(ObjectWithChildren3D,ISerializable) // model is MeshElement3D
  private 
    procedure CreateBase(m: MeshElement3D; x, y, z: real; mat: GMaterial);
    begin
      CreateBase0(m, x, y, z);
      if mat = nil then
        mat := DefaultMaterial;
      m.Material := mat;
      //MaterialHelper.ChangeOpacity(mat,0.1);
      //MaterialHelper.ChangeOpacity(BackMaterial,0.1);
      //m.BackMaterial := nil;
    end;
    
    function GetColorP: GColor;
    begin
      Result := EmptyColor;
      var g := Material as System.Windows.Media.Media3D.MaterialGroup;
      if g = nil then exit;      
      var t := g.Children[0] as System.Windows.Media.Media3D.DiffuseMaterial;
      if t = nil then exit;
      var v := t.Brush as System.Windows.Media.SolidColorBrush;
      if v = nil then exit;
      Result := v.Color; 
    end;
    
    function GetColor: GColor := Invoke&<GColor>(GetColorP);
    procedure SetColorP(c: GColor) := (model as MeshElement3D).Material := MaterialHelper.CreateMaterial(c);
    procedure SetColor(c: GColor) := Invoke(SetColorP, c); 
    procedure SetVP(v: boolean) := (model as MeshElement3D).Visible := v;
    procedure SetV(v: boolean) := Invoke(SetVP, v);
    function GetV: boolean := Invoke&<boolean>(()->(model as MeshElement3D).Visible);
    
    procedure SetMP(mat: GMaterial) := (model as MeshElement3D).Material := mat;
    procedure SetMaterial(mat: GMaterial) := Invoke(SetMP, mat);
    function GetMaterial: GMaterial := Invoke&<GMaterial>(()->(model as MeshElement3D).Material);
    procedure SetBMP(mat: GMaterial) := (model as MeshElement3D).BackMaterial := mat;
    procedure SetBMaterial(mat: GMaterial) := Invoke(SetBMP, mat);
    function GetBMaterial: GMaterial := Invoke&<GMaterial>(()->(model as MeshElement3D).BackMaterial);
  public 
  /// Цвет объекта
    property Color: GColor read GetColor write SetColor; override;
  /// Материал объекта
    property Material: GMaterial read GetMaterial write SetMaterial;
  /// Материал задней поверхности объекта
    property BackMaterial: GMaterial read GetBMaterial write SetBMaterial;
  /// Видим ли объект
    property Visible: boolean read GetV write SetV;
    
    function CreateModel: Visual3D; override;
    begin
      Result := nil;
    end;
  private
    procedure SaveMaterialHelper(mat: GMaterial; info: SerializationInfo; i: integer; back: string := '');
    begin
      match mat with
        GDiffuseMaterial(dm): 
          begin
            if dm.Brush is SolidColorBrush(var scb) then
            begin  
              info.AddValue('materialkind' + back + i, 'diffuse' + back, typeof(string));
              info.AddValue('diffusebrushcolor' + back, ColorToLongWord(scb.Color), typeof(longword));
            end
            else if dm.Brush is ImageBrush(var ib) then
            begin
              info.AddValue('materialkind' + back + i, 'diffusetexture' + back, typeof(string));
              var bi := (ib.ImageSource as System.Windows.Media.Imaging.BitmapImage);
              info.AddValue('ibViewPortWidth' + back, ib.Viewport.Width, typeof(real));
              info.AddValue('ibViewPortHeight' + back, ib.Viewport.Height, typeof(real));
              info.AddValue('texturepath' + back, bi.UriSource.ToString, typeof(string));
            end;
          end;
        GSpecularMaterial(spm): 
          begin
            info.AddValue('materialkind' + back + i, 'specular' + back, typeof(string));
            if spm.Brush is SolidColorBrush(var scb) then
              info.AddValue('specularbrushcolor' + back, ColorToLongWord(scb.Color), typeof(longword));
          end;
        GEmissiveMaterial(em): 
          begin
            info.AddValue('materialkind' + back + i, 'emissive' + back, typeof(string));
            if em.Brush is SolidColorBrush(var scb) then
              info.AddValue('emissivebrushcolor' + back, ColorToLongWord(scb.Color), typeof(longword));
          end;
      end;
    end;
    procedure SaveMaterial(mat: GMaterial; info: SerializationInfo; back: string := '');
    begin
      if mat is MaterialGroup (var mg) then
        info.AddValue('materialscount' + back, mg.Children.Count, typeof(integer))
      else info.AddValue('materialscount' + back, 1, typeof(integer));

      if mat is MaterialGroup (var mg) then
      begin  
        var i := 1;
        foreach var m in (mat as MaterialGroup).Children do
        begin  
          SaveMaterialHelper(m as GMaterial, info,i,back);
          i += 1;
        end  
      end  
      else SaveMaterialHelper(mat,info,1,back);
    end;
  public  
///--
    procedure GetObjectData(info: System.Runtime.Serialization.SerializationInfo; context: System.Runtime.Serialization.StreamingContext);
    begin
      inherited GetObjectData(info,context);
      SaveMaterial(Material,info,'');
      SaveMaterial(BackMaterial,info,'back');
    end;
  private  
    function LoadMaterialHelper(info: SerializationInfo; i: integer; back: string := ''): GMaterial;
    begin
      Result := nil;
      var mk := info.GetString('materialkind'+back+i);
      if mk = 'diffuse' + back then
          begin
            var w := info.GetUInt32('diffusebrushcolor'+back);
            var c := LongWordToColor(w);
            Result := Materials.Diffuse(c);
          end
       else if mk = 'diffusetexture' + back then
          begin
            var fname := info.GetString('texturepath'+back);
            var width := info.GetDouble('ibViewPortWidth' + back);
            var height := info.GetDouble('ibViewPortHeight' + back);
            if (width<>1) or (height<>1) then
              Result := Materials.Image(fname,width,height)
            else Result := Materials.Image(fname);
          end
       else if mk =  'specular' + back then
          begin
            var w := info.GetUInt32('specularbrushcolor'+back);
            var c := LongWordToColor(w);
            Result := Materials.Specular(c);
          end
       else if mk = 'emissive' + back then 
          begin
            var w := info.GetUInt32('emissivebrushcolor'+back);
            var c := LongWordToColor(w);
            Result := Materials.Emissive(c);
          end;
    end;
    function LoadMaterial(info: SerializationInfo; back: string := ''): GMaterial;
    begin
      var count := integer(info.GetValue('materialscount'+back, typeof(integer)));
      if count = 1 then
      begin  
        Result := LoadMaterialHelper(info,1,back);
      end
      else
        begin 
          var mg := new MaterialGroup();
          for var i:=1 to count do
          begin
            var m := LoadMaterialHelper(info,i,back);
            mg.Children.Add(m)
          end;
          Result := mg;
        end;  
    end;
  public
    constructor Create(info: System.Runtime.Serialization.SerializationInfo; context: System.Runtime.Serialization.StreamingContext);
    begin
      inherited Create(info,context);
      (model as MeshElement3D).Material := LoadMaterial(info);
      (model as MeshElement3D).BackMaterial := LoadMaterial(info,'back');
    end;
  end;
  
  [Serializable]
  /// Группа 3D-объектов
  Group3D = class(ObjectWithChildren3D,ISerializable)
  protected 
    function CreateObject: Object3D; override := new Group3D(X, Y, Z);
  //public 
  protected 
    constructor(x, y, z: real) := CreateBase0(new ModelVisual3D, x, y, z);
    
    constructor(x, y, z: real; lst: sequence of Object3D);
    begin
      CreateBase0(new ModelVisual3D, x, y, z);
      foreach var xx in lst do
        AddChild(xx);
    end;
  public  
    procedure UnGroup;
    begin
      for var i := l.Count-1 downto 0 do
      begin
        RemoveChild(l[i]);
      end;  
    end;
///--
    function CreateModel: Visual3D; override;
    begin
      Result := new ModelVisual3D;
    end;
///--
    procedure GetObjectData(info: System.Runtime.Serialization.SerializationInfo; context: System.Runtime.Serialization.StreamingContext);
    begin
      inherited GetObjectData(info,context);
    end;
///--
    constructor Create(info: System.Runtime.Serialization.SerializationInfo; context: System.Runtime.Serialization.StreamingContext);
    begin
      inherited Create(info,context);
    end;
    
  end;
  
//------------------------------ Animations -----------------------------------
  
// -----------------------------------------------------
//>>     Graph3D: класс AnimationBase # Graph3D AnimationBase class
// ----------------------------------------------------- 
  /// Базовый класс анимации 3D-объектов
  AnimationBase = class(Object)
  private 
    Element: Object3D;
    Seconds: real;
    // Completed - действие при завершении элементарной (не составной) анимации (имеющей единую продолжительность). 
    // Не учитывается составными анимациями - у них есть AnimationCompleted. Работает точнее чем sb.Completed.
    Completed: procedure; 
    // AnimationCompleted - фигурирует только в WhenCompleted. Неточна. После неё индивидуальные анимации делают ещё один шаг
    AnimationCompleted: procedure;
    ApplyDecorators := new List<Action0>;
    procedure ApplyAllDecorators; virtual;
    begin
      foreach var d in ApplyDecorators do
        d();
    end;
    
    procedure InitAnimWait; virtual;
    begin
    end;
  
  private 
    class function AddDoubleAnimRemainderHelper(d: DoubleAnimationBase; sb: StoryBoard; seconds: real; ttname: string; prop: Object): DoubleAnimationBase;
    begin
      d.Duration := new System.Windows.Duration(System.TimeSpan.FromSeconds(seconds));
      StoryBoard.SetTargetName(d, ttname);
      StoryBoard.SetTargetProperty(d, new PropertyPath(prop));
      sb.Children.Add(d);
      Result := d;
    end;
  
  protected 
    sb: StoryBoard;
    
    class function AddDoubleAnimByName(sb: StoryBoard; toValue, seconds: real; ttname: string; prop: Object): DoubleAnimationBase;
    begin
      var d := new DoubleAnimation();
      d.To := toValue;
      Result := AddDoubleAnimRemainderHelper(d, sb, seconds, ttname, prop);
    end;
    
    class function AddDoubleAnimOnByName(sb: StoryBoard; toValue, seconds: real; ttname: string; prop: Object): DoubleAnimationBase;
    begin
      var d := new DoubleAnimation();
      d.By := toValue;
      Result := AddDoubleAnimRemainderHelper(d, sb, seconds, ttname, prop);
    end;
    
    class function AddDoubleAnimByNameUsingKeyframes(sb: StoryBoard; a: sequence of real; seconds: real; ttname: string; prop: Object): DoubleAnimationBase;
    begin
      var d := new DoubleAnimationUsingKeyframes;
      d.KeyFrames := new DoubleKeyFrameCollection;
      foreach var x in a do
        d.KeyFrames.Add(new LinearDoubleKeyFrame(x)); // не указываем keytime - надеемся, что по секунде
      Result := AddDoubleAnimRemainderHelper(d, sb, seconds, ttname, prop);
    end;
    
    {class function AddDoubleAnimByNameUsingTrajectory(sb: StoryBoard; a: sequence of real; seconds: real; ttname: string; prop: Object; waittime: real := 0.0): DoubleAnimationBase;
    begin
      var d := new DoubleAnimationUsingKeyframes;
      d.KeyFrames := new DoubleKeyFrameCollection;
      foreach var x in a do
        d.KeyFrames.Add(new LinearDoubleKeyFrame(x)); // не указываем keytime - надеемся, что по секунде
      d.Duration := new System.Windows.Duration(System.TimeSpan.FromSeconds(seconds));
      d.BeginTime := System.TimeSpan.FromSeconds(waittime);
      StoryBoard.SetTargetName(d, ttname);
      StoryBoard.SetTargetProperty(d, new PropertyPath(prop));
      sb.Children.Add(d);
      Result := d;
    end;}
    
    function RegisterName(sb: StoryBoard; element: Object; ttname: string): boolean;
    begin
      Result := False;
      if MainWindow.FindName(ttname) = nil then 
      begin
        MainWindow.RegisterName(ttname, element);
        sb.Completed += (o, e) -> begin
          if MainWindow.FindName(ttname) <> nil then 
            MainWindow.UnregisterName(ttname); 
        end;
        Result := True;
      end;
    end;
    
    procedure InitAnim; virtual := InitAnimWait;
  private 
    function CreateStoryboard: StoryBoard;
    begin
      //Println('CreateStoryboard');
      sb := new StoryBoard;
      var storyboardName := 's' + sb.GetHashCode;
      MainWindow.Resources.Add(storyboardName, sb);
      var an := AnimationCompleted;
      sb.Completed += (o, e) -> begin
        //Println('CompletedStoryBoardStart');
        MainWindow.Resources.Remove(storyboardName);
        //Println('CompletedStoryBoardEnd');
        if an <> nil then
          an;
      end;
      
      Result := sb;
    end;
  
  public 
    constructor(e: Object3D; sec: real; Completed: procedure := nil);
    begin
      Self.Completed := Completed;
      (Element, Seconds) := (e, sec);
    end;
    
  /// Устанавливает действие по завершению анимации
    function WhenCompleted(act: procedure): AnimationBase;
    begin
      Self.AnimationCompleted := act;
      Result := Self;
    end;
    
    function Clone: AnimationBase; virtual;
    begin
      Result := Self;
    end;
  
  private 
    procedure BeginT; 
    begin
      sb := CreateStoryboard;
      InitAnim;
      
      ApplyAllDecorators;
      sb.Begin;
    end;
    
    procedure RemoveT := begin
      sb.Pause;
      sb.Remove;
      sb := new Storyboard;
    end;
    procedure ChangeT(a: AnimationBase);
    begin
      sb := CreateStoryboard;
      foreach var d in a.sb.Children do
        sb.Children.Add(d);
    end;
  
  public 
  /// Начинает анимацию
    procedure &Begin; virtual := Invoke(BeginT);
  /// Удаляет анимацию
    procedure Remove := Invoke(RemoveT);
  /// Меняет анимацию на другую
    procedure Change(a: AnimationBase) := Invoke(ChangeT, a);
  /// Делает паузу анимации
    procedure Pause := if sb <> nil then sb.Pause;
  /// Возобновляет анимацию
    procedure Resume := if sb <> nil then sb.Resume;
    
  /// Возвращает продолжительность анимации
    function Duration: real; virtual := seconds;
  /// Указывает анимацию, выполняющуюся после данной
    function &Then(second: AnimationBase): AnimationBase;
  /// Модификатор анимации, выполняющий её бесконечно. Не может быть применен к группе анимаций
    function Forever: AnimationBase; virtual := Self;
  /// Модификатор анимации, возвращающий объект в исходное положение. Не может быть применен к группе анимаций
    function AutoReverse: AnimationBase; virtual := Self;
  /// Модификатор анимации, устанавливающий ускорение анимации в начале и замедление анимации в конце
    function AccelerationRatio(acceleration: real; deceleration: real := 0): AnimationBase; virtual := Self;
  end;
  
  EmptyAnimation = class(AnimationBase)
  public 
    constructor(wait: real) := Seconds := wait;
    procedure InitAnim(); override := InitAnimWait;
  end;
  
 
  Double1AnimationBase = class(AnimationBase)
  private 
    v: real;
    da: DoubleAnimationBase;
  public 
    constructor(e: Object3D; sec: real; value: real; Completed: procedure := nil);
    begin
      inherited Create(e, sec, Completed);
      v := value;
    end;
    
    function Clone: AnimationBase; override := new Double1AnimationBase(Element,Seconds,v,nil);
    
    function AutoReverse: AnimationBase; override;
    begin
      ApplyDecorators.Add(()-> begin
        da.AutoReverse := True;
      end);  
      Result := Self;
    end;
    
    function Forever: AnimationBase; override;
    begin
      ApplyDecorators.Add(()-> begin
        da.RepeatBehavior := RepeatBehavior.Forever;
      end);  
      Result := Self;
    end;
    
    function AccelerationRatio(acceleration: real; deceleration: real := 0): AnimationBase; override;
    begin
      if acceleration < 0 then acceleration := 0;
      if acceleration > 1 then acceleration := 1;
      if deceleration < 0 then deceleration := 0;
      if deceleration > 1 then deceleration := 1;
      if acceleration + deceleration > 1 then
      begin
        acceleration /= acceleration + deceleration;
        deceleration := 1 - acceleration;
      end;
      ApplyDecorators.Add(()-> begin
        da.AccelerationRatio := acceleration;
        da.DecelerationRatio := deceleration;
      end);  
      Result := Self;
    end;
  end;
  
  Double3AnimationBase = class(AnimationBase)
  private 
    x, y, z: real;
    dax, day, daz: DoubleAnimationBase;
  public 
    constructor(e: Object3D; sec: real; xx, yy, zz: real; Completed: procedure := nil);
    begin
      inherited Create(e, sec, Completed);
      (x, y, z) := (xx, yy, zz);
    end;
    
    function Clone: AnimationBase; override := new Double3AnimationBase(Element,Seconds,x,y,z,nil);    
    
    function AutoReverse: AnimationBase; override;
    begin
      ApplyDecorators.Add(()-> begin
        dax.AutoReverse := True;
        day.AutoReverse := True;
        daz.AutoReverse := True;
      end);  
      Result := Self;
    end;
    
    function Forever: AnimationBase; override;
    begin
      ApplyDecorators.Add(()-> begin
        dax.RepeatBehavior := RepeatBehavior.Forever;
        day.RepeatBehavior := RepeatBehavior.Forever;
        daz.RepeatBehavior := RepeatBehavior.Forever;
      end);  
      Result := Self;
    end;
    
    function AccelerationRatio(acceleration: real; deceleration: real := 0): AnimationBase; override;
    begin
      if acceleration < 0 then acceleration := 0;
      if acceleration > 1 then acceleration := 1;
      if deceleration < 0 then deceleration := 0;
      if deceleration > 1 then deceleration := 1;
      if acceleration + deceleration > 1 then
      begin
        acceleration /= acceleration + deceleration;
        deceleration := 1 - acceleration;
      end;
      ApplyDecorators.Add(()-> begin
        dax.AccelerationRatio := acceleration;
        day.AccelerationRatio := acceleration;
        daz.AccelerationRatio := acceleration;
        dax.DecelerationRatio := deceleration;
        day.DecelerationRatio := deceleration;
        daz.DecelerationRatio := deceleration;
      end);  
      Result := Self;
    end;
  end;
  
  OffsetAnimationOn = class(Double3AnimationBase)
  private 
    el: TranslateTransform3D;
    procedure Hand(o: object; e: System.EventArgs);
    begin
      var el0 := Element.transltransform;
      el0.OffsetX += el.OffsetX;
      el0.OffsetY += el.OffsetY;
      el0.OffsetZ += el.OffsetZ;
      Element.transfgroup.Children.Remove(el);
      if Completed <> nil then 
        Completed();
    end;

    procedure InitAnimWait; override;
    begin
      el := new TranslateTransform3D();
      Element.transfgroup.Children.Add(el);
      var ttname := 't' + el.GetHashCode;
      if not RegisterName(sb, el, ttname) then;
      dax := AddDoubleAnimOnByName(sb, x, seconds, ttname, TranslateTransform3D.OffsetXProperty);
      day := AddDoubleAnimOnByName(sb, y, seconds, ttname, TranslateTransform3D.OffsetYProperty);
      daz := AddDoubleAnimOnByName(sb, z, seconds, ttname, TranslateTransform3D.OffsetZProperty);
      daz.Completed += Hand;  
    end;
  public 
    function Clone: AnimationBase; override := new OffsetAnimationOn(Element,Seconds,x,y,z,nil);    
  end;
  
  OffsetAnimation = class(OffsetAnimationOn)
    procedure InitAnimWait; override;
    begin
      el := new TranslateTransform3D();
      Element.transfgroup.Children.Add(el);
      var ttname := 't' + el.GetHashCode;
      if not RegisterName(sb, el, ttname) then;
      dax := AddDoubleAnimOnByName(sb, x-Element.x, seconds, ttname, TranslateTransform3D.OffsetXProperty);
      day := AddDoubleAnimOnByName(sb, y-Element.y, seconds, ttname, TranslateTransform3D.OffsetYProperty);
      daz := AddDoubleAnimOnByName(sb, z-Element.z, seconds, ttname, TranslateTransform3D.OffsetZProperty);
      daz.Completed += Hand;  
    end;
  public
    function Clone: AnimationBase; override := new OffsetAnimation(Element,Seconds,x,y,z,nil);    
  end;
  
  OffsetAnimationUsingKeyframes = class(Double3AnimationBase)
  private 
    el: TranslateTransform3D;
    a: sequence of Point3D;
    procedure Hand(o: object; e: System.EventArgs);
    begin
      var el0 := Element.transltransform;
      el0.OffsetX += el.OffsetX;
      el0.OffsetY += el.OffsetY;
      el0.OffsetZ += el.OffsetZ;
      Element.transfgroup.Children.Remove(el);
      if Completed <> nil then 
        Completed();
    end;
    procedure InitAnimWait; override;
    begin
      el := new TranslateTransform3D();
      Element.transfgroup.Children.Add(el);
      var ttname := 't' + el.GetHashCode;
      if not RegisterName(sb, el, ttname) then;
      var aa := a.ToArray;
      dax := AddDoubleAnimByNameUsingKeyframes(sb, aa.Select(p -> p.x-Element.x), seconds, ttname, TranslateTransform3D.OffsetXProperty);
      day := AddDoubleAnimByNameUsingKeyframes(sb, aa.Select(p -> p.y-Element.y), seconds, ttname, TranslateTransform3D.OffsetYProperty);
      daz := AddDoubleAnimByNameUsingKeyframes(sb, aa.Select(p -> p.z-Element.z), seconds, ttname, TranslateTransform3D.OffsetZProperty);
      daz.Completed += Hand;  
    end;
  
  public 
    constructor(e: Object3D; sec: real; aa: sequence of Point3D; Completed: procedure := nil);
    begin
      inherited Create(e, sec, Completed);
      a := aa;
    end;
    
    function Clone: AnimationBase; override := new OffsetAnimationUsingKeyframes(Element,Seconds,a,nil);    
  end;
  
  ScaleAnimation = class(Double3AnimationBase)
  private 
    scale: real;
    el: ScaleTransform3D;
    procedure Hand(o: object; e: System.EventArgs);
    begin
      if not dax.AutoReverse then
      begin
        var el0 := Element.scaletransform;
        el0.ScaleX += el.ScaleX;
        el0.ScaleY += el.ScaleY;
        el0.ScaleZ += el.ScaleZ;
      end;
      Element.transfgroup.Children.Remove(el);
      if Completed <> nil then 
        Completed();
    end;
    procedure InitAnimWait; override;
    begin
      el := new ScaleTransform3D();
      Element.transfgroup.Children.Add(el);
      var sctransform := Element.scaletransform; 
      var ttname := 's' + sctransform.GetHashCode;
      if not RegisterName(sb, sctransform, ttname) then;
      dax := AddDoubleAnimByName(sb, scale, seconds, ttname, ScaleTransform3D.ScaleXProperty);
      day := AddDoubleAnimByName(sb, scale, seconds, ttname, ScaleTransform3D.ScaleYProperty);
      daz := AddDoubleAnimByName(sb, scale, seconds, ttname, ScaleTransform3D.ScaleZProperty);
      daz.Completed += Hand;
    end;
  public 
    constructor(e: Object3D; sec: real; sc: real; Completed: procedure := nil);
    begin
      inherited Create(e, sec, Completed);
      scale := sc;
    end;
    function Clone: AnimationBase; override := new ScaleAnimation(Element,Seconds,scale,nil);    
  end;
  
  ScaleXAnimation = class(Double1AnimationBase)
  private 
    scale: real;
    el: ScaleTransform3D;
    procedure Hand(o: object; e: System.EventArgs);
    begin
      if not da.AutoReverse then
      begin
        var el0 := Element.scaletransform;
        el0.ScaleX += el.ScaleX;
      end;  
      Element.transfgroup.Children.Remove(el);
      if Completed <> nil then 
        Completed();
    end;
    procedure InitAnimWait; override;
    begin
      el := new ScaleTransform3D();
      Element.transfgroup.Children.Add(el);
      var sctransform := Element.scaletransform; 
      var ttname := 's' + sctransform.GetHashCode;
      if not RegisterName(sb, sctransform, ttname) then;
      da := AddDoubleAnimByName(sb, scale, seconds, ttname, ScaleTransform3D.ScaleXProperty);
      da.Completed += Hand;
    end;
  public 
    constructor(e: Object3D; sec: real; sc: real; Completed: procedure := nil);
    begin
      inherited Create(e, sec, Completed);
      scale := sc;
    end;
    function Clone: AnimationBase; override := new ScaleXAnimation(Element,Seconds,scale,nil);    
  end;
  
  ScaleYAnimation = class(ScaleXAnimation)
  private 
    procedure Hand(o: object; e: System.EventArgs);
    begin
      if not da.AutoReverse then
      begin
        var el0 := Element.scaletransform;
        el0.ScaleY += el.ScaleY;
      end;  
      Element.transfgroup.Children.Remove(el);
      if Completed <> nil then 
        Completed();
    end;
    procedure InitAnimWait; override;
    begin
      el := new ScaleTransform3D();
      Element.transfgroup.Children.Add(el);
      var sctransform := Element.scaletransform; 
      var ttname := 's' + sctransform.GetHashCode;
      if not RegisterName(sb, sctransform, ttname) then;
      da := AddDoubleAnimByName(sb, scale, seconds, ttname, ScaleTransform3D.ScaleYProperty);
      da.Completed += Hand;
    end;
  public 
    function Clone: AnimationBase; override := new ScaleYAnimation(Element,Seconds,scale,nil);    
  end;
  
  ScaleZAnimation = class(ScaleXAnimation)
  private 
    procedure Hand(o: object; e: System.EventArgs);
    begin
      if not da.AutoReverse then
      begin
        var el0 := Element.scaletransform;
        el0.ScaleZ += el.ScaleZ;
      end;  
      Element.transfgroup.Children.Remove(el);
      if Completed <> nil then 
        Completed();
    end;
    procedure InitAnimWait; override;
    begin
      el := new ScaleTransform3D();
      Element.transfgroup.Children.Add(el);
      var sctransform := Element.scaletransform; 
      var ttname := 's' + sctransform.GetHashCode;
      if not RegisterName(sb, sctransform, ttname) then;
      da := AddDoubleAnimByName(sb, scale, seconds, ttname, ScaleTransform3D.ScaleZProperty);
      da.Completed += Hand;
    end;
  public 
    function Clone: AnimationBase; override := new ScaleZAnimation(Element,Seconds,scale,nil);    
  end;
  
  RotateAtAnimation = class(Double1AnimationBase)
  private 
    vx, vy, vz, angle: real;
    center: Point3D;
    el: RotateTransform3D;
    procedure Hand(o: object; e: System.EventArgs);
    begin
      var rot := el.Rotation as AxisAngleRotation3D;
      if not da.AutoReverse then
      begin
        if Absolute then
          Element.RotateAtAbsolute(rot.Axis, angle, center)
        else  
          Element.RotateAt(rot.Axis, angle, center);
      end;  
      Element.transfgroup.Children.Remove(el);
      if Completed <> nil then 
        Completed();
    end;

    procedure InitAnimWait; override;
    begin
      el := new RotateTransform3D();
      el.Rotation := new AxisAngleRotation3D();
      
      if Absolute then
        Element.transfgroup.Children.Add(el) // После основной матрицы, связанной с поворотом
      else Element.transfgroup.Children.Insert(0,el); // До основной матрицы, связанной с поворотом
      var rottransform := el;
      rottransform.CenterX := center.x;
      rottransform.CenterY := center.y;
      rottransform.CenterZ := center.z;
      var rot := rottransform.Rotation as AxisAngleRotation3D;
      var ttname := 'r' + rot.GetHashCode;
      if not RegisterName(sb, rot, ttname) then;
      
      rot.Angle := 0; //?
      rot.Axis := V3D(vx, vy, vz); //?
      
      //var elem: Object3D := Element;
      // Мб da.Completed
      
      da := AddDoubleAnimByName(sb, angle, seconds, ttname, AxisAngleRotation3D.AngleProperty);
      da.Completed += Hand;
    end;
  
  public 
    constructor(e: Object3D; sec: real; vvx, vvy, vvz, a: real; c: Point3D; Completed: procedure := nil);
    begin
      inherited Create(e, sec, Completed);
      (vx, vy, vz, angle, center) := (vvx, vvy, vvz, a, c)
    end;
  public 
    auto property Absolute: boolean; 
    function Clone: AnimationBase; override := new RotateAtAnimation(Element,Seconds,vx,vy,vz,angle,center,nil);    
  end;
  
  RotateAtAbsoluteAnimation = class(RotateAtAnimation)
  public 
    constructor(e: Object3D; sec: real; vvx, vvy, vvz, a: real; c: Point3D; Completed: procedure := nil);
    begin
      inherited Create(e,sec,vvx,vvy,vvz,a,c,Completed);
      Absolute := True;
    end;
    function Clone: AnimationBase; override := new RotateAtAbsoluteAnimation(Element,Seconds,vx,vy,vz,angle,center,nil);    
  end;
  
  CompositeAnimation = class(AnimationBase)
  private 
    ll: List<AnimationBase>;
  public 
    constructor(params l: array of AnimationBase) := ll := Lst(l);
    constructor(l: List<AnimationBase>) := ll := l;
    function Clone: AnimationBase; override;
    begin
      var r := new CompositeAnimation(new List<AnimationBase>());
      for var i := 0 to ll.Count-1 do
        r.ll.Add(ll[i].Clone);
      Result := r;  
    end;  
  end;  
  
  GroupAnimation = class(CompositeAnimation)
  public  
    function Duration: real; override := ll.Select(l -> l.Duration).Max;
    function Add(b: AnimationBase): GroupAnimation;
    begin
      ll += b;
      Result := Self;
    end;
    class function operator +=(a: GroupAnimation; b: AnimationBase): GroupAnimation;
    begin
      a.ll += b;
      Result := a;
    end;
    procedure BeginT;
    begin
      for var i:=0 to ll.Count-1 do
        ll[i].Begin;
    end;
    procedure &Begin; override := Invoke(BeginT);
  end;
  
  SequenceAnimation = class(CompositeAnimation)
  public 
    function Duration: real; override := ll.Select(l -> l.Duration).Sum;
    function Add(b: AnimationBase): SequenceAnimation;
    begin
      ll += b;
      Result := Self;
    end;
    class function operator +=(a: SequenceAnimation; b: AnimationBase): SequenceAnimation;
    begin
      a.ll += b;
      Result := a;
    end;
    procedure BeginT;
    begin 
      //Println(Self.GetType+' '+ll.Count); 
      for var ii:=0 to ll.Count-2 do
      begin
        var i := ii; // параметр цикла неправильно захватывается лямбдой  
        var lll := ll; // поле предка - вообще не захватывается

        // Если ll[i] - CompositeAnimation, то надо повесить Completed на самую правую не CompositeAnimation
        var lf := ll[i];
        var llf := ll;
        var lli := i;
        while lf is CompositeAnimation do
        begin
          var ca := lf as CompositeAnimation;
          lf := ca.ll[ca.ll.Count-1];
          llf := ca.ll;
          lli := ca.ll.Count-1;
        end;
        
        // lf надо в любом случае клонировать
        // Клонируем. Надо еще списки клонировать! А то в a+a во втором списке последний элемент - клонированный - в обоих списках!
        //lf := lf.Clone;
        llf[lli] := lf;
        lf.Completed := procedure -> 
        begin
          lll[i+1].Begin;
        end;  
      end;
      ll[0].Begin;
    end;
    procedure &Begin; override := Invoke(BeginT);
  end;

  /// Класс, содержащий комбинации анимаций: группа и последовательность
  Animate = class
  public 
    /// Возвращает группу анимаций, выполняющихся параллельно 
    class function Group(params l: array of AnimationBase) := new GroupAnimation(Lst(l));
    /// Возвращает последовательность анимаций, выполняющихся последовательно 
    class function &Sequence(params l: array of AnimationBase) := new SequenceAnimation(l);
  end;
  
type
// -----------------------------------------------------
//>>     Graph3D: класс SphereT # Graph3D SphereT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс сферы
  SphereT = class(ObjectWithMaterial3D,ISerializable)
  private 
    function Model := inherited model as SphereVisual3D;
    procedure SetRP(r: real) := Model.Radius := r;
    procedure SetR(r: real) := Invoke(SetRP, r);
    function GetR: real := InvokeReal(()->Model.Radius);
    function NewVisualObject(r: real): SphereVisual3D;
    begin
      var sph := new SphereVisual3D;
      sph.Center := P3D(0,0,0);
      sph.Radius := r;
      Result := sph;
    end;
  
  protected 
    function CreateObject: Object3D; override := new SphereT(X, Y, Z, Radius, Material.Clone);
    constructor := CreateBase(NewVisualObject(1), 0, 0, 0, Materialhelper.CreateMaterial(Colors.Blue));
  public 
    constructor(x, y, z, r: real; m: Gmaterial) := CreateBase(NewVisualObject(r), x, y, z, m);
/// Радиус сферы
    property Radius: real read GetR write SetR;
/// Возвращает клон сферы
    function Clone := (inherited Clone) as SphereT;

///--
    function CreateModel: Visual3D; override := new SphereVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('radius', Radius, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Radius := info.GetDouble('radius');
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс EllipsoidT # Graph3D EllipsoidT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс эллипсоида
  EllipsoidT = class(ObjectWithMaterial3D,ISerializable)
  private
    function Model := inherited model as EllipsoidVisual3D;
    procedure SetRX(r: real) := Invoke(procedure(r: real)->Model.RadiusX := r, r);
    function GetRX: real := InvokeReal(()->Model.RadiusX);
    procedure SetRYP(r: real) := Model.RadiusY := r;
    procedure SetRY(r: real) := Invoke(SetRYP, r);
    function GetRY: real := InvokeReal(()->Model.RadiusY);
    procedure SetRZP(r: real) := Model.RadiusZ := r;
    procedure SetRZ(r: real) := Invoke(SetRZP, r);
    function GetRZ: real := InvokeReal(()->Model.RadiusZ);
    function NewVisualObject(rx, ry, rz: real): EllipsoidVisual3D;
    begin
      var ell := new EllipsoidVisual3D;
      ell.Center := P3D(0,0,0);
      ell.RadiusX := rx;
      ell.RadiusY := ry;
      ell.RadiusZ := rz;
      Result := ell;
    end;
  
  protected
    function CreateObject: Object3D; override := new EllipsoidT(X, Y, Z, RadiusX, RadiusY, RadiusZ, Material.Clone);
    constructor(x, y, z, rx, ry, rz: real; m: GMaterial) := CreateBase(NewVisualObject(rx, ry, rz), x, y, z, m);
  public 
/// Радиус эллипсоида по оси X
    property RadiusX: real read GetRX write SetRX;
/// Радиус эллипсоида по оси Y
    property RadiusY: real read GetRY write SetRY;
/// Радиус эллипсоида по оси Z
    property RadiusZ: real read GetRZ write SetRZ;
/// Возвращает клон эллипсоида
    function Clone := (inherited Clone) as EllipsoidT;
///--
    function CreateModel: Visual3D; override := new EllipsoidVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('RadiusX', RadiusX, typeof(real));
      info.AddValue('RadiusY', RadiusY, typeof(real));
      info.AddValue('RadiusZ', RadiusZ, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      RadiusX := info.GetDouble('RadiusX');
      RadiusY := info.GetDouble('RadiusY');
      RadiusZ := info.GetDouble('RadiusZ');
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс CubeT # Graph3D CubeT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс куба
  CubeT = class(ObjectWithMaterial3D,ISerializable)
  private
    function model := inherited model as CubeVisual3D;
    procedure SetWP(r: real) := model.SideLength := r;
    procedure SetW(r: real) := Invoke(SetWP, r);
    function GetW: real := InvokeReal(()->model.SideLength);
  private 
    function NewVisualObject(w: real): CubeVisual3D;
    begin
      var bx := new CubeVisual3D;
      bx.Center := P3D(0,0,0);
      bx.SideLength := w;
      Result := bx;
    end;
  
  protected  
    function CreateObject: Object3D; override := new CubeT(X, Y, Z, SideLength, Material.Clone);
    constructor(x, y, z, w: real; m: GMaterial) := CreateBase(NewVisualObject(w), x, y, z, m);
   
  public 
/// Сторона куба
    property SideLength: real read GetW write SetW;
/// Возвращает клон куба
    function Clone := (inherited Clone) as CubeT;
///--
    function CreateModel: Visual3D; override := new CubeVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('SideLength', SideLength, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      SideLength := info.GetDouble('SideLength');
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс BoxT # Graph3D BoxT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс паралеллепипеда
  BoxT = class(ObjectWithMaterial3D,ISerializable)
  private
    function model := inherited model as BoxVisual3D;
    procedure SetWP(r: real) := model.Width := r;
    procedure SetW(r: real) := Invoke(SetWP, r);
    function GetW: real := InvokeReal(()->model.Width);
    
    procedure SetHP(r: real) := model.Height := r;
    procedure SetH(r: real) := Invoke(SetHP, r); 
    function GetH: real := InvokeReal(()->model.Height);
    
    procedure SetLP(r: real) := model.Length := r;
    procedure SetL(r: real) := Invoke(SetLP, r);
    function GetL: real := InvokeReal(()->model.Length);
    
    procedure SetSzP(r: Size3D) := (model.Length, model.Width, model.Height) := (r.X, r.Y, r.Z);
    procedure SetSz(r: Size3D) := Invoke(SetSzP, r);
    function GetSz: Size3D := Inv(()->Sz3D(model.Length, model.Width, model.Height));
  private 
    function NewVisualObject(l, w, h: real): BoxVisual3D;
    begin
      var bx := new BoxVisual3D;
      bx.Center := P3D(0,0,0);
      (bx.Width, bx.Height, bx.Length) := (w, h, l);
      Result := bx;
    end;
  
  protected  
    function CreateObject: Object3D; override := new BoxT(X, Y, Z, Length, Width, Height, Material.Clone);
    constructor(x, y, z, l, w, h: real; m: GMaterial) := CreateBase(NewVisualObject(l, w, h), x, y, z, m);
    
  public 
/// Возвращает длину паралеллепипеда (по оси X)
    property Length: real read GetL write SetL;
/// Возвращает ширину паралеллепипеда (по оси Y)
    property Width: real read GetW write SetW;
/// Возвращает высоту паралеллепипеда (по оси Z)
    property Height: real read GetH write SetH;
/// Возвращает размеры паралеллепипеда
    property Size: Size3D read GetSz write SetSz;
/// Возвращает клон паралеллепипеда
    function Clone := (inherited Clone) as BoxT;
///--
    function CreateModel: Visual3D; override := new BoxVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('Length', Length, typeof(real));
      info.AddValue('Width', Width, typeof(real));
      info.AddValue('Height', Height, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Length := info.GetDouble('Length');
      Width := info.GetDouble('Width');
      Height := info.GetDouble('Height');
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс ArrowT # Graph3D ArrowT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс 3D-стрелки
  ArrowT = class(ObjectWithMaterial3D,ISerializable)
  private
    function model := inherited model as ArrowVisual3D;
    
    procedure SetDP(r: real) := model.Diameter := r;
    procedure SetD(r: real) := Invoke(SetDP, r);
    function GetD: real := InvokeReal(()->model.Diameter);
    
    procedure SetLP(r: real) := model.HeadLength := r;
    procedure SetL(r: real) := Invoke(SetLP, r);
    function GetL: real := InvokeReal(()->model.HeadLength);
    
    procedure SetDirP(r: Vector3D) := model.Direction := r;
    procedure SetDir(r: Vector3D) := Invoke(SetDirP, r);
    function GetDir: Vector3D := Invoke&<Vector3D>(()->model.Direction);
  private 
    function NewVisualObject(dx, dy, dz, d, hl: real): ArrowVisual3D;
    begin
      var a := new ArrowVisual3D;
      a.HeadLength := hl;
      a.Diameter := d;
      a.Origin := P3D(0,0,0);
      Result := a;
    end;
  
  protected  
    function CreateObject: Object3D; override := new ArrowT(X, Y, Z, Direction.X, Direction.Y, Direction.Z, Diameter, HeadLength, Material.Clone);
    constructor(x, y, z, dx, dy, dz, d, hl: real; m: GMaterial);
    begin
      var a := NewVisualObject(dx, dy, dz, d, hl);
      CreateBase(a, x, y, z, m);
      a.Direction := V3D(dx, dy, dz);
    end;
    
  public 
/// Длина наконечника 3D-стрелки
    property HeadLength: real read GetL write SetL;
/// Диаметр 3D-стрелки
    property Diameter: real read GetD write SetD;
/// направление 3D-стрелки
    property Direction: Vector3D read GetDir write SetDir;
/// Возвращает клон 3D-стрелки
    function Clone := (inherited Clone) as ArrowT;
///--
    function CreateModel: Visual3D; override := new ArrowVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('HeadLength', HeadLength, typeof(real));
      info.AddValue('Diameter', Diameter, typeof(real));
      info.AddValue('DirectionX', Direction.X, typeof(real));
      info.AddValue('DirectionY', Direction.Y, typeof(real));
      info.AddValue('DirectionZ', Direction.Z, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      HeadLength := info.GetDouble('HeadLength');
      Diameter := info.GetDouble('Diameter');
      var dx := info.GetDouble('DirectionX');
      var dy := info.GetDouble('DirectionY');
      var dz := info.GetDouble('DirectionZ');
      Direction := V3D(dx,dy,dz);
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс TruncatedConeT # Graph3D TruncatedConeT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс усеченного конуса
  TruncatedConeT = class(ObjectWithMaterial3D,ISerializable)
  private
    function model := inherited model as TruncatedConeVisual3D;
    
    procedure SetH(r: real) := Invoke(procedure(r: real)->model.Height := r, r); 
    function GetH: real := InvokeReal(()->model.Height);
    
    procedure SetBRP(r: real) := model.BaseRadius := r;
    procedure SetBR(r: real) := Invoke(SetBRP, r); 
    function GetBR: real := InvokeReal(()->model.BaseRadius);
    
    procedure SetTRP(r: real) := model.TopRadius := r;
    procedure SetTR(r: real) := Invoke(SetTRP, r); 
    function GetTR: real := InvokeReal(()->model.TopRadius);
    
    procedure SetTCP(r: boolean) := model.TopCap := r;
    procedure SetTC(r: boolean) := Invoke(SetTCP, r); 
    function GetTC: boolean := Invoke&<boolean>(()->model.TopCap);
  private 
    function NewVisualObject(h, baser, topr: real; sides: integer; topcap: boolean): TruncatedConeVisual3D;
    begin
      var a := new TruncatedConeVisual3D;
      a.Origin := P3D(0,0,0);
      a.BaseRadius := baser;
      a.TopRadius := topr;
      a.Height := h;
      a.TopCap := topcap;
      a.ThetaDiv := sides + 1;
      a.BaseCap := True;
      Result := a;
    end;
  
  protected  
    function CreateObject: Object3D; override := new TruncatedConeT(X, Y, Z, Height, BaseRadius, TopRadius, (model as TruncatedConeVisual3D).ThetaDiv - 1, Topcap, Material.Clone);
    constructor(x, y, z, h, baser, topr: real; sides: integer; topcap: boolean; m: GMaterial);
    begin
      var a := NewVisualObject(h, baser, topr, sides, topcap);
      CreateBase(a, x, y, z, m);
    end;
    
  public 
/// Высота усеченного конуса
    property Height: real read GetH write SetH;
/// Радиус основания усеченного конуса
    property BaseRadius: real read GetBR write SetBR;
/// Верхний радиус усеченного конуса
    property TopRadius: real read GetTR write SetTR;
/// Есть ли шапочка у усеченного конуса
    property Topcap: boolean read GetTC write SetTC;
/// Возвращает клон усеченного конуса
    function Clone := (inherited Clone) as TruncatedConeT;
///--
    function CreateModel: Visual3D; override := new TruncatedConeVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('Height', Height, typeof(real));
      info.AddValue('BaseRadius', BaseRadius, typeof(real));
      info.AddValue('TopRadius', TopRadius, typeof(real));
      info.AddValue('Topcap', Topcap, typeof(boolean));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Height := info.GetDouble('Height');
      BaseRadius := info.GetDouble('BaseRadius');
      TopRadius := info.GetDouble('TopRadius');
      Topcap := info.GetBoolean('Topcap');
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс CylinderT # Graph3D CylinderT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс цилиндра
  CylinderT = class(TruncatedConeT,ISerializable)
  private 
    procedure SetR(r: real);
    begin
      BaseRadius := r;
      TopRadius := r;
    end;
    
    function GetR: real := BaseRadius;
  protected  
    function CreateObject: Object3D; override := new CylinderT(X, Y, Z, Height, Radius, (model as TruncatedConeVisual3D).ThetaDiv - 1, Topcap, Material.Clone);
    constructor(x, y, z, h, r: real; ThetaDiv: integer; topcap: boolean; m: GMaterial);
    begin
      var a := NewVisualObject(h, r, r, ThetaDiv, topcap);
      CreateBase(a, x, y, z, m);
    end;
    
  public 
/// Радиус цилиндра
    property Radius: real read GetR write SetR;
/// Возвращает клон цилиндра
    function Clone := (inherited Clone) as CylinderT;
///--
    function CreateModel: Visual3D; override := new TruncatedConeVisual3D;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс TeapotT # Graph3D TeapotT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс чайника
  TeapotT = class(ObjectWithMaterial3D,ISerializable)
  private
    procedure SetVP(v: boolean) := (model as MeshElement3D).Visible := v;
    procedure SetV(v: boolean) := Invoke(SetVP, v);
    function GetV: boolean := Invoke&<boolean>(()->(model as MeshElement3D).Visible);
  protected  
    function CreateObject: Object3D; override := new TeapotT(X, Y, Z, Material.Clone);
    constructor(x, y, z: real; m: GMaterial);
    begin
      var a := new Teapot;
      CreateBase(a, x, y, z, m);
      Rotate(V3D(1,0,0), 90);
    end;
  public 
/// Видим ли чайник
    property Visible: boolean read GetV write SetV;
/// Возвращает клон чайника
    function Clone := (inherited Clone) as TeapotT;
///--
    function CreateModel: Visual3D; override := new Teapot;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс CoordinateSystemT # Graph3D CoordinateSystemT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс системы координат
  CoordinateSystemT = class(ObjectWithChildren3D,ISerializable)
  private
    procedure SetALP(r: real) := (model as CoordinateSystemVisual3D).ArrowLengths := r;
    procedure SetAL(r: real) := Invoke(SetALP, r); 
    function GetAL: real := InvokeReal(()->(model as CoordinateSystemVisual3D).ArrowLengths);
    function GetD: real := InvokeReal(()->((model as CoordinateSystemVisual3D).Children[0] as ArrowVisual3D).Diameter);
  protected  
    function CreateObject: Object3D; override := new CoordinateSystemT(X, Y, Z, ArrowLengths, Diameter);
    constructor(x, y, z, arrlength, diameter: real);
    begin
      var a := new CoordinateSystemVisual3D;
      CreateBase0(a, x, y, z);
      a.ArrowLengths := arrlength;
      (a.Children[0] as ArrowVisual3D).Diameter := diameter;
      (a.Children[1] as ArrowVisual3D).Diameter := diameter;
      (a.Children[2] as ArrowVisual3D).Diameter := diameter;
      (a.Children[3] as CubeVisual3D).SideLength := diameter;
    end;
    
  public 
/// Длина стрелок системы координат
    property ArrowLengths: real read GetAL write SetAL;
/// Диаметр стрелок системы координат
    property Diameter: real read GetD;
/// Возвращает клон системы координат
    function Clone := (inherited Clone) as CoordinateSystemT;
///--
    function CreateModel: Visual3D; override := new CoordinateSystemVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('ArrowLengths', ArrowLengths, typeof(real));
      info.AddValue('Diameter', Diameter, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      ArrowLengths := info.GetDouble('ArrowLengths');
      var d := info.GetDouble('Diameter');
      var a := model as CoordinateSystemVisual3D;
      (a.Children[0] as ArrowVisual3D).Diameter := d;
      (a.Children[1] as ArrowVisual3D).Diameter := d;
      (a.Children[2] as ArrowVisual3D).Diameter := d;
      (a.Children[3] as CubeVisual3D).SideLength := d;
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс BillboardTextT # Graph3D BillboardTextT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс текста на билборде (всегда направлен к камере)
  BillboardTextT = class(ObjectWithChildren3D,ISerializable)
  private
    function model := inherited model as BillboardTextVisual3D;
    
    procedure SetTP(r: string) := model.Text := r;
    procedure SetT(r: string) := Invoke(SetTP, r); 
    function GetT: string := InvokeString(()->model.Text);
    
    procedure SetFSP(r: real) := model.FontSize := r;
    procedure SetFS(r: real) := Invoke(SetFSP, r); 
    function GetFS: real := InvokeReal(()->model.FontSize);
  protected  
    function CreateObject: Object3D; override := new BillboardTextT(X, Y, Z, Text, FontSize);
    constructor(x, y, z: real; text: string; fontsize: real);
    begin
      var a := new BillboardTextVisual3D;
      CreateBase0(a, x, y, z);
      a.Position := p3D(0, 0, 0);
      a.Text := text;
      a.FontSize := fontsize;
    end;
    
  public 
/// Текст на билборде
    property Text: string read GetT write SetT;
/// Размер шрифта на билборде
    property FontSize: real read GetFS write SetFS;
/// Возвращает клон билборда
    function Clone := (inherited Clone) as BillboardTextT;
///--
    function CreateModel: Visual3D; override := new BillboardTextVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('Text', Text, typeof(string));
      info.AddValue('FontSize', FontSize, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Text := info.GetString('Text');
      FontSize := info.GetDouble('FontSize');
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс TextT # Graph3D TextT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс 3D-текстового объекта 
  TextT = class(ObjectWithChildren3D,ISerializable)
  private 
    _fontname: string;
    function model := inherited model as TextVisual3D;
    
    procedure SetTP(r: string) := model.Text := r;
    procedure SetT(r: string) := Invoke(SetTP, r); 
    function GetT: string := InvokeString(()->model.Text);
    
    procedure SetFSP(r: real) := model.Height := r;
    procedure SetFS(r: real) := Invoke(SetFS, r); 
    function GetFS: real := InvokeReal(()->model.Height);
    
    procedure SetUP(v: Vector3D) := model.UpDirection := v;
    procedure SetU(v: Vector3D) := Invoke(SetUP, v); 
    function GetU: Vector3D := Invoke&<Vector3D>(()->model.UpDirection);
    
    procedure SetNP(fontname: string) := model.FontFamily := new FontFamily(fontname);
    procedure SetN(fontname: string) := Invoke(SetNP, fontname); 
    function GetN: string := InvokeString(()->_fontname);
    
    procedure SetColorP(c: GColor) := model.Foreground := new SolidColorBrush(c);
    procedure SetColor(c: GColor) := Invoke(SetColorP, c); 
    function GetColor: GColor := Invoke&<GColor>(()->(model.Foreground as SolidColorBrush).Color);
  protected  
    function CreateObject: Object3D; override := new TextT(X, Y, Z, Text, Height, FontName, Color);
    constructor(x, y, z: real; text: string; height: real; fontname: string; c: GColor);
    begin
      var a := new TextVisual3D;
      a.Position := p3D(0, 0, 0);
      a.Text := text;
      a.Height := height;
      //a.HorizontalAlignment := HorizontalAlignment.Left;
      Self._fontname := fontname;
      a.FontFamily := new FontFamily(fontname);
      a.Foreground := new SolidColorBrush(c);
      CreateBase0(a, x, y, z);
    end;
    
  public 
/// Текст на 3D-текстовом объекте 
    property Text: string read GetT write SetT;
/// Высота 3D-текстового объекта 
    property Height: real read GetFS write SetFS;
/// Имя шрифта 3D-текстового объекта 
    property FontName: string read GetN write SetN;
/// Направление "вверх" для 3D-текстового объекта 
    property UpDirection: Vector3D read GetU write SetU;
/// Цвет 3D-текстового объекта 
    property Color: GColor read GetColor write SetColor; override;
/// Возвращает клон 3D-текстового объекта
    function Clone := (inherited Clone) as TextT;
///--
    function CreateModel: Visual3D; override := new TextVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('Text', Text, typeof(string));
      info.AddValue('Height', Height, typeof(real));
      info.AddValue('FontName', FontName, typeof(string));
      info.AddValue('UpDirectionX', UpDirection.X, typeof(real));
      info.AddValue('UpDirectionY', UpDirection.Y, typeof(real));
      info.AddValue('UpDirectionZ', UpDirection.Z, typeof(real));
      info.AddValue('Color', ColorToLongWord(Color), typeof(longword));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Text := info.GetString('Text');
      model.Height := info.GetDouble('Height');
      FontName := info.GetString('FontName');
      var dx := info.GetDouble('UpDirectionX');
      var dy := info.GetDouble('UpDirectionY');
      var dz := info.GetDouble('UpDirectionZ');
      UpDirection := V3D(dx,dy,dz);
      Color := LongwordToColor(info.GetUInt32('Color'));
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс RectangleT # Graph3D RectangleT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс 3D-прямоугольника
  RectangleT = class(ObjectWithMaterial3D,ISerializable)
  private
    function model := inherited model as RectangleVisual3D;
    procedure SetWP(r: real) := model.Width := r;
    procedure SetW(r: real) := Invoke(SetWP, r);
    function GetW: real := InvokeReal(()->model.Width);
    
    procedure SetLP(r: real) := model.Length := r;
    procedure SetL(r: real) := Invoke(SetLP, r);
    function GetL: real := InvokeReal(()->model.Length);
    
    procedure SetLDP(r: Vector3D) := model.LengthDirection := r;
    procedure SetLD(r: Vector3D) := Invoke(SetLDP, r);
    function GetLD: Vector3D := Invoke&<Vector3D>(()->model.LengthDirection);
    
    procedure SetNP(r: Vector3D) := model.Normal := r;
    procedure SetN(r: Vector3D) := Invoke(SetNP, r);
    function GetN: Vector3D := Invoke&<Vector3D>(()->model.Normal);
  protected  
    function CreateObject: Object3D; override := new RectangleT(X, Y, Z, Length, Width, Normal, LengthDirection, Material);
    constructor(x, y, z, Length, Width: real; Normal, LengthDirection: Vector3D; m: GMaterial);
    begin
      var a := new RectangleVisual3D;
      a.Origin := P3D(0, 0, 0);
      a.Width := Width;
      a.Length := Length;
      a.LengthDirection := lengthdirection;
      a.Normal := normal;
      CreateBase(a, x, y, z, m);
    end;
    
  public 
/// Ширина 3D-прямоугольника 
    property Width: real read GetW write SetW;
/// Длина 3D-прямоугольника 
    property Length: real read GetL write SetL;
/// Направление длины 3D-прямоугольника 
    property LengthDirection: Vector3D read GetLD write SetLD;
/// Нормаль к 3D-прямоугольнику
    property Normal: Vector3D read GetN write SetN;
/// Возвращает клон 3D-прямоугольника
    function Clone := (inherited Clone) as RectangleT;
///--
    function CreateModel: Visual3D; override := new RectangleVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('Width', Width, typeof(real));
      info.AddValue('Length', Length, typeof(real));
      info.AddValue('LengthDirectionX', LengthDirection.X, typeof(real));
      info.AddValue('LengthDirectionY', LengthDirection.Y, typeof(real));
      info.AddValue('LengthDirectionZ', LengthDirection.Z, typeof(real));
      info.AddValue('NormalX', Normal.X, typeof(real));
      info.AddValue('NormalY', Normal.Y, typeof(real));
      info.AddValue('NormalZ', Normal.Z, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Width := info.GetDouble('Width');
      Length := info.GetDouble('Length');
      var dx := info.GetDouble('LengthDirectionX');
      var dy := info.GetDouble('LengthDirectionY');
      var dz := info.GetDouble('LengthDirectionZ');
      LengthDirection := V3D(dx,dy,dz);
      dx := info.GetDouble('NormalX');
      dy := info.GetDouble('NormalY');
      dz := info.GetDouble('NormalZ');
      Normal := V3D(dx,dy,dz);
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс FileModelT # Graph3D FileModelT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс 3D-модели
  FileModelT = class(ObjectWithChildren3D,ISerializable)
  private 
    fn: string;
    procedure SetMP(mat: GMaterial) := (model as FileModelVisual3D).DefaultMaterial := mat;
    procedure SetMaterial(mat: GMaterial) := Invoke(SetMP, mat);
    function GetMaterial: GMaterial := Invoke&<GMaterial>(()->(model as FileModelVisual3D).DefaultMaterial);
  public 
    //property Color: GColor write SetColor;
    property Material: GMaterial read GetMaterial write SetMaterial;// не работает почему-то на запись
  
    {procedure SetVP(v: boolean) := (model as FileModelVisual3D).Visibility := v;
    procedure SetV(v: boolean) := Invoke(SetVP, v);
    function GetV: boolean := Invoke&<boolean>(()->(model as FileModelVisual3D).Visibility);}
  protected  
    function CreateObject: Object3D; override := new FileModelT(X, Y, Z, fn, Material.Clone);
    constructor(x, y, z: real; fname: string; mat: GMaterial);
    begin
      {'.3ds': r := new studioreader(nil);
      '.lwo': r := new lworeader(nil);
      '.stl': r := new stlreader(nil);
      '.obj','.objx': r := new objreader(nil);}
      
      var a := new FileModelVisual3D;
      a.DefaultMaterial := mat;
      a.Source := fname;
      CreateBase0(a, x, y, z);
    end;
    
  public 
/// Возвращает клон 3D-модели
    function Clone := (inherited Clone) as FileModelT;
///--
    function CreateModel: Visual3D; override := new FileModelVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('FileName', (model as FileModelVisual3D).Source as string, typeof(string));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      (model as FileModelVisual3D).Source := info.GetString('FileName');
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс PipeT # Graph3D PipeT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс трубы
  PipeT = class(ObjectWithMaterial3D,ISerializable)
  private
    function model := inherited model as PipeVisual3D;
    procedure SetDP(r: real) := model.Diameter := r * 2;
    procedure SetD(r: real) := Invoke(SetDP, r); 
    function GetD: real := InvokeReal(()->model.Diameter / 2);
    
    procedure SetIDP(r: real) := model.InnerDiameter := r * 2;
    procedure SetID(r: real) := Invoke(SetIDP, r); 
    function GetID: real := InvokeReal(()->model.InnerDiameter / 2);
    
    procedure SetHP(r: real) := model.Point2 := P3D(0, 0, r);
    procedure SetH(r: real) := Invoke(SetHP, r); 
    function GetH: real := InvokeReal(()->model.Point2.Z);
  protected  
    function CreateObject: Object3D; override := new PipeT(X, Y, Z, Height, Radius, InnerRadius, Material);
    constructor(x, y, z, h, r, ir: real; m: GMaterial);
    begin
      var a := new PipeVisual3D;
      a.Diameter := r * 2;
      a.InnerDiameter := ir * 2;
      a.Point1 := P3D(0, 0, 0);
      a.Point2 := P3D(0, 0, h);
      CreateBase(a, x, y, z, m);
    end;
    
  public 
/// Радиус трубы
    property Radius: real read GetD write SetD;
/// Внутренний радиус трубы
    property InnerRadius: real read GetID write SetID;
/// Высота трубы
    property Height: real read GetH write SetH;
/// Возвращает клон трубы
    function Clone := (inherited Clone) as PipeT;
///--
    function CreateModel: Visual3D; override := new PipeVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('Radius', Radius, typeof(real));
      info.AddValue('InnerRadius', InnerRadius, typeof(real));
      info.AddValue('Height', Height, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Radius := info.GetDouble('Radius');
      InnerRadius := info.GetDouble('InnerRadius');
      Height := info.GetDouble('Height');
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс LegoT # Graph3D LegoT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс лего-детали
  LegoT = class(ObjectWithMaterial3D,ISerializable)
  private
    //function model := inherited model as LegoVisual3D;
    procedure SetWP(r: integer);
    procedure SetW(r: integer);
    function GetW: integer;
    
    procedure SetHP(r: integer);
    procedure SetH(r: integer); 
    function GetH: integer;
    
    procedure SetLP(r: integer);
    procedure SetL(r: integer);
    function GetL: integer;
  
    {procedure SetSzP(r: Size3D);
    begin
      var mmm := model as LegoVisual3D;
      (mmm.Columns,mmm.Rows,mmm.Height) := (r.X,r.Y,r.Z);
    end;
    procedure SetSz(r: Size3D) := Invoke(SetSzP,r);
    function GetSz: Size3D := Invoke&<Size3D>(()->begin var mmm := model as LegoVisual3D; Result := Sz3D(mmm.Length,mmm.Width,mmm.Height) end);}
  protected  
    function CreateObject: Object3D; override := new LegoT(X, Y, Z, Columns, Rows, Height, Material);
  public 
    constructor(x, y, z: real; Rows, Columns, Height: integer; m: GMaterial);
    
/// Количество кирпичиков лего-детали по оси X
    property Columns: integer read GetL write SetL;
/// Количество кирпичиков лего-детали по оси Y
    property Rows: integer read GetW write SetW;
/// Высота лего-детали, измеряемая в количестве деталей одинарной высоты 
    property Height: integer read GetH write SetH;
    {property Size: Size3D read GetSz write SetSz;}
/// Возвращает клон лего-детали
    function Clone := (inherited Clone) as LegoT;
///--
    function CreateModel: Visual3D; override;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('Columns', Columns, typeof(integer));
      info.AddValue('Rows', Rows, typeof(integer));
      info.AddValue('Height', Height, typeof(integer));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Columns := info.GetInt32('Columns');
      Rows := info.GetInt32('Rows');
      Height := info.GetInt32('Height');
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс PlatonicAbstractT # Graph3D PlatonicAbstractT class
// ----------------------------------------------------- 
  [Serializable]
/// Абстрактный класс платоновых тел
  PlatonicAbstractT = class(ObjectWithMaterial3D,ISerializable)
  private
    function GetLength: real;
    procedure SetLengthP(r: real);
  public 
/// Длина грани
    property Length: real read GetLength write Invoke(SetLengthP, value);
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('Length', Length, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Length := info.GetDouble('Length');
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс IcosahedronT # Graph3D IcosahedronT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс икосаэдра
  IcosahedronT = class(PlatonicAbstractT,ISerializable)
  protected  
    function CreateObject: Object3D; override := new IcosahedronT(X, Y, Z, Length, Material);
  public 
    constructor(x, y, z, Length: real; m: GMaterial);
/// Возвращает клон икосаэдра
    function Clone := (inherited Clone) as IcosahedronT;
///--
    function CreateModel: Visual3D; override;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс DodecahedronT # Graph3D DodecahedronT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс додекаэдра
  DodecahedronT = class(PlatonicAbstractT,ISerializable)
  protected  
    function CreateObject: Object3D; override := new DodecahedronT(X, Y, Z, Length, Material);
  public 
    constructor(x, y, z, Length: real; m: GMaterial);
/// Возвращает клон додекаэдра
    function Clone := (inherited Clone) as DodecahedronT;
///--
    function CreateModel: Visual3D; override;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс TetrahedronT # Graph3D TetrahedronT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс тетраэдра
  TetrahedronT = class(PlatonicAbstractT,ISerializable)
  protected  
    function CreateObject: Object3D; override := new TetrahedronT(X, Y, Z, Length, Material);
  public 
    constructor(x, y, z, Length: real; m: GMaterial);
/// Возвращает клон тетраэдра
    function Clone := (inherited Clone) as TetrahedronT;
///--
    function CreateModel: Visual3D; override;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс OctahedronT # Graph3D OctahedronT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс октаэдра
  OctahedronT = class(PlatonicAbstractT,ISerializable)
  protected  
    function CreateObject: Object3D; override := new OctahedronT(X, Y, Z, Length, Material);
  public 
    constructor(x, y, z, Length: real; m: GMaterial);
/// Возвращает клон октаэдра
    function Clone := (inherited Clone) as OctahedronT;
///--
    function CreateModel: Visual3D; override;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
    end;
  end;

// -----------------------------------------------------
//>>     Graph3D: класс TriangleT # Graph3D TriangleT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс 3D-треугольника
  TriangleT = class(ObjectWithMaterial3D,ISerializable)
  protected
    procedure SetP1(p: Point3D);
    function  GetP1: Point3D;
    procedure SetP2(p: Point3D);
    function  GetP2: Point3D;
    procedure SetP3(p: Point3D);
    function  GetP3: Point3D;
    
    function CreateObject: Object3D; override;
  public 
    constructor(p1, p2, p3: Point3D; m: Gmaterial);
    
/// Первая точка 3D-треугольника
    property P1: Point3D read GetP1 write SetP1;
/// Вторая точка 3D-треугольника
    property P2: Point3D read GetP2 write SetP2;
/// Третья точка 3D-треугольника
    property P3: Point3D read GetP3 write SetP3;
/// Устанавливает точки 3D-треугольника
    procedure SetPoints(p1, p2, p3: Point3D);
///--
    function CreateModel: Visual3D; override;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('p1x', P1.X, typeof(real));
      info.AddValue('p1y', P1.Y, typeof(real));
      info.AddValue('p1z', P1.Z, typeof(real));
      info.AddValue('p2x', P2.X, typeof(real));
      info.AddValue('p2y', P2.Y, typeof(real));
      info.AddValue('p2z', P2.Z, typeof(real));
      info.AddValue('p3x', P3.X, typeof(real));
      info.AddValue('p3y', P3.Y, typeof(real));
      info.AddValue('p3z', P3.Z, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      SetPoints(P3D(info.GetDouble('p1x'),info.GetDouble('p1y'),info.GetDouble('p1z')),
                P3D(info.GetDouble('p2x'),info.GetDouble('p2y'),info.GetDouble('p2z')),
                P3D(info.GetDouble('p3x'),info.GetDouble('p3y'),info.GetDouble('p3z')));
    end;
  end;

// -----------------------------------------------------
//>>     Graph3D: класс PrismT # Graph3D PrismT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс правильной призмы
  PrismT = class(ObjectWithMaterial3D,ISerializable)
  private
    procedure SetR(r: real);
    function  GetR: real;
    procedure SetH(r: real);
    function  GetH: real;
    procedure SetN(n: integer);
    function  GetN: integer;
  protected
    function CreateObject: Object3D; override := new PrismT(X, Y, Z, Sides, Radius, Height, Material.Clone);
  public 
    constructor(x, y, z: real; N: integer; r, h: real; m: Gmaterial);
/// Радиус правильной призмы
    property Radius: real read GetR write SetR;
/// Высота правильной призмы
    property Height: real read GetH write SetH;
/// Количество боковых граней правильной призмы
    property Sides: integer read GetN write SetN;
/// Возвращает клон правильной призмы
    function Clone := (inherited Clone) as PrismT;
///--
    function CreateModel: Visual3D; override;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('Radius', Radius, typeof(real));
      info.AddValue('Height', Height, typeof(real));
      info.AddValue('Sides', Sides, typeof(integer));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Radius := info.GetDouble('Radius');
      Height := info.GetDouble('Height');
      Sides := info.GetInt32('Sides');
    end;
  end;

// -----------------------------------------------------
//>>     Graph3D: класс PyramidT # Graph3D PyramidT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс правильной пирамиды
  PyramidT = class(PrismT,ISerializable)
  private
  protected
    function CreateObject: Object3D; override := new PyramidT(X, Y, Z, Sides, Radius, Height, Material.Clone);
  public 
    constructor(x, y, z: real; N: integer; r, h: real; m: GMaterial);
{/// Радиус правильной пирамиды
    property Radius: real read GetR write SetR;
/// Высота правильной пирамиды
    property Height: real read GetH write SetH;
/// Количество боковых граней правильной пирамиды
    property Sides: integer read GetN write SetN;}
/// Возвращает клон правильной пирамиды
    function Clone := (inherited Clone) as PyramidT;
    function CreateModel: Visual3D; override;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс PrismTWireframe # Graph3D PrismTWireframe class
// ----------------------------------------------------- 
  [Serializable]
/// Класс проволочной правильной призмы
  PrismTWireframe = class(ObjectWithChildren3D,ISerializable)
  private 
    fn: integer;
    fh, fr: real;
    function Model := inherited model as LinesVisual3D;
    
    procedure SetCP(c: GColor) := (model as LinesVisual3D).Color := c;
    procedure SetC(c: GColor) := Invoke(SetCP, c);
    function GetC: GColor := Invoke&<GColor>(()->(model as LinesVisual3D).Color);
    procedure SetTP(th: real) := (model as LinesVisual3D).Thickness := th;
    procedure SetT(th: real) := Invoke(SetTP, th);
    function GetT: real := InvokeReal(()->(model as LinesVisual3D).Thickness);
    
    procedure SetRP(value: real);
    begin
      if fr = value then 
        exit;
      fr := value;
      (model as LinesVisual3D).Points := CreatePoints;
    end;
    
    procedure SetR(value: real) := Invoke(SetRP, value);
    procedure SetHP(value: real);
    begin
      if fh = value then 
        exit;
      fh := value;
      (model as LinesVisual3D).Points := CreatePoints;
    end;
    
    procedure SetH(value: real) := Invoke(SetHP, value);
    procedure SetNP(value: integer);
    begin
      if fN = value then 
        exit;
      fN := value;
      (model as LinesVisual3D).Points := CreatePoints;
    end;
    
    procedure SetN(value: integer) := Invoke(SetNP, value);
    
    function CreatePoints: Point3DCollection; virtual;
    begin
      var pc := new Point3DCollection;
      
      var a := PartitionPoints(0, 2 * Pi, Sides).Select(x -> P3D(fr * cos(x), fr * sin(x), 0)).ToArray;
      var b := PartitionPoints(0, 2 * Pi, Sides).Select(x -> P3D(fr * cos(x), fr * sin(x), fh)).ToArray;
      for var i := 0 to a.High - 1 do
      begin
        pc.Add(a[i]);
        pc.Add(b[i]);
      end;
      for var i := 0 to a.High - 1 do
      begin
        pc.Add(a[i]);
        pc.Add(a[i + 1]);
      end;
      for var i := 0 to a.High - 1 do
      begin
        pc.Add(b[i]);
        pc.Add(b[i + 1]);
      end;
      
      Result := pc;
    end;
    
    function NewVisualObject(N: integer; Radius, Height: real; Thickness: real; c: GColor): LinesVisual3D;
    begin
      (fn, fr, fh) := (n, Radius, Height);
      var ls := new LinesVisual3D;
      ls.Thickness := Thickness;
      ls.Color := c;
      ls.Points := CreatePoints;
      Result := ls;
    end;
  
  protected
    function CreateObject: Object3D; override := new PrismTWireframe(X, Y, Z, Sides, Radius, Height, (model as LinesVisual3D).Thickness, (model as LinesVisual3D).Color);
  public 
    function Points: Point3DCollection; virtual;
    begin
      var a := PartitionPoints(0, 2 * Pi, Sides).Select(x -> P3D(fr * cos(x), fr * sin(x), 0)).SkipLast;
      var b := PartitionPoints(0, 2 * Pi, Sides).Select(x -> P3D(fr * cos(x), fr * sin(x), fh)).SkipLast;
      var pc := new Point3DCollection(a + b);
      
      Result := pc;
    end;
    
    constructor(x, y, z: real; N: integer; Radius, Height: real; Thickness: real; c: GColor) := 
      CreateBase0(NewVisualObject(N, Radius, Height, Thickness, c), x, y, z);
    
/// Радиус проволочной правильной призмы
    property Radius: real read fr write SetR;
/// Высота проволочной правильной призмы
    property Height: real read fh write SetH;
/// Количество боковых граней проволочной правильной призмы
    property Sides: integer read fn write SetN;
/// Цвет проволоки правильной призмы
    property Color: GColor read GetC write SetC; override;
/// Толщина проволоки правильной призмы
    property Thickness: real read GetT write SetT;
///--
    function CreateModel: Visual3D; override := NewVisualObject(1,0,0,0,Colors.Transparent);
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('Radius', Radius, typeof(real));
      info.AddValue('Height', Height, typeof(real));
      info.AddValue('Sides', Sides, typeof(integer));
      info.AddValue('Color', ColorToLongword(Color), typeof(longword));
      info.AddValue('Thickness', Thickness, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Radius := info.GetDouble('Radius');
      Height := info.GetDouble('Height');
      Sides := info.GetInt32('Sides');
      Color := LongwordToColor(info.GetUInt32('Color'));
      Thickness := info.GetDouble('Thickness');
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс PyramidTWireframe # Graph3D PyramidTWireframe class
// ----------------------------------------------------- 
  [Serializable]
/// Класс проволочной правильной пирамиды
  PyramidTWireframe = class(PrismTWireframe,ISerializable)
  protected
    function CreateObject: Object3D; override := new PyramidTWireframe(X, Y, Z, Sides, Radius, Height, (model as LinesVisual3D).Thickness, (model as LinesVisual3D).Color);
  private 
    function CreatePoints: Point3DCollection; override;
    begin
      var pc := new Point3DCollection;
      
      var a := PartitionPoints(0, 2 * Pi, Sides).Select(x -> P3D(fr * cos(x), fr * sin(x), 0)).ToArray;
      var b := P3D(0, 0, fh);
      for var i := 0 to a.High - 1 do
      begin
        pc.Add(a[i]);
        pc.Add(b);
      end;
      for var i := 0 to a.High - 1 do
      begin
        pc.Add(a[i]);
        pc.Add(a[i + 1]);
      end;
      Result := pc;
    end;
  public    
/// Радиус проволочной правильной пирамиды
    property Radius: real read fr write SetR;
/// Высота проволочной правильной пирамиды
    property Height: real read fh write SetH;
/// Количество боковых граней проволочной правильной пирамиды
    property Sides: integer read fn write SetN;
/// Цвет проволоки правильной пирамиды
    property Color: GColor read GetC write SetC; override;
/// Толщина проволоки правильной пирамиды
    property Thickness: real read GetT write SetT;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
    end;
    
    constructor(x, y, z: real; N: integer; Radius, Height: real; Thickness: real; c: GColor);
    begin
      inherited Create(x, y, z, N, Radius, Height, Thickness, c);
    end;  
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
    end;
  end;
  
  P3DArray = array of Point3D;
  //P3DList = List<Point3D>;
  
// -----------------------------------------------------
//>>     Graph3D: класс SegmentsT # Graph3D SegmentsT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс Набор 3D-отрезков
  SegmentsT = class(ObjectWithChildren3D,ISerializable)
  private
    function Model := inherited model as LinesVisual3D;
    function GetTP: real := Model.Thickness;
    function GetT: real := InvokeReal(GetTP);
    procedure SetT(t: real) := Invoke(procedure(t: real)->Model.Thickness := t, t);
    function GetCP: GColor := Model.Color;
    function GetC: GColor := Invoke&<GColor>(GetCP);
    procedure SetC(t: GColor) := Invoke(procedure(t: GColor)->Model.Color := t, t);
    function GetPP: array of Point3D := Model.Points.ToArray;
    function GetP: array of Point3D; virtual := Invoke&<array of Point3D>(GetPP);
    procedure SetPP(pp: array of Point3D) := Model.Points := new Point3DCollection(pp);
    procedure SetP(pp: array of Point3D) := Invoke(SetPP, pp);
  protected
    function CreateObject: Object3D; override := new SegmentsT(Points, Thickness, Color);
  public 
    constructor(points: sequence of Point3D; thickness: real; c: GColor);
    begin
      var l := new LinesVisual3D;
      l.Thickness := thickness;
      l.Color := c;
      l.Points := new Point3DCollection(points);
      CreateBase0(l, 0, 0, 0);
    end;
    
/// Толщина 3D-отрезков
    property Thickness: real read GetT write SetT;
/// Цвет 3D-отрезков
    property Color: GColor read GetC write SetC; override;
/// Точки 3D-отрезков
    property Points: array of Point3D read GetP write SetP;
/// Возвращает клон 3D-отрезков
    function Clone := (inherited Clone) as SegmentsT;
///--
    function CreateModel: Visual3D; override := new LinesVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('Thickness', Thickness, typeof(real));
      info.AddValue('Color', ColorToLongword(Color), typeof(longword));
      info.AddValue('PointsCount', Points.Count, typeof(integer));
      for var i:=0 to Points.Length-1 do
      begin
        info.AddValue('px'+i, Points[i].X, typeof(real));
        info.AddValue('py'+i, Points[i].Y, typeof(real));
        info.AddValue('pz'+i, Points[i].Z, typeof(real));
      end;
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Thickness := info.GetDouble('Thickness');
      Color := LongwordToColor(info.GetUInt32('Color'));
      var count := info.GetInt32('PointsCount');
      var ll := new List<Point3D>;
      for var i:=0 to count-1 do
      begin
        var x := info.GetDouble('px'+i);
        var y := info.GetDouble('py'+i);
        var z := info.GetDouble('pz'+i);
        ll.Add(P3D(x,y,z));
      end;
      Points := ll.ToArray;
    end;
  end;
  
// -----------------------------------------------------
//>>     Graph3D: класс TorusT # Graph3D TorusT class
// ----------------------------------------------------- 
  [Serializable]
/// Класс Тор (бублик)
  TorusT = class(ObjectWithMaterial3D,ISerializable)
  private
    procedure SetD(d: real) := Invoke(procedure(d: real)->(model as TorusVisual3D).TorusDiameter := d, d);
    function  GetD: real := InvokeReal(()->(model as TorusVisual3D).TorusDiameter);
    procedure SetTD(d: real) := Invoke(procedure(d: real)->(model as TorusVisual3D).TubeDiameter := d, d);
    function  GetTD: real := InvokeReal(()->(model as TorusVisual3D).TubeDiameter);
  protected
    function CreateObject: Object3D; override := new TorusT(X, Y, Z, Diameter, TubeDiameter, Material.Clone);
  public 
    constructor(x, y, z: real; Diameter, TubeDiameter: real; m: Gmaterial);
    begin
      var t := new TorusVisual3D;
      t.TorusDiameter := Diameter;
      t.TubeDiameter := TubeDiameter;
      CreateBase(t, x, y, z, m);
    end;
    
/// Диаметр тора 
    property Diameter: real read GetD write SetD;
/// Диаметр трубы тора
    property TubeDiameter: real read GetTD write SetTD;
/// Возвращает клон тора
    function Clone := (inherited Clone) as PrismT;
    
    function CreateModel: Visual3D; override := new TorusVisual3D;
///--
    procedure GetObjectData(info: SerializationInfo; context: StreamingContext);
    begin
      inherited GetObjectData(info,context);
      info.AddValue('Diameter', Diameter, typeof(real));
      info.AddValue('TubeDiameter', TubeDiameter, typeof(real));
    end;
///--
    constructor Create(info: SerializationInfo; context: StreamingContext);
    begin
      inherited Create(info,context);
      Diameter := info.GetDouble('Diameter');
      TubeDiameter := info.GetDouble('TubeDiameter');
    end;
  end;
  
  
// -----------------------------------------------------
//>>     Graph3D: сервисные функции # Graph3D service functions
// ----------------------------------------------------- 
/// Возвращает пустую группу объектов в точке (x, y, z)
function Group(x, y, z: integer): Group3D;
/// Возвращает пустую группу объектов в точке p
function Group(p: Point3D): Group3D;
/// Возвращает пустую группу объектов в точке (0, 0, 0)
function Group: Group3D;
/// Возвращает группу объектов, перечисленных как параметры
function Group(params lst: array of Object3D): Group3D;
/// Возвращает группу объектов из последовательности en
function Group(en: sequence of Object3D): Group3D;
/// Не отображать слой графических объектов (обычно вызывается в начале до создания графических объектов)
procedure HideObjects;
/// Отображать слой графических объектов (вызывается после HideObjects и создания начальной сцены графических объектов)
procedure ShowObjects;

// -----------------------------------------------------
//>>     Graph3D: функции для создания 3D-объектов # Graph3D functions for creation 3D-objects
// ----------------------------------------------------- 
/// Возвращает сферу с центром в точке (x, y, z) радиуса Radius
function Sphere(x, y, z, Radius: real; m: Material := nil): SphereT;
/// Возвращает сферу с центром в точке center радиуса Radius
function Sphere(center: Point3D; Radius: real; m: Material := nil): SphereT;
/// Возвращает эллипсоид с центром в точке (x, y, z) и радиусами RadiusX, RadiusY, RadiusZ
function Ellipsoid(x, y, z, RadiusX, RadiusY, RadiusZ: real; m: Material := nil): EllipsoidT;
/// Возвращает эллипсоид с центром в точке center и радиусами RadiusX, RadiusY, RadiusZ
function Ellipsoid(center: Point3D; RadiusX, RadiusY, RadiusZ: real; m: Material := nil): EllipsoidT;
/// Возвращает куб с центром в точке (x, y, z) и длиной стороны SideLength
function Cube(x, y, z, SideLength: real; m: Material := nil): CubeT;
/// Возвращает куб с центром в точке center и длиной стороны SideLength
function Cube(center: Point3D; SideLength: real; m: Material := nil): CubeT;
/// Возвращает паралеллепипед с центром в точке (x, y, z) и размерами SizeX, SizeY, SizeZ
function Box(x, y, z, SizeX, SizeY, SizeZ: real; m: Material := nil): BoxT;
/// Возвращает паралеллепипед с центром в точке center и размерами SizeX, SizeY, SizeZ
function Box(center: Point3D; sz: Size3D; m: Material := nil): BoxT;
/// Возвращает 3D-стрелку с центром в точке (x, y, z), направлением (vx, vy, vz), диаметра Diameter и длиной наконечника HeadLength
function Arrow(x, y, z, vx, vy, vz, Diameter, HeadLength: real; m: Material := nil): ArrowT;
/// Возвращает 3D-стрелку с центром в точке (x, y, z), направлением (vx, vy, vz), диаметра Diameter
function Arrow(x, y, z, vx, vy, vz, Diameter: real; m: Material := nil): ArrowT;
/// Возвращает 3D-стрелку с центром в точке (x, y, z), направлением (vx, vy, vz)
function Arrow(x, y, z, vx, vy, vz: real; m: Material := nil): ArrowT;
/// Возвращает 3D-стрелку с центром в точке p, направлением p, диаметра Diameter и длиной наконечника HeadLength
function Arrow(p: Point3D; v: Vector3D; Diameter, HeadLength: real; m: Material := nil): ArrowT;
/// Возвращает 3D-стрелку с центром в точке p, направлением p, диаметра Diameter
function Arrow(p: Point3D; v: Vector3D; Diameter: real; m: Material := nil): ArrowT;
/// Возвращает 3D-стрелку с центром в точке p, направлением p
function Arrow(p: Point3D; v: Vector3D; m: Material := nil): ArrowT;

///--
function TruncatedCone(x, y, z, Height, Radius, TopRadius: real; topcap: boolean; m: Material := nil): TruncatedConeT; 
/// Возвращает усеченный конус с центром основания в точке (x, y, z) высоты Height, радиуса основания Radius, верхнего радиуса TopRadius
function TruncatedCone(x, y, z, Height, Radius, TopRadius: real; m: Material := nil): TruncatedConeT;
///--
function TruncatedCone(p: Point3D; Height, Radius, TopRadius: real; topcap: boolean; m: Material := nil): TruncatedConeT;
/// Возвращает усеченный конус с центром основания в точке p высоты Height, радиуса основания Radius, верхнего радиуса TopRadius
function TruncatedCone(p: Point3D; Height, Radius, TopRadius: real; m: Material := nil): TruncatedConeT;

///--
function Cylinder(x, y, z, Height, Radius: real; topcap: boolean; m: Material := nil): CylinderT;
/// Возвращает цилиндр с центром основания в точке (x, y, z) высоты Height радиуса Radius
function Cylinder(x, y, z, Height, Radius: real; m: Material := nil): CylinderT;
///--
function Cylinder(p: Point3D; Height, Radius: real; topcap: boolean; m: Material := nil): CylinderT;
/// Возвращает цилиндр с центром основания в точке p высоты Height радиуса Radius
function Cylinder(p: Point3D; Height, Radius: real; m: Material := nil): CylinderT;
/// Возвращает трубу с центром основания в точке (x, y, z) высотой Height, радиусом Radius и внутренним радиусом InnerRadius
function Tube(x, y, z, Height, Radius, InnerRadius: real; m: Material := nil): PipeT;
/// Возвращает трубу с центром основания в точке p высотой Height, радиусом Radius и внутренним радиусом InnerRadius
function Tube(p: Point3D; Height, Radius, InnerRadius: real; m: Material := nil): PipeT;
/// Возвращает конус с центром основания в точке (x, y, z) высотой Height, радиусом Radius
function Cone(x, y, z, Height, Radius: real; m: Material := nil): TruncatedConeT;
/// Возвращает конус с центром основания в точке p высотой Height, радиусом Radius
function Cone(p: Point3D; Height, Radius: real; m: Material := nil): TruncatedConeT;
/// Возвращает чайник с центром в точке (x, y, z) 
function Teapot(x, y, z: real; c: Material := nil): TeapotT;
/// Возвращает чайник с центром в точке p 
function Teapot(p: Point3D; c: Material := nil): TeapotT;
/// Возвращает текст на билборде с центром в точке (x, y, z) с размером шрифта Fontsize
function BillboardText(x, y, z: real; Text: string; Fontsize: real := 12): BillboardTextT;
/// Возвращает текст на билборде с центром в точке p с размером шрифта Fontsize
function BillboardText(p: Point3D; Text: string; Fontsize: real := 12): BillboardTextT;
/// Возвращает координатную систему с длиной стрелок ArrowsLength и диаметром стрелок Diameter
function CoordinateSystem(ArrowsLength, Diameter: real): CoordinateSystemT;
/// Возвращает координатную систему с длиной стрелок ArrowsLength
function CoordinateSystem(ArrowsLength: real := 2): CoordinateSystemT;
/// Возвращает 3D-текст с центром в точке (x, y, z) с высотой Height, именем шрифта FontName заданного цвета
function Text3D(x, y, z: real; Text: string; Height: real; FontName: string; c: Color): TextT;
/// Возвращает 3D-текст с центром в точке (x, y, z) с высотой Height, именем шрифта FontName
function Text3D(x, y, z: real; Text: string; Height: real; FontName: string := 'Arial'): TextT;
/// Возвращает 3D-текст с центром в точке p с высотой Height, именем шрифта FontName заданного цвета
function Text3D(p: Point3D; Text: string; Height: real; FontName: string; c: Color): TextT;
/// Возвращает 3D-текст с центром в точке p с высотой Height, именем шрифта FontName 
function Text3D(p: Point3D; Text: string; Height: real; FontName: string := 'Arial'): TextT;
/// Возвращает 3D-текст с центром в точке (x, y, z) с высотой Height заданного цвета
function Text3D(x, y, z: real; Text: string; Height: real; c: Color): TextT;
/// Возвращает 3D-текст с центром в точке p с высотой Height заданного цвета
function Text3D(p: Point3D; Text: string; Height: real; c: Color): TextT;
/// Возвращает 3D-прямоугольник с центром в точке (x, y, z) длины Length, ширины Width, нормалью Normal и направлением длины LengthDirection
function Rectangle3D(x, y, z, Length, Width: real; Normal, LengthDirection: Vector3D; m: Material := nil): RectangleT;
/// Возвращает 3D-прямоугольник с центром в точке p длины Length, ширины Width, нормалью Normal и направлением длины LengthDirection
function Rectangle3D(p: Point3D; Length, Width: real; Normal, LengthDirection: Vector3D; m: Material := nil): RectangleT;
/// Возвращает 3D-прямоугольник с центром в точке (x, y, z) длины Length, ширины Width, нормалью Normal
function Rectangle3D(x, y, z, Length, Width: real; Normal: Vector3D; m: Material := nil): RectangleT; 
/// Возвращает 3D-прямоугольник с центром в точке (x, y, z) длины Length, ширины Width
function Rectangle3D(x, y, z, Length, Width: real; m: Material := nil): RectangleT; 
/// Возвращает 3D-прямоугольник с центром в точке p длины Length, ширины Width, нормалью Normal
function Rectangle3D(p: Point3D; Length, Width: real; Normal: Vector3D; m: Material := nil): RectangleT; 
/// Возвращает 3D-прямоугольник с центром в точке p длины Length, ширины Width
function Rectangle3D(p: Point3D; Length, Width: real; m: Material := nil): RectangleT; 

/// Загружает модель из файла в форматах .obj, .3ds, .lwo, .objz, .stl, .off и отображает ее в точке (x, y, z)
function FileModel3D(x, y, z: real; fname: string; m: Material): FileModelT;
/// Загружает модель из файла в форматах .obj, .3ds, .lwo, .objz, .stl, .off и отображает ее в точке p
function FileModel3D(p: Point3D; fname: string; m: Material): FileModelT;

/// Возвращает правильную призму с центром основания в точке (x, y, z), количеством сторон Sides, высотой Height и радиусом Radius
function Prism(x, y, z: real; Sides: integer; Height, Radius: real; m: Material := nil): PrismT;
/// Возвращает правильную призму с центром основания в точке p, количеством сторон Sides, высотой Height и радиусом Radius
function Prism(p: Point3D; Sides: integer; Height, Radius: real; m: Material := nil): PrismT;
/// Возвращает проволочную правильную призму с центром основания в точке (x, y, z), количеством сторон Sides, высотой Height, радиусом Radius и толщиной проволоки Thickness
function PrismWireFrame(x, y, z: real; Sides: integer; Height, Radius: real; Thickness: real; c: Color): PrismTWireFrame;
/// Возвращает проволочную правильную призму с центром основания в точке (x, y, z), количеством сторон Sides, высотой Height, радиусом Radius и толщиной проволоки Thickness
function PrismWireFrame(x, y, z: real; Sides: integer; Height, Radius: real; Thickness: real := 1.2): PrismTWireFrame;
/// Возвращает проволочную правильную призму с центром основания в точке p, количеством сторон Sides, высотой Height, радиусом Radius и толщиной проволоки Thickness
function PrismWireFrame(p: Point3D; Sides: integer; Height, Radius: real; Thickness: real; c: Color): PrismTWireFrame;
/// Возвращает проволочную правильную призму с центром основания в точке p, количеством сторон Sides, высотой Height, радиусом Radius и толщиной проволоки Thickness
function PrismWireFrame(p: Point3D; Sides: integer; Height, Radius: real; Thickness: real := 1.2): PrismTWireFrame;
/// Возвращает правильную пирамиду с центром основания в точке (x, y, z), количеством сторон Sides, высотой Height и радиусом Radius
function Pyramid(x, y, z: real; Sides: integer; Height, Radius: real; m: Material := nil): PyramidT;
/// Возвращает правильную пирамиду с центром основания в точке p, количеством сторон Sides, высотой Height и радиусом Radius
function Pyramid(p: Point3D; Sides: integer; Height, Radius: real; m: Material := nil): PyramidT;
/// Возвращает проволочную правильную пирамиду с центром основания в точке (x, y, z), количеством сторон Sides, высотой Height и радиусом Radius
function PyramidWireFrame(x, y, z: real; Sides: integer; Height, Radius: real; Thickness: real; c: Color): PyramidTWireFrame;
/// Возвращает проволочную правильную пирамиду с центром основания в точке (x, y, z), количеством сторон Sides, высотой Height и радиусом Radius
function PyramidWireFrame(x, y, z: real; Sides: integer; Height, Radius: real; Thickness: real := 1.2): PyramidTWireFrame;
/// Возвращает проволочную правильную пирамиду с центром основания в точке p, количеством сторон Sides, высотой Height и радиусом Radius
function PyramidWireFrame(p: Point3D; Sides: integer; Height, Radius: real; Thickness: real; c: Color): PyramidTWireFrame;
/// Возвращает проволочную правильную пирамиду с центром основания в точке p, количеством сторон Sides, высотой Height и радиусом Radius
function PyramidWireFrame(p: Point3D; Sides: integer; Height, Radius: real; Thickness: real := 1.2): PyramidTWireFrame;
/// Возвращает лего-деталь с центром в точке (x, y, z), размера (Rows, Columns, Height), измеряемом в количестве кирпичиков по каждой размерности 
function Lego(x, y, z: real; Rows, Columns, Height: integer; m: Material := nil): LegoT;
/// Возвращает икосаэдр с центром в точке (x, y, z) и радиусом описанной окружности Radius
function Icosahedron(x, y, z, Radius: real; m: Material := nil): IcosahedronT;
/// Возвращает додекаэдр с центром в точке (x, y, z) и радиусом описанной окружности Radius
function Dodecahedron(x, y, z, Radius: real; m: Material := nil): DodecahedronT;
/// Возвращает тетраэдр с центром в точке (x, y, z) и радиусом описанной окружности Radius
function Tetrahedron(x, y, z, Radius: real; m: Material := nil): TetrahedronT;
/// Возвращает октаэдр с центром в точке (x, y, z) и радиусом описанной окружности Radius
function Octahedron(x, y, z, Radius: real; m: Material := nil): OctahedronT;
/// Возвращает икосаэдр с центром в точке p и радиусом описанной окружности Radius
function Icosahedron(p: Point3D; Radius: real; m: Material := nil): IcosahedronT;
/// Возвращает додекаэдр с центром в точке p и радиусом описанной окружности Radius
function Dodecahedron(p: Point3D; Radius: real; m: Material := nil): DodecahedronT;
/// Возвращает тетраэдр с центром в точке p и радиусом описанной окружности Radius
function Tetrahedron(p: Point3D; Radius: real; m: Material := nil): TetrahedronT;
/// Возвращает октаэдр с центром в точке p и радиусом описанной окружности Radius
function Octahedron(p: Point3D; Radius: real; m: Material := nil): OctahedronT;
/// Возвращает набор отрезков, задаваемых последовательностью точек points, толщиной Thickness, заданного цвета. Количество точек должно быть четным
function Segments3D(points: sequence of Point3D; Thickness: real; c: Color): SegmentsT;
/// Возвращает набор отрезков, задаваемых последовательностью точек points, толщиной Thickness. Количество точек должно быть четным
function Segments3D(points: sequence of Point3D; Thickness: real := 1.2): SegmentsT;
/// Возвращает ломаную, задаваемую последовательностью точек points, толщиной Thickness, заданного цвета
function Polyline3D(points: sequence of Point3D; Thickness: real; c: Color): SegmentsT;
/// Возвращает ломаную, задаваемую последовательностью точек points, толщиной Thickness
function Polyline3D(points: sequence of Point3D; Thickness: real := 1.2): SegmentsT;
/// Возвращает замкнутую ломаную, задаваемую последовательностью точек points, толщиной Thickness, заданного цвета
function Polygon3D(points: sequence of Point3D; Thickness: real; c: Color): SegmentsT;
/// Возвращает замкнутую ломаную, задаваемую последовательностью точек points, толщиной Thickness
function Polygon3D(points: sequence of Point3D; Thickness: real := 1.2): SegmentsT;
/// Возвращает отрезок из точки p1 в точку p2 толщиной Thickness заданного цвета
function Segment3D(p1, p2: Point3D; Thickness: real; c: Color): SegmentsT;
/// Возвращает отрезок из точки p1 в точку p2 толщиной Thickness заданного цвета
function Segment3D(p1, p2: Point3D; Thickness: real := 1.2): SegmentsT;
/// Возвращает тор (бублик) с центром в точке (x, y, z), диаметром Diameter и диаметром трубы TubeDiameter
function Torus(x, y, z, Diameter, TubeDiameter: real; m: Material := nil): TorusT;
/// Возвращает тор (бублик) с центром в точке p, диаметром Diameter и диаметром трубы TubeDiameter
function Torus(p: Point3D; Diameter, TubeDiameter: real; m: Material := nil): TorusT;
/// Возвращает треугольник, соединяющий точки p1, p2, p3
function Triangle(p1, p2, p3: Point3D; m: Material := nil): TriangleT;

// Конец примитивов
//------------------------------------------------------------------------------------

// -----------------------------------------------------
//>>     Graph3D: функции для создания невизуальных объектов # Graph3D functions for creation non-visual objects
// -----------------------------------------------------
/// Возвращает невизуальную плоскость, проходящую через точку p и имеющую нормаль Normal
function Plane(p: Point3D; Normal: Vector3D): Plane3D;

/// Возвращает невизуальный луч, проходящий через точку p в направлении вектора v
function Ray(p: Point3D; v: Vector3D): Ray3D;

/// Возвращает невизуальную прямую, проходящую через точку p в направлении вектора v
function Line(p: Point3D; v: Vector3D): Line3D;

/// Возвращает невизуальную прямую, проходящую через точки p1 и p2
function Line(p1, p2: Point3D): Line3D;

/// Возвращает невизуальный луч, выпущенный из камеры и проходящий через точку (x,y) экрана
function GetRay(x, y: real): Ray3D;

/// Создает пустую анимацию заданной длительности
function EmptyAnim(sec: real): EmptyAnimation;

// -----------------------------------------------------
//>>     Graph3D: функции для определения ближайших точек и объектов # Graph3D functions for nearest points and objects
// -----------------------------------------------------
/// Создаёт траекторию в виде массива точек, заданную параметрически. Функция fun отображает параметр t на координаты точки в пространстве
function ParametricTrajectory(a,b: real; N: integer; fun: real->Point3D): sequence of Point3D;

/// Возвращает ближайший 3D-объект, который пересекает луч, выпущенный из камеры и проходящий через точку (x,y) экрана
function FindNearestObject(x, y: real): Object3D;

/// Возвращает точку на ближайшем 3D-объекте, который пересекает луч, выпущенный из камеры и проходящий через точку (x,y) экрана. Если пересечения нет, возвращается точка BadPoint
function FindNearestObjectPoint(x, y: real): Point3D;

/// Возвращает точку на плоскости Plane, которую пересекает невизуальный луч, выпущенный из камеры и проходящий через точку (x,y) экрана
function PointOnPlane(Plane: Plane3D; x, y: real): Point3D;

/// Возвращает ближайшую точку на линии Line с лучом, выпущенным из камеры и проходящем через точку (x,y) экрана
function NearestPointOnLine(Line: Ray3D; x, y: real): Point3D;

/// Начинает анимацию, основанную на кадре, и передаёт в каждый обработчик кадра время dt, прошедшее с момента последней перерисовки
procedure BeginFrameBasedAnimationTime(Draw: procedure(dt: real));

/// Заканчивает анимацию, основанную на кадре
procedure EndFrameBasedAnimation;

/// Сериализует трёхмерный объект в файл
procedure SerializeObject3D(filename: string; obj: Object3D);

/// Десериализует трёхмерный объект из файла
function DeserializeObject3D(filename: string): Object3D;

/// Отключить стандартные обработчики событий мыши
procedure SystemMouseEventsOff;

/// Включить стандартные обработчики событий мыши
procedure SystemMouseEventsOn;
  
var  
// -----------------------------------------------------
//>>     События модуля Graph3D # Graph3D events
// -----------------------------------------------------
  /// Событие нажатия на кнопку мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 1, если нажата левая кнопка мыши, и 2, если нажата правая кнопка мыши
  OnMouseDown: procedure(x, y: real; mousebutton: integer);
  /// Событие отжатия кнопки мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 1, если отжата левая кнопка мыши, и 2, если отжата правая кнопка мыши
  OnMouseUp: procedure(x, y: real; mousebutton: integer);
  /// Событие перемещения мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 0, если кнопка мыши не нажата, 1, если нажата левая кнопка мыши, и 2, если нажата правая кнопка мыши
  OnMouseMove: procedure(x, y: real; mousebutton: integer);
  /// Событие вращения колёсика мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 0, если кнопка мыши не нажата, 1, если нажата левая кнопка мыши, и 2, если нажата правая кнопка мыши; delta<0 - колесо мыши вниз, delta>0 - колесо мыши вверх
  OnMouseWheel: procedure(x, y: real; mousebutton,delta: integer);
  /// Событие нажатия клавиши
  OnKeyDown: procedure(k: Key);
  /// Событие отжатия клавиши
  OnKeyUp: procedure(k: Key);
  /// Событие нажатия символьной клавиши
  OnKeyPress: procedure(ch: char);
  /// Событие перерисовки графического 3D-окна. 
  ///Инициализируется процедурой с вещественным параметром dt - временем, прошедшим с момента последнего обновления экрана
  OnDrawFrame: procedure(dt: real);
  /// Событие, происходящее при закрытии основного окна
  var OnClose: procedure;

var
// -----------------------------------------------------
//>>     Переменные модуля Graph3D # Graph3D variables
// -----------------------------------------------------
/// Пространство отображения
  View3D: View3DType;
/// Графическое окно
  Window: WindowType;
/// Камера
  Camera: CameraType;
/// Источники света
  Lights: LightsType;
/// Координатная сетка
  GridLines: GridLinesType;
/// Список 3D-объектов
  Object3DList := new List<Object3D>;
/// Орт (единичный вектор) оси OX
  OrtX: Vector3D := V3D(1, 0, 0);
/// Орт (единичный вектор) оси OY
  OrtY: Vector3D := V3D(0, 1, 0);
/// Орт (единичный вектор) оси OZ
  OrtZ: Vector3D := V3D(0, 0, 1);
/// Нулевой вектор
  ZeroVector: Vector3D := V3D(0, 0, 0);
/// Точка начала координат
  Origin: Point3D := P3D(0, 0, 0);
/// Плоскость OXY
  PlaneXY: Plane3D := Plane(Origin, OrtZ);
/// Плоскость OYZ
  PlaneYZ: Plane3D := Plane(Origin, OrtX);
/// Плоскость OXZ
  PlaneXZ: Plane3D := Plane(Origin, OrtY);
/// Ось OX
  XAxis: Ray3D := Ray(Origin, OrtX);
/// Ось OY
  YAxis: Ray3D := Ray(Origin, OrtY);
/// Ось OZ
  ZAxis: Ray3D := Ray(Origin, OrtZ);
/// Плохая точка (возвращается в случае неуспеха функцией FindNearestObjectPoint) 
  BadPoint: Point3D := P3D(real.MaxValue, real.MaxValue, real.MaxValue);

//{{{--doc: Конец секции 1 }}} 

procedure Proba2;
function Any(x, y, z: real; c: Color): ObjectWithMaterial3D;

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;

implementation

procedure HideObjects;
begin
  Invoke(()->begin hvp.Visibility := Visibility.Hidden end);
end;

procedure ShowObjects;
begin
  Invoke(()->begin hvp.Visibility := Visibility.Visible end);
end;


type 
  CustomBinder = class(SerializationBinder)
    public function BindToType(assemblyName, typeName: string): System.Type; override;
    begin
      var currentasm := System.Reflection.Assembly.GetExecutingAssembly();
      var s := typeName + ', ' +currentasm.ToString();
      Result := System.Type.GetType(s); // игнорировать assemblyname!!!
    end;
  end;  

procedure SerializeObject3D(filename: string; obj: Object3D);
begin
  Invoke(procedure -> begin
    var fs := new System.IO.FileStream(filename,System.IO.FileMode.Create);
    var formatter := new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter;
    formatter.Serialize(fs,obj);
    fs.Close;
  end);  
end;

var magiccounter := 1;

function DeserializeObject3D(filename: string): Object3D;
begin
  if magiccounter = 0 then // никогда не выполнится
  begin
    var a := typeof(Object3D);
    a := typeof(ObjectWithChildren3D);
    a := typeof(ObjectWithMaterial3D);
    a := typeof(Group3D);
    a := typeof(SphereT);
    a := typeof(EllipsoidT);
    a := typeof(CubeT);
    a := typeof(BoxT);
    a := typeof(ArrowT);
    a := typeof(TruncatedConeT);
    a := typeof(CylinderT);
    a := typeof(TeapotT);
    a := typeof(CoordinateSystemT);
    a := typeof(BillboardTextT);
    a := typeof(TextT);
    a := typeof(RectangleT);
    a := typeof(FileModelT);
    a := typeof(PipeT);
    a := typeof(LegoT);
    a := typeof(PlatonicAbstractT);
    a := typeof(IcosahedronT);
    a := typeof(DodecahedronT);
    a := typeof(TetrahedronT);
    a := typeof(OctahedronT);
    a := typeof(TriangleT);
    a := typeof(PrismT);
    a := typeof(PyramidT);
    a := typeof(PrismTWireframe);
    a := typeof(PyramidTWireframe);
    a := typeof(SegmentsT);
    a := typeof(TorusT);
    a := a;
  end;
  var res: Object3D;
  Invoke(procedure -> begin
    var fs := new System.IO.FileStream(filename,System.IO.FileMode.Open);
    var formatter := new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter;
    formatter.Binder := new CustomBinder();
    res := formatter.Deserialize(fs) as Object3D;
    fs.Close;
  end);
  Result := res;
end;  

procedure Redraw(p: procedure) := GraphWPFBase.Invoke(p);
procedure Invoke(p: procedure) := GraphWPFBase.Invoke(p);

function RGB(r, g, b: byte) := Color.Fromrgb(r, g, b);
function ARGB(a, r, g, b: byte) := Color.FromArgb(a, r, g, b);
function P3D(x, y, z: real) := new Point3D(x, y, z);
function V3D(x, y, z: real) := new Vector3D(x, y, z);
function Sz3D(x, y, z: real) := new Size3D(x, y, z);
function Pnt(x, y: real) := new Point(x, y);
function Rect(x, y, w, h: real) := new System.Windows.Rect(x, y, w, h);
function RandomColor: Color := RGB(Random(256), Random(256), Random(256));
function EmptyColor: Color := ARGB(0,0,0,0);
function GrayColor(b: byte): Color := RGB(b, b, b);
function RandomSolidBrush: SolidColorBrush := new SolidColorBrush(RandomColor);

type
  IMHelper = auto class
    fname: string;
    M, N: real;
    function ImageMaterial: Material;
    begin
      //Result := MaterialHelper.CreateImageMaterial(fname)
      var bi := new System.Windows.Media.Imaging.BitmapImage();
      bi.BeginInit();
      bi.UriSource := new System.Uri(fname, System.UriKind.Relative); 
      bi.EndInit();
      var b := new ImageBrush(bi);
      b.Viewport := Rect(0, 0, M, N);
      if (M <> 1) or (N <> 1) then
        b.TileMode := TileMode.Tile;
      Result := new GDiffuseMaterial(b);
    end;  
  end;
  DEMHelper = auto class
    c: Color;
    function Diffuse := new GDiffuseMaterial(new SolidColorBrush(c));
    function Emissive := new GEmissiveMaterial(new SolidColorBrush(c));
  end;
  SpMHelper = auto class
    c: Color;
    specularpower: real;
    function SpecularMaterial := new System.Windows.Media.Media3D.SpecularMaterial(new SolidColorBrush(c), specularpower);
  end;

function ImageMaterial(fname: string; M,N: real): Material := Invoke&<Material>(IMHelper.Create(fname, M, N).ImageMaterial);
function DiffuseMaterial(c: Color): Material := Invoke&<Material>(DEMHelper.Create(c).Diffuse);
function SpecularMaterial(specularBrightness: byte; specularpower: real): Material := Invoke&<Material>(SpMHelper.Create(RGB(specularBrightness, specularBrightness, specularBrightness), specularpower).SpecularMaterial);
function SpecularMaterial(c: Color; specularpower: real): Material := Invoke&<Material>(SpMHelper.Create(c, specularpower).SpecularMaterial);
function EmissiveMaterial(c: Color): Material := Invoke&<Material>(DEMHelper.Create(c).Emissive);
function RainbowMaterial: Material := GMaterials.Rainbow;

//function wplus: real := SystemParameters.WindowResizeBorderThickness.Left + SystemParameters.WindowResizeBorderThickness.Right;

//function hplus: real := SystemParameters.WindowCaptionHeight + SystemParameters.WindowResizeBorderThickness.Top + SystemParameters.WindowResizeBorderThickness.Bottom;

procedure Object3D.Serialize(fname: string) := SerializeObject3D(fname,Self);

static function Object3D.DeSerialize(fname: string): Object3D := DeserializeObject3D(fname) as Object3D;

function Object3D.AnimMoveTo(x, y, z, seconds: real; Completed: procedure) := new OffsetAnimation(Self, seconds, x, y, z, Completed);

function Object3D.AnimMoveTrajectory(trajectory: sequence of Point3D; seconds: real; Completed: procedure) := new OffsetAnimationUsingKeyframes(Self, seconds, trajectory, Completed);

function Object3D.AnimMoveBy(dx, dy, dz, seconds: real; Completed: procedure) := new OffsetAnimationOn(Self, seconds, dx, dy, dz, Completed);

function Object3D.AnimScale(sc, seconds: real; Completed: procedure) := new ScaleAnimation(Self, seconds, sc, Completed);

function Object3D.AnimScaleX(sc, seconds: real; Completed: procedure) := new ScaleXAnimation(Self, seconds, sc, Completed);

function Object3D.AnimScaleY(sc, seconds: real; Completed: procedure) := new ScaleYAnimation(Self, seconds, sc, Completed);

function Object3D.AnimScaleZ(sc, seconds: real; Completed: procedure) := new ScaleZAnimation(Self, seconds, sc, Completed);

function Object3D.AnimRotate(vx, vy, vz, angle, seconds: real; Completed: procedure) := new RotateAtAnimation(Self, seconds, vx, vy, vz, angle, P3D(0, 0, 0), Completed);

function Object3D.AnimRotateAt(axis: Vector3D; angle: real; center: Point3D; seconds: real; Completed: procedure) := new RotateAtAnimation(Self, seconds, axis.X, axis.y, axis.z, angle, center, Completed);

function Object3D.AnimRotateAtAbsolute(axis: Vector3D; angle: real; center: Point3D; seconds: real; Completed: procedure) := new RotateAtAbsoluteAnimation(Self, seconds, axis.X, axis.y, axis.z, angle, center, Completed);

procedure Object3D.AddToObject3DList := Object3DList.Add(Self);

procedure Object3D.DeleteFromObject3DList;
begin
  var oc := Self as ObjectWithChildren3D;
  foreach var c in oc.l do
    c.DeleteFromObject3DList;
  Object3DList.Remove(Self)
end;

function EmptyAnim(sec: real) := EmptyAnimation.Create(sec);

function AnimationBase.&Then(second: AnimationBase): AnimationBase := Animate.Sequence(Self, second);

//------------------------------ End Animation -------------------------------

type
  LegoVisual3D = class(MeshElement3D)
  private
  public 
    class HeightProperty: DependencyProperty;
    class RowsProperty: DependencyProperty;
    class ColumnsProperty: DependencyProperty;
    class DivisionsProperty: DependencyProperty;
  private    
    procedure SetHeight(value: integer) := SetValue(HeightProperty, value);
    function GetHeight := integer(GetValue(HeightProperty));
    procedure SetRows(value: integer) := SetValue(RowsProperty, value);
    function GetRows := integer(GetValue(RowsProperty));
    procedure SetColumns(value: integer) := SetValue(ColumnsProperty, value);
    function GetColumns := integer(GetValue(ColumnsProperty));
    procedure SetDivisions(value: integer) := SetValue(DivisionsProperty, value);
    function GetDivisions := integer(GetValue(DivisionsProperty));

    class constructor;
    begin
      HeightProperty := DependencyProperty.Register('Height', typeof(integer), typeof(LegoVisual3D), new UIPropertyMetadata(3, GeometryChanged));
      RowsProperty := DependencyProperty.Register('Raws', typeof(integer), typeof(LegoVisual3D), new UIPropertyMetadata(3, GeometryChanged));
      ColumnsProperty := DependencyProperty.Register('Columns', typeof(integer), typeof(LegoVisual3D), new UIPropertyMetadata(3, GeometryChanged));
      DivisionsProperty := DependencyProperty.Register('Divisions', typeof(integer), typeof(LegoVisual3D), new UIPropertyMetadata(48));
    end;
    
  public 
    property Height: integer read GetHeight write SetHeight;
    property Rows: integer read GetRows write SetRows;
    property Columns: integer read GetColumns write SetColumns;
    property Divisions: integer read GetDivisions write SetDivisions;
  public 
    function Tessellate(): MeshGeometry3D; override;
    const
      m = 1 / 0.008;
      grid = 0.008 * m;
      margin = 0.0001 * m;
      wallThickness = 0.001 * m;
      plateThickness = 0.0032 * m;
      brickThickness = 0.0096 * m;
      knobHeight = 0.0018 * m;
      knobDiameter = 0.0048 * m;
      outerDiameter = 0.00651 * m;
      axleDiameter = 0.00475 * m;
      holeDiameter = 0.00485 * m;
    begin
      var width := Columns * grid - margin * 2;
      var length := Rows * grid - margin * 2;
      var height1 := Height * plateThickness;
      var builder := new MeshBuilder(true, true);
      for var i := 0 to Columns - 1 do
        for var j := 0 to Rows - 1 do
        begin
          var o := new Point3D((i + 0.5) * grid, (j + 0.5) * grid, height1);
          builder.AddCone(o, new Vector3D(0, 0, 1), knobDiameter / 2, knobDiameter / 2, knobHeight, false, true, Divisions);
          builder.AddPipe(new Point3D(o.X, o.Y, o.Z - wallThickness), new Point3D(o.X, o.Y, wallThickness),
                          knobDiameter, outerDiameter, Divisions);
        end;
      
      builder.AddBox(new Point3D(Columns * 0.5 * grid, Rows * 0.5 * grid, height1 - wallThickness / 2), width, length,
                    wallThickness,
                    BoxFaces.All);
      builder.AddBox(new Point3D(margin + wallThickness / 2, Rows * 0.5 * grid, height1 / 2 - wallThickness / 2),
                     wallThickness, length, height1 - wallThickness,
                     BoxFaces.All xor BoxFaces.Top);
      builder.AddBox(
          new Point3D(Columns * grid - margin - wallThickness / 2, Rows * 0.5 * grid, height1 / 2 - wallThickness / 2),
          wallThickness, length, height1 - wallThickness,
          BoxFaces.All xor BoxFaces.Top);
      builder.AddBox(new Point3D(Columns * 0.5 * grid, margin + wallThickness / 2, height1 / 2 - wallThickness / 2),
                     width, wallThickness, height1 - wallThickness,
                     BoxFaces.All xor BoxFaces.Top);
      builder.AddBox(
          new Point3D(Columns * 0.5 * grid, Rows * grid - margin - wallThickness / 2, height1 / 2 - wallThickness / 2),
          width, wallThickness, height1 - wallThickness,
          BoxFaces.All xor BoxFaces.Top);
      Result := builder.ToMesh(false);
    end;
  end;

constructor LegoT.Create(x, y, z: real; Rows, Columns, Height: integer; m: GMaterial);
begin
  var bx := new LegoVisual3D;
  (bx.Rows, bx.Height, bx.Columns) := (Rows, Height, Columns);
  CreateBase(bx, x, y, z, m);
end;

function LegoT.CreateModel: Visual3D := new LegoVisual3D;


procedure LegoT.SetWP(r: integer) := (model as LegoVisual3D).Rows := r;
procedure LegoT.SetW(r: integer) := Invoke(SetWP, r);
function LegoT.GetW: integer := InvokeInteger(()->(model as LegoVisual3D).Rows);

procedure LegoT.SetHP(r: integer) := (model as LegoVisual3D).Height := r;
procedure LegoT.SetH(r: integer) := Invoke(SetHP, r); 
function LegoT.GetH: integer := InvokeInteger(()->(model as LegoVisual3D).Height);

procedure LegoT.SetLP(r: integer) := (model as LegoVisual3D).Columns := r;
procedure LegoT.SetL(r: integer) := Invoke(SetLP, r);
function LegoT.GetL: integer := InvokeInteger(()->(model as LegoVisual3D).Columns);

type
  Panel = class
    Points: array of Point3D;
    TriangleIndex: integer;
  end;
  
  ModelTypes = (TetrahedronType, OctahedronType, HexahedronType, IcosahedronType, DodecahedronType, StellatedOctahedronType, MyAny);
  
  PanelModelBuilder = class
    Panels: List<Panel> := new List<Panel>;
    TriangleIndexToPanelIndex: List<integer>;
    
    procedure AddPanel(params points: array of Point3D);
    begin
      var p := new Panel;
      p.points := points;
      Panels.Add(p);
    end;
    
    procedure AddPanel(params coords: array of real);
    begin
      var points := new Point3D[coords.Length div 3];
      for var i := 0 to coords.Length div 3 - 1 do
        points[i] := new Point3D(coords[i * 3], coords[i * 3 + 1], coords[i * 3 + 2]);
      Reverse(points);
      AddPanel(points);
    end;
    
    function ToMeshGeometry3D(): MeshGeometry3D;
    begin
      TriangleIndexToPanelIndex := new List<integer>;
      
      var tm := new MeshBuilder(false, false);
      var panelIndex := 0;
      foreach var p in Panels do
      begin
        p.TriangleIndex := tm.Positions.Count;
        tm.AddTriangleFan(p.Points, nil, nil);
        for var i := 0 to p.Points.Length - 3 do
          TriangleIndexToPanelIndex.Add(panelIndex);
        
        panelIndex += 1;
      end;
      var panelsGeometry := tm.ToMesh(false);
      
      Result := panelsGeometry;
    end;
  end;

function CreateModel(CurrentModelType: ModelTypes; a: real): MeshGeometry3D;
begin
  var pmb := new PanelModelBuilder();
  case CurrentModelType of 
    TetrahedronType:
      begin
        a /= Sqrt(8); // тогда длина ребра = 1
        pmb.AddPanel(a, a, a, -a, a, -a, a, -a, -a);
        pmb.AddPanel(-a, a, -a, -a, -a, a, a, -a, -a);
        pmb.AddPanel(a, a, a, a, -a, -a, -a, -a, a);
        pmb.AddPanel(a, a, a, -a, -a, a, -a, a, -a);
      end;
    OctahedronType:
      begin
        a /= 2;
        var b := 0.5 * (2 * Sqrt(2)) * a;
        pmb.AddPanel(-a, 0, a, -a, 0, -a, 0, b, 0);
        pmb.AddPanel(-a, 0, -a, a, 0, -a, 0, b, 0);
        pmb.AddPanel(a, 0, -a, a, 0, a, 0, b, 0);
        pmb.AddPanel(a, 0, a, -a, 0, a, 0, b, 0);
        pmb.AddPanel(a, 0, -a, -a, 0, -a, 0, -b, 0);
        pmb.AddPanel(-a, 0, -a, -a, 0, a, 0, -b, 0);
        pmb.AddPanel(a, 0, a, a, 0, -a, 0, -b, 0);
        pmb.AddPanel(-a, 0, a, a, 0, a, 0, -b, 0);
      end;
    HexahedronType:
      begin
        a /= 2;
        pmb.AddPanel(-a, -a, a, a, -a, a, a, -a, -a, -a, -a, -a);
        pmb.AddPanel(-a, a, -a, -a, a, a, -a, -a, a, -a, -a, -a);
        pmb.AddPanel(-a, a, a, a, a, a, a, -a, a, -a, -a, a);
        pmb.AddPanel(a, a, -a, a, a, a, -a, a, a, -a, a, -a);
        pmb.AddPanel(a, -a, a, a, a, a, a, a, -a, a, -a, -a);
        pmb.AddPanel(a, -a, -a, a, a, -a, -a, a, -a, -a, -a, -a);
      end;
    IcosahedronType:
      begin
        a /= Sqrt(5) - 1;
        var phi := (1 + Sqrt(5)) / 2;
        var b := 1.0 / (2 * phi) * 2 * a;
        pmb.AddPanel(0, b, -a, b, a, 0, -b, a, 0);
        pmb.AddPanel(0, b, a, -b, a, 0, b, a, 0);
        pmb.AddPanel(0, b, a, 0, -b, a, -a, 0, b);
        pmb.AddPanel(0, b, a, a, 0, b, 0, -b, a);
        pmb.AddPanel(0, b, -a, 0, -b, -a, a, 0, -b);
        pmb.AddPanel(0, b, -a, -a, 0, -b, 0, -b, -a);
        pmb.AddPanel(0, -b, a, b, -a, 0, -b, -a, 0);
        pmb.AddPanel(0, -b, -a, -b, -a, 0, b, -a, 0);
        pmb.AddPanel(-b, a, 0, -a, 0, b, -a, 0, -b);
        pmb.AddPanel(-b, -a, 0, -a, 0, -b, -a, 0, b);
        pmb.AddPanel(b, a, 0, a, 0, -b, a, 0, b);
        pmb.AddPanel(b, -a, 0, a, 0, b, a, 0, -b);
        pmb.AddPanel(0, b, a, -a, 0, b, -b, a, 0);
        pmb.AddPanel(0, b, a, b, a, 0, a, 0, b);
        pmb.AddPanel(0, b, -a, -b, a, 0, -a, 0, -b);
        pmb.AddPanel(0, b, -a, a, 0, -b, b, a, 0);
        pmb.AddPanel(0, -b, -a, -a, 0, -b, -b, -a, 0);
        pmb.AddPanel(0, -b, -a, b, -a, 0, a, 0, -b);
        pmb.AddPanel(0, -b, a, -b, -a, 0, -a, 0, b);
        pmb.AddPanel(0, -b, a, a, 0, b, b, -a, 0);
      end;
    DodecahedronType:
      begin
        var phi := (1 + Sqrt(5)) / 2;
        a /= 2 * (2 - phi);
        var b := 1 / phi * a;
        var c := (2 - phi) * a;
        pmb.AddPanel(c, 0, a, -c, 0, a, -b, b, b, 0, a, c, b, b, b);
        pmb.AddPanel(-c, 0, a, c, 0, a, b, -b, b, 0, -a, c, -b, -b, b);
        pmb.AddPanel(c, 0, -a, -c, 0, -a, -b, -b, -b, 0, -a, -c, b, -b, -b);
        pmb.AddPanel(-c, 0, -a, c, 0, -a, b, b, -b, 0, a, -c, -b, b, -b);
        pmb.AddPanel(b, b, -b, a, c, 0, b, b, b, 0, a, c, 0, a, -c);
        
        pmb.AddPanel(-b, b, b, -a, c, 0, -b, b, -b, 0, a, -c, 0, a, c);
        pmb.AddPanel(-b, -b, -b, -a, -c, 0, -b, -b, b, 0, -a, c, 0, -a, -c);
        
        pmb.AddPanel(b, -b, b, a, -c, 0, b, -b, -b, 0, -a, -c, 0, -a, c);
        pmb.AddPanel(a, c, 0, a, -c, 0, b, -b, b, c, 0, a, b, b, b);
        pmb.AddPanel(a, -c, 0, a, c, 0, b, b, -b, c, 0, -a, b, -b, -b);
        pmb.AddPanel(-a, c, 0, -a, -c, 0, -b, -b, -b, -c, 0, -a, -b, b, -b);
        pmb.AddPanel(-a, -c, 0, -a, c, 0, -b, b, b, -c, 0, a, -b, -b, b);
      end;
    StellatedOctahedronType:
      begin
        pmb.AddPanel(a, a, a, -a, a, -a, a, -a, -a);
        pmb.AddPanel(-a, a, -a, -a, -a, a, a, -a, -a);
        pmb.AddPanel(a, a, a, a, -a, -a, -a, -a, a);
        pmb.AddPanel(a, a, a, -a, -a, a, -a, a, -a);
        pmb.AddPanel(-a, a, a, a, a, -a, -a, -a, -a);
        pmb.AddPanel(a, a, -a, a, -a, a, -a, -a, -a);
        pmb.AddPanel(-a, a, a, -a, -a, -a, a, -a, a);
        pmb.AddPanel(-a, a, a, a, -a, a, a, a, -a);
      end;
    MyAny:
      begin
        pmb.AddPanel(0, 0, 0, -a, 0, 0, -a, 0, a, 0, 0, a);
        pmb.AddPanel(-a, 0, 0, 0, a, 0, 0, a, a, -a, 0, a);
        pmb.AddPanel(0, a, 0, 0, 0, 0, 0, 0, a, 0, a, a);
        pmb.AddPanel(0, 0, 0, 0, a, 0, -a, 0, 0);
        pmb.AddPanel(0, 0, a, -a, 0, a, 0, a, a);
      end;
  end;
  Result := pmb.ToMeshGeometry3D;
end;

type
  PlatonicAbstractVisual3D = abstract class(MeshElement3D)
  private 
    fa: real;
    procedure SetA(value: real);begin fa := value; OnGeometryChanged; end;
  
  public 
    constructor(Length: real);
    begin
      inherited Create;
      Self.Length := Length;
    end;
    
    property Length: real read fa write SetA;
  end;
  IcosahedronVisual3D = class(PlatonicAbstractVisual3D)
    public function Tessellate(): MeshGeometry3D; override := CreateModel(ModelTypes.IcosahedronType, Length);
  end;
  DodecahedronVisual3D = class(PlatonicAbstractVisual3D)
    public function Tessellate(): MeshGeometry3D; override := CreateModel(ModelTypes.DodecahedronType, Length);
  end;
  TetrahedronVisual3D = class(PlatonicAbstractVisual3D)
    public function Tessellate(): MeshGeometry3D; override := CreateModel(ModelTypes.TetrahedronType, Length);
  end;
  OctahedronVisual3D = class(PlatonicAbstractVisual3D)
    public function Tessellate(): MeshGeometry3D; override := CreateModel(ModelTypes.OctahedronType, Length);
  end;
  MyAnyVisual3D = class(PlatonicAbstractVisual3D)
    public function Tessellate(): MeshGeometry3D; override := CreateModel(ModelTypes.MyAny, Length);
  end;
  
function PlatonicAbstractT.GetLength: real := InvokeReal(()->(model as PlatonicAbstractVisual3D).Length);
procedure PlatonicAbstractT.SetLengthP(r: real) := (model as PlatonicAbstractVisual3D).Length := r;
  
constructor IcosahedronT.Create(x, y, z, Length: real; m: GMaterial) := CreateBase(new IcosahedronVisual3D(Length), x, y, z, m);
function IcosahedronT.CreateModel: Visual3D := new IcosahedronVisual3D;
  
constructor DodecahedronT.Create(x, y, z, Length: real; m: GMaterial) := CreateBase(new DodecahedronVisual3D(Length), x, y, z, m);
function DodecahedronT.CreateModel: Visual3D := new DodecahedronVisual3D;
  
constructor TetrahedronT.Create(x, y, z, Length: real; m: GMaterial) := CreateBase(new TetrahedronVisual3D(Length), x, y, z, m);
function TetrahedronT.CreateModel: Visual3D := new TetrahedronVisual3D;
  
constructor OctahedronT.Create(x, y, z, Length: real; m: GMaterial) := CreateBase(new OctahedronVisual3D(Length), x, y, z, m);
function OctahedronT.CreateModel: Visual3D := new OctahedronVisual3D;

  
type
  PrismVisual3D = class(MeshElement3D)
  private 
    fn: integer;
    fh, fr: real;
    procedure SetR(value: real); begin fr := value; OnGeometryChanged; end;
    
    procedure SetH(value: real); begin fh := value; OnGeometryChanged; end;
    
    procedure SetN(value: integer); begin fn := value; OnGeometryChanged; end;
  
  public 
    constructor;
    begin
    end;

    constructor(N: integer; Radius, Height: real);
    begin
      (fn, fr, fh) := (n, Radius, Height);
      OnGeometryChanged;
    end;
    
    property Height: real read fh write SetH;
    property Radius: real read fr write SetR;
    property N: integer read fn write SetN;
  protected 
    function Tessellate(): MeshGeometry3D; override;
    begin
      var pmb := new PanelModelBuilder();
      if N > 0 then
      begin
        var a := PartitionPoints(0, 2 * Pi, N).Select(x -> P3D(fr * cos(x), fr * sin(x), 0)).ToArray;
        var b := PartitionPoints(0, 2 * Pi, N).Select(x -> P3D(fr * cos(x), fr * sin(x), fh)).ToArray;
        for var i := 0 to fn - 1 do
          pmb.AddPanel(a[i + 1].X, a[i + 1].Y, a[i + 1].Z, a[i].X, a[i].Y, a[i].Z, b[i].X, b[i].Y, b[i].Z, b[i + 1].X, b[i + 1].Y, b[i + 1].Z);
        pmb.AddPanel(a.Reverse.ToArray);
        pmb.AddPanel(b);
      end;
      Result := pmb.ToMeshGeometry3D
    end;
  end;
  
  PyramidVisual3D = class(PrismVisual3D)
  protected 
    function Tessellate(): MeshGeometry3D; override;
    begin
      var pmb := new PanelModelBuilder();
      if N > 0 then
      begin
        var a := PartitionPoints(0, 2 * Pi, N).Select(x -> P3D(fr * cos(x), fr * sin(x), 0)).ToArray;
        var top := P3D(0, 0, fh);
        for var i := 0 to fn - 1 do
          pmb.AddPanel(a[i + 1].X, a[i + 1].Y, a[i + 1].Z, a[i].X, a[i].Y, a[i].Z, top.X, top.Y, top.Z);
        pmb.AddPanel(a.Reverse.ToArray);
      end;
      Result := pmb.ToMeshGeometry3D
    end;
  end;
  
  TriangleVisual3D = class(MeshElement3D)
  private 
    pp1, pp2, pp3: Point3D;
  protected 
    function Tessellate(): MeshGeometry3D; override;
    begin
      var m := new MeshBuilder(true);
      m.AddTriangle(p1, p2, p3);
      Result := m.ToMesh;
    end;
    
    procedure SetP1(p1: Point3D);
    begin
      pp1 := p1;
      OnGeometryChanged;
    end;
    
    procedure SetP2(p1: Point3D);
    begin
      pp2 := p2;
      OnGeometryChanged;
    end;
    
    procedure SetP3(p3: Point3D);
    begin
      pp3 := p3;
      OnGeometryChanged;
    end;
  
  public 
    constructor(ppp1, ppp2, ppp3: Point3D);
    begin
      (pp1, pp2, pp3) := (ppp1, ppp2, ppp3);
      Material := Materialhelper.CreateMaterial(Colors.Red);
      OnGeometryChanged;
    end;
    
    property P1: Point3D read pp1 write SetP1;
    property P2: Point3D read pp2 write SetP2;
    property P3: Point3D read pp3 write SetP3;
    procedure SetPoints(p1, p2, p3: Point3D);
    begin
      pp1 := p1;
      pp2 := p2;
      pp3 := p3;
      OnGeometryChanged;
    end;
  end;
  

procedure TriangleT.SetP1(p: Point3D) := Invoke(procedure(p: Point3D)->(model as TriangleVisual3D).P1 := p, p);
function  TriangleT.GetP1: Point3D := Invoke&<Point3D>(()->(model as TriangleVisual3D).P1);
procedure TriangleT.SetP2(p: Point3D) := Invoke(procedure(p: Point3D)->(model as TriangleVisual3D).P2 := p, p);
function  TriangleT.GetP2: Point3D := Invoke&<Point3D>(()->(model as TriangleVisual3D).P2);
procedure TriangleT.SetP3(p: Point3D) := Invoke(procedure(p: Point3D)->(model as TriangleVisual3D).P3 := p, p);
function  TriangleT.GetP3: Point3D := Invoke&<Point3D>(()->(model as TriangleVisual3D).P3);
procedure TriangleT.SetPoints(p1, p2, p3: Point3D) := Invoke(procedure(p1, p2, p3: Point3D)->begin (model as TriangleVisual3D).SetPoints(p1, p2, p3); end, p1, p2, p3);
function TriangleT.CreateModel: Visual3D := new TriangleVisual3D;
    
function TriangleT.CreateObject: Object3D := new TriangleT((model as TriangleVisual3D).p1, (model as TriangleVisual3D).p2, (model as TriangleVisual3D).p3, Material.Clone);

constructor TriangleT.Create(p1, p2, p3: Point3D; m: Gmaterial);
begin
  CreateBase(new TriangleVisual3D(p1, p2, p3), 0, 0, 0, m);
  (model as TriangleVisual3D).BackMaterial := (model as TriangleVisual3D).Material;      
end;
    
procedure PrismT.SetR(r: real) := Invoke(procedure(r: real)->(model as PrismVisual3D).Radius := r, r);
function  PrismT.GetR: real := InvokeReal(()->(model as PrismVisual3D).Radius);
procedure PrismT.SetH(r: real) := Invoke(procedure(r: real)->(model as PrismVisual3D).Height := r, r);
function  PrismT.GetH: real := InvokeReal(()->(model as PrismVisual3D).Height);
procedure PrismT.SetN(n: integer) := Invoke(procedure(n: integer)->(model as PrismVisual3D).N := n, n);
function  PrismT.GetN: integer := InvokeInteger(()->(model as PrismVisual3D).N);

constructor PrismT.Create(x, y, z: real; N: integer; r, h: real; m: Gmaterial) := CreateBase(new PrismVisual3D(N, r, h), x, y, z, m);
function PrismT.CreateModel: Visual3D := new PrismVisual3D;
  
constructor PyramidT.Create(x, y, z: real; N: integer; r, h: real; m: GMaterial) := CreateBase(new PyramidVisual3D(N, r, h), x, y, z, m);
function PyramidT.CreateModel: Visual3D := new PyramidVisual3D;
    

  
function DefaultMaterialColor := RandomColor;

function DefaultMaterial: Material := MaterialHelper.CreateMaterial(DefaultMaterialColor);

function Group(x, y, z: integer): Group3D := Inv(()->Group3D.Create(x, y, z));

function Group(p: Point3D): Group3D := Inv(()->Group3D.Create(p.x, p.y, p.z));

function Group: Group3D := Inv(()->Group3D.Create(0, 0, 0));

function Group(params lst: array of Object3D): Group3D := Inv(()->Group3D.Create(0, 0, 0, lst));

function Group(en: sequence of Object3D): Group3D := Inv(()->Group3D.Create(0, 0, 0, en));

function Sphere(x, y, z, Radius: real; m: Material): SphereT := Inv(()->SphereT.Create(x, y, z, Radius, m));

function Sphere(center: Point3D; Radius: real; m: Material) := Sphere(center.x, center.y, center.z, Radius, m);

function Ellipsoid(x, y, z, RadiusX, RadiusY, RadiusZ: real; m: Material): EllipsoidT := Inv(()->EllipsoidT.Create(x, y, z, RadiusX, RadiusY, RadiusZ, m));

function Ellipsoid(center: Point3D; RadiusX, RadiusY, RadiusZ: real; m: Material) := Ellipsoid(center.x, center.y, center.z, RadiusX, RadiusY, RadiusZ, m);

function Cube(x, y, z, SideLength: real; m: Material): CubeT := Inv(()->CubeT.Create(x, y, z, SideLength, m));

function Cube(center: Point3D; SideLength: real; m: Material): CubeT := Cube(center.x, center.y, center.z, SideLength, m);

function Box(x, y, z, SizeX, SizeY, SizeZ: real; m: Material): BoxT := Inv(()->BoxT.Create(x, y, z, SizeX, SizeY, SizeZ, m));

function Box(center: Point3D; sz: Size3D; m: Material): BoxT := Inv(()->BoxT.Create(center.x, center.y, center.z, sz.X, sz.Y, sz.z, m));

const
  arhl = 3; ard = 0.2;

function Arrow(x, y, z, vx, vy, vz, diameter, HeadLength: real; m: Material): ArrowT := Inv(()->ArrowT.Create(x, y, z, vx, vy, vz, diameter, HeadLength, m));

function Arrow(x, y, z, vx, vy, vz, diameter: real; m: Material) := Arrow(x, y, z, vx, vy, vz, diameter, arhl, m);

function Arrow(x, y, z, vx, vy, vz: real; m: Material) := Arrow(x, y, z, vx, vy, vz, ard, arhl, m);

function Arrow(p: Point3D; v: Vector3D; diameter, HeadLength: real; m: Material) := Arrow(p.x, p.y, p.z, v.X, v.Y, v.Z, diameter, HeadLength, m);

function Arrow(p: Point3D; v: Vector3D; diameter: real; m: Material) := Arrow(p.x, p.y, p.z, v.X, v.Y, v.Z, diameter, arhl, m);

function Arrow(p: Point3D; v: Vector3D; m: Material) := Arrow(p.x, p.y, p.z, v.X, v.Y, v.Z, ard, arhl, m);

function TruncatedConeAux(x, y, z, Height, Radius, TopRadius: real; sides: integer; topcap: boolean; c: Material) := Inv(()->TruncatedConeT.Create(x, y, z, Height, Radius, TopRadius, sides, topcap, c));

const
  maxsides = 37;

///--
function TruncatedCone(x, y, z, Height, Radius, TopRadius: real; topcap: boolean; m: Material): TruncatedConeT := TruncatedConeAux(x, y, z, Height, Radius, TopRadius, maxsides, topcap, m); 

function TruncatedCone(x, y, z, Height, Radius, TopRadius: real; m: Material) := TruncatedCone(x, y, z, Height, Radius, TopRadius, True, m);
///--
function TruncatedCone(p: Point3D; Height, Radius, TopRadius: real; topcap: boolean; m: Material) := TruncatedCone(p.x, p.y, p.z, Height, Radius, TopRadius, topcap, m);

function TruncatedCone(p: Point3D; Height, Radius, TopRadius: real; m: Material) := TruncatedCone(p.x, p.y, p.z, Height, Radius, TopRadius, True, m);

///--
function Cylinder(x, y, z, Height, Radius: real; topcap: boolean; m: Material): CylinderT := Inv(()->CylinderT.Create(x, y, z, Height, Radius, maxsides, topcap, m));

function Cylinder(x, y, z, Height, Radius: real; m: Material) := Cylinder(x, y, z, Height, Radius, True, m);
///--
function Cylinder(p: Point3D; Height, Radius: real; topcap: boolean; m: Material) := Cylinder(p.x, p.y, p.z, Height, Radius, topcap, m);

function Cylinder(p: Point3D; Height, Radius: real; m: Material) := Cylinder(p.x, p.y, p.z, Height, Radius, True, m);

function Tube(x, y, z, Height, Radius, InnerRadius: real; m: Material): PipeT := Inv(()->PipeT.Create(x, y, z, Height, Radius, InnerRadius, m));

function Tube(p: Point3D; Height, Radius, InnerRadius: real; m: Material) := Tube(p.x, p.y, p.z, Height, Radius, InnerRadius, m);

function Cone(x, y, z, Height, Radius: real; m: Material): TruncatedConeT := TruncatedCone(x, y, z, Height, Radius, 0, True, m);

function Cone(p: Point3D; Height, Radius: real; m: Material) := TruncatedCone(p.x, p.y, p.z, Height, Radius, 0, True, m);

function Teapot(x, y, z: real; c: Material): TeapotT := Inv(()->TeapotT.Create(x, y, z, c));

function Teapot(p: Point3D; c: Material) := Teapot(p.x, p.y, p.z, c);

function BillboardText(x, y, z: real; Text: string; Fontsize: real): BillboardTextT := Inv(()->BillboardTextT.Create(x, y, z, text, fontsize));

function BillboardText(p: Point3D; Text: string; Fontsize: real) := BillboardText(P.x, p.y, p.z, text, fontsize);

function CoordinateSystem(ArrowsLength, Diameter: real): CoordinateSystemT := Inv(()->CoordinateSystemT.Create(0, 0, 0, arrowslength, diameter));

function CoordinateSystem(ArrowsLength: real) := CoordinateSystem(arrowslength, (arrowslength / 10).ClampTop(0.18));

function Text3D(x, y, z: real; Text: string; Height: real; fontname: string; c: Color): TextT := Inv(()->TextT.Create(x, y, z, text, height, fontname, c));

function Text3D(x, y, z: real; Text: string; Height: real; FontName: string): TextT
  := Text3D(x, y, z, Text, Height, FontName, Colors.Black);

function Text3D(p: Point3D; Text: string; Height: real; fontname: string; c: Color) := Text3D(P.x, p.y, p.z, text, height, fontname, c);

function Text3D(p: Point3D; Text: string; Height: real; FontName: string): TextT
  := Text3D(p, Text, Height, FontName, Colors.Black);

function Text3D(x, y, z: real; Text: string; Height: real; c: Color) := Text3D(x, y, z, text, height, 'Arial', c);

function Text3D(p: Point3D; Text: string; Height: real; c: Color) := Text3D(p.x, p.y, p.z, text, height, 'Arial', c);

function Rectangle3D(x, y, z, Length, Width: real; Normal, LengthDirection: Vector3D; m: Material): RectangleT := Inv(()->RectangleT.Create(x, y, z, Length, Width, normal, LengthDirection, m));

function Rectangle3D(p: Point3D; Length, Width: real; Normal, LengthDirection: Vector3D; m: Material): RectangleT := Rectangle3D(p.x, p.y, p.z, Length, Width, Normal, LengthDirection, m);

function Rectangle3D(x, y, z, Length, Width: real; Normal: Vector3D; m: Material): RectangleT := Rectangle3D(x, y, z, Length, Width, Normal, OrtX, m); 

function Rectangle3D(x, y, z, Length, Width: real; m: Material): RectangleT := Rectangle3D(x, y, z, Length, Width, OrtZ, OrtX, m); 

function Rectangle3D(p: Point3D; Length, Width: real; Normal: Vector3D; m: Material): RectangleT := Rectangle3D(p.x, p.y, p.z, Length, Width, Normal, OrtX, m); 

function Rectangle3D(p: Point3D; Length, Width: real; m: Material): RectangleT := Rectangle3D(p.x, p.y, p.z, Length, Width, OrtZ, OrtX, m); 

/// Загружает модель из файла .obj, .3ds, .lwo, .objz, .stl, .off
function FileModel3D(x, y, z: real; fname: string; m: Material): FileModelT := Inv(()->FileModelT.Create(x, y, z, fname, m));

function FileModel3D(p: Point3D; fname: string; m: Material): FileModelT := FileModel3D(p.x, p.y, p.z, fname, m);

function Prism(x, y, z: real; Sides: integer; Height, Radius: real; m: Material): PrismT := Inv(()->PrismT.Create(x, y, z, Sides, Radius, Height, m));

function Prism(p: Point3D; Sides: integer; Height, Radius: real; m: Material): PrismT := Prism(p.X, p.Y, p.Z, Sides, Height, Radius, m);

function PrismWireFrame(x, y, z: real; Sides: integer; Height, Radius: real; Thickness: real; c: Color): PrismTWireFrame := Inv(()->PrismTWireFrame.Create(x, y, z, Sides, Radius, Height, thickness, c));

function PrismWireFrame(x, y, z: real; Sides: integer; Height, Radius: real; Thickness: real): PrismTWireFrame
  := PrismWireFrame(x, y, z, Sides, Height, Radius, Thickness, GrayColor(64));

function PrismWireFrame(p: Point3D; Sides: integer; Height, Radius: real; Thickness: real; c: Color): PrismTWireFrame := PrismWireFrame(p.x, p.y, p.z, Sides, Height, Radius, thickness, c);

function PrismWireFrame(p: Point3D; Sides: integer; Height, Radius: real; Thickness: real): PrismTWireFrame
  := PrismWireFrame(p, Sides, Height, Radius, Thickness, GrayColor(64));

function Pyramid(x, y, z: real; Sides: integer; Height, Radius: real; m: Material): PyramidT := Inv(()->PyramidT.Create(x, y, z, Sides, Radius, Height, m));

function Pyramid(p: Point3D; Sides: integer; Height, Radius: real; m: Material): PyramidT := Pyramid(p.X, p.Y, p.Z, Sides, Height, Radius, m);

function PyramidWireFrame(x, y, z: real; Sides: integer; Height, Radius: real; Thickness: real; c: Color): PyramidTWireFrame := Inv(()->PyramidTWireFrame.Create(x, y, z, Sides, Radius, Height, thickness, c));

function PyramidWireFrame(x, y, z: real; Sides: integer; Height, Radius: real; Thickness: real): PyramidTWireFrame
  := PyramidWireFrame(x, y, z, Sides, Height, Radius, Thickness, GrayColor(64));

function PyramidWireFrame(p: Point3D; Sides: integer; Height, Radius: real; Thickness: real; c: Color): PyramidTWireFrame := PyramidWireFrame(p.x, p.y, p.z, Sides, Height, Radius, thickness, c);

function PyramidWireFrame(p: Point3D; Sides: integer; Height, Radius: real; Thickness: real): PyramidTWireFrame
  := PyramidWireFrame(p, Sides, Height, Radius, Thickness, GrayColor(64));

function Lego(x, y, z: real; Rows, Columns, Height: integer; m: Material): LegoT := Inv(()->LegoT.Create(x, y, z, Rows, Columns, Height, m));

function Icosahedron(x, y, z, Radius: real; m: Material): IcosahedronT := Inv(()->IcosahedronT.Create(x, y, z, 4 * Radius / Sqrt(2) / Sqrt(5 + Sqrt(5)), m));

function Dodecahedron(x, y, z, Radius: real; m: Material): DodecahedronT := Inv(()->DodecahedronT.Create(x, y, z, Radius * 4 / Sqrt(3) / (1 + Sqrt(5)), m));

function Tetrahedron(x, y, z, Radius: real; m: Material): TetrahedronT := Inv(()->TetrahedronT.Create(x, y, z, 4 * Radius / Sqrt(6), m));

function Octahedron(x, y, z, Radius: real; m: Material): OctahedronT := Inv(()->OctahedronT.Create(x, y, z, Radius * Sqrt(2), m));

function Icosahedron(p: Point3D; Radius: real; m: Material): IcosahedronT := Icosahedron(p.X, p.Y, p.Z, Radius, m);

function Dodecahedron(p: Point3D; Radius: real; m: Material): DodecahedronT := Dodecahedron(p.X, p.Y, p.Z, Radius, m);

function Tetrahedron(p: Point3D; Radius: real; m: Material): TetrahedronT := Tetrahedron(p.X, p.Y, p.Z, Radius, m);

function Octahedron(p: Point3D; Radius: real; m: Material): OctahedronT := Octahedron(p.X, p.Y, p.Z, Radius, m);

function Segments3D(points: sequence of Point3D; thickness: real; c: Color): SegmentsT := Inv(()->SegmentsT.Create(points, thickness, c));

function Segments3D(points: sequence of Point3D; Thickness: real): SegmentsT
  := Segments3D(points, Thickness, GrayColor(64));

function Polyline3D(points: sequence of Point3D; Thickness: real): SegmentsT
  := Polyline3D(points, Thickness, GrayColor(64));

function Polygon3D(points: sequence of Point3D; Thickness: real): SegmentsT
  := Polygon3D(points, Thickness, GrayColor(64));

function Segment3D(p1, p2: Point3D; Thickness: real): SegmentsT
  := Segment3D(p1, p2, Thickness, GrayColor(64));

function Polyline3D(points: sequence of Point3D; thickness: real; c: Color): SegmentsT := Inv(()->SegmentsT.Create(points.Pairwise.SelectMany(x -> Seq(x[0], x[1])), thickness, c));

function Polygon3D(points: sequence of Point3D; thickness: real; c: Color): SegmentsT := Inv(()->SegmentsT.Create((points + points.First).Pairwise.SelectMany(x -> Seq(x[0], x[1])), thickness, c));

function Segment3D(p1, p2: Point3D; thickness: real; c: Color): SegmentsT := Inv(()->SegmentsT.Create(Seq(p1, p2), thickness, c));

function Torus(x, y, z, Diameter, TubeDiameter: real; m: Material): TorusT := Inv(()->TorusT.Create(x, y, z, Diameter, TubeDiameter, m));

function Torus(p: Point3D; Diameter, TubeDiameter: real; m: Material): TorusT := Torus(p.x, p.y, p.z, Diameter, TubeDiameter, m);

function Triangle(p1, p2, p3: Point3D; m: Material): TriangleT := Inv(()->TriangleT.Create(p1, p2, p3, m));

// Конец примитивов
//------------------------------------------------------------------------------------

// Функции для точек, лучей, прямых, плоскостей

function ParametricTrajectory(a,b: real; N: integer; fun: real->Point3D) := PartitionPoints(a,b,N).Select(fun);

function FindNearestObject(x, y: real): Object3D;
begin
  Result := nil;
  var v := hvp.FindNearestVisual(new Point(x, y));
  foreach var obj in Object3DList do
    if obj.model = v then
      Result := obj
end;

function FindNearestObjectPoint(x, y: real): Point3D;
begin
  var p1 := hvp.FindNearestPoint(Pnt(x, y));
  if p1.HasValue then
    Result := p1.Value
  else Result := BadPoint;
end;

function Plane(p: Point3D; normal: Vector3D): Plane3D := new Plane3D(p, normal);

function Ray(p: Point3D; v: Vector3D): Ray3D := new Ray3D(p, v);

function Line(p: Point3D; v: Vector3D): Line3D := new Line3D(p, v);

function Line(p1, p2: Point3D): Line3D := new Line3D(p1, p2 - p1);

function GetRay(x, y: real): Ray3D := hvp.Viewport.GetRay(Pnt(x, y));

function PointOnPlane(Self: Plane3D; x, y: real): Point3D; extensionmethod;
begin
  var r := GetRay(x, y);
  var p1 := r.PlaneIntersection(Self.Position, Self.Normal);
  if p1.HasValue then
    Result := p1.Value
  else Result := BadPoint;
end;

function NearestPointOnLine(Self: Ray3D; x, y: real): Point3D; extensionmethod;
begin
  var ray := GetRay(x, y);
  var a := Self.Direction;
  var b := ray.Direction;
  var ab := Vector3D.CrossProduct(a, b);
  var planeNormal := Vector3D.CrossProduct(b, ab);
  var p := Self.PlaneIntersection(ray.Origin, planeNormal);
  if p.HasValue then
    Result := p.Value
  else Result := BadPoint;
end;

function PointOnPlane(Plane: Plane3D; x, y: real): Point3D := Plane.PointOnPlane(x,y);

function NearestPointOnLine(Line: Ray3D; x, y: real): Point3D := Line.NearestPointOnLine(x,y);


// Методы расширения для анимаций 

function Sec(Self: integer): real; extensionmethod := Self;

function Sec(Self: real): real; extensionmethod := Self;

// А теперь - тадам! - перегрузка + для Animate.Sequence и перегрузка * для Animate.Group
function operator+(a, b: AnimationBase): AnimationBase; extensionmethod := Animate.Sequence(a, b);

function operator*(a, b: AnimationBase): AnimationBase; extensionmethod := Animate.Group(a, b);

// Конец методов расширения для анимаций 

// Методы расширения для точки

function operator*(p: Point3D; r: real): Point3D; extensionmethod := p.Multiply(r);

function operator*(r: real; p: Point3D): Point3D; extensionmethod := p.Multiply(r);

function operator+(p1, p2: Point3D): Point3D; extensionmethod := p3d(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);

function operator-(v: Vector3D): Vector3D; extensionmethod := v3d(-v.x,-v.y,-v.z);

function MoveX(Self: Point3D; dx: real): Point3D; extensionmethod := P3D(Self.x + dx, Self.y, Self.z);

function MoveY(Self: Point3D; dy: real): Point3D; extensionmethod := P3D(Self.x, Self.y + dy, Self.z);

function MoveZ(Self: Point3D; dz: real): Point3D; extensionmethod := P3D(Self.x, Self.y, Self.z + dz);

function Move(Self: Point3D; dx, dy, dz: real): Point3D; extensionmethod := P3D(Self.x + dx, Self.y + dy, Self.z + dz);

// Конец методов расширения для точки 

type 
  TupleInt3 = (integer, integer, integer);
  TupleReal3 = (real, real, real);

// Разные методы расширения

function ChangeOpacity(Self: GColor; value: integer): Color; extensionmethod := ARGB(value, Self.R, Self.G, Self.B);

function operator implicit(t: TupleInt3): Point3D; extensionmethod := new Point3D(t[0], t[1], t[2]);

function operator implicit(t: TupleReal3): Point3D; extensionmethod := new Point3D(t[0], t[1], t[2]);

function operator implicit(ar: array of TupleInt3): Point3DCollection; extensionmethod := new Point3DCollection(ar.Select(t -> new Point3D(t[0], t[1], t[2])));

function operator implicit(ar: array of Point3D): Point3DCollection; extensionmethod := new Point3DCollection(ar);

function operator implicit(ar: List<Point3D>): Point3DCollection; extensionmethod := new Point3DCollection(ar);

// Конец разных методов расширения

type
  GMHelper = auto class
    a, b: Material;
    function GroupMaterial: Material;
    begin
      var g := new MaterialGroup();
      g.Children.Add(a);
      g.Children.Add(b);
      Result := g;
    end;
  end;

// Методы расширения для Material 

function operator+(a, b: Material): Material; extensionmethod := Invoke&<Material>(GMHelper.Create(a, b).GroupMaterial);

function operator implicit(c: GColor): GMaterial; extensionmethod := Materialhelper.CreateMaterial(c);

// Конец методов расширения для Material 


//=============================================================================

// Экспериментальные функции и классы
type
  MyAnyT = class(PlatonicAbstractT)
  protected  
    function CreateObject: Object3D; override := new MyAnyT(X, Y, Z, Length, Material);
  public 
    constructor(x, y, z, Length: real; m: GMaterial);
    begin
      CreateBase(new MyAnyVisual3D(Length), x, y, z, m);
    end;
    
    function Clone := (inherited Clone) as MyAnyT;
  end;


procedure ProbaP;
begin
  //var m := MaterialHelper.CreateMaterial(Brushes.Green,100,100);
  //m.AmbientColor := Colors.Red;
  //m.Color := Colors.Green;
  //var bi := new System.Windows.Media.Imaging.BitmapImage(new System.Uri('dog.png',System.UriKind.Relative));
  //var b := new ImageBrush(bi);
  //b.ViewportUnits := BrushMappingMode.Absolute;
  //b.Viewport := Rect(0,0,0.2,0.3);
  //b.TileMode := System.Windows.Media.TileMode.Tile;
  //Cube(6,-4,0,4,MaterialHelper.CreateMaterial(b));
  Sphere(2, -4, 0, 2, MaterialHelper.CreateMaterial(Brushes.Green, 0.4, 100, 255));
  Sphere(-2, -4, 0, 2, MaterialHelper.CreateMaterial(Brushes.Green, 0.6, 100, 255));
  Sphere(-6, -4, 0, 2, MaterialHelper.CreateMaterial(Brushes.Green, 0.8, 100, 0));
  
  Sphere(6, 0, 0, 2, MaterialHelper.CreateMaterial(Brushes.Green, 0.5, 100, 255));
  Sphere(2, 0, 0, 2, MaterialHelper.CreateMaterial(Brushes.Green, 0.5, 70, 255));
  Sphere(-2, 0, 0, 2, MaterialHelper.CreateMaterial(Brushes.Green, 0.5, 40, 255));
  Sphere(-6, 0, 0, 2, MaterialHelper.CreateMaterial(Brushes.Green, 0.5, 20, 255));
  
  Sphere(6, 4, 0, 2, MaterialHelper.CreateMaterial(Brushes.Green, Brushes.Gray, nil, 1));
  //Sphere(2,4,0,2,MaterialHelper.CreateMaterial((Brushes.Green,new SolidColorBrush(RGB(0,64,0)),new SolidColorBrush(Rgb(128, 128, 128)), 100));
  Sphere(-2, 4, 0, 2, MaterialHelper.CreateMaterial(Brushes.Green, 0.5, 40, 255));
  //Cube(-6,4,0,4,Materials.Rainbow);
  //var g := hvp.Children[1] as DefaultLights;
end;

procedure Proba := Invoke(ProbaP);

procedure ProbaP2;
begin
  //var c := new CubeVisual3D();
  //var c := new IcosahedronVisual3D();
  //c.Length := 1;
  //c.Material := Colors.Green;
  //hvp.Children.Add(c);
  //var ex := new HelixToolkit.Wpf.XamlExporter();
  //ex.Export(c,new System.IO.FileStream('cube.xaml',System.IO.FileMode.Create));
  
  //XamlWriter.Save(c,new System.IO.StreamWriter('www1.xaml'));
  //var c1 := XamlReader.Load(new System.IO.FileStream('cube.xaml',System.IO.FileMode.Open)) as CubeVisual3D;
  //hvp.Children.Add(c1);
  
  {var off := new offreader(nil);
  var s := System.IO.File.OpenRead('boxcube.off');
  off.Load(s);
  
  var m1 := new MeshVisual3D();
  m1.FaceMaterial := Colors.Green;
  m1.EdgeDiameter := 0;
  m1.VertexRadius := 0;
  m1.Mesh := off.CreateMesh;
  hvp.Children.Add(m1);}
  
  {var ex := new ExtrudedVisual3D();
  ex.BackMaterial := Colors.Green;
  ex.Diameters := new DoubleCollection(Arr(1.0,1.5,1.2));
  ex.Path := new Point3DCollection(Arr(P3D(0,0,0),P3D(0,1,0),P3D(0,1,1),P3D(1,1,1)));
  hvp.Children.Add(ex);}
  
  var m := new SphereVisual3D();
  m.Radius := 0.5;
  hvp.Children.Add(m);
  
  var t :=  new TranslateManipulator();
  t.Color := Colors.Green;
  //t.Offset := v3D(2,3,4);
  t.Length := 2;
  t.Diameter := 0.15;
  t.Direction := V3D(1, 2, 0);
  t.Value := 5;
  
  var b := new System.Windows.Data.Binding('Transform');
  b.Source := m;
  
  var b1 := new System.Windows.Data.Binding('Transform');
  b1.Source := m;
  
  System.Windows.Data.BindingOperations.SetBinding(t, Manipulator.TargetTransformProperty, b);
  System.Windows.Data.BindingOperations.SetBinding(t, Manipulator.TransformProperty, b);  
  
  //t.Bind(m);
  hvp.Children.Add(t);
  
  {var l := Lst(P3D(0,1,0),P3D(1,0,0),P3D(0,-1,0),P3D(-1,0,0),P3D(0,1,0));
  
  var l1 := CanonicalSplineHelper.CreateSpline(l,0.5);
  Polyline3D(l1);}
  
end;

procedure Proba2 := Invoke(ProbaP2);

procedure ProbaP3(x,y,z: real);
begin
  hvp.CameraController.AddMoveForce(x,y,z);
end;

procedure Proba3(x,y,z: real) := Invoke(ProbaP3,x,y,z);

type
  My = class(ParametricSurface3D)
  public 
    function Evaluate(u: real; v: real; var textureCoord: System.Windows.Point): Point3D; override;
    begin
      u -= 0.5;
      v -= 0.5;
      u *= 3;
      v *= 3;
      textureCoord := new Point(u, 2 * v);
      Result := P3D(u, v, 0.2 * u * u + sin(u * v) + 2);
    end;
  end;
  
  My13D = class(MeshElement3D)
    public function Tessellate(): MeshGeometry3D; override;
    begin
      var tm := new MeshBuilder(false, false);
      tm.AddRevolvedGeometry(Arr(Pnt(0, 0), Pnt(0, 1), Pnt(0.3, 1), Pnt(0.5, 0.3), Pnt(2, 1), Pnt(3, 0)), nil, Origin, OrtZ, 80);
      Result := tm.ToMesh(false);
    end;
  end;

type
  AnyT = class(ObjectWithMaterial3D)
    constructor(x, y, z: real; c: GColor);
    begin
      {var a := new ExtrudedVisual3D;
      a.Path := new Point3DCollection(Arr(P3D(1,0,-0.5),P3D(1,0,0.5)));
      a.Section := new PointCollection(Arr(Pnt(0,0),Pnt(0.5,0),Pnt(0,0.5)));
      a.IsSectionClosed := True;}
      
      //var a := new TerrainVisual3D;
      //a.Content := (new SphereVisual3D()).Model;
      //a.Text := 'PascalABC';
      //var a := new LinesVisual3D;
      {a.Thickness := 1.99;
      a.Points := Arr(P3D(0, 0, 0), P3D(3, 0, 0), P3D(3, 0, 0), P3D(3, 3, 0), P3D(3, 3, 0), P3D(3, 3, 3));
      a.Color := c;}
      
      {var a := new My13D;
      a.Material := c;}
      
      
      {var aa := 1;
      var b := 80;
      
      var q := Partition(0,2*Pi*20,360*20*10).Select(t->P3D(5*cos(1*t),5*sin(1*t),t/5));
      var q1 := q.Interleave(q.Skip(1));
      
      //a.Points := Lst(P3D(0,0,0),P3D(4,4,2),p3D(4,4,2),p3D(2,8,-1));
      a.Points := Lst(q1);
      a.Color := Colors.Blue;
      
      a.Thickness := 1.5;}
      
      {var a := new HelixToolkit.Wpf.PieSliceVisual3D;
      a.StartAngle := 0;
      a.EndAngle := 360;
      a.ThetaDiv := 60;}
      
      var a := new HelixToolkit.Wpf.TubeVisual3D;
      var p := new Point3DCollection(Arr(P3D(0,0,0),P3D(2,0,0),P3D(2,2,0),P3D(2,2,2)));
      a.Diameter := 0.3;
      a.Path := p;
      
      {var a := new LegoVisual3D();
      a.Rows := 1;
      a.Columns := 2;
      a.Height := 3;
      //a.Divisions := 100;
      a.Fill := Brushes.Blue;}
      
      CreateBase0(a, x, y, z);
    end;
  end;

function MyH(x, y, z, Length: real; c: Color): MyAnyT := Inv(()->MyAnyT.Create(x, y, z, Length, c));

function MyH(x, y, z, Length: real; c: Material): MyAnyT := Inv(()->MyAnyT.Create(x, y, z, Length, c));

function Any(x, y, z: real; c: Color): ObjectWithMaterial3D := Inv(()->AnyT.Create(x, y, z, c));

// Сервисные функции и классы

var LastUpdatedTime := new System.TimeSpan(0); 

procedure RenderFrame(o: Object; e: System.EventArgs);
begin
  if OnDrawFrame<>nil then
  begin
    var e1 := RenderingEventArgs(e).RenderingTime;
    if LastUpdatedTime.Ticks = integer.MinValue then // первый раз
      LastUpdatedTime := e1;
    var dt := e1 - LastUpdatedTime;
    if LastUpdatedTime.TotalMilliseconds<>0 then 
      if OnDrawFrame<>nil then
        OnDrawFrame(dt.Milliseconds/1000);
    LastUpdatedTime := e1;  
  end;  
end;

procedure BeginFrameBasedAnimationTime(Draw: procedure(dt: real));
begin
  OnDrawFrame := Draw;
end;

procedure EndFrameBasedAnimation;
begin
  OnDrawFrame := nil;
end;  

var _SystemMouseEventsEnabled := True;

procedure SystemMouseEventsOff;
begin
  _SystemMouseEventsEnabled := False;
end;

procedure SystemMouseEventsOn;
begin
  _SystemMouseEventsEnabled := True;
end;



type
  Graph3DWindow = class(GMainWindow)
  public 
    procedure InitMainGraphControl; override;
    begin
      var g := Content as DockPanel;
      hvp := new HelixViewport3D();
      g.Children.Add(hvp);
      
      hvp.ZoomExtentsWhenLoaded := True;
      hvp.ShowCoordinateSystem := True;
      
      hvp.Children.Add(new DefaultLights);
      
      var mv := new ModelVisual3D;
      LightsGroup := new Model3DGroup;
      mv.Content := LightsGroup;
      hvp.Children.Add(mv);
      
      gvl := new GridLinesVisual3D();
      gvl.Width := 12;
      gvl.Length := 12;
      gvl.Normal := OrtZ;
      gvl.MinorDistance := 1;
      gvl.MajorDistance := 1;
      gvl.Thickness := 0.02;
      hvp.Children.Add(gvl);
    end;
    
    procedure InitWindowProperties; override;
    begin
      (Width, Height) := (800, 600);
      Title := '3D графика';
      WindowStartupLocation := System.Windows.WindowStartupLocation.CenterScreen;
    end;
    
    procedure InitGlobals; override;
    begin
      Window := new WindowType;
      Camera := new CameraType;
      Lights := new LightsType;
      GridLines := new GridLinesType;
      View3D := new View3DType;
      
      NameScope.SetNameScope(Self, new NameScope());
    end;
    
    /// --- SystemKeyEvents
    procedure SystemOnKeyDown(sender: Object; e: System.Windows.Input.KeyEventArgs);
    begin
      if (e.Key = Key.F4) and (e.KeyboardDevice.Modifiers = ModifierKeys.Control) then
        Close;
      if Graph3D.OnKeyDown <> nil then
        Graph3D.OnKeyDown(e.Key);
      e.Handled := True;
    end;
    
    procedure SystemOnKeyUp(sender: Object; e: System.Windows.Input.KeyEventArgs);
    begin
      if Graph3D.OnKeyUp <> nil then
        Graph3D.OnKeyUp(e.Key);
      e.Handled := True;
    end;    
    
    procedure SystemOnKeyPress(sender: Object; e: TextCompositionEventArgs); 
    begin
      if (OnKeyPress<>nil) and (e.Text<>nil) and (e.Text.Length>0) then
        OnKeyPress(e.Text[1]);
      e.Handled := True;
    end;    
    
    /// --- SystemMouseEvents
    procedure SystemOnMouseDown(sender: Object; e: System.Windows.Input.MouseButtonEventArgs);
    begin
      var mb := 0;
      var p := e.GetPosition(hvp);
      if e.LeftButton = MouseButtonState.Pressed then
        mb := 1
      else if e.RightButton = MouseButtonState.Pressed then
        mb := 2;
      if Graph3D.OnMouseDown <> nil then  
        Graph3D.OnMouseDown(p.x, p.y, mb);
      if not _SystemMouseEventsEnabled then
        e.Handled := True;
    end;
    
    procedure SystemOnMouseUp(sender: Object; e: MouseButtonEventArgs);
    begin
      var mb := 0;
      var p := e.GetPosition(hvp);
      if e.LeftButton = MouseButtonState.Pressed then
        mb := 1
      else if e.RightButton = MouseButtonState.Pressed then
        mb := 2;
      if Graph3D.OnMouseUp <> nil then  
        Graph3D.OnMouseUp(p.x, p.y, mb);
      if not _SystemMouseEventsEnabled then
        e.Handled := True;
    end;
    
    procedure SystemOnMouseMove(sender: Object; e: MouseEventArgs);
    begin
      var mb := 0;
      var p := e.GetPosition(hvp);
      if e.LeftButton = MouseButtonState.Pressed then
        mb := 1
      else if e.RightButton = MouseButtonState.Pressed then
        mb := 2;
      if Graph3D.OnMouseMove <> nil then  
        Graph3D.OnMouseMove(p.x, p.y, mb);
      if not _SystemMouseEventsEnabled then
        e.Handled := True;
    end;
    
    procedure SystemOnMouseWheel(sender: Object; e: MouseWheelEventArgs);
    begin
      var mb := 0;
      var p := e.GetPosition(hvp);
      if e.LeftButton = MouseButtonState.Pressed then
        mb := 1
      else if e.RightButton = MouseButtonState.Pressed then
        mb := 2;
      if Graph3D.OnMouseWheel <> nil then  
        Graph3D.OnMouseWheel(p.x, p.y, mb, e.Delta);
      if not _SystemMouseEventsEnabled then
        e.Handled := True;
    end;

    
    procedure InitHandlers; override;
    begin
      hvp.PreviewMouseDown += (o, e) -> SystemOnMouseDown(o, e);  
      hvp.PreviewMouseUp += (o, e) -> SystemOnMouseUp(o, e);  
      hvp.PreviewMouseMove += (o, e) -> SystemOnMouseMove(o, e);  
      hvp.PreviewMouseWheel += (o, e) -> SystemOnMouseWheel(o, e);  
      
      hvp.PreviewKeyDown += (o, e)-> SystemOnKeyDown(o, e);
      hvp.PreviewKeyUp += (o, e)-> SystemOnKeyUp(o, e);
      hvp.PreviewTextInput += SystemOnKeyPress; // не работает
      
      hvp.Focus();
      Closed += procedure(sender, e) -> begin 
        if OnClose<>nil then 
          OnClose;
        Halt; 
      end;
      
      CompositionTarget.Rendering += RenderFrame;
    end;
  end;

var
  mre := new ManualResetEvent(false);

procedure InitApp;
begin
  app := new Application;
  
  app.Dispatcher.UnhandledException += (o, e) -> begin
    Println(e.Exception.Message); 
    if e.Exception.InnerException <> nil then
      Println(e.Exception.InnerException.Message); 
    halt; 
  end;
  
  MainWindow := new Graph3DWindow;

  mre.Set();
  
  app.Run(MainWindow);
end;

procedure InitMainThread;
begin
  var MainFormThread := new System.Threading.Thread(InitApp);
  MainFormThread.SetApartmentState(ApartmentState.STA);
  MainFormThread.Start;
  
  mre.WaitOne; // Основная программа не начнется пока не будут инициализированы все компоненты приложения
end;

var
  ///--
  __initialized := false;

var
  ///--
  __finalized := false;

procedure __InitModule;
begin
  InitMainThread;
end;

///--
procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    __InitModule;
  end;
end;

///--
procedure __FinalizeModule__;
begin
  if not __finalized then
  begin
    __finalized := true;
  end;
end;

initialization
  __InitModule;

finalization  
end. 