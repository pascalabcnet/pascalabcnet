module Main where

[] `plusplus` ys   = ys
x:xs `plusplus` ys = x:(xs `plusplus` ys)

quicksort []           =  []
quicksort x:xs         =  (((quicksort [y | y <- xs, y<x]) 
                          `plusplus` [x]) 
                          `plusplus` (quicksort [y | y <- xs, y>=x]))

main = do (print(quicksort [3,4,2,5,1,10,8,0,6,7,9]))