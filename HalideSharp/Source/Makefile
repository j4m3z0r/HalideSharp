TARGET := libhalide-wrapper.so

SOURCES := $(wildcard hs*.cpp)
OBJECTS := $(SOURCES:.cpp=.o)

ECS_SOURCES := $(wildcard HS*.ecs)
ECS_OUTPUTS := $(ECS_SOURCES:.ecs=.out.cs)

LDFLAGS := ...

all: $(TARGET) $(ECS_OUTPUTS)

THIRDPARTY_DIR := $(abspath ../3rdparty)
HALIDE_VERSION := linux-64-gcc53-trunk-46d8e9e0cdae456489f1eddfd6d829956fc3c843
HALIDE_DIR := $(abspath $(THIRDPARTY_DIR)/halide)
HALIDE_LIB := $(HALIDE_DIR)/lib/libHalide.a
HALIDE_INC := $(HALIDE_DIR)/include
HALIDE_TOOLS := $(HALIDE_DIR)/tools

LEMP_VERSION := -trunk
LEMP_DIR := $(abspath $(THIRDPARTY_DIR)/LeMP)
LEMP_BIN := $(LEMP_DIR)/LeMP.exe

CXXFLAGS := -O2 -g -I$(HALIDE_INC) -I$(HALIDE_TOOLS) `libpng-config --cflags` -fPIC
LDFLAGS := $(HALIDE_LIB) `libpng-config --ldflags` -ljpeg -lpthread -ldl -fPIC

CXX := g++ -std=c++11
MONO := /usr/bin/mono

$(HALIDE_LIB): $(THIRDPARTY_DIR)/halide-$(HALIDE_VERSION).tgz
	(rm -rf $(HALIDE_DIR))
	(cd $(THIRDPARTY_DIR); tar xzvf $<)
	touch $(HALIDE_LIB)

$(LEMP_BIN): $(THIRDPARTY_DIR)/LeMP$(LEMP_VERSION).zip
	(rm -rf $(LEMP_DIR))
	(cd $(THIRDPARTY_DIR); unzip $<)
	touch $(LEMP_BIN)

THIRDPARTY_LIBS := $(HALIDE_LIB)

$(TARGET): $(OBJECTS) $(THIRDPARTY_LIBS)
	$(CXX) -o $@ $^ -shared -Wl,--exclude-libs,ALL $(LDFLAGS)

%.o: %.cpp $(THIRDPARTY_LIBS)
	$(CXX) -c $(CXXFLAGS) -o $@ $<

%.out.cs: %.ecs $(LEMP_BIN)
	$(MONO) $(LEMP_BIN) --preserve-comments --outext=out.cs --inlang=ecs --forcelang $<

clean:
	rm -f $(TARGET) $(OBJECTS) $(ECS_OUTPUTS)
	rm -rf $(HALIDE_DIR) $(LEMP_DIR)
