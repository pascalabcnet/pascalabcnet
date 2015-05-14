module Main where

head []         =  "Prelude.head: пустой список"
head (x:xs)       =  x

f = (head [[[1],[2,3]],[2],[3]])

g = ("321",2,('3',[5]))

main = do (print f)
          (print g)