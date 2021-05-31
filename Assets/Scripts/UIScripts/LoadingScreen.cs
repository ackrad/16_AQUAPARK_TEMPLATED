using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingScreen : Singleton<LoadingScreen>
{

    public float fadeDuration;
    MaskableGraphic[] graphics;

    bool fadedIn=true;

    // Start is called before the first frame update
    void Start()
    {
        graphics=GetComponentsInChildren<MaskableGraphic>();
        setAlpha(1);
    }


    public void fadeIn(){
        if(fadedIn)
            return;

        setAlpha(0);
        DOTween.ToAlpha(()=>graphics[0].color,x=>setAlpha(x.a),1,fadeDuration);

        fadedIn=true;
    }

    public void fadeOut(){
        if(!fadedIn)
            return;

        setAlpha(1);
        DOTween.ToAlpha(()=>graphics[0].color,x=>setAlpha(x.a),0,fadeDuration);

        fadedIn=true;
            
    }

    void setAlpha(float a){
        foreach(MaskableGraphic m in graphics){
            Color col=m.color;
            col.a=a;
            m.color=col;
        }
    }

}
