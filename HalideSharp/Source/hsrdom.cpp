#include <Halide.h>

using namespace Halide;

extern "C" RDom* RDom_New_IntInt(int min, int extent) { return new RDom(min, extent); }
extern "C" void RDom_Delete(RDom *r) { delete r; }

extern "C" RVar* RDom_GetX(RDom *self) { return new RVar(self->x); }
extern "C" RVar* RDom_GetY(RDom *self) { return new RVar(self->y); }
extern "C" RVar* RDom_GetZ(RDom *self) { return new RVar(self->z); }
extern "C" RVar* RDom_GetW(RDom *self) { return new RVar(self->w); }
