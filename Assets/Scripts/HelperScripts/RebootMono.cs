using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//A class under Reboot Int namespace
namespace RebootInt
{
    //Reboot Mono class contains common helper functions that works with MonoBehavior
    //Not that this script must be instantiated in the game.
    public class RebootMono : Singleton<RebootMono>
    {        

        public static GameObject FindGameObjectInChildWithTag (GameObject parent, string tag)
        {
            Transform t = parent.transform;
    
            for (int i = 0; i < t.childCount; i++) 
            {
                Transform child=t.GetChild(i);

                if(child.gameObject.tag == tag)
                {
                    return child.gameObject;
                }

                if(child.childCount>0){
                    var grandchild=FindGameObjectInChildWithTag(child.gameObject,tag);
                    if(grandchild!=null)
                        return grandchild.gameObject;
                }
                    
            }
                
            return null;
        }

            
        public static void changeMaterialToOpaque(SkinnedMeshRenderer rend){
            
            var mat=rend.material;


            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = -1;

        }

        public static GameObject TestPosition(Vector3 position){
            GameObject test=GameObject.CreatePrimitive(PrimitiveType.Cube);
            test.name="TestObject";
            test.transform.position=position;
            return test;
        }
 

    }

}