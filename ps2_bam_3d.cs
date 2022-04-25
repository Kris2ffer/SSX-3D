Header
Byte type
UInt24 id
UInt32 numOfNodesInArray //(Len23Array)
UInt32 offsetToNodesOffsets
UInt32 offsetToUnknownNodes2
byte[6] unused
byte[2] unknown1
byte[11] unknown2
byte unknown3
UInt32 offsetToUnknownNode3Array //(under Node2Array)


UnknownNodes2
UInt32 UnknownNode2ArrayCount
UnknownNode2[] UnknownNode2Array

UnknownNode2
byte type //? (same as type in header)
Int24 unknown

Len23ArrayNode
byte[23] unknown1
byte unknown2
byte[4] unknown3 // unused 
///UInt32 rows // filling 16 bytes
Uint32 countOffsetOffsettArray

OffsetOffsetArray
UInt32 offsetToOffsetToUnknownNode3

OffsetToUnknownNode3
UInt32 offsetToUnknownNode3

UnknownNode3
Int16 unknown1 //1, 0
Int16 unknown2 //1, 0
Int16 unknown3 //>=0x30, 0
Int16 unknown4 0

MeshBlock
byte[12] head; // 00 00 00 20 40 40 40 40 00 00 00 00
byte unknown;
byte preSizeHead; // 0x80
byte size; // num verts
byte unknown2
Vert[] vertices
…

