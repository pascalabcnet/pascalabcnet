module Main where

g 1 = 1
g z | z<=10 = x-y
where x=125; y=5
g z | z>10  = x+y
where x=125; y=5

main = do { 
              (print(g 1));
              (print(g 5));
              (print(g 15));
          }