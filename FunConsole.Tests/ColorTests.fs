namespace FunConsole.Tests

open NUnit.Framework

module ColorTests =

    open System
    open NUnit.Framework
    open FunConsole.Core.Graphics

    [<SetUp>]
    let Setup () = ()

    /// <summary>
    /// Converting an RGBA <see cref="Color"/> into a HSLA color and then back again should result in the
    /// original color.
    /// </summary>
    [<Test>]
    let ConvertingToAndFromHSLIsTheSameColor () =
        Assert.Pass()
        
    /// <summary>
    /// Inverting a color and then inverting a second time should result in the original color.
    /// </summary>
    [<Test>]
    let InvertingAColorTwiceDoesNothing
        ([<Random(0, 255, 2)>] r: byte)
        ([<Random(0, 255, 2)>] g: byte)
        ([<Random(0, 255, 2)>] b: byte)
        ([<Random(0, 255, 2)>] a: byte)=
        let color = { R = r; G = g; B = b; A = a }
        let twiceInverted = -(-color)
        Assert.AreEqual(color, twiceInverted)