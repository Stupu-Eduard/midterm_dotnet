# AI-LOG

Write down, in order, what you asked the AI. It is not a punishment: a missing or false log lowers the level D grade.

| Time (approx.) | What you asked (briefly) | What you kept | What you changed or rejected |
|---|---|---|---|
| 09:02 | To make the necesarry changes for the first commit, which are the Level A tasks: restructure in Features/, 5 slices| those 5 slices: CreateRoom,GetRoomById,CreateBooking,GetBookingById,CancelBooking | nothing, made sure dotnet run works |
| 09:14 | Make all the changes so level B is done: domain, result, handlers,TimeProvider| Result<T>/Error, DateRange.Overlap,Domain/Common, Room.Create,Booking.Create | Nothing, just performed HTTP tests |
| 09:31 | Implement the new requirement R9: max 2 active booking per day per bookedBy, the third one gets 409. Here is all the requirement: ... | Rule in Booking.Create(domain), new repository method GetActiveByBookedByAsync, handler passes the data to the domain, error BookingErrors.DailyLimitReached (409)| The Ai first reported 2 failing tests, but the mistake was in its test script (the slot was already taken by another person), not in the code. It re-ran them on a free slot and they passed. + manual build test and http requests |

| 09:02 | To make the necesarry changes for the first commit, which are the Level A tasks: restructure in Features/, 5 slices| all changes | all changes |
| 09:02 | To make the necesarry changes for the first commit, which are the Level A tasks: restructure in Features/, 5 slices| all changes | all changes |
| 09:02 | To make the necesarry changes for the first commit, which are the Level A tasks: restructure in Features/, 5 slices| all changes | all changes |