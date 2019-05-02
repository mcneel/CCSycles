C[CS]?ycles : CCycles and CSycles for Cycles
============================================

C[CS]?ycles aims to provide a C API around Cycles. Building on CCycles also a
C# wrapper is available.

*Note 1* Cycles code is forked at https://github.com/mcneel/cycles. Clone this
CCSycles repository recursively so that the submodules get initialized and pulled
correctly as well.

*Note 2* Use the `standalone` branch to work from

*Note 3* Download the boost.zip archive from

https://drive.google.com/open?id=1hqhWd5ZZ38-Bn7U_JE0SGgNwIi1ZPKFJ

if you want to use the Boost that I use. Extract its contents outside of the root
of this repository. So if you have C:\Dev\CCSycles, then you should end up with
C:\Dev\boost, which is the top folder contained in the zip.

*Note 4* No OSL support effort has been made, as for the RhinoCycles plugin the
focus is on CUDA support.

*Note 5* The C[CS]?ycles main developer is Nathan Letwory. You can contact him
at nathan@mcneel.com or find him as jesterKing in IRC channel #blendercoders of
the Freenode network.

ROADMAP / TODO
==============

See Notes above, most are TODO items that need to be tackled in a useful way
before C[CS]?ycles is ready for upstream. In addition:

* Add the rest of shader and texture nodes
* Documentation
* Documentation
* Improve csycles_tester to do complete XML support

Cycles and dependencies
=======================

This folder contains the Cycles source code, C-API source code, C# wrapper
source code and source code for the necessary dependencies pthreads, glew,
clew, cuew and a modified OpenImageIO.

The Cycles source
=================

The Cycles source code is added as a sub-module at the root of this repository.

OpenImageIO tools
=================

Only a small part of the OpenImageIO library is used on the Rhino version
of Cycles. Image loading is handled by Rhino existing image code. The
OpenImageIO DLL is named `OpenImageIOv13.dll`.

pthreads
========

pthreads contains the threading library used by Cycles. The library is
compiled as a DLL.

clew
====

clew is the OpenCL execution wrangler library

cuew
====

cuew is the CUDA execution wrangler library

C-API and C# wrapper code
=========================

* ccycles (C API)
* csycles (C# wrapper around CCycles)
* csycles_tester (C# tester program, reimplementation of Cycles
                standalone)
* csycles_diag (C# diagnostics program, text output only)

Building
========

1. Clone repository, init submodule and pull cycles code as well
2. Get Boost and extract it to the root of the repository, rename the folder to
boost/
3. Open cycles.sln
4. Select one of the stand-alone configurations (debug or release)
4. Build solution for csycles_tester
5. run csycles_tester, it as very simple GUI program. It will list in its menu
   any XML that is in its directory

License for CCycles and CSycles
===============================

Copyright 2014-2019 Robert McNeel and Associates

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
