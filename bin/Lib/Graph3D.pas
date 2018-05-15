// Copyright (©) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
/// Модуль трёхмерной графики
unit Graph3D;

{$reference System.Xml.dll}
{$reference HelixToolkit.Wpf.dll}

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

uses HelixToolkit.Wpf;
//uses Petzold.Media3D;

type
  Key = System.Windows.Input.Key;
  Colors = System.Windows.Media.Colors;
  GColor = System.Windows.Media.Color;
  GMaterial = System.Windows.Media.Media3D.Material;
  GCamera = System.Windows.Media.Media3D.ProjectionCamera;
  GRect = System.Windows.Rect;
  CameraMode = HelixToolkit.Wpf.CameraMode;
  TupleInt3 = (integer, integer, integer);
  TupleReal3 = (real, real, real);
  Point3D = Point3D;
  Vector3D = Vector3D;
  Point = System.Windows.Point;
  Ray3D = HelixToolkit.Wpf.Ray3D;
  Line3D = class(Ray3D) end;
  Plane3D = HelixToolkit.Wpf.Plane3D;

var
  hvp: HelixViewport3D;
  LightsGroup: Model3DGroup;
  gvl: GridLinesVisual3D;
  /// Событие нажатия на кнопку мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 1, если нажата левая кнопка мыши, и 2, если нажата правая кнопка мыши
  OnMouseDown: procedure(x, y: real; mousebutton: integer);
  /// Событие отжатия кнопки мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 1, если отжата левая кнопка мыши, и 2, если отжата правая кнопка мыши
  OnMouseUp: procedure(x, y: real; mousebutton: integer);
  /// Событие перемещения мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 0, если кнопка мыши не нажата, 1, если нажата левая кнопка мыши, и 2, если нажата правая кнопка мыши
  OnMouseMove: procedure(x, y: real; mousebutton: integer);
  /// Событие нажатия клавиши
  OnKeyDown: procedure(k: Key);
  /// Событие отжатия клавиши
  OnKeyUp: procedure(k: Key);

function RGB(r, g, b: byte) := Color.Fromrgb(r, g, b);

function ARGB(a, r, g, b: byte) := Color.FromArgb(a, r, g, b);

function P3D(x, y, z: real) := new Point3D(x, y, z);

function V3D(x, y, z: real) := new Vector3D(x, y, z);

function Sz3D(x, y, z: real) := new Size3D(x, y, z);

function Pnt(x, y: real) := new Point(x, y);

function Rect(x, y, w, h: real) := new System.Windows.Rect(x, y, w, h);

function operator*(p: Point3D; r: real): Point3D; extensionmethod := p.Multiply(r);

function operator*(r: real; p: Point3D): Point3D; extensionmethod := p.Multiply(r);

function operator+(p1, p2: Point3D): Point3D; extensionmethod := p3d(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);

const
  OrtX = V3D(1, 0, 0);
  OrtY = V3D(0, 1, 0);
  OrtZ = V3D(0, 0, 1);
  Origin: Point3D = P3D(0, 0, 0);
  EmptyColor = ARGB(0, 0, 0, 0);

function ChangeOpacity(Self: GColor; value: integer); extensionmethod := ARGB(value, Self.R, Self.G, Self.B);

function MoveX(Self: Point3D; dx: real); extensionmethod := P3D(Self.x + dx, Self.y, Self.z);

function MoveY(Self: Point3D; dy: real); extensionmethod := P3D(Self.x, Self.y + dy, Self.z);

function MoveZ(Self: Point3D; dz: real); extensionmethod := P3D(Self.x, Self.y, Self.z + dz);

function Move(Self: Point3D; dx, dy, dz: real); extensionmethod := P3D(Self.x + dx, Self.y + dy, Self.z + dz);

function operator implicit(t: TupleInt3): Point3D; extensionmethod := new Point3D(t[0], t[1], t[2]);

function operator implicit(t: TupleReal3): Point3D; extensionmethod := new Point3D(t[0], t[1], t[2]);

function operator implicit(ar: array of TupleInt3): Point3DCollection; extensionmethod := new Point3DCollection(ar.Select(t -> new Point3D(t[0], t[1], t[2])));

function operator implicit(ar: array of Point3D): Point3DCollection; extensionmethod := new Point3DCollection(ar);

function operator implicit(ar: List<Point3D>): Point3DCollection; extensionmethod := new Point3DCollection(ar);

function RandomColor := RGB(Random(256), Random(256), Random(256));

function GrayColor(b: byte) := RGB(b, b, b);

function RandomSolidBrush := new SolidColorBrush(RandomColor);

function wplus := SystemParameters.WindowResizeBorderThickness.Left + SystemParameters.WindowResizeBorderThickness.Right;

function hplus := SystemParameters.WindowCaptionHeight + SystemParameters.WindowResizeBorderThickness.Top + SystemParameters.WindowResizeBorderThickness.Bottom;

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
      Result := new DiffuseMaterial(b);
    end;  
  end;
  DEMHelper = auto class
    c: Color;
    function Diffuse := new DiffuseMaterial(new SolidColorBrush(c));
    function Emissive := new EmissiveMaterial(new SolidColorBrush(c));
  end;
  SpMHelper = auto class
    c: Color;
    specularpower: real;
    function SpecularMaterial := new System.Windows.Media.Media3D.SpecularMaterial(new SolidColorBrush(c), specularpower);
  end;

function ImageMaterial(fname: string; M: real := 1; N: real := 1): Material := Invoke&<Material>(IMHelper.Create(fname, M, N).ImageMaterial);

function DiffuseMaterial(c: Color): Material := Invoke&<Material>(DEMHelper.Create(c).Diffuse);

function SpecularMaterial(specularBrightness: byte; specularpower: real := 100): Material := Invoke&<Material>(SpMHelper.Create(RGB(specularBrightness, specularBrightness, specularBrightness), specularpower).SpecularMaterial);

function SpecularMaterial(c: Color; specularpower: real := 100): Material := Invoke&<Material>(SpMHelper.Create(c, specularpower).SpecularMaterial);

function EmissiveMaterial(c: Color): Material := Invoke&<Material>(DEMHelper.Create(c).Emissive);

function RainbowMaterial: Material := Materials.Rainbow;

type
  ///!#
  Materials = class
    class function Diffuse(c: Color) := DiffuseMaterial(c);
    class function Specular(specularBrightness: byte := 255; specularpower: real := 100) := SpecularMaterial(specularBrightness, specularpower);
    class function Specular(c: Color; specularpower: real := 100) := SpecularMaterial(c, specularpower);
    class function Emissive(c: Color) := EmissiveMaterial(c);
    class function Rainbow := RainbowMaterial;
  end;
  
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


function operator+(a, b: Material): Material; extensionmethod := Invoke&<Material>(GMHelper.Create(a, b).GroupMaterial);

type
  ///!#
  View3DT = class
  private
    procedure SetSCSP(v: boolean) := hvp.ShowCoordinateSystem := v;
    procedure SetSCS(v: boolean) := Invoke(SetSCSP, v);
    function GetSCS: boolean := InvokeBoolean(()->hvp.ShowCoordinateSystem);
    
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
    property ShowCoordinateSystem: boolean read GetSCS write SetSCS;
    property ShowGridLines: boolean read GetSGL write SetSGL;
    property ShowCameraInfo: boolean read GetSCI write SetSCI;
    property ShowViewCube: boolean read GetSVC write SetSVC;
    property Title: string read GetT write SetT;
    property SubTitle: string read GetST write SetST;
    property CameraMode: HelixToolkit.Wpf.CameraMode read GetCM write SetCM;
    property BackgroundColor: GColor read GetBC write SetBC;
    
    procedure Save(fname: string) := Invoke(ExportP, fname);
  end;
  
  ///!#
  CameraType = class
  private 
    function Cam: GCamera := hvp.Camera;
    procedure SetPP(p: Point3D);
    begin
      Cam.Position := p; 
      //Cam.Ani
      {if p<>(0,0,0) then
      Cam.LookDirection := Cam.Position.Multiply(-1).ToVector3D
      else Cam.LookDirection := V3D(1,0,0);}
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
  public 
    property Position: Point3D read GetP write SetP;
    property LookDirection: Vector3D read GetLD write SetLD;
    property UpDirection: Vector3D read GetUD write SetUD;
    property Distanse: real read GetD write SetD;
  end;
  
  ///!#
  LightsType = class
  private 
    function GetC: integer := Invoke&<integer>(()->LightsGroup.Children.Count);
    //procedure SetLDP(v: Vector3D) := Cam.LookDirection := v;
    //procedure SetLD(v: Vector3D) := Invoke(SetLDP, v);
    //function GetLD: Vector3D := Invoke&<Vector3D>(()->Cam.LookDirection);
  public 
    property Count: integer read GetC;
    procedure AddDirectionalLight(c: Color; v: Vector3D) := Invoke(()->LightsGroup.Children.Add(new DirectionalLight(c, v)));
    procedure AddSpotLight(c: Color; p: Point3D; v: Vector3D; outerconeangle, innerconeangle: real) := Invoke(()->LightsGroup.Children.Add(new SpotLight(c, p, v, outerconeangle, innerconeangle)));
    procedure AddPointLight(c: Color; p: Point3D) := Invoke(()->LightsGroup.Children.Add(new PointLight(c, p)));
    procedure RemoveLight(i: integer) := Invoke(()->LightsGroup.Children.RemoveAt(i));
    procedure Proba();
    begin
      var p := new PointLight(Colors.Gray, P3D(2, 2, 2));
      p.Color := Colors.Gray;
      p.Position := P3D(3, 3, 3);
    end;
  end;
  
  ///!#
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
    property Width: real read GetW write SetW;
    property Length: real read GetL write SetL;
    property Normal: Vector3D read GetN write SetN;
    property MajorDistance: real read GetMj write SetMj;
    property MinorDistance: real read GetMn write SetMn;
  end;

var
  View3D: View3DT;
  Window: WindowType;
  Camera: CameraType;
  Lights: LightsType;
  GridLines: GridLinesType;

function operator implicit(c: GColor): GMaterial; extensionmethod := Materialhelper.CreateMaterial(c);

type
  MyAnimation = class;
  ObjectWithChildren3D = class;
  
  ///!#
  Object3D = class(DependencyObject)
  private 
    model: Visual3D;
    Parent: ObjectWithChildren3D;
    transfgroup := new Transform3DGroup; 
    //rotatetransform := new MatrixTransform3D;
    rotatetransform_anim := new RotateTransform3D;
    scaletransform := new ScaleTransform3D;
    transltransform: TranslateTransform3D;

    procedure AddToObject3DList;
    procedure DeleteFromObject3DList;
    
    procedure CreateBase0(m: Visual3D; x, y, z: real);
    begin
      model := m;
      transltransform := new TranslateTransform3D(x, y, z);
      transfgroup.Children.Add(new MatrixTransform3D); // ответственен за поворот. Не храним в отдельной переменной т.к. при повороте меняется сам объект, а не поля объекта!!!
      //transfgroup.Children.Add(rotatetransform);
      transfgroup.Children.Add(rotatetransform_anim);
      rotatetransform_anim.Rotation := new AxisAngleRotation3D();
      transfgroup.Children.Add(scaletransform); 
      transfgroup.Children.Add(transltransform);
      model.Transform := transfgroup;
      hvp.Children.Add(model);
      AddToObject3DList;
    end;
    
    procedure SetX(xx: real) := Invoke(()->begin transltransform.OffsetX := xx; end); 
    function GetX: real := InvokeReal(()->transltransform.OffsetX);
    procedure SetY(yy: real) := Invoke(()->begin transltransform.OffsetY := yy; end);
    function GetY: real := InvokeReal(()->transltransform.OffsetY);
    procedure SetZ(zz: real) := Invoke(()->begin transltransform.OffsetZ := zz; end);
    function GetZ: real := InvokeReal(()->transltransform.OffsetZ);
    function GetPos: Point3D := Invoke&<Point3D>(()->P3D(transltransform.OffsetX, transltransform.OffsetY, transltransform.OffsetZ));
    
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
      (Result.model.Transform as Transform3DGroup).Children[0] := (model.Transform as Transform3DGroup).Children[0].Clone;
      //(Result.model.Transform as Transform3DGroup).Children[1] := (model.Transform as Transform3DGroup).Children[1].Clone;
      //(Result.model.Transform as Transform3DGroup).Children[2] := (model.Transform as Transform3DGroup).Children[2].Clone;
      //(Result.model.Transform as Transform3DGroup).Children[3] := (model.Transform as Transform3DGroup).Children[3].Clone; //- почему-то это не нужно!!! с ним не работает!
    end;
  
  public 
    constructor(model: Visual3D);
    begin
      CreateBase0(model, 0, 0, 0);
    end;
    
    property X: real read GetX write SetX;
    property Y: real read GetY write SetY;
    property Z: real read GetZ write SetZ;
    
    function MoveTo(xx, yy, zz: real): Object3D := 
    Invoke&<Object3D>(()->begin
      transltransform.OffsetX := xx;
      transltransform.OffsetY := yy;
      transltransform.OffsetZ := zz;
      Result := Self;
    end);
    function MoveTo(p: Point3D) := MoveTo(p.X, p.y, p.z);
    function MoveOn(dx, dy, dz: real) := MoveTo(x + dx, y + dy, z + dz);
    function MoveOn(v: Vector3D) := MoveOn(v.X, v.Y, v.Z);
    function MoveOnX(dx: real) := MoveOn(dx, 0, 0);
    function MoveOnY(dy: real) := MoveOn(0, dy, 0);
    function MoveOnZ(dz: real) := MoveOn(0, 0, dz);
    property Color: GColor read GetColor write SetColor; virtual;
  private
    procedure MoveToProp(p: Point3D) := MoveTo(p);
  
  ///---------------------------------- Эксперимент - Position для анимации --------------
  (*public
    class PositionProperty: DependencyProperty;
  private    
    procedure SetPositionT(value: Point3D);
    begin
      MoveTo(value.X,value.Y,value.Z);
      SetValue(PositionProperty, value);
    end;
    procedure SetPosition(value: Point3D) := Invoke(SetPositionT,value);
    function GetPosition: Point3D := GetPos;
  public
    class constructor;
    begin
      PositionProperty := DependencyProperty.Register('Position1', typeof (Point3D), typeof (Object3D), nil);
    end;
  
    property Position1: Point3D read GetPosition write SetPosition;  *)
  ///---------------------------------- Конец эксперимента
  
  public 
    property Position: Point3D read GetPos write MoveToProp;
    
    function Scale(f: real): Object3D := 
    Invoke&<Object3D>(()->begin
      scaletransform.ScaleX *= f;
      scaletransform.ScaleY *= f;
      scaletransform.ScaleZ *= f;
      Result := Self;
    end);
    function ScaleX(f: real): Object3D := 
    Invoke&<Object3D>(()->begin
      scaletransform.ScaleX *= f;
      Result := Self;
    end);
    function ScaleY(f: real): Object3D := 
    Invoke&<Object3D>(()->begin
      scaletransform.ScaleY *= f;
      Result := Self;
    end);
    function ScaleZ(f: real): Object3D := 
    Invoke&<Object3D>(()->begin
      scaletransform.ScaleZ *= f;
      Result := Self;
    end);
    /// Поворот на угол angle вокруг оси axis
    function Rotate(axis: Vector3D; angle: real): Object3D := 
    Invoke&<Object3D>(()->begin
      var m := transfgroup.Children[0].Value; 
      m.Rotate(new Quaternion(axis, angle));
      transfgroup.Children[0] := new MatrixTransform3D(m);
      Result := Self;
    end);
    /// Поворот на угол angle вокруг оси axis относительно точки center
    function RotateAt(axis: Vector3D; angle: real; center: Point3D): Object3D :=
    Invoke&<Object3D>(()->begin
      var m := transfgroup.Children[0].Value;    
      m.RotateAt(new Quaternion(axis, angle), center);
      transfgroup.Children[0] := new MatrixTransform3D(m);
      Result := Self;
    end);
    function AnimMoveTo(x, y, z: real; seconds: real := 1; Completed: procedure := nil): MyAnimation;
    function AnimMoveTo(p: Point3D; seconds: real := 1; Completed: procedure := nil) := AnimMoveTo(p.x, p.y, p.z, seconds, Completed);
    function AnimMoveTrajectory(a: sequence of Point3D; seconds: real := 1; Completed: procedure := nil): MyAnimation;
    //function AnimMoveToP3D(x,y,z: real; seconds: real := 1): MyAnimation; - не получилось! Свойство не анимируется!
    function AnimMoveOn(dx, dy, dz: real; seconds: real := 1; Completed: procedure := nil): MyAnimation;
    function AnimMoveOn(v: Vector3D; seconds: real := 1; Completed: procedure := nil) := AnimMoveOn(v.x, v.y, v.z, seconds, Completed);
    function AnimMoveOnX(dx: real; seconds: real := 1; Completed: procedure := nil) := AnimMoveOn(dx, 0, 0, seconds, Completed);
    function AnimMoveOnY(dy: real; seconds: real := 1; Completed: procedure := nil) := AnimMoveOn(0, dy, 0, seconds, Completed);
    function AnimMoveOnZ(dz: real; seconds: real := 1; Completed: procedure := nil) := AnimMoveOn(0, 0, dz, seconds, Completed);
    function AnimScale(sc: real; seconds: real := 1; Completed: procedure := nil): MyAnimation;
    function AnimScaleX(sc: real; seconds: real := 1; Completed: procedure := nil): MyAnimation;
    function AnimScaleY(sc: real; seconds: real := 1; Completed: procedure := nil): MyAnimation;
    function AnimScaleZ(sc: real; seconds: real := 1; Completed: procedure := nil): MyAnimation;
    function AnimRotate(vx, vy, vz, angle: real; seconds: real := 1; Completed: procedure := nil): MyAnimation;
    function AnimRotate(v: Vector3D; angle: real; seconds: real := 1; Completed: procedure := nil) := AnimRotate(v.x, v.y, v.z, angle, seconds, Completed);
    function AnimRotateAt(axis: Vector3D; angle: real; center: Point3D; seconds: real := 1; Completed: procedure := nil): MyAnimation;
    function Clone: Object3D := Invoke&<Object3D>(CloneT);
    
    procedure SaveP(fname: string);
    begin
      var f := new System.IO.StreamWriter(fname);
      XamlWriter.Save(Model, f);
      f.Close()
    end;
    
    procedure Save(fname: string); virtual := Invoke(SaveP, fname); // надо её сделать виртуальной!
    class function Load(fname: string): Object3D := Invoke&<Object3D>(()->begin
      var m := XamlReader.Load(new System.IO.FileStream(fname, System.IO.FileMode.Open)) as Visual3D;
      Result := new Object3D(m);
    end);
    procedure Destroy(); virtual;
    begin
      DeleteFromObject3DList;
    end;
  end;
  
  ObjectWithChildren3D = class(Object3D) // model is ModelVisual3D
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
  
  public  
    procedure AddChild(obj: Object3D) := Invoke(AddT, obj);
    property Items[i: integer]: Object3D read GetObj; default;
    
    function Count: integer := Invoke&<integer>(CountT);
    
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
    
    procedure Destroy; override;
    begin
      inherited Destroy;
      Invoke(DestroyP);
    end;
  end;
  
  ObjectWithMaterial3D = class(ObjectWithChildren3D) // model is MeshElement3D
  private 
    procedure CreateBase(m: MeshElement3D; x, y, z: real; mat: GMaterial);
    begin
      CreateBase0(m, x, y, z);
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
    property Color: GColor read GetColor write SetColor; override;
    property Material: GMaterial read GetMaterial write SetMaterial;
    property BackMaterial: GMaterial read GetBMaterial write SetBMaterial;
    property Visible: boolean read GetV write SetV;
  end;
  
  GroupT = class(ObjectWithChildren3D)
  protected 
    function CreateObject: Object3D; override := new GroupT(X, Y, Z);
  public 
    constructor(x, y, z: real) := CreateBase0(new ModelVisual3D, x, y, z);
    
    constructor(x, y, z: real; lst: sequence of Object3D);
    begin
      CreateBase0(new ModelVisual3D, x, y, z);
      foreach var xx in lst do
        AddChild(xx);
    end;
    
    function Clone := (inherited Clone) as GroupT;
  end;
  
  //------------------------------ Animation -----------------------------------
  
  MyAnimation = class
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
    
    procedure InitAnimWait(sb: StoryBoard; waittime: real); virtual;
    begin
    end;
  
  private 
    class function AddDoubleAnimRemainderHelper(d: DoubleAnimationBase; sb: StoryBoard; seconds: real; ttname: string; prop: Object; waittime: real := 0.0): DoubleAnimationBase;
    begin
      d.Duration := new System.Windows.Duration(System.TimeSpan.FromSeconds(seconds));
      d.BeginTime := System.TimeSpan.FromSeconds(waittime);
      StoryBoard.SetTargetName(d, ttname);
      StoryBoard.SetTargetProperty(d, new PropertyPath(prop));
      sb.Children.Add(d);
      Result := d;
    end;
  
  protected 
    sb: StoryBoard;
    
    class function AddDoubleAnimByName(sb: StoryBoard; toValue, seconds: real; ttname: string; prop: Object; waittime: real := 0.0): DoubleAnimationBase;
    begin
      var d := new DoubleAnimation();
      d.To := toValue;
      Result := AddDoubleAnimRemainderHelper(d, sb, seconds, ttname, prop, waittime);
    end;
    
    class function AddDoubleAnimOnByName(sb: StoryBoard; toValue, seconds: real; ttname: string; prop: Object; waittime: real := 0.0): DoubleAnimationBase;
    begin
      var d := new DoubleAnimation();
      d.By := toValue;
      Result := AddDoubleAnimRemainderHelper(d, sb, seconds, ttname, prop, waittime);
    end;
    
    class function AddDoubleAnimByNameUsingKeyframes(sb: StoryBoard; a: sequence of real; seconds: real; ttname: string; prop: Object; waittime: real := 0.0): DoubleAnimationBase;
    begin
      var d := new DoubleAnimationUsingKeyframes;
      d.KeyFrames := new DoubleKeyFrameCollection;
      foreach var x in a do
        d.KeyFrames.Add(new LinearDoubleKeyFrame(x)); // не указываем keytime - надеемся, что по секунде
      Result := AddDoubleAnimRemainderHelper(d, sb, seconds, ttname, prop, waittime);
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
    
    procedure InitAnim(sb: StoryBoard); virtual := InitAnimWait(sb, 0);
  private 
    function CreateStoryboard: StoryBoard;
    begin
      var sb := new StoryBoard;
      var storyboardName := 's' + sb.GetHashCode;
      MainWindow.Resources.Add(storyboardName, sb);
      var an := AnimationCompleted;
      sb.Completed += (o, e) -> begin
        MainWindow.Resources.Remove(storyboardName);
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
    
    function WhenCompleted(act: procedure): MyAnimation;
    begin
      Self.AnimationCompleted := act;
      Result := Self;
    end;
  
  private 
    procedure BeginT;
    begin
      sb := CreateStoryboard;
      InitAnim(sb);
      
      ApplyAllDecorators;
      sb.Completed += procedure (o, e) -> begin sb.Children.Clear; end;
      sb.Begin;
    end;
    
    procedure RemoveT := begin
      sb.Pause;
      sb := new Storyboard;
    end;
    procedure ChangeT(a: MyAnimation);
    begin
      sb := CreateStoryboard;
      foreach var d in a.sb.Children do
        sb.Children.Add(d);
    end;
  
  public 
    procedure &Begin; virtual := Invoke(BeginT);
    procedure Remove := Invoke(RemoveT);
    procedure Change(a: MyAnimation) := Invoke(ChangeT, a);
    procedure Pause := if sb <> nil then sb.Pause;
    procedure Resume := if sb <> nil then sb.Resume;
    
    function Duration: real; virtual := seconds;
    function &Then(second: MyAnimation): MyAnimation;
    function Forever: MyAnimation; virtual := Self;
    function AutoReverse: MyAnimation; virtual := Self;
    function AccelerationRatio(acceleration: real; deceleration: real := 0): MyAnimation; virtual := Self;
  end;
  
  EmptyAnimation = class(MyAnimation)
  public 
    constructor(wait: real);
    begin
      Self.Seconds := wait;
    end;
    
    procedure InitAnim(sb: StoryBoard); virtual := InitAnimWait(sb, Seconds);
  end;
  
  
  Double1AnimationBase = class(MyAnimation)
  private 
    v: real;
    da: DoubleAnimationBase;
  public 
    constructor(e: Object3D; sec: real; value: real; Completed: procedure := nil);
    begin
      inherited Create(e, sec, Completed);
      v := value;
    end;
    
    function AutoReverse: MyAnimation; override;
    begin
      ApplyDecorators.Add(()-> begin
        da.AutoReverse := True;
      end);  
      Result := Self;
    end;
    
    function Forever: MyAnimation; override;
    begin
      ApplyDecorators.Add(()-> begin
        da.RepeatBehavior := RepeatBehavior.Forever;
      end);  
      Result := Self;
    end;
    
    function AccelerationRatio(acceleration: real; deceleration: real := 0): MyAnimation; override;
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
  
  Double3AnimationBase = class(MyAnimation)
  private 
    x, y, z: real;
    dax, day, daz: DoubleAnimationBase;
  public 
    constructor(e: Object3D; sec: real; xx, yy, zz: real; Completed: procedure := nil);
    begin
      inherited Create(e, sec, Completed);
      (x, y, z) := (xx, yy, zz);
    end;
    
    function AutoReverse: MyAnimation; override;
    begin
      ApplyDecorators.Add(()-> begin
        dax.AutoReverse := True;
        day.AutoReverse := True;
        daz.AutoReverse := True;
      end);  
      Result := Self;
    end;
    
    function Forever: MyAnimation; override;
    begin
      ApplyDecorators.Add(()-> begin
        dax.RepeatBehavior := RepeatBehavior.Forever;
        day.RepeatBehavior := RepeatBehavior.Forever;
        daz.RepeatBehavior := RepeatBehavior.Forever;
      end);  
      Result := Self;
    end;
    
    function AccelerationRatio(acceleration: real; deceleration: real := 0): MyAnimation; override;
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
  
  OffsetAnimation = class(Double3AnimationBase)
  private 
    procedure InitAnimWait(sb: StoryBoard; waittime: real); override;
    begin
      var el := Element.transltransform;
      var ttname := 't' + el.GetHashCode;
      if not RegisterName(sb, el, ttname) then;
      dax := AddDoubleAnimByName(sb, x, seconds, ttname, TranslateTransform3D.OffsetXProperty, waittime);
      day := AddDoubleAnimByName(sb, y, seconds, ttname, TranslateTransform3D.OffsetYProperty, waittime);
      daz := AddDoubleAnimByName(sb, z, seconds, ttname, TranslateTransform3D.OffsetZProperty, waittime);
      daz.Completed += (o, e) -> if Completed <> nil then Completed(); // только на последнюю! Надо выполнить один раз!
    end;
  end;
  
  OffsetAnimationOn = class(Double3AnimationBase)
  private 
    procedure InitAnimWait(sb: StoryBoard; waittime: real); override;
    begin
      var el := Element.transltransform;
      var ttname := 't' + el.GetHashCode;
      if not RegisterName(sb, el, ttname) then;
      dax := AddDoubleAnimOnByName(sb, x, seconds, ttname, TranslateTransform3D.OffsetXProperty, waittime);
      day := AddDoubleAnimOnByName(sb, y, seconds, ttname, TranslateTransform3D.OffsetYProperty, waittime);
      daz := AddDoubleAnimOnByName(sb, z, seconds, ttname, TranslateTransform3D.OffsetZProperty, waittime);
      daz.Completed += (o, e) -> if Completed <> nil then Completed(); // только на первую! Надо выполнить один раз!
    end;
  
  public 
  end;
  
  OffsetAnimationUsingKeyframes = class(Double3AnimationBase)
  private 
    a: sequence of Point3D;
    procedure InitAnimWait(sb: StoryBoard; waittime: real); override;
    begin
      var el := Element.transltransform;
      var ttname := 't' + el.GetHashCode;
      if not RegisterName(sb, el, ttname) then;
      dax := AddDoubleAnimByNameUsingKeyframes(sb, a.Select(p -> p.x), seconds, ttname, TranslateTransform3D.OffsetXProperty, waittime);
      day := AddDoubleAnimByNameUsingKeyframes(sb, a.Select(p -> p.y), seconds, ttname, TranslateTransform3D.OffsetYProperty, waittime);
      daz := AddDoubleAnimByNameUsingKeyframes(sb, a.Select(p -> p.z), seconds, ttname, TranslateTransform3D.OffsetZProperty, waittime);
    end;
  
  public 
    constructor(e: Object3D; sec: real; aa: sequence of Point3D; Completed: procedure := nil);
    begin
      inherited Create(e, sec, Completed);
      a := aa;
    end;
  end;
  
  ScaleAnimation = class(Double3AnimationBase)
  private 
    scale: real;
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      var sctransform := Element.scaletransform; 
      var ttname := 's' + sctransform.GetHashCode;
      if not RegisterName(sb, sctransform, ttname) then;
      dax := AddDoubleAnimByName(sb, scale, seconds, ttname, ScaleTransform3D.ScaleXProperty, wait);
      day := AddDoubleAnimByName(sb, scale, seconds, ttname, ScaleTransform3D.ScaleYProperty, wait);
      daz := AddDoubleAnimByName(sb, scale, seconds, ttname, ScaleTransform3D.ScaleZProperty, wait);
    end;
  
  public 
    constructor(e: Object3D; sec: real; sc: real; Completed: procedure := nil);
    begin
      inherited Create(e, sec, Completed);
      scale := sc;
    end;
  end;
  
  ScaleXAnimation = class(Double1AnimationBase)
  private 
    scale: real;
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      var sctransform := Element.scaletransform; 
      var ttname := 's' + sctransform.GetHashCode;
      if not RegisterName(sb, sctransform, ttname) then;
      da := AddDoubleAnimByName(sb, scale, seconds, ttname, ScaleTransform3D.ScaleXProperty, wait);
    end;
  
  public 
    constructor(e: Object3D; sec: real; sc: real; Completed: procedure := nil);
    begin
      inherited Create(e, sec, Completed);
      scale := sc;
    end;
  end;
  ScaleYAnimation = class(ScaleXAnimation)
  private 
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      var sctransform := Element.scaletransform; 
      var ttname := 's' + sctransform.GetHashCode;
      if not RegisterName(sb, sctransform, ttname) then;
      da := AddDoubleAnimByName(sb, scale, seconds, ttname, ScaleTransform3D.ScaleYProperty, wait);
    end;
  
  public 
  end;
  ScaleZAnimation = class(ScaleXAnimation)
  private 
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      var sctransform := Element.scaletransform; 
      var ttname := 's' + sctransform.GetHashCode;
      if not RegisterName(sb, sctransform, ttname) then;
      da := AddDoubleAnimByName(sb, scale, seconds, ttname, ScaleTransform3D.ScaleZProperty, wait);
    end;
  
  public 
  end;
  
  {RotateAnimation = class(Double1AnimationBase)
  private 
    vx, vy, vz, angle: real;
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      var rottransform := Element.rotatetransform_anim;
      var rot := rottransform.Rotation as AxisAngleRotation3D;
      var ttname := 'r' + rot.GetHashCode;
      if not RegisterName(sb, rot, ttname) then;
  
      rot.Angle := 0; //?
      rot.Axis := V3D(vx, vy, vz); //?
  
      var el: Object3D := Element;
      sb.Completed += (o, e) -> begin
        rottransform.Rotation := new AxisAngleRotation3D();
        el.Rotate(rot.Axis, angle); // переходит в основную матрицу
      end;
  
      da := AddDoubleAnimByName(sb, angle, seconds, ttname, AxisAngleRotation3D.AngleProperty, wait);
    end;
  
  public 
    constructor(e: Object3D; sec: real; vvx, vvy, vvz, a: real);
    begin
      inherited Create(e, sec);
      (vx, vy, vz, angle) := (vvx, vvy, vvz, a)
    end;
  end;}
  
  RotateAtAnimation = class(Double1AnimationBase)
  private 
    vx, vy, vz, angle: real;
    center: Point3D;
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      var rottransform := Element.rotatetransform_anim;
      rottransform.CenterX := center.x;
      rottransform.CenterY := center.y;
      rottransform.CenterZ := center.z;
      var rot := rottransform.Rotation as AxisAngleRotation3D;
      var ttname := 'r' + rot.GetHashCode;
      if not RegisterName(sb, rot, ttname) then;
      
      rot.Angle := 0; //?
      rot.Axis := V3D(vx, vy, vz); //?
      
      var el: Object3D := Element;
      sb.Completed += (o, e) -> begin
        rottransform.Rotation := new AxisAngleRotation3D();
        el.RotateAt(rot.Axis, angle, center); // переходит в основную матрицу. Проблема - оно должно переходить не после всей анимации, а после данной. Потому и ошибка!!!
      end;
      
      da := AddDoubleAnimByName(sb, angle, seconds, ttname, AxisAngleRotation3D.AngleProperty, wait);
    end;
  
  public 
    constructor(e: Object3D; sec: real; vvx, vvy, vvz, a: real; c: Point3D; Completed: procedure := nil);
    begin
      inherited Create(e, sec, Completed);
      (vx, vy, vz, angle, center) := (vvx, vvy, vvz, a, c)
    end;
  end;
  
  GroupAnimation = class(MyAnimation)
  private 
    ll: List<MyAnimation>;
    procedure ApplyAllDecorators; override;
    begin
      foreach var l in ll do
        l.ApplyAllDecorators;
    end;
    
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      foreach var l in ll do
        l.InitAnimWait(sb, wait);
    end;
  
  public 
    constructor(params l: array of MyAnimation) := ll := Lst(l);
    
    constructor(l: List<MyAnimation>) := ll := l;
    
    function Duration: real; override := ll.Select(l -> l.Duration).Max;
    {function AutoReverse: MyAnimation; override;
    begin
      ApplyDecorators.Add(()-> begin
        foreach var l in ll do
          l.AutoReverse
      end);  
      Result := Self;
    end;
    function Forever: MyAnimation; override;
    begin
      ApplyDecorators.Add(()-> begin
        foreach var l in ll do
          l.Forever
      end);  
      Result := Self;
    end;}
    function Add(b: MyAnimation): GroupAnimation;
    begin
      ll += b;
      Result := Self;
    end;
    
    class function operator +=(a: GroupAnimation; b: MyAnimation): GroupAnimation;
    begin
      a.ll += b;
      Result := a;
    end;
  end;
  
  SequenceAnimation = class(MyAnimation)
  private 
    ll: List<MyAnimation>;
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      var w := wait;
      foreach var l in ll do
      begin
        l.InitAnimWait(sb, w);
        w += l.Duration
      end;
    end;
    
    procedure ApplyAllDecorators; override;
    begin
      foreach var l in ll do
        l.ApplyAllDecorators;
    end;
  
  public 
    constructor(params l: array of MyAnimation) := ll := Lst(l);
    constructor(l: List<MyAnimation>) := ll := l;
    
    function Duration: real; override := ll.Sum(l -> l.Duration);
    
    function Add(b: MyAnimation): SequenceAnimation;
    begin
      ll += b;
      Result := Self;
    end;
    
    class function operator +=(a: SequenceAnimation; b: MyAnimation): SequenceAnimation;
    begin
      a.ll += b;
      Result := a;
    end;
  end;

function Object3D.AnimMoveTo(x, y, z, seconds: real; Completed: procedure) := new OffsetAnimation(Self, seconds, x, y, z, Completed);

function Object3D.AnimMoveTrajectory(a: sequence of Point3D; seconds: real; Completed: procedure) := new OffsetAnimationUsingKeyframes(Self, seconds, a, Completed);

function Object3D.AnimMoveOn(dx, dy, dz, seconds: real; Completed: procedure) := new OffsetAnimationOn(Self, seconds, dx, dy, dz, Completed);

function Object3D.AnimScale(sc, seconds: real; Completed: procedure) := new ScaleAnimation(Self, seconds, sc, Completed);

function Object3D.AnimScaleX(sc, seconds: real; Completed: procedure) := new ScaleXAnimation(Self, seconds, sc, Completed);

function Object3D.AnimScaleY(sc, seconds: real; Completed: procedure) := new ScaleYAnimation(Self, seconds, sc, Completed);

function Object3D.AnimScaleZ(sc, seconds: real; Completed: procedure) := new ScaleZAnimation(Self, seconds, sc, Completed);

function Object3D.AnimRotate(vx, vy, vz, angle, seconds: real; Completed: procedure) := new RotateAtAnimation(Self, seconds, vx, vy, vz, angle, P3D(0, 0, 0), Completed);

function Object3D.AnimRotateAt(axis: Vector3D; angle: real; center: Point3D; seconds: real; Completed: procedure) := new RotateAtAnimation(Self, seconds, axis.X, axis.y, axis.z, angle, center, Completed);

var
  Object3DList := new List<Object3D>;

procedure Object3D.AddToObject3DList;
begin
  Object3DList.Add(Self)
end;

procedure Object3D.DeleteFromObject3DList;
begin
  var oc := Self as ObjectWithChildren3D;
  foreach var c in oc.l do
    c.DeleteFromObject3DList;
  Object3DList.Remove(Self)
end;

type
  Animate = class
  public 
    class function Group(params l: array of MyAnimation) := new GroupAnimation(Lst(l));
    //class function &Then(first,second: MyAnimation) := new ThenAnimation(first,second);
    class function &Sequence(params l: array of MyAnimation) := new SequenceAnimation(l);
  end;

function Sec(Self: integer): real; extensionmethod := Self;

function Sec(Self: real): real; extensionmethod := Self;
// А теперь - тадам! - перегрузка + для Animate.Sequence и перегрузка * для Animate.Group
function operator+(a, b: MyAnimation): MyAnimation; extensionmethod := Animate.Sequence(a, b);

function operator*(a, b: MyAnimation): MyAnimation; extensionmethod := Animate.Group(a, b);

function MyAnimation.&Then(second: MyAnimation): MyAnimation := Self + second;

function EmptyAnim(sec: real) := EmptyAnimation.Create(sec);

//------------------------------ End Animation -------------------------------


type
  SphereT = class(ObjectWithMaterial3D)
  private 
    function Model := inherited model as SphereVisual3D;
    procedure SetRP(r: real) := Model.Radius := r;
    procedure SetR(r: real) := Invoke(SetRP, r);
    function GetR: real := InvokeReal(()->Model.Radius);
    function NewVisualObject(r: real): SphereVisual3D;
    begin
      var sph := new SphereVisual3D;
      sph.Center := Origin;
      sph.Radius := r;
      Result := sph;
    end;
  
  protected 
    function CreateObject: Object3D; override := new SphereT(X, Y, Z, Radius, Material.Clone);
  public 
    constructor := CreateBase(NewVisualObject(1), 0, 0, 0, Colors.Blue);
    constructor(x, y, z, r: real; m: Gmaterial) := CreateBase(NewVisualObject(r), x, y, z, m);
    
    property Radius: real read GetR write SetR;
    function Clone := (inherited Clone) as SphereT;
  end;
  
  EllipsoidT = class(ObjectWithMaterial3D)
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
      ell.Center := Origin;
      ell.RadiusX := rx;
      ell.RadiusY := ry;
      ell.RadiusZ := rz;
      Result := ell;
    end;
  
  protected
    function CreateObject: Object3D; override := new EllipsoidT(X, Y, Z, RadiusX, RadiusY, RadiusZ, Material.Clone);
  public 
    constructor(x, y, z, rx, ry, rz: real; m: GMaterial) := CreateBase(NewVisualObject(rx, ry, rz), x, y, z, m);
    
    property RadiusX: real read GetRX write SetRX;
    property RadiusY: real read GetRY write SetRY;
    property RadiusZ: real read GetRZ write SetRZ;
    function Clone := (inherited Clone) as EllipsoidT;
  end;
  
  CubeT = class(ObjectWithMaterial3D)
  private
    function model := inherited model as CubeVisual3D;
    procedure SetWP(r: real) := model.SideLength := r;
    procedure SetW(r: real) := Invoke(SetWP, r);
    function GetW: real := InvokeReal(()->model.SideLength);
  private 
    function NewVisualObject(w: real): CubeVisual3D;
    begin
      var bx := new CubeVisual3D;
      bx.Center := Origin;
      bx.SideLength := w;
      Result := bx;
    end;
  
  protected  
    function CreateObject: Object3D; override := new CubeT(X, Y, Z, SideLength, Material.Clone);
  public 
    constructor(x, y, z, w: real; m: GMaterial) := CreateBase(NewVisualObject(w), x, y, z, m);
    
    property SideLength: real read GetW write SetW;
    function Clone := (inherited Clone) as CubeT;
  end;
  
  BoxT = class(ObjectWithMaterial3D)
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
      bx.Center := Origin;
      (bx.Width, bx.Height, bx.Length) := (w, h, l);
      Result := bx;
    end;
  
  protected  
    function CreateObject: Object3D; override := new BoxT(X, Y, Z, Length, Width, Height, Material.Clone);
  public 
    constructor(x, y, z, l, w, h: real; m: GMaterial) := CreateBase(NewVisualObject(l, w, h), x, y, z, m);
    
    property Length: real read GetL write SetL;
    property Width: real read GetW write SetW;
    property Height: real read GetH write SetH;
    property Size: Size3D read GetSz write SetSz;
    function Clone := (inherited Clone) as BoxT;
  end;
  
  ArrowT = class(ObjectWithMaterial3D)
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
      a.Origin := Origin;
      Result := a;
    end;
  
  protected  
    function CreateObject: Object3D; override := new ArrowT(X, Y, Z, Direction.X, Direction.Y, Direction.Z, Diameter, HeadLength, Material.Clone);
  public 
    constructor(x, y, z, dx, dy, dz, d, hl: real; m: GMaterial);
    begin
      var a := NewVisualObject(dx, dy, dz, d, hl);
      CreateBase(a, x, y, z, m);
      a.Direction := V3D(dx, dy, dz);
    end;
    
    property HeadLength: real read GetL write SetL;
    property Diameter: real read GetD write SetD;
    property Direction: Vector3D read GetDir write SetDir;
    function Clone := (inherited Clone) as ArrowT;
  end;
  
  TruncatedConeT = class(ObjectWithMaterial3D)
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
      a.Origin := Origin;
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
  public 
    constructor(x, y, z, h, baser, topr: real; sides: integer; topcap: boolean; m: GMaterial);
    begin
      var a := NewVisualObject(h, baser, topr, sides, topcap);
      CreateBase(a, x, y, z, m);
    end;
    
    property Height: real read GetH write SetH;
    property BaseRadius: real read GetBR write SetBR;
    property TopRadius: real read GetTR write SetTR;
    property Topcap: boolean read GetTC write SetTC;
    function Clone := (inherited Clone) as TruncatedConeT;
  end;
  
  CylinderT = class(TruncatedConeT)
  private 
    procedure SetR(r: real);
    begin
      BaseRadius := r;
      TopRadius := r;
    end;
    
    function GetR: real := BaseRadius;
  protected  
    function CreateObject: Object3D; override := new CylinderT(X, Y, Z, Height, Radius, (model as TruncatedConeVisual3D).ThetaDiv - 1, Topcap, Material.Clone);
  public 
    constructor(x, y, z, h, r: real; ThetaDiv: integer; topcap: boolean; m: GMaterial);
    begin
      var a := NewVisualObject(h, r, r, ThetaDiv, topcap);
      CreateBase(a, x, y, z, m);
    end;
    
    function Clone := (inherited Clone) as CylinderT;
    property Radius: real read GetR write SetR;
  end;
  
  TeapotT = class(ObjectWithMaterial3D)
  private
    procedure SetVP(v: boolean) := (model as MeshElement3D).Visible := v;
    procedure SetV(v: boolean) := Invoke(SetVP, v);
    function GetV: boolean := Invoke&<boolean>(()->(model as MeshElement3D).Visible);
  protected  
    function CreateObject: Object3D; override := new TeapotT(X, Y, Z, Material.Clone);
  public 
    constructor(x, y, z: real; m: GMaterial);
    begin
      var a := new Teapot;
      CreateBase(a, x, y, z, m);
      Rotate(OrtX, 90);
    end;
    
    property Visible: boolean read GetV write SetV;
    function Clone := (inherited Clone) as TeapotT;
  end;
  
  CoordinateSystemT = class(ObjectWithChildren3D)
  private
    procedure SetALP(r: real) := (model as CoordinateSystemVisual3D).ArrowLengths := r;
    procedure SetAL(r: real) := Invoke(SetALP, r); 
    function GetAL: real := InvokeReal(()->(model as CoordinateSystemVisual3D).ArrowLengths);
    function GetD: real := InvokeReal(()->((model as CoordinateSystemVisual3D).Children[0] as ArrowVisual3D).Diameter);
  protected  
    function CreateObject: Object3D; override := new CoordinateSystemT(X, Y, Z, ArrowLengths, Diameter);
  public 
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
    
    property ArrowLengths: real read GetAL write SetAL;
    property Diameter: real read GetD;
    function Clone := (inherited Clone) as CoordinateSystemT;
  end;
  
  BillboardTextT = class(ObjectWithChildren3D)
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
  public 
    constructor(x, y, z: real; text: string; fontsize: real);
    begin
      var a := new BillboardTextVisual3D;
      CreateBase0(a, x, y, z);
      a.Position := p3D(0, 0, 0);
      a.Text := text;
      a.FontSize := fontsize;
    end;
    
    property Text: string read GetT write SetT;
    property FontSize: real read GetFS write SetFS;
    function Clone := (inherited Clone) as BillboardTextT;
  end;
  
  TextT = class(ObjectWithChildren3D)
  private 
    fontname: string;
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
    procedure SetN(fontname: string) := Invoke(SetTP, fontname); 
    function GetN: string := InvokeString(()->fontname);
    
    procedure SetColorP(c: GColor) := model.Foreground := new SolidColorBrush(c);
    procedure SetColor(c: GColor) := Invoke(SetColorP, c); 
    function GetColor: GColor := Invoke&<GColor>(()->(model.Foreground as SolidColorBrush).Color);
  protected  
    function CreateObject: Object3D; override := new TextT(X, Y, Z, Text, Height, Name, Color);
  public 
    constructor(x, y, z: real; text: string; height: real; fontname: string; c: GColor);
    begin
      var a := new TextVisual3D;
      a.Position := p3D(0, 0, 0);
      a.Text := text;
      a.Height := height;
      //a.HorizontalAlignment := HorizontalAlignment.Left;
      Self.fontname := fontname;
      a.FontFamily := new FontFamily(fontname);
      a.Foreground := new SolidColorBrush(c);
      CreateBase0(a, x, y, z);
    end;
    
    property Text: string read GetT write SetT;
    property Height: real read GetFS write SetFS;
    property Name: string read GetN write SetN;
    property UpDirection: Vector3D read GetU write SetU;
    property Color: GColor read GetColor write SetColor; override;
    function Clone := (inherited Clone) as TextT;
  end;
  
  RectangleT = class(ObjectWithMaterial3D)
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
  public 
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
    
    property Width: real read GetW write SetW;
    property Length: real read GetL write SetL;
    property LengthDirection: Vector3D read GetLD write SetLD;
    property Normal: Vector3D read GetN write SetN;
    function Clone := (inherited Clone) as RectangleT;
  end;
  
  FileModelT = class(ObjectWithChildren3D)
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
  public 
    constructor(x, y, z: real; fname: string; mat: GMaterial);
    begin
        {model := new MeshVisual3D();
      
        var fs := System.IO.File.OpenRead(fname);
        fn := fname;
      
      	var ext := System.IO.Path.GetExtension(fname);
        ext := ext?.ToLower;
      
        var r: ModelReader;
      
        case ext of
      '.off': begin
          r := new offreader(nil);
          (model as MeshVisual3D).FaceMaterial := mat;
          (model as MeshVisual3D).EdgeDiameter := 0;
          (model as MeshVisual3D).VertexRadius := 0;
        end;
      '.3ds': r := new studioreader(nil);
      '.lwo': r := new lworeader(nil);
      '.stl': r := new stlreader(nil);
      '.obj','.objx': r := new objreader(nil);
        end;
      
        r.DefaultMaterial := mat;
        //r.DefaultMaterial := Colors.Gray;
        if ext = '.off' then
        begin
          (r as offreader).Load(fs);
          (model as MeshVisual3D).Mesh := (r as offreader).CreateMesh;
        end
        else 
        begin
          var md := r.Read(fs);
          (model as MeshVisual3D).Content := md;
        end;
      
        fs.Close;}
      
      var a := new FileModelVisual3D;
      a.DefaultMaterial := mat;
      a.Source := fname;
      CreateBase0(a, x, y, z);
    end;
    
    function Clone := (inherited Clone) as FileModelT;
  end;
  
  PipeT = class(ObjectWithMaterial3D)
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
  public 
    constructor(x, y, z, h, r, ir: real; m: GMaterial);
    begin
      var a := new PipeVisual3D;
      a.Diameter := r * 2;
      a.InnerDiameter := ir * 2;
      a.Point1 := P3D(0, 0, 0);
      a.Point2 := P3D(0, 0, h);
      CreateBase(a, x, y, z, m);
    end;
    
    property Radius: real read GetD write SetD;
    property InnerRadius: real read GetID write SetID;
    property Height: real read GetH write SetH;
    function Clone := (inherited Clone) as PipeT;
  end;

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
  public 
    class constructor;
    begin
      HeightProperty := DependencyProperty.Register('Height', typeof(integer), typeof(LegoVisual3D), new UIPropertyMetadata(3, GeometryChanged));
      RowsProperty := DependencyProperty.Register('Raws', typeof(integer), typeof(LegoVisual3D), new UIPropertyMetadata(3, GeometryChanged));
      ColumnsProperty := DependencyProperty.Register('Columns', typeof(integer), typeof(LegoVisual3D), new UIPropertyMetadata(3, GeometryChanged));
      DivisionsProperty := DependencyProperty.Register('Divisions', typeof(integer), typeof(LegoVisual3D), new UIPropertyMetadata(48));
    end;
    
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

type
  LegoT = class(ObjectWithMaterial3D)
  private
    function model := inherited model as LegoVisual3D;
    procedure SetWP(r: integer) := model.Rows := r;
    procedure SetW(r: integer) := Invoke(SetWP, r);
    function GetW: integer := InvokeInteger(()->model.Rows);
    
    procedure SetHP(r: integer) := model.Height := r;
    procedure SetH(r: integer) := Invoke(SetHP, r); 
    function GetH: integer := InvokeInteger(()->model.Height);
    
    procedure SetLP(r: integer) := model.Columns := r;
    procedure SetL(r: integer) := Invoke(SetLP, r);
    function GetL: integer := InvokeInteger(()->model.Columns);
  
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
    begin
      var bx := new LegoVisual3D;
      (bx.Rows, bx.Height, bx.Columns) := (Rows, Height, Columns);
      CreateBase(bx, x, y, z, m);
    end;
    
    property Columns: integer read GetL write SetL;
    property Rows: integer read GetW write SetW;
    property Height: integer read GetH write SetH;
    {property Size: Size3D read GetSz write SetSz;}
    function Clone := (inherited Clone) as LegoT;
  end;

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
  
  PlatonicAbstractT = class(ObjectWithMaterial3D)
  private
    procedure SetLengthP(r: real) := (model as PlatonicAbstractVisual3D).Length := r;
    procedure SetLength(r: real) := Invoke(SetLengthP, r); 
    function GetLength: real := InvokeReal(()->(model as PlatonicAbstractVisual3D).Length);
  public 
    property Length: real read GetLength write SetLength;
  end;
  
  IcosahedronT = class(PlatonicAbstractT)
  protected  
    function CreateObject: Object3D; override := new IcosahedronT(X, Y, Z, Length, Material);
  public 
    constructor(x, y, z, Length: real; m: GMaterial) := CreateBase(new IcosahedronVisual3D(Length), x, y, z, m);
    function Clone := (inherited Clone) as IcosahedronT;
  end;
  
  DodecahedronT = class(PlatonicAbstractT)
  protected  
    function CreateObject: Object3D; override := new DodecahedronT(X, Y, Z, Length, Material);
  public 
    constructor(x, y, z, Length: real; m: GMaterial) := CreateBase(new DodecahedronVisual3D(Length), x, y, z, m);
    function Clone := (inherited Clone) as DodecahedronT;
  end;
  
  TetrahedronT = class(PlatonicAbstractT)
  protected  
    function CreateObject: Object3D; override := new TetrahedronT(X, Y, Z, Length, Material);
  public 
    constructor(x, y, z, Length: real; m: GMaterial) := CreateBase(new TetrahedronVisual3D(Length), x, y, z, m);
    function Clone := (inherited Clone) as TetrahedronT;
  end;
  
  OctahedronT = class(PlatonicAbstractT)
  protected  
    function CreateObject: Object3D; override := new OctahedronT(X, Y, Z, Length, Material);
  public 
    constructor(x, y, z, Length: real; m: GMaterial) := CreateBase(new OctahedronVisual3D(Length), x, y, z, m);
    function Clone := (inherited Clone) as OctahedronT;
  end;
  
  PrismVisual3D = class(MeshElement3D)
  private 
    fn: integer;
    fh, fr: real;
    procedure SetR(value: real);begin fr := value; OnGeometryChanged; end;
    
    procedure SetH(value: real);begin fh := value; OnGeometryChanged; end;
    
    procedure SetN(value: integer);begin fn := value; OnGeometryChanged; end;
  
  public 
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
        var a := Partition(0, 2 * Pi, N).Select(x -> P3D(fr * cos(x), fr * sin(x), 0)).ToArray;
        var b := Partition(0, 2 * Pi, N).Select(x -> P3D(fr * cos(x), fr * sin(x), fh)).ToArray;
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
        var a := Partition(0, 2 * Pi, N).Select(x -> P3D(fr * cos(x), fr * sin(x), 0)).ToArray;
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
      Material := Colors.Red;
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
  
  TriangleT = class(ObjectWithMaterial3D)
  protected
    function Model := inherited model as TriangleVisual3D;
    procedure SetP1(p: Point3D) := Invoke(procedure(p: Point3D)->model.P1 := p, p);
    function  GetP1: Point3D := Invoke&<Point3D>(()->model.P1);
    procedure SetP2(p: Point3D) := Invoke(procedure(p: Point3D)->model.P2 := p, p);
    function  GetP2: Point3D := Invoke&<Point3D>(()->model.P2);
    procedure SetP3(p: Point3D) := Invoke(procedure(p: Point3D)->model.P3 := p, p);
    function  GetP3: Point3D := Invoke&<Point3D>(()->model.P3);
    
    function CreateObject: Object3D; override := new TriangleT(Model.p1, Model.p2, Model.p3, Material.Clone);
  public 
    constructor(p1, p2, p3: Point3D; m: Gmaterial);
    begin
      CreateBase(new TriangleVisual3D(p1, p2, p3), 0, 0, 0, m);
      Model.BackMaterial := Model.Material;      
    end;
    
    property P1: Point3D read GetP1 write SetP1;
    property P2: Point3D read GetP2 write SetP2;
    property P3: Point3D read GetP3 write SetP3;
    procedure SetPoints(p1, p2, p3: Point3D) := Invoke(procedure(p1, p2, p3: Point3D)->begin model.SetPoints(p1, p2, p3); end, p1, p2, p3);
  end;
  
  
  PrismT = class(ObjectWithMaterial3D)
  private
    function Model := inherited model as PrismVisual3D;
    procedure SetR(r: real) := Invoke(procedure(r: real)->model.Radius := r, r);
    function  GetR: real := InvokeReal(()->model.Radius);
    procedure SetH(r: real) := Invoke(procedure(r: real)->model.Height := r, r);
    function  GetH: real := InvokeReal(()->model.Height);
    procedure SetN(n: integer) := Invoke(procedure(n: integer)->model.N := n, n);
    function  GetN: integer := InvokeInteger(()->model.N);
  protected
    function CreateObject: Object3D; override := new PrismT(X, Y, Z, N, Radius, Height, Material.Clone);
  public 
    constructor(x, y, z: real; N: integer; r, h: real; m: Gmaterial) := CreateBase(new PrismVisual3D(N, r, h), x, y, z, m);
    property Radius: real read GetR write SetR;
    property Height: real read GetH write SetH;
    property N: integer read GetN write SetN;
    function Clone := (inherited Clone) as PrismT;
  end;
  
  PyramidT = class(PrismT)
  private
  protected
    function CreateObject: Object3D; override := new PyramidT(X, Y, Z, N, Radius, Height, Material.Clone);
  public 
    constructor(x, y, z: real; N: integer; r, h: real; m: GMaterial) := CreateBase(new PyramidVisual3D(N, r, h), x, y, z, m);
    function Clone := (inherited Clone) as PyramidT;
  end;
  
  PrismTWireframe = class(ObjectWithChildren3D)
  private 
    fn: integer;
    fh, fr: real;
    function Model := inherited model as LinesVisual3D;
    
    procedure SetCP(c: GColor) := Model.Color := c;
    procedure SetC(c: GColor) := Invoke(SetCP, c);
    function GetC: GColor := Invoke&<GColor>(()->Model.Color);
    procedure SetTP(th: real) := Model.Thickness := th;
    procedure SetT(th: real) := Invoke(SetTP, th);
    function GetT: real := InvokeReal(()->Model.Thickness);
    
    procedure SetRP(value: real);
    begin
      if fr = value then 
        exit;
      fr := value;
      model.Points := CreatePoints;
    end;
    
    procedure SetR(value: real) := Invoke(SetRP, value);
    procedure SetHP(value: real);
    begin
      if fh = value then 
        exit;
      fh := value;
      model.Points := CreatePoints;
    end;
    
    procedure SetH(value: real) := Invoke(SetHP, value);
    procedure SetNP(value: integer);
    begin
      if fN = value then 
        exit;
      fN := value;
      model.Points := CreatePoints;
    end;
    
    procedure SetN(value: integer) := Invoke(SetNP, value);
    
    function CreatePoints: Point3DCollection; virtual;
    begin
      var pc := new Point3DCollection;
      
      var a := Partition(0, 2 * Pi, N).Select(x -> P3D(fr * cos(x), fr * sin(x), 0)).ToArray;
      var b := Partition(0, 2 * Pi, N).Select(x -> P3D(fr * cos(x), fr * sin(x), fh)).ToArray;
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
    function CreateObject: Object3D; override := new PrismTWireframe(X, Y, Z, N, Radius, Height, (model as LinesVisual3D).Thickness, (model as LinesVisual3D).Color);
  public 
    function Points: Point3DCollection; virtual;
    begin
      var a := Partition(0, 2 * Pi, N).Select(x -> P3D(fr * cos(x), fr * sin(x), 0)).SkipLast;
      var b := Partition(0, 2 * Pi, N).Select(x -> P3D(fr * cos(x), fr * sin(x), fh)).SkipLast;
      var pc := new Point3DCollection(a + b);
      
      Result := pc;
    end;
    
    constructor(x, y, z: real; N: integer; Radius, Height: real; Thickness: real; c: GColor) := 
    CreateBase0(NewVisualObject(N, Radius, Height, Thickness, c), x, y, z);
    
    property Height: real read fh write SetH;
    property Radius: real read fr write SetR;
    property N: integer read fn write SetN;
    property Color: GColor read GetC write SetC; override;
    property Thickness: real read GetT write SetT;
  end;
  
  PyramidTWireframe = class(PrismTWireframe)
  protected
    function CreateObject: Object3D; override := new PyramidTWireframe(X, Y, Z, N, Radius, Height, (model as LinesVisual3D).Thickness, (model as LinesVisual3D).Color);
  private 
    function CreatePoints: Point3DCollection; override;
    begin
      var pc := new Point3DCollection;
      
      var a := Partition(0, 2 * Pi, N).Select(x -> P3D(fr * cos(x), fr * sin(x), 0)).ToArray;
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
  end;
  
  P3DArray = array of Point3D;
  //P3DList = List<Point3D>;
  SegmentsT = class(ObjectWithChildren3D)
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
    
    property Thickness: real read GetT write SetT;
    property Color: GColor read GetC write SetC; override;
    property Points: array of Point3D read GetP write SetP;
    function Clone := (inherited Clone) as SegmentsT;
  end;
  
  TorusT = class(ObjectWithMaterial3D)
  private
    function Model := inherited model as TorusVisual3D;
    procedure SetD(d: real) := Invoke(procedure(d: real)->model.TorusDiameter := d, d);
    function  GetD: real := InvokeReal(()->model.TorusDiameter);
    procedure SetTD(d: real) := Invoke(procedure(d: real)->model.TubeDiameter := d, d);
    function  GetTD: real := InvokeReal(()->model.TubeDiameter);
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
    
    property Diameter: real read GetD write SetD;
    property TubeDiameter: real read GetTD write SetTD;
    function Clone := (inherited Clone) as PrismT;
  end;
  
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
      
      var a := new My13D;
      a.Material := c;
      
      
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
      
      {var a := new HelixToolkit.Wpf.TubeVisual3D;
      var p := new Point3DCollection(Arr(P3D(1,2,0),P3D(2,1,0),P3D(3,1,0)));
      a.Diameter := 0.05;
      a.Path := p;}
      
      {var a := new LegoVisual3D();
      a.Rows := 1;
      a.Columns := 2;
      a.Height := 3;
      //a.Divisions := 100;
      a.Fill := Brushes.Blue;}
      
      CreateBase0(a, x, y, z);
    end;
  end;

function FindNearestObject(x, y: real): Object3D;
begin
  Result := nil;
  var v := hvp.FindNearestVisual(new Point(x, y));
  foreach var obj in Object3DList do
    if obj.model = v then
      Result := obj
end;

var
  BadPoint := P3D(real.MaxValue, real.MaxValue, real.MaxValue);

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

var
  PlaneXY := Plane(Origin, OrtZ);
  PlaneYZ := Plane(Origin, OrtX);
  PlaneXZ := Plane(Origin, OrtY);
  XAxis := Ray(Origin, OrtX);
  YAxis := Ray(Origin, OrtY);
  ZAxis := Ray(Origin, OrtZ);

function GetRay(x, y: real): Ray3D := hvp.Viewport.GetRay(Pnt(x, y));

function PointOnPlane(Self: Plane3D; x, y: real): Point3D; extensionmethod;
begin
  var r := GetRay(x, y);
  //Println(r);
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

function DefaultMaterialColor := RandomColor;

function DefaultMaterial := MaterialHelper.CreateMaterial(DefaultMaterialColor);

function Group(x, y, z: integer): GroupT := Inv(()->GroupT.Create(x, y, z));

function Group(p: Point3D): GroupT := Inv(()->GroupT.Create(p.x, p.y, p.z));

function Group: GroupT := Inv(()->GroupT.Create(0, 0, 0));

function Group(params lst: array of Object3D): GroupT := Inv(()->GroupT.Create(0, 0, 0, lst));

function Group(en: sequence of Object3D): GroupT := Inv(()->GroupT.Create(0, 0, 0, en));

function Sphere(x, y, z, Radius: real; m: Material := DefaultMaterial): SphereT := Inv(()->SphereT.Create(x, y, z, Radius, m));

function Sphere(center: Point3D; Radius: real; m: Material := DefaultMaterial) := Sphere(center.x, center.y, center.z, Radius, m);

function Ellipsoid(x, y, z, RadiusX, RadiusY, RadiusZ: real; m: Material := DefaultMaterial): EllipsoidT := Inv(()->EllipsoidT.Create(x, y, z, RadiusX, RadiusY, RadiusZ, m));

function Ellipsoid(center: Point3D; RadiusX, RadiusY, RadiusZ: real; m: Material := DefaultMaterial) := Ellipsoid(center.x, center.y, center.z, RadiusX, RadiusY, RadiusZ, m);

function Cube(x, y, z, SideLength: real; m: Material := DefaultMaterial): CubeT := Inv(()->CubeT.Create(x, y, z, SideLength, m));

function Cube(center: Point3D; SideLength: real; m: Material := DefaultMaterial): CubeT := Cube(center.x, center.y, center.z, SideLength, m);

function Box(x, y, z, SizeX, SizeY, SizeZ: real; m: Material := DefaultMaterial): BoxT := Inv(()->BoxT.Create(x, y, z, SizeX, SizeY, SizeZ, m));

function Box(center: Point3D; sz: Size3D; m: Material := DefaultMaterial): BoxT := Inv(()->BoxT.Create(center.x, center.y, center.z, sz.X, sz.Y, sz.z, m));

const
  arhl = 3; ard = 0.2;

function Arrow(x, y, z, vx, vy, vz, diameter, hl: real; m: Material := DefaultMaterial): ArrowT := Inv(()->ArrowT.Create(x, y, z, vx, vy, vz, diameter, hl, m));

function Arrow(x, y, z, vx, vy, vz, diameter: real; m: Material := DefaultMaterial) := Arrow(x, y, z, vx, vy, vz, diameter, arhl, m);

function Arrow(x, y, z, vx, vy, vz: real; m: Material := DefaultMaterial) := Arrow(x, y, z, vx, vy, vz, ard, arhl, m);

function Arrow(p: Point3D; v: Vector3D; diameter, hl: real; m: Material := DefaultMaterial) := Arrow(p.x, p.y, p.z, v.X, v.Y, v.Z, diameter, hl, m);

function Arrow(p: Point3D; v: Vector3D; diameter: real; m: Material := DefaultMaterial) := Arrow(p.x, p.y, p.z, v.X, v.Y, v.Z, diameter, arhl, m);

function Arrow(p: Point3D; v: Vector3D; m: Material := DefaultMaterial) := Arrow(p.x, p.y, p.z, v.X, v.Y, v.Z, ard, arhl, m);

function TruncatedConeAux(x, y, z, Height, Radius, TopRadius: real; sides: integer; topcap: boolean; c: Material) := Inv(()->TruncatedConeT.Create(x, y, z, Height, Radius, TopRadius, sides, topcap, c));

const
  maxsides = 37;

///--
function TruncatedCone(x, y, z, Height, Radius, TopRadius: real; topcap: boolean; m: Material := DefaultMaterial): TruncatedConeT := TruncatedConeAux(x, y, z, Height, Radius, TopRadius, maxsides, topcap, m); 

function TruncatedCone(x, y, z, Height, Radius, TopRadius: real; m: Material := DefaultMaterial) := TruncatedCone(x, y, z, Height, Radius, TopRadius, True, m);
///--
function TruncatedCone(p: Point3D; Height, Radius, TopRadius: real; topcap: boolean; m: Material := DefaultMaterial) := TruncatedCone(p.x, p.y, p.z, Height, Radius, TopRadius, topcap, m);

function TruncatedCone(p: Point3D; Height, Radius, TopRadius: real; m: Material := DefaultMaterial) := TruncatedCone(p.x, p.y, p.z, Height, Radius, TopRadius, True, m);

///--
function Cylinder(x, y, z, Height, Radius: real; topcap: boolean; m: Material := DefaultMaterial): CylinderT := Inv(()->CylinderT.Create(x, y, z, Height, Radius, maxsides, topcap, m));

function Cylinder(x, y, z, Height, Radius: real; m: Material := DefaultMaterial) := Cylinder(x, y, z, Height, Radius, True, m);
///--
function Cylinder(p: Point3D; Height, Radius: real; topcap: boolean; m: Material := DefaultMaterial) := Cylinder(p.x, p.y, p.z, Height, Radius, topcap, m);

function Cylinder(p: Point3D; Height, Radius: real; m: Material := DefaultMaterial) := Cylinder(p.x, p.y, p.z, Height, Radius, True, m);

function Tube(x, y, z, Height, Radius, InnerRadius: real; m: Material := DefaultMaterial): PipeT := Inv(()->PipeT.Create(x, y, z, Height, Radius, InnerRadius, m));

function Tube(p: Point3D; Height, Radius, InnerRadius: real; m: Material := DefaultMaterial) := Tube(p.x, p.y, p.z, Height, Radius, InnerRadius, m);

function Cone(x, y, z, Height, Radius: real; m: Material := DefaultMaterial): TruncatedConeT := TruncatedCone(x, y, z, Height, Radius, 0, True, m);

function Cone(p: Point3D; Height, Radius: real; m: Material := DefaultMaterial) := TruncatedCone(p.x, p.y, p.z, Height, Radius, 0, True, m);

function Teapot(x, y, z: real; c: Material := DefaultMaterial): TeapotT := Inv(()->TeapotT.Create(x, y, z, c));

function Teapot(p: Point3D; c: Material := DefaultMaterial) := Teapot(p.x, p.y, p.z, c);

function BillboardText(x, y, z: real; Text: string; Fontsize: real := 12): BillboardTextT := Inv(()->BillboardTextT.Create(x, y, z, text, fontsize));

function BillboardText(p: Point3D; Text: string; Fontsize: real := 12) := BillboardText(P.x, p.y, p.z, text, fontsize);

function CoordinateSystem(ArrowsLength, Diameter: real): CoordinateSystemT := Inv(()->CoordinateSystemT.Create(0, 0, 0, arrowslength, diameter));

function CoordinateSystem(ArrowsLength: real) := CoordinateSystem(arrowslength, arrowslength / 10);

function Text3D(x, y, z: real; Text: string; Height: real; fontname: string := 'Arial'; c: Color := Colors.Black): TextT := Inv(()->TextT.Create(x, y, z, text, height, fontname, c));

function Text3D(p: Point3D; Text: string; Height: real; fontname: string := 'Arial'; c: Color := Colors.Black) := Text3D(P.x, p.y, p.z, text, height, fontname, c);

function Text3D(x, y, z: real; Text: string; Height: real; c: Color) := Text3D(x, y, z, text, height, 'Arial', c);

function Text3D(p: Point3D; Text: string; Height: real; c: Color) := Text3D(p.x, p.y, p.z, text, height, 'Arial', c);


function Rectangle3D(x, y, z, Length, Width: real; Normal, LengthDirection: Vector3D; m: Material := DefaultMaterial): RectangleT := Inv(()->RectangleT.Create(x, y, z, Length, Width, normal, LengthDirection, m));

function Rectangle3D(p: Point3D; Length, Width: real; Normal, LengthDirection: Vector3D; m: Material := DefaultMaterial): RectangleT := Rectangle3D(p.x, p.y, p.z, Length, Width, Normal, LengthDirection, m);

function Rectangle3D(x, y, z, Length, Width: real; Normal: Vector3D; m: Material := DefaultMaterial): RectangleT := Rectangle3D(x, y, z, Length, Width, Normal, OrtX, m); 

function Rectangle3D(x, y, z, Length, Width: real; m: Material := DefaultMaterial): RectangleT := Rectangle3D(x, y, z, Length, Width, OrtZ, OrtX, m); 

function Rectangle3D(p: Point3D; Length, Width: real; Normal: Vector3D; m: Material := DefaultMaterial): RectangleT := Rectangle3D(p.x, p.y, p.z, Length, Width, Normal, OrtX, m); 

function Rectangle3D(p: Point3D; Length, Width: real; m: Material := DefaultMaterial): RectangleT := Rectangle3D(p.x, p.y, p.z, Length, Width, OrtZ, OrtX, m); 

/// Загружает модель из файла .obj, .3ds, .lwo, .objz, .stl, .off
function FileModel3D(x, y, z: real; fname: string; m: Material): FileModelT := Inv(()->FileModelT.Create(x, y, z, fname, m));

function FileModel3D(p: Point3D; fname: string; m: Material): FileModelT := FileModel3D(p.x, p.y, p.z, fname, m);

function Prism(x, y, z: real; Sides: integer; Height, Radius: real; m: Material := DefaultMaterial): PrismT := Inv(()->PrismT.Create(x, y, z, Sides, Radius, Height, m));

function Prism(p: Point3D; Sides: integer; Height, Radius: real; m: Material := DefaultMaterial): PrismT := Prism(p.X, p.Y, p.Z, Sides, Radius, Height, m);

function PrismWireFrame(x, y, z: real; Sides: integer; Height, Radius: real; Thickness: real := 1.2; c: Color := GrayColor(64)): PrismTWireFrame := Inv(()->PrismTWireFrame.Create(x, y, z, Sides, Radius, Height, thickness, c));

function PrismWireFrame(p: Point3D; Sides: integer; Height, Radius: real; Thickness: real := 1.2; c: Color := GrayColor(64)): PrismTWireFrame := PrismWireFrame(p.x, p.y, p.z, Sides, Radius, Height, thickness, c);

function Pyramid(x, y, z: real; Sides: integer; Height, Radius: real; m: Material := DefaultMaterial): PyramidT := Inv(()->PyramidT.Create(x, y, z, Sides, Radius, Height, m));

function Pyramid(p: Point3D; Sides: integer; Height, Radius: real; m: Material := DefaultMaterial): PyramidT := Pyramid(p.X, p.Y, p.Z, Sides, Radius, Height, m);

function PyramidWireFrame(x, y, z: real; Sides: integer; Height, Radius: real; Thickness: real := 1.2; c: Color := GrayColor(64)): PyramidTWireFrame := Inv(()->PyramidTWireFrame.Create(x, y, z, Sides, Radius, Height, thickness, c));

function PyramidWireFrame(p: Point3D; Sides: integer; Height, Radius: real; Thickness: real := 1.2; c: Color := GrayColor(64)): PyramidTWireFrame := PyramidWireFrame(p.x, p.y, p.z, Sides, Radius, Height, thickness, c);

function Lego(x, y, z: real; Rows, Columns, Height: integer; m: Material := DefaultMaterial): LegoT := Inv(()->LegoT.Create(x, y, z, Rows, Columns, Height, m));

function Icosahedron(x, y, z, r: real; m: Material := DefaultMaterial): IcosahedronT := Inv(()->IcosahedronT.Create(x, y, z, 4 * R / Sqrt(2) / Sqrt(5 + Sqrt(5)), m));

function Dodecahedron(x, y, z, r: real; m: Material := DefaultMaterial): DodecahedronT := Inv(()->DodecahedronT.Create(x, y, z, R * 4 / Sqrt(3) / (1 + Sqrt(5)), m));

function Tetrahedron(x, y, z, r: real; m: Material := DefaultMaterial): TetrahedronT := Inv(()->TetrahedronT.Create(x, y, z, 4 * R / Sqrt(6), m));

function Octahedron(x, y, z, r: real; m: Material := DefaultMaterial): OctahedronT := Inv(()->OctahedronT.Create(x, y, z, R * Sqrt(2), m));

function Icosahedron(p: Point3D; r: real; m: Material := DefaultMaterial): IcosahedronT := Icosahedron(p.X, p.Y, p.Z, R, m);

function Dodecahedron(p: Point3D; r: real; m: Material := DefaultMaterial): DodecahedronT := Dodecahedron(p.X, p.Y, p.Z, R, m);

function Tetrahedron(p: Point3D; r: real; m: Material := DefaultMaterial): TetrahedronT := Tetrahedron(p.X, p.Y, p.Z, R, m);

function Octahedron(p: Point3D; r: real; m: Material := DefaultMaterial): OctahedronT := Octahedron(p.X, p.Y, p.Z, R, m);

function Segments3D(points: sequence of Point3D; thickness: real := 1.2; c: Color := GrayColor(64)): SegmentsT := Inv(()->SegmentsT.Create(points, thickness, c));

function Polyline3D(points: sequence of Point3D; thickness: real := 1.2; c: Color := GrayColor(64)): SegmentsT := Inv(()->SegmentsT.Create(points.Pairwise.SelectMany(x -> Seq(x[0], x[1])), thickness, c));

function Polygon3D(points: sequence of Point3D; thickness: real := 1.2; c: Color := GrayColor(64)): SegmentsT := Inv(()->SegmentsT.Create((points + points.First).Pairwise.SelectMany(x -> Seq(x[0], x[1])), thickness, c));

function Segment3D(p1, p2: Point3D; thickness: real := 1.2; c: Color := GrayColor(64)): SegmentsT := Inv(()->SegmentsT.Create(Seq(p1, p2), thickness, c));

function Torus(x, y, z, Diameter, TubeDiameter: real; m: Material := DefaultMaterial): TorusT := Inv(()->TorusT.Create(x, y, z, Diameter, TubeDiameter, m));

function Torus(p: Point3D; Diameter, TubeDiameter: real; m: Material := DefaultMaterial): TorusT := Torus(p.x, p.y, p.z, Diameter, TubeDiameter, m);

function Triangle(p1, p2, p3: Point3D; m: Material := DefaultMaterial): TriangleT := Inv(()->TriangleT.Create(p1, p2, p3, m));

function MyH(x, y, z, Length: real; c: Color): MyAnyT := Inv(()->MyAnyT.Create(x, y, z, Length, c));

function MyH(x, y, z, Length: real; c: Material): MyAnyT := Inv(()->MyAnyT.Create(x, y, z, Length, c));

function Any(x, y, z: real; c: Color): AnyT := Inv(()->AnyT.Create(x, y, z, c));


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
      View3D := new View3DT;
      
      NameScope.SetNameScope(Self, new NameScope());
    end;
    
    /// --- SystemKeyEvents
    procedure SystemOnKeyDown(sender: Object; e: System.Windows.Input.KeyEventArgs);
    begin
      if Graph3D.OnKeyDown <> nil then
        Graph3D.OnKeyDown(e.Key);
      e.Handled := True;
    end;
    
    procedure SystemOnKeyUp(sender: Object; e: System.Windows.Input.KeyEventArgs) := 
    begin
      if Graph3D.OnKeyUp <> nil then
        Graph3D.OnKeyUp(e.Key);
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
    end;
    
    procedure InitHandlers; override;
    begin
      hvp.PreviewMouseDown += (o, e) -> SystemOnMouseDown(o, e);  
      hvp.PreviewMouseUp += (o, e) -> SystemOnMouseUp(o, e);  
      hvp.PreviewMouseMove += (o, e) -> SystemOnMouseMove(o, e);  
      
      hvp.PreviewKeyDown += (o, e)-> SystemOnKeyDown(o, e);
      hvp.PreviewKeyUp += (o, e)-> SystemOnKeyUp(o, e);
      
      hvp.Focus();
      Closed += procedure(sender, e) -> begin Halt; end;
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
  //MainWindow.MainPanel;
  
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