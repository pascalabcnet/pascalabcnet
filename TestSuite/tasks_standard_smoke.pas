//nopabcrtl
// Tasks and LightPT are standalone standard units, not part of PABCRtl.dll.
uses LightPT, Tasks1Arr, Tasks1Begin, Tasks1BoolIfCase, Tasks1Loops,
  TasksArr, TasksMatr, TasksStr, Tasks;

begin
  assert(Tasks1Arr.AllTaskNames.Length > 0);
  assert(Tasks1Begin.AllTaskNames.Length > 0);
  assert(Tasks1BoolIfCase.AllTaskNames.Length > 0);
  assert(Tasks1Loops.AllTaskNames.Length > 0);
  assert(TasksArr.AllTaskNames.Length > 0);
  assert(TasksMatr.AllTaskNames.Length > 0);
  assert(TasksStr.AllTaskNames.Length > 0);
  TaskResult := NotUnderControl;
  Tasks1Begin.CheckTaskT('Var_5');
  assert(TaskResult = Solved);
  TaskResult := Solved;
  Tasks.CheckTaskT('tasks_smoke');
  assert(TaskResult = NotUnderControl);
end.
