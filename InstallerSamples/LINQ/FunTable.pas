// Вывод таблицы значений функции sin

##
PartitionPoints(0,Pi,20).Select(x->$'({x:f4}, {sin(x):f7})').PrintLines;


