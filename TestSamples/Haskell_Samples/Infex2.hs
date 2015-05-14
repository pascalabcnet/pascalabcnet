module Main where

f x = x*x

g y = y+5

h `dot` a = (h a)

main = do (print (g `dot` (f 5)))
          