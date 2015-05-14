module Main where

a `minus` b = a-b 

f = ((`minus` 1) 5)

g = (`minus` 1)

f1 = ((1 `minus`) 5)

g1 = \x -> (1 `minus` x)

main = do { 
              (print f);
              (print(g 9));
              (print f1);
              (print (g1 9));
          }