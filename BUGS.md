# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R5 | BookingService.cs, create method did not exist (lines 8-45). Doesn't exist not a single check for start in the past and timeProvider | Ai + testing after: Checked with a request with a date from 2019: before you got 201, now you get 400 | Checking is in Create, and compares with nowUTC. The handler CreateBookingHandler takes now from TimeProvider, registered in Program.cs. The error is BookingErrrors.StartInPast(400) |
| R6 |  | _(fill in)_ | _(fill in)_ |
| _(fill in)_ | _(fill in)_ | _(fill in)_ | _(fill in)_ |
| _(fill in)_ | _(fill in)_ | _(fill in)_ | _(fill in)_ |

