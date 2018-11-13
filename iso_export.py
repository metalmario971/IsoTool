#Script to render isometric model with blender and save.
#exec(compile(open("C:/git/dev/utils/IsoPack/bin/Debug/iso_export.py").read(), "C:/git/dev/utils/IsoPack/bin/Debug/iso_export.py", 'exec'))
import bpy
import os
import math
import shutil
import time
import pprint
import bpy_extras.io_utils
from mathutils import Vector, Matrix
from bpy.props import IntProperty, EnumProperty, CollectionProperty, PointerProperty
from bpy.types import PropertyGroup, UIList, Panel, Operator
from bpy import context
import bpy_extras 
import bmesh
import sys, traceback
import argparse

def create_Vertices (name, verts):
    # Create mesh and object
    me = bpy.data.meshes.new(name+'Mesh')
    ob = bpy.data.objects.new(name, me)
    ob.show_name = True
    # Link object to scene
    bpy.context.scene.objects.link(ob)
    me.from_pydata(verts, [], [])
    # Update mesh with new data
    me.update()
    return ob

def look_at(obj_camera, point):
    loc_camera = obj_camera.matrix_world.to_translation()

    direction = point - loc_camera 
    # point the cameras '-Z' and use its 'Y' as up

    # assume we're using euler rotation
    obj_camera.rotation_mode = 'QUATERNION'
    obj_camera.rotation_quaternion = direction.to_track_quat('Z', 'Y')

class IsoExport:
    _fVersion = 0.01
    _strOutpath = "" #This is actually the "temp" path.
    _strModelName = ""
    _fDist = 10.0
    _iAngles = 8
    _iWidth = 128
    _iHeight = 128
    _strAASamples = '0'
    _iKeyframeGrain = 1
    _strObjName = ""
    _strActName = ""
    _bFitModel = False
    def __init__(self, strOutputPath, fDist, iAngles, iWidth, iHeight, iAASamples, iKeyframeGrain, strObjName, strActName, strFitModel):
        self._strOutpath = strOutputPath
        self._strModelName = os.path.splitext(os.path.basename(bpy.data.filepath))[0]
        self._fDist = float(fDist)
        self._iAngles = int(iAngles)
        self._iWidth = int(iWidth)
        self._iHeight = int(iHeight)
        self._iKeyframeGrain = int(iKeyframeGrain)
        
        if self._iWidth == 0 or self._iHeight == 0:
            print("ERROR!: Width or height was zero. " + "width = " + str(self._iWidth) + " height = " + str(self._iHeight))
        
        self._strAASamples = '0'
        if int(iAASamples) > 0:
            if int(iAASamples) >=5:
                self._strAASamples = '5'
            if int(iAASamples) >=8:
                self._strAASamples = '8'
            if int(iAASamples) >=11:
                self._strAASamples = '11'
            if int(iAASamples) >=16:
                self._strAASamples = '16';
        
        if self._iKeyframeGrain <= 0:
            self._iKeyframeGrain = 1
        self._strObjName = strObjName
        self._strActName = strActName
        self._bFitModel = (strFitModel == "True")
        
    def export(self):
        if bpy.context.mode != 'OBJECT':
            bpy.ops.object.mode_set(mode='OBJECT')
        bpy.ops.object.select_all(action='DESELECT') 
    
        #cre3ate ortho cam
        bpy.ops.object.camera_add()
        cam = bpy.context.active_object #The current selected object is the camera
        cam.name = 'iso_camera'
        cam.data.name = 'iso_camera'
        cam.data.type = 'ORTHO'
        cam.location.x = 10 
        cam.location.y = 10
        cam.location.z = 10
        
        # cam.location.normalize()
        # cam.location = cam.location * float(self._fDist)
        
      #  look_at(cam, Vector([0,0,0]))
        bpy.context.scene.camera = cam

        #MUSt set theset to render transparent inmages.
        bpy.context.scene.render.alpha_mode = 'TRANSPARENT'
        bpy.context.scene.render.image_settings.color_mode ='RGBA'
        bpy.context.scene.render.resolution_x = self._iWidth #512 #64
        bpy.context.scene.render.resolution_y = self._iHeight #512 #64
        bpy.context.scene.render.resolution_percentage = 100
        bpy.context.scene.render.use_antialiasing = self._strAASamples != '0'
        if self._strAASamples != '0':
            bpy.context.scene.render.antialiasing_samples = self._strAASamples

        #Compute center of geometry for all objects so we can look at that point
        min = Vector([999,999,999])
        max = Vector([-999,-999,-999])
        for ob in context.visible_objects:
            if ob.type == 'MESH':
                bbox_corners = [ob.matrix_world * Vector(corner) for corner in ob.bound_box]
                for bbc in bbox_corners:
                    if bbc.x > max.x: max.x = bbc.x
                    if bbc.y > max.y: max.y = bbc.y
                    if bbc.z > max.z: max.z = bbc.z
                    if bbc.x < min.x: min.x = bbc.x
                    if bbc.y < min.y: min.y = bbc.y                    
                    if bbc.z < min.z: min.z = bbc.z
        avg = min + (max - min) * 0.5
 
        #Set the camera to look at the center of all objects by adding a constraint, then removing it.  I am not sure why we have neg_z and up_y, but whatever.
        constraint = cam.constraints.new('TRACK_TO')
        constraint.track_axis = 'TRACK_NEGATIVE_Z'
        constraint.up_axis = 'UP_Y'
        dummy = create_Vertices("Dummy", [(0.0,0.0,0.0)])
        dummy.location = avg
        dummy.hide_render = True
        constraint.target = dummy
        bpy.ops.object.visual_transform_apply()
        for c in cam.constraints:
            cam.constraints.remove(c)
        
        cam.data.ortho_scale = self._fDist

        #if self._bFitModel:
            #Zoom the camera to fit the model in the viewport
        
        
        #Basic rotation stuff
        step_count = self._iAngles
        pi = 6.28318530718
        rot_mat = Matrix.Rotation( (pi / step_count), 4, 'Z')   # you can also use as axis Y,Z or a custom vector like (x,y,z)

        #Spin the model
        for step in range(0, step_count):
            #print("angle ", str(step))
            sys.stdout.flush()
            
            #Apply rotation to every model.  Camera stays put.
            for ob in context.visible_objects:
                if ob.type == 'ARMATURE' or ob.type == 'MESH':
                    if not (ob.hide or ob.hide_render):
                        ob.matrix_world = rot_mat * ob.matrix_world; 
                        
            for ob in context.visible_objects:
                if ob.name == self._strObjName:
                    if ob.type == 'ARMATURE' or ob.type == 'MESH':
                        if ob.hide == False:
                            if bpy.context.mode != 'OBJECT':
                                bpy.ops.object.mode_set(mode='OBJECT')
                            bpy.context.scene.objects.active = ob
                            bpy.data.objects[ob.name].select = True 
                            #create the directory where we'll place our model
                            angleOut = os.path.join(os.path.abspath(os.path.normpath(self._strOutpath)), os.path.normpath(str(step)))            
                            modelOut = os.path.join(angleOut, os.path.normpath(ob.name))                        
                         
                            if self._strActName == "" or self._strActName == "__bind":
                                self.exportModelBind(ob, modelOut)
                            else:
                                self.exportAction(ob, modelOut, self._strActName)
                        else:
                            self.emitWarning("Object was hidden, it won't get exported: " + self.getObjectName(ob, None))

        #Cleanup, delete dummy constraint and camera
        if bpy.context.mode != 'OBJECT':
            bpy.ops.object.mode_set(mode='OBJECT')
        bpy.ops.object.select_all(action='DESELECT')
        cam.select = True
        bpy.ops.object.delete() 
        dummy.select = True
        bpy.ops.object.delete() 

    def exportAction(self, ob, modelOut, actName):
        #New export which exports actions

        #AnimData (struct)
        #https://docs.blender.org/api/blender_python_api_2_62_release/bpy.types.AnimData.html
        if ob.animation_data == None:
            return

        print(ob.name + " has animation data " + str(len(ob.animation_data.nla_tracks)) + " NLA Tracks")
        sys.stdout.flush()
        for nla in ob.animation_data.nla_tracks:
            nla.select = True
            print("# NLA STrips: " + str(len(nla.strips)))
            for strip in nla.strips:
                curAction = strip.action
                if curAction.name == actName:
                    print("#Action Found : " + curAction.name)
                    #keyrames
                    keyframes = []

                    iMinKeyf = self.getMinKeyframeForAction(curAction)
                    iMaxKeyf = self.getMaxKeyframeForAction(curAction)
                    
                    #dump keyframes to file.
                    if iMinKeyf < iMaxKeyf == 0:
                        print("Animation had no keyframes.")
                    else:
                        self.renderKeyframeBlockForSelectedObject(ob, iMinKeyf, iMaxKeyf, curAction, modelOut)

    def renderKeyframeBlockForSelectedObject(self, ob, iMinKey, iMaxKey, curAction, modelOut):
        #Create the directory for the action
        actionOut = os.path.join(modelOut, os.path.normpath(curAction.name))
        if not os.path.exists(actionOut):
            os.makedirs(actionOut)
        
        print("exporting " + curAction.name + " to " + str(actionOut))
        
        #if ob.type == 'ARMATURE':
        self.renderKeyframeBlock(ob, iMinKey, iMaxKey, curAction, actionOut)
        #elif ob.type == 'MESH':
        
    def renderKeyframeBlock(self, ob, iMinKey, iMaxKey, curAction, actionOut):
        ob.animation_data.action = curAction
        iGrainKey = 0

        print("Writing keyframe block for ", str(iMinKey), " to ", str(iMaxKey))
        sys.stdout.flush()
        for iKeyFrame in range(iMinKey, iMaxKey+1):
            #this little block gets the final keyframe
            iGrainMax = self._iKeyframeGrain 
            if iKeyFrame == iMaxKey:
                iGrainMax = 1        
            for iGrain in range(0, iGrainMax):
                fGrain = float(iGrain) / float(iGrainMax) 
                bpy.context.scene.frame_set(iKeyFrame, fGrain)

                imgOut = os.path.join(actionOut, self.imgFrame(iGrainKey))
                bpy.context.scene.render.filepath = imgOut
                bpy.ops.render.render( write_still = True )

                iGrainKey+=1
                
    def exportModelBind(self, ob, modelOut):
        #Export the model in bind pose
        
        actionOut = os.path.join(modelOut, os.path.normpath("__bind"))
        if not os.path.exists(actionOut):
            os.makedirs(actionOut)
        print("Writing Model Bind '" + ob.name + "' to '" + actionOut + "'")            
        if bpy.context.mode != 'EDIT':
            bpy.ops.object.mode_set(mode='EDIT')            
            
        imgOut = os.path.join(actionOut, self.imgFrame(0))
        bpy.context.scene.render.filepath = imgOut
        bpy.ops.render.render( write_still = True )
    def imgFrame(self, frameid):
        #Critical that we have EXACTLY 7 digits here, because the C# will parse 7 digits out of the frame 4/6/18
        return "img%07d.png" % frameid
        
    def getMinKeyframeForAction(self, curAction):
        iRet = 9999999
        for fcu in curAction.fcurves:
            for keyf in fcu.keyframe_points:
                x, y = keyf.co
                if x < iRet:
                    iRet = x
        return int(iRet)
    def getMaxKeyframeForAction(self, curAction):        
        iRet = -9999999
        for fcu in curAction.fcurves:
            for keyf in fcu.keyframe_points:
                x, y = keyf.co
                if x > iRet:
                    iRet = x
        return int(iRet)    

            
    def emitWarning(self, strMsg):
        strMsg = "WARNING: " + strMsg
        print(strMsg)
    def emitError(self, strMsg):
        strMsg = "ERROR: " + strMsg
        print(strMsg)
      
class BlenderFileInfo:
    def __init__(self):
        return

    def getFileInfo(self):
        if bpy.context.mode != 'OBJECT':
            bpy.ops.object.mode_set(mode='OBJECT')
        bpy.ops.object.select_all(action='DESELECT')    
        #JSON
        strOut= "$3{\"Objects\":["
        app1 = ""
        for ob in context.visible_objects:
            print("Found " + ob.name + " Type = " + str(ob.type))
            if ob!=None:
                if ob.type == 'MESH' or ob.type == 'ARMATURE':
                            strOut += app1 + "{"
                            strOut += "\"Name\":\"" + ob.name + "\","
                            strOut += "\"Type\":\"" + ob.type + "\","
                            strOut += "\"Checked\":\"False\","
                            strOut += "\"Actions\":["
                            if ob.animation_data != None:
                                if ob.animation_data.nla_tracks != None:
                                    for nla in ob.animation_data.nla_tracks:
                                        nla.select = True
                                        app2 = ""
                                        for strip in nla.strips:
                                            curAction = strip.action
                                            strOut += app2 + "{\"Name\":\"" + curAction.name + "\", \"Checked\":\"False\"}"
                                            app2 = ","
                            
                            strOut += "]"
                            strOut += "}"
                            app1 = ","
        strOut = strOut + "]}$3" 
        
        print(strOut)
        
def getArgs():
    # get the args passed to blender after "--", all of which are ignored by
    # blender so scripts may receive their own arguments
    argv = sys.argv

    if "--" not in argv:
        argv = []  # as if no args are passed
    else:
        argv = argv[argv.index("--") + 1:]  # get all args after "--"

    # When --help or no args are given, print this help
    usage_text = (
            "type -outpath to get output path, -dist for render distance"
            
            )

    parser = argparse.ArgumentParser(description=usage_text)

    # Example utility, add some text and renders or saves it (with options)
    # Possible types are: string, int, long, choice, float and complex.
    parser.add_argument("-outpath", dest="outpath", type=str, required=False, help="Output Path")
    parser.add_argument("-dist", dest="dist", type=str, required=False, help="Camera Render Distance (object size)")
    parser.add_argument("-angles", dest="angles", type=str, required=False, help="Number of Angles To Render (4, or 8 usually)")
    parser.add_argument("-width", dest="width", type=str, required=False, help="render output width")
    parser.add_argument("-height", dest="height", type=str, required=False, help="render output height")
    
    parser.add_argument("-aasamples", dest="aasamples", type=str, required=False, help="antialias samples, 0 = disable")
    parser.add_argument("-keyframegrain", dest="keyframegrain", type=str, required=False, help="keyframe grain")
    parser.add_argument("-objname", dest="objname", type=str, required=False, help="Name of the object to render")
    parser.add_argument("-actname", dest="actname", type=str, required=False, help="Name of the action to render")
    parser.add_argument("-fitmodel", dest="fitmodel", type=str, required=False, help="Fit the model")
    
    parser.add_argument("-fileinfo", dest="fileinfo", type=str, required=False, help="dump information about the file")

    args = parser.parse_args(argv)  # In this example we wont use the args

    if not argv:
        parser.print_help()
        return args

    return args

def printExcept(e):
    print(str(e))
    exc_type, exc_obj, exc_tb = sys.exc_info()
    fname = os.path.split(exc_tb.tb_frame.f_code.co_filename)[1]
    print(fname, "line ", exc_tb.tb_lineno)
    print(traceback.format_exc())

try:

    outpath = os.path.abspath("./");
    dist = 6
    angles = 4
    width = 128
    height = 128
    aasamples = 0
    keyframegrain = 1
    infotype = "Both"
    objname = ""
    actname = ""
    fitmodel = False;
    p = None
    try:
        p = getArgs();
    except Exception as e:
        printExcept(e)
    
    if p == None:
        print("Error parsing args, outpath will default to " + outpath)
    else:
        if p.fileinfo:
            print("Donig info")
        else:
            if not p.outpath:
                print("Warning parsing args, outpath will default to " + outpath)
            else:
                outpath = p.outpath
            if not p.dist:
                print("Warning: dist not set, defaulting to 16")
            else:
                dist = p.dist
            if not p.angles:
                print("Warning: angles not set, defaulting to 8")
            else:
                angles = p.angles
            if not p.width:
                print("Warning: width not set, defaulting to 128")
            else:
                width = p.width
            if not p.height:
                print("Warning: height not set, defaulting to 128")
            else:
                height = p.height            
            if not p.aasamples:
                print("Warning: aa samples not specified")
            else:
                aasamples = p.aasamples            
            if not p.keyframegrain:
                print("Warning: keyframegrain not specified")
            else:
                keyframegrain = p.keyframegrain    
            if not p.objname:
                print("Warning: objname not specified")
            else:
                objname = p.objname    
            #If no actname is specified, then render single.
            if not p.actname:
                print("Warning: actname not specified, defaulting to render the model's bind pose.")
            else:
                actname = p.actname                    
            if not p.fitmodel:
                print("Warning: fitmodel not specified")
            else:
                fitmodel = p.fitmodel     
                
    print("Blender:Starting..")
           
    if p.fileinfo: 
        print("Dumping action info..")
        #Dump action info
        fi = BlenderFileInfo()
        fi.getFileInfo()
    else:
        iso = IsoExport(outpath, dist, angles, width, height, aasamples, keyframegrain, objname, actname, fitmodel)
        iso.export()
    print("Blender:..Done")
except Exception as e:
    #traceback.print_exc()
    print("$2Error$2: Failed to export model: " )
    printExcept(e)

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
#This is useful code - the tracking was broken though    
#Run this simulation the first time computing the camera's maximum distance needed to render all frames from the same distance
# maxDist = Vector([0,0,0]) #loc_camera = obj_camera.matrix_world.to_translation()
# max_ortho_scale = 0
# for step in range(0, step_count):
    # bpy.ops.object.select_all(action='DESELECT') 
    # for ob in context.visible_objects:
        # if ob.type == 'ARMATURE' or ob.type == 'MESH':
            # if not (ob.hide or ob.hide_render):
                # ob.select = True
                # ob.matrix_world = rot_mat * ob.matrix_world; 
                                        
    # bpy.ops.view3d.camera_to_view_selected()         
    # loc_camera = cam.location #cam.matrix_world.to_translation()
    # print(loc_camera)
 # #   print(cam.rotation_euler)
 # #   print(cam.rotation_quaternion)
  # #  print("ortho scale = " + str(bpy.data.cameras[cam.name].ortho_scale))
  # #  print("sensor = " + str(bpy.data.cameras[cam.name].sensor_width)) 
    # if loc_camera.dot(loc_camera) > maxDist.dot(maxDist):
    # #    print("It's greater")
        # maxDist.x = loc_camera.x
        # maxDist.y = loc_camera.y
        # maxDist.z = loc_camera.z
      
    # #if loc_camera.x > maxDist.x:
    # #    maxDist.x = loc_camera.x
    # #if loc_camera.y > maxDist.y:
    # #    maxDist.y = loc_camera.y
    # #if loc_camera.z > maxDist.z:
    # #    maxDist.z = loc_camera.z                    
    # #  
        
    # bpy.context.scene.render.filepath = '\\git\\dev\\cowtower\\dev\\POOP%d.png' % step
    # bpy.ops.render.render( write_still = True )                

# #Put the camera at it's maximum distance
# cam.location.x = maxDist.x
# cam.location.y = maxDist.y
# cam.location.z = maxDist.z



        
        
    #    maxDist = Vector([0,0,0]) #loc_camera = obj_camera.matrix_world.to_translation()
    #    max_ortho_scale = 0
    #    for step in range(0, step_count):
    #        bpy.ops.object.select_all(action='DESELECT') 
    #        for ob in context.visible_objects:
    #            if ob.type == 'ARMATURE' or ob.type == 'MESH':
    #                if not (ob.hide or ob.hide_render):
    #                    ob.select = True
    #                    ob.matrix_world = rot_mat * ob.matrix_world; 
    #                                            
    #        bpy.ops.view3d.camera_to_view_selected()         
    #        loc_camera = cam.location #cam.matrix_world.to_translation()
    #        print(loc_camera)
    #        print("orthos cale = " + str(cam.data.ortho_scale))
    #     #   print(cam.rotation_euler)
    #     #   print(cam.rotation_quaternion)
    #      #  print("ortho scale = " + str(bpy.data.cameras[cam.name].ortho_scale))
    #      #  print("sensor = " + str(bpy.data.cameras[cam.name].sensor_width)) 
    #        if cam.data.ortho_scale > max_ortho_scale: #loc_camera.dot(loc_camera) > maxDist.dot(maxDist):
    #        #    print("It's greater")
    #            maxDist.x = loc_camera.x
    #            maxDist.y = loc_camera.y
    #            maxDist.z = loc_camera.z
    #            max_ortho_scale = cam.data.ortho_scale
    #        #if loc_camera.x > maxDist.x:
    #        #    maxDist.x = loc_camera.x
    #        #if loc_camera.y > maxDist.y:
    #        #    maxDist.y = loc_camera.y
    #        #if loc_camera.z > maxDist.z:
    #        #    maxDist.z = loc_camera.z                    
    #        #  
    #    
    #    
    #    
    #    
    #    
    #            #Normalize location and multiply by our custom distance
    #    #cam.location.normalize()
    #   # cam.location = cam.location * 10
    #   # print("cam location = " + str(cam.location))

    #    
    #    cam.location.x = maxDist.x
    #    cam.location.y = maxDist.y
    #    cam.location.z = maxDist.z
    #    cam.data.ortho_scale = max_ortho_scale
        