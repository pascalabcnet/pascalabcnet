module Main where

length [] = 0
length (h:t) = (length t) + 1

main = do (print(length [1,2,3,4,5,6,7,8]))
          (print(length ["1","2","3"]))
          (print(length [true, true, false,true]))
          