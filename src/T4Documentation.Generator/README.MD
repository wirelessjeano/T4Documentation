# T4Documentation.Generator

T4Documentation.Generator is a T4 template for .NET projects that creates documentation from source code comments.
# Getting Started

Install the Nuget package "T4Documentation.Generator" into your project:

[![Nuget](https://img.shields.io/nuget/v/T4Documentation.Generator.svg?style=flat-square)](https://www.nuget.org/packages/T4Documentation.Generator/)

The package will install the file "T4Documentation.tt" into the root of your project. Right-click the file and select "Run Custom Tool".

It will generate the file "T4Documentation.txt" which will contain your mark-up. Copy & paste it's contents into your WIKI.

[![Build status](https://ci.appveyor.com/api/projects/status/rdsurmfpxpd9e5ae?svg=true)](https://ci.appveyor.com/project/wirelessjeano/t4documentation)

# Features

- Supports both MediaWiki & Markdown text-to-HTML conversion engines.
- Supports the parsing of comments on classes, interfaces and structs.
- Supports the parsing of comments on properties & methods of the above programming structures.
- Support for summary, example and code tags ONLY. ([See Recommended Tags for Documentation Comments](https://msdn.microsoft.com/en-us/library/5ast78ax.aspx))
- Support the splitting documentation into multiple files for easy copy & paste.

# Example

The following class is followed by it's Markdown output:

The class:
~~~
using System;
using System.Collections.Generic;


namespace T4Documentation.Generator.Tests
{
    /// <summary>
    /// A programming structure that represents a person.
    /// </summary>
    /// <code>
    /// var p = new Person();
    /// </code>
    public class Person
    {
        /// <summary>
        /// This is the name of a person.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This is the height of a person.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Makes a person think
        /// </summary>
        /// <code>
        /// var p = new Person();
        /// p.Think();
        /// </code>
        public void Think()
        {
            //Do something
        }

        /// <summary>
        /// Determine if a person is willing to dance
        /// </summary>
        /// <code>
        /// var p = new Person();
        /// var willingnessToDance = p.GetWillingnessToDance();
        /// </code>
        /// <returns></returns>
        public bool GetWillingnessToDance()
        {
            return true;
        }

    }
}
~~~

Generates this Markdown syntax:

# Tests
## Classes
### Person Class
Namespace: T4Documentation.Generator.Tests

Summary: A programming structure that represents a person.

- Using: System

- Using: System.Collections.Generic

#### Example Code
~~~
var p = new Person();
~~~

Access | Signature | Description
--- | --- | ---
Public | int Height | This is the height of a person.
Public | string Name | This is the name of a person.

#### Methods
Access | Signature | Description
--- | --- | ---
Public | bool GetWillingnessToDance() | Determine if a person is willing to dance
Public | void Think() | Makes a person think

#### Method Example(s)
~~~
var p = new Person();
var willingnessToDance = p.GetWillingnessToDance();
~~~

~~~
var p = new Person();
p.Think();
~~~

#Switching Between MediaWiki & Markdown Providers

Find the following line in the "T4Documentation.tt" file:

&lt;#@ Include File="T4Documentation.tt.MediaWiki.provider.t4" #&gt;

Change it to:

&lt;#@ Include File="T4Documentation.tt.MarkDown.provider.t4" #&gt;


# Limitations

- Only tested in Visual Studio 2015
- Does not support parsing of enums (yet).
- Does not support non-XML compliant characters within the comment tags such as:
~~~
        /// <summary>
        /// The character '&' will fail.
        /// It needs to be '&amp;'
        /// </summary>
~~~
- Does not support parsing of embeded tags. eg; a code tag with a summary tag such as:

~~~
        /// <summary>
        /// The GetZero method. Here is an example:
        /// <code>
        /// class TestClass 
        /// {
        ///     static int Main() 
        ///     {
        ///         return GetZero();
        ///     }
        /// }
        /// </code>
        /// </summary>
~~~

# Settings

Settings are contained in the "T4Documentation.tt.settings.xml" file. See for comments.


# License

This is free software, licensed under: [The MIT License (MIT)](https://opensource.org/licenses/MIT)



