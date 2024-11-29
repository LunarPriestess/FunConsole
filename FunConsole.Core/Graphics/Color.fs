namespace FunConsole.Core.Graphics

[<Struct>]
type Color = { R: byte; G: byte; B: byte; A: byte }
    
[<RequireQualifiedAccess>]
module Colors = 
    let Transparent =   { R = 000uy; G = 000uy; B = 000uy; A = 000uy }    
    let Black =         { R = 000uy; G = 000uy; B = 000uy; A = 255uy }    
    let Red =           { R = 255uy; G = 000uy; B = 000uy; A = 255uy }    
    let Green =         { R = 000uy; G = 255uy; B = 000uy; A = 255uy }    
    let Blue =          { R = 000uy; G = 000uy; B = 255uy; A = 255uy }   
    let Yellow =        { R = 255uy; G = 255uy; B = 000uy; A = 255uy }    
    let Magenta =       { R = 255uy; G = 000uy; B = 255uy; A = 255uy }  
    let Cyan =          { R = 000uy; G = 255uy; B = 255uy; A = 255uy }
    let White =         { R = 255uy; G = 255uy; B = 255uy; A = 255uy }