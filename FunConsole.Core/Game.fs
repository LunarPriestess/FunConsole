namespace FunConsole.Core

open SFML.Graphics
open SFML.Window

type GameConfiguration =
    { WindowTitle: string; Width: uint; Height: uint; DefaultFont: unit }

/// <summary>
/// The game object.
/// </summary>
type Game(config) =
    let configuration = config    
    let mutable gameWindow: RenderWindow option = None
    
    /// <summary>
    /// Creates the game window and sets it's event handlers.
    /// </summary>
    let windowSetup () =
        match gameWindow with
        | None ->
            let vm = VideoMode(config.Width, config.Height)
            gameWindow <- Some (new RenderWindow(vm, config.WindowTitle))
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