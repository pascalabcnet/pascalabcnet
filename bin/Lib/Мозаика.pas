unit Мозаика;

interface

uses GraphWPF;

var Window := GraphWPF.Window; 

procedure п;

procedure к;
procedure кк1;
procedure кк2;
procedure кк3;
procedure кк4;
procedure кт1;
procedure кт2;
procedure кт3;
procedure кт4;

procedure ко;
procedure кок1;
procedure кок2;
procedure кок3;
procedure кок4;
procedure кот1;
procedure кот2;
procedure кот3;
procedure кот4;

procedure ж;
procedure жк1;
procedure жк2;
procedure жк3;
procedure жк4;
procedure жт1;
procedure жт2;
procedure жт3;
procedure жт4;

procedure г;
procedure гк1;
procedure гк2;
procedure гк3;
procedure гк4;
procedure гт1;
procedure гт2;
procedure гт3;
procedure гт4;

procedure ч;
procedure чк1;
procedure чк2;
procedure чк3;
procedure чк4;
procedure чт1;
procedure чт2;
procedure чт3;
procedure чт4;

procedure б;
procedure бт1;
procedure бт2;
procedure бт3;
procedure бт4;
procedure бк1;
procedure бк2;
procedure бк3;
procedure бк4;

procedure з;
procedure зт1;
procedure зт2;
procedure зт3;
procedure зт4;
procedure зк1;
procedure зк2;
procedure зк3;
procedure зк4;

procedure с;
procedure ст1;
procedure ст2;
procedure ст3;
procedure ст4;
procedure ск1;
procedure ск2;
procedure ск3;
procedure ск4;

procedure се;
procedure сет1;
procedure сет2;
procedure сет3;
procedure сет4;
procedure сек1;
procedure сек2;
procedure сек3;
procedure сек4;

procedure о;
procedure от1;
procedure от2;
procedure от3;
procedure от4;
procedure ок1;
procedure ок2;
procedure ок3;
procedure ок4;

procedure р;
procedure рт1;
procedure рт2;
procedure рт3;
procedure рт4;
procedure рк1;
procedure рк2;
procedure рк3;
procedure рк4;

procedure ф;
procedure фт1;
procedure фт2;
procedure фт3;
procedure фт4;
procedure фк1;
procedure фк2;
procedure фк3;
procedure фк4;

procedure Ряд(p: procedure);
procedure НарисоватьРяд(p: procedure);

implementation

var
  w := 50;
  z := 2;
  x := 1;
  y := 1;

procedure Draw(x, y: integer) := Rectangle(x * w, y * w, w - z, w - z);

procedure D(c: Color);
begin
  Brush.Color := c;
  Draw(x, y); 
  x += 1;
end;

procedure Ц1(c: Color);
begin
  Brush.Color := c;
  Pie(x * w + w - z,y * w + w - z,w-z,90,180);
  x += 1;
end;
procedure Ц2(c: Color);
begin
  Brush.Color := c;
  Pie(x * w,y * w + w - z,w-z,0,90);
  x += 1;
end;
procedure Ц3(c: Color);
begin
  Brush.Color := c;
  Pie(x * w,y * w,w-z,270,360);
  x += 1;
end;
procedure Ц4(c: Color);
begin
  Brush.Color := c;
  Pie(x * w + w - z,y * w,w-z,180,270);
  x += 1;
end;
procedure Тр1(c: Color);
begin
  Brush.Color := c;
  Polygon(Arr(Pnt(x * w + w - z,y * w + w - z),Pnt(x * w + w - z,y * w),Pnt(x * w,y * w + w - z)));
  x += 1;
end;
procedure Тр2(c: Color);
begin
  Brush.Color := c;
  Polygon(Arr(Pnt(x * w,y * w + w - z),Pnt(x * w,y * w),Pnt(x * w + w - z,y * w + w - z)));
  x += 1;
end;
procedure Тр3(c: Color);
begin
  Brush.Color := c;
  Polygon(Arr(Pnt(x * w,y * w),Pnt(x * w + w - z,y * w),Pnt(x * w,y * w + w - z)));
  x += 1;
end;
procedure Тр4(c: Color);
begin
  Brush.Color := c;
  Polygon(Arr(Pnt(x * w + w - z,y * w),Pnt(x * w,y * w),Pnt(x * w + w - z,y * w + w - z)));
  x += 1;
end;

//------ Соответствие цветов
var КрасныйЦвет := Colors.Red;
var ОранжевыйЦвет := RGB(255,127,$00);
var ЖелтыйЦвет := Colors.Yellow;
var ЗеленыйЦвет := RGB($00,$99,$00);
var ГолубойЦвет := RGB($61,$c3,$ff);
var СинийЦвет := RGB(0,0,255);
var ФиолетовыйЦвет := RGB($94,$00,$d3);

var РозовыйЦвет := Colors.Magenta;
var КоричневыйЦвет := RGB($96,$4B,$00);
var СерыйЦвет := Colors.Gray;
var БелыйЦвет := Colors.White;
var ЧерныйЦвет := Colors.Black;

//----------------

procedure Пусто := x += 1;

procedure НовыйРяд := (x, y) := (1, y + 1); 

procedure Серый := D(СерыйЦвет);
procedure СерыйЦКруг1 := Ц1(Colors.Gray);
procedure СерыйЦКруг2 := Ц2(СерыйЦвет);
procedure СерыйЦКруг3 := Ц3(СерыйЦвет);
procedure СерыйЦКруг4 := Ц4(СерыйЦвет);
procedure СерыйТр1 := Тр1(СерыйЦвет);
procedure СерыйТр2 := Тр2(СерыйЦвет);
procedure СерыйТр3 := Тр3(СерыйЦвет);
procedure СерыйТр4 := Тр4(СерыйЦвет);

procedure Оранжевый := D(ОранжевыйЦвет);
procedure ОранжевыйЦКруг1 := Ц1(ОранжевыйЦвет);
procedure ОранжевыйЦКруг2 := Ц2(ОранжевыйЦвет);
procedure ОранжевыйЦКруг3 := Ц3(ОранжевыйЦвет);
procedure ОранжевыйЦКруг4 := Ц4(ОранжевыйЦвет);
procedure ОранжевыйТр1 := Тр1(ОранжевыйЦвет);
procedure ОранжевыйТр2 := Тр2(ОранжевыйЦвет);
procedure ОранжевыйТр3 := Тр3(ОранжевыйЦвет);
procedure ОранжевыйТр4 := Тр4(ОранжевыйЦвет);

procedure Коричневый := D(КоричневыйЦвет);
procedure КоричневыйЦКруг1 := Ц1(КоричневыйЦвет);
procedure КоричневыйЦКруг2 := Ц2(КоричневыйЦвет);
procedure КоричневыйЦКруг3 := Ц3(КоричневыйЦвет);
procedure КоричневыйЦКруг4 := Ц4(КоричневыйЦвет);
procedure КоричневыйТр1 := Тр1(КоричневыйЦвет);
procedure КоричневыйТр2 := Тр2(КоричневыйЦвет);
procedure КоричневыйТр3 := Тр3(КоричневыйЦвет);
procedure КоричневыйТр4 := Тр4(КоричневыйЦвет);

procedure Белый := D(БелыйЦвет);
procedure БелыйЦКруг1 := Ц1(БелыйЦвет);
procedure БелыйЦКруг2 := Ц2(БелыйЦвет);
procedure БелыйЦКруг3 := Ц3(БелыйЦвет);
procedure БелыйЦКруг4 := Ц4(БелыйЦвет);
procedure БелыйТр1 := Тр1(БелыйЦвет);
procedure БелыйТр2 := Тр2(БелыйЦвет);
procedure БелыйТр3 := Тр3(БелыйЦвет);
procedure БелыйТр4 := Тр4(БелыйЦвет);

procedure Синий := D(СинийЦвет);
procedure СинийЦКруг1 := Ц1(СинийЦвет);
procedure СинийЦКруг2 := Ц2(СинийЦвет);
procedure СинийЦКруг3 := Ц3(СинийЦвет);
procedure СинийЦКруг4 := Ц4(СинийЦвет);
procedure СинийТр1 := Тр1(СинийЦвет);
procedure СинийТр2 := Тр2(СинийЦвет);
procedure СинийТр3 := Тр3(СинийЦвет);
procedure СинийТр4 := Тр4(СинийЦвет);

procedure Фиолетовый := D(ФиолетовыйЦвет);
procedure ФиолетовыйЦКруг1 := Ц1(ФиолетовыйЦвет);
procedure ФиолетовыйЦКруг2 := Ц2(ФиолетовыйЦвет);
procedure ФиолетовыйЦКруг3 := Ц3(ФиолетовыйЦвет);
procedure ФиолетовыйЦКруг4 := Ц4(ФиолетовыйЦвет);
procedure ФиолетовыйТр1 := Тр1(ФиолетовыйЦвет);
procedure ФиолетовыйТр2 := Тр2(ФиолетовыйЦвет);
procedure ФиолетовыйТр3 := Тр3(ФиолетовыйЦвет);
procedure ФиолетовыйТр4 := Тр4(ФиолетовыйЦвет);

procedure Красный := D(КрасныйЦвет);
procedure КрасныйЦКруг1 := Ц1(КрасныйЦвет);
procedure КрасныйЦКруг2 := Ц2(КрасныйЦвет);
procedure КрасныйЦКруг3 := Ц3(КрасныйЦвет);
procedure КрасныйЦКруг4 := Ц4(КрасныйЦвет);
procedure КрасныйТр1 := Тр1(КрасныйЦвет);
procedure КрасныйТр2 := Тр2(КрасныйЦвет);
procedure КрасныйТр3 := Тр3(КрасныйЦвет);
procedure КрасныйТр4 := Тр4(КрасныйЦвет);

procedure Черный := D(ЧерныйЦвет);
procedure Чёрный := D(ЧерныйЦвет);
procedure ЧерныйТр1 := Тр1(ЧерныйЦвет);
procedure ЧерныйТр2 := Тр2(ЧерныйЦвет);
procedure ЧерныйТр3 := Тр3(ЧерныйЦвет);
procedure ЧерныйТр4 := Тр4(ЧерныйЦвет);
procedure ЧерныйЦКруг1 := Ц1(ЧерныйЦвет);
procedure ЧерныйЦКруг2 := Ц2(ЧерныйЦвет);
procedure ЧерныйЦКруг3 := Ц3(ЧерныйЦвет);
procedure ЧерныйЦКруг4 := Ц4(ЧерныйЦвет);
procedure ЧёрныйЦКруг1 := Ц1(ЧерныйЦвет);
procedure ЧёрныйЦКруг2 := Ц2(ЧерныйЦвет);
procedure ЧёрныйЦКруг3 := Ц3(ЧерныйЦвет);
procedure ЧёрныйЦКруг4 := Ц4(ЧерныйЦвет);

procedure Зелёный := D(ЗеленыйЦвет);
procedure ЗелёныйЦКруг1 := Ц1(ЗеленыйЦвет);
procedure ЗелёныйЦКруг2 := Ц2(ЗеленыйЦвет);
procedure ЗелёныйЦКруг3 := Ц3(ЗеленыйЦвет);
procedure ЗелёныйЦКруг4 := Ц4(ЗеленыйЦвет);
procedure ЗелёныйТр1 := Тр1(ЗеленыйЦвет);
procedure ЗелёныйТр2 := Тр2(ЗеленыйЦвет);
procedure ЗелёныйТр3 := Тр3(ЗеленыйЦвет);
procedure ЗелёныйТр4 := Тр4(ЗеленыйЦвет);

procedure Голубой := D(ГолубойЦвет);
procedure ГолубойЦКруг1 := Ц1(ГолубойЦвет);
procedure ГолубойЦКруг2 := Ц2(ГолубойЦвет);
procedure ГолубойЦКруг3 := Ц3(ГолубойЦвет);
procedure ГолубойЦКруг4 := Ц4(ГолубойЦвет);
procedure ГолубойТр1 := Тр1(ГолубойЦвет);
procedure ГолубойТр2 := Тр2(ГолубойЦвет);
procedure ГолубойТр3 := Тр3(ГолубойЦвет);
procedure ГолубойТр4 := Тр4(ГолубойЦвет);

procedure Розовый := D(РозовыйЦвет);
procedure РозовыйЦКруг1 := Ц1(РозовыйЦвет);
procedure РозовыйЦКруг2 := Ц2(РозовыйЦвет);
procedure РозовыйЦКруг3 := Ц3(РозовыйЦвет);
procedure РозовыйЦКруг4 := Ц4(РозовыйЦвет);
procedure РозовыйТр1 := Тр1(РозовыйЦвет);
procedure РозовыйТр2 := Тр2(РозовыйЦвет);
procedure РозовыйТр3 := Тр3(РозовыйЦвет);
procedure РозовыйТр4 := Тр4(РозовыйЦвет);

procedure Желтый := D(ЖелтыйЦвет);
procedure Жёлтый := D(ЖелтыйЦвет);
procedure ЖелтыйЦКруг1 := Ц1(ЖелтыйЦвет);
procedure ЖелтыйЦКруг2 := Ц2(ЖелтыйЦвет);
procedure ЖелтыйЦКруг3 := Ц3(ЖелтыйЦвет);
procedure ЖелтыйЦКруг4 := Ц4(ЖелтыйЦвет);
procedure ЖелтыйТр1 := Тр1(ЖелтыйЦвет);
procedure ЖелтыйТр2 := Тр2(ЖелтыйЦвет);
procedure ЖелтыйТр3 := Тр3(ЖелтыйЦвет);
procedure ЖелтыйТр4 := Тр4(ЖелтыйЦвет);

procedure п := пусто;

procedure к := красный;
procedure кк1 := красныйцкруг1;
procedure кк2 := красныйцкруг2;
procedure кк3 := красныйцкруг3;
procedure кк4 := красныйцкруг4;
procedure кт1 := красныйТр1;
procedure кт2 := красныйТр2;
procedure кт3 := красныйТр3;
procedure кт4 := красныйТр4;

procedure ко := коричневый;
procedure кок1 := коричневыйцкруг1;
procedure кок2 := коричневыйцкруг2;
procedure кок3 := коричневыйцкруг3;
procedure кок4 := коричневыйцкруг4;
procedure кот1 := коричневыйТр1;
procedure кот2 := коричневыйТр2;
procedure кот3 := коричневыйТр3;
procedure кот4 := коричневыйТр4;

procedure ж := желтый;
procedure жк1 := желтыйцкруг1;
procedure жк2 := желтыйцкруг2;
procedure жк3 := желтыйцкруг3;
procedure жк4 := желтыйцкруг4;
procedure жт1 := желтыйТр1;
procedure жт2 := желтыйТр2;
procedure жт3 := желтыйТр3;
procedure жт4 := желтыйТр4;

procedure г := голубой;
procedure гк1 := Голубойцкруг1;
procedure гк2 := Голубойцкруг2;
procedure гк3 := Голубойцкруг3;
procedure гк4 := Голубойцкруг4;
procedure гт1 := ГолубойТр1;
procedure гт2 := ГолубойТр2;
procedure гт3 := ГолубойТр3;
procedure гт4 := ГолубойТр4;

procedure ч := черный;
procedure Чк1 := ЧЕРНыйцкруг1;
procedure Чк2 := ЧЕРНыйцкруг2;
procedure Чк3 := ЧЕРНыйцкруг3;
procedure Чк4 := ЧЕРНыйцкруг4;
procedure Чт1 := ЧЕРНыйТр1;
procedure Чт2 := ЧЕРНыйТр2;
procedure Чт3 := ЧЕРНыйТр3;
procedure Чт4 := ЧЕРНыйТр4;

procedure б := белый;
procedure бт1 := белыйТр1;
procedure бт2 := белыйТр2;
procedure бт3 := белыйТр3;
procedure бт4 := белыйТр4;
procedure Бк1 := белыйцкруг1;
procedure Бк2 := белыйцкруг2;
procedure Бк3 := белыйцкруг3;
procedure Бк4 := белыйцкруг4;

procedure з := зелёный;
procedure зт1 := зелёныйТр1;
procedure зт2 := зелёныйТр2;
procedure зт3 := зелёныйТр3;
procedure зт4 := зелёныйТр4;
procedure зк1 := зелёныйцкруг1;
procedure зк2 := зелёныйцкруг2;
procedure зк3 := зелёныйцкруг3;
procedure зк4 := зелёныйцкруг4;

procedure с := синий;
procedure ст1 := синийТр1;
procedure ст2 := синийТр2;
procedure ст3 := синийТр3;
procedure ст4 := синийТр4;
procedure ск1 := синийцкруг1;
procedure ск2 := синийцкруг2;
procedure ск3 := синийцкруг3;
procedure ск4 := синийцкруг4;

procedure се := серый;
procedure сет1 := серыйТр1;
procedure сет2 := серыйТр2;
procedure сет3 := серыйТр3;
procedure сет4 := серыйТр4;
procedure сек1 := серыйцкруг1;
procedure сек2 := серыйцкруг2;
procedure сек3 := серыйцкруг3;
procedure сек4 := серыйцкруг4;

procedure о := оранжевый;
procedure от1 := оранжевыйТр1;
procedure от2 := оранжевыйТр2;
procedure от3 := оранжевыйТр3;
procedure от4 := оранжевыйТр4;
procedure ок1 := оранжевыйцкруг1;
procedure ок2 := оранжевыйцкруг2;
procedure ок3 := оранжевыйцкруг3;
procedure ок4 := оранжевыйцкруг4;

procedure Р := розовый;
procedure Рт1 := розовыйТр1;
procedure Рт2 := розовыйТр2;
procedure Рт3 := розовыйТр3;
procedure Рт4 := розовыйТр4;
procedure Рк1 := розовыйцкруг1;
procedure Рк2 := розовыйцкруг2;
procedure Рк3 := розовыйцкруг3;
procedure Рк4 := розовыйцкруг4;

procedure ф := фиолетовый;
procedure фт1 := фиолетовыйТр1;
procedure фт2 := фиолетовыйТр2;
procedure фт3 := фиолетовыйТр3;
procedure фт4 := фиолетовыйТр4;
procedure фк1 := фиолетовыйцкруг1;
procedure фк2 := фиолетовыйцкруг2;
procedure фк3 := фиолетовыйцкруг3;
procedure фк4 := фиолетовыйцкруг4;

procedure Run(p: procedure) := p;

procedure НарисоватьРяд(p: procedure) := Run(p + НовыйРяд);
procedure Ряд(p: procedure) := (p + НовыйРяд);

begin
  SetStandardCoords(1 / 1.25, 0.5, 0.5);
  pen.Width := 1 / 1.25;
end.  