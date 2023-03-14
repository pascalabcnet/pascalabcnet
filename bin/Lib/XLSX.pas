unit XLSX;

interface

type
  exCellBorder = class
    IsLeft, IsRight, IsBottom, IsTop, IsDiagonal: boolean;
  end;
  
  exCell = class
    constructor(sheet: array of array of exCell; row, col: integer);
    begin
      _sheet := sheet; 
      Self.row := row; 
      Self.col := col;
    end;
  
  private
    row, col: integer;
    _sheet: array of array of exCell;
    function getBorders: (boolean, boolean, boolean, boolean);// (left,top,right,bottom)
    begin
      var left := Border.IsLeft or (col > 0) and _sheet[row, col - 1].Border.IsRight;
      var top := Border.IsTop or (row > 0) and _sheet[row - 1, col].Border.IsBottom;
      var right := Border.IsRight or (col + 1 < _sheet.First.Length) 
        and (_sheet[row, col + 1].Border <> nil) and _sheet[row, col + 1].Border.IsLeft;
      var bottom := Border.IsBottom or (row + 1 < _sheet.Length) 
        and _sheet[row + 1, col].Border.IsTop;
      Result := (left, top, right, bottom);
    end;
  
  public
    Border: exCellBorder;
    Value: string;
    property AsInt: integer read Value.ToInteger;
    property Index: (integer, integer) read (row, col);
    property Sheet: array of array of exCell read _sheet;
    property Borders: (boolean, boolean, boolean, boolean) read getBorders;
  end;

function ReadXLSX(fileName: string): Dictionary<string, array of array of string>;
function ReadXLSXAsCells(fileName: string; sheetNum: integer := 0): array of array of exCell;
function ReadXLSXAsInts(fileName: string; sheetNum: integer := 0): array of array of integer;
function ToDate(Self: string): DateTime;

implementation

{$reference System.IO.Compression.dll}
{$reference System.IO.Compression.FileSystem.dll}
{$reference System.Xml.dll}

uses System.IO.Compression;
uses System.Xml,sf;

var
  numFmt, numBrd: List<int>;
  brdStyles: List<exCellBorder>;
  arxiv: System.IO.Compression.ZipArchive;
  shStr := new List<string>;
  DateTypeId := |14, 15, 16, 17, 22, 27, 30, 36, 50, 55, 57|.ToHS;

function ToDate(Self: string): DateTime;
begin
  Result := DateTime.Parse(Self);
end;

function ToCR(Self: string): (integer, integer); extensionmethod; // 'AA7' -> (6,26)
begin
  var clm := 0;
  var row := 0;
  foreach var c in Self do
    if c in 'A'..'Z' then
      clm := clm * 26 + (c - 'A' + 1)
    else 
      row := row * 10 + (c - '0');
  Result := (row - 1, clm - 1); 
end;

function Load(path: string): XmlElement;
begin
  if arxiv.Entries.Any(e -> (e as ZipArchiveEntry).FullName = path) then begin
    var stream := arxiv.GetEntry(path).Open;
    var doc := new XmlDocument();
    doc.Load(stream);
    Result := doc.DocumentElement;
  end;
end;

function SheetRange(elms: XmlElement): ((int, int), (int, int));
begin
  var LU, RD: (int, int);
  var dim := elms.Item['dimension'];
  if dim = nil then begin // собираем габариты таблицы
    var (l, u) := (maxint, maxint);
    var (r, d) := (-1, -1);
    foreach var node: XmlNode in elms.Item['sheetData'].ChildNodes do
    begin
      foreach var cell: XmlNode in node.ChildNodes do
      begin
        var (i, j) := cell.Attributes.GetNamedItem('r').Value.ToCR;
        l := min(l, i);
        u := min(u, j);
        r := max(r, i);
        d := max(d, j);
      end
    end;
    LU := (l, u);
    RD := (r, d);
  end else begin // готовые габариты из таблицы
    var ref := dim.GetAttribute('ref');
    if ':' in ref then 
      (LU, RD) := ref.Split(':').Sel(s -> s.ToCR)
    else begin
      RD := ref.ToCR;
      LU := RD;
    end;
  end;
  Result := (LU, RD);
end;

function LoadSheet(path: string): array of array of string;
begin
  var elms := Load(path);
  var (LU, RD) := SheetRange(elms);
  
  var (w, h) := (RD[1] - LU[1] + 1, RD[0] - LU[0] + 1);
  var sheet: array of array of string;
  if (w < 0) or (h < 0) then 
    setLength(sheet, 0)
  else begin
    setLength(sheet, h);
    for var i := 0 to h - 1 do sheet[i] := new string[w];
    
    foreach var node: XmlNode in elms.Item['sheetData'].ChildNodes do
    begin
      foreach var cell: XmlNode in node.ChildNodes do
      begin
        var (i, j) := cell.Attributes.GetNamedItem('r').Value.ToCR;
        i -= LU[0];
        j -= LU[1];
        var typ := cell.Attributes.GetNamedItem('t');
        var st := cell.Attributes.GetNamedItem('s');
        var shared := false;
        if (typ <> nil) and (typ.Value = 's') then 
          shared := true;
        var value := '';
        foreach var vl: XmlNode in cell.ChildNodes do
          if vl.Name = 'v' then value := vl.InnerText;
        if shared then 
          sheet[i, j] := shStr[value.ToInteger]
        else if (st <> nil) and (numFmt[st.Value.ToI] in DateTypeId) then
          sheet[i, j] := (new DateTime(1900, 1, 1)).AddDays(value.ToI - 2).ToString('dd.MM.yyyy')
        else
          sheet[i, j] := value;
      end;
    end;
  end;
  Result := sheet;
end;

function LoadSheetAsCells(path: string): array of array of exCell;
begin
  var elms := Load(path);
  var (LU, RD) := SheetRange(elms);
  
  var (w, h) := (RD[1] - LU[1] + 1, RD[0] - LU[0] + 1);
  var sheet: array of array of exCell;
  if (w < 0) or (h < 0) then 
    setLength(sheet, 0)
  else begin
    setLength(sheet, h);
    for var i := 0 to h - 1 do sheet[i] := new exCell[w];
    
    foreach var node: XmlNode in elms.Item['sheetData'].ChildNodes do
    begin
      foreach var cell: XmlNode in node.ChildNodes do
      begin
        var (i, j) := cell.Attributes.GetNamedItem('r').Value.ToCR;
        i -= LU[0];
        j -= LU[1];
        var typ := cell.Attributes.GetNamedItem('t');
        var st := cell.Attributes.GetNamedItem('s');
        var shared := false;
        if (typ <> nil) and (typ.Value = 's') then 
          shared := true;
        
        var value := '';
        foreach var vl: XmlNode in cell.ChildNodes do 
          if vl.Name = 'v' then 
            value := vl.InnerText;
        
        var cellValue := new exCell(sheet, i, j);
        sheet[i, j] := cellValue;
        if st <> nil then 
          cellValue.Border := brdStyles[numBrd[st.Value.ToI]];
        
        if shared then 
          cellValue.Value := shStr[value.ToInteger]
        else if (st <> nil) and (numFmt[st.Value.ToI] in DateTypeId) then
          cellValue.Value := (new DateTime(1900, 1, 1)).AddDays(value.ToI - 2).ToString('dd.MM.yyyy')
        else
          cellValue.Value := value;
      end;
    end;
  end;
  Result := sheet;
end;

function ReadXLSXAsCells(fileName: string; sheetNum: integer): array of array of exCell;
begin
  arxiv := ZipFile.OpenRead(fileName);
  
  // общие строки
  var elms := Load('xl/sharedStrings.xml');
  if elms <> nil then 
    foreach var node: XmlNode in elms.ChildNodes do
      shStr.Add(node.Item['t'].InnerText.Trim);
  
  // бордюры
  elms := Load('xl/styles.xml');
  brdStyles := new List<exCellBorder>;
  foreach var node: XmlNode in elms.Item['borders'].ChildNodes do
  begin
    var brd := new exCellBorder;
    brd.IsLeft := node.Item['left'].Attributes.Count > 0;
    brd.IsRight := node.Item['right'].Attributes.Count > 0;
    brd.IsTop := node.Item['top'].Attributes.Count > 0;
    brd.IsBottom := node.Item['bottom'].Attributes.Count > 0;
    brd.IsDiagonal := node.Item['diagonal'].Attributes.Count > 0;
    brdStyles.Add(brd);
  end;
  
  // стили
  numFmt := new List<int>;
  numBrd := new List<int>;
  foreach var node: XmlNode in elms.Item['cellXfs'].ChildNodes do
  begin
    numFmt.Add(node.Attributes.GetNamedItem('numFmtId').Value.ToI);
    numBrd.Add(node.Attributes.GetNamedItem('borderId').Value.ToI);
  end;
  
  elms := Load('xl/workbook.xml');
  foreach var node: XmlNode in elms.Item['sheets'].ChildNodes do
  begin
    var name := node.Attributes.GetNamedItem('name').Value;
    var attrs := new List<string>;
    foreach var attr: XmlAttribute in node.Attributes do 
      attrs.Add(attr.Name);
    var sheetId := node.Attributes.GetNamedItem('r:id').Value[4:];
    Result := LoadSheetAsCells($'xl/worksheets/sheet{sheetId}.xml');
    sheetNum -= 1;
    if sheetNum = 0 then 
      break;
  end;
end;

function ReadXLSX(fileName: string): Dictionary<string, array of array of string>;
begin
  arxiv := ZipFile.OpenRead(fileName);
  
  // общие строки
  var elms := Load('xl/sharedStrings.xml');
  if elms <> nil then 
    foreach var node: XmlNode in elms.ChildNodes do
      shStr.Add(node.Item['t'].InnerText.Trim);
  
  // стили
  elms := Load('xl/styles.xml');
  numFmt := new List<int>;
  foreach var node: XmlNode in elms.Item['cellXfs'].ChildNodes do 
    numFmt.Add(node.Attributes.GetNamedItem('numFmtId').Value.ToI);
  
  elms := Load('xl/workbook.xml');
  Result := new Dictionary<string, array of array of string>;
  foreach var node: XmlNode in elms.Item['sheets'].ChildNodes do
  begin
    var name := node.Attributes.GetNamedItem('name').Value;
    var attrs := new List<string>;
    foreach var attr: XmlAttribute in node.Attributes do 
      attrs.Add(attr.Name);
    var sheetId := node.Attributes.GetNamedItem('r:id').Value[4:];
    Result[name] := LoadSheet($'xl/worksheets/sheet{sheetId}.xml');
  end;
end;


function ReadXLSXAsInts(fileName: string; sheetNum: integer) 
:= ReadXLSX(fileName).Values.Skip(sheetNum).First.Select(r -> r.Select(c -> c.ToInteger).ToArray).ToArray;

end.