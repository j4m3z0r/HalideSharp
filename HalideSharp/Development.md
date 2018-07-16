# HalideSharp Development

This document contains a few important notes about how HalideSharp (HS) works. Since Halide is a C++ library, exposing
its functionality to C# requires an intermediate layer to expose the C++ functionality with non-mangled names. This
intermediate layer is implemented in .cpp files with names parallel to their C# interfaces. For example, hsfunc.cpp
exposes the functionality that is represented by the HSFunc.cs file.

Given the large number of combinations of type arguments that Halide is able to work with, macros are used to generate
both the intermediate C/C++ layer, *and* the C# classes. In C++, the standard C preprocessor is used. In C#, LeMP is
used. LeMP is a preprocessor for C# that provides lexical macros, a la Lisp. A copy of LeMP is included in the 3rdparty
directory, and is invoked by BeforeBuild jobs in HalideSharp's project file.

Whilst this approach means that both the C++ and C# code is generated using tools that are native to each language, the
trade-off is that we must rely on naming conventions to ensure that the two can interoperate.

## Naming Conventions

### C# Classes

All the C# classes are named with a HS prefix, in addition to being part of the "HalideSharp" namespace. The reason for
this is pragmatic: Halide exposes classes with names that match those in C#'s standard library and are somewhat likely
to be used by callers in the same contexts as Halide. For instance, `System.Func`. It would be possible to proceed by
fully qualifying all uses of both class names, but including a prefix on HalideSharp classes seemed like it would be
less work for the developer.

As a general rule, most classes will contain a `_cppobj` `IntPtr` that points to the heap allocated C++ object that it
represents. In general the C++ object will be allocated in the C# class's constructor, and will be deleted by the C#
class's destructor.

In addition there are a handful of utility classes. Most notably `HS` and `HSMath`. `HSMath` provides various math
functions. In C++ these functions are globally accessible, however C# does not have global functions. HSMath was chosen
as the home for these functions to mirror C#'s own `Math` class. `HS` exposes other functionality that is globally
accessible in C++, but is not directly math related, such as `print_when` and `cast<>`.

### Mapping C# methods to C Function Names

#### Invoking Methods on Objects

The C# method names have been chosen to essentially map the C++ method names to C#'s standard naming conventions. For
example, `print_when` is exposed as `PrintWhen`. The C methods in the intermediate layer will likewise follow C# naming
conventions. This is to save having to explicitly list out `EntryPoint` values for every `DllImport`. The exception to
this is that Halide types are referred to by their C++ name, since there will be no collisions with C# types in C++!

The basic convention for naming of C methods is as follows:

    <Object Type>_<Method name>_<Arg 1 type><Arg 2 type>...

No attempt is made to encode the return type in the method name, since it is assumed that the arguments will uniquely
identify a function which will have only a single return type. For types that are templated, the Object Type will be

    <Base Type>Of<Templated Type>

For example, `Buffer<int>` will be mapped to `BufferOfInt`.

In order to maintain C# naming, primitive types such as `int` are capitalized to `Int`. Likewise, primitive types are
named by their capitalized C# names, so `Byte` is used, rather than `uint8_t` (for example). This is primarily because
aliasing types is easier in C++ than C#.

#### Constructors and Destructors

Constructors have the special method name `New`, and destructors `Delete`. For example:

    BufferOfInt_New_IntInt()

would construct a new Buffer<int32_t> with 2 dimensions.

    BufferOfInt_Delete

would destroy a Buffer<int32_t>

#### Arguments

Halide objects are always passed by pointer, and primitive types are always passed by value.

#### Operator Overloading

Functions exposing a binary operator are named like so:

    Operator_<Type 1>_<Operator Name>_<Type 2>

For example, the method exposing the add operator of an Expr and an int would be named `Operator_Expr_Plus_Int`. The
following names are used for operators:

* operator- : Minus
* operator+ : Plus
* operator* : Mult
* operator/ : Div
* operator&& : And
* operator|| : Or
* operator> : Gt
* operator< : Lt
* operator>= : Gte
* operator<= : Lte

Note that C# does not provide a way to overload operator&& and operator|| directly, only operator& and operator|, which
C# will then implement short-circuiting on top of. This is problematic since we want both arguments of these operators
to be passed to C++, irrespective of how they might evaluate in C#. To implement this, classes that can be used as
arguments to || or && have operator true *and* operator flase defined to return false. The C# spec includes wording
that this will force all short-circuited evaluations to pass both arguments.
