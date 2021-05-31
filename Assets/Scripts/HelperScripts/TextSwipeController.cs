using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public enum SwipeDirections{
    Left,Right,Up,Down
}

public class TextSwipeController : MonoBehaviour
{
    public float swipeSpeed;
    public  float offset=10;

    Vector3 originalPosition;
    [HideInInspector] public bool swipeIn=true;
    [HideInInspector] public SwipeDirections swipdeDirection=SwipeDirections.Left;

    RectTransform rectTransform;



    // Start is called before the first frame update
    void Awake()
    {
        rectTransform=GetComponent<RectTransform>();
        originalPosition=rectTransform.anchoredPosition;
    }


    public void swipe(){
        Vector2 initialPosition=Vector2.zero;;
        Vector2 aimPosition=Vector2.zero;

        if(swipeIn){
            aimPosition=originalPosition;
            
            switch(swipdeDirection){
                case(SwipeDirections.Right):
                    initialPosition=originalPosition;
                    initialPosition.x=-(Screen.width/2)-offset;
                    break;
                case(SwipeDirections.Left):
                    initialPosition=originalPosition;
                    initialPosition.x=+(Screen.width/2)+offset;
                    break;
                case(SwipeDirections.Up):
                    initialPosition=originalPosition;
                    initialPosition.y=-(Screen.height/2)-offset;
                    break;
                case(SwipeDirections.Down):
                    initialPosition=originalPosition;
                    initialPosition.y=+(Screen.height/2)+offset;
                    break;
                
            }
        }else{
            
            initialPosition=originalPosition;
            
            switch(swipdeDirection){
                case(SwipeDirections.Right):
                    aimPosition=originalPosition;
                    aimPosition.x=(Screen.width/2)+offset;
                    break;
                case(SwipeDirections.Left):
                    aimPosition=originalPosition;
                    aimPosition.x=-(Screen.width/2)-offset;
                    break;
                case(SwipeDirections.Up):
                    aimPosition=originalPosition;
                    aimPosition.y=+(Screen.height/2)+offset;
                    break;
                case(SwipeDirections.Down):
                    aimPosition=originalPosition;
                    aimPosition.y=-(Screen.height/2)-offset;
                    break;
                
            }
        }

        setAnchorPosition(initialPosition);
        rectTransform.DOAnchorPos(aimPosition,1/swipeSpeed).onComplete=()=>{
            if(swipeIn==false){
                gameObject.SetActive(false);
                resetPosition();
            }

        };
    }

    void setAnchorPosition(Vector2 pos){
        rectTransform.anchoredPosition=pos;
    }

    public void resetPosition(){
        setAnchorPosition(originalPosition);
    }

    


}
