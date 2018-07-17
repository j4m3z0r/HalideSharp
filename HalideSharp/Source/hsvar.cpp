#include <Halide.h>

using namespace Halide;

extern "C" Var *Var_New_String(const char* name) {
    return new Var(name);
}

extern "C" void Var_Delete(Var *v) {
    delete v;
}

