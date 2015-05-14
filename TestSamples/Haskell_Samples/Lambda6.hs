module Main where

g x y z t = x+y+z+t

p x = (g x)

main = do {
            (print(p 1 2 3 4));
          }