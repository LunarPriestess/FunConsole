namespace FunConsole.Core.Graphics

/// <summary>
/// An 8-bit depth RGBA Color.
/// </summary>
[<Struct>]
type Color = { R: byte; G: byte; B: byte; A: byte } 

/// <summary>
/// A collection of default <see cref="Color"/>s.
/// </summary>
[<RequireQualifiedAccess>]
module Colors =
    /// <summary>
    /// A completely transparent color. Hex: 0x0000
    /// </summary>
    let Transparent =   { R = 000uy; G = 000uy; B = 000uy; A = 000uy }    
    /// <summary>
    /// The color black. Hex: 0x000F
    /// </summary>
    let Black =         { R = 000uy; G = 000uy; B = 000uy; A = 255uy }    
    /// <summary>
    /// The color Red. Hex: 0xF00F
    /// </summary>
    let Red =           { R = 255uy; G = 000uy; B = 000uy; A = 255uy }    
    /// <summary>
    /// The color green. Hex: 0x0F0F
    /// </summary>
    let Green =         { R = 000uy; G = 255uy; B = 000uy; A = 255uy }    
    /// <summary>
    /// The color blue. Hex: 0x00FF
    /// </summary>
    let Blue =          { R = 000uy; G = 000uy; B = 255uy; A = 255uy }   
    /// <summary>
    /// The color yellow. Hex: 0xFF0F
    /// </summary>
    let Yellow =        { R = 255uy; G = 255uy; B = 000uy; A = 255uy }    
    /// <summary>
    /// The color magenta. Hex: 0xF0FF
    /// </summary>
    let Magenta =       { R = 255uy; G = 000uy; B = 255uy; A = 255uy }  
    /// <summary>
    /// The color cyan. Hex: 0x0FFF
    /// </summary>
    let Cyan =          { R = 000uy; G = 255uy; B = 255uy; A = 255uy }
    /// <summary>
    /// The color white. Hex: 0xFFFF
    /// </summary>
    let White =         { R = 255uy; G = 255uy; B = 255uy; A = 255uy }
    
    (* TODO: Support more colors. *)

/// <summary>
/// A collection of functions for converting between different color spaces.
/// </summary>
[<AutoOpen>]
module private ColorSpaceConversions =
    
    /// <summary>
    /// Finds the maximum and minimum values of the three RGB color channels.
    /// </summary>
    /// <param name="r">The value of the red channel.</param>
    /// <param name="g">The value of the green channel.</param>
    /// <param name="b">The value of the blue channel.</param>
    /// <returns>A tuple of the form (max, min).</returns>
    let findMaxMin r g b =
        let values: float list = List.sort [ r; g; b ]
        (values[0], values[2])
    
    /// <summary>
    /// Converts a given <see cref="Color"/> into HSLA.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>A tuple of the form (Hue, Saturation, Lightness, Alpha).</returns>
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
    
    /// <summary>
    /// Checks to see if the given <see cref="float"/> is between 0 and 1.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>
    /// The value itself if its between 0 and 1, otherwise it returns the value +/- 1 if its below 0 or above 1. 
    /// </returns>
    let between0And1 value =
        if value < 0.0 then value + 1.0
        elif value > 1.0 then value - 1.0
        else value

    /// <summary>
    /// Does a bunch of tests to determine what value to use for the color channel.
    /// </summary>
    let test a b c =
        if   6.0 * a < 1.0 then c + (b - c) * 6.0 * a
        elif 2.0 * a < 1.0 then b
        elif 3.0 * a < 2.0 then c + (b - c) * (0.666 - a) * 6.0
        else c
        
    /// <summary>
    /// Converts an HSLA color to an RGBA <see cref="Color"/>.
    /// </summary>
    /// <param name="hue">The color's hue.</param>
    /// <param name="saturation">The color's saturation.</param>
    /// <param name="lightness">The color's lightness.</param>
    /// <param name="alpha">The color's alpha.</param>
    /// <returns>An RGBA <see cref="Color"/>.</returns>
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
    /// <summary>
    /// Adds each channel of the first <see cref="Color"/> by the channels of the second.
    /// </summary>
    /// <param name="c0">The first <see cref="Color"/>.</param>
    /// <param name="c1">The second <see cref="Color"/>.</param>
    /// <returns>A new <see cref="Color"/> record.</returns>
    static member (+) (c0, c1) =
        { R = c0.R + c1.R; G = c0.G + c1.G; B = c0.B + c1.B; A = c0.A + c1.A }
        
    /// <summary>
    /// Subtracts each channel of the first <see cref="Color"/> by the channels of the second.
    /// </summary>
    /// <param name="c0">The first <see cref="Color"/>.</param>
    /// <param name="c1">The second <see cref="Color"/>.</param>
    /// <returns>A new <see cref="Color"/> record.</returns>
    static member (-) (c0, c1) =
        { R = c0.R - c1.R; G = c0.G - c1.G; B = c0.B - c1.B; A = c0.A - c1.A }
        
    /// <summary>
    /// Multiplies each channel of the first <see cref="Color"/> by the channels of the second.
    /// </summary>
    /// <param name="c0">The first <see cref="Color"/>.</param>
    /// <param name="c1">The second <see cref="Color"/>.</param>
    /// <returns>A new <see cref="Color"/> record.</returns>
    static member (*) (c0, c1) =
        { R = c0.R * c1.R; G = c0.G * c1.G; B = c0.B * c1.B; A = c0.A * c1.A }
        
    /// <summary>
    /// Multiplies each channel of the <see cref="Color"/> by the given byte.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> whose channels will be multiplied.</param>
    /// <param name="value">The value to multiply by.</param>
    /// <returns>A new <see cref="Color"/> record.</returns>    
    static member (*) (color, value) =
        { R = color.R * value; G = color.G * value; B = color.B * value; A = color.A * value }
        
    /// <summary>
    /// Divides each channel of the first <see cref="Color"/> by channels of the second.
    /// </summary>
    /// <param name="c0">The first <see cref="Color"/>.</param>
    /// <param name="c1">The second <see cref="Color"/>.</param>
    /// <returns>A new <see cref="Color"/> record.</returns>
    static member (/) (c0, c1) =
        { R = c0.R / c1.R; G = c0.G / c1.G; B = c0.B / c1.B; A = c0.A / c1.A }
    
    /// <summary>
    /// Divides each channel of the <see cref="Color"/> by the given byte.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> whose channels will be divided.</param>
    /// <param name="value">The value to divide by.</param>
    /// <returns>A new <see cref="Color"/> record.</returns>
    static member (/) (color, value) =
        { R = color.R / value; G = color.G / value; B = color.B / value; A = color.A / value }
    
    /// <summary>
    /// Inverts the given <see cref="Color"/>.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to be be inverted.</param>
    /// <returns>The inverted <see cref="Color"/>.</returns>
    /// <remarks>The alpha value of <paramref name="color"/> is not effected.</remarks>
    static member (~-) color =
        let inverted = Colors.White - color
        { inverted with A = color.A }
        
    /// <summary>
    /// Converts an HSLA color to an RGBA <see cref="Color"/>.
    /// </summary>
    /// <param name="hue">The color's hue.</param>
    /// <param name="saturation">The color's saturation.</param>
    /// <param name="lightness">The color's lightness.</param>
    /// <param name="alpha">The color's alpha.</param>
    /// <returns>An RGBA <see cref="Color"/>.</returns>
    static member FromHSL (hue, saturation, lightness, alpha) =
        convertFromHSL hue saturation lightness alpha
    
    /// <summary>
    /// Converts the <see cref="Color"/> from RGBA to HSLA.
    /// </summary>
    /// <returns>A tuple of the form (Hue, Saturation, Lightness, Alpha).</returns>
    member this.ToHSL () = convertToHSL this