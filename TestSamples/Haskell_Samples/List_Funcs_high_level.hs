module Main where

f (h:t) y = t

g a = ((a [4,5,6]) 7)

main = do {
            (print(f [1,2,3] 3));
            (print(g f));
          }