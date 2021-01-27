// NOTE: currently not supporting _NIS models

struct ContainerHeader {
    Int32 unknown;
    Int16 numModels;
    Int16 modelsListOffset;
    UInt32 modelOffsetAddition;
    ModelHeader[] modelsList;
}

struct ModelHeader {
    char[16] modelName;
    UInt32 modelOffset; // + modelOffsetAddition
    //…
}

struct Model {
    BonePercentOffset[] bonePercentOffsets;
    BonePercent[] bonePercentList;
    NumberListRef[] unknownNumberListRefs; //List # of these in the ModelHeader?
    UInt32[] unknownNumberList; 
    UnknownData unknown2;
    InternalModelRefs internalModelRefs;
    InternalModel[] internalModels;
    byte[32] footer; 
    // footer: 
    // 01 00 00 60 00 00 00 00 00 00 00 00 00 00 00 00
    // 00 00 00 00 01 01 00 01 00 00 00 00 00 00 00 11
}

struct BonePercentOffset {
    UInt32 arrayLength;
    UInt32 offsetToBonePercentList;
    Uint32 unused;
}

struct BonePercent { // Unknown usage, probably used on skeleton
    Uint16 percent;
    Uint8 boneID; // (or boneIndex)
    Uint8 unknown3;
}

struct NumberListRef {
    UInt32 count;
    UInt32 offsetToNumberList;
}

struct UnknownData {
    Int32 unknown1 = 1;
    Int32 unknown2; // 3 with 1 internal models, 4 with 2 int models
    Int32 unknown3 = -1;
}

struct InternalModelRefs {
    UInt32 count;
    UInt32 internalModelOffsetTableOffset;
    InternalModelOffsetOffset[] internalModelOffsetTable;
    InternalModelOffset[] internalModelOffsets;
}

struct InternalModel {
    ModelData[] meshes;
    byte[32] footer;
    // footer: 
    // 01 00 00 60 00 00 00 00 00 00 00 00 00 00 00 00 
    // 00 00 00 00 01 01 00 01 00 00 00 00 00 00 00 00
}

struct InternalModelOffsetOffset { 
    UInt32 internalModelOffsetOffset;
    UInt32 unknown = 1;
}

struct InternalModelOffset {
    UInt32 offsetToModelData;
    Int32 unknown = -1;
}

// Actual mesh data
struct ModelData {
    UInt24 rows; // 1 row = 16 bytes
    byte constUnknown1 = 0x10;
    byte[12] unused;
    byte[13] unknown2; // 00 00 00 00 01 01 00 01 00 00 00 00 00
    byte countPrefix = 0x80;
    byte countMeshInfoRows; // meshInfoRows + tri strip rows
    byte countPostfix = 0x6C;
    MeshInfoRow1 meshInfoRow1;
    MeshInfoRow2 meshInfoRow2;
    TriStripCountRow triStripCountRow;
    UInt24 unknownVertCount;
    byte constUnknown2 = 0x10;
    byte[12] unused2;
    UVBlock uvBlock;
    NormalBlock normalBlock;
    VertexBlock vertBlock;
    byte[32] footer; 
    // footer:
    // 01 00 00 10 00 00 00 00 00 00 00 00 00 00 00 00
    // 01 01 00 01 00 00 00 14 00 00 00 00 00 00 00 00
}

struct MeshDataBeginHead {
    byte[16] bytes; // 00 00 00 00 00 00 00 30 00 00 00 00 00 00 00 00
}

struct MeshPreCount {
    byte[12] bytes; // 00 00 00 00 03 01 00 01 00 00 00 00
}

struct UVBlock {
    MeshDataBeginHead beginHead;
    byte[16] uvHead; // 00 10 00 00 00 10 00 00 00 00 00 20 50 50 50 50
    MeshPreCount preCount;
    byte unknown4;
    byte countPrefix = 0x80;
    byte count;
    byte countTypePostfix = 0x6D; // m
    UV[] uv;
    byte[] filler;
}

struct UV {
    UInt16 u; // /4096
    UInt16 v;
    UInt16 uDis;
    UInt16 vDis;
}

struct NormalBlock {
    MeshDataBeginHead beginHead;
    byte[16] normalHead; // 00 00 00 00 00 80 00 00 00 00 00 20 40 40 40 40
    MeshPreCount preCount;
    byte unknown;
    byte countPrefix = 0x80;
    byte count;
    byte countTypePostfix = 0x79; // y
    Normal[] normals;
    byte[] filler;
}

struct Normal {
    Int16 x;
    Int16 z;
    Int16 y;
}

struct VertexBlock {
    MeshDataBeginHead beginHead;
    byte[16] vertexHead; // 00 00 00 00 00 00 80 3F 00 00 00 20 40 40 40 40
    MeshPreCount preCount;
    byte unknown;
    byte countPrefix = 0x80;
    byte count;
    byte countTypePostfix = 0x78; // x
    Vertex[] vertices;
    byte[] filler;
}

struct Vertex {
    Single x;
    Single z;
    Single y;
}
