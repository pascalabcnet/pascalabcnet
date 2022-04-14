unit XLSX;

interface

function ReadXLSX(fileName: string): Dictionary<string, array of array of string>;
function ToDate(Self: string): DateTime;

implementation

{$reference System.IO.Compression.dll}
{$reference System.IO.Compression.FileSystem.dll}
{$reference System.Xml.dll}

uses System.IO.Compression;
uses System.Xml,sf;

var
  numFmt := new List<int>;  
  arxiv: System.IO.Compression.ZipArchive;
  shStr := new List<string>;
  DateTypeId := |14, 15, 16, 17, 22, 27, 30, 36, 50, 55, 57|.ToHS;

function ToDate(Self: string): DateTime;
begin
  Result := DateTime.Parse(Self);
end;

function ToCR(Self: string); extensionmethod 
:= (self[2:].ToI - 1, self[1] - 'A'); // 'B3' -> [1,2]

function Load(path: string): XmlElement;
begin
  if arxiv.Entries.Any(e -> (e as ZipArchiveEntry).FullName = path) then begin
    var stream := arxiv.GetEntry(path).Open;
    var doc := new XmlDocument();
    doc.Load(stream);
    Result := doc.DocumentElement;
  end;
end;

function LoadSheet(path: string): array of array of string;
begin
  var elms := Load(path);
  var dim := elms.Item['dimension'];
  var LU, RD: (int, int);
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
  end else // готовые габариты из таблицы
    (LU, RD) := dim.GetAttribute('ref').Split(':').Sel(s -> s.ToCR);
  
  var (w, h) := (RD[1] - LU[1] + 1, RD[0] - LU[0] + 1);
  var sheet: array of array of string;
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
  Result := sheet;
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

end.