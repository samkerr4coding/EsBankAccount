﻿[<RequireQualifiedAccess>]
/// A fake message bus
module EsBankAccount.App.MessageBus

let createEventHandler (handle: 'StreamId -> 'Event -> 'State -> Async<unit>) =
    MailboxProcessor<'StreamId * 'Event * 'State>.Start (fun inbox ->
        let rec loop () =
            async {
                let! accountId, event, state = inbox.Receive ()
                do! handle accountId event state
                return! loop ()
            }
        loop ()
    )
