#include <Halide.h>

#include "magicmacros.h"

using namespace Halide;

#define CONSTRUCTOR_IMAGEPARAM(CSTYPE, CPPTYPE) \
    extern "C" Argument *Argument_New_ImageParamOf ## CSTYPE(ImageParam *ip) { return new Argument(*ip); }
#define CONSTRUCTOR_PARAM(CSTYPE, CPPTYPE) \
    extern "C" Argument *Argument_New_ParamOf ## CSTYPE(Param<CPPTYPE> *p) { return new Argument(*p); }

GEN(CONSTRUCTOR_IMAGEPARAM)
GEN(CONSTRUCTOR_PARAM)

extern "C" void Argument_Delete(Argument *a) { delete a; }
