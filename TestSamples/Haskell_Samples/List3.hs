module Main where

func (a:b:c) = c

func1 (l:w:q) = 5

main = do {
            (print(func [4,5,8,7]));
            (print(func (4,5,8,7)));
          }