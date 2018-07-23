#include <Halide.h>

using namespace Halide;

extern "C" RDom* RDom_New_IntInt(int min, int extent) { return new RDom(min, extent); }
extern "C" void RDom_Delete(RDom *r) { delete r; }

