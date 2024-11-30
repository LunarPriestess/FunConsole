namespace FunConsole.Core

/// <summary>
/// An abstract class for relaying messages between different systems.
/// </summary>
[<AbstractClass>]
type Message(lifetime) =
    
    /// Whether a message is marked as handled. Messages that are marked as handled are ignored.
    member val IsHandled = false with get, set
    
    /// The lifetime of the message. At the beginning of every game tick messages with lifetime of zero are
    /// deleted.
    member val Lifetime: int = lifetime with get, set
   
    /// <summary>
    /// Decrements the <see cref="Message"/>'s lifetime by one.
    /// </summary>
    abstract member UpdateLifeTime : unit -> unit
    default this.UpdateLifeTime () = this.Lifetime <- this.Lifetime - 1