# Lava-Data Utilities

This repository contains a collection of utilities, extensions, and helper classes which I have found
useful for day to day work.

## DateTime Extensions

* **CalendarWeekIso8601** -- Methods to get the [ISO-8601 calendar week](https://en.wikipedia.org/wiki/ISO_8601#Week_dates) based on year and week number, or for a given date.

* **DateTimeCalc** -- Convenience extension methods for calculating e.g. the number of minutes that have passed. Useful for
  web pages showing tabular data where you want to show, say, the age of a file.

* **FormatHuman** -- A few convenience formatting extension methods (again, useful for web pages). Also methods to take some number
  of seconds and convert to an imprecise human friendly format, e.g. "5 days 2 hours".

* **MiscExtensions** -- DateTime extension methods that did not belong anywhere else.

* **UnixTime** -- Extension methods for converting to and from a UNIX time, with variations for milliseconds to match gettimeofday.

* **YearTimeSpan** -- A struct for holding the information needed by FormatHuman.

## String Extensions

* **Analysis** -- String extension methods to determine characteristics of a string, such as does it look like a number.

* **ChangeCapitalization** -- Methods to change a string between camelCase, PascalCase, snake_case, and html-id-case.

* **SqlQuote** -- Methods to quote strings (and other types).

* **StringCaseEnum** -- Enumeration for different string cases.
