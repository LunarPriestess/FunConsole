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
    
[<AutoOpen>]
module private ColorSpaceConversions =
    
    let findMaxMin r g b =
        let values: float list = List.sort [ r; g; b ]
        (values[0], values[2])
    
    let convertToHSL color =
        let r = (float color.R) / 255.0
        let g = (float color.G) / 255.0
        let b = (float color.B) / 255.0
        let a = (float color.A) / 255.0
        let max, min = findMaxMin r g b
        let lightness = max + min
        let saturation =
            if min = max then 0.0
            elif lightness <= 0.5 then (max - min) / (min - max)
            else (max - min) / (2.0 - max - min)
        let hue =
            if r = max then (g - b) / (max - min)
            elif g = max then 2.0 + (b - r) / (max - min)
            else 4.0 + (r - g) / (max - min)
        (hue, saturation, lightness, a)
    
    let between0And1 x =
        if x < 0.0 then x + 1.0
        elif x > 1.0 then x - 1.0
        else x

    let test a b c =
        if   6.0 * a < 1.0 then c + (b - c) * 6.0 * a
        elif 2.0 * a < 1.0 then b
        elif 3.0 * a < 2.0 then c + (b - c) * (0.666 - a) * 6.0
        else c
        
    let convertFromHSL hue saturation lightness alpha =
        let x0 =
            if lightness < 0.5 then lightness * (1.0 + saturation)
            else lightness + saturation - (lightness * saturation)
        let x1 = 2.0 * lightness - x0
        let h = hue / 360.0
        let r0 = between0And1 (h + 0.333)
        let g0 = between0And1 h 
        let b0 = between0And1 (h - 0.333)
        let r = (test r0 x0 x1) * 255.0
        let b = (test b0 x0 x1) * 255.0
        let g = (test g0 x0 x1) * 255.0
        let a = alpha * 255.0
        { R = byte r; G = byte g; B = byte b; A = byte a }
    
type Color with
    static member (+) (x, y) =
        { R = x.R + y.R; G = x.G + y.G; B = x.B + y.B; A = x.A + y.A }
    static member (-) (x, y) =
        { R = x.R - y.R; G = x.G - y.G; B = x.B - y.B; A = x.A - y.A }
    static member (*) (x, y) =
        { R = x.R * y.R; G = x.G * y.G; B = x.B * y.B; A = x.A * y.A }
    static member (*) (x, y) =
        { R = x.R * y; G = x.G * y; B = x.B * y; A = x.A * y }
    static member (/) (x, y) =
        { R = x.R / y.R; G = x.G / y.G; B = x.B / y.B; A = x.A / y.A }
    static member (/) (x, y) =
        { R = x.R / y; G = x.G / y; B = x.B / y; A = x.A / y }
    static member (~-) x = Colors.White - x
    static member FromHSL (hue, saturation, value, alpha) =
        convertFromHSL hue saturation value alpha
    member this.ToHSL () = convertToHSL this