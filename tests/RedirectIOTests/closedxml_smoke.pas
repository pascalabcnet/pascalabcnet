// TEST-REGRESSION: console redirection and the NuGet Packaging dependency coexist.
{$package ClosedXML 0.105.1}
uses __RedirectIOMode, System, ClosedXML.Excel;
begin
  var fileName := 'redirect-test.xlsx';
  var workbook := new XLWorkbook;
  var sheet := workbook.Worksheets.Add('Students');
  sheet.Cell(1, 1).Value := 'Alice';
  sheet.Cell(1, 2).Value := 92;
  workbook.SaveAs(fileName);
  workbook.Dispose;
  var reopened := new XLWorkbook(fileName);
  assert(reopened.Worksheet('Students').Cell(1, 1).GetString = 'Alice');
  reopened.Dispose;
  assert(not AppDomain.CurrentDomain.GetAssemblies.Any(a -> a.GetName.Name = 'System.Windows.Forms'));
  Println('redirect-closedxml_smoke-ok');
end.
