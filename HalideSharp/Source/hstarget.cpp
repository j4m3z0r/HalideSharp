#include <Halide.h>

#define LANGUAGE_C 1
#include "SharedEnums.cs"
#undef LANGUAGE_C

using namespace Halide;

extern "C" Target* Target_New() { return new Target(); }
extern "C" void Target_Delete(Target *t) { delete t; }

extern "C" HSOperatingSystem Target_GetOperatingSystem(Target* t) { return (HSOperatingSystem) t->os; }
extern "C" void Target_SetOperatingSystem_OperatingSystem(Target *t, Target::OS os) { t->os = os; }

extern "C" HSArchitecture Target_GetArchitecture(Target* t) { return (HSArchitecture) t->arch; }
extern "C" void Target_SetArchitecture_Architecture(Target *t, Target::Arch arch) { t->arch = arch; }

extern "C" int Target_GetBits(Target *t) { return t->bits; }
extern "C" void Target_SetBits_Int(Target *t, int bits) { t->bits = bits; }

extern "C" void Target_SetFeature_Feature(Target *t, HSFeature f) { t->set_feature((enum Target::Feature) f); }

