TARGET := libhalide-wrapper.so

SOURCES := $(wildcard hs*.cpp)

OBJECTS := $(SOURCES:.cpp=.o)

LDFLAGS := ...

all: $(TARGET)

THIRDPARTY_DIR := $(abspath ../3rdparty)
HALIDE_VERSION := linux-64-gcc53-trunk-46d8e9e0cdae456489f1eddfd6d829956fc3c843
HALIDE_DIR := $(abspath $(THIRDPARTY_DIR)/halide)
HALIDE_LIB := $(HALIDE_DIR)/lib/libHalide.a
HALIDE_INC := $(HALIDE_DIR)/include
HALIDE_TOOLS := $(HALIDE_DIR)/tools

CXXFLAGS := -O0 -g -I$(HALIDE_INC) -I$(HALIDE_TOOLS) `libpng-config --cflags` -fPIC
LDFLAGS := $(HALIDE_LIB) `libpng-config --ldflags` -ljpeg -lpthread -ldl -fPIC

CXX := g++

$(HALIDE_LIB): $(THIRDPARTY_DIR)/halide-$(HALIDE_VERSION).tgz
	(rm -rf $(HALIDE_DIR))
	(cd $(THIRDPARTY_DIR); tar xzvf $<)
	touch $(HALIDE_LIB)

THIRDPARTY_LIBS := $(HALIDE_LIB)

$(TARGET): $(OBJECTS) $(THIRDPARTY_LIBS)
	$(CXX) -o $@ $^ -shared -Wl,--exclude-libs,ALL $(LDFLAGS)

%.o: %.cpp $(THIRDPARTY_LIBS)
	$(CXX) -c $(CXXFLAGS) -o $@ $<

clean:
	rm -f $(TARGET) $(OBJECTS)
	rm -rf $(HALIDE_DIR)
