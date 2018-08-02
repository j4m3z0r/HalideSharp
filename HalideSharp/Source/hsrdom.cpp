#include <vector>
#include <utility>

#include <Halide.h>

using namespace Halide;

extern "C" RDom* RDom_New_IntInt(int min, int extent) { return new RDom(min, extent); }
extern "C" RDom* RDom_New_Varargs(int numArgs, Expr** args) {
    std::vector<std::pair<Expr, Expr>> argPairs;
    for(int i = 0; i < numArgs; i += 2) {
        Expr e1 = *args[i];
        Expr e2 = *args[i + 1];

        argPairs.push_back(std::pair<Expr, Expr>(e1, e2));
    }

    return new RDom(argPairs);
}

extern "C" void RDom_Delete(RDom *r) { delete r; }

extern "C" RVar* RDom_GetX(RDom *self) { return new RVar(self->x); }
extern "C" RVar* RDom_GetY(RDom *self) { return new RVar(self->y); }
extern "C" RVar* RDom_GetZ(RDom *self) { return new RVar(self->z); }
extern "C" RVar* RDom_GetW(RDom *self) { return new RVar(self->w); }

