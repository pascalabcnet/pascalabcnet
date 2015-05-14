module Main where

g x y z = x+y+z

h = 3

p = (g 1)

m = (g 1 2)

f = (p 5)

main = do   (print 2)
            (print(g 3 (m 4) 7))
            (print(p 3 5))
            (print(f 5))
          