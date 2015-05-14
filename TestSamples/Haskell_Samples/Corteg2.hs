module Main where

head []         =  "Prelude.head: пустой список"
head x:xs       =  x

tail []         =  "Prelude.tail: пустой список"
tail x:xs       =  xs

length [] = 0
length h:t = (length t) + 1

f = [[1,2],[2],[3]]

g = (head [[[1],[2,3]],[2],[3]])

h = (tail [[1,2],[2],[3]])

main = do {
              (print f);
              (print g);
              (print h);
              (print(length(head [[1,2],[2],[3]])));
              (print [[1],[5],[6]]);
          }