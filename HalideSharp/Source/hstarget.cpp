#include <Halide.h>

#define LANGUAGE_C 1
#include "SharedEnums.cs"
#undef LANGUAGE_C

using namespace Halide;

extern "C" void Target_Delete(Target *t) { delete t; }
extern "C" HSOperatingSystem Target_GetOperatingSystem(Target* t) { return (HSOperatingSystem) t->os; }
extern "C" void Target_SetFeature_Feature(Target *t, HSFeature f) { t->set_feature((enum Target::Feature) f); }

