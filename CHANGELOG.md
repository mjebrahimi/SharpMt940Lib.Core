## New in 1.2.0 (Released 2016/10/23)
A special thank you to [Rob Kent](http://proofbydesign.com/) who made this release possible.
* Changed: Issue #10 - Transaction AccountServicingReference (subfield 8) not parsed
* Changed: Issue #7 - Transaction without :86 information line is not added to the message
* Changed: Issue #14 - Library is broken with en-GB culture and decimal points instead of commas.

## New in 1.1.1 (Released 2015/06/02)
* Added: Change: Money now supports < and > operators.
* Change: TransactionsBalance now requires CultureInfo
* Changed: DateParser now static
* Changed: Widening of cultureinfo to IFormatProvider
* Changed: Parse methods without CultureInfo are now obsolete and produce a warning (or error depending on compiler configuration).
* Changed: Build script is now based on [FAKE](https://fsharp.github.io/FAKE/)
* Removed: The dependency on xunit test runner, it isn't required as the project runs on nunit.
* Removed: Dependency on system.diagnostics.codecontracts it didn't work with code analysis.

## New in 1.1.0 (Released 2014/11/12)
* Fixed: Serveral methods now accept CultureInfo as argument. This fixes a bug where the parsing environment could have a different culture than the imported file which caused comma's to be removed.

## New in 1.0.0 (Released 2014/09/05)
* Added: Improved unit test suite, now includes full parse test of SNS, Abn Amro and Ing Bank. If you can supply more formats I would love to receive them.
* Fixed: [Issue2]: EntryDate is optional. Both enrty month and day should be supplied for this to work.
* Fixed: Transaction.DateTime has been changed to Transaction.DateTime? (Possible breaking change)
* Fixed: Currency != operator was incorrect and could throw a null reference exception.

## New in 0.1.1-RC2 (Released 2014/07/15)
* Fixed: Added Any CPU target assembly.
* Fixed: Package has been split over multiple nuget packages (Any CPU, x86, x64)

## New in 0.1.0 (Released 2014/06/12)
* Added: [Added a validator/visualizer project to the solution](http://raptorious.nl/posts/the-mt940-visualizer.html)
* Added: Introduced the Money object
* Added: [Added the generic format](http://raptorious.nl/posts/the-generic-mt940-format.html)
* Changed: Changed xunit for nunit
* Changed: the namespace from Raptorious.Finance.Swift.Mt940Format to Raptorious.SharpMt940Lib.Mt940Format 
* Fixed: Improved automated testing
* Fixed: [Fixed issue #1: Regex problem in transaction](https://bitbucket.org/raptux/sharpmt940lib/issue/1/regex-problem-in-transactioncs)
* Fixed: Additonal small fixes

## New in 0.0.3-b2 (Released 2012/01/01)
* Added: This version can parse MT940 :)
* Fixed: [Issue2]: https://bitbucket.org/raptux/sharpmt940lib/issue/2/make-entrymonth-and-entryday-optional
* Changes to SharpMT940Lib are documented in this file. 
* Versioning is based on [Semantic Versioning](http://semver.org/).
* This document is based on the guidelines as described on [Keep a Changelog](http://keepachangelog.com/)
