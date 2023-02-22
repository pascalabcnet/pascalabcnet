var dict := new SortedDictionary<string,List<string>>;

function DictToItems: string;
begin
  var sb := new StringBuilder;
  foreach var kv in dict do
  begin
    sb.AppendLine('<LI> <OBJECT type="text/sitemap">');
    sb.AppendLine($'  <param name="Name" value="{kv.Key}">');
    foreach var v in kv.Value do
      sb.AppendLine($'  <param name="Local" value="{v}">');
    sb.AppendLine('</OBJECT>');
  end;
  Result := sb.ToString;  
end;  

begin
  var tt := ReadAllText('Table of Contents New.hhc');
  var filesmatch := tt.Matches('<param name="Local" value="([\w\\\. ]*)">');
  foreach var fmatch in filesmatch do
  begin  
    var fname := fmatch.Groups[1].Value;
    //Println(fname);
    var txt := ReadAllText('.\'+fname);
    var mm := txt.Matches('<param name="Keyword" value="([\w\\\. ]*)">'); 
    foreach var m in mm do
    begin  
      var keyword := m.Groups[1];
      var s := keyword.ToString.Trim;
      if s.Length > 0 then
      begin  
        if s not in dict then
          dict[s] := new List<string>;
        dict[s].Add(fname)
      end;  
    end;
  end;
  //dict.PrintLines;
  
var s := 
'<!DOCTYPE HTML PUBLIC "-//IETF//DTD HTML//EN">'+NewLine+
'<HTML>'+NewLine+
'<HEAD>'+NewLine+
'<meta http-equiv="Content-Type" content="text/html; charset=windows-1251">'+NewLine+
'<!-- Sitemap 1.0 -->'+NewLine+
'</HEAD><BODY>'+NewLine+
'<UL>'+NewLine+
DictToItems + 
'</UL>'+NewLine+
'</BODY></HTML>'
;

  WriteAllText('Index.hhk',s);
end.