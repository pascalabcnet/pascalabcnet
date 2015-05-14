begin
  var d := Dict('begin'=>0, 'end'=>0);
  var ss := ReadAllText('14_FilesStr1.pas').ToWords().Where(s-> (s='begin') or (s='end'));
  ss.Println;
end.