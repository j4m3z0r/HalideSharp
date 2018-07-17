#include <Halide.h>

using namespace Halide;

#define DEREF_POINTER(a) (*(a))
#define DEREF_NOPOINTER(a) (a)

#define PLUS(a, b) ((a) + (b))
#define MINUS(a, b) ((a) - (b))
#define MULT(a, b) ((a) * (b))
#define DIV(a, b) ((a) / (b))
#define AND(a, b) ((a) && (b))
#define OR(a, b) ((a) || (b))
#define EQUALS(a, b) ((a) == (b))
#define NOTEQUALS(a, b) ((a) != (b))
#define GT(a, b) ((a) > (b))
#define LT(a, b) ((a) < (b))
#define GTE(a, b) ((a) >= (b))
#define LTE(a, b) ((a) <= (b))

#define OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, CSOP, CPPOP, CSTYPE2, CPPTYPE2, DEREF2) \
    extern "C" Expr* Operator_ ## CSTYPE1 ## _ ## CSOP ## _ ## CSTYPE2(CPPTYPE1 operand1, CPPTYPE2 operand2) { \
        return new Expr(CPPOP(DEREF1(operand1), DEREF2(operand2))); \
    }

#define ALL_OPERATORS_FOR_TYPES(CSTYPE1, CPPTYPE1, DEREF1, CSTYPE2, CPPTYPE2, DEREF2) \
    OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, Plus, PLUS, CSTYPE2, CPPTYPE2, DEREF2) \
    OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, Minus, MINUS, CSTYPE2, CPPTYPE2, DEREF2) \
    OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, Mult, MULT, CSTYPE2, CPPTYPE2, DEREF2) \
    OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, Div, DIV, CSTYPE2, CPPTYPE2, DEREF2) \
    OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, And, AND, CSTYPE2, CPPTYPE2, DEREF2) \
    OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, Or, OR, CSTYPE2, CPPTYPE2, DEREF2) \
    OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, Equals, EQUALS, CSTYPE2, CPPTYPE2, DEREF2) \
    OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, NotEquals, NOTEQUALS, CSTYPE2, CPPTYPE2, DEREF2) \
    OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, Gt, GT, CSTYPE2, CPPTYPE2, DEREF2) \
    OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, Lt, LT, CSTYPE2, CPPTYPE2, DEREF2) \
    OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, Gte, GTE, CSTYPE2, CPPTYPE2, DEREF2) \
    OPERATOR(CSTYPE1, CPPTYPE1, DEREF1, Lte, LTE, CSTYPE2, CPPTYPE2, DEREF2)

// NOTE: this will generate some redundant functions, such as operators to add two floats together. However, this
// should be harmless, since the C# code will only include operators that are valid.

#define ALL_OPERATORS_TYPE2(CSTYPE1, CPPTYPE1, DEREF1) \
    ALL_OPERATORS_FOR_TYPES(CSTYPE1, CPPTYPE1, DEREF1, Var, Var*, DEREF_POINTER) \
    ALL_OPERATORS_FOR_TYPES(CSTYPE1, CPPTYPE1, DEREF1, Expr, Expr*, DEREF_POINTER) \
    ALL_OPERATORS_FOR_TYPES(CSTYPE1, CPPTYPE1, DEREF1, Int, int32_t, DEREF_NOPOINTER) \
    ALL_OPERATORS_FOR_TYPES(CSTYPE1, CPPTYPE1, DEREF1, Float, float, DEREF_NOPOINTER)

#define ALL_OPERATORS() \
    ALL_OPERATORS_TYPE2(Var, Var*, DEREF_POINTER) \
    ALL_OPERATORS_TYPE2(Expr, Expr*, DEREF_POINTER) \
    ALL_OPERATORS_TYPE2(Int, int32_t, DEREF_NOPOINTER) \
    ALL_OPERATORS_TYPE2(Float, float, DEREF_NOPOINTER)

ALL_OPERATORS()

