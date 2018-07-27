#include <string>

#include <Halide.h>

#include "magicmacros.h"

using namespace Halide;

// constructors
#define CONSTRUCTOR(CSTYPE, CPPTYPE) \
    extern "C" Param<CPPTYPE> *ParamOf ## CSTYPE ## _New() { return new Param<CPPTYPE>() ; } \
    extern "C" Param<CPPTYPE> *ParamOf ## CSTYPE ## _New_ ## CSTYPE(CPPTYPE val) { return new Param<CPPTYPE>(val) ; } \
    extern "C" Param<CPPTYPE> *ParamOf ## CSTYPE ## _New_String(char * name) { return new Param<CPPTYPE>(std::string(name)) ; } \
    extern "C" Param<CPPTYPE> *ParamOf ## CSTYPE ## _New_String ## CSTYPE(char * name, CPPTYPE val) { return new Param<CPPTYPE>(std::string(name), val) ; }
GEN(CONSTRUCTOR)

#define DESTRUCTOR(CSTYPE, CPPTYPE) \
    extern "C" void ParamOf ## CSTYPE ## _Delete(Param<CPPTYPE> *self) { delete self; }
GEN(DESTRUCTOR)

#define GETSET(CSTYPE, CPPTYPE) \
    extern "C" CPPTYPE ParamOf ## CSTYPE ## _Get(Param<CPPTYPE> *self) { return self->get(); } \
    extern "C" void ParamOf ## CSTYPE ## _Set_ ## CSTYPE(Param<CPPTYPE> *self, CPPTYPE val) { self->set(val); }
GEN(GETSET)

#define TOEXPR(CSTYPE, CPPTYPE) \
    extern "C" Expr* ParamOf ## CSTYPE ## _ToExpr(Param<CPPTYPE> *self) { return new Expr(*self); }
GEN(TOEXPR)

