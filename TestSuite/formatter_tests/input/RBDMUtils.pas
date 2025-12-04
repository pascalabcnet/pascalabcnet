unit RBDMUtils;
// (недокументированные возможности PT4TaskMaker)
interface
//Записывают в файл результатов информацию о выполненном задании для Робота или Чертежника
//taskname - имя задания без префикса RB и DM (префикс добавляется автоматически)
procedure ResRB(taskname: string); 
procedure ResDM(taskname: string); 

//Записывают в файл regtasks.dat в каталоге Lib информацию о новой группе заданий
//для Робота или Чертежника
//Group       - имя группы (от 1 до 7 символов, без префиксов),
//Description - описание группы (первый символ автоматически преобразуется к нижнему
//              регистру, так как описание указывается после двоеточия),
//TaskCount   - число заданий в группе (от 1 до 999),
//Units       - имя модуля, содержащего группу (используется в шаблоне). 
//              Если Units является пустой строкой, то считается, что имя модуля 
//              совпадает с именем группы Group, дополненным префиксом RB или DM.
function RegisterRB(Group, Description: string; TaskCount: integer; 
  Units: string): integer;
function RegisterDM(Group, Description: string; TaskCount: integer; 
  Units: string): integer;

implementation

procedure ResRB(taskname: string); 
  external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'resrb';
procedure ResDM(taskname: string); 
  external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'resdm';
function registerrb(Group, Description: string; TaskCount: integer; 
  Units: string): integer;
  external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'registerrb';
function registerdm(Group, Description: string; TaskCount: integer; 
  Units: string): integer;
  external '%PABCSYSTEM%\PT4\PT4PABC.dll' name 'registerdm';
end.
