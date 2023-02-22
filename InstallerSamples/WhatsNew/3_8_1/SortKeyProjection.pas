begin
  var pp := |('Сидоров',21),('Петров',23), ('Попов',20),('Иванов',22)|;
  // Сортировка с проекцией на ключ
  Sort(pp, p->p[0]);
  Println('По фамилии');
  pp.Println;
  Sort(pp, p->p[1]);
  Println('По возрасту');
  pp.Println;
end. 