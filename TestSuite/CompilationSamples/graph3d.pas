// Copyright (©) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit Graph3D;

{$reference 'PresentationFramework.dll'}
{$reference 'WindowsBase.dll'}
{$reference 'PresentationCore.dll'}
{$reference System.Xml.dll}

{$reference HelixToolkit.Wpf.dll}
{reference Petzold.Media3D.dll}

{$apptype windows}

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
  GWindow = System.Windows.Window;
  CameraMode = HelixToolkit.Wpf.CameraMode;
  TupleInt3 = (integer,integer,integer);  
  TupleReal3 = (real,real,real);

var
  MainFormThread: Thread; 
  app: Application;
  MainWindow: Window;
  hvp: HelixViewport3D;
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

function RGB(r,g,b: byte) := Color.Fromrgb(r, g, b);
function ARGB(a,r,g,b: byte) := Color.FromArgb(a, r, g, b);
function P3D(x,y,z: real) := new Point3D(x,y,z);
function V3D(x,y,z: real) := new Vector3D(x,y,z);
function Sz3D(x,y,z: real) := new Size3D(x,y,z);
function Pnt(x,y: real) := new Point(x,y);
function Rect(x,y,w,h: real) := new System.Windows.Rect(x,y,w,h);

var 
  OrtX := V3D(1,0,0);
  OrtY := V3D(0,1,0);
  OrtZ := V3D(0,0,1);

function ChangeAlpha(Self: GColor; value: integer); extensionmethod := ARGB(value,Self.R, Self.G, Self.B);

function MoveX(Self: Point3D; dx: real); extensionmethod := P3D(Self.x + dx, Self.y, Self.z);
function MoveY(Self: Point3D; dy: real); extensionmethod := P3D(Self.x, Self.y + dy, Self.z);
function MoveZ(Self: Point3D; dz: real); extensionmethod := P3D(Self.x, Self.y, Self.z + dz);
function Move(Self: Point3D; dx,dy,dz: real); extensionmethod := P3D(Self.x + dx, Self.y + dy, Self.z + dz);

function operator implicit(t: TupleInt3): Point3D; extensionmethod := new Point3D(t[0],t[1],t[2]);
function operator implicit(t: TupleReal3): Point3D; extensionmethod := new Point3D(t[0],t[1],t[2]);

function operator implicit(ar: array of TupleInt3): Point3DCollection; extensionmethod := new Point3DCollection(ar.Select(t->new Point3D(t[0],t[1],t[2])));

function RandomColor := Color.Fromrgb(Random(256), Random(256), Random(256));

function RandomSolidBrush := new SolidColorBrush(RandomColor);

procedure Invoke(d: System.Delegate; params args: array of object) := app.Dispatcher.Invoke(d,args);
procedure Invoke(d: ()->()) := app.Dispatcher.Invoke(d);
function Invoke<T>(d: Func0<T>) := app.Dispatcher.Invoke&<T>(d);

function wplus := SystemParameters.WindowResizeBorderThickness.Left + SystemParameters.WindowResizeBorderThickness.Right;
function hplus := SystemParameters.WindowCaptionHeight + SystemParameters.WindowResizeBorderThickness.Top + SystemParameters.WindowResizeBorderThickness.Bottom;

type MHelper = auto class
  fname: string;
  opacity: real;
  function ImageMaterial := MaterialHelper.CreateImageMaterial(fname,opacity);
end;

function ImageMaterial(fname: string): GMaterial 
  := Invoke&<GMaterial>(MHelper.Create(fname,1).ImageMaterial);

/// --- SystemKeyEvents
procedure SystemOnKeyDown(sender: Object; e: System.Windows.Input.KeyEventArgs) := 
begin
  if OnKeyDown<>nil then
    OnKeyDown(e.Key);
end;    

procedure SystemOnKeyUp(sender: Object; e: System.Windows.Input.KeyEventArgs) := 
  if OnKeyUp<>nil then
    OnKeyUp(e.Key);

/// --- SystemMouseEvents
procedure SystemOnMouseDown(sender: Object; e: System.Windows.Input.MouseButtonEventArgs);
begin
  var mb := 0;
  var p := e.GetPosition(hvp);
  if e.LeftButton = MouseButtonState.Pressed then
    mb := 1
  else if e.RightButton = MouseButtonState.Pressed then
    mb := 2;
  if OnMouseDown <> nil then  
    OnMouseDown(p.x, p.y, mb);
end;

procedure SystemOnMouseUp(sender: Object; e: MouseButtonEventArgs);
begin
  var mb := 0;
  var p := e.GetPosition(hvp);
  if e.LeftButton = MouseButtonState.Pressed then
    mb := 1
  else if e.RightButton = MouseButtonState.Pressed then
    mb := 2;
  if OnMouseUp <> nil then  
    OnMouseUp(p.x, p.y, mb);
end;

procedure SystemOnMouseMove(sender: Object; e: MouseEventArgs);
begin
  var mb := 0;
  var p := e.GetPosition(hvp);
  if e.LeftButton = MouseButtonState.Pressed then
    mb := 1
  else if e.RightButton = MouseButtonState.Pressed then
    mb := 2;
  if OnMouseMove <> nil then  
    OnMouseMove(p.x, p.y, mb);
end;

type
///!#
  View3DT = class
  private
    procedure SetSCSP(v: boolean) := hvp.ShowCoordinateSystem := v;
    procedure SetSCS(v: boolean) := Invoke(SetSCSP,v);
    function GetSCS: boolean := Invoke&<boolean>(()->hvp.ShowCoordinateSystem);

    procedure SetSGLP(v: boolean) := gvl.Visible := v;
    procedure SetSGL(v: boolean) := Invoke(SetSGLP,v);
    function GetSGL: boolean := Invoke&<boolean>(()->gvl.Visible);

    procedure SetSCIP(v: boolean) := hvp.ShowCameraInfo := v;
    procedure SetSCI(v: boolean) := Invoke(SetSCIP,v);
    function GetSCI: boolean := Invoke&<boolean>(()->hvp.ShowCameraInfo);

    procedure SetSVCP(v: boolean) := hvp.ShowViewCube := v;
    procedure SetSVC(v: boolean) := Invoke(SetSVCP,v);
    function GetSVC: boolean := Invoke&<boolean>(()->hvp.ShowViewCube);

    procedure SetTP(v: string) := hvp.Title := v;
    procedure SetT(v: string) := Invoke(SetTP,v);
    function GetT: string := Invoke&<string>(()->hvp.Title);

    procedure SetSTP(v: string) := hvp.SubTitle := v;
    procedure SetST(v: string) := Invoke(SetSTP,v);
    function GetST: string := Invoke&<string>(()->hvp.SubTitle);

    procedure SetCMP(v: HelixToolkit.Wpf.CameraMode) := hvp.CameraMode := v;
    procedure SetCM(v: HelixToolkit.Wpf.CameraMode) := Invoke(SetCMP,v);
    function GetCM: HelixToolkit.Wpf.CameraMode := Invoke&<HelixToolkit.Wpf.CameraMode>(()->hvp.CameraMode);

    procedure SetBCP(v: GColor) := hvp.Background := new SolidColorBrush(v);
    procedure SetBC(v: GColor) := Invoke(SetBCP,v);
    function GetBC: GColor := Invoke&<GColor>(()->(hvp.Background as SolidColorBrush).Color);
    procedure ExportP(fname: string) := hvp.Viewport.Export(fname,hvp.Background);
  public
    property ShowCoordinateSystem: boolean read GetSCS write SetSCS;
    property ShowGridLines: boolean read GetSGL write SetSGL;
    property ShowCameraInfo: boolean read GetSCI write SetSCI;
    property ShowViewCube: boolean read GetSVC write SetSVC;
    property Title: string read GetT write SetT;
    property SubTitle: string read GetST write SetST;
    property CameraMode: HelixToolkit.Wpf.CameraMode read GetCM write SetCM;
    property BackgroundColor: GColor read GetBC write SetBC;
    
    procedure Save(fname: string) := Invoke(ExportP,fname);
  end;

 ///!#
  CameraType = class
  private 
    function Cam: GCamera := hvp.Camera;
    procedure SetPP(p: Point3D) := begin Cam.Position := p; Cam.LookDirection := Cam.Position.Multiply(-1).ToVector3D; end;
    procedure SetP(p: Point3D) := Invoke(SetPP,p);
    function GetP: Point3D := Invoke&<Point3D>(()->Cam.Position);
    procedure SetLDP(v: Vector3D) := Cam.LookDirection := v;
    procedure SetLD(v: Vector3D) := Invoke(SetLDP,v);
    function GetLD: Vector3D := Invoke&<Vector3D>(()->Cam.LookDirection);
    procedure SetUDP(v: Vector3D) := Cam.UpDirection := v;
    procedure SetUD(v: Vector3D) := Invoke(SetUDP,v);
    function GetUD: Vector3D := Invoke&<Vector3D>(()->Cam.UpDirection);
    procedure SetDP(d: real);
    begin
      var dist := Cam.Position.DistanceTo(P3D(0,0,0));
      Cam.Position := Cam.Position.Multiply(d/dist);
    end;
    procedure SetD(d: real) := Invoke(SetDP,d);
    function GetD: real := Invoke&<real>(()->Cam.Position.DistanceTo(P3D(0,0,0)));
  public 
    property Position: Point3D read GetP write SetP;
    property LookDirection: Vector3D read GetLD write SetLD;
    property UpDirection: Vector3D read GetUD write SetUD;
    property Distanse: real read GetD write SetD;
  end;

 ///!#
  WindowType = class
  private 
    procedure SetLeft(l: real);
    function GetLeft: real;
    procedure SetTop(t: real);
    function GetTop: real;
    procedure SetWidth(w: real);
    function GetWidth: real;
    procedure SetHeight(h: real);
    function GetHeight: real;
    procedure SetCaption(c: string);
    function GetCaption: string;
  public 
    /// Отступ графического окна от левого края экрана в пикселах
    property Left: real read GetLeft write SetLeft;
    /// Отступ графического окна от верхнего края экрана в пикселах
    property Top: real read GetTop write SetTop;
    /// Ширина клиентской части графического окна в пикселах
    property Width: real read GetWidth write SetWidth;
    /// Высота клиентской части графического окна в пикселах
    property Height: real read GetHeight write SetHeight;
    /// Заголовок графического окна
    property Caption: string read GetCaption write SetCaption;
    /// Заголовок графического окна
    property Title: string read GetCaption write SetCaption;
    /// Устанавливает размеры клиентской части графического окна в пикселах
    procedure SetSize(w, h: real);
    /// Устанавливает отступ графического окна от левого верхнего края экрана в пикселах
    procedure SetPos(l, t: real);
    /// Закрывает графическое окно и завершает приложение
    procedure Close;
    /// Сворачивает графическое окно
    procedure Minimize;
    /// Максимизирует графическое окно
    procedure Maximize;
    /// Возвращает графическое окно к нормальному размеру
    procedure Normalize;
    /// Центрирует графическое окно по центру экрана
    procedure CenterOnScreen;
    /// Возвращает центр графического окна
    function Center: Point;
    /// Возвращает прямоугольник клиентской области окна
    function ClientRect: GRect;
  end;  

var 
  View3D: View3DT;
  Window: WindowType;
  Camera: CameraType;

type
  MyAnimation = class;
  GroupT = class;
///!#
  Object3D = class//(DependencyObject)
  private
    model: Visual3D;
    Parent: GroupT;
    transfgroup := new Transform3DGroup; 
    //rotatetransform := new MatrixTransform3D;
    rotatetransform_anim := new RotateTransform3D;
    scaletransform := new ScaleTransform3D;
    transltransform: TranslateTransform3D;
    
    procedure CreateBase0(m: Visual3D; x,y,z: real);
    begin
      model := m;
      transltransform := new TranslateTransform3D(x,y,z);
      transfgroup.Children.Add(new MatrixTransform3D); // ответственен за поворот. Не храним в отдельной переменной т.к. при повороте меняется сам объект, а не поля объекта!!!
      //transfgroup.Children.Add(rotatetransform);
      transfgroup.Children.Add(rotatetransform_anim);
        rotatetransform_anim.Rotation := new AxisAngleRotation3D();
      transfgroup.Children.Add(scaletransform); 
      transfgroup.Children.Add(transltransform);
      model.Transform := transfgroup;
      hvp.Children.Add(model);
    end;
    procedure SetX(xx: real) := Invoke(()->begin transltransform.OffsetX := xx; end); 
    function GetX: real := Invoke&<real>(()->transltransform.OffsetX);
    procedure SetY(yy: real) := Invoke(()->begin transltransform.OffsetY := yy; end);
    function GetY: real := Invoke&<real>(()->transltransform.OffsetY);
    procedure SetZ(zz: real) := Invoke(()->begin transltransform.OffsetZ := zz; end);
    function GetZ: real := Invoke&<real>(()->transltransform.OffsetZ);
    function GetPos: Point3D := Invoke&<Point3D>(()->P3D(transltransform.OffsetX,transltransform.OffsetY,transltransform.OffsetZ));
  protected  
    function CreateObject: Object3D; virtual;
    begin
      Result := nil;
    end;
    function CloneT: Object3D;
    begin
      Result := CreateObject;
      (Result.model.Transform as Transform3DGroup).Children[0] := (model.Transform as Transform3DGroup).Children[0].Clone;
      (Result.model.Transform as Transform3DGroup).Children[1] := (model.Transform as Transform3DGroup).Children[1].Clone;
      (Result.model.Transform as Transform3DGroup).Children[2] := (model.Transform as Transform3DGroup).Children[2].Clone;
      // (Result.model.Transform as Transform3DGroup).Children[3] := (model.Transform as Transform3DGroup).Children[3].Clone; - почему-то это не нужно!!! с ним не работает!
    end;
  public  
    property X: real read GetX write SetX;
    property Y: real read GetY write SetY;
    property Z: real read GetZ write SetZ;

    function MoveTo(xx,yy,zz: real): Object3D := 
      Invoke&<Object3D>(()->begin 
        transltransform.OffsetX := xx;
        transltransform.OffsetY := yy;
        transltransform.OffsetZ := zz;
        Result := Self;
      end);
    function MoveTo(p: Point3D) := MoveTo(p.X,p.y,p.z);
    function MoveOn(dx,dy,dz: real) := MoveTo(x+dx,y+dy,z+dz);
    function MoveOn(v: Vector3D) := MoveOn(v.X,v.Y,v.Z);
    function MoveOnX(dx: real) := MoveOn(dx,0,0);
    function MoveOnY(dy: real) := MoveOn(0,dy,0);
    function MoveOnZ(dz: real) := MoveOn(0,0,dz);
  private
    procedure MoveToProp(p: Point3D) := MoveTo(p);
  public
    property Position: Point3D read GetPos write MoveToProp;
    
    function Scale(f: real): Object3D := 
      Invoke&<Object3D>(()->begin
        scaletransform.ScaleX *= f;
        scaletransform.ScaleY *= f;
        scaletransform.ScaleZ *= f;
        Result := Self;
      end);
    /// Поворот на угол angle вокруг оси axis
    function Rotate(axis: Vector3D; angle: real): Object3D := 
      Invoke&<Object3D>(()->begin
        var m := transfgroup.Children[0].Value; 
        m.Rotate(new Quaternion(axis,angle));
        transfgroup.Children[0] := new MatrixTransform3D(m);
        Result := Self;
      end);
    /// Поворот на угол angle вокруг оси axis относительно точки center
    function RotateAt(axis: Vector3D; angle: real; center: Point3D): Object3D :=
      Invoke&<Object3D>(()->begin
        var m := transfgroup.Children[0].Value;    
        m.RotateAt(new Quaternion(axis,angle),center);
        transfgroup.Children[0] := new MatrixTransform3D(m);
        Result := Self;
      end);
    function AnimMoveTo(x,y,z: real; seconds: real := 1): MyAnimation;
    function AnimMoveTo(p: Point3D; seconds: real := 1) := AnimMoveTo(p.x,p.y,p.z,seconds);
    function AnimMoveOn(dx,dy,dz: real; seconds: real := 1): MyAnimation;
    function AnimMoveOn(v: Vector3D; seconds: real := 1) := AnimMoveOn(v.x,v.y,v.z,seconds);
    function AnimMoveOnX(dx: real; seconds: real := 1) := AnimMoveOn(dx,0,0,seconds);
    function AnimMoveOnY(dy: real; seconds: real := 1) := AnimMoveOn(0,dy,0,seconds);
    function AnimMoveOnZ(dz: real; seconds: real := 1) := AnimMoveOn(0,0,dz,seconds);
    function AnimScale(sc: real; seconds: real := 1): MyAnimation;
    function AnimRotate(vx,vy,vz,angle: real; seconds: real := 1): MyAnimation;
    function AnimRotate(v: Vector3D; angle:real; seconds: real := 1) := AnimRotate(v.x,v.y,v.z,angle,seconds);
    function AnimRotateAt(axis: Vector3D; angle: real; center: Point3D; seconds: real := 1): MyAnimation;
    function Clone: Object3D := Invoke&<Object3D>(CloneT);
  end;
  
  ObjectColored3D = class(Object3D)
  private
    procedure CreateBase(m: MeshElement3D; x,y,z: real; mat: GMaterial);
    begin
      CreateBase0(m,x,y,z);
      m.Material := mat;
    end;
    procedure CreateBase(m: MeshElement3D; x,y,z: real; c: Color);
    begin
      CreateBase(m,x,y,z,MaterialHelper.CreateMaterial(c));
    end;
    procedure SetColorP(c: GColor) := (model as MeshElement3D).Material := MaterialHelper.CreateMaterial(c);
    procedure SetColor(c: GColor) := Invoke(SetColorP,c); 
    procedure SetVP(v: boolean) := (model as MeshElement3D).Visible := v;
    procedure SetV(v: boolean) := Invoke(SetVP,v);
    function GetV: boolean := Invoke&<boolean>(()->(model as MeshElement3D).Visible);

    procedure SetMP(mat: GMaterial) := (model as MeshElement3D).Material := mat;
    procedure SetMaterial(mat: GMaterial) := Invoke(SetMP,mat);
    function GetMaterial: GMaterial := Invoke&<GMaterial>(()->(model as MeshElement3D).Material);
  public  
    property Color: GColor write SetColor;
    property Material: GMaterial read GetMaterial write SetMaterial;
    property Visible: boolean read GetV write SetV;
  end; 

  GroupT = class(ObjectColored3D)
    l := new List<Object3D>;
  private
    procedure AddT(obj: Object3D);
    begin
      if obj.Parent = Self then
        exit;
      if obj.Parent = nil then
        hvp.Children.Remove(obj.model)
      else 
      begin
        (obj.Parent.model as MeshVisual3D).Children.Remove(obj.model);
        obj.Parent.l.Remove(obj);
      end;
      (model as ModelVisual3D).Children.Add(obj.model);
      l.Add(obj);  
      obj.Parent := Self;
    end;
    procedure RemoveT(obj: Object3D);
    begin
      var b := (model as MeshVisual3D).Children.Remove(obj.model);
      if not b then exit;
      l.Remove(obj);
      hvp.Children.Add(obj.model);
      obj.Parent := nil;
    end;
    function GetObj(i: integer): Object3D := l[i];
    function CountT: integer := (model as ModelVisual3D).Children.Count;
  protected
    function CreateObject: Object3D; override;
    begin
      var g  := new GroupT(X,Y,Z);
      foreach var xx in l do
        g.Add(xx.Clone);
      Result := g;  
    end;  
  public
    constructor(x,y,z: real);
    begin
      CreateBase0(new MeshVisual3D,x,y,z);
    end;
    constructor(x,y,z: real; lst: sequence of Object3D);
    begin
      CreateBase0(new MeshVisual3D,x,y,z);
      foreach var xx in lst do
        Add(xx);
    end;
    function Add(obj: Object3D): GroupT;
    begin 
      Invoke(AddT,obj);
      Result := Self
    end;
    function Count: integer := Invoke&<integer>(CountT);
    procedure Remove(obj: Object3D) := Invoke(RemoveT,obj);
    property Items[i: integer]: Object3D read GetObj; default;
    function Clone := (inherited Clone) as GroupT;
  end;
  
//------------------------------ Animation -----------------------------------
  MyAnimation = class
  private
    Element: Object3D;
    Seconds: real;
    procedure BeginT;
    begin
      sb := CreateStoryboard;
      InitAnim(sb);
      sb.Completed += procedure (o,e) -> sb.Children.Clear;
      sb.Begin;
    end;
  protected 
    sb: StoryBoard;
    procedure AddDoubleAnimByName(sb: StoryBoard; toValue,seconds: real; ttname: string; prop: Object; waittime: real := 0.0); 
    begin
      var d := new DoubleAnimation(toValue,new System.Windows.Duration(System.TimeSpan.FromSeconds(seconds)));
      d.BeginTime := System.TimeSpan.FromSeconds(waittime);
      StoryBoard.SetTargetName(d,ttname);
      StoryBoard.SetTargetProperty(d,new PropertyPath(prop));
      sb.Children.Add(d);
    end;
    function RegisterName(sb: StoryBoard; element: Object; ttname: string): boolean;
    begin
      Result := False;
      if MainWindow.FindName(ttname)=nil then 
      begin
        MainWindow.RegisterName(ttname,element);
        sb.Completed += (o,e) -> begin
          if MainWindow.FindName(ttname)<>nil then 
            MainWindow.UnregisterName(ttname); 
        end;
        Result := True;
      end;
    end;
    procedure InitAnim(sb: StoryBoard); virtual := InitAnimWait(sb,0);
  private  
    AnimationCompleted: procedure;
    procedure InitAnimWait(sb: StoryBoard; waittime: real); virtual;
    begin
    end;
    function CreateStoryboard: StoryBoard;
    begin
      var sb := new StoryBoard;
      var storyboardName := 's' + sb.GetHashCode;
      MainWindow.Resources.Add(storyboardName, sb);
      var an := AnimationCompleted;
      sb.Completed += (o,e) -> begin
        MainWindow.Resources.Remove(storyboardName);
        if an<>nil then
          an;
      end;
      Result := sb;
    end;
  public 
    constructor (e: Object3D; sec: real);
    begin
      Element := e;
      Seconds := sec;
    end;
    function WhenCompleted(act: procedure): MyAnimation;
    begin
      Self.AnimationCompleted := act;
      Result := Self;
    end;

    procedure &Begin; virtual := Invoke(BeginT);
    procedure Pause := sb.Pause;
    procedure Resume := sb.Resume;
    function Duration: real; virtual := seconds;
  end;
  
  OffsetAnimation = class(MyAnimation)
  private
    x,y,z: real;
  public  
    constructor (e: Object3D; sec: real; xx,yy,zz: real);
    begin
      inherited Create(e,sec);
      (x,y,z) := (xx,yy,zz);
      Println(x,y,z)
    end;
    procedure InitAnimWait(sb: StoryBoard; waittime: real); override;
    begin
      var el := Element.transltransform;
      var ttname := 't'+el.GetHashCode;
      if not RegisterName(sb,el,ttname) then;
        //exit;
      AddDoubleAnimByName(sb,x,seconds,ttname,TranslateTransform3D.OffsetXProperty,waittime);
      AddDoubleAnimByName(sb,y,seconds,ttname,TranslateTransform3D.OffsetYProperty,waittime);
      AddDoubleAnimByName(sb,z,seconds,ttname,TranslateTransform3D.OffsetZProperty,waittime);
    end;
  end;

  ScaleAnimation = class(MyAnimation)
  private
    scale: real;
  public  
    constructor (e: Object3D; sec: real; sc: real);
    begin
      inherited Create(e,sec);
      scale := sc;
    end;
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      var sctransform := Element.scaletransform; 
      var ttname := 's'+sctransform.GetHashCode;
      if not RegisterName(sb,sctransform,ttname) then;
        //exit;
      AddDoubleAnimByName(sb,scale,seconds,ttname,ScaleTransform3D.ScaleXProperty,wait);
      AddDoubleAnimByName(sb,scale,seconds,ttname,ScaleTransform3D.ScaleYProperty,wait);
      AddDoubleAnimByName(sb,scale,seconds,ttname,ScaleTransform3D.ScaleZProperty,wait);
    end;
  end;

  RotateAnimation = class(MyAnimation)
  private
    vx,vy,vz,angle: real;
  public  
    constructor (e: Object3D; sec: real; vvx,vvy,vvz,a: real);
    begin
      inherited Create(e,sec);
      (vx,vy,vz,angle) := (vvx,vvy,vvz,a)
    end;
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      var rottransform := Element.rotatetransform_anim;
      var rot := rottransform.Rotation as AxisAngleRotation3D;
      var ttname := 'r'+rot.GetHashCode;
      if not RegisterName(sb,rot,ttname) then;
        //exit;

      rot.Angle := 0; //?
      rot.Axis := V3D(vx,vy,vz); //?
      
      var el: Object3D := Element;
      sb.Completed += (o,e) -> begin
        rottransform.Rotation := new AxisAngleRotation3D();
        el.Rotate(rot.Axis,angle); // переходит в основную матрицу
      end;

      AddDoubleAnimByName(sb,angle,seconds,ttname,AxisAngleRotation3D.AngleProperty,wait);
    end;
  end;

  RotateAtAnimation = class(MyAnimation)
  private
    vx,vy,vz,angle: real;
    center: Point3D;
  public  
    constructor (e: Object3D; sec: real; vvx,vvy,vvz,a: real; c: Point3D);
    begin
      inherited Create(e,sec);
      (vx,vy,vz,angle,center) := (vvx,vvy,vvz,a,c)
    end;
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      var rottransform := Element.rotatetransform_anim;
      rottransform.CenterX := center.x;
      rottransform.CenterY := center.y;
      rottransform.CenterZ := center.z;
      var rot := rottransform.Rotation as AxisAngleRotation3D;
      var ttname := 'r'+rot.GetHashCode;
      if not RegisterName(sb,rot,ttname) then;
        //exit;

      rot.Angle := 0; //?
      rot.Axis := V3D(vx,vy,vz); //?
      
      var el: Object3D := Element;
      sb.Completed += (o,e) -> begin
        rottransform.Rotation := new AxisAngleRotation3D();
        el.RotateAt(rot.Axis,angle,center); // переходит в основную матрицу
      end;

      AddDoubleAnimByName(sb,angle,seconds,ttname,AxisAngleRotation3D.AngleProperty,wait);
    end;
  end;
  
  GroupAnimation = class(MyAnimation)
  private
    ll: List<MyAnimation>;
  public  
    constructor (params l: array of MyAnimation);
    begin
      ll := Lst(l);
    end;
    constructor (l: List<MyAnimation>);
    begin
      ll := l;
    end;
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      foreach var l in ll do
        l.InitAnimWait(sb,wait);
    end;
    function Duration: real; override := ll.Select(l->l.Duration).Max;
  end;  

  SequenceAnimation = class(MyAnimation)
  private
    ll: List<MyAnimation>;
  public  
    constructor (params l: array of MyAnimation);
    begin
      ll := Lst(l);
    end;
    constructor (l: List<MyAnimation>);
    begin
      ll := l;
    end;
    procedure InitAnimWait(sb: StoryBoard; wait: real); override;
    begin
      var w := wait;
      foreach var l in ll do
      begin
        l.InitAnimWait(sb,w);
        w += l.Duration
      end;
    end;
    function Duration: real; override := ll.Sum(l->l.Duration);
  end;
  
function Object3D.AnimMoveTo(x,y,z,seconds: real) := new OffsetAnimation(Self,seconds,x,y,z);
function Object3D.AnimMoveOn(dx,dy,dz,seconds: real) := new OffsetAnimation(Self,seconds,x+dx,y+dy,z+dz);
function Object3D.AnimScale(sc,seconds: real) := new ScaleAnimation(Self,seconds,sc);
function Object3D.AnimRotate(vx,vy,vz,angle,seconds: real) := new RotateAnimation(Self,seconds,vx,vy,vz,angle);
function Object3D.AnimRotateAt(axis: Vector3D; angle: real; center: Point3D; seconds: real) := new RotateAtAnimation(Self,seconds,axis.X,axis.y,axis.z,angle,center);


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
function operator+(a,b: MyAnimation): MyAnimation; extensionmethod := Animate.Sequence(a,b);
function operator*(a,b: MyAnimation): MyAnimation; extensionmethod := Animate.Group(a,b);

//------------------------------ End Animation -------------------------------

type  
  SphereT = class(ObjectColored3D)
  private
    procedure SetR(r: real); 
    begin 
      var m := model as SphereVisual3D;
      Invoke(()->begin m.Radius := r end);
    end;  
    function GetR: real := Invoke&<real>(()->(model as SphereVisual3D).Radius);
    function NewVisualObject(r: real): SphereVisual3D;
    begin
      var sph := new SphereVisual3D;
      sph.Center := P3D(0, 0, 0);
      sph.Radius := r;
      Result := sph;
    end;
  protected
    function CreateObject: Object3D; override;
    begin
      Result := new SphereT(X,Y,Z,Radius,Material.Clone);
    end;
  public
    constructor(x,y,z,r: real; m: Gmaterial);
    begin
      CreateBase(NewVisualObject(r),x,y,z,m);
    end;
    constructor(x,y,z,r: real; c: GColor);
    begin
      CreateBase(NewVisualObject(r),x,y,z,c);
    end;
    property Radius: real read GetR write SetR;
    function Clone := (inherited Clone) as SphereT;
  end;
  
  EllipsoidT = class(ObjectColored3D)
  private
    procedure SetRXP(r: real) := (model as EllipsoidVisual3D).RadiusX := r;
    procedure SetRX(r: real) := Invoke(SetRXP,x);
    function GetRX: real := Invoke&<real>(()->(model as EllipsoidVisual3D).RadiusX);
    procedure SetRYP(r: real) := (model as EllipsoidVisual3D).RadiusY := r;
    procedure SetRY(r: real) := Invoke(SetRYP,x);
    function GetRY: real := Invoke&<real>(()->(model as EllipsoidVisual3D).RadiusY);
    procedure SetRZP(r: real) := (model as EllipsoidVisual3D).RadiusZ := r;
    procedure SetRZ(r: real) := Invoke(SetRZP,x);
    function GetRZ: real := Invoke&<real>(()->(model as EllipsoidVisual3D).RadiusZ);
    function NewVisualObject(rx,ry,rz: real): EllipsoidVisual3D;
    begin
      var ell := new EllipsoidVisual3D;
      ell.Center := P3D(0, 0, 0);
      ell.RadiusX := rx;
      ell.RadiusY := ry;
      ell.RadiusZ := rz;
      Result := ell;
    end;
  protected
    function CreateObject: Object3D; override := new EllipsoidT(X,Y,Z,RadiusX,RadiusY,RadiusZ,Material);
  public
    constructor(x,y,z,rx,ry,rz: real; c: GColor);
    begin
      CreateBase(NewVisualObject(rx,ry,rz),x,y,z,c);
    end;
    constructor(x,y,z,rx,ry,rz: real; m: GMaterial);
    begin
      CreateBase(NewVisualObject(rx,ry,rz),x,y,z,m);
    end;
    property RadiusX: real read GetRX write SetRX;
    property RadiusY: real read GetRY write SetRY;
    property RadiusZ: real read GetRZ write SetRZ;
    function Clone := (inherited Clone) as EllipsoidT;
  end;  
  
  BoxT = class(ObjectColored3D)
  private
    procedure SetWP(r: real) := (model as BoxVisual3D).Width := r;
    procedure SetW(r: real) := Invoke(SetWP,r);
    function GetW: real := Invoke&<real>(()->(model as BoxVisual3D).Width);
    
    procedure SetHP(r: real) := (model as BoxVisual3D).Height := r;
    procedure SetH(r: real) := Invoke(SetHP,r); 
    function GetH: real := Invoke&<real>(()->(model as BoxVisual3D).Height);

    procedure SetLP(r: real) := (model as BoxVisual3D).Length := r;
    procedure SetL(r: real) := Invoke(SetLP,r);
    function GetL: real := Invoke&<real>(()->(model as BoxVisual3D).Length);

    procedure SetSzP(r: Size3D);
    begin
      var mmm := model as BoxVisual3D;
      (mmm.Length,mmm.Width,mmm.Height) := (r.X,r.Y,r.Z);
    end;
    procedure SetSz(r: Size3D) := Invoke(SetSzP,r);
    function GetSz: Size3D := Invoke&<Size3D>(()->begin var mmm := model as BoxVisual3D; Result := Sz3D(mmm.Length,mmm.Width,mmm.Height) end);
  private  
    function NewVisualObject(l,w,h: real): BoxVisual3D;
    begin
      var bx := new BoxVisual3D;
      bx.Center := P3D(0,0,0);
      (bx.Width,bx.Height,bx.Length) := (w,h,l);
      Result := bx;
    end;
  protected  
    function CreateObject: Object3D; override := new BoxT(X,Y,Z,Length,Width,Height,Material.Clone);
  public
    constructor(x,y,z,l,w,h: real; c: GColor);
    begin
      CreateBase(NewVisualObject(l,w,h),x,y,z,c);
    end;
    constructor(x,y,z,l,w,h: real; m: GMaterial);
    begin
      CreateBase(NewVisualObject(l,w,h),x,y,z,m);
    end;
    property Length: real read GetL write SetL;
    property Width: real read GetW write SetW;
    property Height: real read GetH write SetH;
    property Size: Size3D read GetSz write SetSz;
    function Clone := (inherited Clone) as BoxT;
  end;
  
  ArrowT = class(ObjectColored3D)
  private
    procedure SetDP(r: real) := (model as ArrowVisual3D).Diameter := r;
    procedure SetD(r: real) := Invoke(SetDP,r);
    function GetD: real := Invoke&<real>(()->(model as ArrowVisual3D).Diameter);

    procedure SetLP(r: real) := (model as ArrowVisual3D).HeadLength := r;
    procedure SetL(r: real) := Invoke(SetLP,r);
    function GetL: real := Invoke&<real>(()->(model as ArrowVisual3D).HeadLength);

    procedure SetDirP(r: Vector3D) := (model as ArrowVisual3D).Direction := r;
    procedure SetDir(r: Vector3D) := Invoke(SetDirP,r);
    function GetDir: Vector3D := Invoke&<Vector3D>(()->(model as ArrowVisual3D).Direction);
  private  
    function NewVisualObject(dx,dy,dz,d,hl: real): ArrowVisual3D;
    begin
      var a := new ArrowVisual3D;
      a.HeadLength := hl;
      a.Diameter := d;
      a.Origin := P3D(0,0,0);
      Result := a;
    end;
  protected  
    function CreateObject: Object3D; override := new ArrowT(X,Y,Z,Direction.X,Direction.Y,Direction.Z,Diameter,HeadLength,Material.Clone);
  public
    constructor(x,y,z,dx,dy,dz,d,hl: real; c: GColor);
    begin
      var a := NewVisualObject(dx,dy,dz,d,hl);
      CreateBase(a,x,y,z,c);
      a.Direction := V3D(dx,dy,dz);
    end;
    constructor(x,y,z,dx,dy,dz,d,hl: real; m: GMaterial);
    begin
      var a := NewVisualObject(dx,dy,dz,d,hl);
      CreateBase(a,x,y,z,m);
      a.Direction := V3D(dx,dy,dz);
    end;
    property HeadLength: real read GetL write SetL;
    property Diameter: real read GetD write SetD;
    property Direction: Vector3D read GetDir write SetDir;
    function Clone := (inherited Clone) as ArrowT;
  end;
  
  TruncatedConeT = class(ObjectColored3D)
  private
    procedure SetHP(r: real) := (model as TruncatedConeVisual3D).Height := r;
    procedure SetH(r: real) := Invoke(SetHP,r); 
    function GetH: real := Invoke&<real>(()->(model as TruncatedConeVisual3D).Height);

    procedure SetBRP(r: real) := (model as TruncatedConeVisual3D).BaseRadius := r;
    procedure SetBR(r: real) := Invoke(SetBRP,r); 
    function GetBR: real := Invoke&<real>(()->(model as TruncatedConeVisual3D).BaseRadius);

    procedure SetTRP(r: real) := (model as TruncatedConeVisual3D).TopRadius := r;
    procedure SetTR(r: real) := Invoke(SetTRP,r); 
    function GetTR: real := Invoke&<real>(()->(model as TruncatedConeVisual3D).TopRadius);

    procedure SetTCP(r: boolean) := (model as TruncatedConeVisual3D).TopCap := r;
    procedure SetTC(r: boolean) := Invoke(SetTCP,r); 
    function GetTC: boolean := Invoke&<boolean>(()->(model as TruncatedConeVisual3D).TopCap);
  private  
    function NewVisualObject(h,baser,topr: real; sides: integer; topcap: boolean): TruncatedConeVisual3D;
    begin
      var a := new TruncatedConeVisual3D;
      a.Origin := p3D(0,0,0);
      a.BaseRadius := baser;
      a.TopRadius := topr;
      a.Height := h;
      a.TopCap := topcap;
      a.ThetaDiv := sides + 1;
      Result := a;
    end;
  protected  
    function CreateObject: Object3D; override := new TruncatedConeT(X,Y,Z,Height,BaseRadius,TopRadius,(model as TruncatedConeVisual3D).ThetaDiv-1,Topcap,Material.Clone);
  public
    constructor(x,y,z,h,baser,topr: real; sides: integer; topcap: boolean; c: GColor);
    begin
      var a := NewVisualObject(h,baser,topr,sides,topcap);
      CreateBase(a,x,y,z,c);
    end;
    constructor(x,y,z,h,baser,topr: real; sides: integer; topcap: boolean; m: GMaterial);
    begin
      var a := NewVisualObject(h,baser,topr,sides,topcap);
      CreateBase(a,x,y,z,m);
    end;
    property Height: real read GetH write SetH;
    property BaseRadius: real read GetBR write SetBR;
    property TopRadius: real read GetTR write SetTR;
    property Topcap: boolean read GetTC write SetTC;
    function Clone := (inherited Clone) as TruncatedConeT;
  end;
  
  TeapotT = class(ObjectColored3D)
  private
    procedure SetVP(v: boolean) := (model as MeshElement3D).Visible := v;
    procedure SetV(v: boolean) := Invoke(SetVP,v);
    function GetV: boolean := Invoke&<boolean>(()->(model as MeshElement3D).Visible);
  protected  
    function CreateObject: Object3D; override := new TeapotT(X,Y,Z,Material.Clone);
  public
    constructor(x,y,z: real; c: GColor);
    begin
      var a := new Teapot;
      CreateBase(a,x,y,z,c);
      Rotate(OrtX,90);
    end;
    constructor(x,y,z: real; m: GMaterial);
    begin
      var a := new Teapot;
      CreateBase(a,x,y,z,m);
      Rotate(OrtX,90);
    end;
    property Visible: boolean read GetV write SetV;
    function Clone := (inherited Clone) as TeapotT;
  end;

  CoordinateSystemT = class(Object3D)
  private
    procedure SetALP(r: real) := (model as CoordinateSystemVisual3D).ArrowLengths := r;
    procedure SetAL(r: real) := Invoke(SetALP,r); 
    function GetAL: real := Invoke&<real>(()->(model as CoordinateSystemVisual3D).ArrowLengths);
    function GetD: real := Invoke&<real>(()->((model as CoordinateSystemVisual3D).Children[0] as ArrowVisual3D).Diameter);
  protected  
    function CreateObject: Object3D; override := new CoordinateSystemT(X,Y,Z,ArrowLengths,Diameter);
  public
    constructor(x,y,z,arrlength,diameter: real);
    begin
      var a := new CoordinateSystemVisual3D;
      CreateBase0(a,x,y,z);
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

  BillboardTextT = class(Object3D)
  private
    procedure SetTP(r: string) := (model as BillboardTextVisual3D).Text := r;
    procedure SetT(r: string) := Invoke(SetTP,r); 
    function GetT: string := Invoke&<string>(()->(model as BillboardTextVisual3D).Text);

    procedure SetFSP(r: real) := (model as BillboardTextVisual3D).FontSize := r;
    procedure SetFS(r: real) := Invoke(SetFS,r); 
    function GetFS: real := Invoke&<real>(()->(model as BillboardTextVisual3D).FontSize);
  protected  
    function CreateObject: Object3D; override := new BillboardTextT(X,Y,Z,Text,FontSize);
  public
    constructor(x,y,z: real; text: string; fontsize: real);
    begin
      var a := new BillboardTextVisual3D;
      CreateBase0(a,x,y,z);
      a.Position := p3D(0,0,0);
      a.Text := text;
      a.FontSize := fontsize;
    end;
    property Text: string read GetT write SetT;
    property FontSize: real read GetFS write SetFS;
    function Clone := (inherited Clone) as BillboardTextT;
  end;

  TextT = class(Object3D)
  private
    fontname: string;
    procedure SetTP(r: string) := (model as TextVisual3D).Text := r;
    procedure SetT(r: string) := Invoke(SetTP,r); 
    function GetT: string := Invoke&<string>(()->(model as TextVisual3D).Text);

    procedure SetFSP(r: real) := (model as TextVisual3D).Height := r;
    procedure SetFS(r: real) := Invoke(SetFS,r); 
    function GetFS: real := Invoke&<real>(()->(model as TextVisual3D).Height);

    procedure SetNP(fontname: string) := (model as TextVisual3D).FontFamily := new FontFamily(fontname);
    procedure SetN(fontname: string) := Invoke(SetTP,fontname); 
    function GetN: string := Invoke&<string>(()->fontname);

    procedure SetColorP(c: GColor) := (model as TextVisual3D).Foreground := new SolidColorBrush(c);
    procedure SetColor(c: GColor) := Invoke(SetColorP,c); 
    function GetColor: GColor := Invoke&<GColor>(()->((model as TextVisual3D).Foreground as SolidColorBrush).Color);
  protected  
    function CreateObject: Object3D; override := new TextT(X,Y,Z,Text,Height,Name,Color);
  public
    constructor(x,y,z: real; text: string; height: real; fontname: string; c: Color);
    begin
      var a := new TextVisual3D;
      a.Position := p3D(0,0,0);
      a.Text := text;
      a.Height := height;
      //a.HorizontalAlignment := HorizontalAlignment.Left;
      Self.fontname := fontname;
      a.FontFamily := new FontFamily(fontname);
      a.Foreground := new SolidColorBrush(c);
      CreateBase0(a,x,y,z);
    end;
    property Text: string read GetT write SetT;
    property Height: real read GetFS write SetFS;
    property Name: string read GetN write SetN;
    property Color: GColor read GetColor write SetColor;
    function Clone := (inherited Clone) as TextT;
  end;

  RectangleT = class(ObjectColored3D)
  private
    procedure SetWP(r: real) := (model as RectangleVisual3D).Width := r;
    procedure SetW(r: real) := Invoke(SetWP,r);
    function GetW: real := Invoke&<real>(()->(model as RectangleVisual3D).Width);
    
    procedure SetLP(r: real) := (model as RectangleVisual3D).Length := r;
    procedure SetL(r: real) := Invoke(SetLP,r);
    function GetL: real := Invoke&<real>(()->(model as RectangleVisual3D).Length);

    procedure SetLDP(r: Vector3D) := (model as RectangleVisual3D).LengthDirection := r;
    procedure SetLD(r: Vector3D) := Invoke(SetLDP,r);
    function GetLD: Vector3D := Invoke&<Vector3D>(()->(model as RectangleVisual3D).LengthDirection);
   
    procedure SetNP(r: Vector3D) := (model as RectangleVisual3D).Normal := r;
    procedure SetN(r: Vector3D) := Invoke(SetNP,r);
    function GetN: Vector3D := Invoke&<Vector3D>(()->(model as RectangleVisual3D).Normal);
  protected  
    function CreateObject: Object3D; override := new RectangleT(X,Y,Z,Length,Width,Normal,LengthDirection,Material);
  public
    constructor(x,y,z,l,w: real; normal,lendirection: Vector3D; c: GColor);
    begin
      var a := new RectangleVisual3D;
      a.Origin := P3D(0,0,0);
      a.Width := w;
      a.Length := l;
      a.LengthDirection := lendirection;
      a.Normal := normal;
      CreateBase(a,x,y,z,c);
    end;
    constructor(x,y,z,l,w: real; normal,lendirection: Vector3D; m: GMaterial);
    begin
      var a := new RectangleVisual3D;
      a.Origin := P3D(0,0,0);
      a.Width := w;
      a.Length := l;
      a.LengthDirection := lendirection;
      a.Normal := normal;
      CreateBase(a,x,y,z,m);
    end;
    property Width: real read GetW write SetW;
    property Length: real read GetL write SetL;
    property LengthDirection: Vector3D read GetLD write SetLD;
    property Normal: Vector3D read GetN write SetN;
    function Clone := (inherited Clone) as RectangleT;
  end;

  FileModelT = class(Object3D)
  private
    procedure SetVP(v: boolean) := (model as MeshElement3D).Visible := v;
    procedure SetV(v: boolean) := Invoke(SetVP,v);
    function GetV: boolean := Invoke&<boolean>(()->(model as MeshElement3D).Visible);
  protected  
    function CreateObject: Object3D; override := new FileModelT(X,Y,Z,(model as FileModelVisual3D).Source);
  public
    constructor(x,y,z: real; fname: string);
    begin
      var a := new FileModelVisual3D;
      a.Source := fname;
      CreateBase0(a,x,y,z);
    end;
    property Visible: boolean read GetV write SetV;
    function Clone := (inherited Clone) as FileModelT;
  end;
  
  PipeT = class(ObjectColored3D)
  private
    procedure SetDP(r: real) := (model as PipeVisual3D).Diameter := r*2;
    procedure SetD(r: real) := Invoke(SetDP,r); 
    function GetD: real := Invoke&<real>(()->(model as PipeVisual3D).Diameter/2);

    procedure SetIDP(r: real) := (model as PipeVisual3D).InnerDiameter := r*2;
    procedure SetID(r: real) := Invoke(SetIDP,r); 
    function GetID: real := Invoke&<real>(()->(model as PipeVisual3D).InnerDiameter/2);

    procedure SetHP(r: real) := (model as PipeVisual3D).Point2 := P3D(0,0,r);
    procedure SetH(r: real) := Invoke(SetHP,r); 
    function GetH: real := Invoke&<real>(()->(model as PipeVisual3D).Point2.Z);
  protected  
    function CreateObject: Object3D; override := new PipeT(X,Y,Z,Height,Radius,InnerRadius,Material);
  public
    constructor(x,y,z,h,r,ir: real; c: GColor);
    begin
      var a := new PipeVisual3D;
      a.Diameter := r*2;
      a.InnerDiameter := ir*2;
      a.Point1 := P3D(0,0,0);
      a.Point2 := P3D(0,0,h);
      CreateBase(a,x,y,z,c);
    end;
    constructor(x,y,z,h,r,ir: real; m: GMaterial);
    begin
      var a := new PipeVisual3D;
      a.Diameter := r*2;
      a.InnerDiameter := ir*2;
      a.Point1 := P3D(0,0,0);
      a.Point2 := P3D(0,0,h);
      CreateBase(a,x,y,z,m);
    end;
    property Radius: real read GetD write SetD;
    property InnerRadius: real read GetID write SetID;
    property Height: real read GetH write SetH;
    function Clone := (inherited Clone) as PipeT;
  end;
  
type LegoVisual3D = class(MeshElement3D)
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
    HeightProperty := DependencyProperty.Register('Height', typeof (integer), typeof (LegoVisual3D), new UIPropertyMetadata(3, GeometryChanged));
    RowsProperty := DependencyProperty.Register('Raws', typeof (integer), typeof (LegoVisual3D), new UIPropertyMetadata(3, GeometryChanged));
    ColumnsProperty := DependencyProperty.Register('Columns', typeof (integer), typeof (LegoVisual3D), new UIPropertyMetadata(3, GeometryChanged));
    DivisionsProperty := DependencyProperty.Register('Divisions', typeof(integer), typeof(LegoVisual3D), new UIPropertyMetadata(48));
  end;

  property Height: integer read GetHeight write SetHeight;  
  property Rows: integer read GetRows write SetRows;  
  property Columns: integer read GetColumns write SetColumns;  
  property Divisions: integer read GetDivisions write SetDivisions;  
public
  function Tessellate(): MeshGeometry3D; override;
  const m = 1/0.008;
  const grid = 0.008*m;
  const margin = 0.0001*m;
  const wallThickness = 0.001*m;
  const plateThickness = 0.0032*m;
  const brickThickness = 0.0096*m;
  const knobHeight = 0.0018*m;
  const knobDiameter = 0.0048*m;
  const outerDiameter = 0.00651*m;
  const axleDiameter = 0.00475*m;
  const holeDiameter = 0.00485*m;
  begin
    var width := Columns*grid - margin*2;
    var length := Rows*grid - margin*2;
    var height1 := Height*plateThickness;
    var builder := new MeshBuilder(true, true);
    for var i := 0 to Columns-1 do
    for var j := 0 to Rows-1 do
    begin
      var o := new Point3D((i + 0.5)*grid, (j + 0.5)*grid, height1);
      builder.AddCone(o, new Vector3D(0, 0, 1), knobDiameter/2, knobDiameter/2, knobHeight, false, true, Divisions);
      builder.AddPipe(new Point3D(o.X, o.Y, o.Z - wallThickness), new Point3D(o.X, o.Y, wallThickness),
                      knobDiameter, outerDiameter, Divisions);
    end;

    builder.AddBox(new Point3D(Columns * 0.5 * grid, Rows * 0.5 * grid, height1 - wallThickness / 2), width, length,
                  wallThickness,
                  MeshBuilder.BoxFaces.All);
    builder.AddBox(new Point3D(margin + wallThickness / 2, Rows * 0.5 * grid, height1 / 2 - wallThickness / 2),
                   wallThickness, length, height1 - wallThickness,
                   MeshBuilder.BoxFaces.All xor MeshBuilder.BoxFaces.Top);
    builder.AddBox(
        new Point3D(Columns * grid - margin - wallThickness / 2, Rows * 0.5 * grid, height1 / 2 - wallThickness / 2),
        wallThickness, length, height1 - wallThickness,
        MeshBuilder.BoxFaces.All xor MeshBuilder.BoxFaces.Top);
    builder.AddBox(new Point3D(Columns * 0.5 * grid, margin + wallThickness / 2, height1 / 2 - wallThickness / 2),
                   width, wallThickness, height1 - wallThickness,
                   MeshBuilder.BoxFaces.All xor MeshBuilder.BoxFaces.Top);
    builder.AddBox(
        new Point3D(Columns * 0.5 * grid, Rows * grid - margin - wallThickness / 2, height1 / 2 - wallThickness / 2),
        width, wallThickness, height1 - wallThickness,
        MeshBuilder.BoxFaces.All xor MeshBuilder.BoxFaces.Top);
    Result := builder.ToMesh(false);
  end;
end;   

type
  LegoT = class(ObjectColored3D)
  private
    procedure SetWP(r: integer) := (model as LegoVisual3D).Rows := r;
    procedure SetW(r: integer) := Invoke(SetWP,r);
    function GetW: integer := Invoke&<integer>(()->(model as LegoVisual3D).Rows);
    
    procedure SetHP(r: integer) := (model as LegoVisual3D).Height := r;
    procedure SetH(r: integer) := Invoke(SetHP,r); 
    function GetH: integer := Invoke&<integer>(()->(model as LegoVisual3D).Height);

    procedure SetLP(r: integer) := (model as LegoVisual3D).Columns := r;
    procedure SetL(r: integer) := Invoke(SetLP,r);
    function GetL: integer := Invoke&<integer>(()->(model as LegoVisual3D).Columns);

    {procedure SetSzP(r: Size3D);
    begin
      var mmm := model as LegoVisual3D;
      (mmm.Columns,mmm.Rows,mmm.Height) := (r.X,r.Y,r.Z);
    end;
    procedure SetSz(r: Size3D) := Invoke(SetSzP,r);
    function GetSz: Size3D := Invoke&<Size3D>(()->begin var mmm := model as LegoVisual3D; Result := Sz3D(mmm.Length,mmm.Width,mmm.Height) end);}
  protected  
    function CreateObject: Object3D; override := new LegoT(X,Y,Z,Columns,Rows,Height,Material);
  public
    constructor(x,y,z: real; col,r,h: integer; c: GColor);
    begin
      var bx := new LegoVisual3D;
      (bx.Rows,bx.Height,bx.Columns) := (r,h,col);
      CreateBase(bx,x,y,z,c);
    end;
    constructor(x,y,z: real; col,r,h: integer; m: GMaterial);
    begin
      var bx := new LegoVisual3D;
      (bx.Rows,bx.Height,bx.Columns) := (r,h,col);
      CreateBase(bx,x,y,z,m);
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
  
  ModelTypes = (TetrahedronType,OctahedronType,HexahedronType,IcosahedronType,DodecahedronType,StellatedOctahedronType);

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
        tm.AddTriangleFan(p.Points,nil,nil);
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
    pmb.AddPanel(a, a, a, -a, a, -a, a, -a, -a);
    pmb.AddPanel(-a, a, -a, -a, -a, a, a, -a, -a);
    pmb.AddPanel(a, a, a, a, -a, -a, -a, -a, a);
    pmb.AddPanel(a, a, a, -a, -a, a, -a, a, -a);
  end;
OctahedronType:
  begin
    //a := 1.0 / (2 * Sqrt(2));
    var b := 0.5 * (2 * Sqrt(2)) * a;// * 2 * a;
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
    pmb.AddPanel(-a, -a, a, a, -a, a, a, -a, -a, -a, -a, -a);
    pmb.AddPanel(-a, a, -a, -a, a, a, -a, -a, a, -a, -a, -a);
    pmb.AddPanel(-a, a, a, a, a, a, a, -a, a, -a, -a, a);
    pmb.AddPanel(a, a, -a, a, a, a, -a, a, a, -a, a, -a);
    pmb.AddPanel(a, -a, a, a, a, a, a, a, -a, a, -a, -a);
    pmb.AddPanel(a, -a, -a, a, a, -a, -a, a, -a, -a, -a, -a);
  end;
IcosahedronType:
  begin
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
end;
  Result := pmb.ToMeshGeometry3D;
end;

type 
  PlatonicAbstractVisual3D = abstract class(MeshElement3D)
  private    
    fa: real;
    procedure SetA(value: real);
    begin
      fa := value;
      OnGeometryChanged;
    end;
  public
    constructor(Length: real);
    begin
      inherited Create;
      Self.Length := Length;
    end;
    property Length: real read fa write SetA;
  end;
  IcosahedronVisual3D = class(PlatonicAbstractVisual3D)
    public function Tessellate(): MeshGeometry3D; override := CreateModel(ModelTypes.IcosahedronType,Length);
  end;   
  DodecahedronVisual3D = class(PlatonicAbstractVisual3D)
    public function Tessellate(): MeshGeometry3D; override := CreateModel(ModelTypes.DodecahedronType,Length);
  end;   
  TetrahedronVisual3D = class(PlatonicAbstractVisual3D)
    public function Tessellate(): MeshGeometry3D; override := CreateModel(ModelTypes.TetrahedronType,Length);
  end;   
  OctahedronVisual3D = class(PlatonicAbstractVisual3D)
    public function Tessellate(): MeshGeometry3D; override := CreateModel(ModelTypes.OctahedronType,Length);
  end;   
  
  PlatonicAbstractT = class(ObjectColored3D)
  private
    procedure SetLengthP(r: real) := (model as PlatonicAbstractVisual3D).Length := r;
    procedure SetLength(r: real) := Invoke(SetLengthP,r); 
    function GetLength: real := Invoke&<real>(()->(model as PlatonicAbstractVisual3D).Length);
  public
    property Length: real read GetLength write SetLength;
  end;
  
  IcosahedronT = class(PlatonicAbstractT)
  protected  
    function CreateObject: Object3D; override := new IcosahedronT(X,Y,Z,Length,Material);
  public 
    constructor(x,y,z,Length: real; c: GColor);
    begin
      CreateBase(new IcosahedronVisual3D(Length),x,y,z,c);
    end;
    constructor(x,y,z,Length: real; m: GMaterial);
    begin
      CreateBase(new IcosahedronVisual3D(Length),x,y,z,m);
    end;
    function Clone := (inherited Clone) as IcosahedronT;
  end;
  
  DodecahedronT = class(PlatonicAbstractT)
  protected  
    function CreateObject: Object3D; override := new DodecahedronT(X,Y,Z,Length,Material);
  public 
    constructor(x,y,z,Length: real; c: GColor);
    begin
      CreateBase(new DodecahedronVisual3D(Length),x,y,z,c);
    end;
    constructor(x,y,z,Length: real; m: GMaterial);
    begin
      CreateBase(new DodecahedronVisual3D(Length),x,y,z,m);
    end;
    function Clone := (inherited Clone) as DodecahedronT;
  end;
  
  TetrahedronT = class(PlatonicAbstractT)
  protected  
    function CreateObject: Object3D; override := new TetrahedronT(X,Y,Z,Length,Material);
  public 
    constructor(x,y,z,Length: real; c: GColor);
    begin
      CreateBase(new TetrahedronVisual3D(Length),x,y,z,c);
    end;
    constructor(x,y,z,Length: real; m: GMaterial);
    begin
      CreateBase(new TetrahedronVisual3D(Length),x,y,z,m);
    end;
    function Clone := (inherited Clone) as TetrahedronT;
  end;
  
  OctahedronT = class(PlatonicAbstractT)
  protected  
    function CreateObject: Object3D; override := new OctahedronT(X,Y,Z,Length,Material);
  public 
    constructor(x,y,z,Length: real; c: GColor);
    begin
      CreateBase(new OctahedronVisual3D(Length),x,y,z,c);
    end;
    constructor(x,y,z,Length: real; m: GMaterial);
    begin
      CreateBase(new OctahedronVisual3D(Length),x,y,z,m);
    end;
    function Clone := (inherited Clone) as OctahedronT;
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
      textureCoord := new Point(u,2*v);
      Result := P3D(u,v,0.2*u*u+sin(u*v)+2);
    end;
  end;

type
  AnyT = class(ObjectColored3D)
    constructor(x,y,z: real; c: GColor);
    begin
      var a := new MeshVisual3D;
      //var a := new TerrainVisual3D;
      //a.Content := (new SphereVisual3D()).Model;
      //a.Text := 'PascalABC';
      {var a := new LinesVisual3D;
      
      var aa := 1;
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

      CreateBase0(a,x,y,z);
    end;
  end;
  
{procedure ProbaP;
begin
  //var ms := new TorusMesh();
  //ms.Radius := 4;
  //var ms := new IcosahedronMesh();
  //ms.Geometry.
  //var mg3d := ms.Geometry;
  
  var blueMaterial := MaterialHelper.CreateMaterial(Colors.Bisque);
  

  var g3d := new GeometryModel3D(mg3d, blueMaterial);
  var modelGroup := new Model3DGroup();
  modelGroup.Children.Add(g3d);
  
  var model := new ModelVisual3D();
  model.Content := modelGroup;|
  hvp.Children.Add(model);
end;

procedure Proba := Invoke(ProbaP);}

function Group(x,y,z: integer): GroupT := Invoke&<GroupT>(()->GroupT.Create(x,y,z));
function Group(p: Point3D): GroupT := Invoke&<GroupT>(()->GroupT.Create(p.x,p.y,p.z));
function Group: GroupT := Invoke&<GroupT>(()->GroupT.Create(0,0,0));
function Group(params lst: array of Object3D): GroupT := Invoke&<GroupT>(()->GroupT.Create(0,0,0,lst));
function Group(en: sequence of Object3D): GroupT := Invoke&<GroupT>(()->GroupT.Create(0,0,0,en));

function Sphere(x,y,z,r: real; m: GMaterial): SphereT := Invoke&<SphereT>(()->SphereT.Create(x,y,z,r,m));
function Sphere(x,y,z,r: real; c: Color): SphereT := Invoke&<SphereT>(()->SphereT.Create(x,y,z,r,c));
function Sphere(center: Point3D; r: real; m: GMaterial) := Sphere(center.x,center.y,center.z,r,m);
function Sphere(center: Point3D; r: real; c: Color) := Sphere(center.x,center.y,center.z,r,c);

function Ellipsoid(x,y,z,rx,ry,rz: real; c: Color): EllipsoidT := Invoke&<EllipsoidT>(()->EllipsoidT.Create(x,y,z,rx,ry,rz,c));
function Ellipsoid(center: Point3D; rx,ry,rz: real; c: Color): EllipsoidT := Ellipsoid(center.x,center.y,center.z,rx,ry,rz,c);

function Box(x,y,z,l,w,h: real; c: Color): BoxT := Invoke&<BoxT>(()->BoxT.Create(x,y,z,l,w,h,c));
function Box(center: Point3D; sz: Size3D; c: Color): BoxT := Invoke&<BoxT>(()->BoxT.Create(center.x,center.y,center.z,sz.X,sz.Y,sz.z,c));
function Cube(x,y,z,w: real; c: Color) := Box(x,y,z,w,w,w,c);
function Cube(center: Point3D; w: real; c: Color) := Box(center.x,center.y,center.z,w,w,w,c);

const 
  arhl = 3;
  ard  = 0.2;

function Arrow(x,y,z,vx,vy,vz,diameter,hl: real; c: Color): ArrowT := Invoke&<ArrowT>(()->ArrowT.Create(x,y,z,vx,vy,vz,diameter,hl,c));
function Arrow(x,y,z,vx,vy,vz,diameter: real; c: Color) := Arrow(x,y,z,vx,vy,vz,diameter,arhl,c);
function Arrow(x,y,z,vx,vy,vz: real; c: Color): ArrowT := Arrow(x,y,z,vx,vy,vz,ard,arhl,c);
function Arrow(p: Point3D; v: Vector3D; diameter,hl: real; c: Color) := Arrow(p.x,p.y,p.z,v.X,v.Y,v.Z,diameter,hl,c);
function Arrow(p: Point3D; v: Vector3D; diameter: real; c: Color) := Arrow(p.x,p.y,p.z,v.X,v.Y,v.Z,diameter,arhl,c);
function Arrow(p: Point3D; v: Vector3D; c: Color) := Arrow(p.x,p.y,p.z,v.X,v.Y,v.Z,ard,arhl,c);

function TruncatedConeAux(x,y,z,height,baseradius,topradius: real; sides: integer; topcap: boolean; c: GColor) := 
  Invoke&<TruncatedConeT>(()->TruncatedConeT.Create(x,y,z,height,baseradius,topradius,sides,topcap,c));
  
{function Pyramid(x,y,z,height,baseradius,topradius: real; sides: integer; topcap: boolean; c: GColor) 
  := TruncatedConeAux(x,y,z,height,baseradius,topradius,sides,topcap,c); 
function Pyramid(x,y,z,height,baseradius,topradius: real; sides: integer; c: GColor) 
  := TruncatedConeAux(x,y,z,height,baseradius,topradius,sides,True,c); 
function Pyramid(p: Point3D; height,baseradius,topradius: real; sides: integer; topcap: boolean; c: GColor) 
  := TruncatedConeAux(p.x,p.y,p.z,height,baseradius,topradius,sides,topcap,c); 
function Pyramid(p: Point3D; height,baseradius,topradius: real; sides: integer; c: GColor) 
  := TruncatedConeAux(p.x,p.y,p.z,height,baseradius,topradius,sides,True,c); }

const maxsides = 37;
  
function TruncatedCone(x,y,z,height,baseradius,topradius: real; topcap: boolean; c: GColor) 
  := TruncatedConeAux(x,y,z,height,baseradius,topradius,maxsides,topcap,c); 
function TruncatedCone(x,y,z,height,baseradius,topradius: real; c: GColor) 
  := TruncatedCone(x,y,z,height,baseradius,topradius,True,c);
function TruncatedCone(p: Point3D; height,baseradius,topradius: real; topcap: boolean; c: GColor) 
  := TruncatedCone(p.x,p.y,p.z,height,baseradius,topradius,topcap,c);
function TruncatedCone(p: Point3D; height,baseradius,topradius: real; c: GColor) 
  := TruncatedCone(p.x,p.y,p.z,height,baseradius,topradius,True,c);

{function Prism(x,y,z,height,radius: real; sides: integer; topcap: boolean; c: GColor) 
  := TruncatedConeAux(x,y,z,height,radius,radius,sides,topcap,c);
function Prism(x,y,z,height,radius: real; sides: integer; c: GColor) 
  := TruncatedConeAux(x,y,z,height,radius,radius,sides,True,c);
function Prism(p: Point3D; height,radius: real; sides: integer; topcap: boolean; c: GColor) 
  := TruncatedConeAux(p.x,p.y,p.z,height,radius,radius,sides,topcap,c);
function Prism(p: Point3D; height,radius: real; sides: integer; c: GColor) 
  := TruncatedConeAux(p.x,p.y,p.z,height,radius,radius,sides,True,c);}

function Cylinder(x,y,z,height,radius: real; topcap: boolean; c: GColor): TruncatedConeT := TruncatedCone(x,y,z,height,radius,radius,topcap,c);
function Cylinder(x,y,z,height,radius: real; c: GColor) := Cylinder(x,y,z,height,radius,True,c);
function Cylinder(p: Point3D; height,radius: real; topcap: boolean; c: GColor) := Cylinder(p.x,p.y,p.z,height,radius,topcap,c);
function Cylinder(p: Point3D; height,radius: real; c: GColor) := Cylinder(p.x,p.y,p.z,height,radius,True,c);

function Tube(x,y,z,height,radius,innerradius: real; c: GColor): PipeT 
  := Invoke&<PipeT>(()->PipeT.Create(x,y,z,height,radius,innerradius,c));
function Tube(p: Point3D; height,radius,innerradius: real; c: GColor) 
  := Tube(p.x,p.y,p.z,height,radius,innerradius,c);

function Cone(x,y,z,height,radius: real; c: GColor): TruncatedConeT := TruncatedCone(x,y,z,height,radius,0,True,c);
function Cone(p: Point3D; height,radius: real; c: GColor) := TruncatedCone(p.x,p.y,p.z,height,radius,0,True,c);

function Teapot(x,y,z: real; c: GColor): TeapotT := Invoke&<TeapotT>(()->TeapotT.Create(x,y,z,c));
function Teapot(p: Point3D; c: GColor) := Teapot(p.x,p.y,p.z,c);

function BillboardText(x,y,z: real; text: string; fontsize: real): BillboardTextT := Invoke&<BillboardTextT>(()->BillboardTextT.Create(x,y,z,text,fontsize));
function BillboardText(p: Point3D; text: string; fontsize: real) := BillboardText(P.x,p.y,p.z,text,fontsize);
function BillboardText(p: Point3D; text: string) := BillboardText(P.x,p.y,p.z,text,12);
function BillboardText(x,y,z: real; text: string) := BillboardText(x,y,z,text,12);

function CoordinateSystem(arrowslength,diameter: real): CoordinateSystemT 
  := Invoke&<CoordinateSystemT>(()->CoordinateSystemT.Create(0,0,0,arrowslength,diameter));
function CoordinateSystem(arrowslength: real) := CoordinateSystem(arrowslength,arrowslength/10);

function Text3D(x,y,z: real; text: string; height: real; fontname: string; c: Color): TextT := Invoke&<TextT>(()->TextT.Create(x,y,z,text,height,fontname,c));
function Text3D(p: Point3D; text: string; height: real; fontname: string; c: Color) := Text3D(P.x,p.y,p.z,text,height,fontname,c);
function Text3D(x,y,z: real; text: string; height: real; c: Color) := Text3D(x,y,z,text,height,'Arial',c);
function Text3D(p: Point3D; text: string; height: real; c: Color) := Text3D(p.x,p.y,p.z,text,height,'Arial',c);
function Text3D(x,y,z: real; text: string; height: real) := Text3D(x,y,z,text,height,'Arial',Colors.Black);
function Text3D(p: Point3D; text: string; height: real) := Text3D(p.x,p.y,p.z,text,height,'Arial',Colors.Black);

function Rectangle3D(x,y,z,l,w: real; normal,lendirection: Vector3D; c: GColor): RectangleT 
  := Invoke&<RectangleT>(()->RectangleT.Create(x,y,z,l,w,normal,lendirection,c));
function Rectangle3D(p: Point3D; l,w: real; normal,lendirection: Vector3D; c: GColor) 
  := Rectangle3D(p.x,p.y,p.z,l,w,normal,lendirection,c);
function Rectangle3D(x,y,z,l,w: real; normal: Vector3D; c: GColor)
  := Rectangle3D(x,y,z,l,w,normal,v3d(1,0,0),c); 
function Rectangle3D(x,y,z,l,w: real; c: GColor)
  := Rectangle3D(x,y,z,l,w,v3d(0,0,1),v3d(1,0,0),c); 
function Rectangle3D(p: Point3D; l,w: real; normal: Vector3D; c: GColor)
  := Rectangle3D(p.x,p.y,p.z,l,w,normal,v3d(1,0,0),c); 
function Rectangle3D(p: Point3D; l,w: real; c: GColor)
  := Rectangle3D(p.x,p.y,p.z,l,w,v3d(0,0,1),v3d(1,0,0),c); 

/// Загружает модель из файла .obj, .3ds, .lwo, .objz, .stl, .off
function FileModel3D(x,y,z: real; fname: string): FileModelT 
  := Invoke&<FileModelT>(()->FileModelT.Create(x,y,z,fname));
function FileModel3D(p: Point3D; fname: string) :=  FileModel3D(p.x,p.y,p.z,fname);


function Lego(x,y,z: real; col,r,h: integer; c: Color): LegoT := Invoke&<LegoT>(()->LegoT.Create(x,y,z,col,r,h,c));

function Icosahedron(x,y,z,Length: real; c: Color): IcosahedronT := Invoke&<IcosahedronT>(()->IcosahedronT.Create(x,y,z,Length,c));
function Dodecahedron(x,y,z,Length: real; c: Color): DodecahedronT := Invoke&<DodecahedronT>(()->DodecahedronT.Create(x,y,z,Length,c));
function Tetrahedron(x,y,z,Length: real; c: Color): TetrahedronT := Invoke&<TetrahedronT>(()->TetrahedronT.Create(x,y,z,Length,c));
function Octahedron(x,y,z,Length: real; c: Color): OctahedronT := Invoke&<OctahedronT>(()->OctahedronT.Create(x,y,z,Length,c));

function Any(x,y,z: real; c: Color): AnyT := Invoke&<AnyT>(()->AnyT.Create(x,y,z,c));

procedure WindowTypeSetLeftP(l: real) := MainWindow.Left := l;
procedure WindowType.SetLeft(l: real) := Invoke(WindowTypeSetLeftP,l);

function WindowTypeGetLeftP := MainWindow.Left;
function WindowType.GetLeft := Invoke&<real>(WindowTypeGetLeftP);

procedure WindowTypeSetTopP(t: real) := MainWindow.Top := t;
procedure WindowType.SetTop(t: real) := Invoke(WindowTypeSetTopP,t);

function WindowTypeGetTopP := MainWindow.Top;
function WindowType.GetTop := Invoke&<real>(WindowTypeGetTopP);

procedure WindowTypeSetWidthP(w: real) := MainWindow.Width := w + wplus;
procedure WindowType.SetWidth(w: real) := Invoke(WindowTypeSetWidthP,w);

function WindowTypeGetWidthP := MainWindow.Width - wplus;
function WindowType.GetWidth := Invoke&<real>(WindowTypeGetWidthP);

procedure WindowTypeSetHeightP(h: real) := MainWindow.Height := h + hplus;
procedure WindowType.SetHeight(h: real) := Invoke(WindowTypeSetHeightP,h);

function WindowTypeGetHeightP := MainWindow.Height - hplus;
function WindowType.GetHeight := Invoke&<real>(WindowTypeGetHeightP);

procedure WindowTypeSetCaptionP(c: string) := MainWindow.Title := c;
procedure WindowType.SetCaption(c: string) := Invoke(WindowTypeSetCaptionP,c);

function WindowTypeGetCaptionP := MainWindow.Title;
function WindowType.GetCaption := Invoke&<string>(WindowTypeGetCaptionP);

procedure WindowTypeSetSizeP(w, h: real);
begin
  WindowTypeSetWidthP(w);
  WindowTypeSetHeightP(h);
end;
procedure WindowType.SetSize(w, h: real) := Invoke(WindowTypeSetSizeP,w,h);

procedure WindowTypeSetPosP(l, t: real);
begin
  WindowTypeSetLeftP(l);
  WindowTypeSetTopP(t);
end;
procedure WindowType.SetPos(l, t: real) := Invoke(WindowTypeSetPosP,l,t);

procedure WindowType.Close := Invoke(MainWindow.Close);

procedure WindowTypeMinimizeP := MainWindow.WindowState := WindowState.Minimized;
procedure WindowType.Minimize := Invoke(WindowTypeMinimizeP);

procedure WindowTypeMaximizeP := MainWindow.WindowState := WindowState.Maximized;
procedure WindowType.Maximize := Invoke(WindowTypeMaximizeP);

procedure WindowTypeNormalizeP := MainWindow.WindowState := WindowState.Normal;
procedure WindowType.Normalize := Invoke(WindowTypeNormalizeP);

procedure WindowTypeCenterOnScreenP := MainWindow.WindowStartupLocation := WindowStartupLocation.CenterScreen;
procedure WindowType.CenterOnScreen := Invoke(WindowTypeCenterOnScreenP);

function WindowType.Center := new Point(Width/2,Height/2);

function WindowType.ClientRect := Rect(0,0,Window.Width,Window.Height);

var mre := new ManualResetEvent(false);

procedure InitWPF0;
begin
  app := new Application;
  app.Dispatcher.UnhandledException += (o,e) -> Println(e);
  MainWindow := new GWindow;

  Window := new WindowType;
  Camera := new CameraType;

  var g := new Grid;
  MainWindow.Content := g;
  hvp := new HelixViewport3D();
  g.Children.Add(hvp);
  //hvp.ShowTriangleCountInfo := true;;
  //hvp.ShowCameraInfo := True;
  hvp.ZoomExtentsWhenLoaded := True;
  //hvp.Background := Brushes.Black;
  
  hvp.ShowCoordinateSystem := True;
  
  hvp.Children.Add(new DefaultLights);
  
  gvl := new GridLinesVisual3D();

  //gvl.Fill := Brushes.Aqua;
  //gvl.Visible := False;
  gvl.Width := 12; gvl.Length := 12; 
  gvl.MinorDistance := 1;
  gvl.MajorDistance := 1;
  gvl.Thickness := 0.02;
  hvp.Children.Add(gvl);
  
  NameScope.SetNameScope(MainWindow, new NameScope());
  MainWindow.Title := '3D графика';
  MainWindow.WindowStartupLocation := WindowStartupLocation.CenterScreen;
  MainWindow.Width := 640;
  MainWindow.Height := 480;
  MainWindow.Closed += procedure(sender,e) -> begin Halt; end;
  MainWindow.KeyDown += SystemOnKeyDown;
  MainWindow.KeyUp += SystemOnKeyUp;
  
  View3D := new View3DT;
  
  // hvp.PreviewMouseDown += (o,e)-> begin e.Handled := True; end; - крутой способ отключить перехват клавиатуры 
  //hvp.Camera.Position := P3D(12,16,20);

  mre.Set();
  
  app.Run(MainWindow);
end;

initialization
  MainFormThread := new System.Threading.Thread(InitWPF0);
  MainFormThread.SetApartmentState(ApartmentState.STA);
  MainFormThread.Start;
  
  mre.WaitOne; // Основная программа не начнется пока не будут инициализированы все компоненты приложения

finalization  
end.