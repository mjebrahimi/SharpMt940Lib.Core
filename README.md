#SharpMT940Lib#
##What is it?##

SharpMT940Lib implements the MT940 format in C# based on specifications by ABN AMRO. You can use it as a base for financial software or for conversions to other formats like CSV or OFX.

Like many other banks ABN AMRO gives customers the possibility to download financial transactions to this format. I wanted to use this export format to manage my personal finance. So I search for different solutions to import this file format. Most (free) software I evaluated couldn’t directly import the format, you had to convert it to something else first, such as OFX.

Since I couldn’t find an application that didn’t precisely do what I wanted I decided to write my own. But first I needed a library to read the MT940 format.

MT940 Customer Statement Message is a plain text financial format standardized by SWIFT. Banks use this format to export to financial packages can use this for their process.

You can find more information (in dutch) at the following places:

* [MT940 ING Specificiation](http://www.ing.nl/Images/MT940_Technische_handleiding_%20tcm7-69020.pdf)
* [Rabobank MT940 Specification](http://www.rabobank.nl/images/toelichting_op_swift_mt-940_juli_2008_%2029131642.pdf)
* [ABN Amro MT940 Specification](http://www.abnamro.nl/nl/images/Generiek/PDFs/020_Zakelijk/03_%20OfficeNet/Formatenboek_%20MT94_%%2028nederlands%%2029.pdf)

##Sources##
I use Bitbucket as my source control provider, you can find the sources in my repository.

If you know the mt940 format a bit then the code will be obvious. Otherwise just mail me, leave a reply or checkout the code.

[![Build status](https://ci.appveyor.com/api/projects/status/b2fh43sh1g0gaka3?svg=true)](https://ci.appveyor.com/project/raptux/sharpmt940lib)


##Documentation##
Generated sandcastle documentation is available. [Checkout my blog](http://www.raptorious.nl/) for new documentation, tips and tutorials.

##Download##
You can get the prerelease package by using [nuget](https://www.nuget.org/packages/Raptorious.Finance.Swift.Mt940/). For changes checkout the [changelog](http://rptx.nl/mt940-changelog)

##Are you missing a feature or is it broken?##
Please contact me or leave a reply if you are missing a feature or when you run into a bug. If I know about it, then I can fix it. You can report them at the [issue tracker](https://bitbucket.org/raptux/sharpmt940lib/issues). Please note I work alone at this project, so fixes sometimes might take some time.

Also I do not have access to every MT940 format. Different banks can have different headers and footers, the AbnAmro class might work for your file also. If it does or does not, please let me know so I can improve the code.

##Author##
[Jaco Adriaansen](http://adriaansen.org/) [(@raptux)](https://twitter.com/raptux)

##License##
SharpMT940Lib has been given an MIT license. This is an open source approved license. You can use the library freely in both open source software and commercial software.