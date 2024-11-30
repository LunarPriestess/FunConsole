namespace FunConsole.Core

open SFML.Graphics
open SFML.Window

/// <summary>
/// The game object.
/// </summary>
type Game() =
    let mutable gameWindow: RenderWindow option = None
    
    /// <summary>
    /// Creates the game window and sets it's event handlers.
    /// </summary>
    let windowSetup () =
        match gameWindow with
        | None ->
            gameWindow <- Some (new RenderWindow(VideoMode(500u, 500u), "Replace Later"))
            match gameWindow with
            | None -> failwith "The game window could not be created." (* TODO: Create a custom Exception for this. *)
            | Some window -> window.Closed.Add(fun _ -> window.Close())
        | Some _ -> ()

    /// <summary>
    /// Disposes the game window and sets it to None.
    /// </summary>
    let windowCleanUp () =
        match gameWindow with
        | None -> ()
        | Some window ->
            window.Dispose()
            gameWindow <- None
            
    /// <summary>
    /// Runs the game.
    /// </summary>
    member this.Run() =
        windowSetup()
        match gameWindow with
        | None -> failwith "The game window doesn't exist." (* TODO: Create a custom Exception for this. *)
        | Some window -> 
            while window.IsOpen do
                window.DispatchEvents()
                window.Clear()
                window.Display()
            windowCleanUp()