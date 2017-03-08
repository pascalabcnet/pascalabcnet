// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

///Модуль реализует векторные графические объекты с возможностью масштабирования, наложения друг на друга, 
///создания составных графических объектов и многократного их вложения друг в друга. 
///Каждый векторный графический объект перерисовывает себя при перемещении, изменении размеров 
///и частичном перекрытии другими объектами.
unit ABCObjects;

//{$apptype windows}
{$reference 'System.Windows.Forms.dll'}
{$reference 'System.Drawing.dll'}
{$gendoc true}

interface

uses System.Drawing, GraphABC;

type
  Color = GraphABC.Color;
  GColor = GraphABC.Color;
  ContainerABC = class;
  GRectangle = System.Drawing.Rectangle;
  
  /// Базовый класс для всех графических объектов
  ObjectABC = class
  private
    fx,fy: integer;
    fw,fh: integer;
    col,fcol: GColor;
    vis,txtvis: boolean;
    txt,fname: string;
    fstyle: FontStyleType;
    txtscale: real;
    ow: ContainerABC;
    _dx,_dy: integer;
    procedure SetCoords(x,y: integer); 
  protected
  // InternalDraw: низкоуровневая процедура (не вызывается пользователем). Просто вызывает Draw(fx,fy)
    procedure InternalDraw;
    procedure SetVis(v: boolean);
    procedure SetColor(cl: GColor);
    procedure SetOwner(o: ContainerABC);
    procedure SetX(x: integer);
    procedure SetY(y: integer);
    procedure ObjectABCSetSize(Width,Height: integer); // internal
    procedure SetWidth(Width: integer); virtual;
    procedure SetHeight(Height: integer); virtual;
    procedure SetText(t: string); virtual;
    procedure SetTxtVis(b: boolean);
    procedure SetTextScale(r: real);
    procedure SetFontName(name: string);
    procedure SetFontStyle(fs: FontStyleType);
    procedure SetFontColor(fc: GColor);
    
    procedure SetNum(n: integer);
    function GetNum: integer;
    procedure SetRealNum(r: real);
    function GetRealNum: real;

    procedure SetCenter(p: Point);
    function GetCenter: Point;
    procedure SetPosition(p: Point);
    function GetPosition: Point;
    
    procedure CalcOwnerOffset(var l,t: integer);
    procedure DrawAfterChangeBounds(oldBounds,newBounds: GRectangle);
    procedure DrawText(x,y: integer; g: Graphics);
    procedure Init(x,y,w,h: integer; cl: GColor);
    procedure InitBy(g: ObjectABC);
    // Draw: переопределяется в подклассах. Рисует объект на текущей канве
    procedure Draw(x,y: integer; g: Graphics); virtual; begin end;
    procedure Draw(x,y: integer); 
  public
    /// Создает графический объект размера (w,h) цвета cl с координатами левого верхнего угла (x,y)
    constructor Create(x,y,w,h: integer; cl: GColor := clWhite);
    /// Создает графический объект - копию объекта g
    constructor Create(g: ObjectABC);
    /// Уничтожает графический объект
    destructor Destroy;
        // Redraw: низкоуровневая процедура (не вызывается пользователем).
        // Обновляет изображение объекта на экране, вызывая drawRect(Bounds).
        // Вызывается в ситуациях, когда границы объекта не изменяются (напр., смена цвета)
        // или увеличиваются (добавление нового объекта к фигуре)
    procedure Redraw;
        // RedrawNow - вызывает перерисовку объекта, несмотря на LockDrawing
    procedure RedrawNow;
    /// Перемещает левый верхний угол графического объекта к точке (x,y)
    procedure MoveTo(x,y: integer);
    /// Перемещает графический объект на вектор (a,b)
    procedure MoveOn(a,b: integer); 
    /// Перемещает графический объект на вектор, задаваемый свойствами dx,dy
    procedure Move; virtual;
    /// Масштабирует графический объект в f раз (f>1 - увеличение, 0<f<1 - уменьшение)
    procedure Scale(f: real); virtual;
    /// Переносит графический объект на передний план
    procedure ToFront;
    /// Переносит графический объект на задний план
    procedure ToBack;
    /// Возвращает прямоугольник, определяющий границы графического объекта
    function Bounds: GRectangle;
    /// Возвращает True, если точка (x,y) находится внутри графического объекта, и False в противном случае
    function PtInside(x,y: integer): boolean; virtual;
    /// Возвращает True, если изображение данного графического объекта пересекается с изображением 
    ///графического объекта g, и False в противном случае. Белый цвет считается прозрачным и не принадлежащим объекту
    function Intersect(g: ObjectABC): boolean;
    /// Возвращает True, если прямоугольник графического объекта пересекается прямоугольником r, и False в противном случае
    function IntersectRect(r: GRectangle): boolean;
    /// Отступ графического объекта от левого края 
    property Left: integer read fx write SetX;
    /// Отступ графического объекта от верхнего края 
    property Top: integer read fy write SetY;
    /// Ширина графического объекта 
    property Width: integer read fw write SetWidth;
    /// Высота  графического объекта
    property Height: integer read fh write SetHeight;
    ///x-координата вектора перемещения объекта при вызове метода Move.
    ///По умолчанию установлено в 0. Для неподвижных объектов может быть использовано 
    ///для хранения любой дополнительной информации, связанной с объектом
    property dx: integer read _dx write _dx;
    /// y-координата вектора перемещения объекта при вызове метода Move.
    ///По умолчанию установлено в 0. Для неподвижных объектов может быть использовано 
    ///для хранения любой дополнительной информации, связанной с объектом
    property dy: integer read _dy write _dy;
    /// Центр графического объекта
    property Center: Point read GetCenter write SetCenter;
    /// Левый верхний угол графического объекта
    property Position: Point read GetPosition write SetPosition;
    /// Видим ли  графический объект
    property Visible: boolean read vis write SetVis;
    /// Цвет графического объекта
    property Color: GColor read col write SetColor;
    /// Цвет шрифта графического объекта
    property FontColor: GColor read fcol write SetFontColor;

    /// Текст внутри графического объекта
    property Text: string read txt write SetText;
    /// Видимость текста внутри графического объекта
    property TextVisible: boolean read txtvis write SetTxtVis;
    /// Масштаб текста относительно размеров графического объекта,  0<=TextScale<=1. 
    ///При TextScale=1 текст занимает всю ширину или высоту объекта. По умолчанию TextScale=0.8
    property TextScale: real read txtscale write SetTextScale;
    /// Имя шрифта для вывода свойства Text
    property FontName: string read fname write SetFontName;
    /// Стиль шрифта для вывода свойства Text
    property FontStyle: FontStyleType read fstyle write SetFontStyle;
    /// Целое число, выводимое в центре графического объекта. Для вывода используется свойство Text
    property Number: integer read GetNum write SetNum;
    /// Вещественное число, выводимое в центре графического объекта. Для вывода используется свойство Text.
    ///Вещественное число выводится с одним знаком после десятичной точки
    property RealNumber: real read GetRealNum write SetRealNum;
    ///Владелец графического объекта, ответственный также за перерисовку графического объекта внутри себя (по умолчанию nil) 
    property Owner: ContainerABC read ow write SetOwner;
    /// Возвращает клон графического объекта
    function Clone0: ObjectABC; virtual;
    begin 
      Result := nil 
    end;
    /// Возвращает клон графического объекта
    function Clone: ObjectABC; 
    begin 
      Result := Clone0 
    end;
  end;
    
  /// Базовый класс для всех замкнутых графических объектов
  BoundedObjectABC = class(ObjectABC)
  private
    bcol: GColor;
    bw: integer;
    fil,bor: boolean;
    procedure SetBColor(cl: GColor);
    procedure SetBW(w: integer);
    function GetBW: integer;
    procedure SetFilled(f: boolean);
    procedure SetBordered(b: boolean);
  protected
    procedure Init(x,y,w,h: integer; cl: GColor);
    procedure InitBy(g: BoundedObjectABC);
    /// Устанавливает атрибуты пера и кисти перед рисованием
    procedure SetDrawSettings;
  public
    /// Создает замкнутый графический объект размера (w,h) цвета cl с координатами левого верхнего угла (x,y)
    constructor Create(x,y,w,h: integer; cl: GColor := clWhite);
    /// Создает замкнутый графический объект - копию объекта g
    constructor Create(g: BoundedObjectABC);
    /// Цвет границы 
    property BorderColor: GColor read bcol write SetBColor;
    /// Ширина границы
    property BorderWidth: integer read GetBW write SetBW; 
      // ширина границы при рисовании уходит внутрь, т.е. при ее изменении не меняются размеры объекта
    /// Заполнена ли внутренность объекта (по умолчанию True)
    property Filled: boolean read fil write SetFilled;
    /// Имеет ли объект границу (по умолчанию True)
    property Bordered: boolean read bor write SetBordered;
  end;

  /// Класс графических объектов "Прямоугольник"
  RectangleABC = class(BoundedObjectABC)
  protected
    procedure Draw(x,y: integer; g: Graphics); override;
  public
    /// Создает прямоугольник размера (w,h) цвета cl с координатами левого верхнего угла (x,y)
    constructor Create(x,y,w,h: integer; cl: GColor := clWhite);
    /// Создает прямоугольник - копию прямоугольника g
    constructor Create(g: RectangleABC);
    /// Возвращает клон прямоугольника
    function Clone0: ObjectABC; override;
    /// Возвращает клон прямоугольника
    function Clone: RectangleABC; 
  end;
  
  /// Класс графических объектов "Квадрат"
  SquareABC = class(RectangleABC)
  protected
    procedure SetWidth(Width: integer); override;
    procedure SetHeight(Height: integer); override;
  public
    /// Создает квадрат размера w цвета cl с координатами левого верхнего угла (x,y)
    constructor Create(x,y,w: integer; cl: GColor := clWhite);
    /// Создает квадрат - копию квадрата g
    constructor Create(g: SquareABC);
    /// Масштабирует квадрат в f раз (f>1 - увеличение, 0<f<1 - уменьшение)
    procedure Scale(f: real); override;
    /// Возвращает клон квадрата
    function Clone0: ObjectABC; override; 
    /// Возвращает клон квадрата
    function Clone: SquareABC; 
  end;

  /// Класс графических объектов "Эллипс"
  EllipseABC = class(BoundedObjectABC)
  protected
    procedure Draw(x,y: integer; g: Graphics); override;
  public
    /// Создает эллипс размера (w,h) цвета cl с координатами левого верхнего угла (x,y)
    constructor Create(x,y,w,h: integer; cl: GColor := clWhite);
    /// Создает эллипс - копию эллипса g
    constructor Create(g: EllipseABC);
    /// Возвращает клон эллипса
    function Clone0: ObjectABC; override; 
    /// Возвращает клон эллипса
    function Clone: EllipseABC; 
  end;
  
  /// Класс графических объектов "Круг"
  CircleABC = class(EllipseABC)
  protected
    procedure SetWidth(Width: integer); override;
    procedure SetHeight(Height: integer); override;
    procedure SetRadius(r: integer);
    function GetRadius: integer;
  public
    /// Создает круг радиуса r цвета cl с координатами центра (x,y)
    constructor Create(x,y,r: integer; cl: GColor := clWhite); 
    /// Создает круг - копию круга g
    constructor Create(g: CircleABC);
    /// Масштабирует круг в f раз (f>1 - увеличение, 0<f<1 - уменьшение)
    procedure Scale(f: real); override;
    /// Возвращает клон круга
    function Clone0: ObjectABC; override; 
    /// Возвращает клон круга
    function Clone: CircleABC; 
    /// Радиус круга
    property Radius: integer read GetRadius write SetRadius;
  end;
  
  /// Класс графических объектов "Прямоугольник со скругленными краями"
  RoundRectABC = class(BoundedObjectABC)
  private
    r: integer;
    procedure SetRadius(rr: integer);
  protected  
    procedure Draw(x,y: integer; g: Graphics); override;
    procedure Init(x,y,w,h,rr: integer; cl: GColor); 
    procedure InitBy(g: RoundRectABC); 
  public
    /// Создает прямоугольник со скругленными краями размера (w,h), цветом cl, радиусом скругления r и координатами левого верхнего угла (x,y)
    constructor Create(x,y,w,h,rr: integer; cl: GColor := clWhite); 
    /// Создает прямоугольник со скругленными краями - копию прямоугольника со скругленными краями g
    constructor Create(g: RoundRectABC);
    /// Радиус скругления углов
    property Radius: integer read r write SetRadius;
    /// Возвращает клон прямоугольника со скругленными краями
    function Clone0: ObjectABC; override;
    /// Возвращает клон прямоугольника со скругленными краями
    function Clone: RoundRectABC; 
  end;

  /// Класс графических объектов "Квадрат со скругленными краями"
  RoundSquareABC = class(RoundRectABC)
  protected
    procedure SetWidth(Width: integer); override;
    procedure SetHeight(Height: integer); override;
  public
    /// Создает квадрат со скругленными краями размера w, цвета cl с радиусом скругления r и координатами левого верхнего угла (x,y)
    constructor Create(x,y,w,r: integer; cl: GColor := clWhite); 
    /// Создает квадрат со скругленными краями - копию квадрата со скругленными краями g
    constructor Create(g: RoundSquareABC);
    /// Масштабирует квадрат в f раз (f>1 - увеличение, 0<f<1 - уменьшение)
    procedure Scale(f: real); override;
    /// Возвращает клон квадрата со скругленными краями
    function Clone0: ObjectABC; override;
    /// Возвращает клон квадрата со скругленными краями
    function Clone: RoundSquareABC; 
  end;
  
  /// Класс графических объектов "Текст"
  TextABC = class(ObjectABC)
  private
    pointsz: integer;
    tb: boolean;
    bc: GColor;
  protected
    procedure SetFSize(sz: integer);
    procedure SetTB(b: boolean);
    procedure SetBC(c: GColor);
    procedure SetText(t: string); override;
    procedure Init(x,y,pt: integer; cl: GColor; txt: string);
    procedure InitBy(g: TextABC);
    procedure Draw(x,y: integer; g: Graphics); override;
  public
    /// Создает текстовый объект с текстом txt размера pt пунктов, цветом cl и координатами левого верхнего угла (x,y)
    constructor Create(x,y,pt: integer; txt: string; cl: GColor := clBlack);
    /// Создает текстовый объект - копию текстового объекта g
    constructor Create(g: TextABC);
    /// Размер шрифта в пунктах
    property FontSize: integer read pointsz write SetFSize;
    /// Прозрачен ли фон текстового объекта
    property TransparentBackground: boolean read tb write SetTB;
    /// Цвет фона текстового объекта
    property BackgroundColor: GColor read bc write SetBC;
    /// Возвращает клон текстового объекта
    function Clone0: ObjectABC; override;
    /// Возвращает клон текстового объекта
    function Clone: TextABC; 
  end;
  
  /// Класс графических объектов "Правильный многоугольник"
  RegularPolygonABC = class(BoundedObjectABC)
  private
    n: integer;
    angl: real;
    a: array of Point;
  protected
    procedure SetWidth(Width: integer); override;
    procedure SetHeight(Height: integer); override;
    function GetCount: integer; virtual;
    procedure SetCount(c: integer); virtual;
    procedure SetAngle(a: real);
    procedure SetRadius(r: integer);
    function GetRadius: integer;
    procedure Init(x,y,r,nn: integer; cl: GColor);
    procedure InitBy(g: RegularPolygonABC);
    procedure Draw(x,y: integer; g: Graphics); override;
  public
    /// Создает правильный многоугольник с nn вершинами, радиусом r, цветом cl и координатами центра (x,y)
    constructor Create(x,y,r,nn: integer; cl: GColor := clWhite);
    /// Создает правильный многоугольник - копию правильного многоугольника g
    constructor Create(g: RegularPolygonABC);
    /// Находится ли точка (x,y) внутри правильного многоугольника
    function PtInside(x,y: integer): boolean; override;
    /// Количество вершин правильного многоугольника
    property Count: integer read GetCount write SetCount;
    /// Радиус правильного многоугольника
    property Radius: integer read GetRadius write SetRadius;
    /// Угол поворота (в градусах)
    property Angle: real read angl write SetAngle;
    /// Масштабирует правильный многоугольник в f раз (f>1 - увеличение, 0<f<1 - уменьшение)
    procedure Scale(f: real); override;
    /// Возвращает клон правильного многоугольника
    function Clone0: ObjectABC; override;
    /// Возвращает клон правильного многоугольника
    function Clone: RegularPolygonABC;
  end;
  
  /// Класс графических объектов "Звезда"
  StarABC=class(RegularPolygonABC)
  private
    r_rr: real;
  protected
    function GetRR: integer;
    procedure SetRR(r1: integer);
    function GetCount: integer; override;
    procedure SetCount(c: integer); override;
    procedure Draw(x,y: integer; g: Graphics); override;
    procedure Init(x,y,r,r1,nn: integer; cl: GColor);
    procedure InitBy(g: StarABC);
  public
    /// Создает звезду с nn вершинами, радиусом r, внутренним радиусом r1, цветом cl и координатами центра (x,y)
    constructor Create(x,y,r,r1,nn: integer; cl: GColor := clWhite);
    /// Создает звезду - копию звезды g
    constructor Create(g: StarABC);
    /// Внутренний радиус
    property InternalRadius: integer read GetRR write SetRR;
    /// Принадлежит ли точка внутренности объекта
    function PtInside(x,y: integer): boolean; override;
    /// Масштабирует звезду в f раз (f>1 - увеличение, 0<f<1 - уменьшение)
    procedure Scale(f: real); override;
    /// Возвращает клон звезды
    function Clone0: ObjectABC; override;
    /// Возвращает клон звезды
    function Clone: StarABC;
  end;
  
  /// Класс графических объектов "Рисунок"
  PictureABC = class(ObjectABC)
  protected
    p: Picture;
    sx,sy: real;     // масштаб картинки по осям OX и OY; 1 - истинный размер; -1 - зеркальное отображение
  protected
    procedure SetWidth(Width: integer); override;
    procedure SetHeight(Height: integer); override;
    procedure SetTransparent(tt: boolean);
    function GetTransparent: boolean;
    procedure SetTransparentColor(c: GColor);
    function GetTransparentColor: GColor;
    procedure SetScaleX(ssx: real);
    procedure SetScaleY(ssy: real);
    procedure DrawAfterChangePicture(var oldRect: GRectangle);
    procedure Draw(x,y: integer; g: Graphics); override;
    procedure Init(x,y: integer; fname: string);
    procedure Init(x,y: integer; p: Picture);
    procedure InitBy(g: PictureABC);
  public
    /// Создает рисунок с координатами левого верхнего угла (x,y), считывая его из файла fname 
    constructor Create(x,y: integer; fname: string);
    /// Создает рисунок с координатами левого верхнего угла (x,y), считывая его из объекта p
    constructor Create(x,y: integer; p: Picture);
    /// Создает рисунок - копию рисунка g
    constructor Create(g: PictureABC);
    /// Меняет изображение рисунка, считывая его из файла fname
    procedure ChangePicture(fname: string);
    /// Меняет изображение рисунка, считывая его из объекта p
    procedure ChangePicture(p: Picture);
    /// Сохраняет рисунок в файл fname
    procedure Save(fname: string);
    /// Зеркально отображает рисунок относительно вертикальной оси
    procedure FlipVertical;
    /// Зеркально отображает рисунок относительно горизонтальной оси
    procedure FlipHorizontal;
    /// Прозрачен ли рисунок
    property Transparent: boolean read GetTransparent write SetTransparent;
    /// Цвет, считающийся прозрачным
    property TransparentColor: GColor read GetTransparentColor write SetTransparentColor;
    /// Масштаб рисунка по оси X относительно исходного изображения. При отрицательных значениях 
    ///происходит зеркальное отражение относительно вертикальной оси
    property ScaleX: real read sx write SetScaleX;
    /// Масштаб рисунка по оси Y относительно исходного изображения. При отрицательных значениях 
    ///происходит зеркальное отражение относительно вертикальной оси
    property ScaleY: real read sy write SetScaleY;
    /// Возвращает клон рисунка
    function Clone0: ObjectABC; override;
    /// Возвращает клон рисунка
    function Clone: PictureABC;
  end;
  
  /// Класс графических объектов "Набор рисунков"
  MultiPictureABC = class(PictureABC)
  protected
    cur,cnt: integer;
    procedure SetCurrentPicture(i: integer);
    procedure Init(x,y: integer; fname: string);
    procedure Init(x,y: integer; p: Picture);
    procedure Init(x,y,w: integer; fname: string);
    procedure Init(x,y,w: integer; p: Picture);
    procedure InitBy(g: MultiPictureABC);
    procedure Draw(x,y: integer; g: Graphics); override;
  public
    /// Создает набор рисунков, состоящий из одного рисунка, загружая его из файла с именем fname. 
    ///После создания рисунок отображается на экране в позиции (x,y). Остальные рисунки добавляются методом Add
    constructor Create(x,y: integer; fname: string);
    /// Создает набор рисунков, состоящий из одного рисунка, хранящегося в переменной p
    ///После создания рисунок отображается на экране в позиции (x,y). Остальные рисунки добавляются методом Add
    constructor Create(x,y: integer; p: Picture);
    /// Создает набор рисунков из объекта p типа Picture. Объект p должен хранить 
    ///последовательность изображений одного размера, расположенных по горизонтали. 
    ///Каждое изображение считается имеющим ширину w. Если ширина рисунка в объекте p не кратна w, 
    ///то возникает исключение. После создания первый рисунок из набора отображается на экране в позиции (x,y)
    constructor Create(x,y,w: integer; p: Picture);
    /// Создает набор рисунков, загружая его из файла fname. Файл должен хранить 
    ///последовательность изображений одного размера, расположенных по горизонтали. 
    ///Каждое изображение считается имеющим ширину w. Если ширина рисунка в файле fname не кратна w, 
    ///то возникает исключение. После создания первый рисунок из набора отображается на экране в позиции (x,y)
    constructor Create(x,y,w: integer; fname: string);
    /// Создает набор рисунков - копию набора рисунков g
    constructor Create(g: MultiPictureABC);
    /// Добавляет рисунок к набору, загружая его из файла fname. 
    /// Рисунок должен иметь те же размеры, что и все рисунки из набора
    procedure Add(fname: string);
    /// Меняет набор рисунков на набор, состоящий из одного рисунка, загружая его из файла с именем fname
    procedure ChangePicture(fname: string);
    /// Меняет набор рисунков на набор, загружая его из файла с именем fname.
    ///Файл должен хранить последовательность изображений одного размера, 
    ///расположенных по горизонтали. Каждое изображение считается имеющим ширину w
    procedure ChangePicture(w: integer; fname: string);
    ///Циклически переходит к следующему рисунку из набора
    procedure NextPicture;
    ///Циклически переходит к предыдующему рисунку из набора
    procedure PrevPicture;
    ///Номер текущего рисунка (нумерация с 1)
    property CurrentPicture: integer read cur write SetCurrentPicture;
    ///Количество рисунков в наборе
    property Count: integer read cnt;
    /// Возвращает клон набора рисунков
    function Clone0: ObjectABC; override; 
    /// Возвращает клон набора рисунков
    function Clone: MultiPictureABC; 
  end;
  
  /// Контейнер графических объектов. Сам является графическим объектом
  ContainerABC = class(ObjectABC)
  protected
    l: System.Collections.ArrayList;
    procedure SetWidth(Width: integer); override;
    procedure SetHeight(Height: integer); override;
    function GetCount: integer;
    procedure Init(x,y: integer); 
    procedure InitBy(g: ContainerABC); 
    procedure Draw(x,y: integer; g: Graphics); override;
  private
    procedure SetItem(i: integer; o: ObjectABC);
    function GetItem(i: integer): ObjectABC;
    procedure RecalcBounds;
  public
    ///Создает пустой контейнер графических объектов в позиции (x,y)
    ///Для его наполнения следует использовать метод Add. Координаты всех помещаемых 
    ///в него графических объектов пересчитываются относительно точки (x,y)
    constructor Create(x,y: integer); 
    ///Создает контейнер графических объектов - копию контейнера графических объектов g
    constructor Create(g: ContainerABC);
    ///Добавляет в контейнер графический объект g
    procedure Add(g: ObjectABC);
    ///Удаляет из контейнера графический объект g
    procedure Remove(g: ObjectABC);
    ///Отсоединяет от контейнера графический объект g. 
    ///Объект g перестает иметь владельца и продолжает отображаться на экране в той же позиции
    procedure UnLink(g: ObjectABC);
    ///Принадлежит ли точка внутренности одного из графических объектов в контейнере
    function PtInside(x,y: integer): boolean; override;
    /// Количество графических объектов в контейнере
    property Count: integer read GetCount;
    /// Массив графических объектов в контейнере
    property Objects[i: integer]: ObjectABC read GetItem write SetItem; default;
    /// Возвращает клон контейнера графических объектов
    function Clone0: ObjectABC; override; 
    /// Возвращает клон контейнера графических объектов
    function Clone: ContainerABC; 
  end;
  
  /// Класс графических объектов "Доска"
  BoardABC = class(BoundedObjectABC)
  private
    nx,ny,szx,szy: integer;
  protected  
    procedure Init(x,y,nx,ny,sszx,sszy: integer; cl: GColor := clWhite);
    procedure InitBy(g: BoardABC);
    procedure SetNX(nnx: integer);
    procedure SetNY(nny: integer);
    procedure SetSzX(sszx: integer);
    procedure SetSzY(sszy: integer);
    procedure Draw(x,y: integer; g: Graphics); override;
  public
    ///Создает доску nx на ny клеток цвета cl с размером клетки (ssxx,ssyy) в позиции (x,y). 
    constructor Create(x,y,nx,ny,sszx,sszy: integer; cl: GColor := clWhite);
    ///Создает доску - копию доски g
    constructor Create(g: BoardABC);
    ///Количество клеток доски по горизонтали
    property DimX: integer read nx write SetNX;
    ///Количество клеток доски по вертикали
    property DimY: integer read ny write SetNY;
    ///Размер клетки по горизонтали 
    property CellSizeX: integer read szx write SetSzX;
    ///Размер клетки по вертикали
    property CellSizeY: integer read szy write SetSzY;
    /// Возвращает клон доски
    function Clone0: ObjectABC; override;
    /// Возвращает клон доски
    function Clone: BoardABC; 
  end;
  
  /// Класс графических объектов "Доска с объектами"
  ObjectBoardABC = class(BoardABC)
  private
    ar: array of ObjectABC;
  protected  
    procedure Init(x,y,nn,mm,sszx,sszy: integer; cl: GColor);
    procedure InitBy(g: ObjectBoardABC);
    procedure SetObject(x,y: integer; ob: ObjectABC);
    function GetObject(x,y: integer): ObjectABC;
  public
    ///Создает доску с объектами nx на ny клеток цвета cl с размером клетки (ssxx,ssyy) в позиции (x,y). 
    constructor Create(x,y,nn,mm,sszx,sszy: integer; cl: GColor := clWhite);
    ///Создает доску с объектами - копию доски g
    constructor Create(g: ObjectBoardABC);
    /// Удаляет объект в клетке с координатами (x,y)
    procedure DestroyObject(x,y: integer);
    procedure CreateRectangleABC(x,y,w,h: integer; c: GColor);
    function GetRectangle(x,y: integer): RectangleABC;
    /// Объект в клетке с координатами (x,y)
    property Items[x,y: integer]: ObjectABC read GetObject write SetObject; default;
    /// Меняет местами объекты в клетках с координатами (x1,y1) и (x2,y2)
    procedure SwapObjects(x1,y1,x2,y2: integer);
    /// Возвращает клон доски с объектами
    function Clone0: ObjectABC; override;
    /// Возвращает клон доски с объектами
    function Clone: ObjectBoardABC; 
  end;

  /// Тип элемента управления ABCObject
  UIElementABC=class(RectangleABC) end;

  /// Тип массива графических объектов
  ObjectsABCArray = class
  private
    l: System.Collections.ArrayList;
    function GetCount: integer;
  public  
    constructor Create(ll: System.Collections.ArrayList);
    procedure SetItem(i: integer; o: ObjectABC);
    function GetItem(i: integer): ObjectABC;
    /// i-тый графический объект на экране
    property Items[i: integer]: ObjectABC read GetItem write SetItem; default;
    /// Количество графических объектов на экране
    property Count: integer read GetCount;
  end;

/// Блокирует рисование графических объектов. Возможна лишь перерисовка 
///всего экрана вместе со всеми графическими объектами на нем вызовом RedrawObjects 
procedure LockDrawingObjects;
/// Разблокирует рисование графических объектов
procedure UnLockDrawingObjects;
/// Перерисовывает все графическое окно вместе со всеми графическими объектами на нем 
procedure RedrawObjects;

/// Переносит графический объект g на передний план
procedure ToFront(grobj: ObjectABC);
/// Переносит графический объект g на задний план
procedure ToBack(grobj: ObjectABC);

/// Количество графических объектов
function ObjectsCount: integer;
/// Графический объект под точкой (x,y)
function ObjectUnderPoint(x,y: integer): ObjectABC;
/// Графический объект под точкой p
function ObjectUnderPoint(p: Point): ObjectABC;
/// Поменять позиции графических олбъектов o1 и o2
procedure SwapPositions(o1,o2: ObjectABC);

/// Элемент управления ABCObject под точкой (x,y)
function UIElementUnderPoint(x,y: integer): UIElementABC;
 
var
  /// Массив графических объектов
  Objects: ObjectsABCArray;

///--
procedure __InitModule__;

implementation

uses GraphABC, GraphABCHelper;

var
  __l: System.Collections.ArrayList;
  __lUI: System.Collections.ArrayList; // элементы пользовательского интерфейса
  __LockDrawingObjects: boolean;
  tempbmp: Bitmap;
  
procedure drawRect(r: GRectangle);
// Низкоуровневая процедура (не вызывается пользователем). Вызывает безусловную перерисовку прямоугольника.
var
///  b: Picture;
  g: ObjectABC;
  gb: System.Drawing.Graphics;
  db: boolean;
  bmp,bmp1,tb: Bitmap;
  rr,rtmp: System.Drawing.Rectangle;
begin
  LockGraphics;
  // ToDo Сделать то же самое при выходе за границу справа и внизу!!!
  if r.X<0 then 
    r.X := 0;
  if r.Width<=0 then 
    r.Width := 1;
  if r.Y<0 then 
    r.Y := 0;
  if r.Height<=0 then 
    r.Height := 1;
    
  bmp := GraphBufferBitmap;
  rr := new System.Drawing.Rectangle(r.Left,r.Top,r.Right-r.Left,r.Bottom-r.Top);
  rtmp := new System.Drawing.Rectangle(0,0,r.Right-r.Left,r.Bottom-r.Top);

  //tb := new Bitmap(r.Right-r.Left,r.Bottom-r.Top);
  tb := GetView(tempbmp,rtmp);

  gb := System.Drawing.Graphics.FromImage(tb);
  gb.SmoothingMode := GraphWindowGraphics.SmoothingMode;
//  gb.TextRenderingHint := System.Drawing.Text.TextRenderingHint.AntiAlias; 

  bmp1 := GetView(bmp,rr);
  gb.DrawImageUnscaled(bmp1,0,0);
  //gb.Transform := GraphWindowGraphics.Transform; // ???
  bmp1.Dispose;
  bmp1 := nil;

  for var i:=0 to __l.Count-1 do
  begin
    g := ObjectABC(__l[i]);
    if g.Visible and g.IntersectRect(r) then
      g.Draw(g.Left-r.Left,g.Top-r.Top,gb);
  end;
  
  for var i:=0 to __lUI.Count-1 do
  begin
    g := ObjectABC(__lUI[i]);
    if g.Visible and g.IntersectRect(r) then
      g.Draw(g.Left-r.Left,g.Top-r.Top,gb);
  end;
  
  db := DrawInBuffer;
  DrawInBuffer := False;
  
//  if not __LockDrawingObjects then
  //var m := GraphWindowGraphics.Transform;  // ???
  //GraphWindowGraphics.ResetTransform;  // ???
  GraphWindowGraphics.DrawImage(tb,r.Left,r.Top);
  //GraphWindowGraphics.Transform := m;  // ???
///  b.Draw(r.Left,r.Top);
  DrawInBuffer := db;
  gb.Dispose;
  gb := nil;
  UnLockGraphics;
end;

procedure ABCRedrawProc;
begin
  {if __LockDrawingObjects then
    Redraw
  else }RedrawObjects;
end;

procedure RedrawObjects;
begin
  drawRect(new GRectangle(0,0,WindowWidth,WindowHeight));
end;

procedure LockDrawingObjects;
begin
  __LockDrawingObjects:=True;
end;

procedure UnLockDrawingObjects;
begin
  __LockDrawingObjects:=False;
  RedrawObjects;
end;

procedure ToFront(grobj: ObjectABC);
var ind: integer;
begin
//  if grobj is UIElementABC then exit;
  if grobj=nil then exit;
  ind:=__l.IndexOf(grobj);
  if ind=__l.Count-1 then Exit;
  if ind=-1 then Exit;
//  __l.OwnsObjects:=False;
  __l.RemoveAt(ind);
  __l.Add(grobj);
//  __l.OwnsObjects:=True;
  grobj.Redraw;
end;

procedure ToBack(grobj: ObjectABC);
var ind: integer;
begin
//  if grobj is UIElementABC then exit;
  if grobj=nil then exit;
  ind:=__l.IndexOf(grobj);
  if ind=0 then Exit;
  if ind=-1 then Exit;
//  __l.OwnsObjects:=False;
  __l.RemoveAt(ind);
  __l.Insert(0,grobj);
//  __l.OwnsObjects:=True;
  grobj.Redraw;
end;

function ObjectsCount: integer;
begin
  Result:=__l.Count;
end;

function ObjectUnderPoint(x,y: integer): ObjectABC;
begin
  Result:=nil;
  for var i := Objects.Count-1 downto 0 do
  begin
    var g := Objects[i];
    if g.PtInside(x,y) then
    begin
      Result:=g;
      Exit
    end;
  end;
end;

function ObjectUnderPoint(p: Point): ObjectABC;
begin
  Result:=ObjectUnderPoint(p.x,p.y);
end;

procedure SwapPositions(o1,o2: ObjectABC);
var p: Point;
begin
  p := o1.Position;
  o1.Position := o2.Position;
  o2.Position := p;
end;

function UIElementUnderPoint(x,y: integer): UIElementABC;
begin
  Result:=nil;
  for var i:=__lUI.Count-1 downto 0 do
  begin
    var g := UIElementABC(__lUI[i]);
    if g.PtInside(x,y) then
    begin
      Result := g;
      Exit
    end;
  end;
end;

//------ ObjectABC ------
procedure ObjectABC.Init(x,y,w,h: integer; cl: GColor);
begin
  fx := x; fy := y;
  if w<1 then w := 1;
  if h<1 then h := 1;
  fw := w; fh := h;
  col := cl;

  vis := True;
  txt := '';
  txtvis := true;
  txtscale := 0.8;
  fcol := clBlack;
  fname := 'Arial';
  fstyle := fsNormal;
  if Self is UIElementABC then
    __lUI.Add(Self)
  else __l.Add(Self);
  Owner := nil;
end;

procedure ObjectABC.InitBy(g: ObjectABC);
begin
  fx := g.fx; fy := g.fy;
  fw := g.fw; fh := g.fh;
  _dx := g._dx; _dy := g._dy;
  col := g.col;

  vis := g.Visible;
  txt := g.Text;
  txtvis := g.TextVisible;
  txtscale := g.TextScale;
  fname := g.fname;
  fstyle := g.fstyle;
  if Self is UIElementABC then
    __lUI.Add(Self)
  else __l.Add(Self);
  Owner := g.Owner;
end;

constructor ObjectABC.Create(x,y,w,h: integer; cl: GColor);
begin
  Init(x,y,w,h,cl);
end;

constructor ObjectABC.Create(g: ObjectABC);
begin
  InitBy(g);
end;

destructor ObjectABC.Destroy;
begin
  Visible:=False;
  if Owner<>nil then
    Exit;
  if Self is UIElementABC then
    __lUI.Remove(Self)
  else __l.Remove(Self);
end;

procedure ObjectABC.DrawAfterChangeBounds(oldBounds,newBounds: GRectangle);
// Низкоуровневая процедура (не вызывается пользователем). Вызывает перерисовку после изменения объектом своих границ.
var 
  l,t: integer;
  r: GRectangle;
begin
  if __LockDrawingObjects then Exit;
  l:=0; t:=0;
  if Owner<>nil then
  begin
    CalcOwnerOffset(l,t);
    Owner.RecalcBounds;
  end;
  oldBounds.Offset(l,t);
  newBounds.Offset(l,t);
  if oldBounds.IntersectsWith(newBounds) then
  begin
    r := GRectangle.Union(oldBounds,newBounds);
    //TextOut(0,0,r.ToString);
    //r.Width := r.Width + 5;
    drawRect(r);
  end  
  else
  begin
    drawRect(newBounds);
    drawRect(oldBounds)
  end;
end;

procedure ObjectABC.DrawText(x,y: integer; g: Graphics);
var
  tw,th,fs,d: integer;
  m: real;
  bs: BrushStyleType;
begin
  if not TextVisible or (txt='') then
    exit;
  bs := BrushStyle;
  SetBrushStyle(bsClear);
  
  // ToDo очень много выделений памяти!
  GraphABC.SetFontColor(fcol);
  GraphABC.SetFontName(fname);
  GraphABC.SetFontStyle(fstyle);
  SetFontSize(100);
  tw := TextWidth(txt);
  th := TextHeight(txt);
  m := max(tw/Width,th/Height);
//  m:=th/Height;
  fs := round(txtscale/m*100);
  if fs<1 then fs := 1;
  SetFontSize(fs);
  tw := TextWidth(txt);
  th := TextHeight(txt);
{  if fw>50 then
    d:=10
  else if fw>20 then
    d:=5
  else d:=0;}
  if tw > Width-d then
  begin
    fs := round(fs*(Width-(Height-th)*2)/tw);
    SetFontSize(fs);
    tw := TextWidth(txt);
    th := TextHeight(txt);
  end;
  //DrawRectangle(x+(Width-tw) div 2,y+(Height-th) div 2,x+(Width-tw) div 2 + tw,y+(Height-th) div 2 + th,g);
  TextOut(x+(Width-tw) div 2,y+(Height-th) div 2,txt,g);
  SetBrushStyle(bs);
end;

procedure ObjectABC.Draw(x,y: integer); 
begin 
  Draw(x,y,GraphWindowGraphics); 
end;

procedure ObjectABC.SetCenter(p: Point);
begin
  MoveTo(p.x-fw div 2,p.y-fh div 2);
end;

function ObjectABC.GetCenter: Point;
begin
  Result.x := fx + fw div 2;
  Result.y := fy + fh div 2;
end;

procedure ObjectABC.SetPosition(p: Point);
begin
  MoveTo(p.x,p.y);
end;

function ObjectABC.GetPosition: Point;
begin
  Result.x := fx;
  Result.y := fy;
end;

procedure ObjectABC.SetX(x: integer);
begin
  moveto(x,fy);
end;

procedure ObjectABC.SetY(y: integer);
begin
  moveto(fx,y);
end;

procedure ObjectABC.SetVis(v: boolean);
begin
  if vis=v then Exit;
  vis := v;
  Redraw;
end;

procedure ObjectABC.SetFontName(name: string);
begin
  if name = fname then Exit;
  fname := name;
  Redraw;
end;

procedure ObjectABC.SetFontColor(fc: GColor);
begin
  if fcol = fc then Exit;
  fcol := fc;
  Redraw;
end;

procedure ObjectABC.SetFontStyle(fs: FontStyleType);
begin
  if fs = fstyle then Exit;
  fstyle := fs;
  Redraw;
end;

procedure ObjectABC.ObjectABCSetSize(Width,Height: integer);
var r: GRectangle;
begin
  if Width<1 then Exit;
  if Height<1 then Exit;
  if (fw=Width) and (fh=Height) then Exit;
  r := Bounds;
  fw := Width;
  fh := Height;
  if not __LockDrawingObjects then
    DrawAfterChangeBounds(r,Bounds);
end;

procedure ObjectABC.SetWidth(Width: integer);
begin
  ObjectABCSetSize(Width,Height);
end;

procedure ObjectABC.SetHeight(Height: integer);
var r: GRectangle;
begin
  if Height<1 then Exit;
  if fh=Height then Exit;
  r := Bounds;
  fh := Height;
  if not __LockDrawingObjects then
    DrawAfterChangeBounds(r,Bounds)
end;

procedure ObjectABC.SetTxtVis(b: boolean);
begin
  if txtvis=b then Exit;
  txtvis := b;
  Redraw;
end;

procedure ObjectABC.SetNum(n: integer);
begin
  Text := IntToStr(n);
end;

procedure ObjectABC.SetRealNum(r: real);
begin
  Text := string.Format('{0:f1}',r).Replace(',','.');
end;

function ObjectABC.GetNum: integer;
var err: integer;
begin
  Val(Text,Result,err);
  if err<>0 then
    Result:=0;
end;

function ObjectABC.GetRealNum: real;
var err: integer;
begin
  Val(Text,Result,err);
  if err<>0 then
    Result:=0;
end;

procedure ObjectABC.SetTextScale(r: real);
begin
  if txtscale=r then 
    Exit;
  if r<0.01 then 
    r := 0.01
  else if r>1 then 
    r := 1;
  txtscale := r;
  Redraw;
end;

procedure ObjectABC.SetOwner(o: ContainerABC);
begin
  if ow<>nil then
    ow.Unlink(Self);
  ow := nil;
  if o<>nil then
    o.Add(Self)
end;

procedure ObjectABC.SetColor(cl: GColor);
begin
  if col=cl then Exit;
  col := cl;
  Redraw;
end;

procedure ObjectABC.SetText(t: string);
begin
  if txt=t then
    exit;
  txt := t;
  Redraw;
end;

procedure ObjectABC.InternalDraw;
var b: boolean;
begin
  b := DrawInBuffer;
  DrawInBuffer := False;
  if not __LockDrawingObjects then
    Draw(fx,fy);
  DrawInBuffer := b;
end;

procedure ObjectABC.CalcOwnerOffset(var l,t: integer);
var
  o: ContainerABC;
begin
  l:=0; t:=0;
  o:=Owner;
  while o<>nil do
  begin
    l := l + o.Left;
    t := t + o.Top;
    o:=o.Owner;
  end;
end;

procedure ObjectABC.Redraw;
var
  l,t: integer;
  r: GRectangle;
begin
  if not __LockDrawingObjects then
  begin
    CalcOwnerOffset(l,t);
    r := Bounds;
    r.Offset(l,t);
    drawRect(r);
  end;
end;

procedure ObjectABC.RedrawNow;
var
  l,t: integer;
  r: GRectangle;
begin
  CalcOwnerOffset(l,t);
  r := Bounds;
  r.Offset(l,t);
  drawRect(r);
end;

procedure ObjectABC.MoveTo(x,y: integer);
var r,r1: GRectangle;
begin
  if (fx=x) and (fy=y) then Exit;
  r := Bounds;
  setCoords(x,y);
  r1 := Bounds;
  DrawAfterChangeBounds(r,r1)
end;

procedure ObjectABC.MoveOn(a,b: integer); 
begin 
  MoveTo(fx+a,fy+b); 
end;

procedure ObjectABC.Move; 
begin 
  MoveOn(dx,dy); 
end;

procedure ObjectABC.SetCoords(x,y: integer); 
begin 
  fx := x; 
  fy := y;
end;

procedure ObjectABC.Scale(f: real);
begin
//  Assert(f>0,'масштабный коэффициент<0');
  Width := round(Width*f);
  Height := round(Height*f);
end;

procedure ObjectABC.ToFront;
begin
  ABCObjects.ToFront(Self);
end;

procedure ObjectABC.ToBack;
begin
  ABCObjects.ToBack(Self);
end;

function ObjectABC.Bounds: GRectangle;
begin
  Result := new GRectangle(fx,fy,fw,fh);
end;

function ObjectABC.Intersect(g: ObjectABC): boolean;
var
  r: GRectangle;
  b1,b2: Bitmap;
  gb1,gb2: Graphics;
begin
{  Result := IntersectRect(g.Bounds);
  if Result=False then exit;}
  r := GRectangle.Intersect(Bounds,g.Bounds);
  if (r.Width<=0) or (r.Height<=0) then
  begin
    Result := False;
    exit;
  end;  
  //write(r.Width,' ',r.Height,'  ');
  b1 := new Bitmap(r.Width,r.Height);
  gb1 := Graphics.FromImage(b1);
  gb1.FillRectangle(Brushes.White,0,0,r.Width,r.Height);
  Draw(Left-r.Left,Top-r.Top,gb1);
  //GraphWindowGraphics.DrawImage(b1,400,0);
  b2 := new Bitmap(r.Width,r.Height);
  gb2 := Graphics.FromImage(b2);
  GraphABC.Brush.Color := clWhite;
  gb2.FillRectangle(Brushes.White,0,0,r.Width,r.Height);
  g.Draw(g.Left-r.Left,g.Top-r.Top,gb2);
  //GraphWindowGraphics.DrawImage(b2,450,0);
  Result := ImageIntersect(b1,b2);
end;

function ObjectABC.IntersectRect(r: GRectangle): boolean;
begin
  Result := r.IntersectsWith(Bounds);
end;

function ObjectABC.PtInside(x,y: integer): boolean;
begin
  Result := Bounds.Contains(x,y);
end;

//------ BoundedObjectABC -------
constructor BoundedObjectABC.Create(x,y,w,h: integer; cl: GColor);
begin
// А нужно ли это? 
  Init(x,y,w,h,cl);
end;

constructor BoundedObjectABC.Create(g: BoundedObjectABC);
begin
  InitBy(g);
end;

procedure BoundedObjectABC.Init(x,y,w,h: integer; cl: GColor);
begin
  inherited Init(x,y,w,h,cl);
  bcol := clBlack;
  bw := 1;
  fil := True;
  bor := True;
end;

procedure BoundedObjectABC.InitBy(g: BoundedObjectABC);
begin
  inherited InitBy(g);
  bcol := g.bcol;
  bw := g.bw;
  fil := g.fil;
  bor := g.bor;
end;

procedure BoundedObjectABC.SetBColor(cl: GColor);
begin
  if bcol=cl then Exit;
  bcol:=cl;
  Redraw;
end;

procedure BoundedObjectABC.SetBW(w: integer);
begin
  if bw=w then Exit;
  bw:=w;
  Redraw;
end;

function BoundedObjectABC.GetBW: integer;
begin
  Result := bw;
end;

procedure BoundedObjectABC.SetFilled(f: boolean);
begin
  if fil=f then Exit;
  fil := f;
  Redraw;
end;

procedure BoundedObjectABC.SetBordered(b: boolean);
begin
  if bor=b then Exit;
  bor := b;
  Redraw;
end;

procedure BoundedObjectABC.SetDrawSettings;
begin
  SetBrushColor(Color);
  if Fil then
    SetBrushStyle(bsSolid)
  else SetBrushStyle(bsClear);
  if Bor then
    SetPenStyle(psSolid)
  else SetPenStyle(psClear);
  SetPenColor(BCol);
  SetPenWidth(bw);
end;

//------ RectangleABC -------
constructor RectangleABC.Create(x,y,w,h: integer; cl: GColor);
begin
  inherited Init(x,y,w,h,cl);
  InternalDraw;
end;

constructor RectangleABC.Create(g: RectangleABC);
begin
  inherited InitBy(g);
end;

procedure RectangleABC.Draw(x,y: integer; g: Graphics);
var z,z1: integer;
begin
  SetDrawSettings;
  z := BorderWidth div 2;
  z1 := (BorderWidth-1) div 2;
  LockGraphics;
  GraphABCHelper.Rectangle(x+z,y+z,x+Width-z1,y+Height-z1,g);
  DrawText(x,y,g);
  UnLockGraphics;
end;

function RectangleABC.Clone0: ObjectABC; 
begin 
  Result := new RectangleABC(Self); 
end;

function RectangleABC.Clone: RectangleABC; 
begin 
  Result := new RectangleABC(Self); 
end;

//------ SquareABC -------
constructor SquareABC.Create(x,y,w: integer; cl: GColor); 
begin 
  inherited Create(x,y,w,w,cl); 
end;

constructor SquareABC.Create(g: SquareABC); 
begin 
  inherited Create(g);
end;

procedure SquareABC.SetWidth(Width: integer);
begin
  ObjectABCSetSize(Width,Width);
end;

procedure SquareABC.SetHeight(Height: integer);
begin
  SetWidth(Height);
end;

procedure SquareABC.Scale(f: real); 
begin 
  Width := round(Width*f);
end;

function SquareABC.Clone0: ObjectABC; 
begin
  Result := SquareABC.Create(Self); 
end;  

function SquareABC.Clone: SquareABC; 
begin 
  Result := SquareABC.Create(Self); 
end;

//------ EllipseABC -------
constructor EllipseABC.Create(x,y,w,h: integer; cl: GColor);
begin
  inherited Init(x,y,w,h,cl);
  InternalDraw;
end;

constructor EllipseABC.Create(g: EllipseABC);
begin
  inherited InitBy(g);
end;

procedure EllipseABC.Draw(x,y: integer; g: Graphics);
var z,z1: integer;
begin
  SetDrawSettings;
  z := BorderWidth div 2;
  z1 := (BorderWidth-1) div 2;
  LockGraphics;
  Ellipse(x+z,y+z,x+Width-z1,y+Height-z1,g);
  DrawText(x,y,g);
  UnLockGraphics;
end;

function EllipseABC.Clone0: ObjectABC; 
begin 
  Result := new EllipseABC(Self);
end;

function EllipseABC.Clone: EllipseABC; 
begin
  Result := new EllipseABC(Self);
end;

//------ CircleABC -------
constructor CircleABC.Create(x,y,r: integer; cl: GColor); 
begin 
  inherited Create(x-r,y-r,2*r+1,2*r+1,cl); 
end;

constructor CircleABC.Create(g: CircleABC);
begin
  inherited Create(g);
end;

procedure CircleABC.scale(f: real); 
begin 
  Width := round(Width*f); 
end;

procedure CircleABC.SetWidth(Width: integer);
begin
  if Width mod 2 = 0 then
    Width := Width - 1;
  ObjectABCSetSize(Width,Width);
end;

procedure CircleABC.SetHeight(Height: integer);
begin
  SetWidth(Height);
end;

function CircleABC.Clone0: ObjectABC; 
begin 
  Result := new CircleABC(Self);
end;

function CircleABC.Clone: CircleABC; 
begin 
  Result := new CircleABC(Self); 
end;

procedure CircleABC.SetRadius(r: integer);
begin
  if Width = 2*r+1 then 
    Exit;
  MoveOn(Width div 2-r, Width div 2-r);
  Width := 2*r+1;  
end;

function CircleABC.GetRadius: integer;
begin
  Result := Width div 2;
end;

//------ RoundRectABC -------
constructor RoundRectABC.Create(x,y,w,h,rr: integer; cl: GColor); 
begin 
  Init(x,y,w,h,rr,cl);
  InternalDraw;
end;

constructor RoundRectABC.Create(g: RoundRectABC);
begin
  InitBy(g);
end;

procedure RoundRectABC.SetRadius(rr: integer);
begin
  if r=rr then Exit;
  if rr > Width div 2 then 
    rr := Width div 2;
  if rr > Height div 2 then 
    rr := Height div 2;
  r := rr;
  Redraw;
end;

procedure RoundRectABC.Draw(x,y: integer; g: Graphics);
var z,z1: integer;
begin
  SetDrawSettings;
  z := BorderWidth div 2;
  z1 := (BorderWidth-1) div 2;
  LockGraphics;
  RoundRect(x+z,y+z,x+Width-z1,y+Height-z1,2*r,2*r,g);
  DrawText(x,y,g);
  UnLockGraphics;
end;

procedure RoundRectABC.Init(x,y,w,h,rr: integer; cl: GColor); 
begin
  inherited Init(x,y,w,h,cl);
  r := rr;
end;

procedure RoundRectABC.InitBy(g: RoundRectABC); 
begin
  inherited InitBy(g);
  r := g.r;
end;

function RoundRectABC.Clone0: ObjectABC; 
begin 
  Result := new RoundRectABC(Self); 
end;

function RoundRectABC.Clone: RoundRectABC; 
begin 
  Result := new RoundRectABC(Self); 
end;

//------ RoundSquareABC -------
constructor RoundSquareABC.Create(x,y,w,r: integer; cl: GColor); 
begin 
  inherited Create(x,y,w,w,r,cl); 
end;

constructor RoundSquareABC.Create(g: RoundSquareABC); 
begin 
  inherited Create(g); 
end;

procedure RoundSquareABC.SetWidth(Width: integer);
begin
  ObjectABCSetSize(Width,Width);
end;

procedure RoundSquareABC.SetHeight(Height: integer);
begin
  ObjectABCSetSize(Height,Height);
end;

procedure RoundSquareABC.Scale(f: real);
begin 
  Width := round(Width*f); 
end;

function RoundSquareABC.Clone0: ObjectABC;
begin 
  Result := new RoundSquareABC(Self);
end;

function RoundSquareABC.Clone: RoundSquareABC; 
begin 
  Result := new RoundSquareABC(Self);
end;

//------ TextABC -------
procedure TextABC.Init(x,y,pt: integer; cl: GColor; txt: string);
var w,h: integer;
begin
  SetFontName(fname);
  SetFontSize(pt);
  w := TextWidth(txt);
  h := TextHeight(txt);
  inherited Init(x,y,w,h,cl);
  tb := True;
  bc := clWhite;
  pointsz := pt;
  Self.txt := txt;
end;

procedure TextABC.InitBy(g: TextABC);
begin
  inherited InitBy(g);
  tb := g.tb;
  bc := g.bc;
  pointsz := g.pointsz;
  txt := g.txt;
end;

constructor TextABC.Create(x,y,pt: integer; txt: string; cl: GColor);
begin
  Init(x,y,pt,cl,txt);
  InternalDraw;
end;

constructor TextABC.Create(g: TextABC);
begin
  InitBy(g);
  InternalDraw;
end;

procedure TextABC.Draw(x,y: integer; g: Graphics);
var bs: BrushStyleType;
begin
  SetBrushColor(bc);
  if tb then
  begin
    bs := BrushStyle;
    SetBrushStyle(bsClear);
  end
  else 
    SetBrushStyle(bsSolid);
  SetFontName(fname);
  SetFontSize(pointsz);
  GraphABC.SetFontColor(Color);
  LockGraphics;
  TextOut(x,y,Text,g);
  UnLockGraphics;
  if tb then
    SetBrushStyle(bs);
end;

procedure TextABC.SetText(t: string);
var r: GRectangle;
begin
  if Text=t then
    exit;
  r := Bounds;
  txt := t;
  LockGraphics;
  fw := TextWidth(t);
  fh := TextHeight(t);
  UnLockGraphics;
  if not __LockDrawingObjects then
    DrawAfterChangeBounds(r,Bounds)
end;

{procedure TextABC.SetFName(fn: string);
begin
  if fname=fn then
    exit;
  fname:=fn;
  SetFontName(fname);
  SetFontSize(pointsz);
  Width:=TextWidth(Text);
  Height:=TextHeight(Text);
  Redraw;
end;}

procedure TextABC.SetFSize(sz: integer);
var r: GRectangle;
begin
  if pointsz = sz then 
    Exit;
  pointsz := sz;
  r := Bounds;
  SetFontName(fname);
  SetFontSize(pointsz);
  fw := TextWidth(Text);
  fh := TextHeight(Text);
  if not __LockDrawingObjects then
    DrawAfterChangeBounds(r,Bounds)
//  Redraw; // это неверно!
end;

procedure TextABC.SetTB(b: boolean);
begin
  if tb=b then Exit;
  tb := b;
  Redraw;
end;

procedure TextABC.SetBC(c: GColor);
begin
  if bc=c then Exit;
  bc := c;
  Redraw;
end;

function TextABC.Clone0: ObjectABC; 
begin 
  Result := TextABC.Create(Self);
end;

function TextABC.Clone: TextABC; 
begin 
  Result := TextABC.Create(Self);
end;

//------ RegularPolygonABC ------
procedure RegularPolygonABC.Init(x,y,r,nn: integer; cl: GColor);
begin
  inherited Init(x-r,y-r,2*r+1,2*r+1,cl);
  n := nn;                     
  SetLength(a,nn); 
  angl := 0;
end;

procedure RegularPolygonABC.InitBy(g: RegularPolygonABC);
begin
  inherited InitBy(g);
  n := g.n;                     
  SetLength(a,n); 
  angl := g.angl;
end;

constructor RegularPolygonABC.Create(x,y,r,nn: integer; cl: GColor);
begin
  Init(x,y,r,nn,cl);
  InternalDraw;
end;

constructor RegularPolygonABC.Create(g: RegularPolygonABC);
begin
  InitBy(g);
end;

procedure RegularPolygonABC.Draw(x,y: integer; g: Graphics);
begin
  SetDrawSettings;
  var phi := -90 + Angle;
  var z := BorderWidth div 2;
  var r := Width div 2;
  r := r - z;
  var x0 := x + Width div 2;
  var y0 := y + Width div 2;
  for var i:=0 to n-1 do
  begin
    a[i].x := round(r*cos(phi*Pi/180)) + x0;
    a[i].y := round(r*sin(phi*Pi/180)) + y0;
    phi := phi+360/n;
  end;
  LockGraphics;
  Polygon(a,g);
  DrawText(x,y,g);
  UnLockGraphics;
end;

function RegularPolygonABC.PtInside(x,y:integer): boolean;
var
  a,x0,y0,dx,dy,pz: integer;
  phi: real;
  part: real;
begin
  part := 360/n;
  a := Width div 2 + 1;
  x0 := Left + Width div 2;
  y0 := Top + Width div 2;
  dx := x - x0;
  dy := y - y0;
  if dx=0 then
  begin
    if dy<0 then 
      phi := 0
    else phi := 180;
  end
  else
  begin
    phi := 90 + arctan(dy/dx)*180/Pi;
    if dx<0 then 
      phi := phi + 180;
  end;
  pz := trunc(phi/part);
  phi := phi - pz*part;
  if phi>part/2 then 
    phi := part - phi;
  Phi := part/2-Phi;
  Result := sqrt(dx*dx+dy*dy) < a*cos(part/2/180*Pi)/cos(phi/180*Pi);
end;

procedure RegularPolygonABC.SetAngle(a: real);
begin
  if angl=a then 
    Exit;
  angl := a;
  Redraw;
end;

procedure RegularPolygonABC.SetWidth(Width: integer);
begin
  if Width mod 2 = 0 then 
    Width := Width - 1; // Ширина и высота должны быть нечетными
  ObjectABCSetSize(Width,Width);
end;

procedure RegularPolygonABC.SetHeight(Height: integer);
begin
  SetWidth(Height);
end;

procedure RegularPolygonABC.SetCount(c: integer);
begin
  if c<3 then 
    c := 3;
  if c>500 then 
    c := 500;
  if n=c then 
    Exit;
  n := c;
  SetLength(a,n);
  Redraw;
end;

function RegularPolygonABC.GetCount: integer;
begin
  Result := n;
end;

procedure RegularPolygonABC.SetRadius(r: integer);
begin
  if Width = 2*r+1 then 
    Exit;
  MoveOn(Width div 2-r, Width div 2-r);
  Width := 2*r+1;
end;

function RegularPolygonABC.GetRadius: integer;
begin
  Result := Width div 2;
end;

procedure RegularPolygonABC.Scale(f: real); 
begin 
  Width := round(Width*f); 
end;

function RegularPolygonABC.Clone0: ObjectABC; 
begin 
  Result := RegularPolygonABC.Create(Self); 
end;

function RegularPolygonABC.Clone: RegularPolygonABC; 
begin 
  Result := RegularPolygonABC.Create(Self); 
end;

//------ StarABC ------
procedure StarABC.Init(x,y,r,r1,nn: integer; cl: GColor);
var rr: integer;
begin
  if r<r1 then
  begin
    rr := r;
    r := r1;
    r1 := rr;
  end;
  r_rr := r/r1;
  inherited Init(x,y,r,2*nn,cl);
end;

procedure StarABC.InitBy(g: StarABC);
begin
  r_rr := g.r_rr;
  inherited InitBy(g);
end;

constructor StarABC.Create(x,y,r,r1,nn: integer; cl: GColor);
begin
  Init(x,y,r,r1,nn,cl);
  InternalDraw;
end;

constructor StarABC.Create(g: StarABC); 
begin 
  InitBy(g); 
end;

procedure StarABC.Scale(f: real); 
begin 
  Width := round(Width*f); 
end;

function StarABC.Clone0: ObjectABC; 
begin 
  Result := StarABC.Create(Self); 
end;

function StarABC.Clone: StarABC; 
begin 
  Result := StarABC.Create(Self); 
end;

function StarABC.GetCount: integer; 
begin 
  Result := inherited GetCount div 2; 
end;

procedure StarABC.SetCount(c: integer); 
begin 
  inherited SetCount(c*2); 
end;

function StarABC.GetRR: integer;
begin
  Result := round(Radius/r_rr);
end;

procedure StarABC.SetRR(r1: integer);
begin
  if r1>Radius then 
    Exit;
  if r_rr = Radius/r1 then 
    Exit;
  r_rr := Radius/r1;
  Redraw;
end;

procedure StarABC.Draw(x,y: integer; g: Graphics);
var
  r,rr,z,x0,y0: integer;
  phi: real;
begin
  SetDrawSettings;
  phi := -90 + Angle;
  z := BorderWidth;
  r := Width div 2;
  r := r - z;
  x0 := x + Width div 2;
  y0 := y + Width div 2;
  rr := round(r/r_rr);
  for var i:=0 to Count*2-1 do
  begin
    if i mod 2 = 0 then
    begin
      a[i].x := round(r*cos(phi*Pi/180)) + x0;
      a[i].y := round(r*sin(phi*Pi/180)) + y0;
    end
    else
    begin
      a[i].x := round(rr*cos(phi*Pi/180)) + x0;
      a[i].y := round(rr*sin(phi*Pi/180)) + y0;
    end;
    phi := phi + 360/Count/2;
  end;
  LockGraphics;
  Polygon(a,g);
  DrawText(x,y,g);
  UnLockGraphics;
end;

function StarABC.PtInside(x,y: integer): boolean;
var
  a,x0,y0,pz: integer;
  phi: real;
  part,dx,dy,b,c,r: real;
begin
  part := 360/Count;
  a := Width div 2 + 1;
  x0 := Left + Width div 2;
  y0 := Top + Width div 2;
  dx := x - x0;
  dy := y - y0;
  if dx=0 then
  begin
    if dy<0 then 
      phi := 0
    else phi := 180;
  end
  else
  begin
    phi := 90 + arctan(dy/dx)*180/Pi;
    if dx<0 then 
      phi := phi + 180;
  end;
  pz := trunc(phi/part);
  phi := phi - pz*part;
  if phi>part/2 then 
    phi := part - phi;
  b := (InternalRadius+1)*cos(part/2*Pi/180);
  c := (InternalRadius+1)*sin(part/2*Pi/180);
  r := sqrt(dx*dx+dy*dy);
  dx := r * cos(phi*Pi/180);
  dy := r * sin(phi*Pi/180);
  Result := c*(dx-a) < dy*(b-a);
end;

//------ PictureABC ------
procedure PictureABC.Init(x,y: integer; fname: string);
begin
  sx := 1; 
  sy := 1;
  p := Picture.Create(fname);
  p.Transparent := False;
  inherited Init(x,y,p.Width,p.Height,clBlack);
end;

procedure PictureABC.Init(x,y: integer; p: Picture);
begin
  sx := 1; 
  sy := 1;
  Self.p := p;
  inherited Init(x,y,p.Width,p.Height,clBlack);
end;

procedure PictureABC.InitBy(g: PictureABC);
begin
  inherited InitBy(g);
  sx := g.sx; 
  sy := g.sy;
  p := g.p;
end;

constructor PictureABC.Create(x,y: integer; fname: string);
begin
  Init(x,y,fname);
  InternalDraw;
end;

constructor PictureABC.Create(g: PictureABC);
begin
  InitBy(g);
end;

constructor PictureABC.Create(x,y: integer; p: Picture);
begin
  sx := 1; 
  sy := 1;
  Self.p := p;
  inherited Init(x,y,p.Width,p.Height,clBlack);
  InternalDraw;
end;

procedure PictureABC.DrawAfterChangePicture(var oldRect: GRectangle);
begin
  fw := round(abs(sx)*p.Width) + 1;
  fh := round(abs(sy)*p.Height) + 1;
  if not __LockDrawingObjects then
    DrawAfterChangeBounds(oldRect,Bounds)
end;

procedure PictureABC.ChangePicture(fname: string);
var r: GRectangle;
begin
  r := Bounds;
  p.Load(fname);
  p.Transparent := False;
  DrawAfterChangePicture(r);
end;

procedure PictureABC.ChangePicture(p: Picture);
var r: GRectangle;
begin
  r := Bounds;
  Self.p := p;
  p.Transparent := False;
  DrawAfterChangePicture(r);
end;

procedure PictureABC.Draw(x,y: integer; g: Graphics);
var bw,bh: integer;
begin
  LockGraphics;
  if (sx=1) and (sy=1) then
    p.Draw(x,y,g)
  else
  begin
    bw := p.Width;
    bh := p.Height;
    if sx<0 then
      x := x - round(bw*sx);
    if sy<0 then
      y := y - round(bh*sy);
    p.Draw(x,y,round(bw*sx),round(bh*sy),g);
  end;
  DrawText(x,y,g);
  UnLockGraphics;
end;

procedure PictureABC.SetWidth(Width: integer);
begin
  if Width<=0 then 
    exit;
  if sx>0 then
    sx := Width/p.Width
  else sx := -Width/p.Width;
  inherited SetWidth(Width);
end;

procedure PictureABC.SetHeight(Height: integer);
begin
  if Height<=0 then 
    exit;
  if sy>0 then
    sy := Height/p.Height
  else sy := -Height/p.Height;
  inherited SetHeight(Height);
end;

procedure PictureABC.SetTransparent(tt: boolean);
begin
  if p.Transparent = tt then Exit;
  p.Transparent := tt;
  Redraw;
end;

function PictureABC.GetTransparent: boolean;
begin
  Result := p.Transparent;
end;

procedure PictureABC.SetTransparentColor(c: GColor);
begin
  if p.TransparentColor = c then Exit;
  p.TransparentColor := c;
  Redraw;
end;

function PictureABC.GetTransparentColor: GColor;
begin
  Result := p.TransparentColor;
end;

procedure PictureABC.SetScaleX(ssx: real);
var r: GRectangle;
begin
  if sx=ssx then exit;
  r := Bounds;
  fw := round(abs(ssx)*p.Width)+1;
  sx := ssx;
  if not __LockDrawingObjects then
    DrawAfterChangeBounds(r,Bounds)
end;

procedure PictureABC.SetScaleY(ssy: real);
var r: GRectangle;
begin
  if sy=ssy then exit;
  r := Bounds;
  fh := round(abs(ssy)*p.Height)+1;
  sy := ssy;
  if not __LockDrawingObjects then
    DrawAfterChangeBounds(r,Bounds)
end;

procedure PictureABC.Save(fname: string);
begin
  p.Save(fname);
end;

procedure PictureABC.FlipVertical;
begin
  p.FlipVertical;
  Redraw;
end;

procedure PictureABC.FlipHorizontal;
begin
  p.FlipHorizontal;
  Redraw;
end;

function PictureABC.Clone0: ObjectABC; 
begin 
  Result := new PictureABC(Self) 
end;

function PictureABC.Clone: PictureABC; 
begin 
  Result := new PictureABC(Self) 
end;

//------ MultiPictureABC ------
procedure MultiPictureABC.Init(x,y: integer; fname: string);
begin
  cur := 1;
  cnt := 1;
  inherited Init(x,y,fname);
end;

procedure MultiPictureABC.Init(x,y: integer; p: Picture);
begin
  cur := 1;
  cnt := 1;
  inherited Init(x,y,p);
end;

procedure MultiPictureABC.Init(x,y,w: integer; fname: string);
begin
  p := Picture.Create(fname);
  p.Transparent := True;
  if p.Width mod w <> 0 then
    raise Exception.Create('Неверно задана ширина картинки');
  cur := 1;
  cnt := p.Width div w;
  inherited Init(x,y,p);
  fw := w;
end;

procedure MultiPictureABC.Init(x,y,w: integer; p: Picture);
begin
  Self.p := p;
  if p.Width mod w <> 0 then
    raise Exception.Create('Неверно задана ширина картинки');
  cur := 1;
  cnt := p.Width div w;
  inherited Init(x,y,p);
  fw := w;
end;

procedure MultiPictureABC.InitBy(g: MultiPictureABC);
begin
  inherited InitBy(g);
  cur := g.cur;
  cnt := g.cnt;
end;

constructor MultiPictureABC.Create(x,y: integer; fname: string);
begin
  Init(x,y,fname);
  InternalDraw;
end;

constructor MultiPictureABC.Create(x,y: integer; p: Picture);
begin
  Init(x,y,p);
  InternalDraw;
end;

constructor MultiPictureABC.Create(x,y,w: integer; fname: string);
begin
  Init(x,y,w,fname);
  InternalDraw;
end;

constructor MultiPictureABC.Create(x,y,w: integer; p: Picture);
begin
  Init(x,y,w,p);
  InternalDraw;
end;

constructor MultiPictureABC.Create(g: MultiPictureABC);
begin
  InitBy(g);
end;

procedure MultiPictureABC.Draw(x,y: integer; g: Graphics);
begin
  LockGraphics;
  p.Draw(x,y,new System.Drawing.Rectangle((cur-1)*Width,0,Width,Height),g);
  UnLockGraphics;
end;

procedure MultiPictureABC.ChangePicture(fname: string);
begin
  p.Load(fname);
  cur := 1;
  cnt := 1;
  Width := p.Width;
  Height := p.Height;
  Redraw;
end;

procedure MultiPictureABC.ChangePicture(w: integer; fname: string);
begin
  p.Load(fname);
  if p.Width mod w <> 0 then
    raise Exception.Create('Неверно задана ширина картинки');
  cur := 1;
  cnt := p.Width div w;
  Width := w;
  Height := p.Height;
  Redraw;
end;

procedure MultiPictureABC.Add(fname: string);
var
  p1: Picture;
  r,r1: System.Drawing.Rectangle;
begin
  Inc(cnt);
  p1 := Picture.Create(fname);
  if (p1.Width<>Width) or (p1.Height<>Height) then
    raise Exception.Create('Размеры картинки в файле '+fname+' отличаются от размеров MultiPictureABC');
  p1.Transparent := Transparent;
  p.Width := p.Width + Width;
  r1 := new System.Drawing.Rectangle(0,0,Width,Height);
  r := new System.Drawing.Rectangle((cnt-1)*Width,0,Width,Height);
  p.CopyRect(r,p1,r1);
end;

procedure MultiPictureABC.SetCurrentPicture(i: integer);
begin
  if (i<1) or (i>Count) then
    raise Exception.Create('Номер картинки '+IntToStr(i)+' находится вне диапазона '+IntToStr(i)+'..'+IntToStr(Count));
  cur := i;
  Redraw;
end;

procedure MultiPictureABC.NextPicture;
begin
  if cur>=cnt then
    CurrentPicture:=1
  else CurrentPicture:=cur+1
end;

procedure MultiPictureABC.PrevPicture;
begin
  if cur=1 then
    CurrentPicture:=cnt
  else CurrentPicture:=cur-1
end;

function MultiPictureABC.Clone0: ObjectABC; 
begin
  Result := new MultiPictureABC(Self);
end;

function MultiPictureABC.Clone: MultiPictureABC; 
begin
  Result := new MultiPictureABC(Self);
end;

//------ ContainerABC ------
procedure ContainerABC.Init(x,y: integer); 
begin
  l := new System.Collections.ArrayList; 
  inherited Init(x,y,0,0,clWhite); 
end;

procedure ContainerABC.InitBy(g: ContainerABC); 
begin
  l := new System.Collections.ArrayList; 
  inherited InitBy(g);
  for var i:=0 to g.Count-1 do
  begin
    var ob := g[i].Clone0;
    ob.Owner := Self;
  end;
end;

constructor ContainerABC.Create(x,y: integer); 
begin 
  Init(x,y);
end;

constructor ContainerABC.Create(g: ContainerABC);
begin
  InitBy(g);
end;

procedure ContainerABC.SetItem(i: integer; o: ObjectABC);
begin
  l[i] := o;
end;

function ContainerABC.GetItem(i: integer): ObjectABC;
begin
  Result := ObjectABC(l[i])
end;

procedure ContainerABC.Add(g: ObjectABC);
var
  b: boolean;
begin
{  if Self is UIElementABC then
    exit;}
  b := g.Visible;
  g.Visible := False;
  __l.Remove(g);
  l.Add(g);
  g.ow := Self;
  g.Visible := b;
  RecalcBounds;
  Redraw;
end;

procedure ContainerABC.Remove(g: ObjectABC);
begin
  l.Remove(g);
end;

procedure ContainerABC.Draw(x,y: integer; g: Graphics);
begin
  LockGraphics;
  for var i:=0 to l.Count-1 do
  begin
    var ob := ObjectABC(l[i]);
    if ob.Visible then
      ob.Draw(x+ob.Left,y+ob.Top,g);
  end;
  UnLockGraphics;
end;

function ContainerABC.PtInside(x,y:integer): boolean;
begin
  Result:=False;
  for var i:=0 to l.Count-1 do
  begin
    var g := ObjectABC(l[i]);
    if g.PtInside(x-Left,y-Top) then
    begin
      Result := True;
      Exit;
    end;
  end;
end;

procedure ContainerABC.SetWidth(Width: integer);
begin
  if Self.Width = 0 then 
    Exit;
  var scale := Width/Self.Width;
  for var i:=0 to l.Count-1 do
  begin
    var g := ObjectABC(l[i]);
    g.Left := round(g.Left*scale);
    g.Width := round(g.Width*scale);
  end;
  inherited SetWidth(Width);
end;

procedure ContainerABC.SetHeight(Height: integer);
begin
  if Self.Height=0 then Exit;
  var scale:=Height/Self.Height;
  for var i:=1 to l.Count do
  begin
    var g := ObjectABC(l[i]);
    g.Top := round(g.Top*scale);
    g.Height := round(g.Height*scale);
  end;
  inherited SetHeight(Height);
end;

procedure ContainerABC.RecalcBounds;
begin
  if Count=0 then Exit;
  var r := ObjectABC(l[0]).Bounds;
  for var i:=1 to l.Count-1 do
    r := GRectangle.Union(r,ObjectABC(l[i]).Bounds);
  fx := fx + r.Left;
  fy := fy + r.Top;
  fw := r.Right - r.Left;
  fh := r.Bottom - r.Top;
  for var i:=0 to l.Count-1 do
    ObjectABC(l[i]).SetCoords(ObjectABC(l[i]).Left-r.Left,ObjectABC(l[i]).Top-r.Top);
end;

procedure ContainerABC.UnLink(g: ObjectABC);
begin
  l.Remove(g);
  Redraw;
  RecalcBounds;
end;

function ContainerABC.Clone0: ObjectABC; 
begin 
  Result := new ContainerABC(Self); 
end;

function ContainerABC.Clone: ContainerABC; 
begin 
  Result := new ContainerABC(Self); 
end;

function ContainerABC.GetCount: integer; 
begin 
  Result := l.Count; 
end;

//------ BoardABC ------
procedure BoardABC.Init(x,y,nx,ny,sszx,sszy: integer; cl: GColor);
begin 
  Self.nx := nx;
  Self.ny := ny;
  szx := sszx;
  szy := sszy;
  inherited Init(x,y,nx*szx+1,ny*szy+1,cl);
end;

procedure BoardABC.InitBy(g: BoardABC);
begin 
  nx := g.nx;
  ny := g.ny;
  szx := g.szx;
  szy := g.szy;
  inherited InitBy(g);
end;

constructor BoardABC.Create(x,y,nx,ny,sszx,sszy: integer; cl: GColor);
begin 
  Init(x,y,nx,ny,sszx,sszy,cl);
  InternalDraw;
end;

constructor BoardABC.Create(g: BoardABC);
begin 
  InitBy(g);
end;

procedure BoardABC.SetNX(nnx: integer);
var r: GRectangle;
begin 
  if nx = nnx then
    Exit;
  r := Bounds;  
  nx := nnx;
  if not __LockDrawingObjects then
    DrawAfterChangeBounds(r,Bounds)
end;

procedure BoardABC.SetNY(nny: integer);
var r: GRectangle;
begin 
  if ny = nny then
    Exit;
  r := Bounds;  
  ny := nny;
  if not __LockDrawingObjects then
    DrawAfterChangeBounds(r,Bounds)
end;

procedure BoardABC.SetSzX(sszx: integer);
var r: GRectangle;
begin 
  if szx = sszx then
    Exit;
  r := Bounds;  
  szx := sszx;
  if not __LockDrawingObjects then
    DrawAfterChangeBounds(r,Bounds)
end;

procedure BoardABC.SetSzY(sszy: integer);
var r: GRectangle;
begin 
  if szy = sszy then
    Exit;
  r := Bounds;  
  szy := sszy;
  if not __LockDrawingObjects then
    DrawAfterChangeBounds(r,Bounds)
end;

procedure BoardABC.Draw(x,y: integer; g: Graphics); 
begin
  SetDrawSettings;
  LockGraphics;
  FillRectangle(x,y,x+Width,y+Height,g);
  for var i:=0 to nx do
    Line(x+i*szx,y,x+i*szx,y+Height-1,g);
  for var i:=0 to ny do
    Line(x,y+i*szy,x+Width-1,y+i*szy,g);
  UnLockGraphics;  
end;

function BoardABC.Clone0: ObjectABC;
begin 
  Result := new BoardABC(Self);
end;

function BoardABC.Clone: BoardABC; 
begin 
  Result := new BoardABC(Self); 
end;

//------ ObjectBoardABC ------
procedure ObjectBoardABC.Init(x,y,nn,mm,sszx,sszy: integer; cl: GColor);
begin
  inherited Init(x,y,nn,mm,sszx,sszy,cl);
  SetLength(ar,nn*mm);
  for var i:=0 to nn*mm-1 do
    ar[i] := nil;
end;

procedure ObjectBoardABC.InitBy(g: ObjectBoardABC);
begin 
  inherited InitBy(g);
  SetLength(ar,g.ar.Length);
  for var i:=0 to g.ar.Length-1 do
    ar[i] := g.ar[i].Clone;
end;

constructor ObjectBoardABC.Create(x,y,nn,mm,sszx,sszy: integer; cl: GColor);
begin 
  Init(x,y,nn,mm,sszx,sszy,cl);
  InternalDraw;
end;

constructor ObjectBoardABC.Create(g: ObjectBoardABC);
begin 
  InitBy(g);
end;

procedure ObjectBoardABC.CreateRectangleABC(x,y,w,h: integer; c: GColor);
begin 
  SetObject(x,y,new RectangleABC(0,0,w,h,c));
end;

procedure ObjectBoardABC.DestroyObject(x,y: integer);
begin
  if Self[x,y]<>nil then
  begin
    Self[x,y].Destroy;
    Self[x,y] := nil;
  end;
end;

procedure ObjectBoardABC.SetObject(x,y: integer; ob: ObjectABC);
begin
  ar[(y-1)*DimX + x - 1] := ob;
  if ob=nil then
    Exit;
  ob.dx := x; 
  ob.dy := y; // использую dx и dy для задания клеточных координат на доске
  ob.Center := new Point(Left+CellSizeX*(x-1)+CellSizeX div 2,Top+CellSizeY*(y-1)+CellSizeY div 2);
end;

function ObjectBoardABC.GetObject(x,y: integer): ObjectABC;
begin 
  Result := ar[(y-1)*DimX + x - 1];
end;

function ObjectBoardABC.GetRectangle(x,y: integer): RectangleABC;
begin 
  Result := RectangleABC(ar[(y-1)*DimX + x - 1]);
end;

procedure ObjectBoardABC.SwapObjects(x1,y1,x2,y2: integer);
var 
  ob1,ob2: ObjectABC;
  csx,csy: integer;
begin 
  ob1 := Self[x1,y1];
  ob2 := Self[x2,y2];
  Self[x1,y1] := ob2;
  Self[x2,y2] := ob1;
  csx := CellSizeX;
  csy := CellSizeY;

  if ob1<>nil then
  begin
    ob1.dx := x2;
    ob1.dy := y2;
    ob1.Center := new Point(Left+csx*(x2-1)+csx div 2,Top+csy*(y2-1)+csy div 2);
  end;

  if ob2<>nil then
  begin
    ob2.dx := x1;
    ob2.dy := y1;
    ob2.Center := new Point(Left+csx*(x1-1)+csx div 2,Top+csy*(y1-1)+csy div 2);
  end;
end;

function ObjectBoardABC.Clone0: ObjectABC;
begin 
  Result := new ObjectBoardABC(Self);
end;

function ObjectBoardABC.Clone: ObjectBoardABC; 
begin 
  Result := new ObjectBoardABC(Self);
end;

//------ ObjectsABCArray ------
constructor ObjectsABCArray.Create(ll: System.Collections.ArrayList);
begin
  l := ll;
end;

function ObjectsABCArray.GetCount: integer;
begin
  Result := l.Count;
end;

procedure ObjectsABCArray.SetItem(i: integer; o: ObjectABC);
begin
  l[i] := o;
end;

function ObjectsABCArray.GetItem(i: integer): ObjectABC;
begin
  Result := ObjectABC(l[i])
end;

var __initialized := false;

procedure __InitModule;
begin
  __l := new System.Collections.ArrayList;
  __lUI := new System.Collections.ArrayList;
  Objects := new ObjectsABCArray(__l);
  tempbmp := new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
  SetWindowTitle('ABC Objects');
  RedrawProc := ABCRedrawProc;  
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    GraphABC.__InitModule__;
    __InitModule;
  end;
end;

initialization
  __InitModule;
end.