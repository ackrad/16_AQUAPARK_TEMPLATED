using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ObjectSpawnerEditorTool : MonoBehaviour
{
    public Space space;

    public enum Pattern{Line, Square, Circle, Disc  ,Spiral, Cube};
    public Pattern BuilderPattern=Pattern.Line;
    
    public Vector3 patternCenter;
    public float spawnScale=1;
    public GameObject ballPrefab;
    public bool showGizmos = false;
    
    
    //Boolean parameters for show and hide related fields
    //Those will be updated within onvalidate function
    bool isLine = false;
    bool isCircle = false;
    bool isDisc = false;
    bool isSquare = false;
    bool isSpiral = false;
    bool isCube = false;
    
    [Header("Shape Parameters")]
     
    [ShowIf("isLine"),Min(2)] public int numberOfBallsLine = 10;
    [ShowIf("isLine")] public Vector3 lineDirection=Vector3.right;
    [ShowIf("isLine")] public float lineDistance=10f;
    
    [ShowIf("isCircle"),Min(2)] public int numberOfBallsCircle = 10;
    [ShowIf("isCircle")] public float radiusCircle=10;
    [ShowIf("isCircle")] public float arcAngleCircle=360f;
    
    [ShowIf("isDisc"),Min(2)] public int numberOfBallsOfOuterCircle = 10;
    [ShowIf("isDisc")] public float radiusDisc = 10f;
    [ShowIf("isDisc")] public float arcAngleDisc = 360f;
    [ShowIf("isDisc")] public int numberOfCircles;
    
    [ShowIf("isSquare")] public float widthSquare=10f;
    [ShowIf("isSquare")] public float heightSquare=10f;
    [ShowIf("isSquare")] public int numberOfWidthBallSquare=10;
    [ShowIf("isSquare")] public int numberOfHeightBallSquare=10;


    [ShowIf("isSpiral"), Min(2)] public float numberOfBallsSpiral = 10;
    [ShowIf("isSpiral")] public float startRadius=0.5f;
    [ShowIf("isSpiral")] public float growFactor=0.1f;
    [ShowIf("isSpiral")] public float delta=0.2f;
    [ShowIf("isSpiral")] public bool constantDistanceBetweenNodes=false;
    
    [ShowIf("isCube")] public float widthCube=10f;
    [ShowIf("isCube")] public float heightCube=10f;
    [ShowIf("isCube")] public float depthCube=10f;
    [ShowIf("isCube")] public int numberOfWidthBallCube=10;
    [ShowIf("isCube")] public int numberOfHeightBallCube=10;
    [ShowIf("isCube")] public int numberOfDepthBallCube=10;
    
    [Button("Create Pattern")]
    public void createPattern(){
        switch(BuilderPattern){
             case(Pattern.Line):
                createLinePattern();
                 break;
             case(Pattern.Circle):
                createCirclePattern();
                 break;
             case(Pattern.Square):
                createBoxPatter();
                 break;
             case(Pattern.Spiral):
                createSpiralPattern();
                break;
             case(Pattern.Disc):
                 createDiscPattern();
                 break;
             case(Pattern.Cube):
                 createCubePattern();
                 break;
             default:
                 break;
                 
        }
    }

    void createSpiralPattern(){
        
        Transform parent=spawnParent("Spiral").transform;
        List<Vector3> spiralPosition=getSpiralPositions();
        foreach(Vector3 p in spiralPosition){
            spawnBall(p,parent);
        }
    }

    public void createLinePattern(){
        Transform parent=spawnParent("Line").transform;
        float deltaBetweenLines=lineDistance/(numberOfBallsLine-1);

        for(int i=0;i<numberOfBallsLine;i++){
            Vector3 pos=(-lineDistance/2+deltaBetweenLines*i)*lineDirection.normalized;
            spawnBall(pos,parent);
        }
    }
    
    [Button("Remove Last Pattern")]
    public void removeLastPattern(){
        #if UNITY_EDITOR
        if(transform.childCount==0)
            return;
        Transform lastPattern=transform.GetChild(transform.childCount-1);
        if(lastPattern!=null)
            DestroyImmediate(lastPattern.gameObject);
        #endif
    }

    public Transform createCirclePattern(int ballCount, float r, float arc){

        float deltaAngle=arc/(ballCount-1);
        Transform parent=spawnParent("Circle").transform;

        for(int i=0;i<ballCount;i++){
            Vector3 ballDir=Quaternion.AngleAxis(deltaAngle*i,Vector3.forward) * Vector3.up;
            Vector3 ballPos=r*ballDir;
            spawnBall(ballPos,parent);
        }

        return parent;
    }

    public Transform createCirclePattern()
    {
        return createCirclePattern(numberOfBallsCircle,radiusCircle,arcAngleCircle);
        
    }

    public void createDiscPattern()
    {
        float deltaRadius = radiusDisc / numberOfCircles;
        int ballDecreasePerRadius =Mathf.RoundToInt((deltaRadius / radiusDisc) * numberOfBallsOfOuterCircle);
        
        List<Transform> circles=new List<Transform>();
        
        for (int i = 0; i < numberOfCircles; i++)
        {
            Transform t= createCirclePattern(numberOfBallsOfOuterCircle-ballDecreasePerRadius*i,radiusDisc-deltaRadius*i,arcAngleDisc);
            circles.Add(t);
        }

        Transform discParent= spawnParent("Disc").transform;

        foreach (Transform circle in circles)
        {
            circle.SetParent(discParent);
        }

    }
    
    public void createBoxPatter(){
        
        Transform parent=spawnParent("Square").transform;
        float deltaBetweenHorLines=widthSquare/(numberOfWidthBallSquare-1);
        float deltaBetweenVerLines=heightSquare/(numberOfHeightBallSquare-1);

        for(int i=0;i<numberOfWidthBallSquare;i++){
            for(int j=0;j<numberOfHeightBallSquare;j++){
                Vector3 pos=patternCenter+(-widthSquare/2+deltaBetweenHorLines*i)*Vector3.right+(-heightSquare/2+deltaBetweenVerLines*(j))*Vector3.up;
                spawnBall(pos,parent).name="Point_"+i.ToString()+"_"+j.ToString();
            }
        }
    }

    public void createCubePattern()
    {
        Transform parent=spawnParent("Cube").transform;
        float deltaBetweenHorLines=widthCube/(numberOfWidthBallCube-1);
        float deltaBetweenVerLines=heightCube/(numberOfHeightBallCube-1);
        float deltaBetweenDepthLines=depthCube/(numberOfDepthBallCube-1);

        for(int i=0;i<numberOfWidthBallCube;i++){
            for(int j=0;j<numberOfHeightBallCube;j++){
                for (int k = 0; k < numberOfDepthBallCube; k++)
                {
                    Vector3 pos = patternCenter +
                                  (-widthCube / 2 + deltaBetweenHorLines * i) * Vector3.right +
                                  (-heightCube / 2 + deltaBetweenVerLines * (j)) * Vector3.up +
                                  (-depthCube / 2 + deltaBetweenDepthLines * (k)) * Vector3.forward ;
                    spawnBall(pos, parent).name = "Point_" + i.ToString() + "_" + j.ToString();

                }
            }
        }
    }

    List<Vector3> getSpiralPositions(){
        float circleSize=startRadius;
        List<Vector3> spiralPositions=new List<Vector3>();

        float angle=0;

        for(int i=0;i<numberOfBallsSpiral;i++){
            float xPos=0f,yPos=0f;
            if(constantDistanceBetweenNodes){
                float alpha=Mathf.Asin(delta/(2*circleSize))*2;
                xPos = Mathf.Sin(angle + alpha) * circleSize;
                yPos = Mathf.Cos(angle + alpha) * circleSize;
                angle+=alpha;
            }else{
                xPos = Mathf.Sin(i*delta) * circleSize;
                yPos = Mathf.Cos(i*delta) * circleSize;
            }
         
            Vector3 pos=new Vector3(xPos,yPos,0);
            spiralPositions.Add(pos);
            circleSize+=growFactor;
        }
        return spiralPositions;
    }

    GameObject spawnParent(string name){
        Vector3 origin = space == Space.World ? patternCenter : transform.TransformPoint(patternCenter);
        
        GameObject parent=new GameObject();
        parent.transform.SetParent(transform);
        parent.name=name+" Pattern";
        parent.transform.position=origin;
        parent.transform.rotation = transform.rotation;
        return parent;
    }

    GameObject spawnBall(Vector3 localPos,Transform parent){
        #if UNITY_EDITOR
            GameObject spawnedBall=UnityEditor.PrefabUtility.InstantiatePrefab(ballPrefab,parent) as GameObject;
            spawnedBall.transform.localScale = Vector3.one * spawnScale;
            spawnedBall.transform.localPosition=localPos;
            return spawnedBall;
        #endif

        return null;
    }
    
    void OnValidate()
    {
        isLine = BuilderPattern==Pattern.Line ? true : false;
        isCircle = BuilderPattern==Pattern.Circle ? true : false;
        isDisc = BuilderPattern==Pattern.Disc ? true : false;
        isSquare = BuilderPattern==Pattern.Square ? true : false;
        isSpiral = BuilderPattern==Pattern.Spiral ? true : false;
        isCube = BuilderPattern==Pattern.Cube ? true : false;
    }
    
    void OnDrawGizmos()
    {
        #if UNITY_EDITOR
            if(showGizmos==false) return;    
        
            Gizmos.matrix= (space == Space.World)? Matrix4x4.identity : transform.localToWorldMatrix;         
        
            switch(BuilderPattern){
               
                case(Pattern.Line):
                    Vector3 startPos=-lineDistance/2*lineDirection.normalized;
                    float deltaBetweenLines=lineDistance/(numberOfBallsLine-1);
                    for (int i = 0; i < numberOfBallsLine; i++)
                    {
                        Vector3 ballPosition = startPos + (lineDirection.normalized *deltaBetweenLines)*i;
                        Gizmos.DrawSphere(ballPosition,spawnScale);
                    }
                    
                    
                    break;
                case(Pattern.Disc):
                    
                    float deltaRadius = radiusDisc / numberOfCircles;
                    int ballDecreasePerRadius =Mathf.RoundToInt((deltaRadius / radiusDisc) * numberOfBallsOfOuterCircle);
                    
                    for (int i = 0; i < numberOfCircles; i++)
                    {
                        int ballCount = numberOfBallsOfOuterCircle - ballDecreasePerRadius * i;
                        float subRadius = radiusDisc - deltaRadius * i;
                        
                        float delta=arcAngleCircle/(ballCount-1);
                        
                        for(int j=0;j<ballCount;j++){
                            Vector3 ballDir=Quaternion.AngleAxis(delta*j,Vector3.forward) * Vector3.up;
                            Vector3 ballPos=subRadius*ballDir;
                            Gizmos.DrawSphere(ballPos,spawnScale);

                        }
                    }

                    break;
                case(Pattern.Circle):
                    
                    
                    float deltaAngle=arcAngleCircle/(numberOfCircles-1);

                    for(int i=0;i<numberOfCircles;i++){
                        Vector3 ballDir=Quaternion.AngleAxis(deltaAngle*i,Vector3.forward) * Vector3.up;
                        Vector3 ballPos=radiusCircle*ballDir;
                        Gizmos.DrawSphere(ballPos,spawnScale);
                    }
                    
                    break;
                case(Pattern.Square):
                    
                    float deltaBetweenHorLines=widthSquare/(numberOfWidthBallSquare-1);
                    float deltaBetweenVerLines=heightSquare/(numberOfHeightBallSquare-1);

                    for(int i=0;i<numberOfWidthBallSquare;i++){
                        for(int j=0;j<numberOfHeightBallSquare;j++){
                            Vector3 pos=(-widthSquare/2+deltaBetweenHorLines*i)*Vector3.right+(-heightSquare/2+deltaBetweenVerLines*(j))*Vector3.up;
                            Gizmos.DrawSphere(pos,spawnScale);
                        }
                    }
                    
                    break;
                case(Pattern.Cube):
                    float deltaBetweenHorLinesCube=widthCube/(numberOfWidthBallCube-1);
                    float deltaBetweenVerLinesCube=heightCube/(numberOfHeightBallCube-1);
                    float deltaBetweenDepthLinesCube=depthCube/(numberOfDepthBallCube-1);

                    for(int i=0;i<numberOfWidthBallCube;i++){
                        for(int j=0;j<numberOfHeightBallCube;j++){
                            for (int k = 0; k < numberOfDepthBallCube; k++)
                            {
                                Vector3 pos = patternCenter +
                                              (-widthCube / 2 + deltaBetweenHorLinesCube * i) * Vector3.right +
                                              (-heightCube / 2 + deltaBetweenVerLinesCube * (j)) * Vector3.up +
                                              (-depthCube / 2 + deltaBetweenDepthLinesCube * (k)) * Vector3.forward ;
                                Gizmos.DrawSphere(pos,spawnScale);

                            }
                        }
                    }
                    break;
                    
                case(Pattern.Spiral):
                    
                    List<Vector3> spiralPositions=getSpiralPositions();

                    foreach(Vector3 p in spiralPositions){
                        
                        Gizmos.DrawSphere(p,0.3f);
                    }

                    break;
            }

        #endif
    }
}
