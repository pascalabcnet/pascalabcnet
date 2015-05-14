module Main where

func a:b 1 = do { (print 1); }
func a:b n = do { (print a); }

main = do {
            (func [4,5,8,7] 5);
          }