open System
open System.IO
open Aardvark.Application
open Aardvark.Application.Slim
open Aardvark.Base
open Aardvark.Base.Rendering
open Aardvark.Base.Incremental
open Aardvark.SceneGraph

[<EntryPoint>]
let main argv =

    printfn "Current directory %A" (Directory.GetCurrentDirectory())

    // initialize runtime system
    Ag.initialize(); Aardvark.Init()

    // create simple render window
    //use app = new VulkanApplication()
    use win =
        window {
            backend Backend.GL
            samples 8
            debug false
            initialCamera (CameraView.lookAt (V3d(9.3, 9.9, 8.6)) V3d.Zero V3d.OOI)
        }

    // generate 11 x 11 x 11 colored boxes
    let norm x = (x + 5.0) * 0.1
    let boxes = seq {
        for x in -5.0..5.0 do
            for y in -5.0..5.0 do
                for z in -5.0..5.0 do
                    let bounds = Box3d.FromCenterAndSize(V3d(x, y, z), V3d(0.5, 0.5, 0.5))
                    let color = C4b(norm x, norm y, norm z)
                    yield Sg.box' color bounds
    }

    // define scene
    let sg =
        boxes
            |> Sg.group
            |> Sg.effect [
                DefaultSurfaces.trafo |> toEffect
                DefaultSurfaces.vertexColor |> toEffect
                DefaultSurfaces.simpleLighting |> toEffect
               ]

    // start
    win.Scene <- sg
    win.Run()
    0
