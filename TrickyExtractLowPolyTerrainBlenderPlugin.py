import bpy
import struct

def read_some_data(context, filepath, use_some_setting):
    print("running read_some_data...")
    f = open(filepath, 'rb')
    
    # create new collection
    #collection = bpy.data.collections.new("MyTestCollection")
    #bpy.context.scene.collection.children.link(collection)

    #read patch count and offset
    f.seek(8, 0) # 1 = relative to current position, 0 = from beginning of file
    patchCount = read_int(f)
    f.seek(68, 0)
    patchOffset = read_int(f)
    
    coll = bpy.data.collections[0]

    # go to patch offset
    f.seek(patchOffset, 0)
    for i in range(patchCount):
    #for i in range(1800):

        (var1,) = struct.unpack('f', f.read(4))
        (var2,) = struct.unpack('f', f.read(4))
        (var3,) = struct.unpack('f', f.read(4))

        # go to rect
        f.seek(348, 1)
        #patch = bpy.context.active_object
        #bpy.ops.object.mode_set(mode='OBJECT')

        verts = []
        faces = []
        name = 'patch'+i.__str__()+' ('+var1.__str__()+', '+var2.__str__()+', '+var3.__str__()+')'

        for pointIndex in range(4):
            (x,) = struct.unpack('f', f.read(4))
            (y,) = struct.unpack('f', f.read(4))
            (z,) = struct.unpack('f', f.read(4))
            w = struct.unpack('f', f.read(4))
            verts.append( [x / 1000, y / 1000, z / 1000] )
            #(verts[0], verts[1], verts[2]) = (verts[0]/1000, verts[1]/1000, verts[2]/1000)
        
        faces.append([0, 1, 2])
        faces.append([2, 1, 3])

        bdata = bpy.data
        mesh = bdata.meshes.new(name)
        obj = bdata.objects.new(name, mesh)
        coll.objects.link(obj)

        mesh.from_pydata(verts, [[0,1],[1,2],[0,2],[1,3],[2,3]], faces)

        
        f.seek(24, 1)


    # would normally load the data here
    f.close() # close file
    return {'FINISHED'}

def read_int(file: str):
    return int.from_bytes(file.read(4), byteorder='little')


# ImportHelper is a helper class, defines filename and
# invoke() function which calls the file selector.
from bpy_extras.io_utils import ImportHelper
from bpy.props import StringProperty, BoolProperty, EnumProperty
from bpy.types import Operator


class ImportSomeData(Operator, ImportHelper):
    """This appears in the tooltip of the operator and in the generated docs"""
    bl_idname = "import_test.some_data"  # important since its how bpy.ops.import_test.some_data is constructed
    bl_label = "Import Some Data"

    # ImportHelper mixin class uses this
    filename_ext = ".txt"

    filter_glob: StringProperty(
        default="*.pbd", #default='*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.bmp',
        options={'HIDDEN'}, #'HIDDEN' to hide other file extensions
        maxlen=255,  # Max internal buffer length, longer would be clamped.
    )

    # List of operator properties, the attributes will be assigned
    # to the class instance from the operator settings before calling.
    use_setting: BoolProperty(
        name="Example Boolean",
        description="Example Tooltip",
        default=True,
    )

    type: EnumProperty(
        name="Example Enum",
        description="Choose between two items",
        items=(
            ('OPT_A', "First Option", "Description one"),
            ('OPT_B', "Second Option", "Description two"),
        ),
        default='OPT_A',
    )

    def execute(self, context):
        return read_some_data(context, self.filepath, self.use_setting)


# Only needed if you want to add into a dynamic menu
def menu_func_import(self, context):
    self.layout.operator(ImportSomeData.bl_idname, text="Text Import Operator")


def register():
    bpy.utils.register_class(ImportSomeData)
    bpy.types.TOPBAR_MT_file_import.append(menu_func_import)


def unregister():
    bpy.utils.unregister_class(ImportSomeData)
    bpy.types.TOPBAR_MT_file_import.remove(menu_func_import)


if __name__ == "__main__":
    register()

    # test call
    bpy.ops.import_test.some_data('INVOKE_DEFAULT')

